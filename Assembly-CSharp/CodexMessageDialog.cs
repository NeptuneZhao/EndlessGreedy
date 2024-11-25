using System;
using UnityEngine;

// Token: 0x02000CC1 RID: 3265
public class CodexMessageDialog : MessageDialog
{
	// Token: 0x06006501 RID: 25857 RVA: 0x0025D6A2 File Offset: 0x0025B8A2
	public override bool CanDisplay(Message message)
	{
		return typeof(CodexUnlockedMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06006502 RID: 25858 RVA: 0x0025D6B9 File Offset: 0x0025B8B9
	public override void SetMessage(Message base_message)
	{
		this.message = (CodexUnlockedMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06006503 RID: 25859 RVA: 0x0025D6DD File Offset: 0x0025B8DD
	public override void OnClickAction()
	{
	}

	// Token: 0x06006504 RID: 25860 RVA: 0x0025D6DF File Offset: 0x0025B8DF
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x0400446D RID: 17517
	[SerializeField]
	private LocText description;

	// Token: 0x0400446E RID: 17518
	private CodexUnlockedMessage message;
}
