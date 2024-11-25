using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000443 RID: 1091
public class MournChore : Chore<MournChore.StatesInstance>
{
	// Token: 0x06001734 RID: 5940 RVA: 0x0007D758 File Offset: 0x0007B958
	private static int GetStandableCell(int cell, Navigator navigator)
	{
		foreach (CellOffset offset in MournChore.ValidStandingOffsets)
		{
			if (Grid.IsCellOffsetValid(cell, offset))
			{
				int num = Grid.OffsetCell(cell, offset);
				if (!Grid.Reserved[num] && navigator.NavGrid.NavTable.IsValid(num, NavType.Floor) && navigator.GetNavigationCost(num) != -1)
				{
					return num;
				}
			}
		}
		return -1;
	}

	// Token: 0x06001735 RID: 5941 RVA: 0x0007D7C0 File Offset: 0x0007B9C0
	public MournChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.Mourn, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MournChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.NoDeadBodies, null);
		this.AddPrecondition(MournChore.HasValidMournLocation, master);
	}

	// Token: 0x06001736 RID: 5942 RVA: 0x0007D830 File Offset: 0x0007BA30
	public static Grave FindGraveToMournAt()
	{
		Grave result = null;
		float num = -1f;
		foreach (object obj in Components.Graves)
		{
			Grave grave = (Grave)obj;
			if (grave.burialTime > num)
			{
				num = grave.burialTime;
				result = grave;
			}
		}
		return result;
	}

	// Token: 0x06001737 RID: 5943 RVA: 0x0007D8A0 File Offset: 0x0007BAA0
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MournChore null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MournChore null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MournChore null smi.sm");
			return;
		}
		if (MournChore.FindGraveToMournAt() == null)
		{
			global::Debug.LogError("MournChore no grave");
			return;
		}
		base.smi.sm.mourner.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000D0B RID: 3339
	private static readonly CellOffset[] ValidStandingOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04000D0C RID: 3340
	private static readonly Chore.Precondition HasValidMournLocation = new Chore.Precondition
	{
		id = "HasPlaceToStand",
		description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_PLACE_TO_STAND,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Navigator component = ((IStateMachineTarget)data).GetComponent<Navigator>();
			bool result = false;
			Grave grave = MournChore.FindGraveToMournAt();
			if (grave != null && Grid.IsValidCell(MournChore.GetStandableCell(Grid.PosToCell(grave), component)))
			{
				result = true;
			}
			return result;
		}
	};

	// Token: 0x020011CE RID: 4558
	public class StatesInstance : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.GameInstance
	{
		// Token: 0x0600812B RID: 33067 RVA: 0x00315F3F File Offset: 0x0031413F
		public StatesInstance(MournChore master) : base(master)
		{
		}

		// Token: 0x0600812C RID: 33068 RVA: 0x00315F50 File Offset: 0x00314150
		public void CreateLocator()
		{
			int cell = Grid.PosToCell(MournChore.FindGraveToMournAt().transform.GetPosition());
			Navigator component = base.master.GetComponent<Navigator>();
			int standableCell = MournChore.GetStandableCell(cell, component);
			if (standableCell < 0)
			{
				base.smi.GoTo(null);
				return;
			}
			Grid.Reserved[standableCell] = true;
			Vector3 pos = Grid.CellToPosCBC(standableCell, Grid.SceneLayer.Move);
			GameObject value = ChoreHelpers.CreateLocator("MournLocator", pos);
			base.smi.sm.locator.Set(value, base.smi, false);
			this.locatorCell = standableCell;
			base.smi.GoTo(base.sm.moveto);
		}

		// Token: 0x0600812D RID: 33069 RVA: 0x00315FF4 File Offset: 0x003141F4
		public void DestroyLocator()
		{
			if (this.locatorCell >= 0)
			{
				Grid.Reserved[this.locatorCell] = false;
				ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
				base.sm.locator.Set(null, this);
				this.locatorCell = -1;
			}
		}

		// Token: 0x04006179 RID: 24953
		private int locatorCell = -1;
	}

	// Token: 0x020011CF RID: 4559
	public class States : GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore>
	{
		// Token: 0x0600812E RID: 33070 RVA: 0x0031604C File Offset: 0x0031424C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findOffset;
			base.Target(this.mourner);
			this.root.ToggleAnims("anim_react_mourning_kanim", 0f).Exit("DestroyLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.findOffset.Enter("CreateLocator", delegate(MournChore.StatesInstance smi)
			{
				smi.CreateLocator();
			});
			this.moveto.InitializeStates(this.mourner, this.locator, this.mourn, null, null, null);
			this.mourn.PlayAnims((MournChore.StatesInstance smi) => MournChore.States.WORK_ANIMS, KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.completed);
			this.completed.PlayAnim("working_pst").OnAnimQueueComplete(null).Exit(delegate(MournChore.StatesInstance smi)
			{
				this.mourner.Get<Effects>(smi).Remove(Db.Get().effects.Get("Mourning"));
			});
		}

		// Token: 0x0400617A RID: 24954
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter mourner;

		// Token: 0x0400617B RID: 24955
		public StateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.TargetParameter locator;

		// Token: 0x0400617C RID: 24956
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State findOffset;

		// Token: 0x0400617D RID: 24957
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.ApproachSubState<IApproachable> moveto;

		// Token: 0x0400617E RID: 24958
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State mourn;

		// Token: 0x0400617F RID: 24959
		public GameStateMachine<MournChore.States, MournChore.StatesInstance, MournChore, object>.State completed;

		// Token: 0x04006180 RID: 24960
		private static readonly HashedString[] WORK_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};
	}
}
