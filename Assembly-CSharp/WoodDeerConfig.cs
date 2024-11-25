using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class WoodDeerConfig : IEntityConfig
{
	// Token: 0x06000676 RID: 1654 RVA: 0x0002BB4C File Offset: 0x00029D4C
	public static GameObject CreateWoodDeer(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDeerConfig.BaseDeer(id, name, desc, anim_file, "WoodDeerBaseTrait", is_baby, null), DeerTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("WoodDeerBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.66667f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		GameObject gameObject = BaseDeerConfig.SetupDiet(prefab, new List<Diet.Info>
		{
			BaseDeerConfig.CreateDietInfo("HardSkinBerryPlant", SimHashes.Dirt.CreateTag(), WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				"HardSkinBerry"
			}, SimHashes.Dirt.CreateTag(), WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG / 1f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 3f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			BaseDeerConfig.CreateDietInfo("PrickleFlower", SimHashes.Dirt.CreateTag(), WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 2f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				PrickleFruitConfig.ID
			}, SimHashes.Dirt.CreateTag(), WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 1f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 6f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), WoodDeerConfig.MIN_KG_CONSUMED_BEFORE_POOPING);
		gameObject.AddTag(GameTags.OriginalCreature);
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "WoodDeerWellFed";
		def.caloriesPerCycle = 100000f;
		def.growthDurationCycles = WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;
		def.dropMass = WoodDeerConfig.WOOD_MASS_PER_ANTLER;
		def.itemDroppedOnShear = WoodLogConfig.TAG;
		def.levelCount = 6;
		return gameObject;
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0002BDB0 File Offset: 0x00029FB0
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0002BDB8 File Offset: 0x00029FB8
	public GameObject CreatePrefab()
	{
		GameObject prefab = WoodDeerConfig.CreateWoodDeer("WoodDeer", STRINGS.CREATURES.SPECIES.WOODDEER.NAME, STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "ice_floof_kanim", false);
		string eggId = "WoodDeerEgg";
		string eggName = STRINGS.CREATURES.SPECIES.WOODDEER.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.WOODDEER.DESC;
		string egg_anim = "egg_ice_floof_kanim";
		float egg_MASS = DeerTuning.EGG_MASS;
		string baby_id = "WoodDeerBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = DeerTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = WoodDeerConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0002BE3B File Offset: 0x0002A03B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0002BE3D File Offset: 0x0002A03D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000495 RID: 1173
	public const string ID = "WoodDeer";

	// Token: 0x04000496 RID: 1174
	public const string BASE_TRAIT_ID = "WoodDeerBaseTrait";

	// Token: 0x04000497 RID: 1175
	public const string EGG_ID = "WoodDeerEgg";

	// Token: 0x04000498 RID: 1176
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x04000499 RID: 1177
	public const float CALORIES_PER_PLANT_BITE = 100000f;

	// Token: 0x0400049A RID: 1178
	public const float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.2f;

	// Token: 0x0400049B RID: 1179
	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == "HardSkinBerry").cropDuration / 600f;

	// Token: 0x0400049C RID: 1180
	public static float KG_PLANT_EATEN_A_DAY = 0.2f * WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS;

	// Token: 0x0400049D RID: 1181
	public static float HARD_SKIN_CALORIES_PER_KG = 100000f / WoodDeerConfig.KG_PLANT_EATEN_A_DAY;

	// Token: 0x0400049E RID: 1182
	public static float BRISTLE_CALORIES_PER_KG = WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG * 2f;

	// Token: 0x0400049F RID: 1183
	public static float ANTLER_GROWTH_TIME_IN_CYCLES = 6f;

	// Token: 0x040004A0 RID: 1184
	public static float ANTLER_STARTING_GROWTH_PCT = 0.5f;

	// Token: 0x040004A1 RID: 1185
	public static float WOOD_PER_CYCLE = 60f;

	// Token: 0x040004A2 RID: 1186
	public static float WOOD_MASS_PER_ANTLER = WoodDeerConfig.WOOD_PER_CYCLE * WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;

	// Token: 0x040004A3 RID: 1187
	private static float POOP_MASS_CONVERSION_MULTIPLIER = 8.333334f;

	// Token: 0x040004A4 RID: 1188
	private static float MIN_KG_CONSUMED_BEFORE_POOPING = 1f;

	// Token: 0x040004A5 RID: 1189
	public static int EGG_SORT_ORDER = 0;
}
