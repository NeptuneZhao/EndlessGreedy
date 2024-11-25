using System;
using KSerialization;
using STRINGS;

// Token: 0x02000CD2 RID: 3282
public class SkillMasteredMessage : Message
{
	// Token: 0x06006588 RID: 25992 RVA: 0x0025E600 File Offset: 0x0025C800
	public SkillMasteredMessage()
	{
	}

	// Token: 0x06006589 RID: 25993 RVA: 0x0025E608 File Offset: 0x0025C808
	public SkillMasteredMessage(MinionResume resume)
	{
		this.minionName = resume.GetProperName();
	}

	// Token: 0x0600658A RID: 25994 RVA: 0x0025E61C File Offset: 0x0025C81C
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600658B RID: 25995 RVA: 0x0025E624 File Offset: 0x0025C824
	public override string GetMessageBody()
	{
		Debug.Assert(this.minionName != null);
		string arg = string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, this.minionName);
		return string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, arg);
	}

	// Token: 0x0600658C RID: 25996 RVA: 0x0025E665 File Offset: 0x0025C865
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x0600658D RID: 25997 RVA: 0x0025E67C File Offset: 0x0025C87C
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x0600658E RID: 25998 RVA: 0x0025E693 File Offset: 0x0025C893
	public override bool IsValid()
	{
		return this.minionName != null;
	}

	// Token: 0x04004497 RID: 17559
	[Serialize]
	private string minionName;
}
