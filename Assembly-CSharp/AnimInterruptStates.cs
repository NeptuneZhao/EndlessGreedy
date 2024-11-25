using System;

// Token: 0x020000B8 RID: 184
public class AnimInterruptStates : GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>
{
	// Token: 0x06000340 RID: 832 RVA: 0x0001B52C File Offset: 0x0001972C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.play_anim;
		this.play_anim.Enter(new StateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State.Callback(this.PlayAnim)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.PlayInterruptAnim, false);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x0001B56C File Offset: 0x0001976C
	private void PlayAnim(AnimInterruptStates.Instance smi)
	{
		KBatchedAnimController kbatchedAnimController = smi.Get<KBatchedAnimController>();
		HashedString[] anims = smi.GetSMI<AnimInterruptMonitor.Instance>().anims;
		kbatchedAnimController.Play(anims[0], KAnim.PlayMode.Once, 1f, 0f);
		for (int i = 1; i < anims.Length; i++)
		{
			kbatchedAnimController.Queue(anims[i], KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0400024F RID: 591
	public GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State play_anim;

	// Token: 0x04000250 RID: 592
	public GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State behaviourcomplete;

	// Token: 0x02000FDE RID: 4062
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FDF RID: 4063
	public new class Instance : GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.GameInstance
	{
		// Token: 0x06007ABE RID: 31422 RVA: 0x00302826 File Offset: 0x00300A26
		public Instance(Chore<AnimInterruptStates.Instance> chore, AnimInterruptStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.PlayInterruptAnim);
		}
	}
}
