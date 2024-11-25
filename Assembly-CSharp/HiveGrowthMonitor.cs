using System;

// Token: 0x020000DC RID: 220
public class HiveGrowthMonitor : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>
{
	// Token: 0x060003FF RID: 1023 RVA: 0x00020727 File Offset: 0x0001E927
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, new StateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.Transition.ConditionCallback(HiveGrowthMonitor.IsGrowing), null);
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0002074F File Offset: 0x0001E94F
	public static bool IsGrowing(HiveGrowthMonitor.Instance smi)
	{
		return !smi.GetSMI<BeeHive.StatesInstance>().IsFullyGrown();
	}

	// Token: 0x0200105B RID: 4187
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200105C RID: 4188
	public new class Instance : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.GameInstance
	{
		// Token: 0x06007BC9 RID: 31689 RVA: 0x003042A0 File Offset: 0x003024A0
		public Instance(IStateMachineTarget master, HiveGrowthMonitor.Def def) : base(master, def)
		{
		}
	}
}
