using System;
using UnityEngine;

// Token: 0x02000383 RID: 899
public class DeliverToSweepLockerStates : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>
{
	// Token: 0x060012A1 RID: 4769 RVA: 0x00066240 File Offset: 0x00064440
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.movingToStorage;
		this.idle.ScheduleGoTo(1f, this.movingToStorage);
		this.movingToStorage.MoveTo(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			if (!(this.GetSweepLocker(smi) == null))
			{
				return Grid.PosToCell(this.GetSweepLocker(smi));
			}
			return Grid.InvalidCell;
		}, this.unloading, this.idle, false);
		this.unloading.Enter(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			Storage storage = smi.master.gameObject.GetComponents<Storage>()[1];
			float num = Mathf.Max(0f, Mathf.Min(storage.ExactMassStored(), sweepLocker.RemainingCapacity()));
			for (int i = storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = storage.items[i];
				if (!(gameObject == null))
				{
					float num2 = Mathf.Min(gameObject.GetComponent<PrimaryElement>().Mass, num);
					if (num2 != 0f)
					{
						storage.Transfer(sweepLocker, gameObject.GetComponent<KPrefabID>().PrefabTag, num2, false, false);
					}
					num -= num2;
					if (num <= 0f)
					{
						break;
					}
				}
			}
			smi.master.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			smi.master.GetComponent<KBatchedAnimController>().FlipX = false;
			sweepLocker.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			if (storage.MassStored() > 0f)
			{
				smi.ScheduleGoTo(2f, this.lockerFull);
				return;
			}
			smi.ScheduleGoTo(2f, this.behaviourcomplete);
		});
		this.lockerFull.PlayAnim("react_bored", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToStorage);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.UnloadBehaviour, false);
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x000662D8 File Offset: 0x000644D8
	public Storage GetSweepLocker(DeliverToSweepLockerStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x04000ACD RID: 2765
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State idle;

	// Token: 0x04000ACE RID: 2766
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State movingToStorage;

	// Token: 0x04000ACF RID: 2767
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State unloading;

	// Token: 0x04000AD0 RID: 2768
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State lockerFull;

	// Token: 0x04000AD1 RID: 2769
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State behaviourcomplete;

	// Token: 0x0200113C RID: 4412
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200113D RID: 4413
	public new class Instance : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.GameInstance
	{
		// Token: 0x06007EED RID: 32493 RVA: 0x0030B9BF File Offset: 0x00309BBF
		public Instance(Chore<DeliverToSweepLockerStates.Instance> chore, DeliverToSweepLockerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.UnloadBehaviour);
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x0030B9E3 File Offset: 0x00309BE3
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.UnloadingStorage, base.gameObject);
		}

		// Token: 0x06007EEF RID: 32495 RVA: 0x0030BA1B File Offset: 0x00309C1B
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.UnloadingStorage, false);
		}
	}
}
