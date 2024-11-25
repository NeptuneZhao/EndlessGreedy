using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000CF1 RID: 3313
[AddComponentMenu("KMonoBehaviour/scripts/NextUpdateTimer")]
public class NextUpdateTimer : KMonoBehaviour
{
	// Token: 0x060066C0 RID: 26304 RVA: 0x0026630A File Offset: 0x0026450A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.initialAnimScale = this.UpdateAnimController.animScale;
	}

	// Token: 0x060066C1 RID: 26305 RVA: 0x00266323 File Offset: 0x00264523
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060066C2 RID: 26306 RVA: 0x0026632B File Offset: 0x0026452B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshReleaseTimes();
	}

	// Token: 0x060066C3 RID: 26307 RVA: 0x0026633C File Offset: 0x0026453C
	public void UpdateReleaseTimes(string lastUpdateTime, string nextUpdateTime, string textOverride)
	{
		if (!System.DateTime.TryParse(lastUpdateTime, out this.currentReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse last_update_time: " + lastUpdateTime);
		}
		if (!System.DateTime.TryParse(nextUpdateTime, out this.nextReleaseDate))
		{
			global::Debug.LogWarning("Failed to parse next_update_time: " + nextUpdateTime);
		}
		this.m_releaseTextOverride = textOverride;
		this.RefreshReleaseTimes();
	}

	// Token: 0x060066C4 RID: 26308 RVA: 0x00266394 File Offset: 0x00264594
	private void RefreshReleaseTimes()
	{
		TimeSpan timeSpan = this.nextReleaseDate - this.currentReleaseDate;
		TimeSpan timeSpan2 = this.nextReleaseDate - System.DateTime.UtcNow;
		TimeSpan timeSpan3 = System.DateTime.UtcNow - this.currentReleaseDate;
		string s = "4";
		string text;
		if (!string.IsNullOrEmpty(this.m_releaseTextOverride))
		{
			text = this.m_releaseTextOverride;
		}
		else if (timeSpan2.TotalHours < 8.0)
		{
			text = UI.DEVELOPMENTBUILDS.UPDATES.TWENTY_FOUR_HOURS;
			s = "4";
		}
		else if (timeSpan2.TotalDays < 1.0)
		{
			text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, 1);
			s = "3";
		}
		else
		{
			int num = timeSpan2.Days % 7;
			int num2 = (timeSpan2.Days - num) / 7;
			if (num2 <= 0)
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, num);
				s = "2";
			}
			else
			{
				text = string.Format(UI.DEVELOPMENTBUILDS.UPDATES.BIGGER_TIMES, num, num2);
				s = "1";
			}
		}
		this.TimerText.text = text;
		this.UpdateAnimController.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		float positionPercent = Mathf.Clamp01((float)(timeSpan3.TotalSeconds / timeSpan.TotalSeconds));
		this.UpdateAnimMeterController.SetPositionPercent(positionPercent);
	}

	// Token: 0x04004549 RID: 17737
	public LocText TimerText;

	// Token: 0x0400454A RID: 17738
	public KBatchedAnimController UpdateAnimController;

	// Token: 0x0400454B RID: 17739
	public KBatchedAnimController UpdateAnimMeterController;

	// Token: 0x0400454C RID: 17740
	public float initialAnimScale;

	// Token: 0x0400454D RID: 17741
	public System.DateTime nextReleaseDate;

	// Token: 0x0400454E RID: 17742
	public System.DateTime currentReleaseDate;

	// Token: 0x0400454F RID: 17743
	private string m_releaseTextOverride;
}
