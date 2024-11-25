using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000D16 RID: 3350
public class ProductInfoScreen : KScreen
{
	// Token: 0x1700076F RID: 1903
	// (get) Token: 0x06006886 RID: 26758 RVA: 0x00272160 File Offset: 0x00270360
	public FacadeSelectionPanel FacadeSelectionPanel
	{
		get
		{
			return this.facadeSelectionPanel;
		}
	}

	// Token: 0x06006887 RID: 26759 RVA: 0x00272168 File Offset: 0x00270368
	private void RefreshScreen()
	{
		if (this.currentDef != null)
		{
			this.SetTitle(this.currentDef);
			return;
		}
		this.ClearProduct(true);
	}

	// Token: 0x06006888 RID: 26760 RVA: 0x0027218C File Offset: 0x0027038C
	public void ClearProduct(bool deactivateTool = true)
	{
		if (this.materialSelectionPanel == null)
		{
			return;
		}
		this.currentDef = null;
		this.materialSelectionPanel.ClearMaterialToggles();
		if (PlayerController.Instance.ActiveTool == BuildTool.Instance && deactivateTool)
		{
			BuildTool.Instance.Deactivate();
		}
		if (PlayerController.Instance.ActiveTool == UtilityBuildTool.Instance || PlayerController.Instance.ActiveTool == WireBuildTool.Instance)
		{
			ToolMenu.Instance.ClearSelection();
		}
		this.ClearLabels();
		this.Show(false);
	}

