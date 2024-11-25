using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class MooConfig : IEntityConfig
{
	// Token: 0x06000596 RID: 1430 RVA: 0x000287DC File Offset: 0x000269DC
	public static GameObject CreateMoo(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseMooConfig.BaseMoo(id, name, CREATURES.SPECIES.MOO.DESC, "MooBaseTrait", anim_file, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MooTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MooBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MooTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MooTuning.STANDARD_LIFESPAN, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"GasGrass".ToTag()
			}, MooConfig.POOP_ELEMENT, MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, MooConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, Diet.Info.FoodType.EatPlantDirectly, false, null)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = MooConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x0002895D File Offset: 0x00026B5D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x00028964 File Offset: 0x00026B64
	public GameObject CreatePrefab()
	{
		return MooConfig.CreateMoo("Moo", CREATURES.SPECIES.MOO.NAME, CREATURES.SPECIES.MOO.DESC, "gassy_moo_kanim", false);
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0002898A File Offset: 0x00026B8A
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0002898C File Offset: 0x00026B8C
	public void OnSpawn(GameObject inst)
	{
		BaseMooConfig.OnSpawn(inst);
	}

	// Token: 0x040003DF RID: 991
	public const string ID = "Moo";

	// Token: 0x040003E0 RID: 992
	public const string BASE_TRAIT_ID = "MooBaseTrait";

	// Token: 0x040003E1 RID: 993
	public const SimHashes CONSUME_ELEMENT = SimHashes.Carbon;

	// Token: 0x040003E2 RID: 994
	public static Tag POOP_ELEMENT = SimHashes.Methane.CreateTag();

	// Token: 0x040003E3 RID: 995
	public static readonly float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 2f;

	// Token: 0x040003E4 RID: 996
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = MooTuning.STANDARD_CALORIES_PER_CYCLE / MooConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040003E5 RID: 997
	private static float KG_POOP_PER_DAY_OF_PLANT = 5f;

	// Token: 0x040003E6 RID: 998
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x040003E7 RID: 999
	private static float MIN_POOP_SIZE_IN_CALORIES = MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * MooConfig.MIN_POOP_SIZE_IN_KG / MooConfig.KG_POOP_PER_DAY_OF_PLANT;
}
