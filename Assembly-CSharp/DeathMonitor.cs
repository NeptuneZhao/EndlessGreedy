using System;
using STRINGS;

// Token: 0x02000977 RID: 2423
public class DeathMonitor : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>
{
	// Token: 0x060046F0 RID: 18160 RVA: 0x001959B4 File Offset: 0x00193BB4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.alive.ParamTransition<Death>(this.death, this.dying_duplicant, (DeathMonitor.Instance smi, Death p) => p != null && smi.IsDuplicant).ParamTransition<Death>(this.death, this.dying_creature, (DeathMonitor.Instance smi, Death p) => p != null && !smi.IsDuplicant);
		this.dying_duplicant.ToggleAnims("anim_emotes_default_kanim", 0f).ToggleTag(GameTags.Dying).ToggleChore((DeathMonitor.Instance smi) => new DieChore(smi.master, this.death.Get(smi)), this.die);
		this.dying_creature.ToggleBehaviour(GameTags.Creatures.Die, (DeathMonitor.Instance smi) => true, delegate(DeathMonitor.Instance smi)
		{
			smi.GoTo(this.dead_creature);
		});
		this.die.ToggleTag(GameTags.Dying).Enter("Die", delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.PreventChoreInterruption);
			Death death = this.death.Get(smi);
			if (smi.IsDuplicant)
			{
				DeathMessage message = new DeathMessage(smi.gameObject, death);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_localized", false), smi.master.transform.GetPosition(), 1f);
				KFMOD.PlayUISound(GlobalAssets.GetSound("Death_Notification_ST", false));
				Messenger.Instance.QueueMessage(message);
			}
		}).TriggerOnExit(GameHashes.Died, null).GoTo(this.dead);
		this.dead.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.dead.ground).ToggleTag(GameTags.Dead).Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.ApplyDeath();
			Game.Instance.Trigger(282337316, smi.gameObject);
		});
		this.dead.ground.Enter(delegate(DeathMonitor.Instance smi)
		{
			Death death = this.death.Get(smi);
			if (death == null)
			{
				death = Db.Get().Deaths.Generic;
			}
			if (smi.IsDuplicant)
			{
				smi.GetComponent<KAnimControllerBase>().Play(death.loopAnim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}).EventTransition(GameHashes.OnStore, this.dead.carried, (DeathMonitor.Instance smi) => smi.IsDuplicant && smi.HasTag(GameTags.Stored));
		this.dead.carried.ToggleAnims("anim_dead_carried_kanim", 0f).PlayAnim("idle_default", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.dead.ground, (DeathMonitor.Instance smi) => !smi.HasTag(GameTags.Stored));
		this.dead_creature.Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
		}).PlayAnim("idle_dead", KAnim.PlayMode.Loop);
	}

	// Token: 0x04002E38 RID: 11832
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State alive;

	// Token: 0x04002E39 RID: 11833
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_duplicant;

	// Token: 0x04002E3A RID: 11834
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_creature;

	// Token: 0x04002E3B RID: 11835
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State die;

	// Token: 0x04002E3C RID: 11836
	public DeathMonitor.Dead dead;

	// Token: 0x04002E3D RID: 11837
	public DeathMonitor.Dead dead_creature;

	// Token: 0x04002E3E RID: 11838
	public StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.ResourceParameter<Death> death;

	// Token: 0x02001915 RID: 6421
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001916 RID: 6422
	public class Dead : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State
	{
		// Token: 0x0400784D RID: 30797
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State ground;

		// Token: 0x0400784E RID: 30798
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State carried;
	}

	// Token: 0x02001917 RID: 6423
	public new class Instance : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.GameInstance
	{
		// Token: 0x06009B22 RID: 39714 RVA: 0x0036ECEC File Offset: 0x0036CEEC
		public Instance(IStateMachineTarget master, DeathMonitor.Def def) : base(master, def)
		{
			this.isDuplicant = base.GetComponent<MinionIdentity>();
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06009B23 RID: 39715 RVA: 0x0036ED07 File Offset: 0x0036CF07
		public bool IsDuplicant
		{
			get
			{
				return this.isDuplicant;
			}
		}

		// Token: 0x06009B24 RID: 39716 RVA: 0x0036ED0F File Offset: 0x0036CF0F
		public void Kill(Death death)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x06009B25 RID: 39717 RVA: 0x0036ED2A File Offset: 0x0036CF2A
		public void PickedUp(object data = null)
		{
			if (data is Storage || (data != null && (bool)data))
			{
				base.smi.GoTo(base.sm.dead.carried);
			}
		}

		// Token: 0x06009B26 RID: 39718 RVA: 0x0036ED60 File Offset: 0x0036CF60
		public bool IsDead()
		{
			return base.smi.IsInsideState(base.smi.sm.dead);
		}

		// Token: 0x06009B27 RID: 39719 RVA: 0x0036ED80 File Offset: 0x0036CF80
		public void ApplyDeath()
		{
			if (this.isDuplicant)
			{
				Game.Instance.assignmentManager.RemoveFromAllGroups(base.GetComponent<MinionIdentity>().assignableProxy.Get());
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Dead, base.smi.sm.death.Get(base.smi));
				float value = 600f - GameClock.Instance.GetTimeSinceStartOfReport();
				ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, value, string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.IS_DEAD_TASK), base.smi.master.gameObject.GetProperName());
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null)
				{
					component.UpdateListeners(true);
				}
			}
			base.GetComponent<KPrefabID>().AddTag(GameTags.Corpse, false);
		}

		// Token: 0x0400784F RID: 30799
		private bool isDuplicant;
	}
}