	// Token: 0x06006889 RID: 26761 RVA: 0x00272220 File Offset: 0x00270420
	public new void Awake()
	{
		base.Awake();
		this.facadeSelectionPanel = Util.KInstantiateUI<FacadeSelectionPanel>(this.facadeSelectionPanelPrefab.gameObject, base.gameObject, false);
		FacadeSelectionPanel facadeSelectionPanel = this.facadeSelectionPanel;
		facadeSelectionPanel.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel.OnFacadeSelectionChanged, new System.Action(this.OnFacadeSelectionChanged));
		this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x0600688A RID: 26762 RVA: 0x00272294 File Offset: 0x00270494
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (BuildingGroupScreen.Instance != null)
		{
			BuildingGroupScreen instance = BuildingGroupScreen.Instance;
			instance.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildingGroupScreen instance2 = BuildingGroupScreen.Instance;
			instance2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance2.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (PlanScreen.Instance != null)
		{
			PlanScreen instance3 = PlanScreen.Instance;
			instance3.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance3.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			PlanScreen instance4 = PlanScreen.Instance;
			instance4.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance4.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu instance5 = BuildMenu.Instance;
			instance5.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(instance5.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
			BuildMenu instance6 = BuildMenu.Instance;
			instance6.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(instance6.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		}
		this.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(this.pointerEnterActions, new KScreen.PointerEnterActions(this.CheckMouseOver));
		this.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(this.pointerExitActions, new KScreen.PointerExitActions(this.CheckMouseOver));
		base.ConsumeMouseScroll = true;
		this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		MultiToggle multiToggle = this.sandboxInstantBuildToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SandboxToolParameterMenu.instance.settings.InstantBuild = !SandboxToolParameterMenu.instance.settings.InstantBuild;
			this.sandboxInstantBuildToggle.ChangeState(SandboxToolParameterMenu.instance.settings.InstantBuild ? 1 : 0);
		}));
		this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		Game.Instance.Subscribe(-1948169901, delegate(object data)
		{
			this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
		});
	}

	// Token: 0x0600688B RID: 26763 RVA: 0x0027247A File Offset: 0x0027067A
	public void ConfigureScreen(BuildingDef def)
	{
		this.ConfigureScreen(def, this.FacadeSelectionPanel.SelectedFacade);
	}

	// Token: 0x0600688C RID: 26764 RVA: 0x00272490 File Offset: 0x00270690
	public void ConfigureScreen(BuildingDef def, string facadeID)
	{
		this.configuring = true;
		this.currentDef = def;
		this.SetTitle(def);
		this.SetDescription(def);
		this.SetEffects(def);
		this.facadeSelectionPanel.SetBuildingDef(def.PrefabID, null);
		BuildingFacadeResource buildingFacadeResource = null;
		if ("DEFAULT_FACADE" != facadeID)
		{
			buildingFacadeResource = Db.GetBuildingFacades().TryGet(facadeID);
		}
		if (buildingFacadeResource != null && buildingFacadeResource.PrefabID == def.PrefabID && buildingFacadeResource.IsUnlocked())
		{
			this.facadeSelectionPanel.SelectedFacade = facadeID;
		}
		else
		{
			this.facadeSelectionPanel.SelectedFacade = "DEFAULT_FACADE";
		}
		this.SetMaterials(def);
		this.configuring = false;
	}

	// Token: 0x0600688D RID: 26765 RVA: 0x00272537 File Offset: 0x00270737
	private void ExpandInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(true);
	}

	// Token: 0x0600688E RID: 26766 RVA: 0x00272540 File Offset: 0x00270740
	private void CollapseInfo(PointerEventData data)
	{
		this.ToggleExpandedInfo(false);
	}

	// Token: 0x0600688F RID: 26767 RVA: 0x0027254C File Offset: 0x0027074C
	public void ToggleExpandedInfo(bool state)
	{
		this.expandedInfo = state;
		if (this.ProductDescriptionPane != null)
		{
			this.ProductDescriptionPane.SetActive(this.expandedInfo);
		}
		if (this.ProductRequirementsPane != null)
		{
			this.ProductRequirementsPane.gameObject.SetActive(this.expandedInfo && this.ProductRequirementsPane.HasDescriptors());
		}
		if (this.RoomConstrainsPanel != null)
		{
			this.RoomConstrainsPanel.gameObject.SetActive(this.expandedInfo && this.RoomConstrainsPanel.HasDescriptors());
		}
		if (this.ProductEffectsPane != null)
		{
			this.ProductEffectsPane.gameObject.SetActive(this.expandedInfo && this.ProductEffectsPane.HasDescriptors());
		}
		if (this.ProductFlavourPane != null)
		{
			this.ProductFlavourPane.SetActive(this.expandedInfo);
		}
		if (this.materialSelectionPanel != null && this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			this.materialSelectionPanel.ToggleShowDescriptorPanels(this.expandedInfo);
		}
	}

	// Token: 0x06006890 RID: 26768 RVA: 0x00272674 File Offset: 0x00270874
	private void CheckMouseOver(PointerEventData data)
	{
		bool state = base.GetMouseOver || (PlanScreen.Instance != null && ((PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.GetMouseOver) || BuildingGroupScreen.Instance.GetMouseOver)) || (BuildMenu.Instance != null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.GetMouseOver);
		this.ToggleExpandedInfo(state);
	}

	// Token: 0x06006891 RID: 26769 RVA: 0x002726EC File Offset: 0x002708EC
	private void Update()
	{
		if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && this.currentDef != null && this.materialSelectionPanel.CurrentSelectedElement != null && !MaterialSelector.AllowInsufficientMaterialBuild() && this.currentDef.Mass[0] > ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.materialSelectionPanel.CurrentSelectedElement, true))
		{
			this.materialSelectionPanel.AutoSelectAvailableMaterial();
		}
	}

	// Token: 0x06006892 RID: 26770 RVA: 0x00272774 File Offset: 0x00270974
	private void SetTitle(BuildingDef def)
	{
		this.titleBar.SetTitle(def.Name);
		bool flag = (PlanScreen.Instance != null && PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.IsDefBuildable(def)) || (BuildMenu.Instance != null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete);
		this.titleBar.GetComponentInChildren<KImage>().ColorState = (flag ? KImage.ColorSelector.Active : KImage.ColorSelector.Disabled);
	}

	// Token: 0x06006893 RID: 26771 RVA: 0x00272800 File Offset: 0x00270A00
	private void SetDescription(BuildingDef def)
	{
		if (def == null)
		{
			return;
		}
		if (this.productFlavourText == null)
		{
			return;
		}
		string text = "";
		text += def.Desc;
		Dictionary<Klei.AI.Attribute, float> dictionary = new Dictionary<Klei.AI.Attribute, float>();
		Dictionary<Klei.AI.Attribute, float> dictionary2 = new Dictionary<Klei.AI.Attribute, float>();
		foreach (Klei.AI.Attribute key in def.attributes)
		{
			if (!dictionary.ContainsKey(key))
			{
				dictionary[key] = 0f;
			}
		}
		foreach (AttributeModifier attributeModifier in def.attributeModifiers)
		{
			float num = 0f;
			Klei.AI.Attribute key2 = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
			dictionary.TryGetValue(key2, out num);
			num += attributeModifier.Value;
			dictionary[key2] = num;
		}
		if (this.materialSelectionPanel.CurrentSelectedElement != null)
		{
			Element element = ElementLoader.GetElement(this.materialSelectionPanel.CurrentSelectedElement);
			if (element != null)
			{
				using (List<AttributeModifier>.Enumerator enumerator2 = element.attributeModifiers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AttributeModifier attributeModifier2 = enumerator2.Current;
						float num2 = 0f;
						Klei.AI.Attribute key3 = Db.Get().BuildingAttributes.Get(attributeModifier2.AttributeId);
						dictionary2.TryGetValue(key3, out num2);
						num2 += attributeModifier2.Value;
						dictionary2[key3] = num2;
					}
					goto IL_229;
				}
			}
			PrefabAttributeModifiers component = Assets.TryGetPrefab(this.materialSelectionPanel.CurrentSelectedElement).GetComponent<PrefabAttributeModifiers>();
			if (component != null)
			{
				foreach (AttributeModifier attributeModifier3 in component.descriptors)
				{
					float num3 = 0f;
					Klei.AI.Attribute key4 = Db.Get().BuildingAttributes.Get(attributeModifier3.AttributeId);
					dictionary2.TryGetValue(key4, out num3);
					num3 += attributeModifier3.Value;
					dictionary2[key4] = num3;
				}
			}
		}
		IL_229:
		if (dictionary.Count > 0)
		{
			text += "\n\n";
			foreach (KeyValuePair<Klei.AI.Attribute, float> keyValuePair in dictionary)
			{
				float num4 = 0f;
				dictionary.TryGetValue(keyValuePair.Key, out num4);
				float num5 = 0f;
				string text2 = "";
				if (dictionary2.TryGetValue(keyValuePair.Key, out num5))
				{
					num5 = Mathf.Abs(num4 * num5);
					text2 = "(+" + num5.ToString() + ")";
				}
				text = string.Concat(new string[]
				{
					text,
					"\n",
					keyValuePair.Key.Name,
					": ",
					(num4 + num5).ToString(),
					text2
				});
			}
		}
		this.productFlavourText.text = text;
	}

	// Token: 0x06006894 RID: 26772 RVA: 0x00272B6C File Offset: 0x00270D6C
	private void SetEffects(BuildingDef def)
	{
		if (this.productDescriptionText.text != null)
		{
			this.productDescriptionText.text = string.Format("{0}", def.Effect);
		}
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def.BuildingComplete, false);
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
		List<Descriptor> list = new List<Descriptor>();
		if (requirementDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONREQUIREMENTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONREQUIREMENTS, Descriptor.DescriptorType.Effect);
			requirementDescriptors.Insert(0, item);
			this.ProductRequirementsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductRequirementsPane.gameObject.SetActive(false);
		}
		this.ProductRequirementsPane.SetDescriptors(requirementDescriptors);
		List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
		if (effectDescriptors.Count > 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.OPERATIONEFFECTS, UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONEFFECTS, Descriptor.DescriptorType.Effect);
			effectDescriptors.Insert(0, item2);
			this.ProductEffectsPane.gameObject.SetActive(true);
		}
		else
		{
			this.ProductEffectsPane.gameObject.SetActive(false);
		}
		this.ProductEffectsPane.SetDescriptors(effectDescriptors);
		foreach (Tag tag in def.BuildingComplete.GetComponent<KPrefabID>().Tags)
		{
			if (RoomConstraints.ConstraintTags.AllTags.Contains(tag) && !this.HiddenRoomConstrainTags.Contains(tag))
			{
				Descriptor item3 = default(Descriptor);
				item3.SetupDescriptor(RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(tag), null, Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		if (list.Count > 0)
		{
			list = GameUtil.GetEffectDescriptors(list);
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(CODEX.HEADERS.BUILDINGTYPE, UI.BUILDINGEFFECTS.TOOLTIPS.BUILDINGROOMREQUIREMENTCLASS, Descriptor.DescriptorType.Effect);
			list.Insert(0, item4);
			this.RoomConstrainsPanel.gameObject.SetActive(true);
		}
		else
		{
			this.RoomConstrainsPanel.gameObject.SetActive(false);
		}
		this.RoomConstrainsPanel.SetDescriptors(list);
	}

	// Token: 0x06006895 RID: 26773 RVA: 0x00272D80 File Offset: 0x00270F80
	public void ClearLabels()
	{
		List<string> list = new List<string>(this.descLabels.Keys);
		if (list.Count > 0)
		{
			foreach (string key in list)
			{
				GameObject gameObject = this.descLabels[key];
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				this.descLabels.Remove(key);
			}
		}
	}

	// Token: 0x06006896 RID: 26774 RVA: 0x00272E0C File Offset: 0x0027100C
	public void SetMaterials(BuildingDef def)
	{
		this.materialSelectionPanel.gameObject.SetActive(true);
		Recipe craftRecipe = def.CraftRecipe;
		this.materialSelectionPanel.ClearSelectActions();
		this.materialSelectionPanel.ConfigureScreen(craftRecipe, new MaterialSelectionPanel.GetBuildableStateDelegate(PlanScreen.Instance.IsDefBuildable), new MaterialSelectionPanel.GetBuildableTooltipDelegate(PlanScreen.Instance.GetTooltipForBuildable));
		this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshScreen));
		this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.onMenuMaterialChanged));
		this.materialSelectionPanel.AutoSelectAvailableMaterial();
		this.ActivateAppropriateTool(def);
	}

	// Token: 0x06006897 RID: 26775 RVA: 0x00272EB5 File Offset: 0x002710B5
	private void OnFacadeSelectionChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
	}

	// Token: 0x06006898 RID: 26776 RVA: 0x00272ED2 File Offset: 0x002710D2
	private void onMenuMaterialChanged()
	{
		if (this.currentDef == null)
		{
			return;
		}
		this.ActivateAppropriateTool(this.currentDef);
		this.SetDescription(this.currentDef);
	}

	// Token: 0x06006899 RID: 26777 RVA: 0x00272EFC File Offset: 0x002710FC
	private void ActivateAppropriateTool(BuildingDef def)
	{
		global::Debug.Assert(def != null, "def was null");
		if (((PlanScreen.Instance != null) ? PlanScreen.Instance.IsDefBuildable(def) : (BuildMenu.Instance != null && BuildMenu.Instance.BuildableState(def) == PlanScreen.RequirementsState.Complete)) && this.materialSelectionPanel.AllSelectorsSelected() && this.facadeSelectionPanel.SelectedFacade != null)
		{
			this.onElementsFullySelected.Signal();
			return;
		}
		if (!MaterialSelector.AllowInsufficientMaterialBuild() && !DebugHandler.InstantBuildMode)
		{
			if (PlayerController.Instance.ActiveTool == BuildTool.Instance)
			{
				BuildTool.Instance.Deactivate();
			}
			PrebuildTool.Instance.Activate(def, PlanScreen.Instance.GetTooltipForBuildable(def));
		}
	}

	// Token: 0x0600689A RID: 26778 RVA: 0x00272FC0 File Offset: 0x002711C0
	public static bool MaterialsMet(Recipe recipe)
	{
		if (recipe == null)
		{
			global::Debug.LogError("Trying to verify the materials on a null recipe!");
			return false;
		}
		if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
		{
			global::Debug.LogError("Trying to verify the materials on a recipe with no MaterialCategoryTags!");
			return false;
		}
		bool result = true;
		for (int i = 0; i < recipe.Ingredients.Count; i++)
		{
			if (MaterialSelectionPanel.Filter(recipe.Ingredients[i].tag).kgAvailable < recipe.Ingredients[i].amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600689B RID: 26779 RVA: 0x00273048 File Offset: 0x00271248
	public void Close()
	{
		if (this.configuring)
		{
			return;
		}
		this.ClearProduct(true);
		this.Show(false);
	}

	// Token: 0x040046B9 RID: 18105
	public TitleBar titleBar;

	// Token: 0x040046BA RID: 18106
	public GameObject ProductDescriptionPane;

	// Token: 0x040046BB RID: 18107
	public LocText productDescriptionText;

	// Token: 0x040046BC RID: 18108
	public DescriptorPanel ProductRequirementsPane;

	// Token: 0x040046BD RID: 18109
	public DescriptorPanel ProductEffectsPane;

	// Token: 0x040046BE RID: 18110
	public DescriptorPanel RoomConstrainsPanel;

	// Token: 0x040046BF RID: 18111
	public GameObject ProductFlavourPane;

	// Token: 0x040046C0 RID: 18112
	public LocText productFlavourText;

	// Token: 0x040046C1 RID: 18113
	public RectTransform BGPanel;

	// Token: 0x040046C2 RID: 18114
	public MaterialSelectionPanel materialSelectionPanelPrefab;

	// Token: 0x040046C3 RID: 18115
	public FacadeSelectionPanel facadeSelectionPanelPrefab;

	// Token: 0x040046C4 RID: 18116
	private Dictionary<string, GameObject> descLabels = new Dictionary<string, GameObject>();

	// Token: 0x040046C5 RID: 18117
	public MultiToggle sandboxInstantBuildToggle;

	// Token: 0x040046C6 RID: 18118
	private List<Tag> HiddenRoomConstrainTags = new List<Tag>
	{
		RoomConstraints.ConstraintTags.Refrigerator,
		RoomConstraints.ConstraintTags.FarmStationType,
		RoomConstraints.ConstraintTags.LuxuryBedType,
		RoomConstraints.ConstraintTags.MassageTable,
		RoomConstraints.ConstraintTags.MessTable,
		RoomConstraints.ConstraintTags.NatureReserve,
		RoomConstraints.ConstraintTags.Park,
		RoomConstraints.ConstraintTags.SpiceStation,
		RoomConstraints.ConstraintTags.DeStressingBuilding,
		RoomConstraints.ConstraintTags.Decor20,
		RoomConstraints.ConstraintTags.MachineShopType
	};

	// Token: 0x040046C7 RID: 18119
	[NonSerialized]
	public MaterialSelectionPanel materialSelectionPanel;

	// Token: 0x040046C8 RID: 18120
	[SerializeField]
	private FacadeSelectionPanel facadeSelectionPanel;

	// Token: 0x040046C9 RID: 18121
	[NonSerialized]
	public BuildingDef currentDef;

	// Token: 0x040046CA RID: 18122
	public System.Action onElementsFullySelected;

	// Token: 0x040046CB RID: 18123
	private bool expandedInfo = true;

	// Token: 0x040046CC RID: 18124
	private bool configuring;
}
