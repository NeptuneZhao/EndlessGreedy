using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011A RID: 282
public class LightBugOrangeConfig : IEntityConfig
{
	// Token: 0x06000558 RID: 1368 RVA: 0x00027840 File Offset: 0x00025A40
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugOrangeBaseTrait", LIGHT2D.LIGHTBUG_COLOR_ORANGE, DECOR.BONUS.TIER6, is_baby, "org_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugOrangeBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create(MushroomConfig.ID));
		hashSet.Add(TagManager.Create("FriedMushroom"));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		return BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugOrangeConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x000279C8 File Offset: 0x00025BC8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x000279D0 File Offset: 0x00025BD0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugOrangeConfig.CreateLightBug("LightBugOrange", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC, "lightbug_kanim", false);
		string eggId = "LightBugOrangeEgg";
		string eggName = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.EGG_NAME;
		string eggDesc = STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC;
		string egg_anim = "egg_lightbug_kanim";
		float egg_MASS = LightBugTuning.EGG_MASS;
		string baby_id = "LightBugOrangeBaby";
		float fertility_cycles = 15.000001f;
		float incubation_cycles = 5f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_ORANGE = LightBugTuning.EGG_CHANCES_ORANGE;
		int egg_SORT_ORDER = LightBugOrangeConfig.EGG_SORT_ORDER;
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_ORANGE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00027A55 File Offset: 0x00025C55
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x00027A57 File Offset: 0x00025C57
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040003B5 RID: 949
	public const string ID = "LightBugOrange";

	// Token: 0x040003B6 RID: 950
	public const string BASE_TRAIT_ID = "LightBugOrangeBaseTrait";

	// Token: 0x040003B7 RID: 951
	public const string EGG_ID = "LightBugOrangeEgg";

	// Token: 0x040003B8 RID: 952
	private static float KG_ORE_EATEN_PER_CYCLE = 0.25f;

	// Token: 0x040003B9 RID: 953
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugOrangeConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003BA RID: 954
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 1;
}
