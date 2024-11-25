using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003EF RID: 1007
public class UraniumCentrifugeConfig : IBuildingConfig
{
	// Token: 0x0600150F RID: 5391 RVA: 0x00073F5A File Offset: 0x0007215A
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x00073F64 File Offset: 0x00072164
	public override BuildingDef CreateBuildingDef()
	{
		string id = "UraniumCentrifuge";
		int width = 3;
		int height = 4;
		string anim = "enrichmentCentrifuge_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = UraniumCentrifugeConfig.outPipeOffset;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x0007404C File Offset: 0x0007224C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		UraniumCentrifuge uraniumCentrifuge = go.AddOrGet<UraniumCentrifuge>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, uraniumCentrifuge);
		uraniumCentrifuge.outStorage.capacityKg = 2000f;
		uraniumCentrifuge.storeProduced = true;
		uraniumCentrifuge.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		uraniumCentrifuge.duplicantOperated = false;
		uraniumCentrifuge.inStorage.SetDefaultStoredItemModifiers(UraniumCentrifugeConfig.storedItemModifiers);
		uraniumCentrifuge.buildStorage.SetDefaultStoredItemModifiers(UraniumCentrifugeConfig.storedItemModifiers);
		uraniumCentrifuge.outStorage.SetDefaultStoredItemModifiers(UraniumCentrifugeConfig.storedItemModifiers);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = uraniumCentrifuge.outStorage;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.UraniumOre).tag, 10f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag, 2f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false),
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MoltenUranium).tag, 8f, ComplexRecipe.RecipeElement.TemperatureOperation.Melted, false)
		};
		ComplexRecipe complexRecipe = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("UraniumCentrifuge", array, array2), array, array2);
		complexRecipe.time = 40f;
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe.description = STRINGS.BUILDINGS.PREFABS.URANIUMCENTRIFUGE.RECIPE_DESCRIPTION;
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("UraniumCentrifuge")
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x000741AE File Offset: 0x000723AE
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000BF9 RID: 3065
	public const string ID = "UraniumCentrifuge";

	// Token: 0x04000BFA RID: 3066
	public const float OUTPUT_TEMP = 1173.15f;

	// Token: 0x04000BFB RID: 3067
	public const float REFILL_RATE = 2400f;

	// Token: 0x04000BFC RID: 3068
	public static readonly CellOffset outPipeOffset = new CellOffset(1, 3);

	// Token: 0x04000BFD RID: 3069
	private static readonly List<Storage.StoredItemModifier> storedItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate
	};
}
