using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000CBD RID: 3261
public class MaterialSelectionPanel : KScreen, IRender200ms
{
	// Token: 0x060064C6 RID: 25798 RVA: 0x0025BA17 File Offset: 0x00259C17
	public static void ClearStatics()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
	}

	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x060064C7 RID: 25799 RVA: 0x0025BA23 File Offset: 0x00259C23
	public Tag CurrentSelectedElement
	{
		get
		{
			if (this.materialSelectors.Count == 0)
			{
				return null;
			}
			return this.materialSelectors[0].CurrentSelectedElement;
		}
	}

	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x060064C8 RID: 25800 RVA: 0x0025BA4C File Offset: 0x00259C4C
	public IList<Tag> GetSelectedElementAsList
	{
		get
		{
			this.currentSelectedElements.Clear();
			foreach (MaterialSelector materialSelector in this.materialSelectors)
			{
				if (materialSelector.gameObject.activeSelf)
				{
					global::Debug.Assert(materialSelector.CurrentSelectedElement != null);
					this.currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
				}
			}
			return this.currentSelectedElements;
		}
	}

	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x060064C9 RID: 25801 RVA: 0x0025BAE0 File Offset: 0x00259CE0
	public PriorityScreen PriorityScreen
	{
		get
		{
			return this.priorityScreen;
		}
	}

	// Token: 0x060064CA RID: 25802 RVA: 0x0025BAE8 File Offset: 0x00259CE8
	protected override void OnPrefabInit()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		for (int i = 0; i < 3; i++)
		{
			MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(this.MaterialSelectorTemplate, base.gameObject, false);
			materialSelector.selectorIndex = i;
			this.materialSelectors.Add(materialSelector);
		}
		this.materialSelectors[0].gameObject.SetActive(true);
		this.MaterialSelectorTemplate.SetActive(false);
		this.ToggleResearchRequired(false);
		if (this.priorityScreenParent != null)
		{
			this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
			this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
			this.priorityScreenParent.transform.SetAsLastSibling();
		}
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, delegate(object d)
		{
			this.RefreshSelectors();
		}));
	}

	// Token: 0x060064CB RID: 25803 RVA: 0x0025BBE1 File Offset: 0x00259DE1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateOnSpawn = true;
	}

	// Token: 0x060064CC RID: 25804 RVA: 0x0025BBF0 File Offset: 0x00259DF0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
	}

	// Token: 0x060064CD RID: 25805 RVA: 0x0025BC58 File Offset: 0x00259E58
	public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions)Delegate.Combine(selector.selectMaterialActions, action);
		});
	}

	// Token: 0x060064CE RID: 25806 RVA: 0x0025BC89 File Offset: 0x00259E89
	public void ClearSelectActions()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = null;
		});
	}

	// Token: 0x060064CF RID: 25807 RVA: 0x0025BCB5 File Offset: 0x00259EB5
	public void ClearMaterialToggles()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.ClearMaterialToggles();
		});
	}

	// Token: 0x060064D0 RID: 25808 RVA: 0x0025BCE1 File Offset: 0x00259EE1
	public void ConfigureScreen(Recipe recipe, MaterialSelectionPanel.GetBuildableStateDelegate buildableStateCB, MaterialSelectionPanel.GetBuildableTooltipDelegate buildableTooltipCB)
	{
		this.activeRecipe = recipe;
		this.GetBuildableState = buildableStateCB;
		this.GetBuildableTooltip = buildableTooltipCB;
		this.RefreshSelectors();
	}

	// Token: 0x060064D1 RID: 25809 RVA: 0x0025BD00 File Offset: 0x00259F00
	public bool AllSelectorsSelected()
	{
		bool flag = false;
		foreach (MaterialSelector materialSelector in this.materialSelectors)
		{
			flag = (flag || materialSelector.gameObject.activeInHierarchy);
			if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == null)
			{
				return false;
			}
		}
		return flag;
	}

	// Token: 0x060064D2 RID: 25810 RVA: 0x0025BD88 File Offset: 0x00259F88
	public void RefreshSelectors()
	{
		if (this.activeRecipe == null)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.gameObject.SetActive(false);
		});
		BuildingDef buildingDef = this.activeRecipe.GetBuildingDef();
		bool flag = this.GetBuildableState(buildingDef);
		string text = this.GetBuildableTooltip(buildingDef);
		if (!flag)
		{
			this.ToggleResearchRequired(true);
			LocText[] componentsInChildren = this.ResearchRequired.GetComponentsInChildren<LocText>();
			componentsInChildren[0].text = "";
			componentsInChildren[1].text = text;
			componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(false);
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.ToggleResearchRequired(false);
			for (int i = 0; i < this.activeRecipe.Ingredients.Count; i++)
			{
				this.materialSelectors[i].gameObject.SetActive(true);
				this.materialSelectors[i].ConfigureScreen(this.activeRecipe.Ingredients[i], this.activeRecipe);
			}
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.transform.SetAsLastSibling();
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(true);
				this.buildToolRotateButton.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x060064D3 RID: 25811 RVA: 0x0025BF32 File Offset: 0x0025A132
	private void UpdateResourceToggleValues()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			if (selector.gameObject.activeSelf)
			{
				selector.RefreshToggleContents();
			}
		});
	}

	// Token: 0x060064D4 RID: 25812 RVA: 0x0025BF6C File Offset: 0x0025A16C
	private void ToggleResearchRequired(bool state)
	{
		if (this.ResearchRequired == null)
		{
			return;
		}
		this.ResearchRequired.SetActive(state);
	}

	// Token: 0x060064D5 RID: 25813 RVA: 0x0025BF8C File Offset: 0x0025A18C
	public bool AutoSelectAvailableMaterial()
	{
		bool result = true;
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (!this.materialSelectors[i].AutoSelectAvailableMaterial())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x060064D6 RID: 25814 RVA: 0x0025BFC8 File Offset: 0x0025A1C8
	public void SelectSourcesMaterials(Building building)
	{
		Tag[] array = null;
		Deconstructable component = building.gameObject.GetComponent<Deconstructable>();
		if (component != null)
		{
			array = component.constructionElements;
		}
		Constructable component2 = building.GetComponent<Constructable>();
		if (component2 != null)
		{
			array = component2.SelectedElementsTags.ToArray<Tag>();
		}
		if (array != null)
		{
			for (int i = 0; i < Mathf.Min(array.Length, this.materialSelectors.Count); i++)
			{
				if (this.materialSelectors[i].ElementToggles.ContainsKey(array[i]))
				{
					this.materialSelectors[i].OnSelectMaterial(array[i], this.activeRecipe, false);
				}
			}
		}
	}

	// Token: 0x060064D7 RID: 25815 RVA: 0x0025C06E File Offset: 0x0025A26E
	public void ForceSelectPrimaryTag(Tag tag)
	{
		this.materialSelectors[0].OnSelectMaterial(tag, this.activeRecipe, false);
	}

	// Token: 0x060064D8 RID: 25816 RVA: 0x0025C08C File Offset: 0x0025A28C
	public static MaterialSelectionPanel.SelectedElemInfo Filter(Tag _materialCategoryTag)
	{
		MaterialSelectionPanel.SelectedElemInfo selectedElemInfo = default(MaterialSelectionPanel.SelectedElemInfo);
		selectedElemInfo.element = null;
		selectedElemInfo.kgAvailable = 0f;
		if (DiscoveredResources.Instance == null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
		{
			return selectedElemInfo;
		}
		string[] array = _materialCategoryTag.ToString().Split('&', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			Tag tag = array[i];
			List<Tag> list = null;
			if (!MaterialSelectionPanel.elementsWithTag.TryGetValue(tag, out list))
			{
				list = new List<Tag>();
				foreach (Element element in ElementLoader.elements)
				{
					if (element.tag == tag || element.HasTag(tag))
					{
						list.Add(element.tag);
					}
				}
				foreach (Tag tag2 in GameTags.MaterialBuildingElements)
				{
					if (tag2 == tag)
					{
						foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag2))
						{
							KPrefabID component = gameObject.GetComponent<KPrefabID>();
							if (component != null && !list.Contains(component.PrefabTag))
							{
								list.Add(component.PrefabTag);
							}
						}
					}
				}
				MaterialSelectionPanel.elementsWithTag[tag] = list;
			}
			foreach (Tag tag3 in list)
			{
				float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag3, true);
				if (amount > selectedElemInfo.kgAvailable)
				{
					selectedElemInfo.kgAvailable = amount;
					selectedElemInfo.element = tag3;
				}
			}
		}
		return selectedElemInfo;
	}

	// Token: 0x060064D9 RID: 25817 RVA: 0x0025C2C0 File Offset: 0x0025A4C0
	public void ToggleShowDescriptorPanels(bool show)
	{
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (this.materialSelectors[i] != null)
			{
				this.materialSelectors[i].ToggleShowDescriptorsPanel(show);
			}
		}
	}

	// Token: 0x060064DA RID: 25818 RVA: 0x0025C309 File Offset: 0x0025A509
	private void OnPriorityClicked(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x060064DB RID: 25819 RVA: 0x0025C318 File Offset: 0x0025A518
	public void Render200ms(float dt)
	{
		this.UpdateResourceToggleValues();
	}

	// Token: 0x04004446 RID: 17478
	public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();

	// Token: 0x04004447 RID: 17479
	private List<MaterialSelector> materialSelectors = new List<MaterialSelector>();

	// Token: 0x04004448 RID: 17480
	private List<Tag> currentSelectedElements = new List<Tag>();

	// Token: 0x04004449 RID: 17481
	[SerializeField]
	protected PriorityScreen priorityScreenPrefab;

	// Token: 0x0400444A RID: 17482
	[SerializeField]
	protected GameObject priorityScreenParent;

	// Token: 0x0400444B RID: 17483
	[SerializeField]
	protected BuildToolRotateButtonUI buildToolRotateButton;

	// Token: 0x0400444C RID: 17484
	private PriorityScreen priorityScreen;

	// Token: 0x0400444D RID: 17485
	public GameObject MaterialSelectorTemplate;

	// Token: 0x0400444E RID: 17486
	public GameObject ResearchRequired;

	// Token: 0x0400444F RID: 17487
	private Recipe activeRecipe;

	// Token: 0x04004450 RID: 17488
	private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();

	// Token: 0x04004451 RID: 17489
	private MaterialSelectionPanel.GetBuildableStateDelegate GetBuildableState;

	// Token: 0x04004452 RID: 17490
	private MaterialSelectionPanel.GetBuildableTooltipDelegate GetBuildableTooltip;

	// Token: 0x04004453 RID: 17491
	private List<int> gameSubscriptionHandles = new List<int>();

	// Token: 0x02001DC7 RID: 7623
	// (Invoke) Token: 0x0600A997 RID: 43415
	public delegate bool GetBuildableStateDelegate(BuildingDef def);

	// Token: 0x02001DC8 RID: 7624
	// (Invoke) Token: 0x0600A99B RID: 43419
	public delegate string GetBuildableTooltipDelegate(BuildingDef def);

	// Token: 0x02001DC9 RID: 7625
	// (Invoke) Token: 0x0600A99F RID: 43423
	public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

	// Token: 0x02001DCA RID: 7626
	public struct SelectedElemInfo
	{
		// Token: 0x04008845 RID: 34885
		public Tag element;

		// Token: 0x04008846 RID: 34886
		public float kgAvailable;
	}
}
