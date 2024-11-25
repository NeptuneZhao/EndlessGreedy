using System;
using KSerialization;

// Token: 0x02000CC9 RID: 3273
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
	// Token: 0x0600653A RID: 25914
	public abstract string GetTitle();

	// Token: 0x0600653B RID: 25915
	public abstract string GetSound();

	// Token: 0x0600653C RID: 25916
	public abstract string GetMessageBody();

	// Token: 0x0600653D RID: 25917
	public abstract string GetTooltip();

	// Token: 0x0600653E RID: 25918 RVA: 0x0025DAA9 File Offset: 0x0025BCA9
	public virtual bool ShowDialog()
	{
		return true;
	}

	// Token: 0x0600653F RID: 25919 RVA: 0x0025DAAC File Offset: 0x0025BCAC
	public virtual void OnCleanUp()
	{
	}

	// Token: 0x06006540 RID: 25920 RVA: 0x0025DAAE File Offset: 0x0025BCAE
	public virtual bool IsValid()
	{
		return true;
	}

	// Token: 0x06006541 RID: 25921 RVA: 0x0025DAB1 File Offset: 0x0025BCB1
	public virtual bool PlayNotificationSound()
	{
		return true;
	}

	// Token: 0x06006542 RID: 25922 RVA: 0x0025DAB4 File Offset: 0x0025BCB4
	public virtual void OnClick()
	{
	}

	// Token: 0x06006543 RID: 25923 RVA: 0x0025DAB6 File Offset: 0x0025BCB6
	public virtual NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x06006544 RID: 25924 RVA: 0x0025DAB9 File Offset: 0x0025BCB9
	public virtual bool ShowDismissButton()
	{
		return true;
	}
}
