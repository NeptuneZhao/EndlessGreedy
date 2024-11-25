using System;
using UnityEngine;

// Token: 0x02000DEA RID: 3562
public class VictoryScreen : KModalScreen
{
	// Token: 0x06007119 RID: 28953 RVA: 0x002ACD13 File Offset: 0x002AAF13
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x0600711A RID: 28954 RVA: 0x002ACD21 File Offset: 0x002AAF21
	private void Init()
	{
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x0600711B RID: 28955 RVA: 0x002ACD47 File Offset: 0x002AAF47
	private void Retire()
	{
		if (RetireColonyUtility.SaveColonySummaryData())
		{
			this.Show(false);
		}
	}

	// Token: 0x0600711C RID: 28956 RVA: 0x002ACD57 File Offset: 0x002AAF57
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x0600711D RID: 28957 RVA: 0x002ACD60 File Offset: 0x002AAF60
	public void SetAchievements(string[] achievementIDs)
	{
		string text = "";
		for (int i = 0; i < achievementIDs.Length; i++)
		{
			if (i > 0)
			{
				text += "\n";
			}
			text += GameUtil.ApplyBoldString(Db.Get().ColonyAchievements.Get(achievementIDs[i]).Name);
			text = text + "\n" + Db.Get().ColonyAchievements.Get(achievementIDs[i]).description;
		}
		this.descriptionText.text = text;
	}

	// Token: 0x04004DC6 RID: 19910
	[SerializeField]
	private KButton DismissButton;

	// Token: 0x04004DC7 RID: 19911
	[SerializeField]
	private LocText descriptionText;
}
