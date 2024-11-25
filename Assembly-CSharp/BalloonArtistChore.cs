using System;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class BalloonArtistChore : Chore<BalloonArtistChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060016C0 RID: 5824 RVA: 0x0007A00C File Offset: 0x0007820C
	public BalloonArtistChore(IStateMachineTarget target)
	{
		Chore.Precondition hasBalloonStallCell = default(Chore.Precondition);
		hasBalloonStallCell.id = "HasBalloonStallCell";
		hasBalloonStallCell.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_BALLOON_STALL_CELL;
		hasBalloonStallCell.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((BalloonArtistChore)data).smi.HasBalloonStallCell();
		};
		this.HasBalloonStallCell = hasBalloonStallCell;
		base..ctor(Db.Get().ChoreTypes.JoyReaction, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime);
		this.showAvailabilityInHoverText = false;
		base.smi = new BalloonArtistChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(this.HasBalloonStallCell, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x060016C1 RID: 5825 RVA: 0x0007A105 File Offset: 0x00078305
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000CAD RID: 3245
	private int basePriority = RELAXATION.PRIORITY.TIER1;

	// Token: 0x04000CAE RID: 3246
	private Chore.Precondition HasBalloonStallCell;

	// Token: 0x02001197 RID: 4503
	public class States : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore>
	{
		// Token: 0x06008049 RID: 32841 RVA: 0x0030FB00 File Offset: 0x0030DD00
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.goToStand;
			base.Target(this.balloonArtist);
			this.root.EventTransition(GameHashes.ScheduleBlocksChanged, this.idle, (BalloonArtistChore.StatesInstance smi) => !smi.IsRecTime());
			this.idle.DoNothing();
			this.goToStand.Transition(null, (BalloonArtistChore.StatesInstance smi) => !smi.HasBalloonStallCell(), UpdateRate.SIM_200ms).MoveTo((BalloonArtistChore.StatesInstance smi) => smi.GetBalloonStallCell(), this.balloonStand, null, false);
			this.balloonStand.ToggleAnims("anim_interacts_balloon_artist_kanim", 0f).Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.SpawnBalloonStand();
			}).Enter(delegate(BalloonArtistChore.StatesInstance smi)
			{
				this.balloonArtist.GetSMI<BalloonArtist.Instance>(smi).Internal_InitBalloons();
			}).Exit(delegate(BalloonArtistChore.StatesInstance smi)
			{
				smi.DestroyBalloonStand();
			}).DefaultState(this.balloonStand.idle);
			this.balloonStand.idle.PlayAnim("working_pre").QueueAnim("working_loop", true, null).OnSignal(this.giveBalloonOut, this.balloonStand.giveBalloon);
			this.balloonStand.giveBalloon.PlayAnim("working_pst").OnAnimQueueComplete(this.balloonStand.idle);
		}

		// Token: 0x0400607E RID: 24702
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.TargetParameter balloonArtist;

		// Token: 0x0400607F RID: 24703
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter balloonsGivenOut = new StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.IntParameter(0);

		// Token: 0x04006080 RID: 24704
		public StateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.Signal giveBalloonOut;

		// Token: 0x04006081 RID: 24705
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

		// Token: 0x04006082 RID: 24706
		public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State goToStand;

		// Token: 0x04006083 RID: 24707
		public BalloonArtistChore.States.BalloonStandStates balloonStand;

		// Token: 0x020023A2 RID: 9122
		public class BalloonStandStates : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State
		{
			// Token: 0x04009F2B RID: 40747
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State idle;

			// Token: 0x04009F2C RID: 40748
			public GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.State giveBalloon;
		}
	}

	// Token: 0x02001198 RID: 4504
	public class StatesInstance : GameStateMachine<BalloonArtistChore.States, BalloonArtistChore.StatesInstance, BalloonArtistChore, object>.GameInstance
	{
		// Token: 0x0600804C RID: 32844 RVA: 0x0030FCC0 File Offset: 0x0030DEC0
		public StatesInstance(BalloonArtistChore master, GameObject balloonArtist) : base(master)
		{
			this.balloonArtist = balloonArtist;
			base.sm.balloonArtist.Set(balloonArtist, base.smi, false);
		}

		// Token: 0x0600804D RID: 32845 RVA: 0x0030FCE9 File Offset: 0x0030DEE9
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x0600804E RID: 32846 RVA: 0x0030FD0A File Offset: 0x0030DF0A
		public int GetBalloonStallCell()
		{
			return this.balloonArtistCellSensor.GetCell();
		}

		// Token: 0x0600804F RID: 32847 RVA: 0x0030FD17 File Offset: 0x0030DF17
		public int GetBalloonStallTargetCell()
		{
			return this.balloonArtistCellSensor.GetStandCell();
		}

		// Token: 0x06008050 RID: 32848 RVA: 0x0030FD24 File Offset: 0x0030DF24
		public bool HasBalloonStallCell()
		{
			if (this.balloonArtistCellSensor == null)
			{
				this.balloonArtistCellSensor = base.GetComponent<Sensors>().GetSensor<BalloonStandCellSensor>();
			}
			return this.balloonArtistCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x06008051 RID: 32849 RVA: 0x0030FD54 File Offset: 0x0030DF54
		public bool IsSameRoom()
		{
			int cell = Grid.PosToCell(this.balloonArtist);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(this.GetBalloonStallCell());
			return cavityForCell != null && cavityForCell2 != null && cavityForCell.handle == cavityForCell2.handle;
		}

		// Token: 0x06008052 RID: 32850 RVA: 0x0030FDB0 File Offset: 0x0030DFB0
		public void SpawnBalloonStand()
		{
			Vector3 vector = Grid.CellToPos(this.GetBalloonStallTargetCell());
			this.balloonArtist.GetComponent<Facing>().Face(vector);
			this.balloonStand = Util.KInstantiate(Assets.GetPrefab("BalloonStand"), vector, Quaternion.identity, null, null, true, 0);
			this.balloonStand.SetActive(true);
			this.balloonStand.GetComponent<GetBalloonWorkable>().SetBalloonArtist(base.smi);
		}

		// Token: 0x06008053 RID: 32851 RVA: 0x0030FE20 File Offset: 0x0030E020
		public void DestroyBalloonStand()
		{
			this.balloonStand.DeleteObject();
		}

		// Token: 0x06008054 RID: 32852 RVA: 0x0030FE2D File Offset: 0x0030E02D
		public BalloonOverrideSymbol GetBalloonOverride()
		{
			return this.balloonArtist.GetSMI<BalloonArtist.Instance>().GetCurrentBalloonSymbolOverride();
		}

		// Token: 0x06008055 RID: 32853 RVA: 0x0030FE3F File Offset: 0x0030E03F
		public void NextBalloonOverride()
		{
			this.balloonArtist.GetSMI<BalloonArtist.Instance>().ApplyNextBalloonSymbolOverride();
		}

		// Token: 0x06008056 RID: 32854 RVA: 0x0030FE54 File Offset: 0x0030E054
		public void GiveBalloon(BalloonOverrideSymbol balloonOverride)
		{
			BalloonArtist.Instance smi = this.balloonArtist.GetSMI<BalloonArtist.Instance>();
			smi.GiveBalloon();
			balloonOverride.ApplyTo(smi);
			base.smi.sm.giveBalloonOut.Trigger(base.smi);
		}

		// Token: 0x04006084 RID: 24708
		private BalloonStandCellSensor balloonArtistCellSensor;

		// Token: 0x04006085 RID: 24709
		private GameObject balloonArtist;

		// Token: 0x04006086 RID: 24710
		private GameObject balloonStand;
	}
}
