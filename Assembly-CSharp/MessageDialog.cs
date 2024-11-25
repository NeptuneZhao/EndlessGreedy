using System;

// Token: 0x02000CCA RID: 3274
public abstract class MessageDialog : KMonoBehaviour
{
	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x06006546 RID: 25926 RVA: 0x0025DAC4 File Offset: 0x0025BCC4
	public virtual bool CanDontShowAgain
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06006547 RID: 25927
	public abstract bool CanDisplay(Message message);

	// Token: 0x06006548 RID: 25928
	public abstract void SetMessage(Message message);

	// Token: 0x06006549 RID: 25929
	public abstract void OnClickAction();

	// Token: 0x0600654A RID: 25930 RVA: 0x0025DAC7 File Offset: 0x0025BCC7
	public virtual void OnDontShowAgain()
	{
	}
}
