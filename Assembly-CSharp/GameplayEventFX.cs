using System;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class GameplayEventFX : GameStateMachine<GameplayEventFX, GameplayEventFX.Instance>
{
	// Token: 0x06002724 RID: 10020 RVA: 0x000DEBDC File Offset: 0x000DCDDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("event_pre").OnAnimQueueComplete(this.single).Exit("DestroyFX", delegate(GameplayEventFX.Instance smi)
		{
			smi.DestroyFX();
		});
		this.single.PlayAnim("event_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.notificationCount, this.multiple, (GameplayEventFX.Instance smi, int p) => p > 1);
		this.multiple.PlayAnim("event_loop_multiple", KAnim.PlayMode.Loop).ParamTransition<int>(this.notificationCount, this.single, (GameplayEventFX.Instance smi, int p) => p == 1);
	}

	// Token: 0x04001673 RID: 5747
	public StateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001674 RID: 5748
	public StateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.IntParameter notificationCount;

	// Token: 0x04001675 RID: 5749
	public GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.State single;

	// Token: 0x04001676 RID: 5750
	public GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.State multiple;

	// Token: 0x0200141B RID: 5147
	public new class Instance : GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600896A RID: 35178 RVA: 0x0033059C File Offset: 0x0032E79C
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("event_alert_fx_kanim", base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x0600896B RID: 35179 RVA: 0x00330608 File Offset: 0x0032E808
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040068DC RID: 26844
		public int previousCount;
	}
}
