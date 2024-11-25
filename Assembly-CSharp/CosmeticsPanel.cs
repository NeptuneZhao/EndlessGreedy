using System;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1E RID: 3102
public class CosmeticsPanel : TargetPanel
{
	// Token: 0x06005F21 RID: 24353 RVA: 0x002357C7 File Offset: 0x002339C7
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x06005F22 RID: 24354 RVA: 0x002357CC File Offset: 0x002339CC
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		BuildingFacade buildingFacade = this.selectedTarget.GetComponent<BuildingFacade>();
		MinionIdentity component = this.selectedTarget.GetComponent<MinionIdentity>();
		this.selectionPanel.OnFacadeSelectionChanged = null;
		this.outfitCategoryButtonContainer.SetActive(component != null);
		if (component != null)
		{
			ClothingOutfitTarget outfitTarget = ClothingOutfitTarget.FromMinion(this.selectedOutfitCategory, component.gameObject);
			this.selectionPanel.SetOutfitTarget(outfitTarget, this.selectedOutfitCategory);
			FacadeSelectionPanel facadeSelectionPanel = this.selectionPanel;
			facadeSelectionPanel.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel.OnFacadeSelectionChanged, new System.Action(delegate()
			{
				if (this.selectionPanel.SelectedFacade == null || this.selectionPanel.SelectedFacade == "DEFAULT_FACADE")
				{
					outfitTarget.WriteItems(this.selectedOutfitCategory, new string[0]);
				}
				else
				{
					outfitTarget.WriteItems(this.selectedOutfitCategory, ClothingOutfitTarget.FromTemplateId(this.selectionPanel.SelectedFacade).ReadItems());
				}
				this.Refresh();
			}));
		}
		else if (buildingFacade != null)
		{
			this.selectionPanel.SetBuildingDef(this.selectedTarget.GetComponent<Building>().Def.PrefabID, this.selectedTarget.GetComponent<BuildingFacade>().CurrentFacade);
			this.selectionPanel.OnFacadeSelectionChanged = null;
			FacadeSelectionPanel facadeSelectionPanel2 = this.selectionPanel;
			facadeSelectionPanel2.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel2.OnFacadeSelectionChanged, new System.Action(delegate()
			{
				if (this.selectionPanel.SelectedFacade == null || this.selectionPanel.SelectedFacade == "DEFAULT_FACADE" || Db.GetBuildingFacades().TryGet(this.selectionPanel.SelectedFacade).IsNullOrDestroyed())
				{
					buildingFacade.ApplyDefaultFacade(true);
				}
				else
				{
					buildingFacade.ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.selectionPanel.SelectedFacade), true);
				}
				this.Refresh();
			}));
		}
		this.Refresh();
	}

	// Token: 0x06005F23 RID: 24355 RVA: 0x0023590C File Offset: 0x00233B0C
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
	}

	// Token: 0x06005F24 RID: 24356 RVA: 0x00235918 File Offset: 0x00233B18
	public void Refresh()
	{
		UnityEngine.Object component = this.selectedTarget.GetComponent<MinionIdentity>();
		BuildingFacade component2 = this.selectedTarget.GetComponent<BuildingFacade>();
		if (component != null)
		{
			ClothingOutfitTarget outfit = ClothingOutfitTarget.FromMinion(this.selectedOutfitCategory, this.selectedTarget);
			this.editButton.gameObject.SetActive(true);
			this.mannequin.gameObject.SetActive(true);
			this.mannequin.SetOutfit(outfit);
			this.buildingIcon.gameObject.SetActive(false);
			this.editButton.ClearOnClick();
			this.editButton.onClick += this.OnClickEditOutfit;
			this.nameLabel.SetText(outfit.ReadName());
			this.descriptionLabel.SetText("");
		}
		else if (component2 != null)
		{
			this.editButton.gameObject.SetActive(false);
			this.mannequin.gameObject.SetActive(false);
			this.buildingIcon.gameObject.SetActive(true);
			if (component2.CurrentFacade != null && component2.CurrentFacade != "DEFAULT_FACADE" && !Db.GetBuildingFacades().TryGet(component2.CurrentFacade).IsNullOrDestroyed())
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(component2.CurrentFacade);
				this.nameLabel.SetText(buildingFacadeResource.Name);
				this.descriptionLabel.SetText(buildingFacadeResource.Description);
				this.buildingIcon.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile), "ui", false, "");
			}
			else
			{
				string prefabID = component2.GetComponent<Building>().Def.PrefabID;
				StringEntry stringEntry;
				Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					prefabID.ToString().ToUpperInvariant(),
					".FACADES.DEFAULT_",
					prefabID.ToString().ToUpperInvariant(),
					".NAME"
				}), out stringEntry);
				if (stringEntry == null)
				{
					Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".NAME", out stringEntry);
				}
				StringEntry stringEntry2;
				Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					prefabID.ToString().ToUpperInvariant(),
					".FACADES.DEFAULT_",
					prefabID.ToString().ToUpperInvariant(),
					".DESC"
				}), out stringEntry2);
				if (stringEntry2 == null)
				{
					Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".DESC", out stringEntry2);
				}
				this.nameLabel.SetText((stringEntry != null) ? stringEntry : "");
				this.descriptionLabel.SetText((stringEntry2 != null) ? stringEntry2 : "");
				this.buildingIcon.sprite = Def.GetUISprite(prefabID, "ui", false).first;
			}
		}
		this.RefreshOutfitCategories();
		this.selectionPanel.Refresh();
	}

	// Token: 0x06005F25 RID: 24357 RVA: 0x00235C0C File Offset: 0x00233E0C
	public void OnClickEditOutfit()
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
		MinionBrowserScreenConfig.MinionInstances(this.selectedTarget).ApplyAndOpenScreen(delegate
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
		});
	}

	// Token: 0x06005F26 RID: 24358 RVA: 0x00235C68 File Offset: 0x00233E68
	private void RefreshOutfitCategories()
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, GameObject> keyValuePair in this.outfitCategories)
		{
			global::Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.outfitCategories.Clear();
		string[] array = new string[]
		{
			"outfit",
			"atmosuit"
		};
		Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, string>();
		dictionary.Add(ClothingOutfitUtility.OutfitType.Clothing, UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_OUTFIT);
		dictionary.Add(ClothingOutfitUtility.OutfitType.AtmoSuit, UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_ATMOSUIT);
		for (int i = 0; i < 3; i++)
		{
			if (i != 1)
			{
				int idx = i;
				GameObject gameObject = global::Util.KInstantiateUI(this.outfitCategoryButtonPrefab, this.outfitCategoryButtonContainer, true);
				this.outfitCategories.Add((ClothingOutfitUtility.OutfitType)idx, gameObject);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(dictionary[(ClothingOutfitUtility.OutfitType)i]);
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
				{
					this.selectedOutfitCategory = (ClothingOutfitUtility.OutfitType)idx;
					this.Refresh();
					this.selectionPanel.SelectedOutfitCategory = this.selectedOutfitCategory;
				}));
				component.ChangeState((this.selectedOutfitCategory == (ClothingOutfitUtility.OutfitType)idx) ? 1 : 0);
			}
		}
	}

	// Token: 0x04003FF6 RID: 16374
	[SerializeField]
	private GameObject cosmeticSlotContainer;

	// Token: 0x04003FF7 RID: 16375
	[SerializeField]
	private FacadeSelectionPanel selectionPanel;

	// Token: 0x04003FF8 RID: 16376
	[SerializeField]
	private LocText nameLabel;

	// Token: 0x04003FF9 RID: 16377
	[SerializeField]
	private LocText descriptionLabel;

	// Token: 0x04003FFA RID: 16378
	[SerializeField]
	private KButton editButton;

	// Token: 0x04003FFB RID: 16379
	[SerializeField]
	private UIMannequin mannequin;

	// Token: 0x04003FFC RID: 16380
	[SerializeField]
	private Image buildingIcon;

	// Token: 0x04003FFD RID: 16381
	[SerializeField]
	private Dictionary<ClothingOutfitUtility.OutfitType, GameObject> outfitCategories = new Dictionary<ClothingOutfitUtility.OutfitType, GameObject>();

	// Token: 0x04003FFE RID: 16382
	[SerializeField]
	private GameObject outfitCategoryButtonPrefab;

	// Token: 0x04003FFF RID: 16383
	[SerializeField]
	private GameObject outfitCategoryButtonContainer;

	// Token: 0x04004000 RID: 16384
	private ClothingOutfitUtility.OutfitType selectedOutfitCategory;
}
