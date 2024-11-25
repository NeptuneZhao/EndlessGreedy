using System;
using STRINGS;

// Token: 0x02000CC5 RID: 3269
public class DuplicantsLeftMessage : Message
{
	// Token: 0x0600651E RID: 25886 RVA: 0x0025D85F File Offset: 0x0025BA5F
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x0600651F RID: 25887 RVA: 0x0025D866 File Offset: 0x0025BA66
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;
	}

	// Token: 0x06006520 RID: 25888 RVA: 0x0025D872 File Offset: 0x0025BA72
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;
	}

	// Token: 0x06006521 RID: 25889 RVA: 0x0025D87E File Offset: 0x0025BA7E
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
	}
}
