using System;
using STRINGS;

// Token: 0x0200043E RID: 1086
public class FixedCaptureChore : Chore<FixedCaptureChore.FixedCaptureChoreStates.Instance>
{
	// Token: 0x06001726 RID: 5926 RVA: 0x0007D058 File Offset: 0x0007B258
	public FixedCaptureChore(KPrefabID capture_point)
	{
		Chore.Precondition isCreatureAvailableForFixedCapture = default(Chore.Precondition);
		isCreatureAvailableForFixedCapture.id = "IsCreatureAvailableForFixedCapture";
		isCreatureAvailableForFixedCapture.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE;
		isCreatureAvailableForFixedCapture.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return (data as FixedCapturePoint.Instance).IsCreatureAvailableForFixedCapture();
		};
		this.IsCreatureAvailableForFixedCapture = isCreatureAvailableForFixedCapture;
		base..ctor(Db.Get().ChoreTypes.Ranch, capture_point, null, false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		this.AddPrecondition(this.IsCreatureAvailableForFixedCapture, capture_point.GetSMI<FixedCapturePoint.Instance>());
		this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanWrangleCreatures.Id);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, capture_point.GetComponent<Building>());
		Operational component = capture_point.GetComponent<Operational>();
		this.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		Deconstructable component2 = capture_point.GetComponent<Deconstructable>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component2);
		BuildingEnabledButton component3 = capture_point.GetComponent<BuildingEnabledButton>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component3);
		base.smi = new FixedCaptureChore.FixedCaptureChoreStates.Instance(capture_point);
		base.SetPrioritizable(capture_point.GetComponent<Prioritizable>());
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x0007D1A8 File Offset: 0x0007B3A8
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rancher.Set(context.consumerState.gameObject, base.smi, false);
		base.smi.sm.creature.Set(base.smi.fixedCapturePoint.targetCapturable.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x04000D06 RID: 3334
	public Chore.Precondition IsCreatureAvailableForFixedCapture;

	// Token: 0x020011C2 RID: 4546
	public class FixedCaptureChoreStates : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance>
	{
		// Token: 0x06008104 RID: 33028 RVA: 0x00314D2C File Offset: 0x00312F2C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.movetopoint;
			base.Target(this.rancher);
			this.root.Exit("ResetCapturePoint", delegate(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
			{
				smi.fixedCapturePoint.ResetCapturePoint();
			});
			this.movetopoint.MoveTo((FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => Grid.PosToCell(smi.transform.GetPosition()), this.waitforcreature_pre, null, false).Target(this.masterTarget).EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed, null);
			this.waitforcreature_pre.EnterTransition(null, (FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => smi.fixedCapturePoint.IsNullOrStopped()).EnterTransition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft)).EnterTransition(this.waitforcreature, (FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => true);
			this.waitforcreature.ToggleAnims("anim_interacts_rancherstation_kanim", 0f).PlayAnim("calling_loop", KAnim.PlayMode.Loop).Transition(this.failed, new StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(FixedCaptureChore.FixedCaptureChoreStates.HasCreatureLeft), UpdateRate.SIM_200ms).Face(this.creature, 0f).Enter("SetRancherIsAvailableForCapturing", delegate(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
			{
				smi.fixedCapturePoint.SetRancherIsAvailableForCapturing();
			}).Exit("ClearRancherIsAvailableForCapturing", delegate(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
			{
				smi.fixedCapturePoint.ClearRancherIsAvailableForCapturing();
			}).Target(this.masterTarget).EventTransition(GameHashes.CreatureArrivedAtCapturePoint, this.capturecreature, null);
			this.capturecreature.EventTransition(GameHashes.CreatureAbandonedCapturePoint, this.failed, null).EnterTransition(this.failed, (FixedCaptureChore.FixedCaptureChoreStates.Instance smi) => smi.fixedCapturePoint.targetCapturable.IsNullOrStopped()).ToggleWork<Capturable>(this.creature, this.success, this.failed, null);
			this.failed.GoTo(null);
			this.success.ReturnSuccess();
		}

		// Token: 0x06008105 RID: 33029 RVA: 0x00314F63 File Offset: 0x00313163
		private static bool HasCreatureLeft(FixedCaptureChore.FixedCaptureChoreStates.Instance smi)
		{
			return smi.fixedCapturePoint.targetCapturable.IsNullOrStopped() || !smi.fixedCapturePoint.targetCapturable.GetComponent<ChoreConsumer>().IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>();
		}

		// Token: 0x04006149 RID: 24905
		public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter rancher;

		// Token: 0x0400614A RID: 24906
		public StateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.TargetParameter creature;

		// Token: 0x0400614B RID: 24907
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State movetopoint;

		// Token: 0x0400614C RID: 24908
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature_pre;

		// Token: 0x0400614D RID: 24909
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State waitforcreature;

		// Token: 0x0400614E RID: 24910
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State capturecreature;

		// Token: 0x0400614F RID: 24911
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State failed;

		// Token: 0x04006150 RID: 24912
		private GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.State success;

		// Token: 0x020023C4 RID: 9156
		public new class Instance : GameStateMachine<FixedCaptureChore.FixedCaptureChoreStates, FixedCaptureChore.FixedCaptureChoreStates.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600B79E RID: 47006 RVA: 0x003CDD6F File Offset: 0x003CBF6F
			public Instance(KPrefabID capture_point) : base(capture_point)
			{
				this.fixedCapturePoint = capture_point.GetSMI<FixedCapturePoint.Instance>();
			}

			// Token: 0x04009FAF RID: 40879
			public FixedCapturePoint.Instance fixedCapturePoint;
		}
	}
}
