using System;
using STRINGS;

// Token: 0x020000DB RID: 219
public class HiveGrowingStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>
{
	// Token: 0x060003FC RID: 1020 RVA: 0x000205E8 File Offset: 0x0001E7E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.growing.DefaultState(this.growing.loop);
		this.growing.loop.PlayAnim((HiveGrowingStates.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(delegate(HiveGrowingStates.Instance smi)
		{
			smi.RefreshPositionPercent();
		}).Update(delegate(HiveGrowingStates.Instance smi, float dt)
		{
			smi.RefreshPositionPercent();
			if (smi.hive.IsFullyGrown())
			{
				smi.GoTo(this.growing.pst);
			}
		}, UpdateRate.SIM_4000ms, false);
		this.growing.pst.PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.GrowUpBehaviour, false);
	}

	// Token: 0x040002BC RID: 700
	public HiveGrowingStates.GrowUpStates growing;

	// Token: 0x040002BD RID: 701
	public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State behaviourcomplete;

	// Token: 0x02001057 RID: 4183
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001058 RID: 4184
	public new class Instance : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.GameInstance
	{
		// Token: 0x06007BC1 RID: 31681 RVA: 0x00304221 File Offset: 0x00302421
		public Instance(Chore<HiveGrowingStates.Instance> chore, HiveGrowingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x00304245 File Offset: 0x00302445
		public void RefreshPositionPercent()
		{
			this.animController.SetPositionPercent(this.hive.sm.hiveGrowth.Get(this.hive));
		}

		// Token: 0x04005C9B RID: 23707
		[MySmiReq]
		public BeeHive.StatesInstance hive;

		// Token: 0x04005C9C RID: 23708
		[MyCmpReq]
		private KAnimControllerBase animController;
	}

	// Token: 0x02001059 RID: 4185
	public class GrowUpStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State
	{
		// Token: 0x04005C9D RID: 23709
		public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State loop;

		// Token: 0x04005C9E RID: 23710
		public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State pst;
	}
}
