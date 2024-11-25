using System;
using STRINGS;

// Token: 0x02000CC0 RID: 3264
public class AchievementEarnedMessage : Message
{
	// Token: 0x060064F9 RID: 25849 RVA: 0x0025D648 File Offset: 0x0025B848
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x060064FA RID: 25850 RVA: 0x0025D64B File Offset: 0x0025B84B
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x060064FB RID: 25851 RVA: 0x0025D652 File Offset: 0x0025B852
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x060064FC RID: 25852 RVA: 0x0025D659 File Offset: 0x0025B859
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.NAME;
	}

	// Token: 0x060064FD RID: 25853 RVA: 0x0025D665 File Offset: 0x0025B865
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.TOOLTIP;
	}

	// Token: 0x060064FE RID: 25854 RVA: 0x0025D671 File Offset: 0x0025B871
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x060064FF RID: 25855 RVA: 0x0025D674 File Offset: 0x0025B874
	public override void OnClick()
	{
		RetireColonyUtility.SaveColonySummaryData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
	}
}
