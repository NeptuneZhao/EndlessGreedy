using System;

// Token: 0x020007FA RID: 2042
public class CritterElementMonitor : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06003873 RID: 14451 RVA: 0x001342FA File Offset: 0x001324FA
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateInElement", delegate(CritterElementMonitor.Instance smi, float dt)
		{
			smi.UpdateCurrentElement(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x020016D9 RID: 5849
	public new class Instance : GameStateMachine<CritterElementMonitor, CritterElementMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060093B7 RID: 37815 RVA: 0x00359D34 File Offset: 0x00357F34
		// (remove) Token: 0x060093B8 RID: 37816 RVA: 0x00359D6C File Offset: 0x00357F6C
		public event Action<float> OnUpdateEggChances;

		// Token: 0x060093B9 RID: 37817 RVA: 0x00359DA1 File Offset: 0x00357FA1
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060093BA RID: 37818 RVA: 0x00359DAA File Offset: 0x00357FAA
		public void UpdateCurrentElement(float dt)
		{
			this.OnUpdateEggChances(dt);
		}
	}
}
