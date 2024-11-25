using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AA5 RID: 2725
[SerializationConfig(MemberSerialization.OptIn)]
public class SocialGatheringPoint : StateMachineComponent<SocialGatheringPoint.StatesInstance>
{
	// Token: 0x06005032 RID: 20530 RVA: 0x001CD064 File Offset: 0x001CB264
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workables = new SocialGatheringPointWorkable[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("SocialGatheringPointWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.basePriority = this.basePriority;
			socialGatheringPointWorkable.specificEffect = this.socialEffect;
			socialGatheringPointWorkable.OnWorkableEventCB = new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.tracker = new SocialChoreTracker(base.gameObject, this.choreOffsets);
		this.tracker.choreCount = this.choreCount;
		this.tracker.CreateChoreCB = new Func<int, Chore>(this.CreateChore);
		base.smi.StartSM();
		Components.SocialGatheringPoints.Add((int)Grid.WorldIdx[Grid.PosToCell(this)], this);
	}

	// Token: 0x06005033 RID: 20531 RVA: 0x001CD170 File Offset: 0x001CB370
	protected override void OnCleanUp()
	{
		if (this.tracker != null)
		{
			this.tracker.Clear();
			this.tracker = null;
		}
		if (this.workables != null)
		{
			for (int i = 0; i < this.workables.Length; i++)
			{
				if (this.workables[i])
				{
					Util.KDestroyGameObject(this.workables[i]);
					this.workables[i] = null;
				}
			}
		}
		Components.SocialGatheringPoints.Remove((int)Grid.WorldIdx[Grid.PosToCell(this)], this);
		base.OnCleanUp();
	}

	// Token: 0x06005034 RID: 20532 RVA: 0x001CD1F4 File Offset: 0x001CB3F4
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<SocialGatheringPointWorkable> workChore = new WorkChore<SocialGatheringPointWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, false);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x06005035 RID: 20533 RVA: 0x001CD27E File Offset: 0x001CB47E
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.smi.IsInsideState(base.smi.sm.on))
		{
			this.tracker.Update(true);
		}
	}

	// Token: 0x06005036 RID: 20534 RVA: 0x001CD2A9 File Offset: 0x001CB4A9
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			if (this.OnSocializeBeginCB != null)
			{
				this.OnSocializeBeginCB();
				return;
			}
		}
		else if (workable_event == Workable.WorkableEvent.WorkStopped && this.OnSocializeEndCB != null)
		{
			this.OnSocializeEndCB();
		}
	}

	// Token: 0x04003546 RID: 13638
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04003547 RID: 13639
	public int choreCount = 2;

	// Token: 0x04003548 RID: 13640
	public int basePriority;

	// Token: 0x04003549 RID: 13641
	public string socialEffect;

	// Token: 0x0400354A RID: 13642
	public float workTime = 15f;

	// Token: 0x0400354B RID: 13643
	public System.Action OnSocializeBeginCB;

	// Token: 0x0400354C RID: 13644
	public System.Action OnSocializeEndCB;

	// Token: 0x0400354D RID: 13645
	private SocialChoreTracker tracker;

	// Token: 0x0400354E RID: 13646
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x02001AD7 RID: 6871
	public class States : GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint>
	{
		// Token: 0x0600A146 RID: 41286 RVA: 0x00382960 File Offset: 0x00380B60
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.TagTransition(GameTags.Operational, this.on, false);
			this.on.TagTransition(GameTags.Operational, this.off, true).Enter("CreateChore", delegate(SocialGatheringPoint.StatesInstance smi)
			{
				smi.master.tracker.Update(true);
			}).Exit("CancelChore", delegate(SocialGatheringPoint.StatesInstance smi)
			{
				smi.master.tracker.Update(false);
			});
		}

		// Token: 0x04007DE7 RID: 32231
		public GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.State off;

		// Token: 0x04007DE8 RID: 32232
		public GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.State on;
	}

	// Token: 0x02001AD8 RID: 6872
	public class StatesInstance : GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.GameInstance
	{
		// Token: 0x0600A148 RID: 41288 RVA: 0x00382A0B File Offset: 0x00380C0B
		public StatesInstance(SocialGatheringPoint smi) : base(smi)
		{
		}
	}
}
