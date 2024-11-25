using System;
using System.Collections;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C40 RID: 3136
public class EditableTitleBar : TitleBar
{
	// Token: 0x14000027 RID: 39
	// (add) Token: 0x0600605B RID: 24667 RVA: 0x0023D5E0 File Offset: 0x0023B7E0
	// (remove) Token: 0x0600605C RID: 24668 RVA: 0x0023D618 File Offset: 0x0023B818
	public event Action<string> OnNameChanged;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x0600605D RID: 24669 RVA: 0x0023D650 File Offset: 0x0023B850
	// (remove) Token: 0x0600605E RID: 24670 RVA: 0x0023D688 File Offset: 0x0023B888
	public event System.Action OnStartedEditing;

	// Token: 0x0600605F RID: 24671 RVA: 0x0023D6C0 File Offset: 0x0023B8C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.onClick += this.GenerateRandomName;
		}
		if (this.editNameButton != null)
		{
			this.EnableEditButtonClick();
		}
		if (this.inputField != null)
		{
			this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}
	}

	// Token: 0x06006060 RID: 24672 RVA: 0x0023D738 File Offset: 0x0023B938
	public void UpdateRenameTooltip(GameObject target)
	{
		if (this.editNameButton != null && target != null)
		{
			if (target.GetComponent<MinionBrain>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAME;
			}
			if (target.GetComponent<ClustercraftExteriorDoor>() != null || target.GetComponent<CommandModule>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAMEROCKET;
				return;
			}
			this.editNameButton.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.EDITNAMEGENERIC, target.GetProperName());
		}
	}

	// Token: 0x06006061 RID: 24673 RVA: 0x0023D7E8 File Offset: 0x0023B9E8
	private void OnEndEdit(string finalStr)
	{
		finalStr = Localization.FilterDirtyWords(finalStr);
		this.SetEditingState(false);
		if (string.IsNullOrEmpty(finalStr))
		{
			return;
		}
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(finalStr);
		}
		this.titleText.text = finalStr;
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		if (base.gameObject.activeInHierarchy && base.enabled)
		{
			this.postEndEdit = base.StartCoroutine(this.PostOnEndEditRoutine());
		}
	}

	// Token: 0x06006062 RID: 24674 RVA: 0x0023D868 File Offset: 0x0023BA68
	private IEnumerator PostOnEndEditRoutine()
	{
		int i = 0;
		while (i < 10)
		{
			int num = i;
			i = num + 1;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.EnableEditButtonClick();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06006063 RID: 24675 RVA: 0x0023D877 File Offset: 0x0023BA77
	private IEnumerator PreToggleNameEditingRoutine()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.ToggleNameEditing();
		this.preToggleNameEditing = null;
		yield break;
	}

	// Token: 0x06006064 RID: 24676 RVA: 0x0023D886 File Offset: 0x0023BA86
	private void EnableEditButtonClick()
	{
		this.editNameButton.onClick += delegate()
		{
			if (this.preToggleNameEditing != null)
			{
				return;
			}
			this.preToggleNameEditing = base.StartCoroutine(this.PreToggleNameEditingRoutine());
		};
	}

	// Token: 0x06006065 RID: 24677 RVA: 0x0023D8A0 File Offset: 0x0023BAA0
	private void GenerateRandomName()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		string text = GameUtil.GenerateRandomDuplicantName();
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(text);
		}
		this.titleText.text = text;
		this.SetEditingState(true);
	}

	// Token: 0x06006066 RID: 24678 RVA: 0x0023D8F0 File Offset: 0x0023BAF0
	private void ToggleNameEditing()
	{
		this.editNameButton.ClearOnClick();
		bool flag = !this.inputField.gameObject.activeInHierarchy;
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(flag);
		}
		this.SetEditingState(flag);
	}

	// Token: 0x06006067 RID: 24679 RVA: 0x0023D944 File Offset: 0x0023BB44
	private void SetEditingState(bool state)
	{
		this.titleText.gameObject.SetActive(!state);
		if (this.setCameraControllerState)
		{
			CameraController.Instance.DisableUserCameraControl = state;
		}
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.gameObject.SetActive(state);
		if (state)
		{
			this.inputField.text = this.titleText.text;
			this.inputField.Select();
			this.inputField.ActivateInputField();
			if (this.OnStartedEditing != null)
			{
				this.OnStartedEditing();
				return;
			}
		}
		else
		{
			this.inputField.DeactivateInputField();
		}
	}

	// Token: 0x06006068 RID: 24680 RVA: 0x0023D9E6 File Offset: 0x0023BBE6
	public void ForceStopEditing()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		this.editNameButton.ClearOnClick();
		this.SetEditingState(false);
		this.EnableEditButtonClick();
	}

	// Token: 0x06006069 RID: 24681 RVA: 0x0023DA14 File Offset: 0x0023BC14
	public void SetUserEditable(bool editable)
	{
		this.userEditable = editable;
		this.editNameButton.gameObject.SetActive(editable);
		this.editNameButton.ClearOnClick();
		this.EnableEditButtonClick();
	}

	// Token: 0x0400411C RID: 16668
	public KButton editNameButton;

	// Token: 0x0400411D RID: 16669
	public KButton randomNameButton;

	// Token: 0x0400411E RID: 16670
	public KInputTextField inputField;

	// Token: 0x04004121 RID: 16673
	private Coroutine postEndEdit;

	// Token: 0x04004122 RID: 16674
	private Coroutine preToggleNameEditing;
}
