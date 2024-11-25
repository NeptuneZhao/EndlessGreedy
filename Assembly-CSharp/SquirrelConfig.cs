using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class SquirrelConfig : IEntityConfig
{
	// Token: 0x0600063A RID: 1594 RVA: 0x0002AC48 File Offset: 0x00028E48
	public static GameObject CreateSquirrel(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelBaseTrait", is_baby, null, false), SquirrelTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SquirrelBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		GameObject gameObject = BaseSquirrelConfig.SetupDiet(prefab, diet_infos, SquirrelConfig.MIN_POOP_SIZE_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0002AD80 File Offset: 0x00028F80
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0002AD88 File Offset: 0x00028F88
	public GameObject CreatePrefab()
	{
		GameObject prefab = SquirrelConfig.CreateSquirrel("Squirrel", CREATURES.SPECIES.SQUIRREL.NAME, CREATURES.SPECIES.SQUIRREL.DESC, "squirrel_kanim", false);
		string eggId = "SquirrelEgg";
		string eggName = CREATURES.SPECIES.SQUIRREL.EGG_NAME;
		string eggDesc = CREATURES.SPECIES.SQUIRREL.DESC;
		string egg_anim = "egg_squirrel_kanim";
		float egg_MASS = SquirrelTuning.EGG_MASS;
		string baby_id = "SquirrelBaby";
		float fertility_cycles = 60.000004f;
		float incubation_cycles = 20f;
		List<FertilityMonitor.BreedingChance> egg_CHANCES_BASE = SquirrelTuning.EGG_CHANCES_BASE;
		int egg_SORT_ORDER = SquirrelConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, egg_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0002AE0B File Offset: 0x0002900B
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0002AE0D File Offset: 0x0002900D
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400045E RID: 1118
	public const string ID = "Squirrel";

	// Token: 0x0400045F RID: 1119
	public const string BASE_TRAIT_ID = "SquirrelBaseTrait";

	// Token: 0x04000460 RID: 1120
	public const string EGG_ID = "SquirrelEgg";

	// Token: 0x04000461 RID: 1121
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x04000462 RID: 1122
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x04000463 RID: 1123
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x04000464 RID: 1124
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.4f;

	// Token: 0x04000465 RID: 1125
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x04000466 RID: 1126
	private static float KG_POOP_PER_DAY_OF_PLANT = 50f;

	// Token: 0x04000467 RID: 1127
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x04000468 RID: 1128
	public static int EGG_SORT_ORDER = 0;
}
