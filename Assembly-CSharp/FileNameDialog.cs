using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000B94 RID: 2964
public class FileNameDialog : KModalScreen
{
	// Token: 0x0600596F RID: 22895 RVA: 0x00205B25 File Offset: 0x00203D25
	public override float GetSortKey()
	{
		return 150f;
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x00205B2C File Offset: 0x00203D2C
	public void SetTextAndSelect(string text)
	{
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.text = text;
		this.inputField.Select();
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x00205B54 File Offset: 0x00203D54
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.confirmButton.onClick += this.OnConfirm;
		this.cancelButton.onClick += this.OnCancel;
		this.closeButton.onClick += this.OnCancel;
		this.inputField.onValueChanged.AddListener(delegate(string <p0>)
		{
			Util.ScrubInputField(this.inputField, false, false);
		});
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x00205BE4 File Offset: 0x00203DE4
	protected override void OnActivate()
	{
		base.OnActivate();
		this.inputField.Select();
		this.inputField.ActivateInputField();
		CameraController.Instance.DisableUserCameraControl = true;
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x00205C0D File Offset: 0x00203E0D
	protected override void OnDeactivate()
	{
		CameraController.Instance.DisableUserCameraControl = false;
		base.OnDeactivate();
	}

	// Token: 0x06005974 RID: 22900 RVA: 0x00205C20 File Offset: 0x00203E20
	public void OnConfirm()
	{
		if (this.onConfirm != null && !string.IsNullOrEmpty(this.inputField.text))
		{
			string text = this.inputField.text;
			if (!text.EndsWith(".sav"))
			{
				text += ".sav";
			}
			this.onConfirm(text);
			this.Deactivate();
		}
	}

	// Token: 0x06005975 RID: 22901 RVA: 0x00205C7E File Offset: 0x00203E7E
	private void OnEndEdit(string str)
	{
		if (Localization.HasDirtyWords(str))
		{
			this.inputField.text = "";
		}
	}

	// Token: 0x06005976 RID: 22902 RVA: 0x00205C98 File Offset: 0x00203E98
	public void OnCancel()
	{
		if (this.onCancel != null)
		{
			this.onCancel();
		}
		this.Deactivate();
	}

	// Token: 0x06005977 RID: 22903 RVA: 0x00205CB3 File Offset: 0x00203EB3
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		else if (e.TryConsume(global::Action.DialogSubmit))
		{
			this.OnConfirm();
		}
		e.Consumed = true;
	}

	// Token: 0x06005978 RID: 22904 RVA: 0x00205CE0 File Offset: 0x00203EE0
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x04003AC8 RID: 15048
	public Action<string> onConfirm;

	// Token: 0x04003AC9 RID: 15049
	public System.Action onCancel;

	// Token: 0x04003ACA RID: 15050
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04003ACB RID: 15051
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x04003ACC RID: 15052
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04003ACD RID: 15053
	[SerializeField]
	private KButton closeButton;
}
