using System;

// Token: 0x020007F9 RID: 2041
public class CreatureSleepMonitor : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
	// Token: 0x06003870 RID: 14448 RVA: 0x001342BE File Offset: 0x001324BE
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.SleepBehaviour, new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep), null);
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x001342E6 File Offset: 0x001324E6
	public static bool ShouldSleep(CreatureSleepMonitor.Instance smi)
	{
		return GameClock.Instance.IsNighttime();
	}

	// Token: 0x020016D7 RID: 5847
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020016D8 RID: 5848
	public new class Instance : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance
	{
		// Token: 0x060093B6 RID: 37814 RVA: 0x00359D2A File Offset: 0x00357F2A
		public Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
