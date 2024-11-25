using System;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000BEC RID: 3052
public class BuildingGroupScreen : KScreen
{
	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06005CF8 RID: 23800 RVA: 0x00222F7E File Offset: 0x0022117E
	public static bool SearchIsEmpty
	{
		get
		{
			return BuildingGroupScreen.Instance == null || BuildingGroupScreen.Instance.inputField.text.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06005CF9 RID: 23801 RVA: 0x00222FA3 File Offset: 0x002211A3
	public static bool IsEditing
	{
		get
		{
			return !(BuildingGroupScreen.Instance == null) && BuildingGroupScreen.Instance.isEditing;
		}
	}

	// Token: 0x06005CFA RID: 23802 RVA: 0x00222FBE File Offset: 0x002211BE
	protected override void OnPrefabInit()
	{
		BuildingGroupScreen.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06005CFB RID: 23803 RVA: 0x00222FD4 File Offset: 0x002211D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.ConfigurePlanScreenForSearch();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.inputField.onValueChanged.AddListener(delegate(string value)
		{
			PlanScreen.Instance.RefreshCategoryPanelTitle();
		});
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.BUILDMENU.SEARCH_TEXT_PLACEHOLDER;
		this.clearButton.onClick += this.ClearSearch;
	}

	// Token: 0x06005CFC RID: 23804 RVA: 0x0022308F File Offset: 0x0022128F
	protected override void OnActivate()
	{
		base.OnActivate();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06005CFD RID: 23805 RVA: 0x0022309E File Offset: 0x0022129E
	public void ClearSearch()
	{
		this.inputField.text = "";
	}

	// Token: 0x06005CFE RID: 23806 RVA: 0x002230B0 File Offset: 0x002212B0
	private void ConfigurePlanScreenForSearch()
	{
		PlanScreen.Instance.SoftCloseRecipe();
		PlanScreen.Instance.ClearSelection();
		PlanScreen.Instance.ForceRefreshAllBuildingToggles();
		PlanScreen.Instance.ConfigurePanelSize(null);
	}

	// Token: 0x04003E42 RID: 15938
	public static BuildingGroupScreen Instance;

	// Token: 0x04003E43 RID: 15939
	public KInputTextField inputField;

	// Token: 0x04003E44 RID: 15940
	[SerializeField]
	public KButton clearButton;
}
