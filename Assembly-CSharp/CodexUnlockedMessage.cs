using System;
using STRINGS;

// Token: 0x02000CC2 RID: 3266
public class CodexUnlockedMessage : Message
{
	// Token: 0x06006506 RID: 25862 RVA: 0x0025D6FA File Offset: 0x0025B8FA
	public CodexUnlockedMessage()
	{
	}

	// Token: 0x06006507 RID: 25863 RVA: 0x0025D702 File Offset: 0x0025B902
	public CodexUnlockedMessage(string lock_id, string unlock_message)
	{
		this.lockId = lock_id;
		this.unlockMessage = unlock_message;
	}

	// Token: 0x06006508 RID: 25864 RVA: 0x0025D718 File Offset: 0x0025B918
	public string GetLockId()
	{
		return this.lockId;
	}

	// Token: 0x06006509 RID: 25865 RVA: 0x0025D720 File Offset: 0x0025B920
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600650A RID: 25866 RVA: 0x0025D727 File Offset: 0x0025B927
	public override string GetMessageBody()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);
	}

	// Token: 0x0600650B RID: 25867 RVA: 0x0025D73E File Offset: 0x0025B93E
	public override string GetTitle()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;
	}

	// Token: 0x0600650C RID: 25868 RVA: 0x0025D74A File Offset: 0x0025B94A
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x0600650D RID: 25869 RVA: 0x0025D752 File Offset: 0x0025B952
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x0400446F RID: 17519
	private string unlockMessage;

	// Token: 0x04004470 RID: 17520
	private string lockId;
}
