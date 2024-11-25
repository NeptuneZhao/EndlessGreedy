using System;
using KSerialization;

// Token: 0x02000CC6 RID: 3270
public class EODReportMessage : Message
{
	// Token: 0x06006523 RID: 25891 RVA: 0x0025D892 File Offset: 0x0025BA92
	public EODReportMessage(string title, string tooltip)
	{
		this.day = GameUtil.GetCurrentCycle();
		this.title = title;
		this.tooltip = tooltip;
	}

	// Token: 0x06006524 RID: 25892 RVA: 0x0025D8B3 File Offset: 0x0025BAB3
	public EODReportMessage()
	{
	}

	// Token: 0x06006525 RID: 25893 RVA: 0x0025D8BB File Offset: 0x0025BABB
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x06006526 RID: 25894 RVA: 0x0025D8BE File Offset: 0x0025BABE
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x06006527 RID: 25895 RVA: 0x0025D8C5 File Offset: 0x0025BAC5
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x06006528 RID: 25896 RVA: 0x0025D8CD File Offset: 0x0025BACD
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x06006529 RID: 25897 RVA: 0x0025D8D5 File Offset: 0x0025BAD5
	public void OpenReport()
	{
		ManagementMenu.Instance.OpenReports(this.day);
	}

	// Token: 0x0600652A RID: 25898 RVA: 0x0025D8E7 File Offset: 0x0025BAE7
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x0600652B RID: 25899 RVA: 0x0025D8EA File Offset: 0x0025BAEA
	public override void OnClick()
	{
		this.OpenReport();
	}

	// Token: 0x04004474 RID: 17524
	[Serialize]
	private int day;

	// Token: 0x04004475 RID: 17525
	[Serialize]
	private string title;

	// Token: 0x04004476 RID: 17526
	[Serialize]
	private string tooltip;
}
