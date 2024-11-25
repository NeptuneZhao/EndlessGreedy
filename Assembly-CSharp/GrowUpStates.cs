using System;
using STRINGS;

// Token: 0x020000D8 RID: 216
public class GrowUpStates : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>
{
	// Token: 0x060003F4 RID: 1012 RVA: 0x00020334 File Offset: 0x0001E534
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grow_up_pre;
		GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.grow_up_pre.Enter(delegate(GrowUpStates.Instance smi)
		{
			smi.PlayPreGrowAnimation();
		}).OnAnimQueueComplete(this.spawn_adult).ScheduleGoTo(4f, this.spawn_adult);
		this.spawn_adult.Enter(new StateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State.Callback(GrowUpStates.SpawnAdult));
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x000203F0 File Offset: 0x0001E5F0
	private static void SpawnAdult(GrowUpStates.Instance smi)
	{
		smi.GetSMI<BabyMonitor.Instance>().SpawnAdult();
	}

	// Token: 0x040002B7 RID: 695
	public const float GROW_PRE_TIMEOUT = 4f;

	// Token: 0x040002B8 RID: 696
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State grow_up_pre;

	// Token: 0x040002B9 RID: 697
	public GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.State spawn_adult;

	// Token: 0x0200104E RID: 4174
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200104F RID: 4175
	public new class Instance : GameStateMachine<GrowUpStates, GrowUpStates.Instance, IStateMachineTarget, GrowUpStates.Def>.GameInstance
	{
		// Token: 0x06007BAD RID: 31661 RVA: 0x00303F3D File Offset: 0x0030213D
		public Instance(Chore<GrowUpStates.Instance> chore, GrowUpStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x06007BAE RID: 31662 RVA: 0x00303F64 File Offset: 0x00302164
		public void PlayPreGrowAnimation()
		{
			if (base.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("growup_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
