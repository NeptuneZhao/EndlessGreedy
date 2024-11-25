using System;

// Token: 0x02000301 RID: 769
public class MorbRoverMakerDisplay : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>
{
	// Token: 0x0600102E RID: 4142 RVA: 0x0005BA48 File Offset: 0x00059C48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.off.idle;
		this.root.Target(this.monitor);
		this.off.DefaultState(this.off.idle);
		this.off.entering.PlayAnim("display_off").OnAnimQueueComplete(this.off.idle);
		this.off.idle.Target(this.masterTarget).EventTransition(GameHashes.TagsChanged, this.off.exiting, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.ShouldBeOn)).Target(this.monitor).PlayAnim("display_off_idle", KAnim.PlayMode.Loop);
		this.off.exiting.PlayAnim("display_on").OnAnimQueueComplete(this.on);
		this.on.Target(this.masterTarget).TagTransition(GameTags.Operational, this.off.entering, true).Target(this.monitor).DefaultState(this.on.idle);
		this.on.idle.Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_idle", KAnim.PlayMode.Loop);
		this.on.noGerm.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_no_germ", KAnim.PlayMode.Loop);
		this.on.germ.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_germ", KAnim.PlayMode.Loop);
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0005BC5D File Offset: 0x00059E5D
	public static bool NoGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && !smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0005BC72 File Offset: 0x00059E72
	public static bool HasGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0005BC84 File Offset: 0x00059E84
	public static bool ShouldBeOn(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.ShouldBeOn();
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0005BC8C File Offset: 0x00059E8C
	public static bool GermsNoLongerNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return !smi.GermsAreNeeded;
	}

	// Token: 0x040009D1 RID: 2513
	public const string METER_TARGET_NAME = "meter_display_target";

	// Token: 0x040009D2 RID: 2514
	public const string OFF_IDLE_ANIM_NAME = "display_off_idle";

	// Token: 0x040009D3 RID: 2515
	public const string OFF_ENTERING_ANIM_NAME = "display_off";

	// Token: 0x040009D4 RID: 2516
	public const string OFF_EXITING_ANIM_NAME = "display_on";

	// Token: 0x040009D5 RID: 2517
	public const string GERM_ICON_ANIM_NAME = "display_germ";

	// Token: 0x040009D6 RID: 2518
	public const string NO_GERM_ANIM_NAME = "display_no_germ";

	// Token: 0x040009D7 RID: 2519
	public const string ON_IDLE_ANIM_NAME = "display_idle";

	// Token: 0x040009D8 RID: 2520
	public StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.TargetParameter monitor;

	// Token: 0x040009D9 RID: 2521
	public MorbRoverMakerDisplay.OffStates off;

	// Token: 0x040009DA RID: 2522
	public MorbRoverMakerDisplay.OnStates on;

	// Token: 0x02001129 RID: 4393
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005F67 RID: 24423
		public float Timeout = 1f;
	}

	// Token: 0x0200112A RID: 4394
	public class OffStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x04005F68 RID: 24424
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State entering;

		// Token: 0x04005F69 RID: 24425
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x04005F6A RID: 24426
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State exiting;
	}

	// Token: 0x0200112B RID: 4395
	public class OnStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x04005F6B RID: 24427
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x04005F6C RID: 24428
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State shake;

		// Token: 0x04005F6D RID: 24429
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State noGerm;

		// Token: 0x04005F6E RID: 24430
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State germ;

		// Token: 0x04005F6F RID: 24431
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State checkmark;
	}

	// Token: 0x0200112C RID: 4396
	public new class Instance : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.GameInstance
	{
		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06007EBD RID: 32445 RVA: 0x0030B005 File Offset: 0x00309205
		public bool HasRecentlyConsumedGerms
		{
			get
			{
				return GameClock.Instance.GetTime() - this.lastTimeGermsConsumed < base.def.Timeout;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06007EBE RID: 32446 RVA: 0x0030B025 File Offset: 0x00309225
		public bool GermsAreNeeded
		{
			get
			{
				return this.morbRoverMaker.MorbDevelopment_Progress < 1f;
			}
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x0030B03C File Offset: 0x0030923C
		public Instance(IStateMachineTarget master, MorbRoverMakerDisplay.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.meter = new MeterController(component, "meter_display_target", "display_off_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			base.sm.monitor.Set(this.meter.gameObject, base.smi, false);
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x0030B0A4 File Offset: 0x003092A4
		public override void StartSM()
		{
			this.morbRoverMaker = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			MorbRoverMaker.Instance instance = this.morbRoverMaker;
			instance.GermsAdded = (Action<long>)Delegate.Combine(instance.GermsAdded, new Action<long>(this.OnGermsAdded));
			MorbRoverMaker.Instance instance2 = this.morbRoverMaker;
			instance2.OnUncovered = (System.Action)Delegate.Combine(instance2.OnUncovered, new System.Action(this.OnUncovered));
			base.StartSM();
		}

		// Token: 0x06007EC1 RID: 32449 RVA: 0x0030B116 File Offset: 0x00309316
		private void OnGermsAdded(long amount)
		{
			this.lastTimeGermsConsumed = GameClock.Instance.GetTime();
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x0030B128 File Offset: 0x00309328
		public bool ShouldBeOn()
		{
			return this.morbRoverMaker.HasBeenRevealed && this.operational.IsOperational;
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x0030B144 File Offset: 0x00309344
		private void OnUncovered()
		{
			if (base.IsInsideState(base.sm.off.idle))
			{
				this.GoTo(base.sm.off.exiting);
			}
		}

		// Token: 0x04005F70 RID: 24432
		private float lastTimeGermsConsumed = -1f;

		// Token: 0x04005F71 RID: 24433
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04005F72 RID: 24434
		private MorbRoverMaker.Instance morbRoverMaker;

		// Token: 0x04005F73 RID: 24435
		private MeterController meter;
	}
}
