using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class SpaceTreePlant : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>
{
	// Token: 0x060007C2 RID: 1986 RVA: 0x000340B8 File Offset: 0x000322B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessVariable));
		this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.idle);
		this.growing.idle.EventTransition(GameHashes.Grow, this.growing.complete, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature)).EventTransition(GameHashes.Wilt, this.growing.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).PlayAnim((SpaceTreePlant.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshGrowingAnimation)).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.RefreshGrowingAnimationUpdate), UpdateRate.SIM_4000ms, false);
		this.growing.complete.EnterTransition(this.production, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.TrunkHasAtLeastOneBranch)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.production);
		this.growing.wilted.EventTransition(GameHashes.WiltRecover, this.growing.idle, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).PlayAnim(new Func<SpaceTreePlant.Instance, string>(SpaceTreePlant.GetGrowingStatesWiltedAnim), KAnim.PlayMode.Loop);
		this.production.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<bool>(this.ReadyForHarvest, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsTrue).ParamTransition<float>(this.Fullness, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsGTEOne).DefaultState(this.production.producing);
		this.production.producing.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).OnSignal(this.BranchWiltConditionChanged, this.production.halted, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanNOTProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.halted, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanNOTProduce)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).ToggleStatusItem(Db.Get().CreatureStatusItems.ProducingSugarWater, null).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.ProductionUpdate), UpdateRate.SIM_200ms, false);
		this.production.halted.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).EventTransition(GameHashes.TreeBranchCountChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanProduce)).OnSignal(this.BranchWiltConditionChanged, this.production.producing, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.producing, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanProduce)).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionPaused, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation));
		this.production.wilted.EventTransition(GameHashes.WiltRecover, this.production.producing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionWilted, null).PlayAnim("idle_empty", KAnim.PlayMode.Once).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree));
		this.harvest.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<float>(this.Fullness, this.harvestCompleted, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsLTOne).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree)).ToggleStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SetReadyToHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.EnablePiping)).DefaultState(this.harvest.prevented);
		this.harvest.prevented.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.manualHarvest, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi));
		this.harvest.manualHarvest.DefaultState(this.harvest.manualHarvest.awaitingForFarmer).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.ShowSkillRequiredStatusItemIfSkillMissing)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.StartHarvestWorkChore)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.prevented, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested))).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi)).WorkableCompleteTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.farmerWorkCompleted).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.CancelHarvestWorkChore)).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.HideSkillRequiredStatusItemIfSkillMissing));
		this.harvest.manualHarvest.awaitingForFarmer.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).WorkableStartTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.farmerWorking);
		this.harvest.manualHarvest.farmerWorking.WorkableStopTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.awaitingForFarmer);
		this.harvest.farmerWorkCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.DropInventory));
		this.harvest.pipes.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).PlayAnim("harvest_ready", KAnim.PlayMode.Loop).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.prevented, (SpaceTreePlant.Instance smi) => !smi.IsPipingEnabled || !SpaceTreePlant.HasPipeConnected(smi)).ParamTransition<bool>(this.PipingEnabled, this.harvest.prevented, (SpaceTreePlant.Instance smi, bool pipeEnable) => !pipeEnable || !SpaceTreePlant.HasPipeConnected(smi));
		this.harvestCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsetReadyToHarvest)).GoTo(this.production);
		this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(SpaceTreePlant.Instance smi)
		{
			if (!smi.IsWildPlanted && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
			{
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification notification = SpaceTreePlant.CreateDeathNotification(smi);
				notifier.Add(notification, "");
			}
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.Trigger(1623392196, null);
			smi.GetComponent<KBatchedAnimController>().StopAndClear();
			UnityEngine.Object.Destroy(smi.GetComponent<KBatchedAnimController>());
		}).ScheduleAction("Delayed Destroy", 0.5f, new Action<SpaceTreePlant.Instance>(SpaceTreePlant.SelfDestroy));
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x00034988 File Offset: 0x00032B88
	public Workable GetWorkable(SpaceTreePlant.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00034990 File Offset: 0x00032B90
	public static void EnablePiping(SpaceTreePlant.Instance smi)
	{
		smi.SetPipingState(true);
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00034999 File Offset: 0x00032B99
	public static void InformBranchesTrunkWantsToBreakFree(SpaceTreePlant.Instance smi)
	{
		smi.InformBranchesTrunkWantsToUnentomb();
	}

	// Token: 0x060007C6 RID: 1990 RVA: 0x000349A1 File Offset: 0x00032BA1
	public static void UnsubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.UnsubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000349A9 File Offset: 0x00032BA9
	public static void SubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.SubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x000349B1 File Offset: 0x00032BB1
	public static void RefreshFullnessVariable(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessVariable();
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x000349B9 File Offset: 0x00032BB9
	public static void ShowSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(true);
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000349C7 File Offset: 0x00032BC7
	public static void HideSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(false);
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000349D5 File Offset: 0x00032BD5
	public static void StartHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CreateHarvestChore();
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000349DD File Offset: 0x00032BDD
	public static void CancelHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CancelHarvestChore();
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000349E5 File Offset: 0x00032BE5
	public static bool HasPipeConnected(SpaceTreePlant.Instance smi)
	{
		return smi.HasPipeConnected;
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000349ED File Offset: 0x00032BED
	public static bool CanBeManuallyHarvested(SpaceTreePlant.Instance smi)
	{
		return smi.CanBeManuallyHarvested;
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x000349F5 File Offset: 0x00032BF5
	public static void SetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(true, smi, false);
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00034A0B File Offset: 0x00032C0B
	public static void UnsetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(false, smi, false);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00034A21 File Offset: 0x00032C21
	public static void RefreshFullnessAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessTreeTrunkAnimation();
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00034A29 File Offset: 0x00032C29
	public static void ProductionUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.ProduceUpdate(dt);
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00034A32 File Offset: 0x00032C32
	public static void DropInventory(SpaceTreePlant.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00034A3A File Offset: 0x00032C3A
	public static void AddHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(true);
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00034A43 File Offset: 0x00032C43
	public static void RemoveHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(false);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00034A4C File Offset: 0x00032C4C
	public static string GetGrowingStatesWiltedAnim(SpaceTreePlant.Instance smi)
	{
		return smi.GetTrunkWiltAnimation();
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00034A54 File Offset: 0x00032C54
	public static void RefreshGrowingAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00034A5C File Offset: 0x00032C5C
	public static void RefreshGrowingAnimationUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00034A64 File Offset: 0x00032C64
	public static bool TrunkHasAtLeastOneBranch(SpaceTreePlant.Instance smi)
	{
		return smi.HasAtLeastOneBranch;
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00034A6C File Offset: 0x00032C6C
	public static bool IsTrunkMature(SpaceTreePlant.Instance smi)
	{
		return smi.IsMature;
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00034A74 File Offset: 0x00032C74
	public static bool IsTrunkWilted(SpaceTreePlant.Instance smi)
	{
		return smi.IsWilting;
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00034A7C File Offset: 0x00032C7C
	public static bool CanNOTProduce(SpaceTreePlant.Instance smi)
	{
		return !SpaceTreePlant.CanProduce(smi);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00034A87 File Offset: 0x00032C87
	public static void PlayHarvestReadyOnUntentombed(SpaceTreePlant.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayHarvestReadyAnimation();
		}
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x00034A97 File Offset: 0x00032C97
	public static void SelfDestroy(SpaceTreePlant.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x00034AA4 File Offset: 0x00032CA4
	public static bool CanProduce(SpaceTreePlant.Instance smi)
	{
		return !smi.IsUprooted && !smi.IsWilting && smi.IsMature && !smi.IsReadyForHarvest && smi.HasAtLeastOneHealthyFullyGrownBranch();
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00034AD0 File Offset: 0x00032CD0
	public static Notification CreateDeathNotification(SpaceTreePlant.Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + smi.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04000580 RID: 1408
	public const float WILD_PLANTED_SUGAR_WATER_PRODUCTION_SPEED_MODIFIER = 4f;

	// Token: 0x04000581 RID: 1409
	public static Tag SpaceTreeReadyForHarvest = TagManager.Create("SpaceTreeReadyForHarvest");

	// Token: 0x04000582 RID: 1410
	public const string GROWN_WILT_ANIM_NAME = "idle_empty";

	// Token: 0x04000583 RID: 1411
	public const string WILT_ANIM_NAME = "wilt";

	// Token: 0x04000584 RID: 1412
	public const string GROW_ANIM_NAME = "grow";

	// Token: 0x04000585 RID: 1413
	public const string GROW_PST_ANIM_NAME = "grow_pst";

	// Token: 0x04000586 RID: 1414
	public const string FILL_ANIM_NAME = "grow_fill";

	// Token: 0x04000587 RID: 1415
	public const string MANUAL_HARVEST_READY_ANIM_NAME = "harvest_ready";

	// Token: 0x04000588 RID: 1416
	private const int FILLING_ANIMATION_FRAME_COUNT = 42;

	// Token: 0x04000589 RID: 1417
	private const int WILT_LEVELS = 3;

	// Token: 0x0400058A RID: 1418
	private const float PIPING_ENABLE_TRESHOLD = 0.25f;

	// Token: 0x0400058B RID: 1419
	public const SimHashes ProductElement = SimHashes.SugarWater;

	// Token: 0x0400058C RID: 1420
	public SpaceTreePlant.GrowingState growing;

	// Token: 0x0400058D RID: 1421
	public SpaceTreePlant.ProductionStates production;

	// Token: 0x0400058E RID: 1422
	public SpaceTreePlant.HarvestStates harvest;

	// Token: 0x0400058F RID: 1423
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State harvestCompleted;

	// Token: 0x04000590 RID: 1424
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State dead;

	// Token: 0x04000591 RID: 1425
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter ReadyForHarvest;

	// Token: 0x04000592 RID: 1426
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter PipingEnabled;

	// Token: 0x04000593 RID: 1427
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.FloatParameter Fullness;

	// Token: 0x04000594 RID: 1428
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchWiltConditionChanged;

	// Token: 0x04000595 RID: 1429
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchGrownStatusChanged;

	// Token: 0x020010CC RID: 4300
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005E01 RID: 24065
		public int OptimalAmountOfBranches;

		// Token: 0x04005E02 RID: 24066
		public float OptimalProductionDuration;
	}

	// Token: 0x020010CD RID: 4301
	public class GrowingState : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x04005E03 RID: 24067
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State idle;

		// Token: 0x04005E04 RID: 24068
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State complete;

		// Token: 0x04005E05 RID: 24069
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;
	}

	// Token: 0x020010CE RID: 4302
	public class ProductionStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x04005E06 RID: 24070
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x04005E07 RID: 24071
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State halted;

		// Token: 0x04005E08 RID: 24072
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State producing;
	}

	// Token: 0x020010CF RID: 4303
	public class HarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x04005E09 RID: 24073
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x04005E0A RID: 24074
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State prevented;

		// Token: 0x04005E0B RID: 24075
		public SpaceTreePlant.ManualHarvestStates manualHarvest;

		// Token: 0x04005E0C RID: 24076
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorkCompleted;

		// Token: 0x04005E0D RID: 24077
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State pipes;
	}

	// Token: 0x020010D0 RID: 4304
	public class ManualHarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State
	{
		// Token: 0x04005E0E RID: 24078
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State awaitingForFarmer;

		// Token: 0x04005E0F RID: 24079
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorking;
	}

	// Token: 0x020010D1 RID: 4305
	public new class Instance : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.GameInstance
	{
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06007CFF RID: 31999 RVA: 0x0030733D File Offset: 0x0030553D
		public float OptimalProductionDuration
		{
			get
			{
				if (!this.IsWildPlanted)
				{
					return base.def.OptimalProductionDuration;
				}
				return base.def.OptimalProductionDuration * 4f;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06007D00 RID: 32000 RVA: 0x00307364 File Offset: 0x00305564
		public float CurrentProductionProgress
		{
			get
			{
				return base.sm.Fullness.Get(this);
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06007D01 RID: 32001 RVA: 0x00307377 File Offset: 0x00305577
		public bool IsWilting
		{
			get
			{
				return base.gameObject.HasTag(GameTags.Wilting);
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06007D02 RID: 32002 RVA: 0x00307389 File Offset: 0x00305589
		public bool IsMature
		{
			get
			{
				return this.growingComponent.IsGrown();
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06007D03 RID: 32003 RVA: 0x00307396 File Offset: 0x00305596
		public bool HasAtLeastOneBranch
		{
			get
			{
				return this.BranchCount > 0;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06007D04 RID: 32004 RVA: 0x003073A1 File Offset: 0x003055A1
		public bool IsReadyForHarvest
		{
			get
			{
				return base.sm.ReadyForHarvest.Get(base.smi);
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06007D05 RID: 32005 RVA: 0x003073B9 File Offset: 0x003055B9
		public bool CanBeManuallyHarvested
		{
			get
			{
				return this.UserAllowsHarvest && !this.HasPipeConnected;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06007D06 RID: 32006 RVA: 0x003073CE File Offset: 0x003055CE
		public bool UserAllowsHarvest
		{
			get
			{
				return this.harvestDesignatable == null || (this.harvestDesignatable.HarvestWhenReady && this.harvestDesignatable.MarkedForHarvest);
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x06007D07 RID: 32007 RVA: 0x003073FA File Offset: 0x003055FA
		public bool HasPipeConnected
		{
			get
			{
				return this.conduitDispenser.IsConnected;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x06007D08 RID: 32008 RVA: 0x00307407 File Offset: 0x00305607
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06007D09 RID: 32009 RVA: 0x00307424 File Offset: 0x00305624
		public bool IsWildPlanted
		{
			get
			{
				return !this.receptacleMonitor.Replanted;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06007D0A RID: 32010 RVA: 0x00307434 File Offset: 0x00305634
		public bool IsEntombed
		{
			get
			{
				return this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06007D0B RID: 32011 RVA: 0x0030744B File Offset: 0x0030564B
		public bool IsPipingEnabled
		{
			get
			{
				return base.sm.PipingEnabled.Get(this);
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06007D0C RID: 32012 RVA: 0x0030745E File Offset: 0x0030565E
		public int BranchCount
		{
			get
			{
				if (this.tree != null)
				{
					return this.tree.CurrentBranchCount;
				}
				return 0;
			}
		}

		// Token: 0x06007D0D RID: 32013 RVA: 0x00307475 File Offset: 0x00305675
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06007D0E RID: 32014 RVA: 0x0030747D File Offset: 0x0030567D
		public Instance(IStateMachineTarget master, SpaceTreePlant.Def def) : base(master, def)
		{
		}

		// Token: 0x06007D0F RID: 32015 RVA: 0x00307488 File Offset: 0x00305688
		public override void StartSM()
		{
			this.tree = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
			this.tree.ActionPerBranch(new Action<GameObject>(this.SubscribeToBranchCallbacks));
			this.tree.Subscribe(-1586842875, new Action<object>(this.SubscribeToNewBranches));
			this.entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			base.StartSM();
			this.SetPipingState(this.IsPipingEnabled);
			this.RefreshFullnessVariable();
			SpaceTreeSyrupHarvestWorkable spaceTreeSyrupHarvestWorkable = this.workable;
			spaceTreeSyrupHarvestWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(spaceTreeSyrupHarvestWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnManualHarvestWorkableStateChanges));
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x00307529 File Offset: 0x00305729
		private void OnManualHarvestWorkableStateChanges(Workable workable, Workable.WorkableEvent workableEvent)
		{
			if (workableEvent == Workable.WorkableEvent.WorkStarted)
			{
				this.InformBranchesTrunkIsBeingHarvestedManually();
				return;
			}
			if (workableEvent == Workable.WorkableEvent.WorkStopped)
			{
				this.InformBranchesTrunkIsNoLongerBeingHarvestedManually();
			}
		}

		// Token: 0x06007D11 RID: 32017 RVA: 0x00307540 File Offset: 0x00305740
		private void SubscribeToNewBranches(object obj)
		{
			if (obj == null)
			{
				return;
			}
			PlantBranch.Instance instance = (PlantBranch.Instance)obj;
			this.SubscribeToBranchCallbacks(instance.gameObject);
		}

		// Token: 0x06007D12 RID: 32018 RVA: 0x00307564 File Offset: 0x00305764
		private void SubscribeToBranchCallbacks(GameObject branch)
		{
			branch.Subscribe(-724860998, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(712767498, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(-254803949, new Action<object>(this.OnBranchGrowStatusChanged));
		}

		// Token: 0x06007D13 RID: 32019 RVA: 0x003075B9 File Offset: 0x003057B9
		private void OnBranchGrowStatusChanged(object obj)
		{
			base.sm.BranchGrownStatusChanged.Trigger(this);
		}

		// Token: 0x06007D14 RID: 32020 RVA: 0x003075CC File Offset: 0x003057CC
		private void OnBranchWiltStateChanged(object obj)
		{
			base.sm.BranchWiltConditionChanged.Trigger(this);
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x003075DF File Offset: 0x003057DF
		public void SubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.tree.Subscribe(-1586842875, new Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x003075FD File Offset: 0x003057FD
		public void UnsubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.tree.Unsubscribe(-1586842875, new Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x0030761B File Offset: 0x0030581B
		private void OnNewBranchSpawnedWhileTreeIsReadyForHarvest(object data)
		{
			if (data == null)
			{
				return;
			}
			((PlantBranch.Instance)data).gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x00307636 File Offset: 0x00305836
		public void SetPipingState(bool enable)
		{
			base.sm.PipingEnabled.Set(enable, this, false);
			this.SetConduitDispenserAbilityToDispense(enable);
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x00307653 File Offset: 0x00305853
		private void SetConduitDispenserAbilityToDispense(bool canDispense)
		{
			this.conduitDispenser.SetOnState(canDispense);
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x00307664 File Offset: 0x00305864
		public void SetReadyForHarvestTag(bool isReady)
		{
			if (isReady)
			{
				base.gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
			else
			{
				base.gameObject.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
		}

		// Token: 0x06007D1B RID: 32027 RVA: 0x003076FC File Offset: 0x003058FC
		public bool HasAtLeastOneHealthyFullyGrownBranch()
		{
			if (this.tree == null || this.BranchCount <= 0)
			{
				return false;
			}
			bool healthyGrownBranchFound = false;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					healthyGrownBranchFound = (healthyGrownBranchFound || (smi.IsBranchFullyGrown && !smi.wiltCondition.IsWilting()));
				}
			});
			return healthyGrownBranchFound;
		}

		// Token: 0x06007D1C RID: 32028 RVA: 0x00307748 File Offset: 0x00305948
		public void CreateHarvestChore()
		{
			if (this.harvestChore == null)
			{
				this.harvestChore = new WorkChore<SpaceTreeSyrupHarvestWorkable>(Db.Get().ChoreTypes.Harvest, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x0030778E File Offset: 0x0030598E
		public void CancelHarvestChore()
		{
			if (this.harvestChore != null)
			{
				this.harvestChore.Cancel("SpaceTreeSyrupProduction.CancelHarvestChore()");
				this.harvestChore = null;
			}
		}

		// Token: 0x06007D1E RID: 32030 RVA: 0x003077B0 File Offset: 0x003059B0
		public void ProduceUpdate(float dt)
		{
			float mass = Mathf.Min(dt / base.smi.OptimalProductionDuration * base.smi.GetProductionSpeed() * this.storage.capacityKg, this.storage.RemainingCapacity());
			float lowTemp = ElementLoader.GetElement(SimHashes.SugarWater.CreateTag()).lowTemp;
			float num = 8f;
			float temperature = Mathf.Max(this.pe.Temperature, lowTemp + num);
			this.storage.AddLiquid(SimHashes.SugarWater, mass, temperature, byte.MaxValue, 0, false, true);
		}

		// Token: 0x06007D1F RID: 32031 RVA: 0x00307840 File Offset: 0x00305A40
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject in list)
			{
				Vector3 position = gameObject.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
			}
		}

		// Token: 0x06007D20 RID: 32032 RVA: 0x003078D0 File Offset: 0x00305AD0
		public void PlayHarvestReadyAnimation()
		{
			if (this.animController != null)
			{
				this.animController.Play("harvest_ready", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06007D21 RID: 32033 RVA: 0x00307900 File Offset: 0x00305B00
		public void InformBranchesTrunkIsBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(2137182770, null);
			});
		}

		// Token: 0x06007D22 RID: 32034 RVA: 0x0030792C File Offset: 0x00305B2C
		public void InformBranchesTrunkIsNoLongerBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-808006162, null);
			});
		}

		// Token: 0x06007D23 RID: 32035 RVA: 0x00307958 File Offset: 0x00305B58
		public void InformBranchesTrunkWantsToUnentomb()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(570354093, null);
			});
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x00307984 File Offset: 0x00305B84
		public void RefreshFullnessVariable()
		{
			float fullness = this.storage.MassStored() / this.storage.capacityKg;
			base.sm.Fullness.Set(fullness, this, false);
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-824970674, fullness);
			});
			if (fullness < 0.25f)
			{
				this.SetPipingState(false);
			}
		}

		// Token: 0x06007D25 RID: 32037 RVA: 0x003079F8 File Offset: 0x00305BF8
		public float GetProductionSpeed()
		{
			if (this.tree == null)
			{
				return 0f;
			}
			float totalProduction = 0f;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					totalProduction += smi.Productivity;
				}
			});
			return totalProduction / (float)base.def.OptimalAmountOfBranches;
		}

		// Token: 0x06007D26 RID: 32038 RVA: 0x00307A50 File Offset: 0x00305C50
		public string GetTrunkWiltAnimation()
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(this.growing.PercentOfCurrentHarvest() / 0.33333334f), 0, 2);
			return "wilt" + (num + 1).ToString();
		}

		// Token: 0x06007D27 RID: 32039 RVA: 0x00307A90 File Offset: 0x00305C90
		public void RefreshFullnessTreeTrunkAnimation()
		{
			int num = Mathf.FloorToInt(this.CurrentProductionProgress * 42f);
			if (this.animController.currentAnim != "grow_fill")
			{
				this.animController.Play("grow_fill", KAnim.PlayMode.Paused, 1f, 0f);
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
				this.animController.enabled = false;
				this.animController.enabled = true;
				return;
			}
			if (this.animController.currentFrame != num)
			{
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
			}
		}

		// Token: 0x06007D28 RID: 32040 RVA: 0x00307B34 File Offset: 0x00305D34
		public void RefreshGrowingAnimation()
		{
			this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
		}

		// Token: 0x04005E10 RID: 24080
		[MyCmpReq]
		private ReceptacleMonitor receptacleMonitor;

		// Token: 0x04005E11 RID: 24081
		[MyCmpReq]
		private KBatchedAnimController animController;

		// Token: 0x04005E12 RID: 24082
		[MyCmpReq]
		private Growing growingComponent;

		// Token: 0x04005E13 RID: 24083
		[MyCmpReq]
		private ConduitDispenser conduitDispenser;

		// Token: 0x04005E14 RID: 24084
		[MyCmpReq]
		private Storage storage;

		// Token: 0x04005E15 RID: 24085
		[MyCmpReq]
		private SpaceTreeSyrupHarvestWorkable workable;

		// Token: 0x04005E16 RID: 24086
		[MyCmpGet]
		private PrimaryElement pe;

		// Token: 0x04005E17 RID: 24087
		[MyCmpGet]
		private HarvestDesignatable harvestDesignatable;

		// Token: 0x04005E18 RID: 24088
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x04005E19 RID: 24089
		[MyCmpGet]
		private Growing growing;

		// Token: 0x04005E1A RID: 24090
		private PlantBranchGrower.Instance tree;

		// Token: 0x04005E1B RID: 24091
		private UnstableEntombDefense.Instance entombDefenseSMI;

		// Token: 0x04005E1C RID: 24092
		private Chore harvestChore;
	}
}
