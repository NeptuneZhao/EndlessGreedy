using System;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class ReturnToChargeStationStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>
{
	// Token: 0x060012A6 RID: 4774 RVA: 0x0006648C File Offset: 0x0006468C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.emote;
		this.emote.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).PlayAnim("react_lobatt", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToChargingStation);
		this.idle.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).ScheduleGoTo(1f, this.movingToChargingStation);
		this.movingToChargingStation.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).MoveTo(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (!(sweepLocker == null))
			{
				return Grid.PosToCell(sweepLocker);
			}
			return Grid.InvalidCell;
		}, this.chargingstates.waitingForCharging, this.idle, false);
		this.chargingstates.Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker != null)
			{
				smi.master.GetComponent<Facing>().Face(sweepLocker.gameObject.transform.position + Vector3.right);
				Vector3 position = smi.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
				smi.transform.SetPosition(position);
				KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				component.enabled = true;
			}
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			smi.transform.SetPosition(position);
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}).Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, true);
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, false);
		});
		this.chargingstates.waitingForCharging.PlayAnim("react_base", KAnim.PlayMode.Loop).TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.charging, (ReturnToChargeStationStates.Instance smi) => smi.StationReadyToCharge(), UpdateRate.SIM_200ms);
		this.chargingstates.charging.TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.interupted, (ReturnToChargeStationStates.Instance smi) => !smi.StationReadyToCharge(), UpdateRate.SIM_200ms).ToggleEffect("Charging").PlayAnim("sleep_pre").QueueAnim("sleep_idle", true, null).Enter(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StartCharging)).Exit(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StopCharging));
		this.chargingstates.interupted.PlayAnim("sleep_pst").TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).OnAnimQueueComplete(this.chargingstates.waitingForCharging);
		this.chargingstates.completed.PlayAnim("sleep_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.RechargeBehaviour, false);
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x00066784 File Offset: 0x00064984
	public Storage GetSweepLocker(ReturnToChargeStationStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x000667B8 File Offset: 0x000649B8
	public void Station_StartCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StartCharging();
		}
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x000667E4 File Offset: 0x000649E4
	public void Station_StopCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StopCharging();
		}
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x00066810 File Offset: 0x00064A10
	public void Station_DockRobot(ReturnToChargeStationStates.Instance smi, bool dockState)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().DockRobot(dockState);
		}
	}

	// Token: 0x04000AD2 RID: 2770
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State emote;

	// Token: 0x04000AD3 RID: 2771
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State idle;

	// Token: 0x04000AD4 RID: 2772
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State movingToChargingStation;

	// Token: 0x04000AD5 RID: 2773
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State behaviourcomplete;

	// Token: 0x04000AD6 RID: 2774
	public ReturnToChargeStationStates.ChargingStates chargingstates;

	// Token: 0x0200113E RID: 4414
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200113F RID: 4415
	public new class Instance : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.GameInstance
	{
		// Token: 0x06007EF1 RID: 32497 RVA: 0x0030BA47 File Offset: 0x00309C47
		public Instance(Chore<ReturnToChargeStationStates.Instance> chore, ReturnToChargeStationStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.RechargeBehaviour);
		}

		// Token: 0x06007EF2 RID: 32498 RVA: 0x0030BA6C File Offset: 0x00309C6C
		public bool ChargeAborted()
		{
			return base.smi.sm.GetSweepLocker(base.smi) == null || !base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}

		// Token: 0x06007EF3 RID: 32499 RVA: 0x0030BABC File Offset: 0x00309CBC
		public bool StationReadyToCharge()
		{
			return base.smi.sm.GetSweepLocker(base.smi) != null && base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}
	}

	// Token: 0x02001140 RID: 4416
	public class ChargingStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State
	{
		// Token: 0x04005F92 RID: 24466
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State waitingForCharging;

		// Token: 0x04005F93 RID: 24467
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State charging;

		// Token: 0x04005F94 RID: 24468
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State interupted;

		// Token: 0x04005F95 RID: 24469
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State completed;
	}
}
