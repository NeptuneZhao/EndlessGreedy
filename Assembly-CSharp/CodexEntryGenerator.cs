﻿using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using ProcGen;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BFA RID: 3066
public static class CodexEntryGenerator
{
	// Token: 0x06005DD0 RID: 24016 RVA: 0x00227648 File Offset: 0x00225848
	public static Dictionary<string, CodexEntry> GenerateBuildingEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (PlanScreen.PlanInfo category in TUNING.BUILDINGS.PLANORDER)
		{
			CodexEntryGenerator.GenerateEntriesForBuildingsInCategory(category, CodexEntryGenerator.categoryPrefx, ref dictionary);
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			CodexEntryGenerator.GenerateDLC1RocketryEntries();
		}
		CodexEntryGenerator.GenerateBuildingRequirementClassCategoryEntry(CodexEntryGenerator.categoryPrefx, ref dictionary);
		CodexEntryGenerator.PopulateCategoryEntries(dictionary);
		return dictionary;
	}

	// Token: 0x06005DD1 RID: 24017 RVA: 0x002276C4 File Offset: 0x002258C4
	private static void GenerateEntriesForBuildingsInCategory(PlanScreen.PlanInfo category, string categoryPrefx, ref Dictionary<string, CodexEntry> categoryEntries)
	{
		string text = HashCache.Get().Get(category.category);
		string text2 = CodexCache.FormatLinkID(categoryPrefx + text);
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (KeyValuePair<string, string> keyValuePair in category.buildingAndSubcategoryData)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
			if (buildingDef.IsValidDLC())
			{
				CodexEntry codexEntry = CodexEntryGenerator.GenerateSingleBuildingEntry(buildingDef, text2);
				if (buildingDef.ExtendCodexEntry != null)
				{
					codexEntry = buildingDef.ExtendCodexEntry(codexEntry);
				}
				if (codexEntry != null)
				{
					dictionary.Add(codexEntry.id, codexEntry);
				}
			}
		}
		if (dictionary.Count == 0)
		{
			return;
		}
		CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID(text2), Strings.Get("STRINGS.UI.BUILDCATEGORIES." + text.ToUpper() + ".NAME"), dictionary, null, true, true, null);
		categoryEntry.parentId = "BUILDINGS";
		categoryEntry.category = "BUILDINGS";
		categoryEntry.icon = Assets.GetSprite(PlanScreen.IconNameMap[text]);
		categoryEntries.Add(text2, categoryEntry);
	}

	// Token: 0x06005DD2 RID: 24018 RVA: 0x002277FC File Offset: 0x002259FC
	private static void GenerateBuildingRequirementClassCategoryEntry(string categoryPrefix, ref Dictionary<string, CodexEntry> categoryEntries)
	{
		string str = "REQUIREMENTCLASS";
		string text = CodexCache.FormatLinkID(categoryPrefix + str);
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Tag tag in RoomConstraints.ConstraintTags.AllTags)
		{
			if (!CodexEntryGenerator.HiddenRoomConstrainTags.Contains(tag) && (DlcManager.FeatureClusterSpaceEnabled() || !(tag == RoomConstraints.ConstraintTags.RocketInterior)) && (!(tag == RoomConstraints.ConstraintTags.BionicUpkeepType) || SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID")))
			{
				CodexEntry codexEntry = CodexEntryGenerator.GenerateEntryForSpecificBuildingRequirementClass(tag, text);
				dictionary.Add(codexEntry.id, codexEntry);
			}
		}
		CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID(text), CODEX.ROOM_REQUIREMENT_CLASS.NAME, dictionary, null, true, true, null);
		categoryEntry.parentId = "BUILDINGS";
		categoryEntry.category = "BUILDINGS";
		categoryEntry.icon = Assets.GetSprite("icon_categories_placeholder");
		categoryEntries.Add(text, categoryEntry);
	}

	// Token: 0x06005DD3 RID: 24019 RVA: 0x0022790C File Offset: 0x00225B0C
	private static CodexEntry GenerateEntryForSpecificBuildingRequirementClass(Tag requirementClassTag, string categoryEntryID)
	{
		string str = "STRINGS.CODEX.ROOM_REQUIREMENT_CLASS." + requirementClassTag.ToString().ToUpper();
		List<ContentContainer> list = new List<ContentContainer>();
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		ICodexWidget item = new CodexText(Strings.Get(str + ".TITLE"), CodexTextStyle.Title, null);
		list2.Add(item);
		list2.Add(new CodexDividerLine());
		list.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Vertical));
		List<ICodexWidget> list3 = new List<ICodexWidget>();
		ICodexWidget item2 = new CodexText(Strings.Get(str + ".DESCRIPTION"), CodexTextStyle.Body, null);
		list3.Add(item2);
		list3.Add(new CodexSpacer());
		list3.Reverse();
		list.Add(new ContentContainer(list3, ContentContainer.ContentLayout.Vertical));
		List<ICodexWidget> list4 = new List<ICodexWidget>();
		List<ICodexWidget> list5 = new List<ICodexWidget>();
		foreach (PlanScreen.PlanInfo planInfo in TUNING.BUILDINGS.PLANORDER)
		{
			foreach (KeyValuePair<string, string> keyValuePair in planInfo.buildingAndSubcategoryData)
			{
				BuildingDef buildingDef = Assets.GetBuildingDef(keyValuePair.Key);
				if (!buildingDef.DebugOnly && !buildingDef.Deprecated)
				{
					KPrefabID component = buildingDef.BuildingComplete.GetComponent<KPrefabID>();
					if (component != null && component.Tags.Contains(requirementClassTag))
					{
						ICodexWidget item3 = new CodexText("    • " + Strings.Get("STRINGS.BUILDINGS.PREFABS." + buildingDef.PrefabID.ToUpper() + ".NAME"), CodexTextStyle.Body, null);
						list5.Add(item3);
					}
				}
			}
		}
		if (list5.Count > 0)
		{
			ContentContainer contentContainer = new ContentContainer(list5, ContentContainer.ContentLayout.Vertical);
			CodexCollapsibleHeader item4 = new CodexCollapsibleHeader(CODEX.ROOM_REQUIREMENT_CLASS.SHARED.BUILDINGS_LIST_TITLE, contentContainer);
			list4.Add(item4);
			list4.Add(new CodexSpacer());
			list4.Add(new CodexSpacer());
			list4.Reverse();
			list.Add(new ContentContainer(list4, ContentContainer.ContentLayout.Vertical));
			list.Add(contentContainer);
		}
		StringEntry stringEntry;
		if (Strings.TryGet(new StringKey(str + ".ROOMSREQUIRING"), out stringEntry))
		{
			List<ICodexWidget> list6 = new List<ICodexWidget>();
			List<ICodexWidget> list7 = new List<ICodexWidget>();
			string[] array = stringEntry.String.Split('\n', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				ICodexWidget item5 = new CodexText(array[i], CodexTextStyle.Body, null);
				list7.Add(item5);
			}
			new CodexText(stringEntry.String, CodexTextStyle.Body, null);
			ContentContainer contentContainer2 = new ContentContainer(list7, ContentContainer.ContentLayout.Vertical);
			CodexCollapsibleHeader item6 = new CodexCollapsibleHeader(CODEX.ROOM_REQUIREMENT_CLASS.SHARED.ROOMS_REQUIRED_LIST_TITLE, contentContainer2);
			list6.Add(item6);
			list6.Add(new CodexSpacer());
			list6.Add(new CodexSpacer());
			list6.Reverse();
			list.Add(new ContentContainer(list6, ContentContainer.ContentLayout.Vertical));
			list.Add(contentContainer2);
		}
		StringEntry stringEntry2;
		if (Strings.TryGet(new StringKey(str + ".CONFLICTINGROOMS"), out stringEntry2))
		{
			List<ICodexWidget> list8 = new List<ICodexWidget>();
			List<ICodexWidget> list9 = new List<ICodexWidget>();
			string[] array2 = stringEntry2.String.Split('\n', StringSplitOptions.None);
			for (int j = 0; j < array2.Length; j++)
			{
				ICodexWidget item7 = new CodexText(array2[j], CodexTextStyle.Body, null);
				list9.Add(item7);
			}
			ContentContainer contentContainer3 = new ContentContainer(list9, ContentContainer.ContentLayout.Vertical);
			CodexCollapsibleHeader item8 = new CodexCollapsibleHeader(CODEX.ROOM_REQUIREMENT_CLASS.SHARED.ROOMS_CONFLICT_LIST_TITLE, contentContainer3);
			list8.Add(item8);
			list8.Add(new CodexSpacer());
			list8.Add(new CodexSpacer());
			list8.Reverse();
			list.Add(new ContentContainer(list8, ContentContainer.ContentLayout.Vertical));
			list.Add(contentContainer3);
		}
		List<ICodexWidget> list10 = new List<ICodexWidget>();
		ICodexWidget item9 = new CodexText(Strings.Get(str + ".FLAVOUR"), CodexTextStyle.Body, null);
		list10.Add(item9);
		list.Add(new ContentContainer(list10, ContentContainer.ContentLayout.Vertical));
		CodexEntry codexEntry = new CodexEntry(categoryEntryID, list, RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(requirementClassTag));
		Tag tag;
		codexEntry.icon = (CodexEntryGenerator.RoomConstrainTagIcons.TryGetValue(requirementClassTag, out tag) ? Def.GetUISprite(tag, "ui", false).first : null);
		codexEntry.parentId = categoryEntryID;
		Tag tag2 = requirementClassTag;
		CodexCache.AddEntry(CodexCache.FormatLinkID(categoryEntryID + tag2.ToString()), codexEntry, null);
		return codexEntry;
	}

	// Token: 0x06005DD4 RID: 24020 RVA: 0x00227D88 File Offset: 0x00225F88
	private static CodexEntry GenerateSingleBuildingEntry(BuildingDef def, string categoryEntryID)
	{
		if (def.DebugOnly || def.Deprecated)
		{
			return null;
		}
		List<ContentContainer> list = new List<ContentContainer>();
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		list2.Add(new CodexText(def.Name, CodexTextStyle.Title, null));
		Tech tech = Db.Get().Techs.TryGetTechForTechItem(def.PrefabID);
		if (tech != null)
		{
			list2.Add(new CodexLabelWithIcon(tech.Name, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(Assets.GetSprite("research_type_alpha_icon"), Color.white)));
		}
		list2.Add(new CodexDividerLine());
		list.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Vertical));
		CodexEntryGenerator.GenerateImageContainers(def.GetUISprite("ui", false), list);
		CodexEntryGenerator.GenerateBuildingDescriptionContainers(def, list);
		CodexEntryGenerator.GenerateFabricatorContainers(def.BuildingComplete, list);
		CodexEntryGenerator.GenerateReceptacleContainers(def.BuildingComplete, list);
		CodexEntryGenerator.GenerateConfigurableConsumerContainers(def.BuildingComplete, list);
		CodexEntry codexEntry = new CodexEntry(categoryEntryID, list, Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".NAME"));
		codexEntry.icon = def.GetUISprite("ui", false);
		codexEntry.parentId = categoryEntryID;
		CodexCache.AddEntry(def.PrefabID, codexEntry, null);
		return codexEntry;
	}

	// Token: 0x06005DD5 RID: 24021 RVA: 0x00227EB4 File Offset: 0x002260B4
	private static void GenerateDLC1RocketryEntries()
	{
		PlanScreen.PlanInfo planInfo = TUNING.BUILDINGS.PLANORDER.Find((PlanScreen.PlanInfo match) => match.category == new HashedString("Rocketry"));
		foreach (string prefab_id in SelectModuleSideScreen.moduleButtonSortOrder)
		{
			string str = HashCache.Get().Get(planInfo.category);
			string categoryEntryID = CodexCache.FormatLinkID(CodexEntryGenerator.categoryPrefx + str);
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			if (!(buildingDef == null))
			{
				CodexEntry codexEntry = CodexEntryGenerator.GenerateSingleBuildingEntry(buildingDef, categoryEntryID);
				List<ICodexWidget> list = new List<ICodexWidget>();
				list.Add(new CodexSpacer());
				list.Add(new CodexText(UI.CLUSTERMAP.ROCKETS.MODULE_STATS.NAME_HEADER, CodexTextStyle.Subtitle, null));
				list.Add(new CodexSpacer());
				list.Add(new CodexText(UI.CLUSTERMAP.ROCKETS.SPEED.TOOLTIP, CodexTextStyle.Body, null));
				RocketModuleCluster component = buildingDef.BuildingComplete.GetComponent<RocketModuleCluster>();
				float burden = component.performanceStats.Burden;
				float enginePower = component.performanceStats.EnginePower;
				RocketEngineCluster component2 = buildingDef.BuildingComplete.GetComponent<RocketEngineCluster>();
				if (component2 != null)
				{
					list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME_MAX_SUPPORTED + component2.maxHeight.ToString(), CodexTextStyle.Body, null));
				}
				list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.MAX_HEIGHT.NAME_RAW + buildingDef.HeightInCells.ToString(), CodexTextStyle.Body, null));
				if (burden != 0f)
				{
					list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.BURDEN_MODULE.NAME + burden.ToString(), CodexTextStyle.Body, null));
				}
				if (enginePower != 0f)
				{
					list.Add(new CodexText("    • " + UI.CLUSTERMAP.ROCKETS.POWER_MODULE.NAME + enginePower.ToString(), CodexTextStyle.Body, null));
				}
				ContentContainer container = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
				codexEntry.AddContentContainer(container);
			}
		}
	}

	// Token: 0x06005DD6 RID: 24022 RVA: 0x002280D8 File Offset: 0x002262D8
	public static void GeneratePageNotFound()
	{
		CodexCache.AddEntry("PageNotFound", new CodexEntry("ROOT", new List<ContentContainer>
		{
			new ContentContainer
			{
				content = 
				{
					new CodexText(CODEX.PAGENOTFOUND.TITLE, CodexTextStyle.Title, null),
					new CodexText(CODEX.PAGENOTFOUND.SUBTITLE, CodexTextStyle.Subtitle, null),
					new CodexDividerLine(),
					new CodexImage(312, 312, Assets.GetSprite("outhouseMessage"))
				}
			}
		}, CODEX.PAGENOTFOUND.TITLE)
		{
			searchOnly = true
		}, null);
	}

	// Token: 0x06005DD7 RID: 24023 RVA: 0x00228194 File Offset: 0x00226394
	public static Dictionary<string, CodexEntry> GenerateRoomsEntries()
	{
		Dictionary<string, CodexEntry> result = new Dictionary<string, CodexEntry>();
		RoomTypes roomTypesData = Db.Get().RoomTypes;
		string parentCategoryName = "ROOMS";
		Action<RoomTypeCategory> action = delegate(RoomTypeCategory roomCategory)
		{
			bool flag = false;
			List<ContentContainer> contentContainers = new List<ContentContainer>();
			CodexEntry codexEntry = new CodexEntry(parentCategoryName, contentContainers, roomCategory.Name);
			for (int i = 0; i < roomTypesData.Count; i++)
			{
				RoomType roomType = roomTypesData[i];
				if (roomType.category.Id == roomCategory.Id)
				{
					if (!flag)
					{
						flag = true;
						codexEntry.parentId = parentCategoryName;
						codexEntry.name = roomCategory.Name;
						CodexCache.AddEntry(parentCategoryName + roomCategory.Id, codexEntry, null);
						result.Add(parentCategoryName + roomType.category.Id, codexEntry);
						ContentContainer container = new ContentContainer(new List<ICodexWidget>
						{
							new CodexImage(312, 312, Assets.GetSprite(roomCategory.icon))
						}, ContentContainer.ContentLayout.Vertical);
						codexEntry.AddContentContainer(container);
					}
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(roomType.Name, list);
					CodexEntryGenerator.GenerateRoomTypeDescriptionContainers(roomType, list);
					CodexEntryGenerator.GenerateRoomTypeDetailsContainers(roomType, list);
					SubEntry subEntry = new SubEntry(roomType.Id, parentCategoryName + roomType.category.Id, list, roomType.Name);
					subEntry.icon = Assets.GetSprite(roomCategory.icon);
					subEntry.iconColor = Color.white;
					codexEntry.subEntries.Add(subEntry);
				}
			}
		};
		action(Db.Get().RoomTypeCategories.Agricultural);
		action(Db.Get().RoomTypeCategories.Bathroom);
		if (SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID"))
		{
			action(Db.Get().RoomTypeCategories.Bionic);
		}
		action(Db.Get().RoomTypeCategories.Food);
		action(Db.Get().RoomTypeCategories.Hospital);
		action(Db.Get().RoomTypeCategories.Industrial);
		action(Db.Get().RoomTypeCategories.Park);
		action(Db.Get().RoomTypeCategories.Recreation);
		action(Db.Get().RoomTypeCategories.Sleep);
		action(Db.Get().RoomTypeCategories.Science);
		return result;
	}

	// Token: 0x06005DD8 RID: 24024 RVA: 0x002282C4 File Offset: 0x002264C4
	public static Dictionary<string, CodexEntry> GeneratePlantEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Harvestable>();
		prefabsWithComponent.AddRange(Assets.GetPrefabsWithComponent<WiltCondition>());
		foreach (GameObject gameObject in prefabsWithComponent)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (!dictionary.ContainsKey(component.PrefabID().ToString()) && SaveLoader.Instance.IsDlcListActiveForCurrentSave(component.requiredDlcIds) && !(gameObject.GetComponent<BudUprootedMonitor>() != null))
			{
				List<ContentContainer> list = new List<ContentContainer>();
				Sprite first = Def.GetUISprite(gameObject, "ui", false).first;
				CodexEntryGenerator.GenerateImageContainers(first, list);
				CodexEntryGenerator.GeneratePlantDescriptionContainers(gameObject, list);
				CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(gameObject.PrefabID(), list);
				CodexEntry codexEntry = new CodexEntry("PLANTS", list, gameObject.GetProperName());
				codexEntry.parentId = "PLANTS";
				codexEntry.icon = first;
				CodexCache.AddEntry(gameObject.PrefabID().ToString(), codexEntry, null);
				dictionary.Add(gameObject.PrefabID().ToString(), codexEntry);
			}
		}
		return dictionary;
	}

	// Token: 0x06005DD9 RID: 24025 RVA: 0x0022841C File Offset: 0x0022661C
	public static Dictionary<string, CodexEntry> GenerateFoodEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			GameObject prefab = Assets.GetPrefab(foodInfo.Id);
			if (!prefab.HasTag(GameTags.DeprecatedContent) && !prefab.HasTag(GameTags.IncubatableEgg))
			{
				List<ContentContainer> list = new List<ContentContainer>();
				CodexEntryGenerator.GenerateTitleContainers(foodInfo.Name, list);
				Sprite first = Def.GetUISprite(foodInfo.ConsumableId, "ui", false).first;
				CodexEntryGenerator.GenerateImageContainers(first, list);
				CodexEntryGenerator.GenerateFoodDescriptionContainers(foodInfo, list);
				CodexEntryGenerator.GenerateRecipeContainers(foodInfo.ConsumableId.ToTag(), list);
				CodexEntryGenerator_Elements.GenerateMadeAndUsedContainers(foodInfo.ConsumableId.ToTag(), list);
				CodexEntry codexEntry = new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, list, foodInfo.Name);
				codexEntry.icon = first;
				codexEntry.parentId = CodexEntryGenerator.FOOD_CATEGORY_ID;
				CodexCache.AddEntry(foodInfo.Id, codexEntry, null);
				dictionary.Add(foodInfo.Id, codexEntry);
			}
		}
		CodexEntry codexEntry2 = CodexEntryGenerator.GenerateFoodEffectEntry();
		CodexCache.AddEntry(CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID, codexEntry2, null);
		dictionary.Add(CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID, codexEntry2);
		CodexEntry codexEntry3 = CodexEntryGenerator.GenerateTabelSaltEntry();
		CodexCache.AddEntry(CodexEntryGenerator.TABLE_SALT_ENTRY_ID, codexEntry3, null);
		dictionary.Add(CodexEntryGenerator.TABLE_SALT_ENTRY_ID, codexEntry3);
		return dictionary;
	}

	// Token: 0x06005DDA RID: 24026 RVA: 0x00228594 File Offset: 0x00226794
	private static CodexEntry GenerateFoodEffectEntry()
	{
		List<ICodexWidget> content = new List<ICodexWidget>();
		CodexEntry codexEntry = new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, new List<ContentContainer>
		{
			new ContentContainer(content, ContentContainer.ContentLayout.Vertical)
		}, CODEX.HEADERS.FOODEFFECTS);
		codexEntry.parentId = CodexEntryGenerator.FOOD_CATEGORY_ID;
		codexEntry.icon = Assets.GetSprite("icon_category_food");
		Dictionary<string, List<EdiblesManager.FoodInfo>> dictionary = new Dictionary<string, List<EdiblesManager.FoodInfo>>();
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			foreach (string key in foodInfo.Effects)
			{
				List<EdiblesManager.FoodInfo> list;
				if (!dictionary.TryGetValue(key, out list))
				{
					list = new List<EdiblesManager.FoodInfo>();
					dictionary[key] = list;
				}
				list.Add(foodInfo);
			}
		}
		foreach (KeyValuePair<string, List<EdiblesManager.FoodInfo>> keyValuePair in dictionary)
		{
			string text;
			List<EdiblesManager.FoodInfo> list2;
			keyValuePair.Deconstruct(out text, out list2);
			string text2 = text;
			List<EdiblesManager.FoodInfo> list3 = list2;
			Klei.AI.Modifier modifier = Db.Get().effects.Get(text2);
			string id = CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID + "::" + text2.ToUpper();
			string text3 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".NAME");
			string text4 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text2.ToUpper() + ".DESCRIPTION");
			List<ICodexWidget> list4 = new List<ICodexWidget>();
			list4.Add(new CodexText(text3, CodexTextStyle.Title, null));
			SubEntry item = new SubEntry(id, CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID, new List<ContentContainer>
			{
				new ContentContainer(list4, ContentContainer.ContentLayout.Vertical)
			}, text3);
			codexEntry.subEntries.Add(item);
			list4.Add(new CodexText(text4, CodexTextStyle.Body, null));
			foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
			{
				string str = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME");
				string tooltip = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".DESC");
				list4.Add(new CodexTextWithTooltip("    • " + str + ": " + attributeModifier.GetFormattedString(), tooltip, CodexTextStyle.Body));
			}
			list4.Add(new CodexText(CODEX.HEADERS.FOODSWITHEFFECT + ": ", CodexTextStyle.Body, null));
			foreach (EdiblesManager.FoodInfo foodInfo2 in list3)
			{
				list4.Add(new CodexTextWithTooltip("    • " + foodInfo2.Name, foodInfo2.Description, CodexTextStyle.Body));
			}
			list4.Add(new CodexSpacer());
		}
		return codexEntry;
	}

	// Token: 0x06005DDB RID: 24027 RVA: 0x00228924 File Offset: 0x00226B24
	private static CodexEntry GenerateTabelSaltEntry()
	{
		LocString name = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME;
		LocString desc = STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.DESC;
		Sprite sprite = Assets.GetSprite("ui_food_table_salt");
		List<ContentContainer> list = new List<ContentContainer>();
		CodexEntryGenerator.GenerateImageContainers(sprite, list);
		list.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(name, CodexTextStyle.Title, null),
			new CodexText(desc, CodexTextStyle.Body, null)
		}, ContentContainer.ContentLayout.Vertical));
		return new CodexEntry(CodexEntryGenerator.FOOD_CATEGORY_ID, list, name)
		{
			parentId = CodexEntryGenerator.FOOD_CATEGORY_ID,
			icon = sprite
		};
	}

	// Token: 0x06005DDC RID: 24028 RVA: 0x002289B4 File Offset: 0x00226BB4
	public static Dictionary<string, CodexEntry> GenerateMinionModifierEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Effect effect in Db.Get().effects.resources)
		{
			if (effect.triggerFloatingText || !effect.showInUI)
			{
				string id = effect.Id;
				string text = "AVOID_COLLISIONS_" + id;
				StringEntry stringEntry;
				StringEntry stringEntry2;
				if (Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + id.ToUpper() + ".NAME", out stringEntry) && (Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + id.ToUpper() + ".DESCRIPTION", out stringEntry2) || Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + id.ToUpper() + ".TOOLTIP", out stringEntry2)))
				{
					string @string = stringEntry.String;
					string string2 = stringEntry2.String;
					List<ContentContainer> list = new List<ContentContainer>();
					ContentContainer contentContainer = new ContentContainer();
					List<ICodexWidget> content = contentContainer.content;
					content.Add(new CodexText(effect.Name, CodexTextStyle.Title, null));
					content.Add(new CodexText(effect.description, CodexTextStyle.Body, null));
					foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
					{
						string str = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME");
						string tooltip = Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".DESC");
						content.Add(new CodexTextWithTooltip("    • " + str + ": " + attributeModifier.GetFormattedString(), tooltip, CodexTextStyle.Body));
					}
					content.Add(new CodexSpacer());
					list.Add(contentContainer);
					CodexEntry codexEntry = new CodexEntry(CodexEntryGenerator.MINION_MODIFIERS_CATEGORY_ID, list, effect.Name);
					codexEntry.icon = Assets.GetSprite(effect.customIcon);
					codexEntry.parentId = CodexEntryGenerator.MINION_MODIFIERS_CATEGORY_ID;
					CodexCache.AddEntry(text, codexEntry, null);
					dictionary.Add(text, codexEntry);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06005DDD RID: 24029 RVA: 0x00228C1C File Offset: 0x00226E1C
	public static Dictionary<string, CodexEntry> GenerateTechEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Tech tech in Db.Get().Techs.resources)
		{
			List<ContentContainer> list = new List<ContentContainer>();
			CodexEntryGenerator.GenerateTitleContainers(tech.Name, list);
			CodexEntryGenerator.GenerateTechDescriptionContainers(tech, list);
			CodexEntryGenerator.GeneratePrerequisiteTechContainers(tech, list);
			CodexEntryGenerator.GenerateUnlockContainers(tech, list);
			CodexEntry codexEntry = new CodexEntry("TECH", list, tech.Name);
			TechItem techItem = (tech.unlockedItems.Count != 0) ? tech.unlockedItems[0] : null;
			codexEntry.icon = ((techItem == null) ? null : techItem.getUISprite("ui", false));
			codexEntry.parentId = "TECH";
			CodexCache.AddEntry(tech.Id, codexEntry, null);
			dictionary.Add(tech.Id, codexEntry);
		}
		return dictionary;
	}

	// Token: 0x06005DDE RID: 24030 RVA: 0x00228D20 File Offset: 0x00226F20
	public static Dictionary<string, CodexEntry> GenerateRoleEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Skill skill in Db.Get().Skills.resources)
		{
			if (!skill.deprecated && SaveLoader.Instance.IsDLCActiveForCurrentSave(skill.dlcId))
			{
				List<ContentContainer> list = new List<ContentContainer>();
				Sprite sprite = Assets.GetSprite(skill.hat);
				CodexEntryGenerator.GenerateTitleContainers(skill.Name, list);
				CodexEntryGenerator.GenerateImageContainers(sprite, list);
				CodexEntryGenerator.GenerateGenericDescriptionContainers(skill.description, list);
				CodexEntryGenerator.GenerateSkillRequirementsAndPerksContainers(skill, list);
				CodexEntryGenerator.GenerateRelatedSkillContainers(skill, list);
				CodexEntry codexEntry = new CodexEntry("ROLES", list, skill.Name);
				codexEntry.parentId = "ROLES";
				codexEntry.icon = sprite;
				CodexCache.AddEntry(skill.Id, codexEntry, null);
				dictionary.Add(skill.Id, codexEntry);
			}
		}
		return dictionary;
	}

	// Token: 0x06005DDF RID: 24031 RVA: 0x00228E30 File Offset: 0x00227030
	public static Dictionary<string, CodexEntry> GenerateGeyserEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Geyser>();
		if (prefabsWithComponent != null)
		{
			foreach (GameObject gameObject in prefabsWithComponent)
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!component.HasTag(GameTags.DeprecatedContent) && SaveLoader.Instance.IsDlcListActiveForCurrentSave(component.requiredDlcIds))
				{
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(gameObject.GetProperName(), list);
					Sprite first = Def.GetUISprite(gameObject, "ui", false).first;
					CodexEntryGenerator.GenerateImageContainers(first, list);
					List<ICodexWidget> list2 = new List<ICodexWidget>();
					string text = gameObject.PrefabID().ToString().ToUpper();
					string text2 = "GENERICGEYSER_";
					if (text.StartsWith(text2))
					{
						text.Remove(0, text2.Length);
					}
					list2.Add(new CodexText(UI.CODEX.GEYSERS.DESC, CodexTextStyle.Body, null));
					ContentContainer item = new ContentContainer(list2, ContentContainer.ContentLayout.Vertical);
					list.Add(item);
					CodexEntry codexEntry = new CodexEntry("GEYSERS", list, gameObject.GetProperName());
					codexEntry.icon = first;
					codexEntry.parentId = "GEYSERS";
					codexEntry.id = gameObject.PrefabID().ToString();
					CodexCache.AddEntry(codexEntry.id, codexEntry, null);
					dictionary.Add(codexEntry.id, codexEntry);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06005DE0 RID: 24032 RVA: 0x00228FCC File Offset: 0x002271CC
	public static Dictionary<string, CodexEntry> GenerateEquipmentEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Equippable>();
		if (prefabsWithComponent != null)
		{
			foreach (GameObject gameObject in prefabsWithComponent)
			{
				if (SaveLoader.Instance.IsDlcListActiveForCurrentSave(gameObject.GetComponent<KPrefabID>().requiredDlcIds))
				{
					bool flag = false;
					Equippable component = gameObject.GetComponent<Equippable>();
					if (component.def.AdditionalTags != null)
					{
						Tag[] additionalTags = component.def.AdditionalTags;
						for (int i = 0; i < additionalTags.Length; i++)
						{
							if (additionalTags[i] == GameTags.DeprecatedContent)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag && !component.hideInCodex)
					{
						List<ContentContainer> list = new List<ContentContainer>();
						CodexEntryGenerator.GenerateTitleContainers(gameObject.GetProperName(), list);
						Sprite first = Def.GetUISprite(gameObject, "ui", false).first;
						CodexEntryGenerator.GenerateImageContainers(first, list);
						List<ICodexWidget> list2 = new List<ICodexWidget>();
						string text = gameObject.PrefabID().ToString();
						if (component.def.Id == "SleepClinicPajamas")
						{
							list2.Add(new CodexText(Strings.Get("STRINGS.EQUIPMENT.PREFABS." + text.ToUpper() + ".DESC"), CodexTextStyle.Body, null));
							list2.Add(new CodexText(Strings.Get("STRINGS.EQUIPMENT.PREFABS." + text.ToUpper() + ".EFFECT"), CodexTextStyle.Body, null));
						}
						else
						{
							list2.Add(new CodexText(Strings.Get("STRINGS.EQUIPMENT.PREFABS." + text.ToUpper() + ".RECIPE_DESC"), CodexTextStyle.Body, null));
						}
						if (component.def.AttributeModifiers.Count > 0 || component.def.additionalDescriptors.Count > 0)
						{
							list2.Add(new CodexSpacer());
							list2.Add(new CodexText(CODEX.HEADERS.EQUIPMENTEFFECTS, CodexTextStyle.Subtitle, null));
						}
						foreach (AttributeModifier attributeModifier in component.def.AttributeModifiers)
						{
							list2.Add(new CodexTextWithTooltip("    • " + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attributeModifier.GetName(), attributeModifier.GetFormattedString()), Db.Get().Attributes.Get(attributeModifier.AttributeId).Description, CodexTextStyle.Body));
						}
						foreach (Descriptor descriptor in component.def.additionalDescriptors)
						{
							list2.Add(new CodexTextWithTooltip("    • " + descriptor.text, descriptor.tooltipText, CodexTextStyle.Body));
						}
						list.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Vertical));
						CodexEntry codexEntry = new CodexEntry("EQUIPMENT", list, gameObject.GetProperName());
						codexEntry.icon = first;
						codexEntry.parentId = "EQUIPMENT";
						codexEntry.id = gameObject.PrefabID().ToString();
						CodexCache.AddEntry(codexEntry.id, codexEntry, null);
						dictionary.Add(codexEntry.id, codexEntry);
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06005DE1 RID: 24033 RVA: 0x0022937C File Offset: 0x0022757C
	public static Dictionary<string, CodexEntry> GenerateBiomeEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		ListPool<YamlIO.Error, WorldGen>.PooledList pooledList = ListPool<YamlIO.Error, WorldGen>.Allocate();
		Application.streamingAssetsPath + "/worldgen/worlds/";
		Application.streamingAssetsPath + "/worldgen/biomes/";
		Application.streamingAssetsPath + "/worldgen/subworlds/";
		WorldGen.LoadSettings(false);
		Dictionary<string, List<WeightedSubworldName>> dictionary2 = new Dictionary<string, List<WeightedSubworldName>>();
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
		{
			ClusterLayout value = keyValuePair.Value;
			string filePath = value.filePath;
			foreach (WorldPlacement worldPlacement in value.worldPlacements)
			{
				foreach (WeightedSubworldName weightedSubworldName in SettingsCache.worlds.GetWorldData(worldPlacement.world).subworldFiles)
				{
					string text = weightedSubworldName.name.Substring(weightedSubworldName.name.LastIndexOf("/"));
					string text2 = weightedSubworldName.name.Substring(0, weightedSubworldName.name.Length - text.Length);
					text2 = text2.Substring(text2.LastIndexOf("/") + 1);
					if (!(text2 == "subworlds"))
					{
						if (!dictionary2.ContainsKey(text2))
						{
							dictionary2.Add(text2, new List<WeightedSubworldName>
							{
								weightedSubworldName
							});
						}
						else
						{
							dictionary2[text2].Add(weightedSubworldName);
						}
					}
				}
			}
		}
		foreach (KeyValuePair<string, List<WeightedSubworldName>> keyValuePair2 in dictionary2)
		{
			string text3 = CodexCache.FormatLinkID(keyValuePair2.Key);
			global::Tuple<Sprite, Color> tuple = null;
			string text4 = Strings.Get("STRINGS.SUBWORLDS." + text3.ToUpper() + ".NAME");
			if (text4.Contains("MISSING"))
			{
				text4 = text3 + " (missing string key)";
			}
			List<ContentContainer> list = new List<ContentContainer>();
			CodexEntryGenerator.GenerateTitleContainers(text4, list);
			string text5 = "biomeIcon" + char.ToUpper(text3[0]).ToString() + text3.Substring(1).ToLower();
			Sprite sprite = Assets.GetSprite(text5);
			if (sprite != null)
			{
				tuple = new global::Tuple<Sprite, Color>(sprite, Color.white);
			}
			else
			{
				global::Debug.LogWarning("Missing codex biome icon: " + text5);
			}
			string text6 = Strings.Get("STRINGS.SUBWORLDS." + text3.ToUpper() + ".DESC");
			string text7 = Strings.Get("STRINGS.SUBWORLDS." + text3.ToUpper() + ".UTILITY");
			ContentContainer item = new ContentContainer(new List<ICodexWidget>
			{
				new CodexText(string.IsNullOrEmpty(text6) ? "Basic description of the biome." : text6, CodexTextStyle.Body, null),
				new CodexSpacer(),
				new CodexText(string.IsNullOrEmpty(text7) ? "Description of the biomes utility." : text7, CodexTextStyle.Body, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item);
			Dictionary<string, float> dictionary3 = new Dictionary<string, float>();
			ContentContainer item2 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(UI.CODEX.SUBWORLDS.ELEMENTS, CodexTextStyle.Subtitle, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item2);
			ContentContainer contentContainer = new ContentContainer();
			contentContainer.contentLayout = ContentContainer.ContentLayout.Vertical;
			contentContainer.content = new List<ICodexWidget>();
			list.Add(contentContainer);
			foreach (WeightedSubworldName weightedSubworldName2 in keyValuePair2.Value)
			{
				SubWorld subWorld = SettingsCache.subworlds[weightedSubworldName2.name];
				foreach (WeightedBiome weightedBiome in SettingsCache.subworlds[weightedSubworldName2.name].biomes)
				{
					foreach (ElementGradient elementGradient in SettingsCache.biomes.BiomeBackgroundElementBandConfigurations[weightedBiome.name])
					{
						if (dictionary3.ContainsKey(elementGradient.content))
						{
							dictionary3[elementGradient.content] = dictionary3[elementGradient.content] + elementGradient.bandSize;
						}
						else
						{
							if (ElementLoader.FindElementByName(elementGradient.content) == null)
							{
								global::Debug.LogError("Biome " + weightedBiome.name + " contains non-existent element " + elementGradient.content);
							}
							dictionary3.Add(elementGradient.content, elementGradient.bandSize);
						}
					}
				}
				foreach (Feature feature in subWorld.features)
				{
					foreach (KeyValuePair<string, ElementChoiceGroup<WeightedSimHash>> keyValuePair3 in SettingsCache.GetCachedFeature(feature.type).ElementChoiceGroups)
					{
						foreach (WeightedSimHash weightedSimHash in keyValuePair3.Value.choices)
						{
							if (dictionary3.ContainsKey(weightedSimHash.element))
							{
								dictionary3[weightedSimHash.element] = dictionary3[weightedSimHash.element] + 1f;
							}
							else
							{
								dictionary3.Add(weightedSimHash.element, 1f);
							}
						}
					}
				}
			}
			foreach (KeyValuePair<string, float> keyValuePair4 in dictionary3.OrderBy(delegate(KeyValuePair<string, float> pair)
			{
				KeyValuePair<string, float> keyValuePair5 = pair;
				return keyValuePair5.Value;
			}))
			{
				Element element = ElementLoader.FindElementByName(keyValuePair4.Key);
				if (tuple == null)
				{
					tuple = Def.GetUISprite(element.substance, "ui", false);
				}
				contentContainer.content.Add(new CodexIndentedLabelWithIcon(element.name, CodexTextStyle.Body, Def.GetUISprite(element.substance, "ui", false)));
			}
			List<Tag> list2 = new List<Tag>();
			ContentContainer item3 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(UI.CODEX.SUBWORLDS.PLANTS, CodexTextStyle.Subtitle, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item3);
			ContentContainer contentContainer2 = new ContentContainer();
			contentContainer2.contentLayout = ContentContainer.ContentLayout.Vertical;
			contentContainer2.content = new List<ICodexWidget>();
			list.Add(contentContainer2);
			foreach (WeightedSubworldName weightedSubworldName3 in keyValuePair2.Value)
			{
				foreach (WeightedBiome weightedBiome2 in SettingsCache.subworlds[weightedSubworldName3.name].biomes)
				{
					if (weightedBiome2.tags != null)
					{
						foreach (string s in weightedBiome2.tags)
						{
							if (!list2.Contains(s))
							{
								GameObject gameObject = Assets.TryGetPrefab(s);
								if (gameObject != null && (gameObject.GetComponent<Harvestable>() != null || gameObject.GetComponent<SeedProducer>() != null))
								{
									list2.Add(s);
									contentContainer2.content.Add(new CodexIndentedLabelWithIcon(gameObject.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject, "ui", false)));
								}
							}
						}
					}
				}
				foreach (Feature feature2 in SettingsCache.subworlds[weightedSubworldName3.name].features)
				{
					foreach (MobReference mobReference in SettingsCache.GetCachedFeature(feature2.type).internalMobs)
					{
						Tag tag = mobReference.type.ToTag();
						if (!list2.Contains(tag))
						{
							GameObject gameObject2 = Assets.TryGetPrefab(tag);
							if (gameObject2 != null && (gameObject2.GetComponent<Harvestable>() != null || gameObject2.GetComponent<SeedProducer>() != null))
							{
								list2.Add(tag);
								contentContainer2.content.Add(new CodexIndentedLabelWithIcon(gameObject2.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject2, "ui", false)));
							}
						}
					}
				}
			}
			if (list2.Count == 0)
			{
				contentContainer2.content.Add(new CodexIndentedLabelWithIcon(UI.CODEX.SUBWORLDS.NONE, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(Assets.GetSprite("inspectorUI_cannot_build"), Color.red)));
			}
			List<Tag> list3 = new List<Tag>();
			ContentContainer item4 = new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer(),
				new CodexText(UI.CODEX.SUBWORLDS.CRITTERS, CodexTextStyle.Subtitle, null),
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical);
			list.Add(item4);
			ContentContainer contentContainer3 = new ContentContainer();
			contentContainer3.contentLayout = ContentContainer.ContentLayout.Vertical;
			contentContainer3.content = new List<ICodexWidget>();
			list.Add(contentContainer3);
			foreach (WeightedSubworldName weightedSubworldName4 in keyValuePair2.Value)
			{
				foreach (WeightedBiome weightedBiome3 in SettingsCache.subworlds[weightedSubworldName4.name].biomes)
				{
					if (weightedBiome3.tags != null)
					{
						foreach (string s2 in weightedBiome3.tags)
						{
							if (!list3.Contains(s2))
							{
								GameObject gameObject3 = Assets.TryGetPrefab(s2);
								if (gameObject3 != null && gameObject3.HasTag(GameTags.Creature))
								{
									list3.Add(s2);
									contentContainer3.content.Add(new CodexIndentedLabelWithIcon(gameObject3.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject3, "ui", false)));
								}
							}
						}
					}
				}
				foreach (Feature feature3 in SettingsCache.subworlds[weightedSubworldName4.name].features)
				{
					foreach (MobReference mobReference2 in SettingsCache.GetCachedFeature(feature3.type).internalMobs)
					{
						Tag tag2 = mobReference2.type.ToTag();
						if (!list3.Contains(tag2))
						{
							GameObject gameObject4 = Assets.TryGetPrefab(tag2);
							if (gameObject4 != null && gameObject4.HasTag(GameTags.Creature))
							{
								list3.Add(tag2);
								contentContainer3.content.Add(new CodexIndentedLabelWithIcon(gameObject4.GetProperName(), CodexTextStyle.Body, Def.GetUISprite(gameObject4, "ui", false)));
							}
						}
					}
				}
			}
			if (list3.Count == 0)
			{
				contentContainer3.content.Add(new CodexIndentedLabelWithIcon(UI.CODEX.SUBWORLDS.NONE, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(Assets.GetSprite("inspectorUI_cannot_build"), Color.red)));
			}
			string text8 = "BIOME" + text3;
			CodexEntry codexEntry = new CodexEntry("BIOMES", list, text8);
			codexEntry.name = text4;
			codexEntry.parentId = "BIOMES";
			codexEntry.icon = tuple.first;
			codexEntry.iconColor = tuple.second;
			CodexCache.AddEntry(text8, codexEntry, null);
			dictionary.Add(text8, codexEntry);
		}
		if (Application.isPlaying)
		{
			Global.Instance.modManager.HandleErrors(pooledList);
		}
		else
		{
			foreach (YamlIO.Error error in pooledList)
			{
				YamlIO.LogError(error, false);
			}
		}
		pooledList.Recycle();
		return dictionary;
	}

	// Token: 0x06005DE2 RID: 24034 RVA: 0x0022A288 File Offset: 0x00228488
	public static Dictionary<string, CodexEntry> GenerateConstructionMaterialEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		Dictionary<Tag, List<BuildingDef>> dictionary2 = new Dictionary<Tag, List<BuildingDef>>();
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (!buildingDef.Deprecated && !buildingDef.DebugOnly && buildingDef.IsValidDLC() && (buildingDef.ShowInBuildMenu || buildingDef.BuildingComplete.HasTag(GameTags.RocketModule)))
			{
				string[] materialCategory = buildingDef.MaterialCategory;
				for (int i = 0; i < materialCategory.Length; i++)
				{
					foreach (string name in materialCategory[i].Split('&', StringSplitOptions.None))
					{
						Tag key = new Tag(name);
						if (!dictionary2.ContainsKey(key))
						{
							dictionary2.Add(key, new List<BuildingDef>());
						}
						dictionary2[key].Add(buildingDef);
					}
				}
			}
		}
		foreach (Tag tag in dictionary2.Keys)
		{
			if (ElementLoader.GetElement(tag) == null)
			{
				string text = tag.ToString();
				string name2 = Strings.Get("STRINGS.MISC.TAGS." + text.ToUpper());
				List<ContentContainer> list = new List<ContentContainer>();
				CodexEntryGenerator.GenerateTitleContainers(name2, list);
				list.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexText(Strings.Get("STRINGS.MISC.TAGS." + text.ToUpper() + "_DESC"), CodexTextStyle.Body, null),
					new CodexSpacer()
				}, ContentContainer.ContentLayout.Vertical));
				List<ICodexWidget> list2 = new List<ICodexWidget>();
				List<Tag> validMaterials = MaterialSelector.GetValidMaterials(tag, true);
				foreach (Tag tag2 in validMaterials)
				{
					list2.Add(new CodexIndentedLabelWithIcon(tag2.ProperName(), CodexTextStyle.Body, Def.GetUISprite(tag2, "ui", false)));
				}
				list.Add(new ContentContainer(list2, ContentContainer.ContentLayout.GridTwoColumn));
				list.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexSpacer(),
					new CodexText(CODEX.HEADERS.MATERIALUSEDTOCONSTRUCT, CodexTextStyle.Title, null),
					new CodexDividerLine()
				}, ContentContainer.ContentLayout.Vertical));
				List<ICodexWidget> list3 = new List<ICodexWidget>();
				foreach (BuildingDef buildingDef2 in dictionary2[tag])
				{
					list3.Add(new CodexIndentedLabelWithIcon(buildingDef2.Name, CodexTextStyle.Body, Def.GetUISprite(buildingDef2.Tag, "ui", false)));
				}
				list.Add(new ContentContainer(list3, ContentContainer.ContentLayout.GridTwoColumn));
				CodexEntry codexEntry = new CodexEntry("BUILDINGMATERIALCLASSES", list, name2);
				codexEntry.parentId = codexEntry.category;
				CodexEntry codexEntry2 = codexEntry;
				Sprite icon;
				if ((icon = Assets.GetSprite("ui_" + tag.Name.ToLower())) == null)
				{
					icon = (((validMaterials.Count != 0) ? Def.GetUISprite(validMaterials[0], "ui", false).first : null) ?? Assets.GetSprite("ui_elements_classes"));
				}
				codexEntry2.icon = icon;
				if (tag == GameTags.BuildableAny)
				{
					codexEntry.icon = Assets.GetSprite("ui_elements_classes");
				}
				CodexCache.AddEntry(CodexCache.FormatLinkID(text), codexEntry, null);
				dictionary.Add(text, codexEntry);
			}
		}
		return dictionary;
	}

	// Token: 0x06005DE3 RID: 24035 RVA: 0x0022A6A4 File Offset: 0x002288A4
	public static Dictionary<string, CodexEntry> GenerateDiseaseEntries()
	{
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			if (!disease.Disabled)
			{
				List<ContentContainer> list = new List<ContentContainer>();
				CodexEntryGenerator.GenerateTitleContainers(disease.Name, list);
				CodexEntryGenerator.GenerateDiseaseDescriptionContainers(disease, list);
				CodexEntry codexEntry = new CodexEntry("DISEASE", list, disease.Name);
				codexEntry.parentId = "DISEASE";
				dictionary.Add(disease.Id, codexEntry);
				codexEntry.icon = Assets.GetSprite("overlay_disease");
				CodexCache.AddEntry(disease.Id, codexEntry, null);
			}
		}
		return dictionary;
	}

	// Token: 0x06005DE4 RID: 24036 RVA: 0x0022A778 File Offset: 0x00228978
	public static CategoryEntry GenerateCategoryEntry(string id, string name, Dictionary<string, CodexEntry> entries, Sprite icon = null, bool largeFormat = true, bool sort = true, string overrideHeader = null)
	{
		List<ContentContainer> list = new List<ContentContainer>();
		CodexEntryGenerator.GenerateTitleContainers((overrideHeader == null) ? name : overrideHeader, list);
		List<CodexEntry> list2 = new List<CodexEntry>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in entries)
		{
			list2.Add(keyValuePair.Value);
			if (icon == null)
			{
				icon = keyValuePair.Value.icon;
			}
		}
		CategoryEntry categoryEntry = new CategoryEntry("Root", list, name, list2, largeFormat, sort);
		categoryEntry.icon = icon;
		CodexCache.AddEntry(id, categoryEntry, null);
		return categoryEntry;
	}

	// Token: 0x06005DE5 RID: 24037 RVA: 0x0022A824 File Offset: 0x00228A24
	public static Dictionary<string, CodexEntry> GenerateTutorialNotificationEntries()
	{
		CodexEntry codexEntry = new CodexEntry("MISCELLANEOUSTIPS", new List<ContentContainer>
		{
			new ContentContainer(new List<ICodexWidget>
			{
				new CodexSpacer()
			}, ContentContainer.ContentLayout.Vertical)
		}, Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES.MISCELLANEOUSTIPS"));
		Dictionary<string, CodexEntry> dictionary = new Dictionary<string, CodexEntry>();
		for (int i = 0; i < 24; i++)
		{
			TutorialMessage tutorialMessage = (TutorialMessage)Tutorial.Instance.TutorialMessage((Tutorial.TutorialMessages)i, false);
			if (tutorialMessage != null && DlcManager.IsDlcListValidForCurrentContent(tutorialMessage.DLCIDs))
			{
				if (!string.IsNullOrEmpty(tutorialMessage.videoClipId))
				{
					List<ContentContainer> list = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(tutorialMessage.GetTitle(), list);
					CodexVideo codexVideo = new CodexVideo();
					codexVideo.videoName = tutorialMessage.videoClipId;
					codexVideo.overlayName = tutorialMessage.videoOverlayName;
					codexVideo.overlayTexts = new List<string>
					{
						tutorialMessage.videoTitleText,
						VIDEOS.TUTORIAL_HEADER
					};
					list.Add(new ContentContainer(new List<ICodexWidget>
					{
						codexVideo
					}, ContentContainer.ContentLayout.Vertical));
					list.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexText(tutorialMessage.GetMessageBody(), CodexTextStyle.Body, tutorialMessage.GetTitle())
					}, ContentContainer.ContentLayout.Vertical));
					CodexEntry codexEntry2 = new CodexEntry("Videos", list, UI.FormatAsLink(tutorialMessage.GetTitle(), "videos_" + i.ToString()));
					codexEntry2.icon = Assets.GetSprite("codexVideo");
					CodexCache.AddEntry("videos_" + i.ToString(), codexEntry2, null);
					dictionary.Add(codexEntry2.id, codexEntry2);
				}
				else
				{
					List<ContentContainer> list2 = new List<ContentContainer>();
					CodexEntryGenerator.GenerateTitleContainers(tutorialMessage.GetTitle(), list2);
					list2.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexText(tutorialMessage.GetMessageBody(), CodexTextStyle.Body, tutorialMessage.GetTitle())
					}, ContentContainer.ContentLayout.Vertical));
					list2.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexSpacer(),
						new CodexSpacer()
					}, ContentContainer.ContentLayout.Vertical));
					SubEntry item = new SubEntry("MISCELLANEOUSTIPS" + i.ToString(), "MISCELLANEOUSTIPS", list2, tutorialMessage.GetTitle());
					codexEntry.subEntries.Add(item);
				}
			}
		}
		CodexCache.AddEntry("MISCELLANEOUSTIPS", codexEntry, null);
		return dictionary;
	}

	// Token: 0x06005DE6 RID: 24038 RVA: 0x0022AA84 File Offset: 0x00228C84
	public static void PopulateCategoryEntries(Dictionary<string, CodexEntry> categoryEntries)
	{
		List<CategoryEntry> list = new List<CategoryEntry>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in categoryEntries)
		{
			list.Add(keyValuePair.Value as CategoryEntry);
		}
		CodexEntryGenerator.PopulateCategoryEntries(list, null);
	}

	// Token: 0x06005DE7 RID: 24039 RVA: 0x0022AAEC File Offset: 0x00228CEC
	public static void PopulateCategoryEntries(List<CategoryEntry> categoryEntries, Comparison<CodexEntry> comparison = null)
	{
		foreach (CategoryEntry categoryEntry in categoryEntries)
		{
			List<ContentContainer> contentContainers = categoryEntry.contentContainers;
			List<CodexEntry> list = new List<CodexEntry>();
			foreach (CodexEntry item in categoryEntry.entriesInCategory)
			{
				list.Add(item);
			}
			if (categoryEntry.sort)
			{
				if (comparison == null)
				{
					list.Sort((CodexEntry a, CodexEntry b) => UI.StripLinkFormatting(a.name).CompareTo(UI.StripLinkFormatting(b.name)));
				}
				else
				{
					list.Sort(comparison);
				}
			}
			if (categoryEntry.largeFormat)
			{
				ContentContainer contentContainer = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Grid);
				foreach (CodexEntry codexEntry in list)
				{
					contentContainer.content.Add(new CodexLabelWithLargeIcon(codexEntry.name, CodexTextStyle.BodyWhite, new global::Tuple<Sprite, Color>((codexEntry.icon != null) ? codexEntry.icon : Assets.GetSprite("unknown"), codexEntry.iconColor), codexEntry.id));
				}
				if (categoryEntry.showBeforeGeneratedCategoryLinks)
				{
					contentContainers.Add(contentContainer);
				}
				else
				{
					ContentContainer item2 = contentContainers[contentContainers.Count - 1];
					contentContainers.RemoveAt(contentContainers.Count - 1);
					contentContainers.Insert(0, item2);
					contentContainers.Insert(1, contentContainer);
					contentContainers.Insert(2, new ContentContainer(new List<ICodexWidget>
					{
						new CodexSpacer()
					}, ContentContainer.ContentLayout.Vertical));
				}
			}
			else
			{
				ContentContainer contentContainer2 = new ContentContainer(new List<ICodexWidget>(), ContentContainer.ContentLayout.Vertical);
				foreach (CodexEntry codexEntry2 in list)
				{
					if (codexEntry2.icon == null)
					{
						contentContainer2.content.Add(new CodexText(codexEntry2.name, CodexTextStyle.Body, null));
					}
					else
					{
						contentContainer2.content.Add(new CodexLabelWithIcon(codexEntry2.name, CodexTextStyle.Body, new global::Tuple<Sprite, Color>(codexEntry2.icon, codexEntry2.iconColor), 64, 48));
					}
				}
				if (categoryEntry.showBeforeGeneratedCategoryLinks)
				{
					contentContainers.Add(contentContainer2);
				}
				else
				{
					ContentContainer item3 = contentContainers[contentContainers.Count - 1];
					contentContainers.RemoveAt(contentContainers.Count - 1);
					contentContainers.Insert(0, item3);
					contentContainers.Insert(1, contentContainer2);
				}
			}
		}
	}

	// Token: 0x06005DE8 RID: 24040 RVA: 0x0022ADE0 File Offset: 0x00228FE0
	public static void GenerateTitleContainers(string name, List<ContentContainer> containers)
	{
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(name, CodexTextStyle.Title, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DE9 RID: 24041 RVA: 0x0022AE1C File Offset: 0x0022901C
	private static void GeneratePrerequisiteTechContainers(Tech tech, List<ContentContainer> containers)
	{
		if (tech.requiredTech == null || tech.requiredTech.Count == 0)
		{
			return;
		}
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexText(CODEX.HEADERS.PREREQUISITE_TECH, CodexTextStyle.Subtitle, null));
		list.Add(new CodexDividerLine());
		list.Add(new CodexSpacer());
		foreach (Tech tech2 in tech.requiredTech)
		{
			list.Add(new CodexText(tech2.Name, CodexTextStyle.Body, null));
		}
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DEA RID: 24042 RVA: 0x0022AEDC File Offset: 0x002290DC
	private static void GenerateSkillRequirementsAndPerksContainers(Skill skill, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(CODEX.HEADERS.ROLE_PERKS, CodexTextStyle.Subtitle, null);
		CodexText item2 = new CodexText(CODEX.HEADERS.ROLE_PERKS_DESC, CodexTextStyle.Body, null);
		list.Add(item);
		list.Add(new CodexDividerLine());
		list.Add(item2);
		list.Add(new CodexSpacer());
		foreach (SkillPerk skillPerk in skill.perks)
		{
			if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds))
			{
				CodexText item3 = new CodexText(skillPerk.Name, CodexTextStyle.Body, null);
				list.Add(item3);
			}
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
		list.Add(new CodexSpacer());
	}

	// Token: 0x06005DEB RID: 24043 RVA: 0x0022AFBC File Offset: 0x002291BC
	private static void GenerateRelatedSkillContainers(Skill skill, List<ContentContainer> containers)
	{
		bool flag = false;
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(CODEX.HEADERS.PREREQUISITE_ROLES, CodexTextStyle.Subtitle, null);
		list.Add(item);
		list.Add(new CodexDividerLine());
		list.Add(new CodexSpacer());
		foreach (string id in skill.priorSkills)
		{
			CodexText item2 = new CodexText(Db.Get().Skills.Get(id).Name, CodexTextStyle.Body, null);
			list.Add(item2);
			flag = true;
		}
		if (flag)
		{
			list.Add(new CodexSpacer());
			containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
		}
		bool flag2 = false;
		List<ICodexWidget> list2 = new List<ICodexWidget>();
		CodexText item3 = new CodexText(CODEX.HEADERS.UNLOCK_ROLES, CodexTextStyle.Subtitle, null);
		CodexText item4 = new CodexText(CODEX.HEADERS.UNLOCK_ROLES_DESC, CodexTextStyle.Body, null);
		list2.Add(item3);
		list2.Add(new CodexDividerLine());
		list2.Add(item4);
		list2.Add(new CodexSpacer());
		foreach (Skill skill2 in Db.Get().Skills.resources)
		{
			if (!skill2.deprecated)
			{
				using (List<string>.Enumerator enumerator = skill2.priorSkills.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == skill.Id)
						{
							CodexText item5 = new CodexText(skill2.Name, CodexTextStyle.Body, null);
							list2.Add(item5);
							flag2 = true;
						}
					}
				}
			}
		}
		if (flag2)
		{
			list2.Add(new CodexSpacer());
			containers.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Vertical));
		}
	}

	// Token: 0x06005DEC RID: 24044 RVA: 0x0022B1B0 File Offset: 0x002293B0
	private static void GenerateUnlockContainers(Tech tech, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(CODEX.HEADERS.TECH_UNLOCKS, CodexTextStyle.Subtitle, null);
		list.Add(item);
		list.Add(new CodexDividerLine());
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
		foreach (TechItem techItem in tech.unlockedItems)
		{
			List<ICodexWidget> list2 = new List<ICodexWidget>();
			CodexImage item2 = new CodexImage(64, 64, techItem.getUISprite("ui", false));
			list2.Add(item2);
			CodexText item3 = new CodexText(techItem.Name, CodexTextStyle.Body, null);
			list2.Add(item3);
			containers.Add(new ContentContainer(list2, ContentContainer.ContentLayout.Horizontal));
		}
	}

	// Token: 0x06005DED RID: 24045 RVA: 0x0022B290 File Offset: 0x00229490
	private static void GenerateRecipeContainers(Tag prefabID, List<ContentContainer> containers)
	{
		Recipe recipe = null;
		foreach (Recipe recipe2 in RecipeManager.Get().recipes)
		{
			if (recipe2.Result == prefabID)
			{
				recipe = recipe2;
				break;
			}
		}
		if (recipe == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(CODEX.HEADERS.RECIPE, CodexTextStyle.Subtitle, null),
			new CodexSpacer(),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		Func<Recipe, List<ContentContainer>> func = delegate(Recipe rec)
		{
			List<ContentContainer> list = new List<ContentContainer>();
			foreach (Recipe.Ingredient ingredient in rec.Ingredients)
			{
				GameObject prefab = Assets.GetPrefab(ingredient.tag);
				if (prefab != null)
				{
					list.Add(new ContentContainer(new List<ICodexWidget>
					{
						new CodexImage(64, 64, Def.GetUISprite(prefab, "ui", false)),
						new CodexText(string.Format(UI.CODEX.RECIPE_ITEM, Assets.GetPrefab(ingredient.tag).GetProperName(), ingredient.amount, (ElementLoader.GetElement(ingredient.tag) == null) ? "" : UI.UNITSUFFIXES.MASS.KILOGRAM.text), CodexTextStyle.Body, null)
					}, ContentContainer.ContentLayout.Horizontal));
				}
			}
			return list;
		};
		containers.AddRange(func(recipe));
		GameObject gameObject = (recipe.fabricators == null) ? null : Assets.GetPrefab(recipe.fabricators[0]);
		if (gameObject != null)
		{
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexText(UI.CODEX.RECIPE_FABRICATOR_HEADER, CodexTextStyle.Subtitle, null),
				new CodexDividerLine()
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexImage(64, 64, Def.GetUISpriteFromMultiObjectAnim(gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0], "ui", false, "")),
				new CodexText(string.Format(UI.CODEX.RECIPE_FABRICATOR, recipe.FabricationTime, gameObject.GetProperName()), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Horizontal));
		}
	}

	// Token: 0x06005DEE RID: 24046 RVA: 0x0022B430 File Offset: 0x00229630
	private static void GenerateRoomTypeDetailsContainers(RoomType roomType, List<ContentContainer> containers)
	{
		ICodexWidget item = new CodexText(UI.CODEX.DETAILS, CodexTextStyle.Subtitle, null);
		ICodexWidget item2 = new CodexDividerLine();
		ContentContainer item3 = new ContentContainer(new List<ICodexWidget>
		{
			item,
			item2
		}, ContentContainer.ContentLayout.Vertical);
		containers.Add(item3);
		List<ICodexWidget> list = new List<ICodexWidget>();
		if (!string.IsNullOrEmpty(roomType.effect))
		{
			string roomEffectsString = roomType.GetRoomEffectsString();
			list.Add(new CodexText(roomEffectsString, CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		if (roomType.primary_constraint != null || roomType.additional_constraints != null)
		{
			list.Add(new CodexText(ROOMS.CRITERIA.HEADER, CodexTextStyle.Body, null));
			string text = "";
			if (roomType.primary_constraint != null)
			{
				text = text + "    • " + roomType.primary_constraint.name;
			}
			if (roomType.additional_constraints != null)
			{
				for (int i = 0; i < roomType.additional_constraints.Length; i++)
				{
					text = text + "\n    • " + roomType.additional_constraints[i].name;
				}
			}
			list.Add(new CodexText(text, CodexTextStyle.Body, null));
		}
		ContentContainer item4 = new ContentContainer(list, ContentContainer.ContentLayout.Vertical);
		containers.Add(item4);
	}

	// Token: 0x06005DEF RID: 24047 RVA: 0x0022B560 File Offset: 0x00229760
	private static void GenerateRoomTypeDescriptionContainers(RoomType roomType, List<ContentContainer> containers)
	{
		ContentContainer item = new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(roomType.description, CodexTextStyle.Body, null),
			new CodexSpacer()
		}, ContentContainer.ContentLayout.Vertical);
		containers.Add(item);
	}

	// Token: 0x06005DF0 RID: 24048 RVA: 0x0022B5A0 File Offset: 0x002297A0
	private static void GeneratePlantDescriptionContainers(GameObject plant, List<ContentContainer> containers)
	{
		SeedProducer component = plant.GetComponent<SeedProducer>();
		if (component != null)
		{
			GameObject prefab = Assets.GetPrefab(component.seedInfo.seedId);
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexText(CODEX.HEADERS.GROWNFROMSEED, CodexTextStyle.Subtitle, null),
				new CodexDividerLine()
			}, ContentContainer.ContentLayout.Vertical));
			containers.Add(new ContentContainer(new List<ICodexWidget>
			{
				new CodexImage(48, 48, Def.GetUISprite(prefab, "ui", false)),
				new CodexText(prefab.GetProperName(), CodexTextStyle.Body, null)
			}, ContentContainer.ContentLayout.Horizontal));
		}
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexSpacer());
		list.Add(new CodexText(UI.CODEX.DETAILS, CodexTextStyle.Subtitle, null));
		list.Add(new CodexDividerLine());
		InfoDescription component2 = Assets.GetPrefab(plant.PrefabID()).GetComponent<InfoDescription>();
		if (component2 != null)
		{
			list.Add(new CodexText(component2.description, CodexTextStyle.Body, null));
		}
		string text = "";
		List<Descriptor> plantRequirementDescriptors = GameUtil.GetPlantRequirementDescriptors(plant);
		if (plantRequirementDescriptors.Count > 0)
		{
			text += plantRequirementDescriptors[0].text;
			for (int i = 1; i < plantRequirementDescriptors.Count; i++)
			{
				text = text + "\n    • " + plantRequirementDescriptors[i].text;
			}
			list.Add(new CodexText(text, CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		text = "";
		List<Descriptor> plantEffectDescriptors = GameUtil.GetPlantEffectDescriptors(plant);
		if (plantEffectDescriptors.Count > 0)
		{
			text += plantEffectDescriptors[0].text;
			for (int j = 1; j < plantEffectDescriptors.Count; j++)
			{
				text = text + "\n    • " + plantEffectDescriptors[j].text;
			}
			CodexText item = new CodexText(text, CodexTextStyle.Body, null);
			list.Add(item);
			list.Add(new CodexSpacer());
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DF1 RID: 24049 RVA: 0x0022B7A6 File Offset: 0x002299A6
	private static ICodexWidget GetIconWidget(object entity)
	{
		return new CodexImage(32, 32, Def.GetUISprite(entity, "ui", false));
	}

	// Token: 0x06005DF2 RID: 24050 RVA: 0x0022B7C0 File Offset: 0x002299C0
	private static void GenerateDiseaseDescriptionContainers(Disease disease, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexSpacer());
		StringEntry stringEntry = null;
		if (Strings.TryGet("STRINGS.DUPLICANTS.DISEASES." + disease.Id.ToUpper() + ".DESC", out stringEntry))
		{
			list.Add(new CodexText(stringEntry.String, CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		foreach (Descriptor descriptor in disease.GetQuantitativeDescriptors())
		{
			list.Add(new CodexText(descriptor.text, CodexTextStyle.Body, null));
		}
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DF3 RID: 24051 RVA: 0x0022B88C File Offset: 0x00229A8C
	private static void GenerateFoodDescriptionContainers(EdiblesManager.FoodInfo food, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>
		{
			new CodexText(food.Description, CodexTextStyle.Body, null),
			new CodexSpacer(),
			new CodexText(string.Format(UI.CODEX.FOOD.QUALITY, GameUtil.GetFormattedFoodQuality(food.Quality)), CodexTextStyle.Body, null),
			new CodexText(string.Format(UI.CODEX.FOOD.CALORIES, GameUtil.GetFormattedCalories(food.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), CodexTextStyle.Body, null),
			new CodexSpacer(),
			new CodexText(food.CanRot ? string.Format(UI.CODEX.FOOD.SPOILPROPERTIES, GameUtil.GetFormattedTemperature(food.RotTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(food.PreserveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedCycles(food.SpoilTime, "F1", false)) : UI.CODEX.FOOD.NON_PERISHABLE.ToString(), CodexTextStyle.Body, null),
			new CodexSpacer()
		};
		if (food.Effects.Count > 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.FOODEFFECTS + ":", CodexTextStyle.Body, null));
			foreach (string text in food.Effects)
			{
				Klei.AI.Modifier modifier = Db.Get().effects.Get(text);
				string text2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string text3 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".DESCRIPTION");
				string text4 = "";
				foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
				{
					text4 = string.Concat(new string[]
					{
						text4,
						"\n    • ",
						Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME"),
						": ",
						attributeModifier.GetFormattedString()
					});
				}
				text3 += text4;
				text2 = UI.FormatAsLink(text2, CodexEntryGenerator.FOOD_EFFECTS_ENTRY_ID + "::" + text.ToUpper());
				list.Add(new CodexTextWithTooltip("    • " + text2, text3, CodexTextStyle.Body));
			}
			list.Add(new CodexSpacer());
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DF4 RID: 24052 RVA: 0x0022BB54 File Offset: 0x00229D54
	private static void GenerateTechDescriptionContainers(Tech tech, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(Strings.Get("STRINGS.RESEARCH.TECHS." + tech.Id.ToUpper() + ".DESC"), CodexTextStyle.Body, null);
		list.Add(item);
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DF5 RID: 24053 RVA: 0x0022BBB4 File Offset: 0x00229DB4
	private static void GenerateGenericDescriptionContainers(string description, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexText item = new CodexText(description, CodexTextStyle.Body, null);
		list.Add(item);
		list.Add(new CodexSpacer());
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DF6 RID: 24054 RVA: 0x0022BBF0 File Offset: 0x00229DF0
	private static void GenerateBuildingDescriptionContainers(BuildingDef def, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		list.Add(new CodexText(Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".EFFECT"), CodexTextStyle.Body, null));
		list.Add(new CodexSpacer());
		List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def.BuildingComplete, false);
		List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
		if (requirementDescriptors.Count > 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGREQUIREMENTS, CodexTextStyle.Subtitle, null));
			foreach (Descriptor descriptor in requirementDescriptors)
			{
				list.Add(new CodexTextWithTooltip("    " + descriptor.text, descriptor.tooltipText, CodexTextStyle.Body));
			}
			list.Add(new CodexSpacer());
		}
		if (def.MaterialCategory.Length != def.Mass.Length)
		{
			global::Debug.LogWarningFormat("{0} Required Materials({1}) and Masses({2}) mismatch!", new object[]
			{
				def.name,
				string.Join(", ", def.MaterialCategory),
				string.Join<float>(", ", def.Mass)
			});
		}
		if (def.MaterialCategory.Length + def.Mass.Length != 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGCONSTRUCTIONPROPS, CodexTextStyle.Subtitle, null));
			list.Add(new CodexText("    " + string.Format(CODEX.FORMAT_STRINGS.BUILDING_SIZE, def.WidthInCells, def.HeightInCells), CodexTextStyle.Body, null));
			list.Add(new CodexText("    " + string.Format(CODEX.FORMAT_STRINGS.CONSTRUCTION_TIME, def.ConstructionTime), CodexTextStyle.Body, null));
			List<string> list2 = new List<string>();
			for (int i = 0; i < Math.Min(def.MaterialCategory.Length, def.Mass.Length); i++)
			{
				list2.Add(string.Format(CODEX.FORMAT_STRINGS.MATERIAL_MASS, MATERIALS.GetMaterialString(def.MaterialCategory[i]), GameUtil.GetFormattedMass(def.Mass[i], GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
			}
			list.Add(new CodexText("    " + CODEX.HEADERS.BUILDINGCONSTRUCTIONMATERIALS + string.Join(", ", list2), CodexTextStyle.Body, null));
			list.Add(new CodexSpacer());
		}
		List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
		if (effectDescriptors.Count > 0)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGEFFECTS, CodexTextStyle.Subtitle, null));
			foreach (Descriptor descriptor2 in effectDescriptors)
			{
				list.Add(new CodexTextWithTooltip("    " + descriptor2.text, descriptor2.tooltipText, CodexTextStyle.Body));
			}
			list.Add(new CodexSpacer());
		}
		string[] roomClassForObject = CodexEntryGenerator.GetRoomClassForObject(def.BuildingComplete);
		if (roomClassForObject != null)
		{
			list.Add(new CodexText(CODEX.HEADERS.BUILDINGTYPE, CodexTextStyle.Subtitle, null));
			foreach (string str in roomClassForObject)
			{
				list.Add(new CodexText("    " + str, CodexTextStyle.Body, null));
			}
			list.Add(new CodexSpacer());
		}
		list.Add(new CodexText("<i>" + Strings.Get("STRINGS.BUILDINGS.PREFABS." + def.PrefabID.ToUpper() + ".DESC") + "</i>", CodexTextStyle.Body, null));
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x0022BFAC File Offset: 0x0022A1AC
	public static string[] GetRoomClassForObject(GameObject obj)
	{
		List<string> list = new List<string>();
		KPrefabID component = obj.GetComponent<KPrefabID>();
		if (component != null)
		{
			foreach (Tag tag in component.Tags)
			{
				if (RoomConstraints.ConstraintTags.AllTags.Contains(tag))
				{
					list.Add(RoomConstraints.ConstraintTags.GetRoomConstraintLabelText(tag));
				}
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x0022C038 File Offset: 0x0022A238
	public static void GenerateImageContainers(Sprite[] sprites, List<ContentContainer> containers, ContentContainer.ContentLayout layout)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (Sprite sprite in sprites)
		{
			if (!(sprite == null))
			{
				CodexImage item = new CodexImage(128, 128, sprite);
				list.Add(item);
			}
		}
		containers.Add(new ContentContainer(list, layout));
	}

	// Token: 0x06005DF9 RID: 24057 RVA: 0x0022C090 File Offset: 0x0022A290
	public static void GenerateImageContainers(global::Tuple<Sprite, Color>[] sprites, List<ContentContainer> containers, ContentContainer.ContentLayout layout)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (global::Tuple<Sprite, Color> tuple in sprites)
		{
			if (tuple != null)
			{
				CodexImage item = new CodexImage(128, 128, tuple);
				list.Add(item);
			}
		}
		containers.Add(new ContentContainer(list, layout));
	}

	// Token: 0x06005DFA RID: 24058 RVA: 0x0022C0E4 File Offset: 0x0022A2E4
	public static void GenerateImageContainers(Sprite sprite, List<ContentContainer> containers)
	{
		List<ICodexWidget> list = new List<ICodexWidget>();
		CodexImage item = new CodexImage(128, 128, sprite);
		list.Add(item);
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DFB RID: 24059 RVA: 0x0022C11C File Offset: 0x0022A31C
	public static void CreateUnlockablesContentContainer(SubEntry subentry)
	{
		subentry.lockedContentContainer = new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(CODEX.HEADERS.SECTION_UNLOCKABLES, CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical)
		{
			showBeforeGeneratedContent = false
		};
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x0022C168 File Offset: 0x0022A368
	private static void GenerateFabricatorContainers(GameObject entity, List<ContentContainer> containers)
	{
		ComplexFabricator component = entity.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexSpacer(),
			new CodexText(Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (ComplexRecipe recipe in component.GetRecipes())
		{
			list.Add(new CodexRecipePanel(recipe, false));
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x0022C20C File Offset: 0x0022A40C
	private static void GenerateConfigurableConsumerContainers(GameObject buildingComplete, List<ContentContainer> containers)
	{
		IConfigurableConsumer component = buildingComplete.GetComponent<IConfigurableConsumer>();
		if (component == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexSpacer(),
			new CodexText(Strings.Get("STRINGS.CODEX.HEADERS.FABRICATIONS"), CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		List<ICodexWidget> list = new List<ICodexWidget>();
		foreach (IConfigurableConsumerOption data in component.GetSettingOptions())
		{
			list.Add(new CodexConfigurableConsumerRecipePanel(data));
		}
		containers.Add(new ContentContainer(list, ContentContainer.ContentLayout.Vertical));
	}

	// Token: 0x06005DFE RID: 24062 RVA: 0x0022C2A8 File Offset: 0x0022A4A8
	private static void GenerateReceptacleContainers(GameObject entity, List<ContentContainer> containers)
	{
		SingleEntityReceptacle plot = entity.GetComponent<SingleEntityReceptacle>();
		if (plot == null)
		{
			return;
		}
		containers.Add(new ContentContainer(new List<ICodexWidget>
		{
			new CodexText(Strings.Get("STRINGS.CODEX.HEADERS.RECEPTACLE"), CodexTextStyle.Subtitle, null),
			new CodexDividerLine()
		}, ContentContainer.ContentLayout.Vertical));
		Predicate<GameObject> <>9__0;
		foreach (Tag tag in plot.possibleDepositObjectTags)
		{
			List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
			if (plot.rotatable == null)
			{
				List<GameObject> list = prefabsWithTag;
				Predicate<GameObject> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = delegate(GameObject go)
					{
						IReceptacleDirection component = go.GetComponent<IReceptacleDirection>();
						return component != null && component.Direction != plot.Direction;
					});
				}
				list.RemoveAll(match);
			}
			foreach (GameObject gameObject in prefabsWithTag)
			{
				containers.Add(new ContentContainer(new List<ICodexWidget>
				{
					new CodexImage(64, 64, Def.GetUISprite(gameObject, "ui", false).first),
					new CodexText(gameObject.GetProperName(), CodexTextStyle.Body, null)
				}, ContentContainer.ContentLayout.Horizontal));
			}
		}
	}

	// Token: 0x06005DFF RID: 24063 RVA: 0x0022C418 File Offset: 0x0022A618
	// Note: this type is marked as 'beforefieldinit'.
	static CodexEntryGenerator()
	{
		Dictionary<Tag, Tag> dictionary = new Dictionary<Tag, Tag>();
		Tag industrialMachinery = RoomConstraints.ConstraintTags.IndustrialMachinery;
		dictionary[industrialMachinery] = "ManualGenerator";
		Tag recBuilding = RoomConstraints.ConstraintTags.RecBuilding;
		dictionary[recBuilding] = "ArcadeMachine";
		Tag clinic = RoomConstraints.ConstraintTags.Clinic;
		dictionary[clinic] = "MedicalCot";
		Tag washStation = RoomConstraints.ConstraintTags.WashStation;
		dictionary[washStation] = "WashBasin";
		Tag advancedWashStation = RoomConstraints.ConstraintTags.AdvancedWashStation;
		dictionary[advancedWashStation] = ShowerConfig.ID;
		Tag toiletType = RoomConstraints.ConstraintTags.ToiletType;
		dictionary[toiletType] = "Outhouse";
		Tag flushToiletType = RoomConstraints.ConstraintTags.FlushToiletType;
		dictionary[flushToiletType] = "FlushToilet";
		Tag scienceBuilding = RoomConstraints.ConstraintTags.ScienceBuilding;
		dictionary[scienceBuilding] = "ResearchCenter";
		Tag decoration = GameTags.Decoration;
		dictionary[decoration] = "FlowerVase";
		Tag ranchStationType = RoomConstraints.ConstraintTags.RanchStationType;
		dictionary[ranchStationType] = "RanchStation";
		Tag bedType = RoomConstraints.ConstraintTags.BedType;
		dictionary[bedType] = "Bed";
		Tag generatorType = RoomConstraints.ConstraintTags.GeneratorType;
		dictionary[generatorType] = "Generator";
		Tag lightSource = RoomConstraints.ConstraintTags.LightSource;
		dictionary[lightSource] = "FloorLamp";
		Tag rocketInterior = RoomConstraints.ConstraintTags.RocketInterior;
		dictionary[rocketInterior] = RocketControlStationConfig.ID;
		Tag creatureRelocator = RoomConstraints.ConstraintTags.CreatureRelocator;
		dictionary[creatureRelocator] = "CreatureDeliveryPoint";
		Tag cookTop = RoomConstraints.ConstraintTags.CookTop;
		dictionary[cookTop] = "CookingStation";
		Tag warmingStation = RoomConstraints.ConstraintTags.WarmingStation;
		dictionary[warmingStation] = "SpaceHeater";
		CodexEntryGenerator.RoomConstrainTagIcons = dictionary;
	}

	// Token: 0x04003EC8 RID: 16072
	private static string categoryPrefx = "BUILD_CATEGORY_";

	// Token: 0x04003EC9 RID: 16073
	public static readonly string FOOD_CATEGORY_ID = CodexCache.FormatLinkID("FOOD");

	// Token: 0x04003ECA RID: 16074
	public static readonly string FOOD_EFFECTS_ENTRY_ID = CodexCache.FormatLinkID("id_food_effects");

	// Token: 0x04003ECB RID: 16075
	public static readonly string TABLE_SALT_ENTRY_ID = CodexCache.FormatLinkID("id_table_salt");

	// Token: 0x04003ECC RID: 16076
	public static readonly string MINION_MODIFIERS_CATEGORY_ID = CodexCache.FormatLinkID("MINION_MODIFIERS");

	// Token: 0x04003ECD RID: 16077
	public static Tag[] HiddenRoomConstrainTags = new Tag[]
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
		RoomConstraints.ConstraintTags.MachineShopType,
		RoomConstraints.ConstraintTags.LightDutyGeneratorType,
		RoomConstraints.ConstraintTags.HeavyDutyGeneratorType
	};

	// Token: 0x04003ECE RID: 16078
	public static Dictionary<Tag, Tag> RoomConstrainTagIcons;
}
