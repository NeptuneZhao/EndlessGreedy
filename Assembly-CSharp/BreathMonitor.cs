using System;
using Klei.AI;
using TUNING;

// Token: 0x0200096F RID: 2415
public class BreathMonitor : GameStateMachine<BreathMonitor, BreathMonitor.Instance>
{
	// Token: 0x060046C9 RID: 18121 RVA: 0x00194B9C File Offset: 0x00192D9C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DefaultState(this.satisfied.full).Transition(this.lowbreath, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsLowBreath), UpdateRate.SIM_200ms);
		this.satisfied.full.Transition(this.satisfied.notfull, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsNotFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.HideBreathBar));
		this.satisfied.notfull.Transition(this.satisfied.full, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar));
		this.lowbreath.DefaultState(this.lowbreath.nowheretorecover).Transition(this.satisfied, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.RecoverBreath, new Func<BreathMonitor.Instance, bool>(BreathMonitor.IsNotInBreathableArea)).ToggleUrge(Db.Get().Urges.RecoverBreath).ToggleThought(Db.Get().Thoughts.Suffocating, null).ToggleTag(GameTags.HoldingBreath).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).Update(new Action<BreathMonitor.Instance, float>(BreathMonitor.UpdateRecoverBreathCell), UpdateRate.RENDER_1000ms, true);
		this.lowbreath.nowheretorecover.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.recoveryavailable, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsValidRecoverCell));
		this.lowbreath.recoveryavailable.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.nowheretorecover, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsNotValidRecoverCell)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).ToggleChore(new Func<BreathMonitor.Instance, Chore>(BreathMonitor.CreateRecoverBreathChore), this.lowbreath.nowheretorecover);
	}

	// Token: 0x060046CA RID: 18122 RVA: 0x00194D94 File Offset: 0x00192F94
	private static bool IsLowBreath(BreathMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
		if (!(myWorld == null) && myWorld.AlertManager.IsRedAlert())
		{
			return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}
		return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.RETREAT_AMOUNT;
	}

	// Token: 0x060046CB RID: 18123 RVA: 0x00194E01 File Offset: 0x00193001
	private static Chore CreateRecoverBreathChore(BreathMonitor.Instance smi)
	{
		return new RecoverBreathChore(smi.master);
	}

	// Token: 0x060046CC RID: 18124 RVA: 0x00194E0E File Offset: 0x0019300E
	private static bool IsNotFullBreath(BreathMonitor.Instance smi)
	{
		return !BreathMonitor.IsFullBreath(smi);
	}

	// Token: 0x060046CD RID: 18125 RVA: 0x00194E19 File Offset: 0x00193019
	private static bool IsFullBreath(BreathMonitor.Instance smi)
	{
		return smi.breath.value >= smi.breath.GetMax();
	}

	// Token: 0x060046CE RID: 18126 RVA: 0x00194E36 File Offset: 0x00193036
	private static bool IsNotInBreathableArea(BreathMonitor.Instance smi)
	{
		return smi.breather.IsSuffocating || !smi.breather.IsBreathableElementAtCell(Grid.PosToCell(smi), null);
	}

	// Token: 0x060046CF RID: 18127 RVA: 0x00194E5C File Offset: 0x0019305C
	private static void ShowBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, new Func<float>(smi.GetBreath), true);
		}
	}

	// Token: 0x060046D0 RID: 18128 RVA: 0x00194E88 File Offset: 0x00193088
	private static void HideBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, null, false);
		}
	}

	// Token: 0x060046D1 RID: 18129 RVA: 0x00194EA9 File Offset: 0x001930A9
	private static bool IsValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return cell != Grid.InvalidCell;
	}

	// Token: 0x060046D2 RID: 18130 RVA: 0x00194EB6 File Offset: 0x001930B6
	private static bool IsNotValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return !BreathMonitor.IsValidRecoverCell(smi, cell);
	}

	// Token: 0x060046D3 RID: 18131 RVA: 0x00194EC2 File Offset: 0x001930C2
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi, float dt)
	{
		BreathMonitor.UpdateRecoverBreathCell(smi);
	}

	// Token: 0x060046D4 RID: 18132 RVA: 0x00194ECC File Offset: 0x001930CC
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi)
	{
		if (smi.canRecoverBreath)
		{
			smi.query.Reset();
			smi.navigator.RunQuery(smi.query);
			int num = smi.query.GetResultCell();
			if (!smi.breather.IsBreathableElementAtCell(num, null))
			{
				num = PathFinder.InvalidCell;
			}
			smi.sm.recoverBreathCell.Set(num, smi, false);
		}
	}

	// Token: 0x04002E1D RID: 11805
	public BreathMonitor.SatisfiedState satisfied;

	// Token: 0x04002E1E RID: 11806
	public BreathMonitor.LowBreathState lowbreath;

	// Token: 0x04002E1F RID: 11807
	public StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell;

	// Token: 0x020018FF RID: 6399
	public class LowBreathState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007815 RID: 30741
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover;

		// Token: 0x04007816 RID: 30742
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryavailable;
	}

	// Token: 0x02001900 RID: 6400
	public class SatisfiedState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04007817 RID: 30743
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full;

		// Token: 0x04007818 RID: 30744
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull;
	}

	// Token: 0x02001901 RID: 6401
	public new class Instance : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009AD2 RID: 39634 RVA: 0x0036E3E8 File Offset: 0x0036C5E8
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
			this.breather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x06009AD3 RID: 39635 RVA: 0x0036E460 File Offset: 0x0036C660
		public int GetRecoverCell()
		{
			return base.sm.recoverBreathCell.Get(base.smi);
		}

		// Token: 0x06009AD4 RID: 39636 RVA: 0x0036E478 File Offset: 0x0036C678
		public float GetBreath()
		{
			return this.breath.value / this.breath.GetMax();
		}

		// Token: 0x04007819 RID: 30745
		public AmountInstance breath;

		// Token: 0x0400781A RID: 30746
		public SafetyQuery query;

		// Token: 0x0400781B RID: 30747
		public Navigator navigator;

		// Token: 0x0400781C RID: 30748
		public OxygenBreather breather;

		// Token: 0x0400781D RID: 30749
		public bool canRecoverBreath = true;
	}
}
