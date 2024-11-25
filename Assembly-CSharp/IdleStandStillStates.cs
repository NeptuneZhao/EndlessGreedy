using System;
using STRINGS;

// Token: 0x020000E2 RID: 226
public class IdleStandStillStates : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>
{
	// Token: 0x0600041A RID: 1050 RVA: 0x000210D8 File Offset: 0x0001F2D8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.Idle);
		this.loop.Enter(new StateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State.Callback(this.PlayIdle));
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00021158 File Offset: 0x0001F358
	public void PlayIdle(IdleStandStillStates.Instance smi)
	{
		KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					component.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		component.Play("idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x040002C9 RID: 713
	private GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.State loop;

	// Token: 0x0200106B RID: 4203
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005CC6 RID: 23750
		public IdleStandStillStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02002386 RID: 9094
		// (Invoke) Token: 0x0600B6CF RID: 46799
		public delegate HashedString IdleAnimCallback(IdleStandStillStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x0200106C RID: 4204
	public new class Instance : GameStateMachine<IdleStandStillStates, IdleStandStillStates.Instance, IStateMachineTarget, IdleStandStillStates.Def>.GameInstance
	{
		// Token: 0x06007BF2 RID: 31730 RVA: 0x00304692 File Offset: 0x00302892
		public Instance(Chore<IdleStandStillStates.Instance> chore, IdleStandStillStates.Def def) : base(chore, def)
		{
		}
	}
}
