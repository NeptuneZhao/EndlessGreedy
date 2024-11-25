using System;

// Token: 0x02000CEE RID: 3310
public abstract class NewGameFlowScreen : KModalScreen
{
	// Token: 0x1400002B RID: 43
	// (add) Token: 0x060066A8 RID: 26280 RVA: 0x00265E68 File Offset: 0x00264068
	// (remove) Token: 0x060066A9 RID: 26281 RVA: 0x00265EA0 File Offset: 0x002640A0
	public event System.Action OnNavigateForward;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060066AA RID: 26282 RVA: 0x00265ED8 File Offset: 0x002640D8
	// (remove) Token: 0x060066AB RID: 26283 RVA: 0x00265F10 File Offset: 0x00264110
	public event System.Action OnNavigateBackward;

	// Token: 0x060066AC RID: 26284 RVA: 0x00265F45 File Offset: 0x00264145
	protected void NavigateBackward()
	{
		this.OnNavigateBackward();
	}

	// Token: 0x060066AD RID: 26285 RVA: 0x00265F52 File Offset: 0x00264152
	protected void NavigateForward()
	{
		this.OnNavigateForward();
	}

	// Token: 0x060066AE RID: 26286 RVA: 0x00265F5F File Offset: 0x0026415F
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.NavigateBackward();
		}
		base.OnKeyDown(e);
	}
}
