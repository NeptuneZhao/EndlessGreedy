using System;
using TUNING;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class RoboDancerChore : Chore<RoboDancerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001767 RID: 5991 RVA: 0x0007EDB8 File Offset: 0x0007CFB8
	public RoboDancerChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new RoboDancerChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x0007EE52 File Offset: 0x0007D052
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000D19 RID: 3353
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x020011F8 RID: 4600
	public class States : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore>
	{
		// Token: 0x060081BF RID: 33215 RVA: 0x00318FD8 File Offset: 0x003171D8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.roboDancer);
			this.idle.EventTransition(GameHashes.ScheduleBlocksChanged, this.goToStand, (RoboDancerChore.StatesInstance smi) => !smi.IsRecTime());
			this.goToStand.MoveTo((RoboDancerChore.StatesInstance smi) => smi.GetTargetCell(), this.dancing, this.idle, false);
			this.dancing.ToggleEffect("Dancing").ToggleAnims("anim_bionic_joy_kanim", 0f).DefaultState(this.dancing.pre).Update(delegate(RoboDancerChore.StatesInstance smi, float dt)
			{
				RoboDancer.Instance smi2 = this.roboDancer.Get(smi).GetSMI<RoboDancer.Instance>();
				RoboDancer sm = smi2.sm;
				sm.timeSpentDancing.Set(sm.timeSpentDancing.Get(smi2) + dt, smi2, false);
			}, UpdateRate.SIM_33ms, false).Exit(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
			});
			this.dancing.pre.QueueAnim("robotdance_pre", false, null).OnAnimQueueComplete(this.dancing.variation_1).Enter(delegate(RoboDancerChore.StatesInstance smi)
			{
				smi.ClearAudienceWorkables();
				smi.CreateAudienceWorkables();
			});
			this.dancing.variation_1.QueueAnim("robotdance_loop", false, null).OnAnimQueueComplete(this.dancing.variation_2);
			this.dancing.variation_2.QueueAnim("robotdance_2_loop", false, null).OnAnimQueueComplete(this.dancing.pst);
			this.dancing.pst.QueueAnim("robotdance_pst", false, null).OnAnimQueueComplete(this.dancing.pre);
		}

		// Token: 0x040061F0 RID: 25072
		public StateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.TargetParameter roboDancer;

		// Token: 0x040061F1 RID: 25073
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State idle;

		// Token: 0x040061F2 RID: 25074
		public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State goToStand;

		// Token: 0x040061F3 RID: 25075
		public RoboDancerChore.States.DancingStates dancing;

		// Token: 0x020023E8 RID: 9192
		public class DancingStates : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State
		{
			// Token: 0x0400A043 RID: 41027
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pre;

			// Token: 0x0400A044 RID: 41028
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_1;

			// Token: 0x0400A045 RID: 41029
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State variation_2;

			// Token: 0x0400A046 RID: 41030
			public GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.State pst;
		}
	}

	// Token: 0x020011F9 RID: 4601
	public class StatesInstance : GameStateMachine<RoboDancerChore.States, RoboDancerChore.StatesInstance, RoboDancerChore, object>.GameInstance
	{
		// Token: 0x060081C2 RID: 33218 RVA: 0x003191DE File Offset: 0x003173DE
		public StatesInstance(RoboDancerChore master, GameObject roboDancer) : base(master)
		{
			this.roboDancer = roboDancer;
			base.sm.roboDancer.Set(roboDancer, base.smi, false);
		}

		// Token: 0x060081C3 RID: 33219 RVA: 0x00319213 File Offset: 0x00317413
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x060081C4 RID: 33220 RVA: 0x00319234 File Offset: 0x00317434
		public int GetTargetCell()
		{
			Navigator component = base.GetComponent<Navigator>();
			float num = float.MaxValue;
			SocialGatheringPoint socialGatheringPoint = null;
			foreach (SocialGatheringPoint socialGatheringPoint2 in Components.SocialGatheringPoints.GetItems((int)Grid.WorldIdx[Grid.PosToCell(this)]))
			{
				float num2 = (float)component.GetNavigationCost(Grid.PosToCell(socialGatheringPoint2));
				if (num2 != -1f && num2 < num)
				{
					num = num2;
					socialGatheringPoint = socialGatheringPoint2;
				}
			}
			if (socialGatheringPoint != null)
			{
				return Grid.PosToCell(socialGatheringPoint);
			}
			return Grid.PosToCell(base.master.gameObject);
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x003192E4 File Offset: 0x003174E4
		public void CreateAudienceWorkables()
		{
			int num = Grid.PosToCell(base.gameObject);
			Vector3Int[] array = new Vector3Int[]
			{
				Vector3Int.left * 3,
				Vector3Int.left * 2,
				Vector3Int.left,
				Vector3Int.right,
				Vector3Int.right * 2,
				Vector3Int.right * 3
			};
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				int cell = Grid.OffsetCell(num, array[i].x, array[i].y);
				if (Grid.IsValidCellInWorld(cell, (int)Grid.WorldIdx[num]))
				{
					GameObject gameObject = ChoreHelpers.CreateLocator("WatchRoboDancerWorkable", Grid.CellToPos(cell));
					this.audienceWorkables[i] = gameObject;
					KSelectable kselectable = gameObject.AddOrGet<KSelectable>();
					kselectable.SetName("WatchRoboDancerWorkable");
					kselectable.IsSelectable = false;
					WatchRoboDancerWorkable watchRoboDancerWorkable = gameObject.AddOrGet<WatchRoboDancerWorkable>();
					watchRoboDancerWorkable.owner = this.roboDancer;
					new WorkChore<WatchRoboDancerWorkable>(Db.Get().ChoreTypes.JoyReaction, watchRoboDancerWorkable, null, true, null, null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true).AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
				}
			}
		}

		// Token: 0x060081C6 RID: 33222 RVA: 0x00319440 File Offset: 0x00317640
		public void ClearAudienceWorkables()
		{
			for (int i = 0; i < this.audienceWorkables.Length; i++)
			{
				if (!(this.audienceWorkables[i] == null))
				{
					WorkerBase worker = this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().worker;
					if (worker != null)
					{
						this.audienceWorkables[i].GetComponent<WatchRoboDancerWorkable>().CompleteWork(worker);
					}
					ChoreHelpers.DestroyLocator(this.audienceWorkables[i]);
				}
			}
		}

		// Token: 0x040061F4 RID: 25076
		private GameObject roboDancer;

		// Token: 0x040061F5 RID: 25077
		private GameObject[] audienceWorkables = new GameObject[4];
	}
}
