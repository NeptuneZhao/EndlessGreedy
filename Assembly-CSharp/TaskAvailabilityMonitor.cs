using System;

// Token: 0x020009A7 RID: 2471
public class TaskAvailabilityMonitor : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance>
{
	// Token: 0x060047EA RID: 18410 RVA: 0x0019BF64 File Offset: 0x0019A164
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventTransition(GameHashes.NewDay, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance, this.unavailable, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance.GetCycle() > 0);
		this.unavailable.Enter("RefreshStatusItem", delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).EventHandler(GameHashes.ScheduleChanged, delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		});
	}

	// Token: 0x04002F14 RID: 12052
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002F15 RID: 12053
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State unavailable;

	// Token: 0x02001995 RID: 6549
	public new class Instance : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D34 RID: 40244 RVA: 0x00374491 File Offset: 0x00372691
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06009D35 RID: 40245 RVA: 0x0037449C File Offset: 0x0037269C
		public void RefreshStatusItem()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			WorldContainer myWorld = base.gameObject.GetMyWorld();
			if (myWorld != null && myWorld.IsModuleInterior && myWorld.ParentWorldId == myWorld.id)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.IdleInRockets, null);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Idle, null);
		}
	}
}
