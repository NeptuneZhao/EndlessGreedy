using System;

// Token: 0x020000E8 RID: 232
public class MinionChoreBrokerStates : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>
{
	// Token: 0x06000434 RID: 1076 RVA: 0x00021C60 File Offset: 0x0001FE60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.hasChore;
		this.root.DoNothing();
		this.hasChore.Enter(delegate(MinionChoreBrokerStates.Instance smi)
		{
		});
	}

	// Token: 0x040002D9 RID: 729
	private GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.State hasChore;

	// Token: 0x0200107C RID: 4220
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200107D RID: 4221
	public new class Instance : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.GameInstance
	{
		// Token: 0x06007C1D RID: 31773 RVA: 0x00304BD6 File Offset: 0x00302DD6
		public Instance(Chore<MinionChoreBrokerStates.Instance> chore, MinionChoreBrokerStates.Def def) : base(chore, def)
		{
		}
	}
}
