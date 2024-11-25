using System;
using Database;
using UnityEngine;

// Token: 0x02000636 RID: 1590
public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
	// Token: 0x06002719 RID: 10009 RVA: 0x000DE688 File Offset: 0x000DC888
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.Exit("DestroyFX", delegate(BalloonFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001661 RID: 5729
	public StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001662 RID: 5730
	public KAnimFile defaultAnim = Assets.GetAnim("balloon_anim_kanim");

	// Token: 0x04001663 RID: 5731
	private KAnimFile defaultBalloon = Assets.GetAnim("balloon_basic_red_kanim");

	// Token: 0x04001664 RID: 5732
	private const string defaultAnimName = "balloon_anim_kanim";

	// Token: 0x04001665 RID: 5733
	private const string balloonAnimName = "balloon_basic_red_kanim";

	// Token: 0x04001666 RID: 5734
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04001667 RID: 5735
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x02001412 RID: 5138
	public new class Instance : GameStateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008948 RID: 35144 RVA: 0x00330048 File Offset: 0x0032E248
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.balloonAnimController = FXHelpers.CreateEffectOverride(new string[]
			{
				"balloon_anim_kanim",
				"balloon_basic_red_kanim"
			}, master.gameObject.transform.GetPosition() + new Vector3(0f, 0.3f, 1f), master.transform, true, Grid.SceneLayer.Creatures, false);
			base.sm.fx.Set(this.balloonAnimController.gameObject, base.smi, false);
			this.balloonAnimController.defaultAnim = "idle_default";
			master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add(this.balloonAnimController.GetComponent<KBatchedAnimController>());
		}

		// Token: 0x06008949 RID: 35145 RVA: 0x00330100 File Offset: 0x0032E300
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverride)
		{
			KAnimFile kanimFile = balloonOverride.animFile.IsSome() ? balloonOverride.animFile.Unwrap() : base.smi.sm.defaultBalloon;
			this.balloonAnimController.SwapAnims(new KAnimFile[]
			{
				base.smi.sm.defaultAnim,
				kanimFile
			});
			SymbolOverrideController component = this.balloonAnimController.GetComponent<SymbolOverrideController>();
			if (this.currentBodyOverrideSymbol.IsSome())
			{
				component.RemoveSymbolOverride("body", 0);
			}
			if (balloonOverride.symbol.IsNone())
			{
				if (this.currentBodyOverrideSymbol.IsSome())
				{
					component.AddSymbolOverride("body", base.smi.sm.defaultAnim.GetData().build.GetSymbol("body"), 0);
				}
				this.balloonAnimController.SetBatchGroupOverride(HashedString.Invalid);
			}
			else
			{
				component.AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
				this.balloonAnimController.SetBatchGroupOverride(kanimFile.batchTag);
			}
			this.currentBodyOverrideSymbol = balloonOverride;
		}

		// Token: 0x0600894A RID: 35146 RVA: 0x00330230 File Offset: 0x0032E430
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040068C6 RID: 26822
		private KBatchedAnimController balloonAnimController;

		// Token: 0x040068C7 RID: 26823
		private Option<BalloonOverrideSymbol> currentBodyOverrideSymbol;
	}
}
