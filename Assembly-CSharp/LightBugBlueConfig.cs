using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class LightBugBlueConfig : IEntityConfig
{
	// Token: 0x06000534 RID: 1332 RVA: 0x00027014 File Offset: 0x00025214
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlueBaseTrait", LIGHT2D.LIGHTBUG_COLOR_BLUE, DECOR.BONUS.TIER6, is_baby, "blu_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBlueBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("SpiceBread"),
			TagManager.Create("Salsa"),
			SimHashes.Phosphorite.CreateTag(),
			SimHashes.Phosphorus.CreateTag()
		}, Tag.Invalid, LightBugBlueConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Phosphorite.CreateTag(),
			SimHashes.Phosphorus.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x000271C2 File Offset: 0x000253C2
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x000271CC File Offset: 0x000253CC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlueConfig.CreateLightBug("LightBugBlue", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugBlueEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLUE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBlueBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_BLUE, this.GetDlcIds(), LightBugBlueConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		return gameObject;
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0002724F File Offset: 0x0002544F
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00027251 File Offset: 0x00025451
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040003A0 RID: 928
	public const string ID = "LightBugBlue";

	// Token: 0x040003A1 RID: 929
	public const string BASE_TRAIT_ID = "LightBugBlueBaseTrait";

	// Token: 0x040003A2 RID: 930
	public const string EGG_ID = "LightBugBlueEgg";

	// Token: 0x040003A3 RID: 931
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x040003A4 RID: 932
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlueConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003A5 RID: 933
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 4;
}
