using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DA0 RID: 3488
public class SelectedRecipeQueueScreen : KScreen
{
	// Token: 0x06006E1B RID: 28187 RVA: 0x00296970 File Offset: 0x00294B70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DecrementButton.onClick = delegate()
		{
			this.target.DecrementRecipeQueueCount(this.selectedRecipe, false);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.IncrementButton.onClick = delegate()
		{
			this.target.IncrementRecipeQueueCount(this.selectedRecipe);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.InfiniteButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER;
		this.InfiniteButton.onClick += delegate()
		{
			if (this.target.GetRecipeQueueCount(this.selectedRecipe) != ComplexFabricator.QUEUE_INFINITE)
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, ComplexFabricator.QUEUE_INFINITE);
			}
			else
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, 0);
			}
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onEndEdit += delegate()
		{
			base.isEditing = false;
			this.target.SetRecipeQueueCount(this.selectedRecipe, Mathf.RoundToInt(this.QueueCount.currentValue));
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onStartEdit += delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
		};
		MultiToggle multiToggle = this.previousRecipeButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.CyclePreviousRecipe));
		MultiToggle multiToggle2 = this.nextRecipeButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.CycleNextRecipe));
	}

	// Token: 0x06006E1C RID: 28188 RVA: 0x00296A60 File Offset: 0x00294C60
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedRecipe != null)
		{
			GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
			Equippable equippable = (prefab != null) ? prefab.GetComponent<Equippable>() : null;
			if (equippable != null && equippable.GetBuildOverride() != null)
			{
				this.minionWidget.RemoveEquipment(equippable);
			}
		}
	}

	// Token: 0x06006E1D RID: 28189 RVA: 0x00296ACC File Offset: 0x00294CCC
	public void SetRecipe(ComplexFabricatorSideScreen owner, ComplexFabricator target, ComplexRecipe recipe)
	{
		this.ownerScreen = owner;
		this.target = target;
		this.selectedRecipe = recipe;
		this.recipeName.text = recipe.GetUIName(false);
		global::Tuple<Sprite, Color> tuple = (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? Def.GetUISprite(recipe.ingredients[0].material, "ui", false) : Def.GetUISprite(recipe.results[0].material, recipe.results[0].facadeID);
		if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
		{
			this.recipeIcon.sprite = owner.radboltSprite;
		}
		else
		{
			this.recipeIcon.sprite = tuple.first;
			this.recipeIcon.color = tuple.second;
		}
		string text = (recipe.time.ToString() + " " + UI.UNITSUFFIXES.SECONDS).ToLower();
		this.recipeMainDescription.SetText(recipe.description);
		this.recipeDuration.SetText(text);
		string simpleTooltip = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPE_WORKTIME, text);
		this.recipeDurationTooltip.SetSimpleTooltip(simpleTooltip);
		this.RefreshIngredientDescriptors();
		this.RefreshResultDescriptors();
		this.RefreshQueueCountDisplay();
		this.ToggleAndRefreshMinionDisplay();
	}

	// Token: 0x06006E1E RID: 28190 RVA: 0x00296BFC File Offset: 0x00294DFC
	private void CyclePreviousRecipe()
	{
		this.ownerScreen.CycleRecipe(-1);
	}

	// Token: 0x06006E1F RID: 28191 RVA: 0x00296C0A File Offset: 0x00294E0A
	private void CycleNextRecipe()
	{
		this.ownerScreen.CycleRecipe(1);
	}

	// Token: 0x06006E20 RID: 28192 RVA: 0x00296C18 File Offset: 0x00294E18
	private void ToggleAndRefreshMinionDisplay()
	{
		this.minionWidget.gameObject.SetActive(this.RefreshMinionDisplayAnim());
	}

	// Token: 0x06006E21 RID: 28193 RVA: 0x00296C30 File Offset: 0x00294E30
	private bool RefreshMinionDisplayAnim()
	{
		GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
		if (prefab == null)
		{
			return false;
		}
		Equippable component = prefab.GetComponent<Equippable>();
		if (component == null)
		{
			return false;
		}
		KAnimFile buildOverride = component.GetBuildOverride();
		if (buildOverride == null)
		{
			return false;
		}
		this.minionWidget.SetDefaultPortraitAnimator();
		KAnimFile animFile = buildOverride;
		if (!this.selectedRecipe.results[0].facadeID.IsNullOrWhiteSpace())
		{
			EquippableFacadeResource equippableFacadeResource = Db.GetEquippableFacades().TryGet(this.selectedRecipe.results[0].facadeID);
			if (equippableFacadeResource != null)
			{
				animFile = Assets.GetAnim(equippableFacadeResource.BuildOverride);
			}
		}
		this.minionWidget.UpdateEquipment(component, animFile);
		return true;
	}

	// Token: 0x06006E22 RID: 28194 RVA: 0x00296CEC File Offset: 0x00294EEC
	private void RefreshQueueCountDisplay()
	{
		this.ResearchRequiredContainer.SetActive(!this.selectedRecipe.IsRequiredTechUnlocked());
		bool flag = this.target.GetRecipeQueueCount(this.selectedRecipe) == ComplexFabricator.QUEUE_INFINITE;
		if (!flag)
		{
			this.QueueCount.SetAmount((float)this.target.GetRecipeQueueCount(this.selectedRecipe));
		}
		else
		{
			this.QueueCount.SetDisplayValue("");
		}
		this.InfiniteIcon.gameObject.SetActive(flag);
	}

	// Token: 0x06006E23 RID: 28195 RVA: 0x00296D70 File Offset: 0x00294F70
	private void RefreshResultDescriptors()
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		list.AddRange(this.GetResultDescriptions(this.selectedRecipe));
		foreach (Descriptor desc in this.target.AdditionalEffectsForRecipe(this.selectedRecipe))
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
		}
		if (list.Count > 0)
		{
			this.EffectsDescriptorPanel.gameObject.SetActive(true);
			foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeEffectsDescriptorRows)
			{
				Util.KDestroyGameObject(keyValuePair.Value);
			}
			this.recipeEffectsDescriptorRows.Clear();
			foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list)
			{
				GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.EffectsDescriptorPanel.gameObject, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(descriptorWithSprite.descriptor.IndentedText());
				component.GetReference<Image>("Icon").sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
				component.GetReference<Image>("Icon").color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
				component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
				component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
				this.recipeEffectsDescriptorRows.Add(descriptorWithSprite, gameObject);
			}
		}
	}

	// Token: 0x06006E24 RID: 28196 RVA: 0x00296F70 File Offset: 0x00295170
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetResultDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		if (recipe.producedHEP > 0)
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.producedHEP), string.Format("<b>{0}</b>: {1}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.producedHEP), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.results)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, recipeElement.facadeID), false));
			Element element = ElementLoader.GetElement(recipeElement.material);
			if (element != null)
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list2 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc in GameUtil.GetMaterialDescriptors(element))
				{
					list2.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list2)
				{
					descriptorWithSprite.descriptor.IncreaseIndent();
				}
				list.AddRange(list2);
			}
			else
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list3 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc2 in GameUtil.GetEffectDescriptors(GameUtil.GetAllDescriptors(prefab, false)))
				{
					list3.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc2, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite2 in list3)
				{
					descriptorWithSprite2.descriptor.IncreaseIndent();
				}
				list.AddRange(list3);
			}
		}
		return list;
	}

	// Token: 0x06006E25 RID: 28197 RVA: 0x00297240 File Offset: 0x00295440
	private void RefreshIngredientDescriptors()
	{
		new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> ingredientDescriptions = this.GetIngredientDescriptions(this.selectedRecipe);
		this.IngredientsDescriptorPanel.gameObject.SetActive(true);
		foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeIngredientDescriptorRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.recipeIngredientDescriptorRows.Clear();
		foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in ingredientDescriptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.IngredientsDescriptorPanel.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(descriptorWithSprite.descriptor.IndentedText());
			component.GetReference<Image>("Icon").sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
			component.GetReference<Image>("Icon").color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
			component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
			component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
			this.recipeIngredientDescriptorRows.Add(descriptorWithSprite, gameObject);
		}
	}

	// Token: 0x06006E26 RID: 28198 RVA: 0x002973D8 File Offset: 0x002955D8
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetIngredientDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			float amount = this.target.GetMyWorld().worldInventory.GetAmount(recipeElement.material, true);
			string formattedByTag2 = GameUtil.GetFormattedByTag(recipeElement.material, amount, GameUtil.TimeSlice.None);
			string text = (amount >= recipeElement.amount) ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) : ("<color=#F44A47>" + string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) + "</color>");
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(text, text, Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, "ui", false), Assets.GetPrefab(recipeElement.material).GetComponent<MutantPlant>() != null));
		}
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = this.target.GetComponent<HighEnergyParticleStorage>();
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1} / {2}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.consumedHEP, component.Particles), string.Format("<b>{0}</b>: {1} / {2}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.consumedHEP, component.Particles), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		return list;
	}

	// Token: 0x04004B23 RID: 19235
	public Image recipeIcon;

	// Token: 0x04004B24 RID: 19236
	public LocText recipeName;

	// Token: 0x04004B25 RID: 19237
	public LocText recipeMainDescription;

	// Token: 0x04004B26 RID: 19238
	public LocText recipeDuration;

	// Token: 0x04004B27 RID: 19239
	public ToolTip recipeDurationTooltip;

	// Token: 0x04004B28 RID: 19240
	public GameObject IngredientsDescriptorPanel;

	// Token: 0x04004B29 RID: 19241
	public GameObject EffectsDescriptorPanel;

	// Token: 0x04004B2A RID: 19242
	public KNumberInputField QueueCount;

	// Token: 0x04004B2B RID: 19243
	public MultiToggle DecrementButton;

	// Token: 0x04004B2C RID: 19244
	public MultiToggle IncrementButton;

	// Token: 0x04004B2D RID: 19245
	public KButton InfiniteButton;

	// Token: 0x04004B2E RID: 19246
	public GameObject InfiniteIcon;

	// Token: 0x04004B2F RID: 19247
	public GameObject ResearchRequiredContainer;

	// Token: 0x04004B30 RID: 19248
	private ComplexFabricator target;

	// Token: 0x04004B31 RID: 19249
	private ComplexFabricatorSideScreen ownerScreen;

	// Token: 0x04004B32 RID: 19250
	private ComplexRecipe selectedRecipe;

	// Token: 0x04004B33 RID: 19251
	[SerializeField]
	private GameObject recipeElementDescriptorPrefab;

	// Token: 0x04004B34 RID: 19252
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeIngredientDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x04004B35 RID: 19253
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeEffectsDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x04004B36 RID: 19254
	[SerializeField]
	private FullBodyUIMinionWidget minionWidget;

	// Token: 0x04004B37 RID: 19255
	[SerializeField]
	private MultiToggle previousRecipeButton;

	// Token: 0x04004B38 RID: 19256
	[SerializeField]
	private MultiToggle nextRecipeButton;

	// Token: 0x02001EBF RID: 7871
	private class DescriptorWithSprite
	{
		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600AC51 RID: 44113 RVA: 0x003A6F2F File Offset: 0x003A512F
		public Descriptor descriptor { get; }

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x0600AC52 RID: 44114 RVA: 0x003A6F37 File Offset: 0x003A5137
		public global::Tuple<Sprite, Color> tintedSprite { get; }

		// Token: 0x0600AC53 RID: 44115 RVA: 0x003A6F3F File Offset: 0x003A513F
		public DescriptorWithSprite(Descriptor desc, global::Tuple<Sprite, Color> sprite, bool filterRowVisible = false)
		{
			this.descriptor = desc;
			this.tintedSprite = sprite;
			this.showFilterRow = filterRowVisible;
		}

		// Token: 0x04008B68 RID: 35688
		public bool showFilterRow;
	}
}
