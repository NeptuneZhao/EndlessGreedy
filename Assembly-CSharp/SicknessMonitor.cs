using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200099D RID: 2461
public class SicknessMonitor : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance>
{
	// Token: 0x060047B7 RID: 18359 RVA: 0x0019A6BC File Offset: 0x001988BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.healthy;
		this.healthy.EventTransition(GameHashes.SicknessAdded, this.sick, (SicknessMonitor.Instance smi) => smi.IsSick());
		this.sick.DefaultState(this.sick.minor).EventTransition(GameHashes.SicknessCured, this.post_nocheer, (SicknessMonitor.Instance smi) => !smi.IsSick()).ToggleThought(Db.Get().Thoughts.GotInfected, null);
		this.sick.minor.EventTransition(GameHashes.SicknessAdded, this.sick.major, (SicknessMonitor.Instance smi) => smi.HasMajorDisease());
		this.sick.major.EventTransition(GameHashes.SicknessCured, this.sick.minor, (SicknessMonitor.Instance smi) => !smi.HasMajorDisease()).ToggleUrge(Db.Get().Urges.RestDueToDisease).Update("AutoAssignClinic", delegate(SicknessMonitor.Instance smi, float dt)
		{
			smi.AutoAssignClinic();
		}, UpdateRate.SIM_4000ms, false).Exit(delegate(SicknessMonitor.Instance smi)
		{
			smi.UnassignClinic();
		});
		this.post_nocheer.Enter(delegate(SicknessMonitor.Instance smi)
		{
			new SicknessCuredFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f)).StartSM();
			if (smi.IsSleepingOrSleepSchedule())
			{
				smi.GoTo(this.healthy);
				return;
			}
			smi.GoTo(this.post);
		});
		this.post.ToggleChore((SicknessMonitor.Instance smi) => new EmoteChore(smi.master, Db.Get().ChoreTypes.EmoteHighPriority, SicknessMonitor.SickPostKAnim, SicknessMonitor.SickPostAnims, KAnim.PlayMode.Once, false), this.healthy);
	}

	// Token: 0x04002EDF RID: 11999
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x04002EE0 RID: 12000
	public SicknessMonitor.SickStates sick;

	// Token: 0x04002EE1 RID: 12001
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post;

	// Token: 0x04002EE2 RID: 12002
	public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State post_nocheer;

	// Token: 0x04002EE3 RID: 12003
	private static readonly HashedString SickPostKAnim = "anim_cheer_kanim";

	// Token: 0x04002EE4 RID: 12004
	private static readonly HashedString[] SickPostAnims = new HashedString[]
	{
		"cheer_pre",
		"cheer_loop",
		"cheer_pst"
	};

	// Token: 0x02001978 RID: 6520
	public class SickStates : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400798B RID: 31115
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State minor;

		// Token: 0x0400798C RID: 31116
		public GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.State major;
	}

	// Token: 0x02001979 RID: 6521
	public new class Instance : GameStateMachine<SicknessMonitor, SicknessMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009CB5 RID: 40117 RVA: 0x00373051 File Offset: 0x00371251
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
		}

		// Token: 0x06009CB6 RID: 40118 RVA: 0x0037306B File Offset: 0x0037126B
		private string OnGetToolTip(List<Notification> notifications, object data)
		{
			return DUPLICANTS.STATUSITEMS.HASDISEASE.TOOLTIP;
		}

		// Token: 0x06009CB7 RID: 40119 RVA: 0x00373077 File Offset: 0x00371277
		public bool IsSick()
		{
			return this.sicknesses.Count > 0;
		}

		// Token: 0x06009CB8 RID: 40120 RVA: 0x00373088 File Offset: 0x00371288
		public bool HasMajorDisease()
		{
			using (IEnumerator<SicknessInstance> enumerator = this.sicknesses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.modifier.severity >= Sickness.Severity.Major)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06009CB9 RID: 40121 RVA: 0x003730E4 File Offset: 0x003712E4
		public void AutoAssignClinic()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot == null)
			{
				return;
			}
			if (slot.assignable != null)
			{
				return;
			}
			soleOwner.AutoAssignSlot(clinic);
		}

		// Token: 0x06009CBA RID: 40122 RVA: 0x00373148 File Offset: 0x00371348
		public void UnassignClinic()
		{
			Assignables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			AssignableSlot clinic = Db.Get().AssignableSlots.Clinic;
			AssignableSlotInstance slot = soleOwner.GetSlot(clinic);
			if (slot != null)
			{
				slot.Unassign(true);
			}
		}

		// Token: 0x06009CBB RID: 40123 RVA: 0x00373198 File Offset: 0x00371398
		public bool IsSleepingOrSleepSchedule()
		{
			Schedulable component = base.GetComponent<Schedulable>();
			if (component != null && component.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return true;
			}
			KPrefabID component2 = base.GetComponent<KPrefabID>();
			return component2 != null && component2.HasTag(GameTags.Asleep);
		}

		// Token: 0x0400798D RID: 31117
		private Sicknesses sicknesses;
	}
}
