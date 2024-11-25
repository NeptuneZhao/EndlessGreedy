using System;
using UnityEngine;

// Token: 0x02000CCB RID: 3275
public class MessageDialogFrame : KScreen
{
	// Token: 0x0600654C RID: 25932 RVA: 0x0025DAD1 File Offset: 0x0025BCD1
	public override float GetSortKey()
	{
		return 15f;
	}

	// Token: 0x0600654D RID: 25933 RVA: 0x0025DAD8 File Offset: 0x0025BCD8
	protected override void OnActivate()
	{
		this.closeButton.onClick += this.OnClickClose;
		this.nextMessageButton.onClick += this.OnClickNextMessage;
		MultiToggle multiToggle = this.dontShowAgainButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickDontShowAgain));
		bool flag = KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) == 1;
		this.dontShowAgainButton.ChangeState(flag ? 0 : 1);
		base.Subscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
		this.OnMessagesChanged(null);
	}

	// Token: 0x0600654E RID: 25934 RVA: 0x0025DB84 File Offset: 0x0025BD84
	protected override void OnDeactivate()
	{
		base.Unsubscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
	}

	// Token: 0x0600654F RID: 25935 RVA: 0x0025DBA7 File Offset: 0x0025BDA7
	private void OnClickClose()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006550 RID: 25936 RVA: 0x0025DBBA File Offset: 0x0025BDBA
	private void OnClickNextMessage()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
		NotificationScreen.Instance.OnClickNextMessage();
	}

	// Token: 0x06006551 RID: 25937 RVA: 0x0025DBD8 File Offset: 0x0025BDD8
	private void OnClickDontShowAgain()
	{
		this.dontShowAgainButton.NextState();
		bool flag = this.dontShowAgainButton.CurrentState == 0;
		KPlayerPrefs.SetInt("HideTutorial_CheckState", flag ? 1 : 0);
	}

	// Token: 0x06006552 RID: 25938 RVA: 0x0025DC10 File Offset: 0x0025BE10
	private void OnMessagesChanged(object data)
	{
		this.nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
	}

	// Token: 0x06006553 RID: 25939 RVA: 0x0025DC30 File Offset: 0x0025BE30
	public void SetMessage(MessageDialog dialog, Message message)
	{
		this.title.text = message.GetTitle().ToUpper();
		dialog.GetComponent<RectTransform>().SetParent(this.body.GetComponent<RectTransform>());
		RectTransform component = dialog.GetComponent<RectTransform>();
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		dialog.transform.SetLocalPosition(Vector3.zero);
		dialog.SetMessage(message);
		dialog.OnClickAction();
		if (dialog.CanDontShowAgain)
		{
			this.dontShowAgainElement.SetActive(true);
			this.dontShowAgainDelegate = new System.Action(dialog.OnDontShowAgain);
			return;
		}
		this.dontShowAgainElement.SetActive(false);
		this.dontShowAgainDelegate = null;
	}

	// Token: 0x06006554 RID: 25940 RVA: 0x0025DCDD File Offset: 0x0025BEDD
	private void TryDontShowAgain()
	{
		if (this.dontShowAgainDelegate != null && this.dontShowAgainButton.CurrentState == 0)
		{
			this.dontShowAgainDelegate();
		}
	}

	// Token: 0x0400447C RID: 17532
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400447D RID: 17533
	[SerializeField]
	private KToggle nextMessageButton;

	// Token: 0x0400447E RID: 17534
	[SerializeField]
	private GameObject dontShowAgainElement;

	// Token: 0x0400447F RID: 17535
	[SerializeField]
	private MultiToggle dontShowAgainButton;

	// Token: 0x04004480 RID: 17536
	[SerializeField]
	private LocText title;

	// Token: 0x04004481 RID: 17537
	[SerializeField]
	private RectTransform body;

	// Token: 0x04004482 RID: 17538
	private System.Action dontShowAgainDelegate;
}
