using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D0C RID: 3340
public class PlanBuildingToggle : KToggle
{
	// Token: 0x06006802 RID: 26626 RVA: 0x0026E038 File Offset: 0x0026C238
	public void Config(BuildingDef def, PlanScreen planScreen, HashedString buildingCategory)
	{
		this.def = def;
		this.planScreen = planScreen;
		this.buildingCategory = buildingCategory;
		this.techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(1557339983, new Action<object>(this.CheckResearch)));
		this.sprite = def.GetUISprite("ui", false);
		base.onClick += delegate()
		{
			PlanScreen.Instance.OnSelectBuilding(this.gameObject, def, null);
			this.RefreshDisplay();
		};
		if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(def.PrefabID))
		{
			Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[def.PrefabID].ToUpper() + ".NAME", out this.subcategoryName);
		}
		else
		{
			global::Debug.LogWarning("Building " + def.PrefabID + " has not been added to plan screen subcategory organization in BuildingTuning.cs");
		}
		this.CheckResearch(null);
		this.Refresh();
	}

	// Token: 0x06006803 RID: 26627 RVA: 0x0026E1AC File Offset: 0x0026C3AC
	protected override void OnDestroy()
	{
		if (Game.Instance != null)
		{
			foreach (int id in this.gameSubscriptions)
			{
				Game.Instance.Unsubscribe(id);
			}
		}
		this.gameSubscriptions.Clear();
		base.OnDestroy();
	}

	// Token: 0x06006804 RID: 26628 RVA: 0x0026E224 File Offset: 0x0026C424
	private void CheckResearch(object data = null)
	{
		this.researchComplete = PlanScreen.TechRequirementsMet(this.techItem);
	}

	// Token: 0x06006805 RID: 26629 RVA: 0x0026E238 File Offset: 0x0026C438
	public bool CheckBuildingPassesSearchFilter(Def building)
	{
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			return this.StandardDisplayFilter();
		}
		string text = BuildingGroupScreen.Instance.inputField.text;
		string text2 = UI.StripLinkFormatting(building.Name).ToLower();
		text = text.ToUpper();
		return text2.ToUpper().Contains(text) || (this.subcategoryName != null && this.subcategoryName.String.ToUpper().Contains(text));
	}

	// Token: 0x06006806 RID: 26630 RVA: 0x0026E2AC File Offset: 0x0026C4AC
	private bool StandardDisplayFilter()
	{
		return (this.researchComplete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive) && (this.planScreen.ActiveCategoryToggleInfo == null || this.buildingCategory == (HashedString)this.planScreen.ActiveCategoryToggleInfo.userData);
	}

	// Token: 0x06006807 RID: 26631 RVA: 0x0026E308 File Offset: 0x0026C508
	public bool Refresh()
	{
		bool flag;
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			flag = this.StandardDisplayFilter();
		}
		else
		{
			flag = this.CheckBuildingPassesSearchFilter(this.def);
		}
		bool result = false;
		if (base.gameObject.activeSelf != flag)
		{
			base.gameObject.SetActive(flag);
			result = true;
		}
		if (!base.gameObject.activeSelf)
		{
			return result;
		}
		this.PositionTooltip();
		this.RefreshLabel();
		this.RefreshDisplay();
		return result;
	}

	// Token: 0x06006808 RID: 26632 RVA: 0x0026E374 File Offset: 0x0026C574
	public void SwitchViewMode(bool listView)
	{
		this.text.gameObject.SetActive(!listView);
		this.text_listView.gameObject.SetActive(listView);
		this.buildingIcon.gameObject.SetActive(!listView);
		this.buildingIcon_listView.gameObject.SetActive(listView);
	}

	// Token: 0x06006809 RID: 26633 RVA: 0x0026E3CC File Offset: 0x0026C5CC
	private void RefreshLabel()
	{
		if (this.text != null)
		{
			this.text.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text_listView.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text.text = this.def.Name;
			this.text_listView.text = this.def.Name;
		}
	}

	// Token: 0x0600680A RID: 26634 RVA: 0x0026E454 File Offset: 0x0026C654
	private void RefreshDisplay()
	{
		PlanScreen.RequirementsState buildableState = PlanScreen.Instance.GetBuildableState(this.def);
		bool flag = buildableState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		bool flag2 = base.gameObject == PlanScreen.Instance.SelectedBuildingGameObject;
		if (flag2 && flag)
		{
			this.toggle.ChangeState(1);
		}
		else if (!flag2 && flag)
		{
			this.toggle.ChangeState(0);
		}
		else if (flag2 && !flag)
		{
			this.toggle.ChangeState(3);
		}
		else if (!flag2 && !flag)
		{
			this.toggle.ChangeState(2);
		}
		this.RefreshBuildingButtonIconAndColors(flag);
		this.RefreshFG(buildableState);
	}

	// Token: 0x0600680B RID: 26635 RVA: 0x0026E504 File Offset: 0x0026C704
	private void PositionTooltip()
	{
		this.tooltip.overrideParentObject = (PlanScreen.Instance.ProductInfoScreen.gameObject.activeSelf ? PlanScreen.Instance.ProductInfoScreen.rectTransform() : PlanScreen.Instance.buildingGroupsRoot);
		this.tooltip.tooltipPivot = Vector2.zero;
		this.tooltip.parentPositionAnchor = new Vector2(1f, 0f);
		this.tooltip.tooltipPositionOffset = new Vector2(4f, 0f);
		this.tooltip.ClearMultiStringTooltip();
		string name = this.def.Name;
		string effect = this.def.Effect;
		this.tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
		this.tooltip.AddMultiStringTooltip(effect, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x0600680C RID: 26636 RVA: 0x0026E5EC File Offset: 0x0026C7EC
	private void RefreshBuildingButtonIconAndColors(bool buttonAvailable)
	{
		if (this.sprite == null)
		{
			this.sprite = PlanScreen.Instance.defaultBuildingIconSprite;
		}
		this.buildingIcon.sprite = this.sprite;
		this.buildingIcon.SetNativeSize();
		this.buildingIcon_listView.sprite = this.sprite;
		float d = ScreenResolutionMonitor.UsingGamepadUIMode() ? 3.25f : 4f;
		this.buildingIcon.rectTransform().sizeDelta /= d;
		Material material = buttonAvailable ? PlanScreen.Instance.defaultUIMaterial : PlanScreen.Instance.desaturatedUIMaterial;
		if (this.buildingIcon.material != material)
		{
			this.buildingIcon.material = material;
			this.buildingIcon_listView.material = material;
		}
	}

	// Token: 0x0600680D RID: 26637 RVA: 0x0026E6BC File Offset: 0x0026C8BC
	private void RefreshFG(PlanScreen.RequirementsState requirementsState)
	{
		if (requirementsState == PlanScreen.RequirementsState.Tech)
		{
			this.fgImage.sprite = PlanScreen.Instance.Overlay_NeedTech;
			this.fgImage.gameObject.SetActive(true);
		}
		else
		{
			this.fgImage.gameObject.SetActive(false);
		}
		string tooltipForRequirementsState = PlanScreen.GetTooltipForRequirementsState(this.def, requirementsState);
		if (tooltipForRequirementsState != null)
		{
			this.tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
			this.tooltip.AddMultiStringTooltip(tooltipForRequirementsState, PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x04004633 RID: 17971
	private BuildingDef def;

	// Token: 0x04004634 RID: 17972
	private HashedString buildingCategory;

	// Token: 0x04004635 RID: 17973
	private TechItem techItem;

	// Token: 0x04004636 RID: 17974
	private List<int> gameSubscriptions = new List<int>();

	// Token: 0x04004637 RID: 17975
	private bool researchComplete;

	// Token: 0x04004638 RID: 17976
	private Sprite sprite;

	// Token: 0x04004639 RID: 17977
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x0400463A RID: 17978
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x0400463B RID: 17979
	[SerializeField]
	private LocText text;

	// Token: 0x0400463C RID: 17980
	[SerializeField]
	private LocText text_listView;

	// Token: 0x0400463D RID: 17981
	[SerializeField]
	private Image buildingIcon;

	// Token: 0x0400463E RID: 17982
	[SerializeField]
	private Image buildingIcon_listView;

	// Token: 0x0400463F RID: 17983
	[SerializeField]
	private Image fgIcon;

	// Token: 0x04004640 RID: 17984
	[SerializeField]
	private PlanScreen planScreen;

	// Token: 0x04004641 RID: 17985
	private StringEntry subcategoryName;
}
