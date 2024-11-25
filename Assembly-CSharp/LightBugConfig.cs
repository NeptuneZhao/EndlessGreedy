using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class LightBugConfig : IEntityConfig
{
	// Token: 0x06000540 RID: 1344 RVA: 0x000272DC File Offset: 0x000254DC
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBaseTrait", LIGHT2D.LIGHTBUG_COLOR, DECOR.BONUS.TIER4, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create(PrickleFruitConfig.ID));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("HardSkinBerry"));
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		GameObject gameObject = BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0002746B File Offset: 0x0002566B
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00027474 File Offset: 0x00025674
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugConfig.CreateLightBug("LightBug", STRINGS.CREATURES.SPECIES.LIGHTBUG.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, "LightBugEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_BASE, this.GetDlcIds(), LightBugConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
		gameObject.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
		return gameObject;
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00027508 File Offset: 0x00025708
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002750A File Offset: 0x0002570A
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x040003A7 RID: 935
	public const string ID = "LightBug";

	// Token: 0x040003A8 RID: 936
	public const string BASE_TRAIT_ID = "LightBugBaseTrait";

	// Token: 0x040003A9 RID: 937
	public const string EGG_ID = "LightBugEgg";

	// Token: 0x040003AA RID: 938
	private static float KG_ORE_EATEN_PER_CYCLE = 0.166f;

	// Token: 0x040003AB RID: 939
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040003AC RID: 940
	public static int EGG_SORT_ORDER = 100;
}
