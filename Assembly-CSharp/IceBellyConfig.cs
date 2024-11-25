using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000110 RID: 272
[EntityConfigOrder(1)]
public class IceBellyConfig : IEntityConfig
{
	// Token: 0x0600051C RID: 1308 RVA: 0x00026A5C File Offset: 0x00024C5C
	public static GameObject CreateIceBelly(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BaseBellyConfig.BaseBelly(id, name, desc, anim_file, "IceBellyBaseTrait", is_baby, null), MooTuning.PEN_SIZE_PER_CREATURE);
		gameObject.AddOrGet<WarmBlooded>().BaseGenerationKW = 1.3f;
		Trait trait = Db.Get().CreateTrait("IceBellyBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BellyTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -BellyTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name, false, false, true));
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = BellyTuning.GERM_ID_EMMITED_ON_POOP;
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "IceBellyWellFed";
		def.caloriesPerCycle = BellyTuning.STANDARD_CALORIES_PER_CYCLE;
		def.growthDurationCycles = IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def.dropMass = IceBellyConfig.FIBER_PER_CYCLE * IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def.itemDroppedOnShear = IceBellyConfig.SCALE_GROWTH_EMIT_ELEMENT;
		def.levelCount = 6;
		def.hideSymbols = GoldBellyConfig.SCALE_SYMBOLS;
		GameObject gameObject2 = BaseBellyConfig.SetupDiet(gameObject, BaseBellyConfig.StandardDiets(), BellyTuning.CALORIES_PER_UNIT_EATEN, 1f);
		gameObject2.AddTag(GameTags.OriginalCreature);
		return gameObject2;
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00026BF0 File Offset: 0x00024DF0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00026BF8 File Offset: 0x00024DF8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(IceBellyConfig.CreateIceBelly("IceBelly", CREATURES.SPECIES.ICEBELLY.NAME, CREATURES.SPECIES.ICEBELLY.DESC, "ice_belly_kanim", false), "IceBellyEgg", CREATURES.SPECIES.ICEBELLY.EGG_NAME, CREATURES.SPECIES.ICEBELLY.DESC, "egg_icebelly_kanim", 8f, "IceBellyBaby", 120.00001f, 40f, BellyTuning.EGG_CHANCES_BASE, this.GetDlcIds(), IceBellyConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		gameObject.AddTag(GameTags.LargeCreature);
		return gameObject;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00026C84 File Offset: 0x00024E84
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00026C86 File Offset: 0x00024E86
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000390 RID: 912
	public const string ID = "IceBelly";

	// Token: 0x04000391 RID: 913
	public const string BASE_TRAIT_ID = "IceBellyBaseTrait";

	// Token: 0x04000392 RID: 914
	public const string EGG_ID = "IceBellyEgg";

	// Token: 0x04000393 RID: 915
	public static Tag SCALE_GROWTH_EMIT_ELEMENT = BasicFabricConfig.ID;

	// Token: 0x04000394 RID: 916
	public static float SCALE_INITIAL_GROWTH_PCT = 0.25f;

	// Token: 0x04000395 RID: 917
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 10f;

	// Token: 0x04000396 RID: 918
	public static float FIBER_PER_CYCLE = 0.5f;

	// Token: 0x04000397 RID: 919
	public static int EGG_SORT_ORDER = 0;
}
