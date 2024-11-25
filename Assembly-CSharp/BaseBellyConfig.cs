﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000096 RID: 150
public static class BaseBellyConfig
{
	// Token: 0x060002DA RID: 730 RVA: 0x00014D1C File Offset: 0x00012F1C
	public static GameObject BaseBelly(string id, string name, string desc, string anim_file, string traitId, bool is_baby, string symbolOverridePrefix = null)
	{
		float mass = 400f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 2, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		KBoxCollider2D kboxCollider2D = gameObject.AddOrGet<KBoxCollider2D>();
		kboxCollider2D.offset = new Vector2f(0f, kboxCollider2D.offset.y);
		gameObject.GetComponent<KBatchedAnimController>().Offset = new Vector3(0f, 0f, 0f);
		string navGridName = "WalkerNavGrid2x2";
		if (is_baby)
		{
			navGridName = "WalkerBabyNavGrid";
		}
		EntityTemplates.ExtendEntityToBasicCreature(true, gameObject, FactionManager.FactionID.Pest, traitId, navGridName, NavType.Floor, 32, 2f, "Meat", 14, true, false, 303.15f, 343.15f, 173.15f, 373.15f);
		gameObject.GetDef<CritterTemperatureMonitor.Def>().isBammoth = true;
		gameObject.AddOrGet<Navigator>();
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["IceBelly"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGetDef<WorldSpawnableMonitor.Def>();
		gameObject.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
		gameObject.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_idle", NOISE_POLLUTION.CREATURES.TIER2);
		SoundEventVolumeCache.instance.AddVolume("FloorSoundEvent", "Hatch_footstep", NOISE_POLLUTION.CREATURES.TIER1);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_land", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_chew", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_hurt", NOISE_POLLUTION.CREATURES.TIER5);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_voice_die", NOISE_POLLUTION.CREATURES.TIER5);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_emerge", NOISE_POLLUTION.CREATURES.TIER6);
		SoundEventVolumeCache.instance.AddVolume("hatch_kanim", "Hatch_drill_hide", NOISE_POLLUTION.CREATURES.TIER6);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Walker, false);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		bool condition = !is_baby;
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Burrowed, true, "idle_mound", STRINGS.CREATURES.STATUSITEMS.BURROWED.NAME, STRINGS.CREATURES.STATUSITEMS.BURROWED.TOOLTIP), condition, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("eat_pre", "eat_pst", null), condition, -1).PushInterruptGroup().Add(new CreatureSleepStates.Def(), true, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def(), !is_baby, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.WantsToEnterBurrow, false, "hide", STRINGS.CREATURES.STATUSITEMS.BURROWING.NAME, STRINGS.CREATURES.STATUSITEMS.BURROWING.TOOLTIP), condition, -1).Add(new LayEggStates.Def(), !is_baby, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def
		{
			shouldBeBehindMilkTank = false,
			drinkCellOffsetGetFn = (is_baby ? new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_CritterOneByOne) : new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_BammothAdult))
		}, true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).Add(new CallAdultStates.Def(), is_baby, -1).Add(new CritterCondoStates.Def(), !is_baby, -1).PopInterruptGroup().Add(new IdleStates.Def(), true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.BellySpecies, symbolOverridePrefix);
		return gameObject;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0001516C File Offset: 0x0001336C
	public static Diet.Info BasicDiet(Tag foodTag, Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		return new Diet.Info(new HashSet<Tag>
		{
			foodTag
		}, poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatPlantDirectly, true, null);
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00015198 File Offset: 0x00013398
	public static List<Diet.Info> StandardDiets()
	{
		return new List<Diet.Info>
		{
			BaseBellyConfig.BasicDiet("CarrotPlant", "IceBellyPoop", BellyTuning.CALORIES_PER_UNIT_EATEN / BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS, 67.474f / BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f),
			new Diet.Info(new HashSet<Tag>
			{
				CarrotConfig.ID
			}, "IceBellyPoop", BellyTuning.CALORIES_PER_UNIT_EATEN / 1f, 67.474f, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f, false, Diet.Info.FoodType.EatSolid, true, null),
			new Diet.Info(new HashSet<Tag>
			{
				"FriesCarrot"
			}, "IceBellyPoop", FOOD.FOOD_TYPES.FRIES_CARROT.CaloriesPerUnit / 0.75928f, 120.01f, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f, false, Diet.Info.FoodType.EatSolid, true, null),
			BaseBellyConfig.BasicDiet("BeanPlant", "IceBellyPoop", BellyTuning.CALORIES_PER_UNIT_EATEN / BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS / 0.744f, 67.474f / BellyTuning.CONSUMABLE_PLANT_MATURITY_LEVELS / 0.744f, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f),
			new Diet.Info(new HashSet<Tag>
			{
				"BeanPlantSeed"
			}, "IceBellyPoop", 1100000f, 18.570995f, BellyTuning.GERM_ID_EMMITED_ON_POOP, 1000f, false, Diet.Info.FoodType.EatSolid, true, null)
		};
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00015314 File Offset: 0x00013514
	public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos, float referenceCaloriesPerKg, float minPoopSizeInKg)
	{
		Diet diet = new Diet(diet_infos.ToArray());
		CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = referenceCaloriesPerKg * minPoopSizeInKg;
		prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}
}
