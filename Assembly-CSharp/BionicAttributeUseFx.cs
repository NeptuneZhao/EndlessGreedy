using System;
using UnityEngine;

// Token: 0x02000637 RID: 1591
public class BionicAttributeUseFx : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance>
{
	// Token: 0x0600271B RID: 10011 RVA: 0x000DE70C File Offset: 0x000DC90C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.fx);
		this.root.OnSignal(this.wasProductive, this.productive, (BionicAttributeUseFx.Instance smi) => smi.GetCurrentState() != smi.sm.pst).OnSignal(this.destroyFX, this.pst);
		this.pre.PlayAnim("bionic_upgrade_active_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		this.idle.PlayAnim("bionic_upgrade_active_loop", KAnim.PlayMode.Loop);
		this.productive.QueueAnim("bionic_upgrade_active_achievement", false, null).OnAnimQueueComplete(this.idle);
		this.pst.PlayAnim("bionic_upgrade_active_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BionicAttributeUseFx.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001668 RID: 5736
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal wasProductive;

	// Token: 0x04001669 RID: 5737
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal destroyFX;

	// Token: 0x0400166A RID: 5738
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x0400166B RID: 5739
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pre;

	// Token: 0x0400166C RID: 5740
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x0400166D RID: 5741
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State productive;

	// Token: 0x0400166E RID: 5742
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pst;

	// Token: 0x02001414 RID: 5140
	public new class Instance : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600894E RID: 35150 RVA: 0x0033026C File Offset: 0x0032E46C
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("bionic_upgrade_active_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x0600894F RID: 35151 RVA: 0x003302CE File Offset: 0x0032E4CE
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
			base.smi.StopSM("destroyed");
		}
	}
}
