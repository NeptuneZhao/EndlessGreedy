using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class DataMinerConfig : IBuildingConfig
{
	// Token: 0x060001DC RID: 476 RVA: 0x0000D492 File Offset: 0x0000B692
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000D49C File Offset: 0x0000B69C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DataMiner";
		int width = 3;
		int height = 2;
		string anim = "data_miner_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.EnergyConsumptionWhenActive = 2000f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 5f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000D540 File Offset: 0x0000B740
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		go.AddOrGet<LogicOperationalController>();
		DataMiner dataMiner = go.AddOrGet<DataMiner>();
		dataMiner.duplicantOperated = false;
		dataMiner.showProgressBar = true;
		dataMiner.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		BuildingTemplates.CreateComplexFabricatorStorage(go, dataMiner);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(this.INPUT_MATERIAL_TAG, 5f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(this.OUTPUT_MATERIAL_TAG, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("DataMiner", this.OUTPUT_MATERIAL_TAG);
		string text = ComplexRecipeManager.MakeRecipeID("DataMiner", array, array2);
		ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2);
		complexRecipe.time = 0.0033333334f;
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(this.INPUT_MATERIAL).name, "TODO");
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("DataMiner")
		};
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
		complexRecipe.sortOrder = 300;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		Prioritizable.AddRef(go);
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000D66D File Offset: 0x0000B86D
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000127 RID: 295
	public const string ID = "DataMiner";

	// Token: 0x04000128 RID: 296
	public const float POWER_USAGE_W = 2000f;

	// Token: 0x04000129 RID: 297
	public const float BASE_UNITS_PRODUCED_PER_CYCLE = 2f;

	// Token: 0x0400012A RID: 298
	public const float BASE_DTU_PRODUCTION = 5f;

	// Token: 0x0400012B RID: 299
	public const float STORAGE_CAPACITY_KG = 1000f;

	// Token: 0x0400012C RID: 300
	public const float MASS_CONSUMED_PER_BANK_KG = 5f;

	// Token: 0x0400012D RID: 301
	public const float BASE_DURATION = 0.0033333334f;

	// Token: 0x0400012E RID: 302
	public static MathUtil.MinMax PRODUCTION_RATE_SCALE = new MathUtil.MinMax(0.6f, 4f);

	// Token: 0x0400012F RID: 303
	public static MathUtil.MinMax TEMPERATURE_SCALING_RANGE = new MathUtil.MinMax(5f, 350f);

	// Token: 0x04000130 RID: 304
	public SimHashes INPUT_MATERIAL = SimHashes.Polypropylene;

	// Token: 0x04000131 RID: 305
	public Tag INPUT_MATERIAL_TAG = SimHashes.Polypropylene.CreateTag();

	// Token: 0x04000132 RID: 306
	public Tag OUTPUT_MATERIAL_TAG = OrbitalResearchDatabankConfig.TAG;

	// Token: 0x04000133 RID: 307
	public const float BASE_PRODUCTION_PROGRESS_PER_TICK = 0.00066666666f;
}
