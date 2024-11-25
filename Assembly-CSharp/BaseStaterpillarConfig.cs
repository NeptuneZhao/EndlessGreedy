﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class BaseStaterpillarConfig
{
	// Token: 0x0600032F RID: 815 RVA: 0x0001AEAC File Offset: 0x000190AC
	public static GameObject BaseStaterpillar(string id, string name, string desc, string anim_file, string trait_id, bool is_baby, ObjectLayer conduitLayer, string connectorDefId, Tag inhaleTag, string symbolOverridePrefix = null, float warningLowTemperature = 283.15f, float warningHighTemperature = 293.15f, float lethalLowTemperature = 243.15f, float lethalHighTemperature = 343.15f, InhaleStates.Def inhaleDef = null)
	{
		float mass = 200f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		gameObject.AddTag(GameTags.Amphibious);
		string navGridName = "WalkerBabyNavGrid";
		if (!is_baby)
		{
			navGridName = "DreckoNavGrid";
			gameObject.AddOrGetDef<ConduitSleepMonitor.Def>().conduitLayer = conduitLayer;
		}
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, trait_id, navGridName, NavType.Floor, 32, 1f, "Meat", 2, false, false, warningLowTemperature, warningHighTemperature, lethalLowTemperature, lethalHighTemperature);
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Staterpillar"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGet<LoopingSounds>();
		Staterpillar staterpillar = gameObject.AddOrGet<Staterpillar>();
		staterpillar.conduitLayer = conduitLayer;
		staterpillar.connectorDefId = connectorDefId;
		gameObject.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
		gameObject.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("eat_pre", "eat_pst", null), !is_baby, -1).Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def
		{
			WaitCellOffset = 2
		}, !is_baby, -1).PushInterruptGroup().Add(new LayEggStates.Def(), !is_baby, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def(), true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).Add(inhaleDef, inhaleTag != Tag.Invalid, -1).Add(new ConduitSleepStates.Def(), true, -1).Add(new CallAdultStates.Def(), is_baby, -1).Add(new CritterCondoStates.Def(), !is_baby, -1).PopInterruptGroup().Add(new CreatureSleepStates.Def(), true, -1).Add(new IdleStates.Def
		{
			customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseStaterpillarConfig.CustomIdleAnim)
		}, true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.StaterpillarSpecies, symbolOverridePrefix);
		return gameObject;
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0001B17C File Offset: 0x0001937C
	public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos)
	{
		Diet diet = new Diet(diet_infos.ToArray());
		prefab.AddOrGetDef<CreatureCalorieMonitor.Def>().diet = diet;
		prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0001B1B0 File Offset: 0x000193B0
	public static List<Diet.Info> RawMetalDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		List<SimHashes> list = new List<SimHashes>
		{
			SimHashes.FoolsGold
		};
		List<Diet.Info> list2 = new List<Diet.Info>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid && element.materialCategory == GameTags.Metal && element.HasTag(GameTags.Ore) && !element.disabled && !list.Contains(element.id))
			{
				list2.Add(new Diet.Info(new HashSet<Tag>(new Tag[]
				{
					element.tag
				}), poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null));
			}
		}
		return list2;
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0001B280 File Offset: 0x00019480
	public static List<Diet.Info> RefinedMetalDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		List<Diet.Info> list = new List<Diet.Info>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid && element.materialCategory == GameTags.RefinedMetal && !element.disabled)
			{
				list.Add(new Diet.Info(new HashSet<Tag>(new Tag[]
				{
					element.tag
				}), poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced, false, Diet.Info.FoodType.EatSolid, false, null));
			}
		}
		return list;
	}

	// Token: 0x06000333 RID: 819 RVA: 0x0001B324 File Offset: 0x00019524
	private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
	{
		CellOffset offset = new CellOffset(0, -1);
		bool facing = smi.GetComponent<Facing>().GetFacing();
		NavType currentNavType = smi.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Floor)
		{
			if (currentNavType == NavType.Ceiling)
			{
				offset = (facing ? new CellOffset(1, 1) : new CellOffset(-1, 1));
			}
		}
		else
		{
			offset = (facing ? new CellOffset(1, -1) : new CellOffset(-1, -1));
		}
		HashedString result = "idle_loop";
		int num = Grid.OffsetCell(Grid.PosToCell(smi), offset);
		if (Grid.IsValidCell(num) && !Grid.Solid[num])
		{
			pre_anim = "idle_loop_hang_pre";
			result = "idle_loop_hang";
		}
		return result;
	}
}
