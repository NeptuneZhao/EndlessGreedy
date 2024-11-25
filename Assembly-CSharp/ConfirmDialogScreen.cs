using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1C RID: 3100
public class ConfirmDialogScreen : KModalScreen
{
	// Token: 0x06005F15 RID: 24341 RVA: 0x00235475 File Offset: 0x00233675
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06005F16 RID: 24342 RVA: 0x00235489 File Offset: 0x00233689
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06005F17 RID: 24343 RVA: 0x0023548C File Offset: 0x0023368C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_CANCEL();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005F18 RID: 24344 RVA: 0x002354A8 File Offset: 0x002336A8
	public void PopupConfirmDialog(string text, System.Action on_confirm, System.Action on_cancel, string configurable_text = null, System.Action on_configurable_clicked = null, string title_text = null, string confirm_text = null, string cancel_text = null, Sprite image_sprite = null)
	{
		while (base.transform.parent.GetComponent<Canvas>() == null && base.transform.parent.parent != null)
		{
			base.transform.SetParent(base.transform.parent.parent);
		}
		base.transform.SetAsLastSibling();
		this.confirmAction = on_confirm;
		this.cancelAction = on_cancel;
		this.configurableAction = on_configurable_clicked;
		int num = 0;
		if (this.confirmAction != null)
		{
			num++;
		}
		if (this.cancelAction != null)
		{
			num++;
		}
		if (this.configurableAction != null)
		{
			num++;
		}
		this.confirmButton.GetComponentInChildren<LocText>().text = ((confirm_text == null) ? UI.CONFIRMDIALOG.OK.text : confirm_text);
		this.cancelButton.GetComponentInChildren<LocText>().text = ((cancel_text == null) ? UI.CONFIRMDIALOG.CANCEL.text : cancel_text);
		this.confirmButton.GetComponent<KButton>().onClick += this.OnSelect_OK;
		this.cancelButton.GetComponent<KButton>().onClick += this.OnSelect_CANCEL;
		this.configurableButton.GetComponent<KButton>().onClick += this.OnSelect_third;
		this.cancelButton.SetActive(on_cancel != null);
		if (this.configurableButton != null)
		{
			this.configurableButton.SetActive(this.configurableAction != null);
			if (configurable_text != null)
			{
				this.configurableButton.GetComponentInChildren<LocText>().text = configurable_text;
			}
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.key = "";
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x06005F19 RID: 24345 RVA: 0x0023567D File Offset: 0x0023387D
	public void OnSelect_OK()
	{
		if (this.deactivateOnConfirmAction)
		{
			this.Deactivate();
		}
		if (this.confirmAction != null)
		{
			this.confirmAction();
		}
	}

	// Token: 0x06005F1A RID: 24346 RVA: 0x002356A0 File Offset: 0x002338A0
	public void OnSelect_CANCEL()
	{
		if (this.deactivateOnCancelAction)
		{
			this.Deactivate();
		}
		if (this.cancelAction != null)
		{
			this.cancelAction();
		}
	}

	// Token: 0x06005F1B RID: 24347 RVA: 0x002356C3 File Offset: 0x002338C3
	public void OnSelect_third()
	{
		if (this.deactivateOnConfigurableAction)
		{
			this.Deactivate();
		}
		if (this.configurableAction != null)
		{
			this.configurableAction();
		}
	}

	// Token: 0x06005F1C RID: 24348 RVA: 0x002356E6 File Offset: 0x002338E6
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04003FE8 RID: 16360
	private System.Action confirmAction;

	// Token: 0x04003FE9 RID: 16361
	private System.Action cancelAction;

	// Token: 0x04003FEA RID: 16362
	private System.Action configurableAction;

	// Token: 0x04003FEB RID: 16363
	public bool deactivateOnConfigurableAction = true;

	// Token: 0x04003FEC RID: 16364
	public bool deactivateOnConfirmAction = true;

	// Token: 0x04003FED RID: 16365
	public bool deactivateOnCancelAction = true;

	// Token: 0x04003FEE RID: 16366
	public System.Action onDeactivateCB;

	// Token: 0x04003FEF RID: 16367
	[SerializeField]
	private GameObject confirmButton;

	// Token: 0x04003FF0 RID: 16368
	[SerializeField]
	private GameObject cancelButton;

	// Token: 0x04003FF1 RID: 16369
	[SerializeField]
	private GameObject configurableButton;

	// Token: 0x04003FF2 RID: 16370
	[SerializeField]
	private LocText titleText;

	// Token: 0x04003FF3 RID: 16371
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04003FF4 RID: 16372
	[SerializeField]
	private Image image;
}
