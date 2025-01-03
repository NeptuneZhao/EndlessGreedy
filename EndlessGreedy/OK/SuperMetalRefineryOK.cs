﻿using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

[HarmonyPatch(typeof(MetalRefineryConfig))]
internal class SuperMetalRefinery
{
	[HarmonyPostfix]
	[HarmonyPatch("CreateBuildingDef")]
	public static void Postfix(ref BuildingDef __result)
	{
		__result.SelfHeatKilowattsWhenActive = -200f;
	}

	[HarmonyPrefix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static bool Prefix(GameObject go)
	{
		List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Preserve,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Seal
		};

		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

		//ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		//elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
		//{
		//	new ElementConverter.ConsumedElement(new Tag("Dirt"), 1f)
		//};
		//elementConverter.outputElements = new ElementConverter.OutputElement[2]
		//{
		//	new ElementConverter.OutputElement(500f, SimHashes.LiquidHydrogen, 15f, useEntityTemperature: false, storeOutput: true, 0f, 0.5f, 0.75f),
		//	new ElementConverter.OutputElement(500f, SimHashes.LiquidOxygen, 75f, useEntityTemperature: false, storeOutput: true, 0f, 0.5f, 0.75f)
		//};

		LiquidCooledRefinery liquidCooledRefinery = go.AddOrGet<LiquidCooledRefinery>();
		liquidCooledRefinery.duplicantOperated = true;
		liquidCooledRefinery.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		liquidCooledRefinery.keepExcessLiquids = true;
		liquidCooledRefinery.outputTemperature = 278.15f;

		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		ComplexFabricatorWorkable complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, liquidCooledRefinery);

		liquidCooledRefinery.coolantTag = GameTags.Liquid;
		liquidCooledRefinery.minCoolantMass = 0f;
		liquidCooledRefinery.outStorage.capacityKg = 0f;
		liquidCooledRefinery.thermalFudge = 0f;
		liquidCooledRefinery.inStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
		liquidCooledRefinery.buildStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
		liquidCooledRefinery.outStorage.SetDefaultStoredItemModifiers(RefineryStoredItemModifiers);
		liquidCooledRefinery.outputOffset = new Vector3(1f, 0.5f);

		complexFabricatorWorkable.overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_metalrefinery_kanim") };
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.capacityTag = GameTags.Liquid;
		conduitConsumer.capacityKG = 0f;
		conduitConsumer.storage = liquidCooledRefinery.inStorage;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.forceAlwaysSatisfied = true;

		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.storage = liquidCooledRefinery.outStorage;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = null;
		conduitDispenser.alwaysDispense = true;

		foreach (Element item in ElementLoader.elements.FindAll((Element e) => e.IsSolid && e.HasTag(GameTags.Metal)))
		{
			if (!item.HasTag(GameTags.Noncrushable))
			{
				Element lowTempTransition = item.highTempTransition.lowTempTransition;
				if (lowTempTransition != item)
				{
					ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[1]
					{
						new ComplexRecipe.RecipeElement(item.tag, 10f)
					};
					ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[1]
					{
						new ComplexRecipe.RecipeElement(lowTempTransition.tag, 100f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
					};
					string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", item.tag);
					string text = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array, array2);
					new ComplexRecipe(text, array, array2)
					{
						time = 15f,
						description = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, lowTempTransition.name, item.name),
						nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
						fabricators = new List<Tag> { TagManager.Create("MetalRefinery") }
					};
					ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
				}
			}
		}

		Element element = ElementLoader.FindElementByHash(SimHashes.Steel);
		ComplexRecipe.RecipeElement[] array3 = new ComplexRecipe.RecipeElement[3]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Iron).tag, 7f),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.RefinedCarbon).tag, 2f),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Lime).tag, 1f)
		};
		ComplexRecipe.RecipeElement[] array4 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Steel).tag, 1000f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature)
		};
		string obsolete_id2 = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element.tag);
		string text2 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array3, array4);
		new ComplexRecipe(text2, array3, array4)
		{
			time = 15f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			description = string.Format(STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(SimHashes.Steel).name, ElementLoader.FindElementByHash(SimHashes.Iron).name),
			fabricators = new List<Tag> { TagManager.Create("MetalRefinery") }
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id2, text2);

		Element element1 = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen);
		ComplexRecipe.RecipeElement[] array5 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Dirt).tag, 1f)
		};
		ComplexRecipe.RecipeElement[] array6 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag, 500f)
		};
		string obsolete_id3 = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element1.tag);
		string text3 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array5, array6);
		new ComplexRecipe(text3, array5, array6)
		{
			time = 1f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			description = string.Format("From Dirt to LiquidHydrogen"),
			fabricators = new List<Tag> { TagManager.Create("MetalRefinery") }
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id3, text3);

		Element element2 = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen);
		ComplexRecipe.RecipeElement[] array7 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Dirt).tag, 1f)
		};
		ComplexRecipe.RecipeElement[] array8 = new ComplexRecipe.RecipeElement[1]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.LiquidOxygen).tag, 500f)
		};
		string obsolete_id4 = ComplexRecipeManager.MakeObsoleteRecipeID("MetalRefinery", element1.tag);
		string text4 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", array7, array8);
		new ComplexRecipe(text4, array7, array8)
		{
			time = 1f,
			nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult,
			description = string.Format("From Dirt to LiquidOxygen"),
			fabricators = new List<Tag> { TagManager.Create("MetalRefinery") }
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id4, text4);
		Prioritizable.AddRef(go);
		return false;
	}
}