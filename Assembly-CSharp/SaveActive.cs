using System;

// Token: 0x02000D2D RID: 3373
public class SaveActive : KScreen
{
	// Token: 0x06006A09 RID: 27145 RVA: 0x0027F6CA File Offset: 0x0027D8CA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.SetAutoSaveCallbacks(new Game.SavingPreCB(this.ActiveateSaveIndicator), new Game.SavingActiveCB(this.SetActiveSaveIndicator), new Game.SavingPostCB(this.DeactivateSaveIndicator));
	}

	// Token: 0x06006A0A RID: 27146 RVA: 0x0027F700 File Offset: 0x0027D900
	private void DoCallBack(HashedString name)
	{
		this.controller.onAnimComplete -= this.DoCallBack;
		this.readyForSaveCallback();
		this.readyForSaveCallback = null;
	}

	// Token: 0x06006A0B RID: 27147 RVA: 0x0027F72B File Offset: 0x0027D92B
	private void ActiveateSaveIndicator(Game.CansaveCB cb)
	{
		this.readyForSaveCallback = cb;
		this.controller.onAnimComplete += this.DoCallBack;
		this.controller.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06006A0C RID: 27148 RVA: 0x0027F76B File Offset: 0x0027D96B
	private void SetActiveSaveIndicator()
	{
		this.controller.Play("working_loop", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06006A0D RID: 27149 RVA: 0x0027F78D File Offset: 0x0027D98D
	private void DeactivateSaveIndicator()
	{
		this.controller.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06006A0E RID: 27150 RVA: 0x0027F7AF File Offset: 0x0027D9AF
	public override void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x04004841 RID: 18497
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04004842 RID: 18498
	private Game.CansaveCB readyForSaveCallback;
}
