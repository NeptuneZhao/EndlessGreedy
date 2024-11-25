using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class LightBugPinkConfig : IEntityConfig
{
	// Token: 0x06000564 RID: 1380 RVA: 0x00027AE0 File Offset: 0x00025CE0
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugPinkBaseTrait", LIGHT2D.LIGHTBUG_COLOR_PINK, DECOR.BONUS.TIER6, is_baby, "pnk_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugPinkBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create("FriedMushroom"));
		hashSet.Add(TagManager.Create("SpiceBread"));
		hashSet.Add(TagManager.Create(PrickleFruitConfig.ID));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(TagManager.Create("Salsa"));
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		return BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugPinkConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00027C8A File Offset: 0x00025E8A
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00027C94 File Offset: 0x00025E94
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugPinkConfig.CreateLightBug("LightBugPink", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugPinkEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugPinkBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_PINK, this.GetDlcIds(), LightBugPinkConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00027D17 File Offset: 0x00025F17
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00027D19 File Offset: 0x00025F19
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040003BC RID: 956
	public const string ID = "LightBugPink";

	// Token: 0x040003BD RID: 957
	public const string BASE_TRAIT_ID = "LightBugPinkBaseTrait";

	// Token: 0x040003BE RID: 958
	public const string EGG_ID = "LightBugPinkEgg";

	// Token: 0x040003BF RID: 959
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x040003C0 RID: 960
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugPinkConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003C1 RID: 961
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 3;
}
