using System;

// Token: 0x02000AF2 RID: 2802
public class ClusterMapFXAnimator : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x0600539C RID: 21404 RVA: 0x001DF5C0 File Offset: 0x001DD7C0
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.play;
		this.play.OnSignal(this.onAnimComplete, this.finished);
		this.finished.Enter(delegate(ClusterMapFXAnimator.StatesInstance smi)
		{
			smi.DestroyEntity();
		});
	}

	// Token: 0x04003708 RID: 14088
	private KBatchedAnimController animController;

	// Token: 0x04003709 RID: 14089
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x0400370A RID: 14090
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State play;

	// Token: 0x0400370B RID: 14091
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State finished;

	// Token: 0x0400370C RID: 14092
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.Signal onAnimComplete;

	// Token: 0x02001B4B RID: 6987
	public class StatesInstance : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600A315 RID: 41749 RVA: 0x00388F6C File Offset: 0x0038716C
		public StatesInstance(ClusterMapVisualizer visualizer, ClusterGridEntity entity) : base(visualizer)
		{
			base.sm.entityTarget.Set(entity, this);
			visualizer.GetFirstAnimController().gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
		}

		// Token: 0x0600A316 RID: 41750 RVA: 0x00388FA9 File Offset: 0x003871A9
		private void OnAnimQueueComplete(object data)
		{
			base.sm.onAnimComplete.Trigger(this);
		}

		// Token: 0x0600A317 RID: 41751 RVA: 0x00388FBC File Offset: 0x003871BC
		public void DestroyEntity()
		{
			base.sm.entityTarget.Get(this).DeleteObject();
		}
	}
}
