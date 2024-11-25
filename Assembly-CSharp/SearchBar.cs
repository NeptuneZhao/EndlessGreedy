using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000D9E RID: 3486
public class SearchBar : KMonoBehaviour
{
	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06006DF2 RID: 28146 RVA: 0x00295372 File Offset: 0x00293572
	public string CurrentSearchValue
	{
		get
		{
			if (!string.IsNullOrEmpty(this.inputField.text))
			{
				return this.inputField.text;
			}
			return "";
		}
	}

	// Token: 0x170007B8 RID: 1976
	// (get) Token: 0x06006DF3 RID: 28147 RVA: 0x00295397 File Offset: 0x00293597
	public bool IsInputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x170007B9 RID: 1977
	// (get) Token: 0x06006DF5 RID: 28149 RVA: 0x002953B7 File Offset: 0x002935B7
	// (set) Token: 0x06006DF4 RID: 28148 RVA: 0x002953AE File Offset: 0x002935AE
	public bool isEditing { get; protected set; }

	// Token: 0x06006DF6 RID: 28150 RVA: 0x002953BF File Offset: 0x002935BF
	public virtual void SetPlaceholder(string text)
	{
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = text;
	}

	// Token: 0x06006DF7 RID: 28151 RVA: 0x002953D8 File Offset: 0x002935D8
	protected override void OnSpawn()
	{
		this.inputField.ActivateInputField();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.OnFocus));
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.clearButton.onClick += this.ClearSearch;
		this.SetPlaceholder(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER);
	}

	// Token: 0x06006DF8 RID: 28152 RVA: 0x0029547A File Offset: 0x0029367A
	protected void SetEditingState(bool editing)
	{
		this.isEditing = editing;
		Action<bool> editingStateChanged = this.EditingStateChanged;
		if (editingStateChanged != null)
		{
			editingStateChanged(this.isEditing);
		}
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06006DF9 RID: 28153 RVA: 0x002954A4 File Offset: 0x002936A4
	protected virtual void OnValueChanged(string value)
	{
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x06006DFA RID: 28154 RVA: 0x002954B7 File Offset: 0x002936B7
	protected virtual void OnEndEdit(string value)
	{
		this.SetEditingState(false);
	}

	// Token: 0x06006DFB RID: 28155 RVA: 0x002954C0 File Offset: 0x002936C0
	protected virtual void OnFocus()
	{
		this.SetEditingState(true);
		UISounds.PlaySound(UISounds.Sound.ClickHUD);
		System.Action focused = this.Focused;
		if (focused == null)
		{
			return;
		}
		focused();
	}

	// Token: 0x06006DFC RID: 28156 RVA: 0x002954DF File Offset: 0x002936DF
	public virtual void ClearSearch()
	{
		this.SetValue("");
	}

	// Token: 0x06006DFD RID: 28157 RVA: 0x002954EC File Offset: 0x002936EC
	public void SetValue(string value)
	{
		this.inputField.text = value;
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x04004B06 RID: 19206
	[SerializeField]
	protected KInputTextField inputField;

	// Token: 0x04004B07 RID: 19207
	[SerializeField]
	protected KButton clearButton;

	// Token: 0x04004B09 RID: 19209
	public Action<string> ValueChanged;

	// Token: 0x04004B0A RID: 19210
	public Action<bool> EditingStateChanged;

	// Token: 0x04004B0B RID: 19211
	public System.Action Focused;
}
