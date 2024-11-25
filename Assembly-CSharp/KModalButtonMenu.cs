using System;
using UnityEngine;

// Token: 0x02000C74 RID: 3188
public class KModalButtonMenu : KButtonMenu
{
	// Token: 0x060061EC RID: 25068 RVA: 0x00249349 File Offset: 0x00247549
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.modalBackground = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x060061ED RID: 25069 RVA: 0x0024935D File Offset: 0x0024755D
	protected override void OnCmpEnable()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x060061EE RID: 25070 RVA: 0x00249390 File Offset: 0x00247590
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.childDialog == null)
		{
			base.Trigger(476357528, null);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x060061EF RID: 25071 RVA: 0x002493E3 File Offset: 0x002475E3
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
	}

	// Token: 0x060061F0 RID: 25072 RVA: 0x002493F0 File Offset: 0x002475F0
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060061F1 RID: 25073 RVA: 0x002493F3 File Offset: 0x002475F3
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x060061F2 RID: 25074 RVA: 0x002493FC File Offset: 0x002475FC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SpeedControlScreen.Instance != null)
		{
			if (show && !this.shown)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			else if (!show && this.shown)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			this.shown = show;
		}
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = show;
		}
	}

	// Token: 0x060061F3 RID: 25075 RVA: 0x0024946B File Offset: 0x0024766B
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x060061F4 RID: 25076 RVA: 0x0024947B File Offset: 0x0024767B
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x060061F5 RID: 25077 RVA: 0x0024948B File Offset: 0x0024768B
	public void SetBackgroundActive(bool active)
	{
	}

	// Token: 0x060061F6 RID: 25078 RVA: 0x00249490 File Offset: 0x00247690
	protected GameObject ActivateChildScreen(GameObject screenPrefab)
	{
		GameObject gameObject = Util.KInstantiateUI(screenPrefab, base.transform.parent.gameObject, false);
		this.childDialog = gameObject;
		gameObject.Subscribe(476357528, new Action<object>(this.Unhide));
		this.Hide();
		return gameObject;
	}

	// Token: 0x060061F7 RID: 25079 RVA: 0x002494DB File Offset: 0x002476DB
	private void Hide()
	{
		this.panelRoot.rectTransform().localScale = Vector3.zero;
	}

	// Token: 0x060061F8 RID: 25080 RVA: 0x002494F2 File Offset: 0x002476F2
	private void Unhide(object data = null)
	{
		this.panelRoot.rectTransform().localScale = Vector3.one;
		this.childDialog.Unsubscribe(476357528, new Action<object>(this.Unhide));
		this.childDialog = null;
	}

	// Token: 0x04004271 RID: 17009
	private bool shown;

	// Token: 0x04004272 RID: 17010
	[SerializeField]
	private GameObject panelRoot;

	// Token: 0x04004273 RID: 17011
	private GameObject childDialog;

	// Token: 0x04004274 RID: 17012
	private RectTransform modalBackground;
}
