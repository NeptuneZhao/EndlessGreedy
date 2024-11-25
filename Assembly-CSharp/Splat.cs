using System;

// Token: 0x02000B18 RID: 2840
public class Splat : GameStateMachine<Splat, Splat.StatesInstance>
{
	// Token: 0x06005490 RID: 21648 RVA: 0x001E3E7C File Offset: 0x001E207C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleChore((Splat.StatesInstance smi) => new WorkChore<SplatWorkable>(Db.Get().ChoreTypes.Mop, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.complete);
		this.complete.Enter(delegate(Splat.StatesInstance smi)
		{
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x04003765 RID: 14181
	public GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.State complete;

	// Token: 0x02001B62 RID: 7010
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001B63 RID: 7011
	public class StatesInstance : GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600A359 RID: 41817 RVA: 0x00389955 File Offset: 0x00387B55
		public StatesInstance(IStateMachineTarget master, Splat.Def def) : base(master, def)
		{
		}
	}
}
