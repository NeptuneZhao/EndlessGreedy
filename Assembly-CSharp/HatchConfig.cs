using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000108 RID: 264
[EntityConfigOrder(1)]
public class HatchConfig : IEntityConfig
{
	// Token: 0x060004EB RID: 1259 RVA: 0x00026030 File Offset: 0x00024230
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchBaseTrait", is_baby, null), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.BasicRockDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		list.AddRange(BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f));
		GameObject gameObject = BaseHatchConfig.SetupDiet(prefab, list, HatchConfig.CALORIES_PER_KG_OF_ORE, HatchConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00026191 File Offset: 0x00024391
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00026198 File Offset: 0x00024398
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchConfig.CreateHatch("Hatch", STRINGS.CREATURES.SPECIES.HATCH.NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "hatch_kanim", false), "HatchEgg", STRINGS.CREATURES.SPECIES.HATCH.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_BASE, this.GetDlcIds(), HatchConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00026219 File Offset: 0x00024419
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0002621B File Offset: 0x0002441B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400036D RID: 877
	public const string ID = "Hatch";

	// Token: 0x0400036E RID: 878
	public const string BASE_TRAIT_ID = "HatchBaseTrait";

	// Token: 0x0400036F RID: 879
	public const string EGG_ID = "HatchEgg";

	// Token: 0x04000370 RID: 880
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x04000371 RID: 881
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x04000372 RID: 882
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000373 RID: 883
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000374 RID: 884
	public static int EGG_SORT_ORDER = 0;
}
