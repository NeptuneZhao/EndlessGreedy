using System;
using UnityEngine;

// Token: 0x0200063E RID: 1598
public class SuperProductiveFX : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance>
{
	// Token: 0x0600272F RID: 10031 RVA: 0x000DF3F4 File Offset: 0x000DD5F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.fx);
		this.root.OnSignal(this.wasProductive, this.productive, (SuperProductiveFX.Instance smi) => smi.GetCurrentState() != smi.sm.pst).OnSignal(this.destroyFX, this.pst);
		this.pre.PlayAnim("productive_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		this.idle.PlayAnim("productive_loop", KAnim.PlayMode.Loop);
		this.productive.QueueAnim("productive_achievement", false, null).OnAnimQueueComplete(this.idle);
		this.pst.PlayAnim("productive_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(SuperProductiveFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x0400167F RID: 5759
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal wasProductive;

	// Token: 0x04001680 RID: 5760
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal destroyFX;

	// Token: 0x04001681 RID: 5761
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001682 RID: 5762
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pre;

	// Token: 0x04001683 RID: 5763
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04001684 RID: 5764
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State productive;

	// Token: 0x04001685 RID: 5765
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pst;

	// Token: 0x02001422 RID: 5154
	public new class Instance : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008984 RID: 35204 RVA: 0x00330ADC File Offset: 0x0032ECDC
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("productive_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06008985 RID: 35205 RVA: 0x00330B3E File Offset: 0x0032ED3E
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
			base.smi.StopSM("destroyed");
		}
	}
}
