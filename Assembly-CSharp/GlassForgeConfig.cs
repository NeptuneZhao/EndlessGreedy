using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class GlassForgeConfig : IBuildingConfig
{
	// Token: 0x06000A72 RID: 2674 RVA: 0x0003E33C File Offset: 0x0003C53C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GlassForge";
		int width = 5;
		int height = 4;
		string anim = "glassrefinery_kanim";
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
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = GlassForgeConfig.outPipeOffset;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0003E3D4 File Offset: 0x0003C5D4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		GlassForge glassForge = go.AddOrGet<GlassForge>();
		glassForge.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
		Workable workable = go.AddOrGet<ComplexFabricatorWorkable>();
		glassForge.duplicantOperated = true;
		BuildingTemplates.CreateComplexFabricatorStorage(go, glassForge);
		glassForge.outStorage.capacityKg = 2000f;
		glassForge.storeProduced = true;
		glassForge.inStorage.SetDefaultStoredItemModifiers(GlassForgeConfig.RefineryStoredItemModifiers);
		glassForge.buildStorage.SetDefaultStoredItemModifiers(GlassForgeConfig.RefineryStoredItemModifiers);
		glassForge.outStorage.SetDefaultStoredItemModifiers(GlassForgeConfig.OutputItemModifiers);
		glassForge.outputOffset = new Vector3(1f, 0.5f);
		workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_metalrefinery_kanim")
		};
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.storage = glassForge.outStorage;
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = null;
		conduitDispenser.alwaysDispense = true;
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Sand).tag, 100f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MoltenGlass).tag, 25f, ComplexRecipe.RecipeElement.TemperatureOperation.Melted, false)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("GlassForge", array[0].material);
		string text = ComplexRecipeManager.MakeRecipeID("GlassForge", array, array2);
		ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2);
		complexRecipe.time = 40f;
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.Result;
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION, ElementLoader.GetElement(array2[0].material).name, ElementLoader.GetElement(array[0].material).name);
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("GlassForge")
		};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0003E5AD File Offset: 0x0003C7AD
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

	// Token: 0x040006EE RID: 1774
	public const string ID = "GlassForge";

	// Token: 0x040006EF RID: 1775
	private const float INPUT_KG = 100f;

	// Token: 0x040006F0 RID: 1776
	public static readonly CellOffset outPipeOffset = new CellOffset(1, 3);

	// Token: 0x040006F1 RID: 1777
	private static readonly List<Storage.StoredItemModifier> RefineryStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve
	};

	// Token: 0x040006F2 RID: 1778
	public static readonly List<Storage.StoredItemModifier> OutputItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Preserve,
		Storage.StoredItemModifier.Insulate
	};
}
