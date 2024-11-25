using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DBB RID: 3515
public class WarpPortalSideScreen : SideScreenContent
{
	// Token: 0x06006F5D RID: 28509 RVA: 0x0029C9EC File Offset: 0x0029ABEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.BUTTON);
		this.cancelButtonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CANCELBUTTON);
		this.button.onClick += this.OnButtonClick;
		this.cancelButton.onClick += this.OnCancelClick;
		this.Refresh(null);
	}

	// Token: 0x06006F5E RID: 28510 RVA: 0x0029CA5E File Offset: 0x0029AC5E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<WarpPortal>() != null;
	}

	// Token: 0x06006F5F RID: 28511 RVA: 0x0029CA6C File Offset: 0x0029AC6C
	public override void SetTarget(GameObject target)
	{
		WarpPortal component = target.GetComponent<WarpPortal>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a WarpPortal associated with it.");
			return;
		}
		this.target = component;
		target.GetComponent<Assignable>().OnAssign += new Action<IAssignableIdentity>(this.Refresh);
		this.Refresh(null);
	}

	// Token: 0x06006F60 RID: 28512 RVA: 0x0029CABC File Offset: 0x0029ACBC
	private void Update()
	{
		if (this.progressBar.activeSelf)
		{
			RectTransform rectTransform = this.progressBar.GetComponentsInChildren<Image>()[1].rectTransform;
			float num = this.target.rechargeProgress / 3000f;
			rectTransform.sizeDelta = new Vector2(rectTransform.transform.parent.GetComponent<LayoutElement>().minWidth * num, 24f);
			this.progressLabel.text = GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
		}
	}

	// Token: 0x06006F61 RID: 28513 RVA: 0x0029CB38 File Offset: 0x0029AD38
	private void OnButtonClick()
	{
		if (this.target.ReadyToWarp)
		{
			this.target.StartWarpSequence();
			this.Refresh(null);
		}
	}

	// Token: 0x06006F62 RID: 28514 RVA: 0x0029CB59 File Offset: 0x0029AD59
	private void OnCancelClick()
	{
		this.target.CancelAssignment();
		this.Refresh(null);
	}

	// Token: 0x06006F63 RID: 28515 RVA: 0x0029CB70 File Offset: 0x0029AD70
	private void Refresh(object data = null)
	{
		this.progressBar.SetActive(false);
		this.cancelButton.gameObject.SetActive(false);
		if (!(this.target != null))
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
			this.button.gameObject.SetActive(false);
			return;
		}
		if (this.target.ReadyToWarp)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.WAITING;
			this.button.gameObject.SetActive(true);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.button.gameObject.SetActive(false);
			this.progressBar.SetActive(true);
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CONSUMED;
			return;
		}
		if (this.target.IsWorking)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.UNDERWAY;
			this.button.gameObject.SetActive(false);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
		this.button.gameObject.SetActive(false);
	}

	// Token: 0x04004BF3 RID: 19443
	[SerializeField]
	private LocText label;

	// Token: 0x04004BF4 RID: 19444
	[SerializeField]
	private KButton button;

	// Token: 0x04004BF5 RID: 19445
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x04004BF6 RID: 19446
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04004BF7 RID: 19447
	[SerializeField]
	private LocText cancelButtonLabel;

	// Token: 0x04004BF8 RID: 19448
	[SerializeField]
	private WarpPortal target;

	// Token: 0x04004BF9 RID: 19449
	[SerializeField]
	private GameObject contents;

	// Token: 0x04004BFA RID: 19450
	[SerializeField]
	private GameObject progressBar;

	// Token: 0x04004BFB RID: 19451
	[SerializeField]
	private LocText progressLabel;
}
