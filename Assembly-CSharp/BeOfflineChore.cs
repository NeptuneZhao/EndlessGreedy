using System;

// Token: 0x0200042D RID: 1069
public class BeOfflineChore : Chore<BeOfflineChore.StatesInstance>
{
	// Token: 0x060016CC RID: 5836 RVA: 0x0007A3C4 File Offset: 0x000785C4
	public static string GetPowerDownAnimPre(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down";
		}
		return "power_down";
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x0007A3F8 File Offset: 0x000785F8
	public static string GetPowerDownAnimLoop(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down_idle";
		}
		return "power_down_idle";
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x0007A42C File Offset: 0x0007862C
	public BeOfflineChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeOffline, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeOfflineChore.StatesInstance(this);
	}

	// Token: 0x04000CB6 RID: 3254
	public const string EFFECT_NAME = "BionicOffline";

	// Token: 0x020011A0 RID: 4512
	public class StatesInstance : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore, object>.GameInstance
	{
		// Token: 0x0600806C RID: 32876 RVA: 0x00310AED File Offset: 0x0030ECED
		public StatesInstance(BeOfflineChore master) : base(master)
		{
		}
	}

	// Token: 0x020011A1 RID: 4513
	public class States : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore>
	{
		// Token: 0x0600806D RID: 32877 RVA: 0x00310AF8 File Offset: 0x0030ECF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAnims("anim_bionic_kanim", 0f).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicOfflineIncapacitated, (BeOfflineChore.StatesInstance smi) => smi.master.gameObject.GetSMI<BionicBatteryMonitor.Instance>()).ToggleEffect("BionicOffline").PlayAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimPre), KAnim.PlayMode.Once).QueueAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimLoop), true, null);
		}
	}
}
