using System;
using UnityEngine;

// Token: 0x0200063D RID: 1597
public class SicknessCuredFX : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance>
{
	// Token: 0x0600272D RID: 10029 RVA: 0x000DF388 File Offset: 0x000DD588
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).Exit("DestroyFX", delegate(SicknessCuredFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x0400167E RID: 5758
	public StateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02001420 RID: 5152
	public new class Instance : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600897F RID: 35199 RVA: 0x00330A40 File Offset: 0x0032EC40
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("recentlyhealed_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06008980 RID: 35200 RVA: 0x00330AA2 File Offset: 0x0032ECA2
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
