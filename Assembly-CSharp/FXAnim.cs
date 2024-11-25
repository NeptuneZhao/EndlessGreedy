using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class FXAnim : GameStateMachine<FXAnim, FXAnim.Instance>
{
	// Token: 0x0600271D RID: 10013 RVA: 0x000DE804 File Offset: 0x000DCA04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		base.Target(this.fx);
		this.loop.Enter(delegate(FXAnim.Instance smi)
		{
			smi.Enter();
		}).EventTransition(GameHashes.AnimQueueComplete, this.restart, null).Exit("Post", delegate(FXAnim.Instance smi)
		{
			smi.Exit();
		});
		this.restart.GoTo(this.loop);
	}

	// Token: 0x0400166F RID: 5743
	public StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001670 RID: 5744
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State loop;

	// Token: 0x04001671 RID: 5745
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State restart;

	// Token: 0x02001416 RID: 5142
	public new class Instance : GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008954 RID: 35156 RVA: 0x00330330 File Offset: 0x0032E530
		public Instance(IStateMachineTarget master, string kanim_file, string anim, KAnim.PlayMode mode, Vector3 offset, Color32 tint_colour) : base(master)
		{
			this.animController = FXHelpers.CreateEffect(kanim_file, base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			this.animController.gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
			this.animController.TintColour = tint_colour;
			base.sm.fx.Set(this.animController.gameObject, base.smi, false);
			this.anim = anim;
			this.mode = mode;
		}

		// Token: 0x06008955 RID: 35157 RVA: 0x003303E1 File Offset: 0x0032E5E1
		public void Enter()
		{
			this.animController.Play(this.anim, this.mode, 1f, 0f);
		}

		// Token: 0x06008956 RID: 35158 RVA: 0x00330409 File Offset: 0x0032E609
		public void Exit()
		{
			this.DestroyFX();
		}

		// Token: 0x06008957 RID: 35159 RVA: 0x00330411 File Offset: 0x0032E611
		private void OnAnimQueueComplete(object data)
		{
			this.DestroyFX();
		}

		// Token: 0x06008958 RID: 35160 RVA: 0x00330419 File Offset: 0x0032E619
		private void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040068CD RID: 26829
		private string anim;

		// Token: 0x040068CE RID: 26830
		private KAnim.PlayMode mode;

		// Token: 0x040068CF RID: 26831
		private KBatchedAnimController animController;
	}
}
