using System;
using KSerialization;

// Token: 0x0200081F RID: 2079
public class RoverChoreMonitor : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>
{
	// Token: 0x0600397D RID: 14717 RVA: 0x001396E4 File Offset: 0x001378E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.ToggleBehaviour(GameTags.Creatures.Tunnel, (RoverChoreMonitor.Instance smi) => true, null).ToggleBehaviour(GameTags.Creatures.Builder, (RoverChoreMonitor.Instance smi) => true, null);
	}

	// Token: 0x04002299 RID: 8857
	public GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.State loop;

	// Token: 0x02001732 RID: 5938
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001733 RID: 5939
	public new class Instance : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.GameInstance
	{
		// Token: 0x060094EC RID: 38124 RVA: 0x0035E4C7 File Offset: 0x0035C6C7
		public Instance(IStateMachineTarget master, RoverChoreMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060094ED RID: 38125 RVA: 0x0035E4D8 File Offset: 0x0035C6D8
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x04007202 RID: 29186
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x04007203 RID: 29187
		private Action<object> OnDestinationReachedDelegate;
	}
}
