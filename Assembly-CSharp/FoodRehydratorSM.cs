using System;

// Token: 0x020006D6 RID: 1750
public class FoodRehydratorSM : GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>
{
	// Token: 0x06002C51 RID: 11345 RVA: 0x000F8EC0 File Offset: 0x000F70C0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional);
		this.off.PlayAnim("off", KAnim.PlayMode.Loop).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional).EventTransition(GameHashes.FunctionalChanged, this.on, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsFunctional);
		this.on.PlayAnim("on", KAnim.PlayMode.Loop).EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.active, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsActive).EventTransition(GameHashes.FunctionalChanged, this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EventTransition(GameHashes.ActiveChanged, this.active, (FoodRehydratorSM.StatesInstance smi) => smi.operational.IsActive);
		this.active.EnterTransition(this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EnterTransition(this.on, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsActive).EventTransition(GameHashes.FunctionalChanged, this.off, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsFunctional).EventTransition(GameHashes.ActiveChanged, this.postactive, (FoodRehydratorSM.StatesInstance smi) => !smi.operational.IsActive);
		this.postactive.OnAnimQueueComplete(this.on);
	}

	// Token: 0x04001997 RID: 6551
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State off;

	// Token: 0x04001998 RID: 6552
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State on;

	// Token: 0x04001999 RID: 6553
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State active;

	// Token: 0x0400199A RID: 6554
	private GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.State postactive;

	// Token: 0x020014E0 RID: 5344
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014E1 RID: 5345
	public class StatesInstance : GameStateMachine<FoodRehydratorSM, FoodRehydratorSM.StatesInstance, IStateMachineTarget, FoodRehydratorSM.Def>.GameInstance
	{
		// Token: 0x06008C6B RID: 35947 RVA: 0x0033A7E1 File Offset: 0x003389E1
		public StatesInstance(IStateMachineTarget master, FoodRehydratorSM.Def def) : base(master, def)
		{
		}

		// Token: 0x04006B38 RID: 27448
		[MyCmpReq]
		public Operational operational;
	}
}
