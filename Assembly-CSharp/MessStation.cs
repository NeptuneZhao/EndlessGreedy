using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200072A RID: 1834
[AddComponentMenu("KMonoBehaviour/Workable/MessStation")]
public class MessStation : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x060030AB RID: 12459 RVA: 0x0010C842 File Offset: 0x0010AA42
	protected override void OnPrefabInit()
	{
		this.ownable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.HasCaloriesOwnablePrecondition));
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x0010C880 File Offset: 0x0010AA80
	private bool HasCaloriesOwnablePrecondition(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = (Db.Get().Amounts.Calories.Lookup(minionIdentity) != null);
		}
		return result;
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x0010C8BE File Offset: 0x0010AABE
	protected override void OnCompleteWork(WorkerBase worker)
	{
		worker.GetWorkable().GetComponent<Edible>().CompleteWork(worker);
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x0010C8D1 File Offset: 0x0010AAD1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new MessStation.MessStationSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x0010C8F0 File Offset: 0x0010AAF0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (go.GetComponent<Storage>().Has(TableSaltConfig.ID.ToTag()))
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MESS_TABLE_SALT, TableSaltTuning.MORALE_MODIFIER), Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x060030B0 RID: 12464 RVA: 0x0010C95A File Offset: 0x0010AB5A
	public bool HasSalt
	{
		get
		{
			return this.smi.HasSalt;
		}
	}

	// Token: 0x04001C90 RID: 7312
	[MyCmpGet]
	private Ownable ownable;

	// Token: 0x04001C91 RID: 7313
	private MessStation.MessStationSM.Instance smi;

	// Token: 0x02001579 RID: 5497
	public class MessStationSM : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation>
	{
		// Token: 0x06008EA5 RID: 36517 RVA: 0x00344A00 File Offset: 0x00342C00
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.salt.none;
			this.salt.none.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("off");
			this.salt.salty.Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt, UpdateRate.SIM_200ms).PlayAnim("salt").EventTransition(GameHashes.EatStart, this.eating, null);
			this.eating.Transition(this.salt.salty, (MessStation.MessStationSM.Instance smi) => smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).Transition(this.salt.none, (MessStation.MessStationSM.Instance smi) => !smi.HasSalt && !smi.IsEating(), UpdateRate.SIM_200ms).PlayAnim("off");
		}

		// Token: 0x04006CE3 RID: 27875
		public MessStation.MessStationSM.SaltState salt;

		// Token: 0x04006CE4 RID: 27876
		public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State eating;

		// Token: 0x0200250B RID: 9483
		public class SaltState : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State
		{
			// Token: 0x0400A4DC RID: 42204
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State none;

			// Token: 0x0400A4DD RID: 42205
			public GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.State salty;
		}

		// Token: 0x0200250C RID: 9484
		public new class Instance : GameStateMachine<MessStation.MessStationSM, MessStation.MessStationSM.Instance, MessStation, object>.GameInstance
		{
			// Token: 0x0600BCE4 RID: 48356 RVA: 0x003D699D File Offset: 0x003D4B9D
			public Instance(MessStation master) : base(master)
			{
				this.saltStorage = master.GetComponent<Storage>();
				this.assigned = master.GetComponent<Assignable>();
			}

			// Token: 0x17000C19 RID: 3097
			// (get) Token: 0x0600BCE5 RID: 48357 RVA: 0x003D69BE File Offset: 0x003D4BBE
			public bool HasSalt
			{
				get
				{
					return this.saltStorage.Has(TableSaltConfig.ID.ToTag());
				}
			}

			// Token: 0x0600BCE6 RID: 48358 RVA: 0x003D69D8 File Offset: 0x003D4BD8
			public bool IsEating()
			{
				if (this.assigned == null || this.assigned.assignee == null)
				{
					return false;
				}
				Ownables soleOwner = this.assigned.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return false;
				}
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject == null)
				{
					return false;
				}
				ChoreDriver component = targetGameObject.GetComponent<ChoreDriver>();
				return component != null && component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
			}

			// Token: 0x0400A4DE RID: 42206
			private Storage saltStorage;

			// Token: 0x0400A4DF RID: 42207
			private Assignable assigned;
		}
	}
}
