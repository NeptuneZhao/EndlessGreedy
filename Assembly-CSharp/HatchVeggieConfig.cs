using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200010E RID: 270
[EntityConfigOrder(1)]
public class HatchVeggieConfig : IEntityConfig
{
	// Token: 0x06000510 RID: 1296 RVA: 0x000267E8 File Offset: 0x000249E8
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchVeggieBaseTrait", is_baby, "veg_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchVeggieBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.VeggieDiet(SimHashes.Carbon.CreateTag(), HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f);
		list.AddRange(BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f));
		return BaseHatchConfig.SetupDiet(prefab, list, HatchVeggieConfig.CALORIES_PER_KG_OF_ORE, HatchVeggieConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00026942 File Offset: 0x00024B42
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0002694C File Offset: 0x00024B4C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchVeggieConfig.CreateHatch("HatchVeggie", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.DESC, "hatch_kanim", false), "HatchVeggieEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_VEGGIE.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchVeggieBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_VEGGIE, this.GetDlcIds(), HatchVeggieConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x000269CD File Offset: 0x00024BCD
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x000269CF File Offset: 0x00024BCF
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000387 RID: 903
	public const string ID = "HatchVeggie";

	// Token: 0x04000388 RID: 904
	public const string BASE_TRAIT_ID = "HatchVeggieBaseTrait";

	// Token: 0x04000389 RID: 905
	public const string EGG_ID = "HatchVeggieEgg";

	// Token: 0x0400038A RID: 906
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x0400038B RID: 907
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x0400038C RID: 908
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchVeggieConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400038D RID: 909
	private static float MIN_POOP_SIZE_IN_KG = 50f;

	// Token: 0x0400038E RID: 910
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 1;
}
