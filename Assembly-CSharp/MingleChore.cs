using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public class MingleChore : Chore<MingleChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001731 RID: 5937 RVA: 0x0007D640 File Offset: 0x0007B840
	public MingleChore(IStateMachineTarget target)
	{
		Chore.Precondition hasMingleCell = default(Chore.Precondition);
		hasMingleCell.id = "HasMingleCell";
		hasMingleCell.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_MINGLE_CELL;
		hasMingleCell.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((MingleChore)data).smi.HasMingleCell();
		};
		this.HasMingleCell = hasMingleCell;
		base..ctor(Db.Get().ChoreTypes.Relax, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime);
		this.showAvailabilityInHoverText = false;
		base.smi = new MingleChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(this.HasMingleCell, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x0007D739 File Offset: 0x0007B939
	protected override StatusItem GetStatusItem()
	{
		return Db.Get().DuplicantStatusItems.Mingling;
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x0007D74A File Offset: 0x0007B94A
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000D09 RID: 3337
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x04000D0A RID: 3338
	private Chore.Precondition HasMingleCell;

	// Token: 0x020011CB RID: 4555
	public class States : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore>
	{
		// Token: 0x06008121 RID: 33057 RVA: 0x00315C50 File Offset: 0x00313E50
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.mingle;
			base.Target(this.mingler);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, null, (MingleChore.StatesInstance smi) => !smi.IsRecTime());
			this.mingle.Transition(this.walk, (MingleChore.StatesInstance smi) => smi.IsSameRoom(), UpdateRate.SIM_200ms).Transition(this.move, (MingleChore.StatesInstance smi) => !smi.IsSameRoom(), UpdateRate.SIM_200ms);
			this.move.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.walk.Transition(null, (MingleChore.StatesInstance smi) => !smi.HasMingleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f).MoveTo((MingleChore.StatesInstance smi) => smi.GetMingleCell(), this.onfloor, null, false);
			this.onfloor.ToggleAnims("anim_generic_convo_kanim", 0f).PlayAnim("idle", KAnim.PlayMode.Loop).ScheduleGoTo((MingleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 10), this.success).ToggleTag(GameTags.AlwaysConverse);
			this.success.ReturnSuccess();
		}

		// Token: 0x0400616F RID: 24943
		public StateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.TargetParameter mingler;

		// Token: 0x04006170 RID: 24944
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State mingle;

		// Token: 0x04006171 RID: 24945
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State move;

		// Token: 0x04006172 RID: 24946
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State walk;

		// Token: 0x04006173 RID: 24947
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State onfloor;

		// Token: 0x04006174 RID: 24948
		public GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.State success;
	}

	// Token: 0x020011CC RID: 4556
	public class StatesInstance : GameStateMachine<MingleChore.States, MingleChore.StatesInstance, MingleChore, object>.GameInstance
	{
		// Token: 0x06008123 RID: 33059 RVA: 0x00315E3F File Offset: 0x0031403F
		public StatesInstance(MingleChore master, GameObject mingler) : base(master)
		{
			this.mingler = mingler;
			base.sm.mingler.Set(mingler, base.smi, false);
			this.mingleCellSensor = base.GetComponent<Sensors>().GetSensor<MingleCellSensor>();
		}

		// Token: 0x06008124 RID: 33060 RVA: 0x00315E79 File Offset: 0x00314079
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06008125 RID: 33061 RVA: 0x00315E9A File Offset: 0x0031409A
		public int GetMingleCell()
		{
			return this.mingleCellSensor.GetCell();
		}

		// Token: 0x06008126 RID: 33062 RVA: 0x00315EA7 File Offset: 0x003140A7
		public bool HasMingleCell()
		{
			return this.mingleCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x06008127 RID: 33063 RVA: 0x00315EC0 File Offset: 0x003140C0
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.mingler);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetMingleCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x04006175 RID: 24949
		private MingleCellSensor mingleCellSensor;

		// Token: 0x04006176 RID: 24950
		private GameObject mingler;
	}
}
