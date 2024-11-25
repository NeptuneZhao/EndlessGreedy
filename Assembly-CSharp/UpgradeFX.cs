using System;
using UnityEngine;

// Token: 0x0200063F RID: 1599
public class UpgradeFX : GameStateMachine<UpgradeFX, UpgradeFX.Instance>
{
	// Token: 0x06002731 RID: 10033 RVA: 0x000DF4EC File Offset: 0x000DD6EC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).ToggleReactable((UpgradeFX.Instance smi) => smi.CreateReactable()).Exit("DestroyFX", delegate(UpgradeFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x04001686 RID: 5766
	public StateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02001424 RID: 5156
	public new class Instance : GameStateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600898A RID: 35210 RVA: 0x00330BA0 File Offset: 0x0032EDA0
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("upgrade_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x0600898B RID: 35211 RVA: 0x00330C02 File Offset: 0x0032EE02
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x0600898C RID: 35212 RVA: 0x00330C20 File Offset: 0x0032EE20
		public Reactable CreateReactable()
		{
			return new EmoteReactable(base.master.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
		}
	}
}
