using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class LightBugPurpleConfig : IEntityConfig
{
	// Token: 0x06000570 RID: 1392 RVA: 0x00027DA4 File Offset: 0x00025FA4
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPurpleBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PURPLE, DECOR.BONUS.TIER6, is_baby, "prp_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugPurpleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create("FriedMushroom"));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(TagManager.Create(SpiceNutConfig.ID));
		hashSet.Add(TagManager.Create("SpiceBread"));
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		return BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugPurpleConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00027F3D File Offset: 0x0002613D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00027F44 File Offset: 0x00026144
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPurpleConfig.CreateLightBug("LightBugPurple", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC, "lightbug_kanim", false);
		string eggId = "LightBugPurpleEgg";
		string eggName = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.DESC;
		string egg_anim = "egg_lightbug_kanim";
		float egg_MASS = LightBugTuning.EGG_MASS;
		string baby_id = "LightBugPurpleBaby";
		float fertility_cycles = 15.000001f;
		float incubation_cycles = 5f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_PURPLE = LightBugTuning.EGG_CHANCES_PURPLE;
		int egg_SORT_ORDER = LightBugPurpleConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_PURPLE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00027FC9 File Offset: 0x000261C9
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00027FCB File Offset: 0x000261CB
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040003C3 RID: 963
	public const string ID = "LightBugPurple";

	// Token: 0x040003C4 RID: 964
	public const string BASE_TRAIT_ID = "LightBugPurpleBaseTrait";

	// Token: 0x040003C5 RID: 965
	public const string EGG_ID = "LightBugPurpleEgg";

	// Token: 0x040003C6 RID: 966
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x040003C7 RID: 967
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPurpleConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003C8 RID: 968
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 2;
}
