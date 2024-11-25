﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000101 RID: 257
public class DreckoConfig : IEntityConfig
{
	// Token: 0x060004C2 RID: 1218 RVA: 0x000252E8 File Offset: 0x000234E8
	public static GameObject CreateDrecko(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoBaseTrait", is_baby, "fbr_", 283.15f, 333.15f, 243.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, DreckoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DreckoBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"SpiceVine".ToTag(),
				SwampLilyConfig.ID.ToTag(),
				"BasicSingleHarvestPlant".ToTag()
			}, DreckoConfig.POOP_ELEMENT, DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, Diet.Info.FoodType.EatPlantDirectly, false, null)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = DreckoConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		ScaleGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ScaleGrowthMonitor.Def>();
		def2.defaultGrowthRate = 1f / DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600f;
		def2.dropMass = DreckoConfig.FIBER_PER_CYCLE * DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def2.itemDroppedOnShear = DreckoConfig.EMIT_ELEMENT;
		def2.levelCount = 6;
		def2.targetAtmosphere = SimHashes.Hydrogen;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x000254E4 File Offset: 0x000236E4
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x000254EC File Offset: 0x000236EC
	public virtual GameObject CreatePrefab()
	{
		GameObject prefab = DreckoConfig.CreateDrecko("Drecko", CREATURES.SPECIES.DRECKO.NAME, CREATURES.SPECIES.DRECKO.DESC, "drecko_kanim", false);
		string eggId = "DreckoEgg";
		string eggName = CREATURES.SPECIES.DRECKO.EGG_NAME;
		string eggDesc = CREATURES.SPECIES.DRECKO.DESC;
		string egg_anim = "egg_drecko_kanim";
		float egg_MASS = DreckoTuning.EGG_MASS;
		string baby_id = "DreckoBaby";
		float fertility_cycles = 90f;
		float incubation_cycles = 30f;
		int egg_SORT_ORDER = DreckoConfig.EGG_SORT_ORDER;
		return EntityTemplates.ExtendEntityToFertileCreature(prefab, eggId, eggName, eggDesc, egg_anim, egg_MASS, baby_id, fertility_cycles, incubation_cycles, DreckoTuning.EGG_CHANCES_BASE, this.GetDlcIds(), egg_SORT_ORDER, true, false, true, 1f, false);
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x0002556F File Offset: 0x0002376F
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00025571 File Offset: 0x00023771
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400033D RID: 829
	public const string ID = "Drecko";

	// Token: 0x0400033E RID: 830
	public const string BASE_TRAIT_ID = "DreckoBaseTrait";

	// Token: 0x0400033F RID: 831
	public const string EGG_ID = "DreckoEgg";

	// Token: 0x04000340 RID: 832
	public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();

	// Token: 0x04000341 RID: 833
	public static Tag EMIT_ELEMENT = BasicFabricConfig.ID;

	// Token: 0x04000342 RID: 834
	private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.75f;

	// Token: 0x04000343 RID: 835
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x04000344 RID: 836
	private static float KG_POOP_PER_DAY_OF_PLANT = 13.33f;

	// Token: 0x04000345 RID: 837
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x04000346 RID: 838
	private static float MIN_POOP_SIZE_IN_CALORIES = DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoConfig.MIN_POOP_SIZE_IN_KG / DreckoConfig.KG_POOP_PER_DAY_OF_PLANT;

	// Token: 0x04000347 RID: 839
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 8f;

	// Token: 0x04000348 RID: 840
	public static float FIBER_PER_CYCLE = 0.25f;

	// Token: 0x04000349 RID: 841
	public static int EGG_SORT_ORDER = 800;
}
