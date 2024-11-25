using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class StaterpillarConfig : IEntityConfig
{
	// Token: 0x06000652 RID: 1618 RVA: 0x0002B114 File Offset: 0x00029314
	public static GameObject CreateStaterpillar(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseStaterpillarConfig.BaseStaterpillar(id, name, desc, anim_file, "StaterpillarBaseTrait", is_baby, ObjectLayer.Wire, StaterpillarGeneratorConfig.ID, Tag.Invalid, null, 283.15f, 313.15f, 173.15f, 373.15f, null), TUNING.CREATURES.SPACE_REQUIREMENTS.TIER3);
		Trait trait = Db.Get().CreateTrait("StaterpillarBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, StaterpillarTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = new List<Diet.Info>();
		list.AddRange(BaseStaterpillarConfig.RawMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		list.AddRange(BaseStaterpillarConfig.RefinedMetalDiet(SimHashes.Hydrogen.CreateTag(), StaterpillarConfig.CALORIES_PER_KG_OF_ORE, StaterpillarTuning.POOP_CONVERSTION_RATE, null, 0f));
		GameObject gameObject = BaseStaterpillarConfig.SetupDiet(prefab, list);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x0002B297 File Offset: 0x00029497
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0002B2A0 File Offset: 0x000294A0
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(StaterpillarConfig.CreateStaterpillar("Staterpillar", STRINGS.CREATURES.SPECIES.STATERPILLAR.NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "caterpillar_kanim", false), "StaterpillarEgg", STRINGS.CREATURES.SPECIES.STATERPILLAR.EGG_NAME, STRINGS.CREATURES.SPECIES.STATERPILLAR.DESC, "egg_caterpillar_kanim", StaterpillarTuning.EGG_MASS, "StaterpillarBaby", 60.000004f, 20f, StaterpillarTuning.EGG_CHANCES_BASE, this.GetDlcIds(), 0, true, false, true, 1f, false);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x0002B31D File Offset: 0x0002951D
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gulp", false);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0002B335 File Offset: 0x00029535
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000476 RID: 1142
	public const string ID = "Staterpillar";

	// Token: 0x04000477 RID: 1143
	public const string BASE_TRAIT_ID = "StaterpillarBaseTrait";

	// Token: 0x04000478 RID: 1144
	public const string EGG_ID = "StaterpillarEgg";

	// Token: 0x04000479 RID: 1145
	public const int EGG_SORT_ORDER = 0;

	// Token: 0x0400047A RID: 1146
	private static float KG_ORE_EATEN_PER_CYCLE = 60f;

	// Token: 0x0400047B RID: 1147
	private static float CALORIES_PER_KG_OF_ORE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE / StaterpillarConfig.KG_ORE_EATEN_PER_CYCLE;
}
