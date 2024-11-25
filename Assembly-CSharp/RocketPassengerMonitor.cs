using System;

// Token: 0x02000999 RID: 2457
public class RocketPassengerMonitor : GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance>
{
	// Token: 0x0600479E RID: 18334 RVA: 0x0019A0BC File Offset: 0x001982BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.satisfied.ParamTransition<int>(this.targetCell, this.moving, (RocketPassengerMonitor.Instance smi, int p) => p != Grid.InvalidCell);
		this.moving.ParamTransition<int>(this.targetCell, this.satisfied, (RocketPassengerMonitor.Instance smi, int p) => p == Grid.InvalidCell).ToggleChore((RocketPassengerMonitor.Instance smi) => this.CreateChore(smi), this.satisfied).Exit(delegate(RocketPassengerMonitor.Instance smi)
		{
			this.targetCell.Set(Grid.InvalidCell, smi, false);
		});
		this.movingToModuleDeployPre.Enter(delegate(RocketPassengerMonitor.Instance smi)
		{
			this.targetCell.Set(smi.moduleDeployTaskTargetMoveCell, smi, false);
			smi.GoTo(this.movingToModuleDeploy);
		});
		this.movingToModuleDeploy.ParamTransition<int>(this.targetCell, this.satisfied, (RocketPassengerMonitor.Instance smi, int p) => p == Grid.InvalidCell).ToggleChore((RocketPassengerMonitor.Instance smi) => this.CreateChore(smi), this.moduleDeploy);
		this.moduleDeploy.Enter(delegate(RocketPassengerMonitor.Instance smi)
		{
			smi.moduleDeployCompleteCallback(null);
			this.targetCell.Set(Grid.InvalidCell, smi, false);
			smi.moduleDeployCompleteCallback = null;
			smi.GoTo(smi.sm.satisfied);
		});
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x0019A1EC File Offset: 0x001983EC
	public Chore CreateChore(RocketPassengerMonitor.Instance smi)
	{
		MoveChore moveChore = new MoveChore(smi.master, Db.Get().ChoreTypes.RocketEnterExit, (MoveChore.StatesInstance mover_smi) => this.targetCell.Get(smi), false);
		moveChore.AddPrecondition(ChorePreconditions.instance.CanMoveToCell, this.targetCell.Get(smi));
		return moveChore;
	}

	// Token: 0x04002ECE RID: 11982
	public StateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.IntParameter targetCell = new StateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.IntParameter(Grid.InvalidCell);

	// Token: 0x04002ECF RID: 11983
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002ED0 RID: 11984
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State moving;

	// Token: 0x04002ED1 RID: 11985
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State movingToModuleDeployPre;

	// Token: 0x04002ED2 RID: 11986
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State movingToModuleDeploy;

	// Token: 0x04002ED3 RID: 11987
	public GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.State moduleDeploy;

	// Token: 0x02001971 RID: 6513
	public new class Instance : GameStateMachine<RocketPassengerMonitor, RocketPassengerMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009C98 RID: 40088 RVA: 0x00372B1B File Offset: 0x00370D1B
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009C99 RID: 40089 RVA: 0x00372B24 File Offset: 0x00370D24
		public bool ShouldMoveThroughRocketDoor()
		{
			int num = base.sm.targetCell.Get(this);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if ((int)Grid.WorldIdx[num] == this.GetMyWorldId())
			{
				base.sm.targetCell.Set(Grid.InvalidCell, this, false);
				return false;
			}
			return true;
		}

		// Token: 0x06009C9A RID: 40090 RVA: 0x00372B77 File Offset: 0x00370D77
		public void SetMoveTarget(int cell)
		{
			if ((int)Grid.WorldIdx[cell] == this.GetMyWorldId())
			{
				return;
			}
			base.sm.targetCell.Set(cell, this, false);
		}

		// Token: 0x06009C9B RID: 40091 RVA: 0x00372B9D File Offset: 0x00370D9D
		public void SetModuleDeployChore(int cell, Action<Chore> OnChoreCompleteCallback)
		{
			this.moduleDeployCompleteCallback = OnChoreCompleteCallback;
			this.moduleDeployTaskTargetMoveCell = cell;
			this.GoTo(base.sm.movingToModuleDeployPre);
			base.sm.targetCell.Set(cell, this, false);
		}

		// Token: 0x06009C9C RID: 40092 RVA: 0x00372BD2 File Offset: 0x00370DD2
		public void CancelModuleDeployChore()
		{
			this.moduleDeployCompleteCallback = null;
			this.moduleDeployTaskTargetMoveCell = Grid.InvalidCell;
			base.sm.targetCell.Set(Grid.InvalidCell, base.smi, false);
		}

		// Token: 0x06009C9D RID: 40093 RVA: 0x00372C04 File Offset: 0x00370E04
		public void ClearMoveTarget(int testCell)
		{
			int num = base.sm.targetCell.Get(this);
			if (Grid.IsValidCell(num) && Grid.WorldIdx[num] == Grid.WorldIdx[testCell])
			{
				base.sm.targetCell.Set(Grid.InvalidCell, this, false);
				if (base.IsInsideState(base.sm.moving))
				{
					this.GoTo(base.sm.satisfied);
				}
			}
		}

		// Token: 0x04007974 RID: 31092
		public int lastWorldID;

		// Token: 0x04007975 RID: 31093
		public Action<Chore> moduleDeployCompleteCallback;

		// Token: 0x04007976 RID: 31094
		public int moduleDeployTaskTargetMoveCell;
	}
}
