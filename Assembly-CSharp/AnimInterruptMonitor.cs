using System;

// Token: 0x020007EE RID: 2030
public class AnimInterruptMonitor : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
	// Token: 0x0600382A RID: 14378 RVA: 0x00132F78 File Offset: 0x00131178
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.PlayInterruptAnim, new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim), new Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim));
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x00132FAB File Offset: 0x001311AB
	private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi)
	{
		return smi.anims != null;
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x00132FB6 File Offset: 0x001311B6
	private static void ClearAnim(AnimInterruptMonitor.Instance smi)
	{
		smi.anims = null;
	}

	// Token: 0x020016BF RID: 5823
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020016C0 RID: 5824
	public new class Instance : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance
	{
		// Token: 0x06009371 RID: 37745 RVA: 0x003593C7 File Offset: 0x003575C7
		public Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06009372 RID: 37746 RVA: 0x003593D1 File Offset: 0x003575D1
		public void PlayAnim(HashedString anim)
		{
			this.PlayAnimSequence(new HashedString[]
			{
				anim
			});
		}

		// Token: 0x06009373 RID: 37747 RVA: 0x003593E7 File Offset: 0x003575E7
		public void PlayAnimSequence(HashedString[] anims)
		{
			this.anims = anims;
			base.GetComponent<CreatureBrain>().UpdateBrain();
		}

		// Token: 0x040070BF RID: 28863
		public HashedString[] anims;
	}
}
