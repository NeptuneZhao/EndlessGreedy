using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CBE RID: 3262
public class MaterialSelector : KScreen
{
	// Token: 0x060064DF RID: 25823 RVA: 0x0025C368 File Offset: 0x0025A568
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleGroup = base.GetComponent<ToggleGroup>();
	}

	// Token: 0x060064E0 RID: 25824 RVA: 0x0025C37C File Offset: 0x0025A57C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060064E1 RID: 25825 RVA: 0x0025C390 File Offset: 0x0025A590
	public void ClearMaterialToggles()
	{
		this.CurrentSelectedElement = null;
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			keyValuePair.Value.gameObject.SetActive(false);
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.ElementToggles.Clear();
	}

	// Token: 0x060064E2 RID: 25826 RVA: 0x0025C428 File Offset: 0x0025A628
	public static List<Tag> GetValidMaterials(Tag _materialTypeTag, bool omitDisabledElements = false)
	{
		string[] array = _materialTypeTag.ToString().Split('&', StringSplitOptions.None);
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < array.Length; i++)
		{
			Tag tag = array[i];
			foreach (Element element in ElementLoader.elements)
			{
				if ((!element.disabled || !omitDisabledElements) && element.IsSolid && (element.tag == tag || element.HasTag(tag)))
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
		}
		return list;
	}

	// Token: 0x060064E3 RID: 25827 RVA: 0x0025C598 File Offset: 0x0025A798
	public void ConfigureScreen(Recipe.Ingredient ingredient, Recipe recipe)
	{
		this.activeIngredient = ingredient;
		this.activeRecipe = recipe;
		this.activeMass = ingredient.amount;
		List<Tag> validMaterials = MaterialSelector.GetValidMaterials(ingredient.tag, false);
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (!validMaterials.Contains(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.ElementToggles[key].gameObject.SetActive(false);
			Util.KDestroyGameObject(this.ElementToggles[key].gameObject);
			this.ElementToggles.Remove(key);
		}
		foreach (Tag tag in validMaterials)
		{
			if (!this.ElementToggles.ContainsKey(tag))
			{
				GameObject gameObject = Util.KInstantiate(this.TogglePrefab, this.LayoutContainer, "MaterialSelection_" + tag.ProperName());
				gameObject.transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				KToggle component = gameObject.GetComponent<KToggle>();
				this.ElementToggles.Add(tag, component);
				component.group = this.toggleGroup;
				gameObject.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();
			}
		}
		this.ConfigureMaterialTooltips();
		this.RefreshToggleContents();
	}

	// Token: 0x060064E4 RID: 25828 RVA: 0x0025C770 File Offset: 0x0025A970
	private void SetToggleBGImage(KToggle toggle, Tag elem)
	{
		if (toggle == this.selectedToggle)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponent<ImageToggleState>().SetActive();
			return;
		}
		if (ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponentsInChildren<Image>()[1].color = Color.white;
			toggle.GetComponent<ImageToggleState>().SetInactive();
			return;
		}
		toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimMaterialUIDesaturated;
		toggle.GetComponentsInChildren<Image>()[1].color = new Color(1f, 1f, 1f, 0.6f);
		if (!MaterialSelector.AllowInsufficientMaterialBuild())
		{
			toggle.GetComponent<ImageToggleState>().SetDisabled();
		}
	}

	// Token: 0x060064E5 RID: 25829 RVA: 0x0025C864 File Offset: 0x0025AA64
	public void OnSelectMaterial(Tag elem, Recipe recipe, bool focusScrollRect = false)
	{
		KToggle x = this.ElementToggles[elem];
		if (x != this.selectedToggle)
		{
			this.selectedToggle = x;
			if (recipe != null)
			{
				SaveGame.Instance.materialSelectorSerializer.SetSelectedElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, recipe.Result, elem);
			}
			this.CurrentSelectedElement = elem;
			if (this.selectMaterialActions != null)
			{
				this.selectMaterialActions();
			}
			this.UpdateHeader();
			this.SetDescription(elem);
			this.SetEffects(elem);
			if (this.MaterialDescriptionPane != null)
			{
				if (!this.MaterialDescriptionPane.gameObject.activeSelf && !this.MaterialEffectsPane.gameObject.activeSelf)
				{
					this.DescriptorsPanel.SetActive(false);
				}
				else
				{
					this.DescriptorsPanel.SetActive(true);
				}
			}
		}
		if (focusScrollRect && this.ElementToggles.Count > 1)
		{
			List<Tag> list = new List<Tag>();
			foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
			{
				list.Add(keyValuePair.Key);
			}
			list.Sort(new Comparison<Tag>(this.ElementSorter));
			float num = (float)list.IndexOf(elem);
			int constraintCount = this.LayoutContainer.GetComponent<GridLayoutGroup>().constraintCount;
			float num2 = num / (float)constraintCount / (float)Math.Max((list.Count - 1) / constraintCount, 1);
			this.ScrollRect.normalizedPosition = new Vector2(0f, 1f - num2);
		}
		this.RefreshToggleContents();
	}

	// Token: 0x060064E6 RID: 25830 RVA: 0x0025CA08 File Offset: 0x0025AC08
	public void RefreshToggleContents()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			KToggle value = keyValuePair.Value;
			Tag elem = keyValuePair.Key;
			GameObject gameObject = value.gameObject;
			bool flag = DiscoveredResources.Instance.IsDiscovered(elem) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
			if (gameObject.activeSelf != flag)
			{
				gameObject.SetActive(flag);
			}
			if (flag)
			{
				LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>();
				LocText locText = componentsInChildren[0];
				TMP_Text tmp_Text = componentsInChildren[1];
				Image image = gameObject.GetComponentsInChildren<Image>()[1];
				tmp_Text.text = Util.FormatWholeNumber(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true));
				locText.text = Util.FormatWholeNumber(this.activeMass);
				GameObject gameObject2 = Assets.TryGetPrefab(keyValuePair.Key);
				if (gameObject2 != null)
				{
					KBatchedAnimController component = gameObject2.GetComponent<KBatchedAnimController>();
					image.sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
				}
				this.SetToggleBGImage(keyValuePair.Value, keyValuePair.Key);
				value.soundPlayer.AcceptClickCondition = (() => this.IsEnoughMass(elem));
				value.ClearOnClick();
				if (this.IsEnoughMass(elem))
				{
					value.onClick += delegate()
					{
						this.OnSelectMaterial(elem, this.activeRecipe, false);
					};
				}
			}
		}
		this.SortElementToggles();
		this.UpdateHeader();
	}

	// Token: 0x060064E7 RID: 25831 RVA: 0x0025CBC4 File Offset: 0x0025ADC4
	private bool IsEnoughMass(Tag t)
	{
		return ClusterManager.Instance.activeWorld.worldInventory.GetAmount(t, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || MaterialSelector.AllowInsufficientMaterialBuild();
	}

	// Token: 0x060064E8 RID: 25832 RVA: 0x0025CC00 File Offset: 0x0025AE00
	public bool AutoSelectAvailableMaterial()
	{
		if (this.activeRecipe == null || this.ElementToggles.Count == 0)
		{
			return false;
		}
		Tag previousElement = SaveGame.Instance.materialSelectorSerializer.GetPreviousElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, this.activeRecipe.Result);
		if (previousElement != null)
		{
			KToggle x;
			this.ElementToggles.TryGetValue(previousElement, out x);
			if (x != null && (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || ClusterManager.Instance.activeWorld.worldInventory.GetAmount(previousElement, true) >= this.activeMass))
			{
				this.OnSelectMaterial(previousElement, this.activeRecipe, true);
				return true;
			}
		}
		float num = -1f;
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort(new Comparison<Tag>(this.ElementSorter));
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			this.OnSelectMaterial(list[0], this.activeRecipe, true);
			return true;
		}
		Tag tag = null;
		foreach (Tag tag2 in list)
		{
			float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag2, true);
			if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag2))
			{
				num2 = Mathf.Min(this.activeMass, num2);
			}
			if (num2 >= this.activeMass && num2 > num)
			{
				num = num2;
				tag = tag2;
			}
		}
		if (tag != null)
		{
			UISounds.PlaySound(UISounds.Sound.Object_AutoSelected);
			string arg = (ElementLoader.GetElement(tag) == null) ? tag.ToString() : tag.Name;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(MISC.POPFX.RESOURCE_SELECTION_CHANGED, arg), null, Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()), 1.5f, false, false);
			this.OnSelectMaterial(tag, this.activeRecipe, true);
			return true;
		}
		return false;
	}

	// Token: 0x060064E9 RID: 25833 RVA: 0x0025CE5C File Offset: 0x0025B05C
	private void SortElementToggles()
	{
		bool flag = false;
		int num = -1;
		this.elementsToSort.Clear();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				this.elementsToSort.Add(keyValuePair.Key);
			}
		}
		this.elementsToSort.Sort(new Comparison<Tag>(this.ElementSorter));
		for (int i = 0; i < this.elementsToSort.Count; i++)
		{
			int siblingIndex = this.ElementToggles[this.elementsToSort[i]].transform.GetSiblingIndex();
			if (siblingIndex <= num)
			{
				flag = true;
				break;
			}
			num = siblingIndex;
		}
		if (flag)
		{
			foreach (Tag key in this.elementsToSort)
			{
				this.ElementToggles[key].transform.SetAsLastSibling();
			}
		}
		this.UpdateScrollBar();
	}

	// Token: 0x060064EA RID: 25834 RVA: 0x0025CF9C File Offset: 0x0025B19C
	private void ConfigureMaterialTooltips()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			ToolTip component = keyValuePair.Value.gameObject.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = GameUtil.GetMaterialTooltips(keyValuePair.Key);
			}
		}
	}

	// Token: 0x060064EB RID: 25835 RVA: 0x0025D018 File Offset: 0x0025B218
	private void UpdateScrollBar()
	{
		if (this.Scrollbar == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		if (this.Scrollbar.activeSelf != num > 5)
		{
			this.Scrollbar.SetActive(num > 5);
		}
		this.ScrollRect.GetComponent<LayoutElement>().minHeight = (float)(74 * ((num <= 5) ? 1 : 2));
	}

	// Token: 0x060064EC RID: 25836 RVA: 0x0025D0C8 File Offset: 0x0025B2C8
	private void UpdateHeader()
	{
		if (this.activeIngredient == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		LocText componentInChildren = this.Headerbar.GetComponentInChildren<LocText>();
		string[] array = this.activeIngredient.tag.ToString().Split('&', StringSplitOptions.None);
		string text = array[0].ToTag().ProperName();
		for (int i = 1; i < array.Length; i++)
		{
			text = text + " or " + array[i].ToTag().ProperName();
		}
		if (num == 0)
		{
			componentInChildren.text = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_TITLE, text, GameUtil.GetFormattedMass(this.activeIngredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string text2 = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_DESC, text);
			this.NoMaterialDiscovered.text = text2;
			this.NoMaterialDiscovered.gameObject.SetActive(true);
			this.NoMaterialDiscovered.color = Constants.NEGATIVE_COLOR;
			this.BadBG.SetActive(true);
			if (this.Scrollbar != null)
			{
				this.Scrollbar.SetActive(false);
			}
			this.LayoutContainer.SetActive(false);
			return;
		}
		componentInChildren.text = string.Format(UI.PRODUCTINFO_SELECTMATERIAL, text);
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		this.BadBG.SetActive(false);
		this.LayoutContainer.SetActive(true);
		this.UpdateScrollBar();
	}

	// Token: 0x060064ED RID: 25837 RVA: 0x0025D28C File Offset: 0x0025B48C
	public void ToggleShowDescriptorsPanel(bool show)
	{
		if (this.DescriptorsPanel == null)
		{
			return;
		}
		this.DescriptorsPanel.gameObject.SetActive(show);
	}

	// Token: 0x060064EE RID: 25838 RVA: 0x0025D2B0 File Offset: 0x0025B4B0
	private void SetDescription(Tag element)
	{
		if (this.DescriptorsPanel == null)
		{
			return;
		}
		StringEntry stringEntry = null;
		if (Strings.TryGet(new StringKey("STRINGS.ELEMENTS." + element.ToString().ToUpper() + ".BUILD_DESC"), out stringEntry))
		{
			this.MaterialDescriptionText.text = stringEntry.ToString();
			this.MaterialDescriptionPane.SetActive(true);
			return;
		}
		this.MaterialDescriptionPane.SetActive(false);
	}

	// Token: 0x060064EF RID: 25839 RVA: 0x0025D328 File Offset: 0x0025B528
	private void SetEffects(Tag element)
	{
		if (this.MaterialDescriptionPane == null)
		{
			return;
		}
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.MaterialEffectsPane.gameObject.SetActive(true);
			this.MaterialEffectsPane.SetDescriptors(materialDescriptors);
			return;
		}
		this.MaterialEffectsPane.gameObject.SetActive(false);
	}

	// Token: 0x060064F0 RID: 25840 RVA: 0x0025D3AF File Offset: 0x0025B5AF
	public static bool AllowInsufficientMaterialBuild()
	{
		return GenericGameSettings.instance.allowInsufficientMaterialBuild;
	}

	// Token: 0x060064F1 RID: 25841 RVA: 0x0025D3BC File Offset: 0x0025B5BC
	private int ElementSorter(Tag at, Tag bt)
	{
		GameObject gameObject = Assets.TryGetPrefab(at);
		IHasSortOrder hasSortOrder = (gameObject != null) ? gameObject.GetComponent<IHasSortOrder>() : null;
		GameObject gameObject2 = Assets.TryGetPrefab(bt);
		IHasSortOrder hasSortOrder2 = (gameObject2 != null) ? gameObject2.GetComponent<IHasSortOrder>() : null;
		if (hasSortOrder == null || hasSortOrder2 == null)
		{
			return 0;
		}
		Element element = ElementLoader.GetElement(at);
		Element element2 = ElementLoader.GetElement(bt);
		if (element != null && element2 != null && element.buildMenuSort == element2.buildMenuSort)
		{
			return element.idx.CompareTo(element2.idx);
		}
		return hasSortOrder.sortOrder.CompareTo(hasSortOrder2.sortOrder);
	}

	// Token: 0x04004454 RID: 17492
	public static List<Tag> DeprioritizeAutoSelectElementList = new List<Tag>
	{
		SimHashes.WoodLog.ToString().ToTag(),
		SimHashes.SolidMercury.ToString().ToTag(),
		SimHashes.Lead.ToString().ToTag()
	};

	// Token: 0x04004455 RID: 17493
	public Tag CurrentSelectedElement;

	// Token: 0x04004456 RID: 17494
	public Dictionary<Tag, KToggle> ElementToggles = new Dictionary<Tag, KToggle>();

	// Token: 0x04004457 RID: 17495
	public int selectorIndex;

	// Token: 0x04004458 RID: 17496
	public MaterialSelector.SelectMaterialActions selectMaterialActions;

	// Token: 0x04004459 RID: 17497
	public MaterialSelector.SelectMaterialActions deselectMaterialActions;

	// Token: 0x0400445A RID: 17498
	private ToggleGroup toggleGroup;

	// Token: 0x0400445B RID: 17499
	public GameObject TogglePrefab;

	// Token: 0x0400445C RID: 17500
	public GameObject LayoutContainer;

	// Token: 0x0400445D RID: 17501
	public KScrollRect ScrollRect;

	// Token: 0x0400445E RID: 17502
	public GameObject Scrollbar;

	// Token: 0x0400445F RID: 17503
	public GameObject Headerbar;

	// Token: 0x04004460 RID: 17504
	public GameObject BadBG;

	// Token: 0x04004461 RID: 17505
	public LocText NoMaterialDiscovered;

	// Token: 0x04004462 RID: 17506
	public GameObject MaterialDescriptionPane;

	// Token: 0x04004463 RID: 17507
	public LocText MaterialDescriptionText;

	// Token: 0x04004464 RID: 17508
	public DescriptorPanel MaterialEffectsPane;

	// Token: 0x04004465 RID: 17509
	public GameObject DescriptorsPanel;

	// Token: 0x04004466 RID: 17510
	private KToggle selectedToggle;

	// Token: 0x04004467 RID: 17511
	private Recipe.Ingredient activeIngredient;

	// Token: 0x04004468 RID: 17512
	private Recipe activeRecipe;

	// Token: 0x04004469 RID: 17513
	private float activeMass;

	// Token: 0x0400446A RID: 17514
	private List<Tag> elementsToSort = new List<Tag>();

	// Token: 0x02001DCD RID: 7629
	// (Invoke) Token: 0x0600A9AB RID: 43435
	public delegate void SelectMaterialActions();
}
