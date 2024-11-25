using System;
using TUNING;

// Token: 0x02000992 RID: 2450
public class PeeChoreMonitor : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance>
{
	// Token: 0x06004778 RID: 18296 RVA: 0x00198DB4 File Offset: 0x00196FB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.building;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.building.Update(delegate(PeeChoreMonitor.Instance smi, float dt)
		{
			this.pee_fuse.Delta(-dt, smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.paused, (PeeChoreMonitor.Instance smi) => this.IsSleeping(smi), UpdateRate.SIM_200ms).Transition(this.critical, (PeeChoreMonitor.Instance smi) => this.pee_fuse.Get(smi) <= 60f, UpdateRate.SIM_200ms);
		this.critical.Update(delegate(PeeChoreMonitor.Instance smi, float dt)
		{
			this.pee_fuse.Delta(-dt, smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.paused, (PeeChoreMonitor.Instance smi) => this.IsSleeping(smi), UpdateRate.SIM_200ms).Transition(this.pee, (PeeChoreMonitor.Instance smi) => this.pee_fuse.Get(smi) <= 0f, UpdateRate.SIM_200ms).Toggle("Components", delegate(PeeChoreMonitor.Instance smi)
		{
			Components.CriticalBladders.Add(smi);
		}, delegate(PeeChoreMonitor.Instance smi)
		{
			Components.CriticalBladders.Remove(smi);
		});
		this.paused.Transition(this.building, (PeeChoreMonitor.Instance smi) => !this.IsSleeping(smi), UpdateRate.SIM_200ms);
		this.pee.ToggleChore(new Func<PeeChoreMonitor.Instance, Chore>(this.CreatePeeChore), this.building);
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x00198EEC File Offset: 0x001970EC
	private bool IsSleeping(PeeChoreMonitor.Instance smi)
	{
		StaminaMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StaminaMonitor.Instance>();
		if (smi2 != null)
		{
			smi2.IsSleeping();
		}
		return false;
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x00198F15 File Offset: 0x00197115
	private Chore CreatePeeChore(PeeChoreMonitor.Instance smi)
	{
		return new PeeChore(smi.master);
	}

	// Token: 0x04002EA5 RID: 11941
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State building;

	// Token: 0x04002EA6 RID: 11942
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State critical;

	// Token: 0x04002EA7 RID: 11943
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State paused;

	// Token: 0x04002EA8 RID: 11944
	public GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.State pee;

	// Token: 0x04002EA9 RID: 11945
	private StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter pee_fuse = new StateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.FloatParameter(DUPLICANTSTATS.STANDARD.Secretions.PEE_FUSE_TIME);

	// Token: 0x0200195D RID: 6493
	public new class Instance : GameStateMachine<PeeChoreMonitor, PeeChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C48 RID: 40008 RVA: 0x00372015 File Offset: 0x00370215
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009C49 RID: 40009 RVA: 0x0037201E File Offset: 0x0037021E
		public bool IsCritical()
		{
			return base.IsInsideState(base.sm.critical);
		}
	}
}
