using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class LightBugBlackConfig : IEntityConfig
{
	// Token: 0x06000528 RID: 1320 RVA: 0x00026D4C File Offset: 0x00024F4C
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlackBaseTrait", Color.black, DECOR.BONUS.TIER7, is_baby, "blk_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBlackBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("Salsa"),
			TagManager.Create("Meat"),
			TagManager.Create("CookedMeat"),
			SimHashes.Katairite.CreateTag(),
			SimHashes.Phosphorus.CreateTag()
		}, Tag.Invalid, LightBugBlackConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Phosphorus.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00026EFA File Offset: 0x000250FA
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00026F04 File Offset: 0x00025104
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlack", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "lightbug_kanim", false);
		string eggId = "LightBugBlackEgg";
		string eggName = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC;
		string egg_anim = "egg_lightbug_kanim";
		float egg_MASS = LightBugTuning.EGG_MASS;
		string baby_id = "LightBugBlackBaby";
		float fertility_cycles = 45f;
		float incubation_cycles = 15f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BLACK = LightBugTuning.EGG_CHANCES_BLACK;
		int egg_SORT_ORDER = LightBugBlackConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BLACK, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00026F89 File Offset: 0x00025189
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00026F8B File Offset: 0x0002518B
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000399 RID: 921
	public const string ID = "LightBugBlack";

	// Token: 0x0400039A RID: 922
	public const string BASE_TRAIT_ID = "LightBugBlackBaseTrait";

	// Token: 0x0400039B RID: 923
	public const string EGG_ID = "LightBugBlackEgg";

	// Token: 0x0400039C RID: 924
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x0400039D RID: 925
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlackConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400039E RID: 926
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 5;
}
