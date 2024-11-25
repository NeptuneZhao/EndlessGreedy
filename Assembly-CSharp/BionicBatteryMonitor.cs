using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000967 RID: 2407
public class BionicBatteryMonitor : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>
{
	// Token: 0x0600466F RID: 18031 RVA: 0x00192AF4 File Offset: 0x00190CF4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.firstSpawn;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.RefreshCharge));
		this.firstSpawn.ParamTransition<bool>(this.InitialElectrobanksSpawned, this.online, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsTrue).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SpawnAndInstallInitialElectrobanks));
		this.online.TriggerOnEnter(GameHashes.BionicOnline, null).Transition(this.offline, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.DoesNotHaveCharge), UpdateRate.SIM_200ms).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.ReorganizeElectrobankStorage)).Update(new Action<BionicBatteryMonitor.Instance, float>(BionicBatteryMonitor.DischargeUpdate), UpdateRate.SIM_200ms, false).DefaultState(this.online.idle);
		this.online.idle.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.OnStorageChange, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep));
		this.online.batterySaveMode.EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).EventTransition(GameHashes.ScheduleChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).DefaultState(this.online.batterySaveMode.idle).Exit(delegate(BionicBatteryMonitor.Instance smi)
		{
			smi.master.Trigger(-120107884, null);
		});
		this.online.batterySaveMode.idle.ToggleChore((BionicBatteryMonitor.Instance smi) => new BeInBatterySaveModeChore(smi.master), this.online.idle, this.online.batterySaveMode.failed).DefaultState(this.online.batterySaveMode.idle.normal);
		this.online.batterySaveMode.idle.normal.ParamTransition<int>(this.ChargedElectrobankCount, this.online.batterySaveMode.idle.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int);
		this.online.batterySaveMode.idle.critical.ParamTransition<int>(this.ChargedElectrobankCount, this.online.batterySaveMode.idle.normal, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTOne_Int).OnSignal(this.OnClosestAvailableElectrobankChangedSignal, this.online.batterySaveMode.idle.exit, new Func<BionicBatteryMonitor.Instance, bool>(BionicBatteryMonitor.IsCriticallyLowAndThereIsAvailableElectrobank));
		this.online.batterySaveMode.idle.exit.ScheduleGoTo(10f, this.online.idle);
		this.online.batterySaveMode.failed.EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime))).GoTo(this.online.batterySaveMode.idle);
		this.online.upkeep.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleChanged, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.batterySaveMode, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.IsBatterySaveModeTime)).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.OnStorageChange, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).DefaultState(this.online.upkeep.emptyDepletedElectrobank);
		this.online.upkeep.emptyDepletedElectrobank.ParamTransition<int>(this.DepletedElectrobankCount, this.online.upkeep.seekElectrobank, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsZero_Int).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new RemoveDischargedElectrobankChore(smi.master), null).ToggleUrge(Db.Get().Urges.RemoveDischargedElectrobank);
		this.online.upkeep.seekElectrobank.ParamTransition<int>(this.DepletedElectrobankCount, this.online.upkeep.emptyDepletedElectrobank, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Parameter<int>.Callback(BionicBatteryMonitor.DoesNotHaveSpaceForElectrobankAndHasDepletedBatteriesStored)).EventTransition(GameHashes.OnStorageChange, this.online.upkeep.emptyDepletedElectrobank, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToGetRidOfDepletedBattery)).ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), this.online.idle);
		this.online.critical.DefaultState(this.online.critical.seekElectrobank).ParamTransition<int>(this.ChargedElectrobankCount, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTOne_Int).DoTutorial(Tutorial.TutorialMessages.TM_BionicBattery);
		this.online.critical.seekElectrobank.EventTransition(GameHashes.ScheduleBlocksChanged, this.online.batterySaveMode, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.IsBatterySaveModeTime(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EventTransition(GameHashes.ScheduleChanged, this.online.batterySaveMode, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.IsBatterySaveModeTime(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.batterySaveMode, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.IsBatterySaveModeTime(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EnterTransition(this.online.critical.emptyDepletedElectrobank, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.HasSpaceForNewElectrobank))).EnterTransition(this.online.critical.emptyDepletedElectrobank, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).EventTransition(GameHashes.TargetElectrobankLost, this.online.critical.emptyDepletedElectrobank, (BionicBatteryMonitor.Instance smi) => BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi) && !BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi)).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), null).ToggleUrge(Db.Get().Urges.ReloadElectrobank);
		this.online.critical.emptyDepletedElectrobank.ParamTransition<int>(this.DepletedElectrobankCount, this.online.critical.seekElectrobank, (BionicBatteryMonitor.Instance smi, int v) => BionicBatteryMonitor.HasSpaceForNewElectrobank(smi) && (BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi) || v == 0)).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new RemoveDischargedElectrobankChore(smi.master), null).ToggleUrge(Db.Get().Urges.RemoveDischargedElectrobank);
		this.offline.DefaultState(this.offline.waitingForBatteryDelivery).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new BeOfflineChore(smi.master), null).ToggleUrge(Db.Get().Urges.BeOffline).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOffline)).TriggerOnEnter(GameHashes.BionicOffline, null);
		this.offline.waitingForBatteryDelivery.ParamTransition<int>(this.ChargedElectrobankCount, this.offline.waitingForBatteryInstallation, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTZero_Int).Toggle("Enable Delivery of new Electrobanks", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnableManualDelivery), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisableManualDelivery));
		this.offline.waitingForBatteryInstallation.Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.StartReanimateWorkChore)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.CancelReanimateWorkChore)).WorkableCompleteTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.reboot).DefaultState(this.offline.waitingForBatteryInstallation.waiting);
		this.offline.waitingForBatteryInstallation.waiting.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWaitingForReboot, null).WorkableStartTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.working);
		this.offline.waitingForBatteryInstallation.working.WorkableStopTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.waiting);
		this.offline.reboot.PlayAnim("power_up").OnAnimQueueComplete(this.online).ScheduleGoTo(10f, this.online).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.AutomaticallyDropAllDepletedElectrobanks)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOnline));
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x00193593 File Offset: 0x00191793
	public static ReanimateBionicWorkable GetReanimateWorkable(BionicBatteryMonitor.Instance smi)
	{
		return smi.reanimateWorkable;
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x0019359B File Offset: 0x0019179B
	public static float CurrentCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge;
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x001935A3 File Offset: 0x001917A3
	public static bool IsCriticallyLowAndThereIsAvailableElectrobank(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.IsCriticallyLow(smi) && BionicBatteryMonitor.IsAnyElectrobankAvailableToBeFetched(smi);
	}

	// Token: 0x06004673 RID: 18035 RVA: 0x001935B5 File Offset: 0x001917B5
	public static bool IsBatterySaveModeTime(BionicBatteryMonitor.Instance smi)
	{
		return smi.InBatterySaveModeTime;
	}

	// Token: 0x06004674 RID: 18036 RVA: 0x001935BD File Offset: 0x001917BD
	public static bool HasCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge > 0f;
	}

	// Token: 0x06004675 RID: 18037 RVA: 0x001935CC File Offset: 0x001917CC
	public static bool DoesNotHaveCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge <= 0f;
	}

	// Token: 0x06004676 RID: 18038 RVA: 0x001935DE File Offset: 0x001917DE
	public static bool IsCriticallyLow(BionicBatteryMonitor.Instance smi)
	{
		return smi.ChargedElectrobankCount <= 1;
	}

	// Token: 0x06004677 RID: 18039 RVA: 0x001935EC File Offset: 0x001917EC
	public static bool IsAnyElectrobankAvailableToBeFetched(BionicBatteryMonitor.Instance smi)
	{
		return smi.GetClosestElectrobank() != null;
	}

	// Token: 0x06004678 RID: 18040 RVA: 0x001935FA File Offset: 0x001917FA
	public static bool WantsToGetRidOfDepletedBattery(BionicBatteryMonitor.Instance smi)
	{
		return smi.DepletedElectrobankCount > 0;
	}

	// Token: 0x06004679 RID: 18041 RVA: 0x00193605 File Offset: 0x00191805
	public static bool WantsToInstallNewBattery(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.IsCriticallyLow(smi) || (smi.InUpkeepTime && smi.ChargedElectrobankCount < 3);
	}

	// Token: 0x0600467A RID: 18042 RVA: 0x00193624 File Offset: 0x00191824
	public static bool WantsToUpkeep(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi) || BionicBatteryMonitor.WantsToInstallNewBattery(smi);
	}

	// Token: 0x0600467B RID: 18043 RVA: 0x00193636 File Offset: 0x00191836
	public static bool HasSpaceForNewElectrobank(BionicBatteryMonitor.Instance smi)
	{
		return smi.HasSpaceForNewElectrobank;
	}

	// Token: 0x0600467C RID: 18044 RVA: 0x0019363E File Offset: 0x0019183E
	public static bool DoesNotHaveSpaceForElectrobankAndHasDepletedBatteriesStored(BionicBatteryMonitor.Instance smi, int depletedAmount)
	{
		return !BionicBatteryMonitor.HasSpaceForNewElectrobank(smi) && BionicBatteryMonitor.WantsToGetRidOfDepletedBattery(smi);
	}

	// Token: 0x0600467D RID: 18045 RVA: 0x00193650 File Offset: 0x00191850
	public static void SpawnAndInstallInitialElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.SpawnAndInstallInitialElectrobanks();
	}

	// Token: 0x0600467E RID: 18046 RVA: 0x00193658 File Offset: 0x00191858
	public static void RefreshCharge(BionicBatteryMonitor.Instance smi)
	{
		smi.RefreshCharge();
	}

	// Token: 0x0600467F RID: 18047 RVA: 0x00193660 File Offset: 0x00191860
	public static void EnableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(true);
	}

	// Token: 0x06004680 RID: 18048 RVA: 0x00193669 File Offset: 0x00191869
	public static void DisableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(false);
	}

	// Token: 0x06004681 RID: 18049 RVA: 0x00193672 File Offset: 0x00191872
	public static void StartReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06004682 RID: 18050 RVA: 0x0019367A File Offset: 0x0019187A
	public static void CancelReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06004683 RID: 18051 RVA: 0x00193682 File Offset: 0x00191882
	public static void SetOffline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(false);
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x0019368B File Offset: 0x0019188B
	public static void SetOnline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(true);
	}

	// Token: 0x06004685 RID: 18053 RVA: 0x00193694 File Offset: 0x00191894
	public static void AutomaticallyDropAllDepletedElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.AutomaticallyDropAllDepletedElectrobanks();
	}

	// Token: 0x06004686 RID: 18054 RVA: 0x0019369C File Offset: 0x0019189C
	public static void ReorganizeElectrobankStorage(BionicBatteryMonitor.Instance smi)
	{
		smi.ReorganizeElectrobanks();
	}

	// Token: 0x06004687 RID: 18055 RVA: 0x001936A4 File Offset: 0x001918A4
	public static void DischargeUpdate(BionicBatteryMonitor.Instance smi, float dt)
	{
		float joules = Mathf.Min(dt * smi.Wattage, smi.CurrentCharge);
		smi.ConsumePower(joules);
	}

	// Token: 0x04002DD7 RID: 11735
	public const int MAX_ELECTROBANK_COUNT = 3;

	// Token: 0x04002DD8 RID: 11736
	public const float BATTERY_SAVE_WATTAGE = 20f;

	// Token: 0x04002DD9 RID: 11737
	public const float MIN_WATTS = 200f;

	// Token: 0x04002DDA RID: 11738
	public const float MAX_WATTS = 2000f;

	// Token: 0x04002DDB RID: 11739
	public const float EXIT_BATTERY_SAVE_MODE_TIMEOUT = 10f;

	// Token: 0x04002DDC RID: 11740
	public const string INITIAL_ELECTROBANK_TYPE_ID = "DisposableElectrobank_BasicSingleHarvestPlant";

	// Token: 0x04002DDD RID: 11741
	public static readonly string ChargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_charged_electrobank\">";

	// Token: 0x04002DDE RID: 11742
	public static readonly string DischargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_discharged_electrobank\">";

	// Token: 0x04002DDF RID: 11743
	public static readonly string CriticalBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_critical_electrobank\">";

	// Token: 0x04002DE0 RID: 11744
	public static readonly string SavingBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_saving_electrobank\">";

	// Token: 0x04002DE1 RID: 11745
	public static readonly string EmptySlotBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_empty_slot_electrobank\">";

	// Token: 0x04002DE2 RID: 11746
	private const string ANIM_NAME_REBOOT = "power_up";

	// Token: 0x04002DE3 RID: 11747
	public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State firstSpawn;

	// Token: 0x04002DE4 RID: 11748
	public BionicBatteryMonitor.OnlineStates online;

	// Token: 0x04002DE5 RID: 11749
	public BionicBatteryMonitor.OfflineStates offline;

	// Token: 0x04002DE6 RID: 11750
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter ChargedElectrobankCount;

	// Token: 0x04002DE7 RID: 11751
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter DepletedElectrobankCount;

	// Token: 0x04002DE8 RID: 11752
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnClosestAvailableElectrobankChangedSignal;

	// Token: 0x04002DE9 RID: 11753
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter InitialElectrobanksSpawned;

	// Token: 0x04002DEA RID: 11754
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter IsOnline;

	// Token: 0x020018D6 RID: 6358
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018D7 RID: 6359
	public struct WattageModifier
	{
		// Token: 0x060099FF RID: 39423 RVA: 0x0036BF46 File Offset: 0x0036A146
		public WattageModifier(string id, string name, float value, float potentialValue)
		{
			this.id = id;
			this.name = name;
			this.value = value;
			this.potentialValue = potentialValue;
		}

		// Token: 0x04007790 RID: 30608
		public float potentialValue;

		// Token: 0x04007791 RID: 30609
		public float value;

		// Token: 0x04007792 RID: 30610
		public string name;

		// Token: 0x04007793 RID: 30611
		public string id;
	}

	// Token: 0x020018D8 RID: 6360
	public class OnlineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x04007794 RID: 30612
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State idle;

		// Token: 0x04007795 RID: 30613
		public BionicBatteryMonitor.BatterySaveModeStates batterySaveMode;

		// Token: 0x04007796 RID: 30614
		public BionicBatteryMonitor.UpkeepStates upkeep;

		// Token: 0x04007797 RID: 30615
		public BionicBatteryMonitor.UpkeepStates critical;
	}

	// Token: 0x020018D9 RID: 6361
	public class BatterySaveModeStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x04007798 RID: 30616
		public BionicBatteryMonitor.BatterySaveModeIdleStates idle;

		// Token: 0x04007799 RID: 30617
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State failed;
	}

	// Token: 0x020018DA RID: 6362
	public class BatterySaveModeIdleStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x0400779A RID: 30618
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State normal;

		// Token: 0x0400779B RID: 30619
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State critical;

		// Token: 0x0400779C RID: 30620
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State exit;
	}

	// Token: 0x020018DB RID: 6363
	public class UpkeepStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x0400779D RID: 30621
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State emptyDepletedElectrobank;

		// Token: 0x0400779E RID: 30622
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State seekElectrobank;
	}

	// Token: 0x020018DC RID: 6364
	public class OfflineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x0400779F RID: 30623
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waitingForBatteryDelivery;

		// Token: 0x040077A0 RID: 30624
		public BionicBatteryMonitor.RebootWorkableState waitingForBatteryInstallation;

		// Token: 0x040077A1 RID: 30625
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State reboot;
	}

	// Token: 0x020018DD RID: 6365
	public class RebootWorkableState : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040077A2 RID: 30626
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waiting;

		// Token: 0x040077A3 RID: 30627
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State working;
	}

	// Token: 0x020018DE RID: 6366
	public new class Instance : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.GameInstance
	{
		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06009A06 RID: 39430 RVA: 0x0036BF95 File Offset: 0x0036A195
		public float Wattage
		{
			get
			{
				return this.GetBaseWattage() + this.GetModifiersWattage();
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06009A07 RID: 39431 RVA: 0x0036BFA4 File Offset: 0x0036A1A4
		public bool IsBatterySaveModeActive
		{
			get
			{
				return this.prefabID.HasTag(GameTags.BatterySaveMode);
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06009A08 RID: 39432 RVA: 0x0036BFB6 File Offset: 0x0036A1B6
		public bool IsOnline
		{
			get
			{
				return base.sm.IsOnline.Get(this);
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06009A09 RID: 39433 RVA: 0x0036BFC9 File Offset: 0x0036A1C9
		public bool InBatterySaveModeTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep);
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06009A0A RID: 39434 RVA: 0x0036BFE5 File Offset: 0x0036A1E5
		public bool InUpkeepTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06009A0B RID: 39435 RVA: 0x0036C001 File Offset: 0x0036A201
		public bool HaveInitialElectrobanksBeenSpawned
		{
			get
			{
				return base.sm.InitialElectrobanksSpawned.Get(this);
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06009A0C RID: 39436 RVA: 0x0036C014 File Offset: 0x0036A214
		public bool HasSpaceForNewElectrobank
		{
			get
			{
				return this.ChargedElectrobankCount + this.DepletedElectrobankCount < 3;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06009A0D RID: 39437 RVA: 0x0036C026 File Offset: 0x0036A226
		public int ChargedElectrobankCount
		{
			get
			{
				return base.sm.ChargedElectrobankCount.Get(this);
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06009A0E RID: 39438 RVA: 0x0036C039 File Offset: 0x0036A239
		public int DepletedElectrobankCount
		{
			get
			{
				return (int)(this.storage.GetMassAvailable("EmptyElectrobank") / 20f);
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06009A0F RID: 39439 RVA: 0x0036C057 File Offset: 0x0036A257
		public int DamagedElectrobankCount
		{
			get
			{
				return (int)(this.storage.GetMassAvailable("GarbageElectrobank") / 20f);
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06009A10 RID: 39440 RVA: 0x0036C075 File Offset: 0x0036A275
		public float CurrentCharge
		{
			get
			{
				return this.BionicBattery.value;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06009A12 RID: 39442 RVA: 0x0036C08B File Offset: 0x0036A28B
		// (set) Token: 0x06009A11 RID: 39441 RVA: 0x0036C082 File Offset: 0x0036A282
		public ReanimateBionicWorkable reanimateWorkable { get; private set; }

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06009A14 RID: 39444 RVA: 0x0036C09C File Offset: 0x0036A29C
		// (set) Token: 0x06009A13 RID: 39443 RVA: 0x0036C093 File Offset: 0x0036A293
		public List<BionicBatteryMonitor.WattageModifier> Modifiers { get; set; } = new List<BionicBatteryMonitor.WattageModifier>();

		// Token: 0x06009A15 RID: 39445 RVA: 0x0036C0A4 File Offset: 0x0036A2A4
		public Instance(IStateMachineTarget master, BionicBatteryMonitor.Def def) : base(master, def)
		{
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicBatteryStorage);
			this.reanimateWorkable = base.GetComponent<ReanimateBionicWorkable>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.manualDelivery = base.GetComponent<ManualDeliveryKG>();
			this.selectable = base.GetComponent<KSelectable>();
			this.prefabID = base.GetComponent<KPrefabID>();
			this.BionicBattery = Db.Get().Amounts.BionicInternalBattery.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeath));
			Storage storage2 = this.storage;
			storage2.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage2.OnStorageChange, new Action<GameObject>(this.OnElectrobankStorageChanged));
		}

		// Token: 0x06009A16 RID: 39446 RVA: 0x0036C1A4 File Offset: 0x0036A3A4
		public override void StartSM()
		{
			this.closestElectrobankSensor = base.GetComponent<Sensors>().GetSensor<ClosestElectrobankSensor>();
			ClosestElectrobankSensor closestElectrobankSensor = this.closestElectrobankSensor;
			closestElectrobankSensor.OnItemChanged = (Action<Electrobank>)Delegate.Combine(closestElectrobankSensor.OnItemChanged, new Action<Electrobank>(this.OnClosestElectrobankChanged));
			base.StartSM();
			this.RefreshCharge();
		}

		// Token: 0x06009A17 RID: 39447 RVA: 0x0036C1F5 File Offset: 0x0036A3F5
		private void OnClosestElectrobankChanged(Electrobank newItem)
		{
			base.sm.OnClosestAvailableElectrobankChangedSignal.Trigger(this);
		}

		// Token: 0x06009A18 RID: 39448 RVA: 0x0036C208 File Offset: 0x0036A408
		public float GetBaseWattage()
		{
			if (!this.IsBatterySaveModeActive)
			{
				return 200f;
			}
			return 20f;
		}

		// Token: 0x06009A19 RID: 39449 RVA: 0x0036C220 File Offset: 0x0036A420
		public float GetModifiersWattage()
		{
			float num = 0f;
			foreach (BionicBatteryMonitor.WattageModifier wattageModifier in this.Modifiers)
			{
				num += wattageModifier.value;
			}
			return num;
		}

		// Token: 0x06009A1A RID: 39450 RVA: 0x0036C27C File Offset: 0x0036A47C
		private void OnElectrobankStorageChanged(object o)
		{
			this.ReorganizeElectrobanks();
		}

		// Token: 0x06009A1B RID: 39451 RVA: 0x0036C284 File Offset: 0x0036A484
		public void ReorganizeElectrobanks()
		{
			this.storage.items.Sort(delegate(GameObject b1, GameObject b2)
			{
				Electrobank component = b1.GetComponent<Electrobank>();
				Electrobank component2 = b2.GetComponent<Electrobank>();
				if (component == null)
				{
					return -1;
				}
				if (component2 == null)
				{
					return 1;
				}
				return component.Charge.CompareTo(component2.Charge);
			});
		}

		// Token: 0x06009A1C RID: 39452 RVA: 0x0036C2B8 File Offset: 0x0036A4B8
		public void CreateWorkableChore()
		{
			if (this.reanimateChore == null)
			{
				this.reanimateChore = new WorkChore<ReanimateBionicWorkable>(Db.Get().ChoreTypes.RescueIncapacitated, this.reanimateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06009A1D RID: 39453 RVA: 0x0036C2FE File Offset: 0x0036A4FE
		public void CancelWorkChore()
		{
			if (this.reanimateChore != null)
			{
				this.reanimateChore.Cancel("BionicBatteryMonitor.CancelChore");
				this.reanimateChore = null;
			}
		}

		// Token: 0x06009A1E RID: 39454 RVA: 0x0036C31F File Offset: 0x0036A51F
		public void SetOnlineState(bool online)
		{
			base.sm.IsOnline.Set(online, this, false);
			this.RefreshCharge();
		}

		// Token: 0x06009A1F RID: 39455 RVA: 0x0036C33C File Offset: 0x0036A53C
		public void SetManualDeliveryEnableState(bool enable)
		{
			if (!enable)
			{
				this.manualDelivery.capacity = 0f;
				this.manualDelivery.refillMass = 0f;
				this.manualDelivery.RequestedItemTag = null;
				this.manualDelivery.AbortDelivery("Manual delivery disabled");
				return;
			}
			base.smi.storage.capacityKg = 3f;
			base.smi.manualDelivery.capacity = 1f;
			base.smi.manualDelivery.refillMass = 1f;
			base.smi.manualDelivery.MinimumMass = 1f;
			this.manualDelivery.RequestedItemTag = GameTags.ChargedPortableBattery;
		}

		// Token: 0x06009A20 RID: 39456 RVA: 0x0036C3F2 File Offset: 0x0036A5F2
		public GameObject GetFirstDischargedElectrobankInInventory()
		{
			return this.storage.FindFirst(GameTags.EmptyPortableBattery);
		}

		// Token: 0x06009A21 RID: 39457 RVA: 0x0036C404 File Offset: 0x0036A604
		public Electrobank GetClosestElectrobank()
		{
			return this.closestElectrobankSensor.GetItem();
		}

		// Token: 0x06009A22 RID: 39458 RVA: 0x0036C414 File Offset: 0x0036A614
		public void RefreshCharge()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			this.storage.Find(GameTags.EmptyPortableBattery, list2);
			float num = 0f;
			if (this.IsOnline)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Electrobank component = list[i].GetComponent<Electrobank>();
					num += component.Charge;
				}
			}
			this.BionicBattery.SetValue(num);
			base.sm.ChargedElectrobankCount.Set(list.Count, this, false);
			base.sm.DepletedElectrobankCount.Set(list2.Count, this, false);
			this.UpdateNotifications();
		}

		// Token: 0x06009A23 RID: 39459 RVA: 0x0036C4D0 File Offset: 0x0036A6D0
		public void ConsumePower(float joules)
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			float num = joules;
			for (int i = 0; i < list.Count; i++)
			{
				Electrobank component = list[i].GetComponent<Electrobank>();
				float joules2 = Mathf.Min(component.Charge, num);
				float num2 = component.RemovePower(joules2, false);
				num -= num2;
				WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountConsumed(component.ID, num2);
			}
			this.RefreshCharge();
		}

		// Token: 0x06009A24 RID: 39460 RVA: 0x0036C54C File Offset: 0x0036A74C
		public void DebugAddCharge(float joules)
		{
			float num = MathF.Min(joules, 360000f - this.CurrentCharge);
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			int num2 = 0;
			while (num > 0f && num2 < list.Count)
			{
				Electrobank component = list[num2].GetComponent<Electrobank>();
				float num3 = Mathf.Min(120000f - component.Charge, num);
				component.AddPower(num3);
				num -= num3;
				num2++;
			}
			if (num > 0f && list.Count < 3)
			{
				int num4 = this.storage.items.Count - 1;
				while (num > 0f && num4 >= 0)
				{
					GameObject gameObject = this.storage.items[num4];
					if (!(gameObject == null))
					{
						Electrobank component2 = gameObject.GetComponent<Electrobank>();
						if (component2 == null && gameObject.HasTag(GameTags.EmptyPortableBattery))
						{
							this.storage.Drop(gameObject, true);
							GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), base.transform.position);
							gameObject2.SetActive(true);
							component2 = gameObject2.GetComponent<Electrobank>();
							float joules2 = Mathf.Clamp(component2.Charge - num, 0f, float.MaxValue);
							component2.RemovePower(joules2, true);
							num -= component2.Charge;
							this.storage.Store(gameObject2, false, false, true, false);
						}
					}
					num4--;
				}
			}
			if (num > 0f && this.storage.items.Count < 3)
			{
				do
				{
					GameObject gameObject3 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), base.transform.position);
					gameObject3.SetActive(true);
					Electrobank component3 = gameObject3.GetComponent<Electrobank>();
					float joules3 = Mathf.Clamp(component3.Charge - num, 0f, float.MaxValue);
					component3.RemovePower(joules3, true);
					num -= component3.Charge;
					this.storage.Store(gameObject3, false, false, true, false);
				}
				while (num > 0f && this.storage.items.Count < 3 && num > 0f);
			}
			this.RefreshCharge();
		}

		// Token: 0x06009A25 RID: 39461 RVA: 0x0036C79C File Offset: 0x0036A99C
		private void UpdateNotifications()
		{
			this.criticalBatteryStatusItemGuid = this.selectable.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicCriticalBattery, this.criticalBatteryStatusItemGuid, BionicBatteryMonitor.IsCriticallyLow(base.smi) && !this.prefabID.HasTag(GameTags.Incapacitated), base.gameObject);
		}

		// Token: 0x06009A26 RID: 39462 RVA: 0x0036C7F8 File Offset: 0x0036A9F8
		public bool AddOrUpdateModifier(BionicBatteryMonitor.WattageModifier modifier, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifier.id);
			bool flag;
			if (num >= 0)
			{
				flag = (this.Modifiers[num].name != modifier.name || this.Modifiers[num].value != modifier.value || this.Modifiers[num].potentialValue != modifier.potentialValue);
				this.Modifiers[num] = modifier;
			}
			else
			{
				this.Modifiers.Add(modifier);
				flag = true;
			}
			if (flag)
			{
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
			}
			if (triggerCallbacks)
			{
				base.Trigger(1361471071, this.Wattage);
			}
			return flag;
		}

		// Token: 0x06009A27 RID: 39463 RVA: 0x0036C904 File Offset: 0x0036AB04
		public bool RemoveModifier(string modifierID, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifierID);
			if (num >= 0)
			{
				this.Modifiers.RemoveAt(num);
				if (triggerCallbacks)
				{
					base.Trigger(1361471071, this.Wattage);
				}
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
				return true;
			}
			return false;
		}

		// Token: 0x06009A28 RID: 39464 RVA: 0x0036C98C File Offset: 0x0036AB8C
		private void OnBatteriesDroppedFromDeath(List<GameObject> items)
		{
			if (items != null)
			{
				for (int i = 0; i < items.Count; i++)
				{
					Electrobank component = items[i].GetComponent<Electrobank>();
					if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
					{
						component.RemovePower(component.Charge, true);
					}
				}
			}
		}

		// Token: 0x06009A29 RID: 39465 RVA: 0x0036C9E8 File Offset: 0x0036ABE8
		public void SpawnAndInstallInitialElectrobanks()
		{
			for (int i = 0; i < 3; i++)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), base.transform.position);
				gameObject.SetActive(true);
				this.storage.Store(gameObject, false, false, true, false);
			}
			this.RefreshCharge();
			this.SetOnlineState(true);
			base.sm.InitialElectrobanksSpawned.Set(true, this, false);
		}

		// Token: 0x06009A2A RID: 39466 RVA: 0x0036CA5C File Offset: 0x0036AC5C
		public void AutomaticallyDropAllDepletedElectrobanks()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.EmptyPortableBattery, list);
			for (int i = 0; i < list.Count; i++)
			{
				GameObject go = list[i];
				this.storage.Drop(go, true);
			}
		}

		// Token: 0x040077A4 RID: 30628
		public Storage storage;

		// Token: 0x040077A5 RID: 30629
		public KPrefabID prefabID;

		// Token: 0x040077A7 RID: 30631
		private Schedulable schedulable;

		// Token: 0x040077A8 RID: 30632
		private AmountInstance BionicBattery;

		// Token: 0x040077A9 RID: 30633
		private ManualDeliveryKG manualDelivery;

		// Token: 0x040077AA RID: 30634
		private ClosestElectrobankSensor closestElectrobankSensor;

		// Token: 0x040077AB RID: 30635
		private KSelectable selectable;

		// Token: 0x040077AC RID: 30636
		private Guid criticalBatteryStatusItemGuid;

		// Token: 0x040077AE RID: 30638
		private Chore reanimateChore;
	}
}
