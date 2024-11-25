using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class RescueSweepBotChore : Chore<RescueSweepBotChore.StatesInstance>
{
	// Token: 0x06001762 RID: 5986 RVA: 0x0007EB68 File Offset: 0x0007CD68
	public RescueSweepBotChore(IStateMachineTarget master, GameObject sweepBot, GameObject baseStation)
	{
		Chore.Precondition canReachBaseStation = default(Chore.Precondition);
		canReachBaseStation.id = "CanReachBaseStation";
		canReachBaseStation.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		canReachBaseStation.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			return !(kmonoBehaviour == null) && context.consumerState.consumer.navigator.CanReach(Grid.PosToCell(kmonoBehaviour));
		};
		this.CanReachBaseStation = canReachBaseStation;
		base..ctor(Db.Get().ChoreTypes.RescueIncapacitated, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		base.smi = new RescueSweepBotChore.StatesInstance(this);
		this.runUntilComplete = true;
		this.AddPrecondition(RescueSweepBotChore.CanReachIncapacitated, sweepBot.GetComponent<Storage>());
		this.AddPrecondition(this.CanReachBaseStation, baseStation.GetComponent<Storage>());
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x0007EC20 File Offset: 0x0007CE20
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rescuer.Set(context.consumerState.gameObject, base.smi, false);
		base.smi.sm.rescueTarget.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverTarget.Set(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>().sm.GetSweepLocker(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>()).gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x0007ECC1 File Offset: 0x0007CEC1
	protected override void End(string reason)
	{
		this.DropSweepBot();
		base.End(reason);
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x0007ECD0 File Offset: 0x0007CED0
	private void DropSweepBot()
	{
		if (base.smi.sm.rescuer.Get(base.smi) != null && base.smi.sm.rescueTarget.Get(base.smi) != null)
		{
			base.smi.sm.rescuer.Get(base.smi).GetComponent<Storage>().Drop(base.smi.sm.rescueTarget.Get(base.smi), true);
		}
	}

	// Token: 0x04000D17 RID: 3351
	public Chore.Precondition CanReachBaseStation;

	// Token: 0x04000D18 RID: 3352
	public static Chore.Precondition CanReachIncapacitated = new Chore.Precondition
	{
		id = "CanReachIncapacitated",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			if (kmonoBehaviour == null)
			{
				return false;
			}
			int navigationCost = context.consumerState.navigator.GetNavigationCost(Grid.PosToCell(kmonoBehaviour.transform.GetPosition()));
			if (-1 != navigationCost)
			{
				context.cost += navigationCost;
				return true;
			}
			return false;
		}
	};

	// Token: 0x020011F5 RID: 4597
	public class StatesInstance : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.GameInstance
	{
		// Token: 0x060081B4 RID: 33204 RVA: 0x00318BAE File Offset: 0x00316DAE
		public StatesInstance(RescueSweepBotChore master) : base(master)
		{
		}
	}

	// Token: 0x020011F6 RID: 4598
	public class States : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore>
	{
		// Token: 0x060081B5 RID: 33205 RVA: 0x00318BB8 File Offset: 0x00316DB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approachSweepBot;
			this.approachSweepBot.InitializeStates(this.rescuer, this.rescueTarget, this.holding.pickup, this.failure, Grid.DefaultOffset, null);
			this.holding.Target(this.rescuer).Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				if (this.rescuer.Get(smi).gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().AddAnimOverrides(anim, 0f);
				}
			}).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				if (this.rescuer.Get(smi).gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
				}
			});
			this.holding.pickup.Target(this.rescuer).PlayAnim("pickup").Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
			}).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				this.rescuer.Get(smi).GetComponent<Storage>().Store(this.rescueTarget.Get(smi), false, false, true, false);
				this.rescueTarget.Get(smi).transform.SetLocalPosition(Vector3.zero);
				KBatchedAnimTracker component = this.rescueTarget.Get(smi).GetComponent<KBatchedAnimTracker>();
				if (component != null)
				{
					component.symbol = new HashedString("snapTo_pivot");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			}).EventTransition(GameHashes.AnimQueueComplete, this.holding.delivering, null);
			this.holding.delivering.InitializeStates(this.rescuer, this.deliverTarget, this.holding.deposit, this.holding.ditch, null, null).Update(delegate(RescueSweepBotChore.StatesInstance smi, float dt)
			{
				if (this.deliverTarget.Get(smi) == null)
				{
					smi.GoTo(this.holding.ditch);
				}
			}, UpdateRate.SIM_200ms, false);
			this.holding.deposit.PlayAnim("place").EventHandler(GameHashes.AnimQueueComplete, delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.master.DropSweepBot();
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("complete");
			});
			this.holding.ditch.PlayAnim("place").ScheduleGoTo(0.5f, this.failure).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.master.DropSweepBot();
			});
			this.failure.Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("failed");
			});
		}

		// Token: 0x040061E8 RID: 25064
		public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<Storage> approachSweepBot;

		// Token: 0x040061E9 RID: 25065
		public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State failure;

		// Token: 0x040061EA RID: 25066
		public RescueSweepBotChore.States.HoldingSweepBot holding;

		// Token: 0x040061EB RID: 25067
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescueTarget;

		// Token: 0x040061EC RID: 25068
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter deliverTarget;

		// Token: 0x040061ED RID: 25069
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescuer;

		// Token: 0x020023E6 RID: 9190
		public class HoldingSweepBot : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State
		{
			// Token: 0x0400A03A RID: 41018
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State pickup;

			// Token: 0x0400A03B RID: 41019
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<IApproachable> delivering;

			// Token: 0x0400A03C RID: 41020
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State deposit;

			// Token: 0x0400A03D RID: 41021
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State ditch;
		}
	}
}
