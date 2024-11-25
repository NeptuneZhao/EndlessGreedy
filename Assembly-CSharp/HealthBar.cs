using System;
using UnityEngine;

// Token: 0x02000C5C RID: 3164
public class HealthBar : ProgressBar
{
	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x06006132 RID: 24882 RVA: 0x00243B6C File Offset: 0x00241D6C
	private bool ShouldShow
	{
		get
		{
			return this.showTimer > 0f || base.PercentFull < this.alwaysShowThreshold;
		}
	}

	// Token: 0x06006133 RID: 24883 RVA: 0x00243B8B File Offset: 0x00241D8B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
		base.gameObject.SetActive(this.ShouldShow);
	}

	// Token: 0x06006134 RID: 24884 RVA: 0x00243BB9 File Offset: 0x00241DB9
	public void OnChange()
	{
		base.enabled = true;
		this.showTimer = this.maxShowTime;
	}

	// Token: 0x06006135 RID: 24885 RVA: 0x00243BD0 File Offset: 0x00241DD0
	public override void Update()
	{
		base.Update();
		if (Time.timeScale > 0f)
		{
			this.showTimer = Mathf.Max(0f, this.showTimer - Time.unscaledDeltaTime);
		}
		if (!this.ShouldShow)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06006136 RID: 24886 RVA: 0x00243C1F File Offset: 0x00241E1F
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x06006137 RID: 24887 RVA: 0x00243C28 File Offset: 0x00241E28
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x06006138 RID: 24888 RVA: 0x00243C34 File Offset: 0x00241E34
	public override void OnOverlayChanged(object data = null)
	{
		if (!this.autoHide)
		{
			return;
		}
		if ((HashedString)data == OverlayModes.None.ID)
		{
			if (!base.gameObject.activeSelf && this.ShouldShow)
			{
				base.enabled = true;
				base.gameObject.SetActive(true);
				return;
			}
		}
		else if (base.gameObject.activeSelf)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x040041E9 RID: 16873
	private float showTimer;

	// Token: 0x040041EA RID: 16874
	private float maxShowTime = 10f;

	// Token: 0x040041EB RID: 16875
	private float alwaysShowThreshold = 0.8f;
}
