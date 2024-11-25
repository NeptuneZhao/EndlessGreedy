using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class LightBugCrystalConfig : IEntityConfig
{
	// Token: 0x0600054C RID: 1356 RVA: 0x000275AC File Offset: 0x000257AC
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugCrystalBaseTrait", LIGHT2D.LIGHTBUG_COLOR_CRYSTAL, DECOR.BONUS.TIER8, is_baby, "cry_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugCrystalBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("CookedMeat"),
			SimHashes.Diamond.CreateTag()
		}, Tag.Invalid, LightBugCrystalConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Diamond.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00027727 File Offset: 0x00025927
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00027730 File Offset: 0x00025930
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugCrystalConfig.CreateLightBug("LightBugCrystal", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugCrystalEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugCrystalBaby", 45f, 15f, LightBugTuning.EGG_CHANCES_CRYSTAL, this.GetDlcIds(), LightBugCrystalConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x000277B3 File Offset: 0x000259B3
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x000277B5 File Offset: 0x000259B5
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040003AE RID: 942
	public const string ID = "LightBugCrystal";

	// Token: 0x040003AF RID: 943
	public const string BASE_TRAIT_ID = "LightBugCrystalBaseTrait";

	// Token: 0x040003B0 RID: 944
	public const string EGG_ID = "LightBugCrystalEgg";

	// Token: 0x040003B1 RID: 945
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x040003B2 RID: 946
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugCrystalConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003B3 RID: 947
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 7;
}
