using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000106 RID: 262
[EntityConfigOrder(1)]
public class GoldBellyConfig : IEntityConfig
{
	// Token: 0x060004DF RID: 1247 RVA: 0x00025CBC File Offset: 0x00023EBC
	public static GameObject CreateGoldBelly(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BaseBellyConfig.BaseBelly(id, name, desc, anim_file, "GoldBellyBaseTrait", is_baby, "king_"), MooTuning.PEN_SIZE_PER_CREATURE);
		gameObject.AddOrGet<WarmBlooded>().BaseGenerationKW = 1.3f;
		Trait trait = Db.Get().CreateTrait("GoldBellyBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BellyTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -BellyTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name, false, false, true));
		string alwaysShowDisease = "PollenGerms";
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = alwaysShowDisease;
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "GoldBellyWellFed";
		def.caloriesPerCycle = BellyTuning.STANDARD_CALORIES_PER_CYCLE;
		def.growthDurationCycles = GoldBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def.dropMass = 1f;
		def.itemDroppedOnShear = GoldBellyConfig.SCALE_GROWTH_EMIT_ELEMENT;
		def.requiredDiet = "FriesCarrot";
		def.levelCount = 6;
		def.scaleGrowthSymbols = GoldBellyConfig.SCALE_SYMBOLS;
		GameObject gameObject2 = BaseBellyConfig.SetupDiet(gameObject, BaseBellyConfig.StandardDiets(), BellyTuning.CALORIES_PER_UNIT_EATEN, 1f);
		gameObject2.AddTag(GameTags.OriginalCreature);
		return gameObject2;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00025E60 File Offset: 0x00024060
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00025E68 File Offset: 0x00024068
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(GoldBellyConfig.CreateGoldBelly("GoldBelly", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "ice_belly_kanim", false), "GoldBellyEgg", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.EGG_NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "egg_icebelly_kanim", 8f, "GoldBellyBaby", 120.00001f, 40f, BellyTuning.EGG_CHANCES_GOLD, this.GetDlcIds(), GoldBellyConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>();
		gameObject.AddTag(GameTags.LargeCreature);
		return gameObject;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00025EFB File Offset: 0x000240FB
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00025EFD File Offset: 0x000240FD
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000362 RID: 866
	public const string ID = "GoldBelly";

	// Token: 0x04000363 RID: 867
	public const string BASE_TRAIT_ID = "GoldBellyBaseTrait";

	// Token: 0x04000364 RID: 868
	public const string EGG_ID = "GoldBellyEgg";

	// Token: 0x04000365 RID: 869
	public const int GERMS_EMMITED_PER_KG_POOPED = 1000;

	// Token: 0x04000366 RID: 870
	public static Tag SCALE_GROWTH_EMIT_ELEMENT = "GoldBellyCrown";

	// Token: 0x04000367 RID: 871
	public static float SCALE_INITIAL_GROWTH_PCT = 0.25f;

	// Token: 0x04000368 RID: 872
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 10f;

	// Token: 0x04000369 RID: 873
	public static float GOLD_PER_CYCLE = 25f;

	// Token: 0x0400036A RID: 874
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x0400036B RID: 875
	public static KAnimHashedString[] SCALE_SYMBOLS = new KAnimHashedString[]
	{
		"antler_0",
		"antler_1",
		"antler_2",
		"antler_3",
		"antler_4"
	};
}
