using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200017F RID: 383
public class SpaceTreeBranch : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>
{
	// Token: 0x06000788 RID: 1928 RVA: 0x00032074 File Offset: 0x00030274
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.growing;
		this.root.EventTransition(GameHashes.Uprooted, this.die, null).EventHandler(GameHashes.Wilt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWilt)).EventHandler(GameHashes.WiltRecover, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWiltRecover));
		this.growing.InitializeStates(this.masterTarget, this.die).EnterTransition(this.grown, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).EventTransition(GameHashes.Wilt, this.halt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsWiltedConditionReportingWilted)).EventTransition(GameHashes.RootHealthChanged, this.halt, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.PlanterStorage, this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).EventTransition(GameHashes.PlanterStorage, this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, null).Update("CheckGrown", delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			if (smi.GetcurrentGrowthPercentage() >= 1f)
			{
				smi.gameObject.Trigger(-254803949, null);
				smi.GoTo(this.grown);
			}
		}, UpdateRate.SIM_4000ms, false);
		this.growing.wild.DefaultState(this.growing.wild.visible).EnterTransition(this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).ToggleAttributeModifier("GrowingWild", (SpaceTreeBranch.Instance smi) => smi.wildGrowingRate, null);
		this.growing.wild.visible.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.wild.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.hidden, false).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
		this.growing.wild.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.visible, true).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.wild.hidden), KAnim.PlayMode.Once);
		this.growing.planted.DefaultState(this.growing.planted.visible).EnterTransition(this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleAttributeModifier("Growing", (SpaceTreeBranch.Instance smi) => smi.baseGrowingRate, null);
		this.growing.planted.visible.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.planted.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.hidden, false).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
		this.growing.planted.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.visible, true).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.planted.hidden), KAnim.PlayMode.Once);
		this.halt.InitializeStates(this.masterTarget, this.die).DefaultState(this.halt.wilted).EventHandlerTransition(GameHashes.RootHealthChanged, this.growing, (SpaceTreeBranch.Instance smi, object o) => SpaceTreeBranch.IsTrunkHealthy(smi) && !SpaceTreeBranch.IsWiltedConditionReportingWilted(smi)).EventTransition(GameHashes.WiltRecover, this.growing, null).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.hidden, false);
		this.halt.wilted.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.wilted), KAnim.PlayMode.Paused).EventTransition(GameHashes.RootHealthChanged, this.halt.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.halt.trunkWilted.EventTransition(GameHashes.RootHealthChanged, this.halt.wilted, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.trunkWilted), KAnim.PlayMode.Once).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.halt.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.shaking), KAnim.PlayMode.Once).ScheduleGoTo(1.8f, this.halt.wilted);
		this.halt.hidden.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.hidden), KAnim.PlayMode.Once).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.wilted, true);
		this.grown.InitializeStates(this.masterTarget, this.die).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.AllowItToBeHarvestForWood)).EventTransition(GameHashes.Harvest, this.harvestedForWood, null).EventTransition(GameHashes.ConsumePlant, this.growing, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown))).DefaultState(this.grown.spawn);
		this.grown.spawn.EventTransition(GameHashes.Wilt, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).ParamTransition<bool>(this.HasSpawn, this.grown.healthy, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.IsTrue).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.spawn), KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.spawnPST);
		this.grown.spawnPST.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.spawnPST), KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.healthy);
		this.grown.healthy.Enter(delegate(SpaceTreeBranch.Instance smi)
		{
			this.HasSpawn.Set(true, smi, false);
		}).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableGlowFlowerMeter)).Exit(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).ToggleStatusItem(Db.Get().CreatureStatusItems.SpaceTreeBranchLightStatus, null).DefaultState(this.grown.healthy.filling);
		this.grown.healthy.filling.EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationOnUnentomb)).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).EventTransition(GameHashes.Wilt, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.trunkReadyForHarvest, false);
		this.grown.healthy.trunkReadyForHarvest.DefaultState(this.grown.healthy.trunkReadyForHarvest.idle).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.filling, true);
		this.grown.healthy.trunkReadyForHarvest.idle.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.idle), KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayReadyForHarvestAnimationOnUnentomb)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.healthy.trunkReadyForHarvest.shaking, (SpaceTreeBranch.Instance o, object smi) => true).EventTransition(GameHashes.SpaceTreeManualHarvestBegan, this.grown.healthy.trunkReadyForHarvest.harvestInProgress, null).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "harvest_ready");
		}, UpdateRate.SIM_200ms, false);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.DefaultState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre).EventTransition(GameHashes.SpaceTreeManualHarvestStopped, this.grown.healthy.trunkReadyForHarvest.idle, null);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre), KAnim.PlayMode.Once).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pre");
		}, UpdateRate.SIM_200ms, false).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Loop), UpdateRate.SIM_200ms);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop), KAnim.PlayMode.Loop).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_loop");
		}, UpdateRate.SIM_200ms, false).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Pst), UpdateRate.SIM_200ms);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst), KAnim.PlayMode.Once).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pst");
		}, UpdateRate.SIM_200ms, false);
		this.grown.healthy.trunkReadyForHarvest.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.entombDefenseSMI.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.healthy.trunkReadyForHarvest.idle);
		this.grown.trunkWilted.DefaultState(this.grown.trunkWilted.wilted).EventTransition(GameHashes.RootHealthChanged, this.grown.spawn, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).EventTransition(GameHashes.WiltRecover, this.grown.spawn, null).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.trunkReadyForHarvest, false);
		this.grown.trunkWilted.wilted.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.trunkWilted), KAnim.PlayMode.Once).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.trunkWilted.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.grown.trunkWilted.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.entombDefenseSMI.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.trunkWilted.wilted);
		this.harvestedForWood.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.harvestedForWood), KAnim.PlayMode.Once).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnHarvest)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).Exit(delegate(SpaceTreeBranch.Instance smi)
		{
			smi.Trigger(113170146, null);
		}).OnAnimQueueComplete(this.growing);
		this.die.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).DefaultState(this.die.entering);
		this.die.entering.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.die.entering), KAnim.PlayMode.Once).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnDeath)).OnAnimQueueComplete(this.die.selfDelete).ScheduleGoTo(2f, this.die.selfDelete);
		this.die.selfDelete.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SelfDestroy));
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00032DA7 File Offset: 0x00030FA7
	public static bool TransitToManualHarvest_Loop(SpaceTreeBranch.Instance smi)
	{
		return smi.GetCurrentTrunkAnim() != null && smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_loop";
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00032DD3 File Offset: 0x00030FD3
	public static bool TransitToManualHarvest_Pst(SpaceTreeBranch.Instance smi)
	{
		return smi.GetCurrentTrunkAnim() != null && smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_pst";
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00032DFF File Offset: 0x00030FFF
	public static bool IsWiltedConditionReportingWilted(SpaceTreeBranch.Instance smi)
	{
		return smi.wiltCondition.IsWilting();
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00032E0C File Offset: 0x0003100C
	public static bool IsBranchFullyGrown(SpaceTreeBranch.Instance smi)
	{
		return smi.IsBranchFullyGrown;
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00032E14 File Offset: 0x00031014
	public static bool IsTrunkWildPlanted(SpaceTreeBranch.Instance smi)
	{
		return smi.IsTrunkWildPlanted;
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00032E1C File Offset: 0x0003101C
	public static bool IsEntombed(SpaceTreeBranch.Instance smi)
	{
		return smi.IsEntombed;
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00032E24 File Offset: 0x00031024
	public static bool IsTrunkHealthy(SpaceTreeBranch.Instance smi)
	{
		return smi.IsTrunkHealthy;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00032E2C File Offset: 0x0003102C
	public static void PlayFillAnimationForThisState(SpaceTreeBranch.Instance smi)
	{
		smi.PlayFillAnimation();
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00032E34 File Offset: 0x00031034
	public static void OnTrunkSyrupFullnessChanged(SpaceTreeBranch.Instance smi, object obj)
	{
		smi.PlayFillAnimation((float)obj);
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00032E42 File Offset: 0x00031042
	public static void SynchAnimationWithTrunk(SpaceTreeBranch.Instance smi, HashedString animName)
	{
		smi.SynchCurrentAnimWithTrunkAnim(animName);
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00032E4B File Offset: 0x0003104B
	public static void EnableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
	{
		smi.ActivateGlowFlowerMeter();
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00032E53 File Offset: 0x00031053
	public static void DisableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
	{
		smi.DeactivateGlowFlowerMeter();
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x00032E5B File Offset: 0x0003105B
	public static void UpdateFlowerOnWilt(SpaceTreeBranch.Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnim_flowerWilted, KAnim.PlayMode.Loop);
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00032E6F File Offset: 0x0003106F
	public static void UpdateFlowerOnWiltRecover(SpaceTreeBranch.Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnimNames, KAnim.PlayMode.Loop);
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00032E83 File Offset: 0x00031083
	public static void EnableEntombDefenses(SpaceTreeBranch.Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(true);
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00032E91 File Offset: 0x00031091
	public static void DisableEntombDefenses(SpaceTreeBranch.Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(false);
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00032E9F File Offset: 0x0003109F
	public static void AllowItToBeHarvestForWood(SpaceTreeBranch.Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(true);
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00032EAD File Offset: 0x000310AD
	public static void ForbidBranchToBeHarvestedForWood(SpaceTreeBranch.Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(false);
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00032EBB File Offset: 0x000310BB
	public static void SpawnWoodOnHarvest(SpaceTreeBranch.Instance smi)
	{
		smi.crop.SpawnConfiguredFruit(null);
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00032EC9 File Offset: 0x000310C9
	public static void SpawnWoodOnDeath(SpaceTreeBranch.Instance smi)
	{
		if (smi.harvestable != null && smi.harvestable.CanBeHarvested)
		{
			smi.crop.SpawnConfiguredFruit(null);
		}
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00032EF2 File Offset: 0x000310F2
	public static void OnConsumed(SpaceTreeBranch.Instance smi)
	{
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00032EF4 File Offset: 0x000310F4
	public static void SelfDestroy(SpaceTreeBranch.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x00032F01 File Offset: 0x00031101
	public static void PlayFillAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			SpaceTreeBranch.PlayFillAnimationForThisState(smi);
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00032F11 File Offset: 0x00031111
	public static void PlayReadyForHarvestAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayReadyForHarvestAnimation();
		}
	}

	// Token: 0x0400055C RID: 1372
	public const int FILL_ANIM_FRAME_COUNT = 42;

	// Token: 0x0400055D RID: 1373
	public const int SHAKE_ANIM_FRAME_COUNT = 54;

	// Token: 0x0400055E RID: 1374
	public const float SHAKE_ANIM_DURATION = 1.8f;

	// Token: 0x0400055F RID: 1375
	private StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.BoolParameter HasSpawn;

	// Token: 0x04000560 RID: 1376
	private SpaceTreeBranch.GrowingStates growing;

	// Token: 0x04000561 RID: 1377
	private SpaceTreeBranch.GrowHaltState halt;

	// Token: 0x04000562 RID: 1378
	private SpaceTreeBranch.GrownStates grown;

	// Token: 0x04000563 RID: 1379
	private GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State harvestedForWood;

	// Token: 0x04000564 RID: 1380
	private SpaceTreeBranch.DieStates die;

	// Token: 0x020010BE RID: 4286
	public class AnimSet
	{
		// Token: 0x04005DB6 RID: 23990
		public string[] meterTargets;

		// Token: 0x04005DB7 RID: 23991
		public string[] meterAnimNames;

		// Token: 0x04005DB8 RID: 23992
		public string undeveloped;

		// Token: 0x04005DB9 RID: 23993
		public string spawn;

		// Token: 0x04005DBA RID: 23994
		public string spawn_pst;

		// Token: 0x04005DBB RID: 23995
		public string fill;

		// Token: 0x04005DBC RID: 23996
		public string ready_harvest;

		// Token: 0x04005DBD RID: 23997
		public string[] meterAnim_flowerWilted;

		// Token: 0x04005DBE RID: 23998
		public string wilted;

		// Token: 0x04005DBF RID: 23999
		public string wilted_short_trunk_healthy;

		// Token: 0x04005DC0 RID: 24000
		public string wilted_short_trunk_wilted;

		// Token: 0x04005DC1 RID: 24001
		public string hidden;

		// Token: 0x04005DC2 RID: 24002
		public string die;

		// Token: 0x04005DC3 RID: 24003
		public string manual_harvest_pre;

		// Token: 0x04005DC4 RID: 24004
		public string manual_harvest_loop;

		// Token: 0x04005DC5 RID: 24005
		public string manual_harvest_pst;
	}

	// Token: 0x020010BF RID: 4287
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005DC6 RID: 24006
		public int OPTIMAL_LUX_LEVELS;

		// Token: 0x04005DC7 RID: 24007
		public float GROWTH_RATE = 0.0016666667f;

		// Token: 0x04005DC8 RID: 24008
		public float WILD_GROWTH_RATE = 0.00041666668f;
	}

	// Token: 0x020010C0 RID: 4288
	public class GrowingState : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04005DC9 RID: 24009
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State visible;

		// Token: 0x04005DCA RID: 24010
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
	}

	// Token: 0x020010C1 RID: 4289
	public class GrowingStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x04005DCB RID: 24011
		public SpaceTreeBranch.GrowingState wild;

		// Token: 0x04005DCC RID: 24012
		public SpaceTreeBranch.GrowingState planted;
	}

	// Token: 0x020010C2 RID: 4290
	public class GrownStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x04005DCD RID: 24013
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawn;

		// Token: 0x04005DCE RID: 24014
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawnPST;

		// Token: 0x04005DCF RID: 24015
		public SpaceTreeBranch.HealthyStates healthy;

		// Token: 0x04005DD0 RID: 24016
		public SpaceTreeBranch.WiltStates trunkWilted;
	}

	// Token: 0x020010C3 RID: 4291
	public class GrowHaltState : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x04005DD1 RID: 24017
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;

		// Token: 0x04005DD2 RID: 24018
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State trunkWilted;

		// Token: 0x04005DD3 RID: 24019
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;

		// Token: 0x04005DD4 RID: 24020
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
	}

	// Token: 0x020010C4 RID: 4292
	public class WiltStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04005DD5 RID: 24021
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;

		// Token: 0x04005DD6 RID: 24022
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;
	}

	// Token: 0x020010C5 RID: 4293
	public class DieStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04005DD7 RID: 24023
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State entering;

		// Token: 0x04005DD8 RID: 24024
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State selfDelete;
	}

	// Token: 0x020010C6 RID: 4294
	public class ReadyForHarvest : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04005DD9 RID: 24025
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State idle;

		// Token: 0x04005DDA RID: 24026
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;

		// Token: 0x04005DDB RID: 24027
		public SpaceTreeBranch.ManualHarvestStates harvestInProgress;
	}

	// Token: 0x020010C7 RID: 4295
	public class ManualHarvestStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04005DDC RID: 24028
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pre;

		// Token: 0x04005DDD RID: 24029
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State loop;

		// Token: 0x04005DDE RID: 24030
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pst;
	}

	// Token: 0x020010C8 RID: 4296
	public class HealthyStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04005DDF RID: 24031
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State filling;

		// Token: 0x04005DE0 RID: 24032
		public SpaceTreeBranch.ReadyForHarvest trunkReadyForHarvest;
	}

	// Token: 0x020010C9 RID: 4297
	public new class Instance : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameInstance, IManageGrowingStates
	{
		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06007CCA RID: 31946 RVA: 0x0030688A File Offset: 0x00304A8A
		public int CurrentAmountOfLux
		{
			get
			{
				return Grid.LightIntensity[this.cell];
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06007CCB RID: 31947 RVA: 0x0030689C File Offset: 0x00304A9C
		public float Productivity
		{
			get
			{
				if (!this.IsBranchFullyGrown)
				{
					return 0f;
				}
				return Mathf.Clamp((float)this.CurrentAmountOfLux / (float)base.def.OPTIMAL_LUX_LEVELS, 0f, 1f);
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06007CCC RID: 31948 RVA: 0x003068CF File Offset: 0x00304ACF
		public bool IsTrunkHealthy
		{
			get
			{
				return this.trunk != null && !this.trunk.HasTag(GameTags.Wilting);
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x06007CCD RID: 31949 RVA: 0x003068EE File Offset: 0x00304AEE
		public bool IsTrunkWildPlanted
		{
			get
			{
				return this.trunk != null && !this.trunk.GetComponent<ReceptacleMonitor>().Replanted;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06007CCE RID: 31950 RVA: 0x0030690D File Offset: 0x00304B0D
		public bool IsEntombed
		{
			get
			{
				return this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06007CCF RID: 31951 RVA: 0x00306924 File Offset: 0x00304B24
		public bool IsBranchFullyGrown
		{
			get
			{
				return this.GetcurrentGrowthPercentage() >= 1f;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06007CD0 RID: 31952 RVA: 0x00306936 File Offset: 0x00304B36
		private PlantBranchGrower.Instance trunk
		{
			get
			{
				if (this._trunk == null)
				{
					this._trunk = this.branch.GetTrunk();
					if (this._trunk != null)
					{
						this.trunkAnimController = this._trunk.GetComponent<KBatchedAnimController>();
					}
				}
				return this._trunk;
			}
		}

		// Token: 0x06007CD1 RID: 31953 RVA: 0x00306970 File Offset: 0x00304B70
		public void OverrideMaturityLevel(float percent)
		{
			float value = this.maturity.GetMax() * percent;
			this.maturity.SetValue(value);
		}

		// Token: 0x06007CD2 RID: 31954 RVA: 0x00306998 File Offset: 0x00304B98
		public Instance(IStateMachineTarget master, SpaceTreeBranch.Def def) : base(master, def)
		{
			this.cell = Grid.PosToCell(this);
			Amounts amounts = base.gameObject.GetAmounts();
			this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
			this.baseGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			base.Subscribe(1272413801, new Action<object>(this.ResetGrowth));
		}

		// Token: 0x06007CD3 RID: 31955 RVA: 0x00306A5E File Offset: 0x00304C5E
		public float GetcurrentGrowthPercentage()
		{
			return this.maturity.value / this.maturity.GetMax();
		}

		// Token: 0x06007CD4 RID: 31956 RVA: 0x00306A77 File Offset: 0x00304C77
		public void ResetGrowth(object data = null)
		{
			this.maturity.value = 0f;
			base.sm.HasSpawn.Set(false, this, false);
			base.smi.gameObject.Trigger(-254803949, null);
		}

		// Token: 0x06007CD5 RID: 31957 RVA: 0x00306AB4 File Offset: 0x00304CB4
		public override void StartSM()
		{
			this.branch = base.smi.GetSMI<PlantBranch.Instance>();
			this.entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			if (this.Animations.meterTargets != null)
			{
				this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
			}
			base.StartSM();
		}

		// Token: 0x06007CD6 RID: 31958 RVA: 0x00306B14 File Offset: 0x00304D14
		public void CreateMeters(string[] meterTargets, string[] meterAnimNames)
		{
			this.flowerMeters = new MeterController[meterTargets.Length];
			for (int i = 0; i < this.flowerMeters.Length; i++)
			{
				this.flowerMeters[i] = new MeterController(this.animController, meterTargets[i], meterAnimNames[i], Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
			}
		}

		// Token: 0x06007CD7 RID: 31959 RVA: 0x00306B64 File Offset: 0x00304D64
		public void RefreshAnimation()
		{
			if (this.flowerMeters == null && this.Animations.meterTargets != null)
			{
				this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
			}
			KAnim.PlayMode mode = base.IsInsideState(base.sm.grown.healthy) ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.Play(this.GetAnimationForState(this.GetCurrentState()), mode, 1f, 0f);
			}
			if (base.IsInsideState(base.smi.sm.grown.healthy))
			{
				this.ActivateGlowFlowerMeter();
				return;
			}
			this.DeactivateGlowFlowerMeter();
		}

		// Token: 0x06007CD8 RID: 31960 RVA: 0x00306C1B File Offset: 0x00304E1B
		public HashedString GetCurrentTrunkAnim()
		{
			if (this.trunk != null && this.trunkAnimController != null)
			{
				return this.trunkAnimController.currentAnim;
			}
			return null;
		}

		// Token: 0x06007CD9 RID: 31961 RVA: 0x00306C48 File Offset: 0x00304E48
		public void SynchCurrentAnimWithTrunkAnim(HashedString trunkAnimNameToSynchTo)
		{
			if (this.trunk != null && this.trunkAnimController != null && this.trunkAnimController.currentAnim == trunkAnimNameToSynchTo)
			{
				float elapsedTime = this.trunkAnimController.GetElapsedTime();
				base.smi.animController.SetElapsedTime(elapsedTime);
			}
		}

		// Token: 0x06007CDA RID: 31962 RVA: 0x00306C9C File Offset: 0x00304E9C
		public string GetAnimationForState(StateMachine.BaseState state)
		{
			if (state == base.sm.growing.wild.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.wild.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.growing.planted.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.grown.spawn)
			{
				return this.Animations.spawn;
			}
			if (state == base.sm.grown.spawnPST)
			{
				return this.Animations.spawn_pst;
			}
			if (state == base.sm.grown.healthy.filling)
			{
				return this.Animations.fill;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.idle)
			{
				return this.Animations.ready_harvest;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre)
			{
				return this.Animations.manual_harvest_pre;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop)
			{
				return this.Animations.manual_harvest_loop;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst)
			{
				return this.Animations.manual_harvest_pst;
			}
			if (state == base.sm.grown.trunkWilted)
			{
				return this.Animations.wilted;
			}
			if (state == base.sm.halt.wilted)
			{
				return this.Animations.wilted_short_trunk_healthy;
			}
			if (state == base.sm.halt.trunkWilted)
			{
				return this.Animations.wilted_short_trunk_wilted;
			}
			if (state == base.sm.halt.shaking)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.halt.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.harvestedForWood)
			{
				return this.Animations.die;
			}
			if (state == base.sm.die.entering)
			{
				return this.Animations.die;
			}
			return this.Animations.spawn;
		}

		// Token: 0x06007CDB RID: 31963 RVA: 0x00306F30 File Offset: 0x00305130
		public string GetFillAnimNameForState(StateMachine.BaseState state)
		{
			string fill = this.Animations.fill;
			if (state == base.sm.grown.healthy.filling)
			{
				return this.Animations.fill;
			}
			if (state == base.sm.growing.wild.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.halt.wilted)
			{
				return this.Animations.wilted_short_trunk_healthy;
			}
			return fill;
		}

		// Token: 0x06007CDC RID: 31964 RVA: 0x00306FD5 File Offset: 0x003051D5
		public void PlayReadyForHarvestAnimation()
		{
			if (this.animController != null)
			{
				this.animController.Play(this.Animations.ready_harvest, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06007CDD RID: 31965 RVA: 0x0030700B File Offset: 0x0030520B
		public void PlayFillAnimation()
		{
			this.PlayFillAnimation(this.lastFillAmountRecorded);
		}

		// Token: 0x06007CDE RID: 31966 RVA: 0x0030701C File Offset: 0x0030521C
		public void PlayFillAnimation(float fillLevel)
		{
			string fillAnimNameForState = this.GetFillAnimNameForState(base.smi.GetCurrentState());
			this.lastFillAmountRecorded = fillLevel;
			if (this.entombDefenseSMI.IsEntombed && this.entombDefenseSMI.IsActive)
			{
				return;
			}
			if (this.animController != null)
			{
				int num = Mathf.FloorToInt(fillLevel * 42f);
				if (this.animController.currentAnim != fillAnimNameForState)
				{
					this.animController.Play(fillAnimNameForState, KAnim.PlayMode.Once, 0f, 0f);
				}
				if (this.animController.currentFrame != num)
				{
					this.animController.SetPositionPercent(fillLevel);
				}
			}
		}

		// Token: 0x06007CDF RID: 31967 RVA: 0x003070C8 File Offset: 0x003052C8
		public void ActivateGlowFlowerMeter()
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].gameObject.SetActive(true);
					this.flowerMeters[i].meterController.Play(this.flowerMeters[i].meterController.currentAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
		}

		// Token: 0x06007CE0 RID: 31968 RVA: 0x00307134 File Offset: 0x00305334
		public void PlayAnimOnFlower(string[] animNames, KAnim.PlayMode playMode)
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].meterController.Play(animNames[i], playMode, 1f, 0f);
				}
			}
		}

		// Token: 0x06007CE1 RID: 31969 RVA: 0x00307184 File Offset: 0x00305384
		public void DeactivateGlowFlowerMeter()
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06007CE2 RID: 31970 RVA: 0x003071BF File Offset: 0x003053BF
		public float TimeUntilNextHarvest()
		{
			return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
		}

		// Token: 0x06007CE3 RID: 31971 RVA: 0x003071E4 File Offset: 0x003053E4
		public float PercentGrown()
		{
			return this.GetcurrentGrowthPercentage();
		}

		// Token: 0x06007CE4 RID: 31972 RVA: 0x003071EC File Offset: 0x003053EC
		public Crop GetGropComponent()
		{
			return base.GetComponent<Crop>();
		}

		// Token: 0x06007CE5 RID: 31973 RVA: 0x003071F4 File Offset: 0x003053F4
		public float DomesticGrowthTime()
		{
			return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x00307212 File Offset: 0x00305412
		public float WildGrowthTime()
		{
			return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
		}

		// Token: 0x04005DE1 RID: 24033
		[MyCmpGet]
		public WiltCondition wiltCondition;

		// Token: 0x04005DE2 RID: 24034
		[MyCmpGet]
		public Crop crop;

		// Token: 0x04005DE3 RID: 24035
		[MyCmpGet]
		public Harvestable harvestable;

		// Token: 0x04005DE4 RID: 24036
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04005DE5 RID: 24037
		public SpaceTreeBranch.AnimSet Animations = new SpaceTreeBranch.AnimSet();

		// Token: 0x04005DE6 RID: 24038
		private int cell;

		// Token: 0x04005DE7 RID: 24039
		private float lastFillAmountRecorded;

		// Token: 0x04005DE8 RID: 24040
		private AmountInstance maturity;

		// Token: 0x04005DE9 RID: 24041
		public AttributeModifier baseGrowingRate;

		// Token: 0x04005DEA RID: 24042
		public AttributeModifier wildGrowingRate;

		// Token: 0x04005DEB RID: 24043
		public UnstableEntombDefense.Instance entombDefenseSMI;

		// Token: 0x04005DEC RID: 24044
		private MeterController[] flowerMeters;

		// Token: 0x04005DED RID: 24045
		private PlantBranch.Instance branch;

		// Token: 0x04005DEE RID: 24046
		private KBatchedAnimController trunkAnimController;

		// Token: 0x04005DEF RID: 24047
		private PlantBranchGrower.Instance _trunk;
	}
}
