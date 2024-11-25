using System;

// Token: 0x02000822 RID: 2082
public class SubmergedMonitor : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>
{
	// Token: 0x06003991 RID: 14737 RVA: 0x00139EEC File Offset: 0x001380EC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Enter("SetNavType", delegate(SubmergedMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover);
		}).Update("SetNavType", delegate(SubmergedMonitor.Instance smi, float dt)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover);
		}, UpdateRate.SIM_1000ms, false).Transition(this.submerged, (SubmergedMonitor.Instance smi) => smi.IsSubmerged(), UpdateRate.SIM_1000ms);
		this.submerged.Enter("SetNavType", delegate(SubmergedMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim);
		}).Update("SetNavType", delegate(SubmergedMonitor.Instance smi, float dt)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim);
		}, UpdateRate.SIM_1000ms, false).Transition(this.satisfied, (SubmergedMonitor.Instance smi) => !smi.IsSubmerged(), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Submerged);
	}

	// Token: 0x040022A2 RID: 8866
	public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State satisfied;

	// Token: 0x040022A3 RID: 8867
	public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State submerged;

	// Token: 0x0200173C RID: 5948
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200173D RID: 5949
	public new class Instance : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.GameInstance
	{
		// Token: 0x06009502 RID: 38146 RVA: 0x0035E8FC File Offset: 0x0035CAFC
		public Instance(IStateMachineTarget master, SubmergedMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009503 RID: 38147 RVA: 0x0035E906 File Offset: 0x0035CB06
		public bool IsSubmerged()
		{
			return Grid.IsSubstantialLiquid(Grid.PosToCell(base.transform.GetPosition()), 0.35f);
		}
	}
}
