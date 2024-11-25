using System;
using UnityEngine;

// Token: 0x02000AF1 RID: 2801
public class ClusterMapBallisticAnimator : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x0600539A RID: 21402 RVA: 0x001DF4DC File Offset: 0x001DD6DC
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.moving;
		this.root.Target(this.entityTarget).TagTransition(GameTags.BallisticEntityLaunching, this.launching, false).TagTransition(GameTags.BallisticEntityLanding, this.landing, false).TagTransition(GameTags.BallisticEntityMoving, this.moving, false);
		this.moving.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.landing.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
		});
		this.launching.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
		});
	}

	// Token: 0x04003704 RID: 14084
	public StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04003705 RID: 14085
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State launching;

	// Token: 0x04003706 RID: 14086
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State moving;

	// Token: 0x04003707 RID: 14087
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;

	// Token: 0x02001B49 RID: 6985
	public class StatesInstance : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600A30A RID: 41738 RVA: 0x00388E32 File Offset: 0x00387032
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600A30B RID: 41739 RVA: 0x00388E5B File Offset: 0x0038705B
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x0600A30C RID: 41740 RVA: 0x00388E6C File Offset: 0x0038706C
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x0600A30D RID: 41741 RVA: 0x00388EA4 File Offset: 0x003870A4
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x0600A30E RID: 41742 RVA: 0x00388EE6 File Offset: 0x003870E6
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClustermapBallisticAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x0600A30F RID: 41743 RVA: 0x00388F20 File Offset: 0x00387120
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x04007F50 RID: 32592
		public ClusterGridEntity entity;

		// Token: 0x04007F51 RID: 32593
		private int animCompleteHandle = -1;

		// Token: 0x04007F52 RID: 32594
		private GameObject animCompleteSubscriber;
	}
}
