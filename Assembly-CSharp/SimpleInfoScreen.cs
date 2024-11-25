﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using ProcGen;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DBD RID: 3517
public class SimpleInfoScreen : DetailScreenTab, ISim4000ms, ISim1000ms
{
	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06006F67 RID: 28519 RVA: 0x0029CCD4 File Offset: 0x0029AED4
	// (set) Token: 0x06006F68 RID: 28520 RVA: 0x0029CCDC File Offset: 0x0029AEDC
	public CollapsibleDetailContentPanel StoragePanel { get; private set; }

	// Token: 0x06006F69 RID: 28521 RVA: 0x0029CCE5 File Offset: 0x0029AEE5
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x06006F6A RID: 28522 RVA: 0x0029CCE8 File Offset: 0x0029AEE8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.processConditionContainer = base.CreateCollapsableSection(UI.DETAILTABS.PROCESS_CONDITIONS.NAME);
		this.statusItemPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_STATUS);
		this.statusItemPanel.Content.GetComponent<VerticalLayoutGroup>().padding.bottom = 10;
		this.statusItemPanel.scalerMask.hoverLock = true;
		this.statusItemsFolder = this.statusItemPanel.Content.gameObject;
		this.spaceSimpleInfoPOIPanel = new SpacePOISimpleInfoPanel(this);
		this.spacePOIPanel = base.CreateCollapsableSection(null);
		this.rocketSimpleInfoPanel = new RocketSimpleInfoPanel(this);
		this.rocketStatusContainer = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ROCKET);
		this.vitalsPanel = global::Util.KInstantiateUI(this.VitalsPanelTemplate, base.gameObject, false).GetComponent<MinionVitalsPanel>();
		this.fertilityPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_FERTILITY);
		this.infoPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_DESCRIPTION);
		this.requirementsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
		this.requirementContent = global::Util.KInstantiateUI<DescriptorPanel>(this.DescriptorContentPrefab.gameObject, this.requirementsPanel.Content.gameObject, false);
		this.effectsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_EFFECTS);
		this.effectsContent = global::Util.KInstantiateUI<DescriptorPanel>(this.DescriptorContentPrefab.gameObject, this.effectsPanel.Content.gameObject, false);
		this.worldMeteorShowersPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_METEORSHOWERS);
		this.worldElementsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_ELEMENTS);
		this.worldGeysersPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_GEYSERS);
		this.worldTraitsPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_WORLDTRAITS);
		this.worldBiomesPanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_BIOMES);
		this.worldLifePanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_LIFE);
		this.StoragePanel = base.CreateCollapsableSection(null);
		this.stressPanel = base.CreateCollapsableSection(null);
		this.stressDrawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.stressPanel.Content.gameObject);
		this.movePanel = base.CreateCollapsableSection(UI.DETAILTABS.SIMPLEINFO.GROUPNAME_MOVABLE);
		base.Subscribe<SimpleInfoScreen>(-1514841199, SimpleInfoScreen.OnRefreshDataDelegate);
	}

	// Token: 0x06006F6B RID: 28523 RVA: 0x0029CF50 File Offset: 0x0029B150
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		base.Subscribe(target, -1697596308, new Action<object>(this.TriggerRefreshStorage));
		base.Subscribe(target, -1197125120, new Action<object>(this.TriggerRefreshStorage));
		base.Subscribe(target, 1059811075, new Action<object>(this.OnBreedingChanceChanged));
		KSelectable component = target.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
			if (statusItemGroup != null)
			{
				StatusItemGroup statusItemGroup2 = statusItemGroup;
				statusItemGroup2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(statusItemGroup2.OnAddStatusItem, new Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem));
				StatusItemGroup statusItemGroup3 = statusItemGroup;
				statusItemGroup3.OnRemoveStatusItem = (Action<StatusItemGroup.Entry, bool>)Delegate.Combine(statusItemGroup3.OnRemoveStatusItem, new Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem));
				foreach (StatusItemGroup.Entry entry in statusItemGroup)
				{
					if (entry.category != null && entry.category.Id == "Main")
					{
						this.DoAddStatusItem(entry, entry.category, false);
					}
				}
				foreach (StatusItemGroup.Entry entry2 in statusItemGroup)
				{
					if (entry2.category == null || entry2.category.Id != "Main")
					{
						this.DoAddStatusItem(entry2, entry2.category, false);
					}
				}
			}
		}
		this.statusItemPanel.gameObject.SetActive(true);
		this.statusItemPanel.scalerMask.UpdateSize();
		this.Refresh(true);
		this.RefreshWorldPanel();
		this.RefreshProcessConditionsPanel();
		this.spaceSimpleInfoPOIPanel.Refresh(this.spacePOIPanel, this.selectedTarget);
	}

	// Token: 0x06006F6C RID: 28524 RVA: 0x0029D124 File Offset: 0x0029B324
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
		if (target != null)
		{
			base.Unsubscribe(target, -1697596308, new Action<object>(this.TriggerRefreshStorage));
			base.Unsubscribe(target, -1197125120, new Action<object>(this.TriggerRefreshStorage));
			base.Unsubscribe(target, 1059811075, new Action<object>(this.OnBreedingChanceChanged));
		}
		KSelectable component = target.GetComponent<KSelectable>();
		if (component != null)
		{
			StatusItemGroup statusItemGroup = component.GetStatusItemGroup();
			if (statusItemGroup != null)
			{
				StatusItemGroup statusItemGroup2 = statusItemGroup;
				statusItemGroup2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(statusItemGroup2.OnAddStatusItem, new Action<StatusItemGroup.Entry, StatusItemCategory>(this.OnAddStatusItem));
				StatusItemGroup statusItemGroup3 = statusItemGroup;
				statusItemGroup3.OnRemoveStatusItem = (Action<StatusItemGroup.Entry, bool>)Delegate.Remove(statusItemGroup3.OnRemoveStatusItem, new Action<StatusItemGroup.Entry, bool>(this.OnRemoveStatusItem));
				foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry in this.statusItems)
				{
					statusItemEntry.Destroy(true);
				}
				this.statusItems.Clear();
				foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry2 in this.oldStatusItems)
				{
					statusItemEntry2.onDestroy = null;
					statusItemEntry2.Destroy(true);
				}
				this.oldStatusItems.Clear();
			}
		}
	}

	// Token: 0x06006F6D RID: 28525 RVA: 0x0029D290 File Offset: 0x0029B490
	private void OnStorageChange(object data)
	{
		SimpleInfoScreen.RefreshStoragePanel(this.StoragePanel, this.selectedTarget);
	}

	// Token: 0x06006F6E RID: 28526 RVA: 0x0029D2A3 File Offset: 0x0029B4A3
	private void OnBreedingChanceChanged(object data)
	{
		SimpleInfoScreen.RefreshFertilityPanel(this.fertilityPanel, this.selectedTarget);
	}

	// Token: 0x06006F6F RID: 28527 RVA: 0x0029D2B6 File Offset: 0x0029B4B6
	private void OnAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category)
	{
		this.DoAddStatusItem(status_item, category, false);
	}

	// Token: 0x06006F70 RID: 28528 RVA: 0x0029D2C4 File Offset: 0x0029B4C4
	private void DoAddStatusItem(StatusItemGroup.Entry status_item, StatusItemCategory category, bool show_immediate = false)
	{
		GameObject gameObject = this.statusItemsFolder;
		Color color;
		if (status_item.item.notificationType == NotificationType.BadMinor || status_item.item.notificationType == NotificationType.Bad || status_item.item.notificationType == NotificationType.DuplicantThreatening)
		{
			color = GlobalAssets.Instance.colorSet.statusItemBad;
		}
		else if (status_item.item.notificationType == NotificationType.Event)
		{
			color = GlobalAssets.Instance.colorSet.statusItemEvent;
		}
		else if (status_item.item.notificationType == NotificationType.MessageImportant)
		{
			color = GlobalAssets.Instance.colorSet.statusItemMessageImportant;
		}
		else
		{
			color = this.statusItemTextColor_regular;
		}
		TextStyleSetting style = (category == Db.Get().StatusItemCategories.Main) ? this.StatusItemStyle_Main : this.StatusItemStyle_Other;
		SimpleInfoScreen.StatusItemEntry statusItemEntry = new SimpleInfoScreen.StatusItemEntry(status_item, category, this.StatusItemPrefab, gameObject.transform, this.ToolTipStyle_Property, color, style, show_immediate, new Action<SimpleInfoScreen.StatusItemEntry>(this.OnStatusItemDestroy));
		statusItemEntry.SetSprite(status_item.item.sprite);
		if (category != null)
		{
			int num = -1;
			foreach (SimpleInfoScreen.StatusItemEntry statusItemEntry2 in this.oldStatusItems.FindAll((SimpleInfoScreen.StatusItemEntry e) => e.category == category))
			{
				num = statusItemEntry2.GetIndex();
				statusItemEntry2.Destroy(true);
				this.oldStatusItems.Remove(statusItemEntry2);
			}
			if (category == Db.Get().StatusItemCategories.Main)
			{
				num = 0;
			}
			if (num != -1)
			{
				statusItemEntry.SetIndex(num);
			}
		}
		this.statusItems.Add(statusItemEntry);
	}

	// Token: 0x06006F71 RID: 28529 RVA: 0x0029D494 File Offset: 0x0029B694
	private void OnRemoveStatusItem(StatusItemGroup.Entry status_item, bool immediate = false)
	{
		this.DoRemoveStatusItem(status_item, immediate);
	}

	// Token: 0x06006F72 RID: 28530 RVA: 0x0029D4A0 File Offset: 0x0029B6A0
	private void DoRemoveStatusItem(StatusItemGroup.Entry status_item, bool destroy_immediate = false)
	{
		for (int i = 0; i < this.statusItems.Count; i++)
		{
			if (this.statusItems[i].item.item == status_item.item)
			{
				SimpleInfoScreen.StatusItemEntry statusItemEntry = this.statusItems[i];
				this.statusItems.RemoveAt(i);
				this.oldStatusItems.Add(statusItemEntry);
				statusItemEntry.Destroy(destroy_immediate);
				return;
			}
		}
	}

	// Token: 0x06006F73 RID: 28531 RVA: 0x0029D50E File Offset: 0x0029B70E
	private void OnStatusItemDestroy(SimpleInfoScreen.StatusItemEntry item)
	{
		this.oldStatusItems.Remove(item);
	}

	// Token: 0x06006F74 RID: 28532 RVA: 0x0029D51D File Offset: 0x0029B71D
	private void OnRefreshData(object obj)
	{
		this.Refresh(false);
	}

	// Token: 0x06006F75 RID: 28533 RVA: 0x0029D528 File Offset: 0x0029B728
	protected override void Refresh(bool force = false)
	{
		if (this.selectedTarget != this.lastTarget || force)
		{
			this.lastTarget = this.selectedTarget;
		}
		int count = this.statusItems.Count;
		this.statusItemPanel.gameObject.SetActive(count > 0);
		for (int i = 0; i < count; i++)
		{
			this.statusItems[i].Refresh();
		}
		SimpleInfoScreen.RefreshStressPanel(this.stressPanel, this.selectedTarget);
		SimpleInfoScreen.RefreshStoragePanel(this.StoragePanel, this.selectedTarget);
		SimpleInfoScreen.RefreshMovePanel(this.movePanel, this.selectedTarget);
		SimpleInfoScreen.RefreshFertilityPanel(this.fertilityPanel, this.selectedTarget);
		SimpleInfoScreen.RefreshEffectsPanel(this.effectsPanel, this.selectedTarget, this.effectsContent);
		SimpleInfoScreen.RefreshRequirementsPanel(this.requirementsPanel, this.selectedTarget, this.requirementContent);
		SimpleInfoScreen.RefreshInfoPanel(this.infoPanel, this.selectedTarget);
		this.vitalsPanel.Refresh(this.selectedTarget);
		this.rocketSimpleInfoPanel.Refresh(this.rocketStatusContainer, this.selectedTarget);
	}

	// Token: 0x06006F76 RID: 28534 RVA: 0x0029D63F File Offset: 0x0029B83F
	public void Sim1000ms(float dt)
	{
		if (this.selectedTarget != null && this.selectedTarget.GetComponent<IProcessConditionSet>() != null)
		{
			this.RefreshProcessConditionsPanel();
		}
	}

	// Token: 0x06006F77 RID: 28535 RVA: 0x0029D662 File Offset: 0x0029B862
	public void Sim4000ms(float dt)
	{
		this.RefreshWorldPanel();
		this.spaceSimpleInfoPOIPanel.Refresh(this.spacePOIPanel, this.selectedTarget);
	}

	// Token: 0x06006F78 RID: 28536 RVA: 0x0029D684 File Offset: 0x0029B884
	private static void RefreshInfoPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		string text = "";
		string text2 = "";
		UnityEngine.Object component = targetEntity.GetComponent<MinionIdentity>();
		InfoDescription component2 = targetEntity.GetComponent<InfoDescription>();
		BuildingComplete component3 = targetEntity.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component4 = targetEntity.GetComponent<BuildingUnderConstruction>();
		Edible component5 = targetEntity.GetComponent<Edible>();
		PrimaryElement component6 = targetEntity.GetComponent<PrimaryElement>();
		CellSelectionObject component7 = targetEntity.GetComponent<CellSelectionObject>();
		if (!component)
		{
			if (component2)
			{
				text = component2.description;
				text2 = component2.effect;
			}
			else if (component3 != null)
			{
				text = component3.DescEffect + "\n\n" + component3.Desc;
			}
			else if (component4 != null)
			{
				text = component4.DescEffect + "\n\n" + component4.Desc;
			}
			else if (component5 != null)
			{
				EdiblesManager.FoodInfo foodInfo = component5.FoodInfo;
				text += string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true));
			}
			else if (component7 != null)
			{
				text = component7.element.FullDescription(false);
			}
			else if (component6 != null)
			{
				Element element = ElementLoader.FindElementByHash(component6.ElementID);
				text = ((element != null) ? element.FullDescription(false) : "");
			}
			if (!string.IsNullOrEmpty(text))
			{
				targetPanel.SetLabel("Description", text, "");
			}
			bool flag = !string.IsNullOrEmpty(text2) && text2 != "\n";
			string text3 = "\n" + text2;
			if (flag)
			{
				targetPanel.SetLabel("Flavour", text3, "");
			}
			string[] roomClassForObject = CodexEntryGenerator.GetRoomClassForObject(targetEntity);
			if (roomClassForObject != null)
			{
				string text4 = "\n" + CODEX.HEADERS.BUILDINGTYPE + ":";
				foreach (string str in roomClassForObject)
				{
					text4 = text4 + "\n    • " + str;
				}
				targetPanel.SetLabel("RoomClass", text4, "");
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x0029D884 File Offset: 0x0029BA84
	private static void RefreshEffectsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity, DescriptorPanel effectsContent)
	{
		if (targetEntity.GetComponent<MinionIdentity>() != null)
		{
			targetPanel.SetActive(false);
			return;
		}
		targetEntity.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component = targetEntity.GetComponent<BuildingUnderConstruction>();
		List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(component ? component.Def.BuildingComplete : targetEntity, true);
		bool flag = gameObjectEffects.Count > 0;
		effectsContent.gameObject.SetActive(flag);
		if (flag)
		{
			effectsContent.SetDescriptors(gameObjectEffects);
		}
		targetPanel.SetActive(targetEntity != null && flag);
	}

	// Token: 0x06006F7A RID: 28538 RVA: 0x0029D904 File Offset: 0x0029BB04
	private static void RefreshRequirementsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity, DescriptorPanel requirementContent)
	{
		MinionIdentity component = targetEntity.GetComponent<MinionIdentity>();
		UnityEngine.Object component2 = targetEntity.GetComponent<WiltCondition>();
		CreatureBrain component3 = targetEntity.GetComponent<CreatureBrain>();
		if (component2 != null || component != null || component3 != null)
		{
			targetPanel.SetActive(false);
			return;
		}
		targetPanel.SetActive(true);
		BuildingUnderConstruction component4 = targetEntity.GetComponent<BuildingUnderConstruction>();
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(component4 ? component4.Def.BuildingComplete : targetEntity, true), false);
		bool flag = requirementDescriptors.Count > 0;
		requirementContent.gameObject.SetActive(flag);
		if (flag)
		{
			requirementContent.SetDescriptors(requirementDescriptors);
		}
		targetPanel.SetActive(flag);
	}

	// Token: 0x06006F7B RID: 28539 RVA: 0x0029D9A4 File Offset: 0x0029BBA4
	private static void RefreshFertilityPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		FertilityMonitor.Instance smi = targetEntity.GetSMI<FertilityMonitor.Instance>();
		if (smi != null)
		{
			int num = 0;
			foreach (FertilityMonitor.BreedingChance breedingChance in smi.breedingChances)
			{
				List<FertilityModifier> forTag = Db.Get().FertilityModifiers.GetForTag(breedingChance.egg);
				if (forTag.Count > 0)
				{
					string text = "";
					foreach (FertilityModifier fertilityModifier in forTag)
					{
						text += string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_MOD_FORMAT, fertilityModifier.GetTooltip());
					}
					targetPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None), text));
				}
				else
				{
					targetPanel.SetLabel("breeding_" + num++.ToString(), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.EGG_CHANCES.CHANCE_FORMAT_TOOLTIP_NOMOD, breedingChance.egg.ProperName(), GameUtil.GetFormattedPercent(breedingChance.weight * 100f, GameUtil.TimeSlice.None)));
				}
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06006F7C RID: 28540 RVA: 0x0029DB98 File Offset: 0x0029BD98
	private void TriggerRefreshStorage(object data = null)
	{
		SimpleInfoScreen.RefreshStoragePanel(this.StoragePanel, this.selectedTarget);
	}

	// Token: 0x06006F7D RID: 28541 RVA: 0x0029DBAC File Offset: 0x0029BDAC
	private static void RefreshStoragePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			targetPanel.gameObject.SetActive(false);
			targetPanel.Commit();
			return;
		}
		IStorage[] array = targetEntity.GetComponentsInChildren<IStorage>();
		if (array == null)
		{
			targetPanel.gameObject.SetActive(false);
			targetPanel.Commit();
			return;
		}
		array = Array.FindAll<IStorage>(array, (IStorage n) => n.ShouldShowInUI());
		if (array.Length == 0)
		{
			targetPanel.gameObject.SetActive(false);
			targetPanel.Commit();
			return;
		}
		string title = (targetEntity.GetComponent<MinionIdentity>() != null) ? UI.DETAILTABS.DETAILS.GROUPNAME_MINION_CONTENTS : UI.DETAILTABS.DETAILS.GROUPNAME_CONTENTS;
		targetPanel.gameObject.SetActive(true);
		targetPanel.SetTitle(title);
		int num = 0;
		IStorage[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			using (List<GameObject>.Enumerator enumerator = array2[i].GetItems().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject go = enumerator.Current;
					if (!(go == null))
					{
						PrimaryElement component = go.GetComponent<PrimaryElement>();
						if (!(component != null) || component.Mass != 0f)
						{
							Rottable.Instance smi = go.GetSMI<Rottable.Instance>();
							HighEnergyParticleStorage component2 = go.GetComponent<HighEnergyParticleStorage>();
							string text = "";
							string text2 = "";
							if (component != null && component2 == null)
							{
								text = GameUtil.GetUnitFormattedName(go, false);
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedMass(component.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE, text, GameUtil.GetFormattedTemperature(component.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
							}
							if (component2 != null)
							{
								text = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedHighEnergyParticles(component2.Particles, GameUtil.TimeSlice.None, true));
							}
							if (smi != null)
							{
								string text3 = smi.StateString();
								if (!string.IsNullOrEmpty(text3))
								{
									text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, text3);
								}
								text2 += smi.GetToolTip();
							}
							if (component.DiseaseIdx != 255)
							{
								text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_DISEASED, GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, false));
								text2 += GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, true);
							}
							targetPanel.SetLabelWithButton("storage_" + num.ToString(), text, text2, delegate
							{
								SelectTool.Instance.Select(go.GetComponent<KSelectable>(), false);
							});
							num++;
						}
					}
				}
			}
		}
		if (num == 0)
		{
			targetPanel.SetLabel("storage_empty", UI.DETAILTABS.DETAILS.STORAGE_EMPTY, "");
		}
		targetPanel.Commit();
	}

	// Token: 0x06006F7E RID: 28542 RVA: 0x0029DED0 File Offset: 0x0029C0D0
	private void CreateWorldTraitRow()
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		this.worldTraitRows.Add(gameObject);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").gameObject.SetActive(false);
		component.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
	}

	// Token: 0x06006F7F RID: 28543 RVA: 0x0029DF38 File Offset: 0x0029C138
	private static void RefreshMovePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		CancellableMove component = targetEntity.GetComponent<CancellableMove>();
		Movable moving = targetEntity.GetComponent<Movable>();
		if (component != null)
		{
			List<Ref<Movable>> movingObjects = component.movingObjects;
			int num = 0;
			using (List<Ref<Movable>>.Enumerator enumerator = movingObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<Movable> @ref = enumerator.Current;
					Movable movable = @ref.Get();
					GameObject go = (movable != null) ? movable.gameObject : null;
					if (!(go == null))
					{
						PrimaryElement component2 = go.GetComponent<PrimaryElement>();
						if (!(component2 != null) || component2.Mass != 0f)
						{
							Rottable.Instance smi = go.GetSMI<Rottable.Instance>();
							HighEnergyParticleStorage component3 = go.GetComponent<HighEnergyParticleStorage>();
							string text = "";
							string text2 = "";
							if (component2 != null && component3 == null)
							{
								text = GameUtil.GetUnitFormattedName(go, false);
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedMass(component2.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_TEMPERATURE, text, GameUtil.GetFormattedTemperature(component2.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
							}
							if (component3 != null)
							{
								text = ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME;
								text = string.Format(UI.DETAILTABS.DETAILS.CONTENTS_MASS, text, GameUtil.GetFormattedHighEnergyParticles(component3.Particles, GameUtil.TimeSlice.None, true));
							}
							if (smi != null)
							{
								string text3 = smi.StateString();
								if (!string.IsNullOrEmpty(text3))
								{
									text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_ROTTABLE, text3);
								}
								text2 += smi.GetToolTip();
							}
							if (component2.DiseaseIdx != 255)
							{
								text += string.Format(UI.DETAILTABS.DETAILS.CONTENTS_DISEASED, GameUtil.GetFormattedDisease(component2.DiseaseIdx, component2.DiseaseCount, false));
								string formattedDisease = GameUtil.GetFormattedDisease(component2.DiseaseIdx, component2.DiseaseCount, true);
								text2 += formattedDisease;
							}
							targetPanel.SetLabelWithButton("move_" + num.ToString(), text, text2, delegate
							{
								SelectTool.Instance.SelectAndFocus(go.transform.GetPosition(), go.GetComponent<KSelectable>(), new Vector3(5f, 0f, 0f));
							});
							num++;
						}
					}
				}
				goto IL_29A;
			}
		}
		if (moving != null && moving.IsMarkedForMove)
		{
			targetPanel.SetLabelWithButton("moveplacer", MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS, MISC.PLACERS.MOVEPICKUPABLEPLACER.PLACER_STATUS_TOOLTIP, delegate
			{
				SelectTool.Instance.SelectAndFocus(moving.StorageProxy.transform.GetPosition(), moving.StorageProxy.GetComponent<KSelectable>(), new Vector3(5f, 0f, 0f));
			});
		}
		IL_29A:
		targetPanel.Commit();
	}

	// Token: 0x06006F80 RID: 28544 RVA: 0x0029E204 File Offset: 0x0029C404
	private void RefreshWorldPanel()
	{
		WorldContainer worldContainer = (this.selectedTarget == null) ? null : this.selectedTarget.GetComponent<WorldContainer>();
		AsteroidGridEntity x = (this.selectedTarget == null) ? null : this.selectedTarget.GetComponent<AsteroidGridEntity>();
		bool flag = ManagementMenu.Instance.IsScreenOpen(ClusterMapScreen.Instance) && worldContainer != null && x != null;
		this.worldBiomesPanel.gameObject.SetActive(flag);
		this.worldGeysersPanel.gameObject.SetActive(flag);
		this.worldMeteorShowersPanel.gameObject.SetActive(flag);
		this.worldTraitsPanel.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.biomeRows)
		{
			keyValuePair.Value.SetActive(false);
		}
		if (worldContainer.Biomes != null)
		{
			using (List<string>.Enumerator enumerator2 = worldContainer.Biomes.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string text = enumerator2.Current;
					Sprite biomeSprite = GameUtil.GetBiomeSprite(text);
					if (!this.biomeRows.ContainsKey(text))
					{
						this.biomeRows.Add(text, global::Util.KInstantiateUI(this.bigIconLabelRow, this.worldBiomesPanel.Content.gameObject, true));
						HierarchyReferences component = this.biomeRows[text].GetComponent<HierarchyReferences>();
						component.GetReference<Image>("Icon").sprite = biomeSprite;
						component.GetReference<LocText>("NameLabel").SetText(UI.FormatAsLink(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".NAME"), "BIOME" + text.ToUpper()));
						component.GetReference<LocText>("DescriptionLabel").SetText(Strings.Get("STRINGS.SUBWORLDS." + text.ToUpper() + ".DESC"));
					}
					this.biomeRows[text].SetActive(true);
				}
				goto IL_23C;
			}
		}
		this.worldBiomesPanel.gameObject.SetActive(false);
		IL_23C:
		List<Tag> list = new List<Tag>();
		foreach (Geyser cmp in Components.Geysers.GetItems(worldContainer.id))
		{
			list.Add(cmp.PrefabID());
		}
		list.AddRange(SaveGame.Instance.worldGenSpawner.GetUnspawnedWithType<Geyser>(worldContainer.id));
		list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("OilWell", worldContainer.id, true));
		foreach (KeyValuePair<Tag, GameObject> keyValuePair2 in this.geyserRows)
		{
			keyValuePair2.Value.SetActive(false);
		}
		foreach (Tag tag in list)
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			if (!this.geyserRows.ContainsKey(tag))
			{
				this.geyserRows.Add(tag, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
				HierarchyReferences component2 = this.geyserRows[tag].GetComponent<HierarchyReferences>();
				component2.GetReference<Image>("Icon").sprite = uisprite.first;
				component2.GetReference<Image>("Icon").color = uisprite.second;
				component2.GetReference<LocText>("NameLabel").SetText(Assets.GetPrefab(tag).GetProperName());
				component2.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
			}
			this.geyserRows[tag].SetActive(true);
		}
		int count = SaveGame.Instance.worldGenSpawner.GetSpawnersWithTag("GeyserGeneric", worldContainer.id, false).Count;
		if (count > 0)
		{
			global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite("GeyserGeneric", "ui", false);
			Tag key = "GeyserGeneric";
			if (!this.geyserRows.ContainsKey(key))
			{
				this.geyserRows.Add(key, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
			}
			HierarchyReferences component3 = this.geyserRows[key].GetComponent<HierarchyReferences>();
			component3.GetReference<Image>("Icon").sprite = uisprite2.first;
			component3.GetReference<Image>("Icon").color = uisprite2.second;
			component3.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.UNKNOWN_GEYSERS.Replace("{num}", count.ToString()));
			component3.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
			this.geyserRows[key].SetActive(true);
		}
		Tag key2 = "NoGeysers";
		if (!this.geyserRows.ContainsKey(key2))
		{
			this.geyserRows.Add(key2, global::Util.KInstantiateUI(this.iconLabelRow, this.worldGeysersPanel.Content.gameObject, true));
			HierarchyReferences component4 = this.geyserRows[key2].GetComponent<HierarchyReferences>();
			component4.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
			component4.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_GEYSERS);
			component4.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
		}
		this.geyserRows[key2].gameObject.SetActive(list.Count == 0 && count == 0);
		foreach (KeyValuePair<Tag, GameObject> keyValuePair3 in this.meteorShowerRows)
		{
			keyValuePair3.Value.SetActive(false);
		}
		bool flag2 = false;
		foreach (string id in worldContainer.GetSeasonIds())
		{
			GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(id);
			if (gameplaySeason != null)
			{
				foreach (GameplayEvent gameplayEvent in gameplaySeason.events)
				{
					if (gameplayEvent.tags.Contains(GameTags.SpaceDanger) && gameplayEvent is MeteorShowerEvent)
					{
						flag2 = true;
						MeteorShowerEvent meteorShowerEvent = gameplayEvent as MeteorShowerEvent;
						string id2 = meteorShowerEvent.Id;
						global::Tuple<Sprite, Color> uisprite3 = Def.GetUISprite(meteorShowerEvent.GetClusterMapMeteorShowerID(), "ui", false);
						if (!this.meteorShowerRows.ContainsKey(id2))
						{
							this.meteorShowerRows.Add(id2, global::Util.KInstantiateUI(this.iconLabelRow, this.worldMeteorShowersPanel.Content.gameObject, true));
							HierarchyReferences component5 = this.meteorShowerRows[id2].GetComponent<HierarchyReferences>();
							component5.GetReference<Image>("Icon").sprite = uisprite3.first;
							component5.GetReference<Image>("Icon").color = uisprite3.second;
							component5.GetReference<LocText>("NameLabel").SetText(Assets.GetPrefab(meteorShowerEvent.GetClusterMapMeteorShowerID()).GetProperName());
							component5.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
						}
						this.meteorShowerRows[id2].SetActive(true);
					}
				}
			}
		}
		Tag key3 = "NoMeteorShowers";
		if (!this.meteorShowerRows.ContainsKey(key3))
		{
			this.meteorShowerRows.Add(key3, global::Util.KInstantiateUI(this.iconLabelRow, this.worldMeteorShowersPanel.Content.gameObject, true));
			HierarchyReferences component6 = this.meteorShowerRows[key3].GetComponent<HierarchyReferences>();
			component6.GetReference<Image>("Icon").sprite = Assets.GetSprite("icon_action_cancel");
			component6.GetReference<LocText>("NameLabel").SetText(UI.DETAILTABS.SIMPLEINFO.NO_METEORSHOWERS);
			component6.GetReference<LocText>("ValueLabel").gameObject.SetActive(false);
		}
		this.meteorShowerRows[key3].gameObject.SetActive(!flag2);
		List<string> worldTraitIds = worldContainer.WorldTraitIds;
		if (worldTraitIds != null)
		{
			for (int i = 0; i < worldTraitIds.Count; i++)
			{
				if (i > this.worldTraitRows.Count - 1)
				{
					this.CreateWorldTraitRow();
				}
				WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(worldTraitIds[i], false);
				Image reference = this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
				if (cachedWorldTrait != null)
				{
					Sprite sprite = Assets.GetSprite(cachedWorldTrait.filePath.Substring(cachedWorldTrait.filePath.LastIndexOf("/") + 1));
					reference.gameObject.SetActive(true);
					reference.sprite = ((sprite == null) ? Assets.GetSprite("unknown") : sprite);
					reference.color = global::Util.ColorFromHex(cachedWorldTrait.colorHex);
					this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(Strings.Get(cachedWorldTrait.name));
					this.worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip(Strings.Get(cachedWorldTrait.description));
				}
				else
				{
					Sprite sprite2 = Assets.GetSprite("NoTraits");
					reference.gameObject.SetActive(true);
					reference.sprite = sprite2;
					reference.color = Color.white;
					this.worldTraitRows[i].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(WORLD_TRAITS.MISSING_TRAIT);
					this.worldTraitRows[i].AddOrGet<ToolTip>().SetSimpleTooltip("");
				}
			}
			for (int j = 0; j < this.worldTraitRows.Count; j++)
			{
				this.worldTraitRows[j].SetActive(j < worldTraitIds.Count);
			}
			if (worldTraitIds.Count == 0)
			{
				if (this.worldTraitRows.Count < 1)
				{
					this.CreateWorldTraitRow();
				}
				Image reference2 = this.worldTraitRows[0].GetComponent<HierarchyReferences>().GetReference<Image>("Icon");
				Sprite sprite3 = Assets.GetSprite("NoTraits");
				reference2.gameObject.SetActive(true);
				reference2.sprite = sprite3;
				reference2.color = Color.black;
				this.worldTraitRows[0].GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(WORLD_TRAITS.NO_TRAITS.NAME_SHORTHAND);
				this.worldTraitRows[0].AddOrGet<ToolTip>().SetSimpleTooltip(WORLD_TRAITS.NO_TRAITS.DESCRIPTION);
				this.worldTraitRows[0].SetActive(true);
			}
		}
		for (int k = this.surfaceConditionRows.Count - 1; k >= 0; k--)
		{
			global::Util.KDestroyGameObject(this.surfaceConditionRows[k]);
		}
		this.surfaceConditionRows.Clear();
		GameObject gameObject = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		HierarchyReferences component7 = gameObject.GetComponent<HierarchyReferences>();
		component7.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_lights");
		component7.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.LIGHT);
		component7.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedLux(worldContainer.SunlightFixedTraits[worldContainer.sunlightFixedTrait]));
		component7.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		this.surfaceConditionRows.Add(gameObject);
		GameObject gameObject2 = global::Util.KInstantiateUI(this.iconLabelRow, this.worldTraitsPanel.Content.gameObject, true);
		HierarchyReferences component8 = gameObject2.GetComponent<HierarchyReferences>();
		component8.GetReference<Image>("Icon").sprite = Assets.GetSprite("overlay_radiation");
		component8.GetReference<LocText>("NameLabel").SetText(UI.CLUSTERMAP.ASTEROIDS.SURFACE_CONDITIONS.RADIATION);
		component8.GetReference<LocText>("ValueLabel").SetText(GameUtil.GetFormattedRads((float)worldContainer.CosmicRadiationFixedTraits[worldContainer.cosmicRadiationFixedTrait], GameUtil.TimeSlice.None));
		component8.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		this.surfaceConditionRows.Add(gameObject2);
	}

	// Token: 0x06006F81 RID: 28545 RVA: 0x0029EFB8 File Offset: 0x0029D1B8
	private void RefreshProcessConditionsPanel()
	{
		foreach (GameObject original in this.processConditionRows)
		{
			global::Util.KDestroyGameObject(original);
		}
		this.processConditionRows.Clear();
		this.processConditionContainer.SetActive(this.selectedTarget.GetComponent<IProcessConditionSet>() != null);
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			if (this.selectedTarget.GetComponent<LaunchableRocket>() != null)
			{
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketPrep);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketStorage);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketBoard);
				return;
			}
			this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.All);
			return;
		}
		else
		{
			if (this.selectedTarget.GetComponent<LaunchPad>() != null || this.selectedTarget.GetComponent<RocketProcessConditionDisplayTarget>() != null)
			{
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketFlight);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketPrep);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketStorage);
				this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.RocketBoard);
				return;
			}
			this.RefreshProcessConditionsForType(this.selectedTarget, ProcessCondition.ProcessConditionType.All);
			return;
		}
	}

	// Token: 0x06006F82 RID: 28546 RVA: 0x0029F0E4 File Offset: 0x0029D2E4
	private static void RefreshStressPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		MinionIdentity identity = (targetEntity != null) ? targetEntity.GetComponent<MinionIdentity>() : null;
		if (identity != null)
		{
			List<ReportManager.ReportEntry.Note> stressNotes = new List<ReportManager.ReportEntry.Note>();
			targetPanel.gameObject.SetActive(true);
			targetPanel.SetTitle(UI.DETAILTABS.STATS.GROUPNAME_STRESS);
			ReportManager.ReportEntry reportEntry = ReportManager.Instance.TodaysReport.reportEntries.Find((ReportManager.ReportEntry entry) => entry.reportType == ReportManager.ReportType.StressDelta);
			float num = 0f;
			stressNotes.Clear();
			int num2 = reportEntry.contextEntries.FindIndex((ReportManager.ReportEntry entry) => entry.context == identity.GetProperName());
			ReportManager.ReportEntry reportEntry2 = (num2 != -1) ? reportEntry.contextEntries[num2] : null;
			if (reportEntry2 != null)
			{
				reportEntry2.IterateNotes(delegate(ReportManager.ReportEntry.Note note)
				{
					stressNotes.Add(note);
				});
				stressNotes.Sort((ReportManager.ReportEntry.Note a, ReportManager.ReportEntry.Note b) => a.value.CompareTo(b.value));
				for (int i = 0; i < stressNotes.Count; i++)
				{
					string text = float.IsNegativeInfinity(stressNotes[i].value) ? UI.NEG_INFINITY.ToString() : global::Util.FormatTwoDecimalPlace(stressNotes[i].value);
					targetPanel.SetLabel("stressNotes_" + i.ToString(), string.Concat(new string[]
					{
						(stressNotes[i].value > 0f) ? UIConstants.ColorPrefixRed : "",
						stressNotes[i].note,
						": ",
						text,
						"%",
						(stressNotes[i].value > 0f) ? UIConstants.ColorSuffix : ""
					}), "");
					num += stressNotes[i].value;
				}
			}
			string arg = float.IsNegativeInfinity(num) ? UI.NEG_INFINITY.ToString() : global::Util.FormatTwoDecimalPlace(num);
			targetPanel.SetLabel("net_stress", ((num > 0f) ? UIConstants.ColorPrefixRed : "") + string.Format(UI.DETAILTABS.DETAILS.NET_STRESS, arg) + ((num > 0f) ? UIConstants.ColorSuffix : ""), "");
		}
		targetPanel.Commit();
	}

	// Token: 0x06006F83 RID: 28547 RVA: 0x0029F38C File Offset: 0x0029D58C
	private void RefreshProcessConditionsForType(GameObject target, ProcessCondition.ProcessConditionType conditionType)
	{
		IProcessConditionSet component = target.GetComponent<IProcessConditionSet>();
		if (component == null)
		{
			return;
		}
		List<ProcessCondition> conditionSet = component.GetConditionSet(conditionType);
		if (conditionSet.Count == 0)
		{
			return;
		}
		HierarchyReferences hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.processConditionHeader.gameObject, this.processConditionContainer.Content.gameObject, true);
		hierarchyReferences.GetReference<LocText>("Label").text = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." + conditionType.ToString().ToUpper());
		hierarchyReferences.GetComponent<ToolTip>().toolTip = Strings.Get("STRINGS.UI.DETAILTABS.PROCESS_CONDITIONS." + conditionType.ToString().ToUpper() + "_TOOLTIP");
		this.processConditionRows.Add(hierarchyReferences.gameObject);
		List<ProcessCondition> list = new List<ProcessCondition>();
		using (List<ProcessCondition>.Enumerator enumerator = conditionSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ProcessCondition condition = enumerator.Current;
				if (condition.ShowInUI() && (condition.GetType() == typeof(RequireAttachedComponent) || list.Find((ProcessCondition match) => match.GetType() == condition.GetType()) == null))
				{
					list.Add(condition);
					GameObject gameObject = global::Util.KInstantiateUI(this.processConditionRow, this.processConditionContainer.Content.gameObject, true);
					this.processConditionRows.Add(gameObject);
					ConditionListSideScreen.SetRowState(gameObject, condition);
				}
			}
		}
	}

	// Token: 0x04004BFC RID: 19452
	public GameObject iconLabelRow;

	// Token: 0x04004BFD RID: 19453
	public GameObject spacerRow;

	// Token: 0x04004BFE RID: 19454
	[SerializeField]
	private GameObject attributesLabelTemplate;

	// Token: 0x04004BFF RID: 19455
	[SerializeField]
	private GameObject attributesLabelButtonTemplate;

	// Token: 0x04004C00 RID: 19456
	[SerializeField]
	private DescriptorPanel DescriptorContentPrefab;

	// Token: 0x04004C01 RID: 19457
	[SerializeField]
	private GameObject VitalsPanelTemplate;

	// Token: 0x04004C02 RID: 19458
	[SerializeField]
	private GameObject StatusItemPrefab;

	// Token: 0x04004C03 RID: 19459
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x04004C04 RID: 19460
	[SerializeField]
	private HierarchyReferences processConditionHeader;

	// Token: 0x04004C05 RID: 19461
	[SerializeField]
	private GameObject processConditionRow;

	// Token: 0x04004C06 RID: 19462
	[SerializeField]
	private Text StatusPanelCurrentActionLabel;

	// Token: 0x04004C07 RID: 19463
	[SerializeField]
	private GameObject bigIconLabelRow;

	// Token: 0x04004C08 RID: 19464
	[SerializeField]
	private TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04004C09 RID: 19465
	[SerializeField]
	private TextStyleSetting StatusItemStyle_Main;

	// Token: 0x04004C0A RID: 19466
	[SerializeField]
	private TextStyleSetting StatusItemStyle_Other;

	// Token: 0x04004C0B RID: 19467
	[SerializeField]
	private Color statusItemTextColor_regular = Color.black;

	// Token: 0x04004C0C RID: 19468
	[SerializeField]
	private Color statusItemTextColor_old = new Color(0.8235294f, 0.8235294f, 0.8235294f);

	// Token: 0x04004C0E RID: 19470
	private CollapsibleDetailContentPanel statusItemPanel;

	// Token: 0x04004C0F RID: 19471
	private MinionVitalsPanel vitalsPanel;

	// Token: 0x04004C10 RID: 19472
	private CollapsibleDetailContentPanel fertilityPanel;

	// Token: 0x04004C11 RID: 19473
	private CollapsibleDetailContentPanel rocketStatusContainer;

	// Token: 0x04004C12 RID: 19474
	private CollapsibleDetailContentPanel worldLifePanel;

	// Token: 0x04004C13 RID: 19475
	private CollapsibleDetailContentPanel worldElementsPanel;

	// Token: 0x04004C14 RID: 19476
	private CollapsibleDetailContentPanel worldBiomesPanel;

	// Token: 0x04004C15 RID: 19477
	private CollapsibleDetailContentPanel worldGeysersPanel;

	// Token: 0x04004C16 RID: 19478
	private CollapsibleDetailContentPanel worldMeteorShowersPanel;

	// Token: 0x04004C17 RID: 19479
	private CollapsibleDetailContentPanel spacePOIPanel;

	// Token: 0x04004C18 RID: 19480
	private CollapsibleDetailContentPanel worldTraitsPanel;

	// Token: 0x04004C19 RID: 19481
	private CollapsibleDetailContentPanel processConditionContainer;

	// Token: 0x04004C1A RID: 19482
	private CollapsibleDetailContentPanel requirementsPanel;

	// Token: 0x04004C1B RID: 19483
	private CollapsibleDetailContentPanel effectsPanel;

	// Token: 0x04004C1C RID: 19484
	private CollapsibleDetailContentPanel stressPanel;

	// Token: 0x04004C1D RID: 19485
	private CollapsibleDetailContentPanel infoPanel;

	// Token: 0x04004C1E RID: 19486
	private CollapsibleDetailContentPanel movePanel;

	// Token: 0x04004C1F RID: 19487
	private DescriptorPanel effectsContent;

	// Token: 0x04004C20 RID: 19488
	private DescriptorPanel requirementContent;

	// Token: 0x04004C21 RID: 19489
	private RocketSimpleInfoPanel rocketSimpleInfoPanel;

	// Token: 0x04004C22 RID: 19490
	private SpacePOISimpleInfoPanel spaceSimpleInfoPOIPanel;

	// Token: 0x04004C23 RID: 19491
	private DetailsPanelDrawer stressDrawer;

	// Token: 0x04004C24 RID: 19492
	private bool TargetIsMinion;

	// Token: 0x04004C25 RID: 19493
	private GameObject lastTarget;

	// Token: 0x04004C26 RID: 19494
	private GameObject statusItemsFolder;

	// Token: 0x04004C27 RID: 19495
	private Dictionary<Tag, GameObject> lifeformRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04004C28 RID: 19496
	private Dictionary<Tag, GameObject> biomeRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04004C29 RID: 19497
	private Dictionary<Tag, GameObject> geyserRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04004C2A RID: 19498
	private Dictionary<Tag, GameObject> meteorShowerRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04004C2B RID: 19499
	private List<GameObject> worldTraitRows = new List<GameObject>();

	// Token: 0x04004C2C RID: 19500
	private List<GameObject> surfaceConditionRows = new List<GameObject>();

	// Token: 0x04004C2D RID: 19501
	private List<SimpleInfoScreen.StatusItemEntry> statusItems = new List<SimpleInfoScreen.StatusItemEntry>();

	// Token: 0x04004C2E RID: 19502
	private List<SimpleInfoScreen.StatusItemEntry> oldStatusItems = new List<SimpleInfoScreen.StatusItemEntry>();

	// Token: 0x04004C2F RID: 19503
	private List<GameObject> processConditionRows = new List<GameObject>();

	// Token: 0x04004C30 RID: 19504
	private static readonly EventSystem.IntraObjectHandler<SimpleInfoScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<SimpleInfoScreen>(delegate(SimpleInfoScreen component, object data)
	{
		component.OnRefreshData(data);
	});

	// Token: 0x02001ECB RID: 7883
	[DebuggerDisplay("{item.item.Name}")]
	public class StatusItemEntry : IRenderEveryTick
	{
		// Token: 0x17000BDE RID: 3038
		// (get) Token: 0x0600AC8D RID: 44173 RVA: 0x003A76E0 File Offset: 0x003A58E0
		public Image GetImage
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x0600AC8E RID: 44174 RVA: 0x003A76E8 File Offset: 0x003A58E8
		public StatusItemEntry(StatusItemGroup.Entry item, StatusItemCategory category, GameObject status_item_prefab, Transform parent, TextStyleSetting tooltip_style, Color color, TextStyleSetting style, bool skip_fade, Action<SimpleInfoScreen.StatusItemEntry> onDestroy)
		{
			this.item = item;
			this.category = category;
			this.tooltipStyle = tooltip_style;
			this.onDestroy = onDestroy;
			this.color = color;
			this.style = style;
			this.widget = global::Util.KInstantiateUI(status_item_prefab, parent.gameObject, false);
			this.text = this.widget.GetComponentInChildren<LocText>(true);
			SetTextStyleSetting.ApplyStyle(this.text, style);
			this.toolTip = this.widget.GetComponentInChildren<ToolTip>(true);
			this.image = this.widget.GetComponentInChildren<Image>(true);
			item.SetIcon(this.image);
			this.widget.SetActive(true);
			this.toolTip.OnToolTip = new Func<string>(this.OnToolTip);
			this.button = this.widget.GetComponentInChildren<KButton>();
			if (item.item.statusItemClickCallback != null)
			{
				this.button.onClick += this.OnClick;
			}
			else
			{
				this.button.enabled = false;
			}
			this.fadeStage = (skip_fade ? SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT : SimpleInfoScreen.StatusItemEntry.FadeStage.IN);
			SimAndRenderScheduler.instance.Add(this, false);
			this.Refresh();
			this.SetColor(1f);
		}

		// Token: 0x0600AC8F RID: 44175 RVA: 0x003A7829 File Offset: 0x003A5A29
		internal void SetSprite(TintedSprite sprite)
		{
			if (sprite != null)
			{
				this.image.sprite = sprite.sprite;
			}
		}

		// Token: 0x0600AC90 RID: 44176 RVA: 0x003A783F File Offset: 0x003A5A3F
		public int GetIndex()
		{
			return this.widget.transform.GetSiblingIndex();
		}

		// Token: 0x0600AC91 RID: 44177 RVA: 0x003A7851 File Offset: 0x003A5A51
		public void SetIndex(int index)
		{
			this.widget.transform.SetSiblingIndex(index);
		}

		// Token: 0x0600AC92 RID: 44178 RVA: 0x003A7864 File Offset: 0x003A5A64
		public void RenderEveryTick(float dt)
		{
			switch (this.fadeStage)
			{
			case SimpleInfoScreen.StatusItemEntry.FadeStage.IN:
			{
				this.fade = Mathf.Min(this.fade + Time.deltaTime / this.fadeInTime, 1f);
				float num = this.fade;
				this.SetColor(num);
				if (this.fade >= 1f)
				{
					this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT;
					return;
				}
				break;
			}
			case SimpleInfoScreen.StatusItemEntry.FadeStage.WAIT:
				break;
			case SimpleInfoScreen.StatusItemEntry.FadeStage.OUT:
			{
				float num2 = this.fade;
				this.SetColor(num2);
				this.fade = Mathf.Max(this.fade - Time.deltaTime / this.fadeOutTime, 0f);
				if (this.fade <= 0f)
				{
					this.Destroy(true);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600AC93 RID: 44179 RVA: 0x003A7916 File Offset: 0x003A5B16
		private string OnToolTip()
		{
			this.item.ShowToolTip(this.toolTip, this.tooltipStyle);
			return "";
		}

		// Token: 0x0600AC94 RID: 44180 RVA: 0x003A7934 File Offset: 0x003A5B34
		private void OnClick()
		{
			this.item.OnClick();
		}

		// Token: 0x0600AC95 RID: 44181 RVA: 0x003A7944 File Offset: 0x003A5B44
		public void Refresh()
		{
			string name = this.item.GetName();
			if (name != this.text.text)
			{
				this.text.text = name;
				this.SetColor(1f);
			}
		}

		// Token: 0x0600AC96 RID: 44182 RVA: 0x003A7988 File Offset: 0x003A5B88
		private void SetColor(float alpha = 1f)
		{
			Color color = new Color(this.color.r, this.color.g, this.color.b, alpha);
			this.image.color = color;
			this.text.color = color;
		}

		// Token: 0x0600AC97 RID: 44183 RVA: 0x003A79D8 File Offset: 0x003A5BD8
		public void Destroy(bool immediate)
		{
			if (this.toolTip != null)
			{
				this.toolTip.OnToolTip = null;
			}
			if (this.button != null && this.button.enabled)
			{
				this.button.onClick -= this.OnClick;
			}
			if (immediate)
			{
				if (this.onDestroy != null)
				{
					this.onDestroy(this);
				}
				SimAndRenderScheduler.instance.Remove(this);
				UnityEngine.Object.Destroy(this.widget);
				return;
			}
			this.fade = 0.5f;
			this.fadeStage = SimpleInfoScreen.StatusItemEntry.FadeStage.OUT;
		}

		// Token: 0x04008B8A RID: 35722
		public StatusItemGroup.Entry item;

		// Token: 0x04008B8B RID: 35723
		public StatusItemCategory category;

		// Token: 0x04008B8C RID: 35724
		public Color color;

		// Token: 0x04008B8D RID: 35725
		public TextStyleSetting style;

		// Token: 0x04008B8E RID: 35726
		public Action<SimpleInfoScreen.StatusItemEntry> onDestroy;

		// Token: 0x04008B8F RID: 35727
		private LayoutElement spacerLayout;

		// Token: 0x04008B90 RID: 35728
		private GameObject widget;

		// Token: 0x04008B91 RID: 35729
		private ToolTip toolTip;

		// Token: 0x04008B92 RID: 35730
		private TextStyleSetting tooltipStyle;

		// Token: 0x04008B93 RID: 35731
		private Image image;

		// Token: 0x04008B94 RID: 35732
		private LocText text;

		// Token: 0x04008B95 RID: 35733
		private KButton button;

		// Token: 0x04008B96 RID: 35734
		private SimpleInfoScreen.StatusItemEntry.FadeStage fadeStage;

		// Token: 0x04008B97 RID: 35735
		private float fade;

		// Token: 0x04008B98 RID: 35736
		private float fadeInTime;

		// Token: 0x04008B99 RID: 35737
		private float fadeOutTime = 1.8f;

		// Token: 0x02002663 RID: 9827
		private enum FadeStage
		{
			// Token: 0x0400AA7C RID: 43644
			IN,
			// Token: 0x0400AA7D RID: 43645
			WAIT,
			// Token: 0x0400AA7E RID: 43646
			OUT
		}
	}
}
