using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200010A RID: 266
[EntityConfigOrder(1)]
public class HatchHardConfig : IEntityConfig
{
	// Token: 0x060004F7 RID: 1271 RVA: 0x000262A4 File Offset: 0x000244A4
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchHardBaseTrait", is_baby, "hvy_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchHardBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 200f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.HardRockDiet(SimHashes.Carbon.CreateTag(), HatchHardConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		list.AddRange(BaseHatchConfig.MetalDiet(SimHashes.Carbon.CreateTag(), HatchHardConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_1, null, 0f));
		return BaseHatchConfig.SetupDiet(prefab, list, HatchHardConfig.CALORIES_PER_KG_OF_ORE, HatchHardConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000263FE File Offset: 0x000245FE
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00026408 File Offset: 0x00024608
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchHardConfig.CreateHatch("HatchHard", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.DESC, "hatch_kanim", false), "HatchHardEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchHardBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_HARD, this.GetDlcIds(), HatchHardConfig.EGG_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00026489 File Offset: 0x00024689
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0002648B File Offset: 0x0002468B
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000376 RID: 886
	public const string ID = "HatchHard";

	// Token: 0x04000377 RID: 887
	public const string BASE_TRAIT_ID = "HatchHardBaseTrait";

	// Token: 0x04000378 RID: 888
	public const string EGG_ID = "HatchHardEgg";

	// Token: 0x04000379 RID: 889
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x0400037A RID: 890
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x0400037B RID: 891
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchHardConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x0400037C RID: 892
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x0400037D RID: 893
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 2;
}
