using System;
using UnityEngine;

// Token: 0x02000CD5 RID: 3285
public class TargetMessageDialog : MessageDialog
{
	// Token: 0x06006597 RID: 26007 RVA: 0x0025E70F File Offset: 0x0025C90F
	public override bool CanDisplay(Message message)
	{
		return typeof(TargetMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x06006598 RID: 26008 RVA: 0x0025E726 File Offset: 0x0025C926
	public override void SetMessage(Message base_message)
	{
		this.message = (TargetMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x06006599 RID: 26009 RVA: 0x0025E74C File Offset: 0x0025C94C
	public override void OnClickAction()
	{
		MessageTarget target = this.message.GetTarget();
		SelectTool.Instance.SelectAndFocus(target.GetPosition(), target.GetSelectable());
	}

	// Token: 0x0600659A RID: 26010 RVA: 0x0025E77B File Offset: 0x0025C97B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x0400449B RID: 17563
	[SerializeField]
	private LocText description;

	// Token: 0x0400449C RID: 17564
	private TargetMessage message;
}
