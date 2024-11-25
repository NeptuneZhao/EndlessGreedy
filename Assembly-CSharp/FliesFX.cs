using System;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class FliesFX : GameStateMachine<FliesFX, FliesFX.Instance>
{
	// Token: 0x06002722 RID: 10018 RVA: 0x000DEB6C File Offset: 0x000DCD6C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("swarm_pre").QueueAnim("swarm_loop", true, null).Exit("DestroyFX", delegate(FliesFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001672 RID: 5746
	public StateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02001419 RID: 5145
	public new class Instance : GameStateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008965 RID: 35173 RVA: 0x003304F4 File Offset: 0x0032E6F4
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("fly_swarm_kanim", base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06008966 RID: 35174 RVA: 0x00330560 File Offset: 0x0032E760
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
