using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200099E RID: 2462
public class SleepChoreMonitor : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance>
{
	// Token: 0x060047BB RID: 18363 RVA: 0x0019A950 File Offset: 0x00198B50
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.AssignablesChanged, delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
		});
		this.satisfied.EventTransition(GameHashes.AddUrge, this.checkforbed, (SleepChoreMonitor.Instance smi) => smi.HasSleepUrge());
		this.checkforbed.Enter("SetBed", delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
			if (smi.GetSMI<StaminaMonitor.Instance>().NeedsToSleep())
			{
				if (this.bed.Get(smi) != null && smi.IsBedReachable())
				{
					smi.GoTo(this.passingout_bedassigned);
					return;
				}
				smi.GoTo(this.passingout);
				return;
			}
			else
			{
				if (this.bed.Get(smi) == null || !smi.IsBedReachable())
				{
					smi.GoTo(this.sleeponfloor);
					return;
				}
				smi.GoTo(this.bedassigned);
				return;
			}
		});
		this.passingout.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventHandlerTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi, object data) => smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreatePassingOutChore), this.satisfied, this.satisfied);
		this.passingout_bedassigned.ParamTransition<GameObject>(this.bed, this.checkforbed, (SleepChoreMonitor.Instance smi, GameObject p) => p == null).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi) => !smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateExhaustedSleepChore), this.satisfied, this.satisfied);
		this.sleeponfloor.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventHandlerTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi, object data) => smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepOnFloorChore), this.satisfied, this.satisfied);
		this.bedassigned.ParamTransition<GameObject>(this.bed, this.checkforbed, (SleepChoreMonitor.Instance smi, GameObject p) => p == null).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi) => !smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepChore), this.satisfied, this.satisfied);
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x0019ABE0 File Offset: 0x00198DE0
	private Chore CreatePassingOutChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreatePassedOutLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, false);
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x0019AC14 File Offset: 0x00198E14
	private Chore CreateSleepOnFloorChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreateFloorLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, true);
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x0019AC45 File Offset: 0x00198E45
	private Chore CreateSleepChore(SleepChoreMonitor.Instance smi)
	{
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true);
	}

	// Token: 0x060047BF RID: 18367 RVA: 0x0019AC70 File Offset: 0x00198E70
	private Chore CreateExhaustedSleepChore(SleepChoreMonitor.Instance smi)
	{
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true, new StatusItem[]
		{
			Db.Get().DuplicantStatusItems.SleepingExhausted
		});
	}

	// Token: 0x04002EE5 RID: 12005
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002EE6 RID: 12006
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State checkforbed;

	// Token: 0x04002EE7 RID: 12007
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout;

	// Token: 0x04002EE8 RID: 12008
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout_bedassigned;

	// Token: 0x04002EE9 RID: 12009
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State sleeponfloor;

	// Token: 0x04002EEA RID: 12010
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State bedassigned;

	// Token: 0x04002EEB RID: 12011
	public StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.TargetParameter bed;

	// Token: 0x0200197B RID: 6523
	public new class Instance : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009CC5 RID: 40133 RVA: 0x00373260 File Offset: 0x00371460
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009CC6 RID: 40134 RVA: 0x0037326C File Offset: 0x0037146C
		public void UpdateBed()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			Assignable assignable = soleOwner.GetAssignable(Db.Get().AssignableSlots.MedicalBed);
			Assignable assignable2;
			if (assignable != null && assignable.CanAutoAssignTo(base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().assignableProxy.Get()))
			{
				assignable2 = assignable;
			}
			else
			{
				assignable2 = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);
				if (assignable2 == null)
				{
					assignable2 = soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
					if (assignable2 != null)
					{
						AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
						if (sensor.IsEnabled)
						{
							sensor.Update();
						}
					}
				}
			}
			base.smi.sm.bed.Set(assignable2, base.smi);
		}

		// Token: 0x06009CC7 RID: 40135 RVA: 0x00373360 File Offset: 0x00371560
		public bool HasSleepUrge()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x06009CC8 RID: 40136 RVA: 0x0037337C File Offset: 0x0037157C
		public bool IsBedReachable()
		{
			AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
			return sensor.IsReachable(Db.Get().AssignableSlots.Bed) || sensor.IsReachable(Db.Get().AssignableSlots.MedicalBed);
		}

		// Token: 0x06009CC9 RID: 40137 RVA: 0x003733C3 File Offset: 0x003715C3
		public GameObject CreatePassedOutLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "PassedOutSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x06009CCA RID: 40138 RVA: 0x00373402 File Offset: 0x00371602
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "FloorSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x04007996 RID: 31126
		private int locatorCell;

		// Token: 0x04007997 RID: 31127
		public GameObject locator;
	}
}
