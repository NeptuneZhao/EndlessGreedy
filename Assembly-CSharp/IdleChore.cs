using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
public class IdleChore : Chore<IdleChore.StatesInstance>
{
	// Token: 0x06001730 RID: 5936 RVA: 0x0007D5F0 File Offset: 0x0007B7F0
	public IdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Idle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.IdleTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new IdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x020011C9 RID: 4553
	public class StatesInstance : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.GameInstance
	{
		// Token: 0x06008117 RID: 33047 RVA: 0x003157E5 File Offset: 0x003139E5
		public StatesInstance(IdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
			this.idleCellSensor = base.GetComponent<Sensors>().GetSensor<IdleCellSensor>();
		}

		// Token: 0x06008118 RID: 33048 RVA: 0x00315818 File Offset: 0x00313A18
		public void UpdateNavType()
		{
			NavType currentNavType = base.GetComponent<Navigator>().CurrentNavType;
			base.sm.isOnLadder.Set(currentNavType == NavType.Ladder || currentNavType == NavType.Pole, this, false);
			base.sm.isOnTube.Set(currentNavType == NavType.Tube, this, false);
			int num = Grid.PosToCell(base.smi);
			base.sm.isOnSuitMarkerCell.Set(Grid.IsValidCell(num) && Grid.HasSuitMarker[num], this, false);
		}

		// Token: 0x06008119 RID: 33049 RVA: 0x0031589B File Offset: 0x00313A9B
		public int GetIdleCell()
		{
			return this.idleCellSensor.GetCell();
		}

		// Token: 0x0600811A RID: 33050 RVA: 0x003158A8 File Offset: 0x00313AA8
		public bool HasIdleCell()
		{
			return this.idleCellSensor.GetCell() != Grid.InvalidCell;
		}

		// Token: 0x04006169 RID: 24937
		private IdleCellSensor idleCellSensor;
	}

	// Token: 0x020011CA RID: 4554
	public class States : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore>
	{
		// Token: 0x0600811B RID: 33051 RVA: 0x003158C0 File Offset: 0x00313AC0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.DefaultState(this.idle.onfloor).Enter("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				smi.UpdateNavType();
			}).Update("UpdateNavType", delegate(IdleChore.StatesInstance smi, float dt)
			{
				smi.UpdateNavType();
			}, UpdateRate.SIM_200ms, false).ToggleStateMachine((IdleChore.StatesInstance smi) => new TaskAvailabilityMonitor.Instance(smi.master)).ToggleTag(GameTags.Idle);
			this.idle.onfloor.PlayAnim("idle_default", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isOnLadder, this.idle.onladder, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnTube, this.idle.ontube, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ParamTransition<bool>(this.isOnSuitMarkerCell, this.idle.onsuitmarker, GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.IsTrue).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.onladder.PlayAnim("ladder_idle", KAnim.PlayMode.Loop).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.ontube.PlayAnim("tube_idle_loop", KAnim.PlayMode.Loop).Update("IdleMove", delegate(IdleChore.StatesInstance smi, float dt)
			{
				if (smi.HasIdleCell())
				{
					smi.GoTo(this.idle.move);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.idle.onsuitmarker.PlayAnim("idle_default", KAnim.PlayMode.Loop).Enter(delegate(IdleChore.StatesInstance smi)
			{
				Navigator component = smi.GetComponent<Navigator>();
				int cell = Grid.PosToCell(component);
				Grid.SuitMarker.Flags flags;
				PathFinder.PotentialPath.Flags flags2;
				Grid.TryGetSuitMarkerFlags(cell, out flags, out flags2);
				IdleSuitMarkerCellQuery idleSuitMarkerCellQuery = new IdleSuitMarkerCellQuery((flags & Grid.SuitMarker.Flags.Rotated) > (Grid.SuitMarker.Flags)0, Grid.CellToXY(cell).X);
				component.RunQuery(idleSuitMarkerCellQuery);
				component.GoTo(idleSuitMarkerCellQuery.GetResultCell(), null);
			}).EventTransition(GameHashes.DestinationReached, this.idle, null).ToggleScheduleCallback("IdleMove", (IdleChore.StatesInstance smi) => (float)UnityEngine.Random.Range(5, 15), delegate(IdleChore.StatesInstance smi)
			{
				smi.GoTo(this.idle.move);
			});
			this.idle.move.Transition(this.idle, (IdleChore.StatesInstance smi) => !smi.HasIdleCell(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.BeginWalk, null).TriggerOnExit(GameHashes.EndWalk, null).ToggleAnims("anim_loco_walk_kanim", 0f).MoveTo((IdleChore.StatesInstance smi) => smi.GetIdleCell(), this.idle, this.idle, false).Exit("UpdateNavType", delegate(IdleChore.StatesInstance smi)
			{
				smi.UpdateNavType();
			}).Exit("ClearWalk", delegate(IdleChore.StatesInstance smi)
			{
				smi.GetComponent<KBatchedAnimController>().Play("idle_default", KAnim.PlayMode.Once, 1f, 0f);
			});
		}

		// Token: 0x0400616A RID: 24938
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnLadder;

		// Token: 0x0400616B RID: 24939
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnTube;

		// Token: 0x0400616C RID: 24940
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.BoolParameter isOnSuitMarkerCell;

		// Token: 0x0400616D RID: 24941
		public StateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.TargetParameter idler;

		// Token: 0x0400616E RID: 24942
		public IdleChore.States.IdleState idle;

		// Token: 0x020023CA RID: 9162
		public class IdleState : GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State
		{
			// Token: 0x04009FC1 RID: 40897
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onfloor;

			// Token: 0x04009FC2 RID: 40898
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onladder;

			// Token: 0x04009FC3 RID: 40899
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State ontube;

			// Token: 0x04009FC4 RID: 40900
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State onsuitmarker;

			// Token: 0x04009FC5 RID: 40901
			public GameStateMachine<IdleChore.States, IdleChore.StatesInstance, IdleChore, object>.State move;
		}
	}
}
