using System;
using STRINGS;

// Token: 0x020000CC RID: 204
public class DefendStates : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>
{
	// Token: 0x060003BA RID: 954 RVA: 0x0001EB78 File Offset: 0x0001CD78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.protectEntity.moveToThreat;
		GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State state = this.root.Enter("SetTarget", delegate(DefendStates.Instance smi)
		{
			this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi, false);
		});
		string name = CREATURES.STATUSITEMS.ATTACKINGENTITY.NAME;
		string tooltip = CREATURES.STATUSITEMS.ATTACKINGENTITY.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.protectEntity.moveToThreat.InitializeStates(this.masterTarget, this.target, this.protectEntity.attackThreat, null, CrabTuning.DEFEND_OFFSETS, null);
		this.protectEntity.attackThreat.Enter(delegate(DefendStates.Instance smi)
		{
			smi.Play("slap_pre", KAnim.PlayMode.Once);
			smi.Queue("slap", KAnim.PlayMode.Once);
			smi.Queue("slap_pst", KAnim.PlayMode.Once);
			smi.Schedule(0.5f, delegate
			{
				smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi));
			}, null);
		}).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Defend, false);
	}

	// Token: 0x04000290 RID: 656
	public StateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.TargetParameter target;

	// Token: 0x04000291 RID: 657
	public DefendStates.ProtectStates protectEntity;

	// Token: 0x04000292 RID: 658
	public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State behaviourcomplete;

	// Token: 0x02001027 RID: 4135
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001028 RID: 4136
	public new class Instance : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.GameInstance
	{
		// Token: 0x06007B5E RID: 31582 RVA: 0x0030376F File Offset: 0x0030196F
		public Instance(Chore<DefendStates.Instance> chore, DefendStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Defend);
		}
	}

	// Token: 0x02001029 RID: 4137
	public class ProtectStates : GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State
	{
		// Token: 0x04005C57 RID: 23639
		public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.ApproachSubState<AttackableBase> moveToThreat;

		// Token: 0x04005C58 RID: 23640
		public GameStateMachine<DefendStates, DefendStates.Instance, IStateMachineTarget, DefendStates.Def>.State attackThreat;
	}
}
