using System;
using UnityEngine;

// Token: 0x02000CD3 RID: 3283
public class StandardMessageDialog : MessageDialog
{
	// Token: 0x0600658F RID: 25999 RVA: 0x0025E69E File Offset: 0x0025C89E
	public override bool CanDisplay(Message message)
	{
		return typeof(Message).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06006590 RID: 26000 RVA: 0x0025E6B5 File Offset: 0x0025C8B5
	public override void SetMessage(Message base_message)
	{
		this.message = base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06006591 RID: 26001 RVA: 0x0025E6D4 File Offset: 0x0025C8D4
	public override void OnClickAction()
	{
	}

	// Token: 0x04004498 RID: 17560
	[SerializeField]
	private LocText description;

	// Token: 0x04004499 RID: 17561
	private Message message;
}
