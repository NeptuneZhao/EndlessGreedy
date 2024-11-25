using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class SealConfig : IEntityConfig
{
	// Token: 0x06000629 RID: 1577 RVA: 0x0002A858 File Offset: 0x00028A58
	public static GameObject CreateSeal(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSealConfig.BaseSeal(id, name, desc, anim_file, "SealBaseTrait", is_baby, null);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SealTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SealBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		gameObject = BaseSealConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>
			{
				"SpaceTree"
			}, SimHashes.Ethanol.CreateTag(), 2500f, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f, false, Diet.Info.FoodType.EatPlantStorage, false, null),
			new Diet.Info(new HashSet<Tag>
			{
				SimHashes.Sucrose.CreateTag()
			}, SimHashes.Ethanol.CreateTag(), 3246.7532f, 1.2987013f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, new string[]
			{
				"eat_ore_pre",
				"eat_ore_loop",
				"eat_ore_pst"
			})
		}, 2500f, SealConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<CreaturePoopLoot.Def>().Loot = new CreaturePoopLoot.LootData[]
		{
			new CreaturePoopLoot.LootData
			{
				tag = "SpaceTreeSeed",
				probability = 0.2f
			}
		};
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0002AA59 File Offset: 0x00028C59
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0002AA60 File Offset: 0x00028C60
	public GameObject CreatePrefab()
	{
		GameObject prefab = SealConfig.CreateSeal("Seal", STRINGS.CREATURES.SPECIES.SEAL.NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "seal_kanim", false);
		string eggId = "SealEgg";
		string eggName = STRINGS.CREATURES.SPECIES.SEAL.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.SEAL.DESC;
		string egg_anim = "egg_seal_kanim";
		float egg_MASS = SealTuning.EGG_MASS;
		string baby_id = "SealBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = SealTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = SealConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0002AAE3 File Offset: 0x00028CE3
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0002AAE5 File Offset: 0x00028CE5
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000454 RID: 1108
	public const string ID = "Seal";

	// Token: 0x04000455 RID: 1109
	public const string BASE_TRAIT_ID = "SealBaseTrait";

	// Token: 0x04000456 RID: 1110
	public const string EGG_ID = "SealEgg";

	// Token: 0x04000457 RID: 1111
	public const float SUGAR_TREE_SEED_PROBABILITY_ON_POOP = 0.2f;

	// Token: 0x04000458 RID: 1112
	public const float SUGAR_WATER_KG_CONSUMED_PER_DAY = 40f;

	// Token: 0x04000459 RID: 1113
	public const float CALORIES_PER_1KG_OF_SUGAR_WATER = 2500f;

	// Token: 0x0400045A RID: 1114
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x0400045B RID: 1115
	public static int EGG_SORT_ORDER = 0;
}
