﻿using System;
using UnityEngine;

// Token: 0x02000461 RID: 1121
public class WorkChore<WorkableType> : Chore<WorkChore<WorkableType>.StatesInstance> where WorkableType : Workable
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06001788 RID: 6024 RVA: 0x0007F9AF File Offset: 0x0007DBAF
	// (set) Token: 0x06001789 RID: 6025 RVA: 0x0007F9B7 File Offset: 0x0007DBB7
	public bool onlyWhenOperational { get; private set; }

	// Token: 0x0600178A RID: 6026 RVA: 0x0007F9C0 File Offset: 0x0007DBC0
	public override string ToString()
	{
		return "WorkChore<" + typeof(WorkableType).ToString() + ">";
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x0007F9E0 File Offset: 0x0007DBE0
	public WorkChore(ChoreType chore_type, IStateMachineTarget target, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, bool allow_in_red_alert = true, ScheduleBlockType schedule_block = null, bool ignore_schedule_block = false, bool only_when_operational = true, KAnimFile override_anims = null, bool is_preemptable = false, bool allow_in_context_menu = true, bool allow_prioritization = true, PriorityScreen.PriorityClass priority_class = PriorityScreen.PriorityClass.basic, int priority_class_value = 5, bool ignore_building_assignment = false, bool add_to_daily_report = true) : base(chore_type, target, chore_provider, run_until_complete, on_complete, on_begin, on_end, priority_class, priority_class_value, is_preemptable, allow_in_context_menu, 0, add_to_daily_report, ReportManager.ReportType.WorkTime)
	{
		base.smi = new WorkChore<WorkableType>.StatesInstance(this, target.gameObject, override_anims);
		this.onlyWhenOperational = only_when_operational;
		if (allow_prioritization)
		{
			base.SetPrioritizable(target.GetComponent<Prioritizable>());
		}
		this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, null);
		if (!allow_in_red_alert)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		}
		if (schedule_block != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, schedule_block);
		}
		else if (!ignore_schedule_block)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		}
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, base.smi.sm.workable.Get<WorkableType>(base.smi));
		Operational component = target.GetComponent<Operational>();
		if (only_when_operational && component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		}
		if (only_when_operational)
		{
			Deconstructable component2 = target.GetComponent<Deconstructable>();
			if (component2 != null)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component2);
			}
			BuildingEnabledButton component3 = target.GetComponent<BuildingEnabledButton>();
			if (component3 != null)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component3);
			}
		}
		if (!ignore_building_assignment && base.smi.sm.workable.Get(base.smi).GetComponent<Assignable>() != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, base.smi.sm.workable.Get<Assignable>(base.smi));
		}
		WorkableType workableType = target as WorkableType;
		if (workableType != null)
		{
			if (!string.IsNullOrEmpty(workableType.requiredSkillPerk))
			{
				HashedString hashedString = workableType.requiredSkillPerk;
				this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, hashedString);
			}
			if (workableType.requireMinionToWork)
			{
				this.AddPrecondition(ChorePreconditions.instance.IsMinion, null);
			}
		}
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x0007FC07 File Offset: 0x0007DE07
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.worker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x0007FC38 File Offset: 0x0007DE38
	public override bool IsValid()
	{
		WorkableType workableType = this.target as WorkableType;
		if (workableType != null)
		{
			return this.provider != null && Grid.IsWorldValidCell(workableType.GetCell());
		}
		return base.IsValid();
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x0007FC8C File Offset: 0x0007DE8C
	public bool IsOperationalValid()
	{
		if (this.onlyWhenOperational)
		{
			Operational component = base.smi.master.GetComponent<Operational>();
			if (component != null && !component.IsOperational)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x0007FCC8 File Offset: 0x0007DEC8
	public override bool CanPreempt(Chore.Precondition.Context context)
	{
		if (!base.CanPreempt(context))
		{
			return false;
		}
		if (context.chore.driver == null)
		{
			return false;
		}
		if (context.chore.driver == context.consumerState.choreDriver)
		{
			return false;
		}
		Workable workable = base.smi.sm.workable.Get<WorkableType>(base.smi);
		if (workable == null)
		{
			return false;
		}
		if (workable.worker != null && (workable.worker.GetState() == WorkerBase.State.PendingCompletion || workable.worker.GetState() == WorkerBase.State.Completing))
		{
			return false;
		}
		if (this.preemption_cb != null)
		{
			if (!this.preemption_cb(context))
			{
				return false;
			}
		}
		else
		{
			int num = 4;
			int navigationCost = context.chore.driver.GetComponent<Navigator>().GetNavigationCost(workable);
			if (navigationCost == -1 || navigationCost < num)
			{
				return false;
			}
			if (context.consumerState.navigator.GetNavigationCost(workable) * 2 > navigationCost)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04000D23 RID: 3363
	public Func<Chore.Precondition.Context, bool> preemption_cb;

	// Token: 0x02001215 RID: 4629
	public class StatesInstance : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.GameInstance
	{
		// Token: 0x06008220 RID: 33312 RVA: 0x0031BD44 File Offset: 0x00319F44
		public StatesInstance(WorkChore<WorkableType> master, GameObject workable, KAnimFile override_anims) : base(master)
		{
			this.overrideAnims = override_anims;
			base.sm.workable.Set(workable, base.smi, false);
		}

		// Token: 0x06008221 RID: 33313 RVA: 0x0031BD6D File Offset: 0x00319F6D
		public void EnableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).AddAnimOverrides(this.overrideAnims, 0f);
			}
		}

		// Token: 0x06008222 RID: 33314 RVA: 0x0031BDA3 File Offset: 0x00319FA3
		public void DisableAnimOverrides()
		{
			if (this.overrideAnims != null)
			{
				base.sm.worker.Get<KAnimControllerBase>(base.smi).RemoveAnimOverrides(this.overrideAnims);
			}
		}

		// Token: 0x04006256 RID: 25174
		private KAnimFile overrideAnims;
	}

	// Token: 0x02001216 RID: 4630
	public class States : GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>>
	{
		// Token: 0x06008223 RID: 33315 RVA: 0x0031BDD4 File Offset: 0x00319FD4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.worker);
			this.approach.InitializeStates(this.worker, this.workable, this.work, null, null, null).Update("CheckOperational", delegate(WorkChore<WorkableType>.StatesInstance smi, float dt)
			{
				if (!smi.master.IsOperationalValid())
				{
					smi.StopSM("Building not operational");
				}
			}, UpdateRate.SIM_200ms, false);
			this.work.Enter(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.EnableAnimOverrides();
			}).ToggleWork<WorkableType>(this.workable, this.success, null, (WorkChore<WorkableType>.StatesInstance smi) => smi.master.IsOperationalValid()).Exit(delegate(WorkChore<WorkableType>.StatesInstance smi)
			{
				smi.DisableAnimOverrides();
			});
			this.success.ReturnSuccess();
		}

		// Token: 0x04006257 RID: 25175
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.ApproachSubState<WorkableType> approach;

		// Token: 0x04006258 RID: 25176
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State work;

		// Token: 0x04006259 RID: 25177
		public GameStateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.State success;

		// Token: 0x0400625A RID: 25178
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter workable;

		// Token: 0x0400625B RID: 25179
		public StateMachine<WorkChore<WorkableType>.States, WorkChore<WorkableType>.StatesInstance, WorkChore<WorkableType>, object>.TargetParameter worker;
	}
}
