﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000A7 RID: 167
public static class BaseMooConfig
{
	// Token: 0x06000308 RID: 776 RVA: 0x000180A8 File Offset: 0x000162A8
	public static GameObject BaseMoo(string id, string name, string desc, string traitId, string anim_file, bool is_baby, string symbol_override_prefix)
	{
		float mass = 50f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 2, 2, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Prey, traitId, "FlyerNavGrid2x2", NavType.Hover, 32, 2f, "Meat", 10, true, true, 223.15f, 323.15f, 73.149994f, 473.15f);
		if (!string.IsNullOrEmpty(symbol_override_prefix))
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbol_override_prefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Moo"];
		pickupable.sortOrder = sortOrder;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Flyer, false);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		gameObject.AddOrGetDef<BeckoningMonitor.Def>().caloriesPerCycle = MooTuning.WELLFED_CALORIES_PER_CYCLE;
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.BleachStone.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		gameObject.AddOrGetDef<ThreatMonitor.Def>();
		gameObject.AddOrGetDef<SubmergedMonitor.Def>();
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		gameObject.AddOrGetDef<RanchableMonitor.Def>();
		gameObject.AddOrGetDef<FixedCapturableMonitor.Def>();
		MilkProductionMonitor.Def def = gameObject.AddOrGetDef<MilkProductionMonitor.Def>();
		def.CaloriesPerCycle = MooTuning.WELLFED_CALORIES_PER_CYCLE;
		def.Capacity = MooTuning.MILK_CAPACITY;
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new TrappedStates.Def(), true, -1).Add(new BaggedStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).PushInterruptGroup().Add(new BeckonFromSpaceStates.Def
		{
			prefab = GassyMooCometConfig.ID
		}, true, -1).Add(new CreatureSleepStates.Def(), true, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def
		{
			WaitCellOffset = 2
		}, true, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def
		{
			shouldBeBehindMilkTank = false,
			drinkCellOffsetGetFn = new DrinkMilkStates.Def.DrinkCellOffsetGetFn(DrinkMilkStates.Def.DrinkCellOffsetGet_GassyMoo)
		}, true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP), true, -1).Add(new MoveToLureStates.Def(), true, -1).Add(new CritterCondoStates.Def
		{
			working_anim = "cc_working_moo"
		}, !is_baby, -1).PopInterruptGroup().Add(new IdleStates.Def
		{
			customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseMooConfig.CustomIdleAnim)
		}, true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.MooSpecies, symbol_override_prefix);
		gameObject.AddOrGetDef<CritterCondoInteractMontior.Def>().condoPrefabTag = "AirBorneCritterCondo";
		return gameObject;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x00018398 File Offset: 0x00016598
	public static GameObject SetupDiet(GameObject prefab, Tag consumed_tag, Tag producedTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced, float minPoopSizeInKg)
	{
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				consumed_tag
			}, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null)
		});
		CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = minPoopSizeInKg * caloriesPerKg;
		prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x000183F8 File Offset: 0x000165F8
	private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
	{
		CreatureCalorieMonitor.Instance smi2 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
		return (smi2 != null && smi2.stomach.IsReadyToPoop()) ? "idle_loop_full" : "idle_loop";
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00018430 File Offset: 0x00016630
	public static void OnSpawn(GameObject inst)
	{
		Navigator component = inst.GetComponent<Navigator>();
		component.transitionDriver.overrideLayers.Add(new FullPuftTransitionLayer(component));
	}
}
