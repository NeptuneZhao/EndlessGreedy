using System;
using STRINGS;

// Token: 0x020000D4 RID: 212
public class FallStates : GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>
{
	// Token: 0x060003E2 RID: 994 RVA: 0x0001F9D8 File Offset: 0x0001DBD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.FALLING.NAME;
		string tooltip = CREATURES.STATUSITEMS.FALLING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.loop.PlayAnim((FallStates.Instance smi) => smi.GetSMI<CreatureFallMonitor.Instance>().anim, KAnim.PlayMode.Loop).ToggleGravity().EventTransition(GameHashes.Landed, this.snaptoground, null).Transition(this.pst, (FallStates.Instance smi) => smi.GetSMI<CreatureFallMonitor.Instance>().CanSwimAtCurrentLocation(), UpdateRate.SIM_33ms);
		this.snaptoground.Enter(delegate(FallStates.Instance smi)
		{
			smi.GetSMI<CreatureFallMonitor.Instance>().SnapToGround();
		}).GoTo(this.pst);
		this.pst.Enter(new StateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State.Callback(FallStates.PlayLandAnim)).BehaviourComplete(GameTags.Creatures.Falling, false);
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0001FAFC File Offset: 0x0001DCFC
	private static void PlayLandAnim(FallStates.Instance smi)
	{
		smi.GetComponent<KBatchedAnimController>().Queue(smi.def.getLandAnim(smi), KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x040002A9 RID: 681
	private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State loop;

	// Token: 0x040002AA RID: 682
	private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State snaptoground;

	// Token: 0x040002AB RID: 683
	private GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.State pst;

	// Token: 0x02001042 RID: 4162
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005C79 RID: 23673
		public Func<FallStates.Instance, string> getLandAnim = (FallStates.Instance smi) => "idle_loop";
	}

	// Token: 0x02001043 RID: 4163
	public new class Instance : GameStateMachine<FallStates, FallStates.Instance, IStateMachineTarget, FallStates.Def>.GameInstance
	{
		// Token: 0x06007B92 RID: 31634 RVA: 0x00303C8E File Offset: 0x00301E8E
		public Instance(Chore<FallStates.Instance> chore, FallStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Falling);
		}
	}
}
