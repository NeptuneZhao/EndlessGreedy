using System;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public class BeInBatterySaveModeChore : Chore<BeInBatterySaveModeChore.StatesInstance>
{
	// Token: 0x060016C3 RID: 5827 RVA: 0x0007A15C File Offset: 0x0007835C
	public BeInBatterySaveModeChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeBatterySaveMode, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeInBatterySaveModeChore.StatesInstance(this, master.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x0007A1B4 File Offset: 0x000783B4
	public static bool IsBatteryMonitorWaitingForUsToExit(BeInBatterySaveModeChore.StatesInstance smi, float dt)
	{
		return smi.batteryMonitor.IsInsideState(smi.batteryMonitor.sm.online.batterySaveMode.idle.exit);
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x0007A1E0 File Offset: 0x000783E0
	public static string GetEnterAnim(BeInBatterySaveModeChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Ladder)
		{
		}
		return "low_power_pre";
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x0007A20C File Offset: 0x0007840C
	public static string GetIdleAnim(BeInBatterySaveModeChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Ladder)
		{
		}
		return "low_power_loop";
	}

	// Token: 0x060016C7 RID: 5831 RVA: 0x0007A238 File Offset: 0x00078438
	public static string GetExitAnim(BeInBatterySaveModeChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType != NavType.Ladder)
		{
		}
		return "low_power_pst";
	}

	// Token: 0x04000CB0 RID: 3248
	public const string EFFECT_NAME = "BionicBatterySaveMode";

	// Token: 0x0200119C RID: 4508
	public class States : GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore>
	{
		// Token: 0x06008062 RID: 32866 RVA: 0x003105F8 File Offset: 0x0030E7F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			this.root.ToggleTag(GameTags.BatterySaveMode).TriggerOnEnter(GameHashes.BionicBatterySaveModeChanged, (BeInBatterySaveModeChore.StatesInstance smi) => true).TriggerOnExit(GameHashes.BionicBatterySaveModeChanged, (BeInBatterySaveModeChore.StatesInstance smi) => false).ToggleEffect("BionicBatterySaveMode");
			this.enter.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<BeInBatterySaveModeChore.StatesInstance, string>(BeInBatterySaveModeChore.GetEnterAnim), KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.idle.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<BeInBatterySaveModeChore.StatesInstance, string>(BeInBatterySaveModeChore.GetIdleAnim), KAnim.PlayMode.Loop).UpdateTransition(this.exit, new Func<BeInBatterySaveModeChore.StatesInstance, float, bool>(BeInBatterySaveModeChore.IsBatteryMonitorWaitingForUsToExit), UpdateRate.SIM_1000ms, false);
			this.exit.ToggleAnims("anim_bionic_kanim", 0f).PlayAnim(new Func<BeInBatterySaveModeChore.StatesInstance, string>(BeInBatterySaveModeChore.GetExitAnim), KAnim.PlayMode.Once).OnAnimQueueComplete(this.end);
			this.end.ReturnSuccess();
		}

		// Token: 0x04006095 RID: 24725
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State enter;

		// Token: 0x04006096 RID: 24726
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State idle;

		// Token: 0x04006097 RID: 24727
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State exit;

		// Token: 0x04006098 RID: 24728
		public GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.State end;
	}

	// Token: 0x0200119D RID: 4509
	public class StatesInstance : GameStateMachine<BeInBatterySaveModeChore.States, BeInBatterySaveModeChore.StatesInstance, BeInBatterySaveModeChore, object>.GameInstance
	{
		// Token: 0x06008064 RID: 32868 RVA: 0x00310735 File Offset: 0x0030E935
		public StatesInstance(BeInBatterySaveModeChore master, GameObject duplicant) : base(master)
		{
			this.batteryMonitor = duplicant.GetSMI<BionicBatteryMonitor.Instance>();
		}

		// Token: 0x04006099 RID: 24729
		public BionicBatteryMonitor.Instance batteryMonitor;
	}
}
