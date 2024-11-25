using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000CA RID: 202
public class DeathStates : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>
{
	// Token: 0x060003B5 RID: 949 RVA: 0x0001E96C File Offset: 0x0001CB6C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State state = this.loop;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter("EnableGravity", delegate(DeathStates.Instance smi)
		{
			smi.EnableGravityIfNecessary();
		}).Enter("Play Death Animations", delegate(DeathStates.Instance smi)
		{
			smi.PlayDeathAnimations();
		}).OnAnimQueueComplete(this.pst).ScheduleGoTo((DeathStates.Instance smi) => smi.def.DIE_ANIMATION_EXPIRATION_TIME, this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, null).Enter("Butcher", delegate(DeathStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Butcherable>() != null)
			{
				smi.GetComponent<Butcherable>().OnButcherComplete();
			}
		}).Enter("Destroy", delegate(DeathStates.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
			smi.gameObject.DeleteObject();
		}).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x0400028C RID: 652
	private GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop;

	// Token: 0x0400028D RID: 653
	public GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State pst;

	// Token: 0x02001022 RID: 4130
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C50 RID: 23632
		public float DIE_ANIMATION_EXPIRATION_TIME = 4f;
	}

	// Token: 0x02001023 RID: 4131
	public new class Instance : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.GameInstance
	{
		// Token: 0x06007B4F RID: 31567 RVA: 0x003035CF File Offset: 0x003017CF
		public Instance(Chore<DeathStates.Instance> chore, DeathStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}

		// Token: 0x06007B50 RID: 31568 RVA: 0x003035F4 File Offset: 0x003017F4
		public void EnableGravityIfNecessary()
		{
			if (base.HasTag(GameTags.Creatures.Flyer) && !base.HasTag(GameTags.Stored))
			{
				GameComps.Gravities.Add(base.smi.gameObject, Vector2.zero, delegate()
				{
					base.smi.DisableGravity();
				});
			}
		}

		// Token: 0x06007B51 RID: 31569 RVA: 0x00303642 File Offset: 0x00301842
		public void DisableGravity()
		{
			if (GameComps.Gravities.Has(base.smi.gameObject))
			{
				GameComps.Gravities.Remove(base.smi.gameObject);
			}
		}

		// Token: 0x06007B52 RID: 31570 RVA: 0x00303670 File Offset: 0x00301870
		public void PlayDeathAnimations()
		{
			if (base.gameObject.HasTag(GameTags.PreventDeadAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("Death", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
