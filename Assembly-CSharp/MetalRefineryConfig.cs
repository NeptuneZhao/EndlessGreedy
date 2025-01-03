﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class MetalRefineryConfig : IBuildingConfig
{
	// Token: 0x06000DB5 RID: 3509 RVA: 0x0004E4F0 File Offset: 0x0004C6F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MetalRefinery";
		int width = 3;
		int height = 4;
		string anim = "metalrefinery_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] all_MINERALS = MATERIALS.ALL_MINERALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.SelfHeatKilowattsWhenActive = 16f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 1);
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x0004E59C File Offset: 0x0004C79C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		LiquidCooledRefinery liquidCooledRefinery = go.AddOrGet<LiquidCooledRefinery>();
		liquidCooledRefinery.duplicantOperated = true;
		liquidCooledRefinery.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		liquidCooledRefinery.keepExcessLiquids = true;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		Workable workable = go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, liquidCooledRefinery);
		liquidCooledRefinery.coolantTag = MetalRefineryConfig.COOLANT_TAG;
		liquidCooledRefinery.minCoolantMass = 400f;
		liquidCooledRefinery.outStorage.capacityKg = 2000f;
		liquidCooledRefinery.thermalFudge = 0.8f;
		liquidCooledRefinery.inStorage.SetDefaultStoredItemModifiers(MetalRefineryConfig.RefineryStoredItemModifiers);
		liquidCooledRefinery.buildStorage.SetDefaultStoredItemModifiers(MetalRefineryConfig.RefineryStoredItemModifiers);
		liquidCooledRefinery.outStorage.SetDefaultStoredItemModifiers(MetalRefineryConfig.RefineryStoredItemModifiers);
		liquidCooledRefinery.outputOffset = new Vector3(1f, 0.5f);
		workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_metalrefinery_kanim")
		};
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.capacityTag = GameTags.Liquid;
		conduitConsumer.capacityKG = 800f;
		conduitConsumer.storage = liquidCooledRefinery.inStorage;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.storage = liquidCooledRefinery.outStorage;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = null;
		conduitDispenser.alwaysDispense = true;
		foreach (Element element in ElementLoader.elements.FindAll((Element e) => e.IsSolid && e.HasTag(GameTags.Metal)))
		{
			if (!element.HasTag(GameTags.Noncrushable))
			{
				Element lowTempTransition = element.highTempTransition.lowTempTransition;
				if (lowTempTransition != element)
				{
					ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
					{
						new ComplexRecipe.RecipeElement(element.tag, 100f)
					};
					ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
					{
						new ComplexRecipe.RecipeElement(lowTempTransition.tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
					};
					string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element.tag);
					string text = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array, array2);
					ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2)
					{
						time = 40f,
						description = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, lowTempTransition.name, element.name),
						nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
						fabricators = new List<Tag>
						{
							TagManager.Create("MetalRefinery")
						}
					};
					ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
				}
			}
		}
		Element element2 = ElementLoader.FindElementByHash(SimHashes.Steel);
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Iron).tag, 70f),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).tag, 20f),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 10f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Steel).tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element2.tag);
		string text2 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array3, array4);
		ComplexRecipe complexRecipe2 = new ComplexRecipe(text2, array3, array4)
		{
			time = 40f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Steel).name, ElementLoader.FindElementByHash(SimHashes.Iron).name),
			fabricators = new List<Tag>
			{
				TagManager.Create("MetalRefinery")
			}
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x0004E978 File Offset: 0x0004CB78
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGetDef<PoweredActiveStoppableController.Def>();
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
			component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		};
	}

	public const string ID = "MetalRefinery";

	private static readonly Tag COOLANT_TAG = GameTags.Liquid;

	private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};
}
