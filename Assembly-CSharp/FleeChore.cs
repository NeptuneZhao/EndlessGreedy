using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class FleeChore : Chore<FleeChore.StatesInstance>
{
	// Token: 0x06001728 RID: 5928 RVA: 0x0007D218 File Offset: 0x0007B418
	public FleeChore(IStateMachineTarget target, GameObject enemy) : base(Db.Get().ChoreTypes.Flee, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new FleeChore.StatesInstance(this);
		base.smi.sm.self.Set(this.gameObject, base.smi, false);
		this.nav = this.gameObject.GetComponent<Navigator>();
		base.smi.sm.fleeFromTarget.Set(enemy, base.smi, false);
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x0007D2AC File Offset: 0x0007B4AC
	private bool isInFavoredDirection(int cell, int fleeFromCell)
	{
		bool flag = Grid.CellToPos(fleeFromCell).x < this.gameObject.transform.GetPosition().x;
		bool flag2 = Grid.CellToPos(fleeFromCell).x < Grid.CellToPos(cell).x;
		return flag == flag2;
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x0007D300 File Offset: 0x0007B500
	private bool CanFleeTo(int cell)
	{
		return this.nav.CanReach(cell) || this.nav.CanReach(Grid.OffsetCell(cell, -1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, -1)) || this.nav.CanReach(Grid.OffsetCell(cell, -1, 1)) || this.nav.CanReach(Grid.OffsetCell(cell, 1, 1));
	}

	// Token: 0x0600172B RID: 5931 RVA: 0x0007D36F File Offset: 0x0007B56F
	public GameObject CreateLocator(Vector3 pos)
	{
		return ChoreHelpers.CreateLocator("GoToLocator", pos);
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x0007D37C File Offset: 0x0007B57C
	protected override void OnStateMachineStop(string reason, StateMachine.Status status)
	{
		if (base.smi.sm.fleeToTarget.Get(base.smi) != null)
		{
			ChoreHelpers.DestroyLocator(base.smi.sm.fleeToTarget.Get(base.smi));
		}
		base.OnStateMachineStop(reason, status);
	}

	// Token: 0x04000D07 RID: 3335
	private Navigator nav;

	// Token: 0x020011C4 RID: 4548
	public class StatesInstance : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.GameInstance
	{
		// Token: 0x0600810A RID: 33034 RVA: 0x00314FBA File Offset: 0x003131BA
		public StatesInstance(FleeChore master) : base(master)
		{
		}
	}

	// Token: 0x020011C5 RID: 4549
	public class States : GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore>
	{
		// Token: 0x0600810B RID: 33035 RVA: 0x00314FC4 File Offset: 0x003131C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.planFleeRoute;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.Fleeing, null);
			this.planFleeRoute.Enter(delegate(FleeChore.StatesInstance smi)
			{
				int num = Grid.PosToCell(this.fleeFromTarget.Get(smi));
				HashSet<int> hashSet = GameUtil.FloodCollectCells(Grid.PosToCell(smi.master.gameObject), new Func<int, bool>(smi.master.CanFleeTo), 300, null, true);
				int num2 = -1;
				int num3 = -1;
				foreach (int num4 in hashSet)
				{
					if (smi.master.nav.CanReach(num4))
					{
						int num5 = -1;
						num5 += Grid.GetCellDistance(num4, num);
						if (smi.master.isInFavoredDirection(num4, num))
						{
							num5 += 8;
						}
						if (num5 > num3)
						{
							num3 = num5;
							num2 = num4;
						}
					}
				}
				int num6 = num2;
				if (num6 == -1)
				{
					smi.GoTo(this.cower);
					return;
				}
				smi.sm.fleeToTarget.Set(smi.master.CreateLocator(Grid.CellToPos(num6)), smi, false);
				smi.sm.fleeToTarget.Get(smi).name = "FleeLocator";
				if (num6 == num)
				{
					smi.GoTo(this.cower);
					return;
				}
				smi.GoTo(this.flee);
			});
			this.flee.InitializeStates(this.self, this.fleeToTarget, this.cower, this.cower, null, NavigationTactics.ReduceTravelDistance).ToggleAnims("anim_loco_run_insane_kanim", 2f);
			this.cower.ToggleAnims("anim_cringe_kanim", 4f).PlayAnim("cringe_pre").QueueAnim("cringe_loop", false, null).QueueAnim("cringe_pst", false, null).OnAnimQueueComplete(this.end);
			this.end.Enter(delegate(FleeChore.StatesInstance smi)
			{
				smi.StopSM("stopped");
			});
		}

		// Token: 0x04006153 RID: 24915
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeFromTarget;

		// Token: 0x04006154 RID: 24916
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter fleeToTarget;

		// Token: 0x04006155 RID: 24917
		public StateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.TargetParameter self;

		// Token: 0x04006156 RID: 24918
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State planFleeRoute;

		// Token: 0x04006157 RID: 24919
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.ApproachSubState<IApproachable> flee;

		// Token: 0x04006158 RID: 24920
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State cower;

		// Token: 0x04006159 RID: 24921
		public GameStateMachine<FleeChore.States, FleeChore.StatesInstance, FleeChore, object>.State end;
	}
}
