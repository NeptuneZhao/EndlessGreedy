using System;

// Token: 0x020000EA RID: 234
public class MoltStatesChore : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>
{
	// Token: 0x0600043A RID: 1082 RVA: 0x00021E00 File Offset: 0x00020000
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.molting;
		this.molting.PlayAnim((MoltStatesChore.Instance smi) => smi.def.moltAnimName, KAnim.PlayMode.Once).ScheduleGoTo(5f, this.complete).OnAnimQueueComplete(this.complete);
		this.complete.BehaviourComplete(GameTags.Creatures.ReadyToMolt, false);
	}

	// Token: 0x040002DD RID: 733
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State molting;

	// Token: 0x040002DE RID: 734
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State complete;

	// Token: 0x02001081 RID: 4225
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005CF6 RID: 23798
		public string moltAnimName;
	}

	// Token: 0x02001082 RID: 4226
	public new class Instance : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.GameInstance
	{
		// Token: 0x06007C24 RID: 31780 RVA: 0x00304C2A File Offset: 0x00302E2A
		public Instance(Chore<MoltStatesChore.Instance> chore, MoltStatesChore.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ReadyToMolt);
		}
	}
}
