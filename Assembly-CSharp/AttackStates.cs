using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class AttackStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>
{
	// Token: 0x06000343 RID: 835 RVA: 0x0001B5D4 File Offset: 0x000197D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.waitBeforeAttack;
		this.root.Enter("SetTarget", delegate(AttackStates.Instance smi)
		{
			this.target.Set(smi.GetSMI<ThreatMonitor.Instance>().MainThreat, smi, false);
			this.cellOffsets = smi.def.cellOffsets;
		});
		this.waitBeforeAttack.ScheduleGoTo((AttackStates.Instance smi) => UnityEngine.Random.Range(0f, 4f), this.approach);
		GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state = this.approach.InitializeStates(this.masterTarget, this.target, this.attack, null, this.cellOffsets, null);
		string name = CREATURES.STATUSITEMS.ATTACK_APPROACH.NAME;
		string tooltip = CREATURES.STATUSITEMS.ATTACK_APPROACH.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State state2 = this.attack.DefaultState(this.attack.pre);
		string name2 = CREATURES.STATUSITEMS.ATTACK.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.ATTACK.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.attack.pre.PlayAnim((AttackStates.Instance smi) => smi.def.preAnim, KAnim.PlayMode.Once).Exit(delegate(AttackStates.Instance smi)
		{
			smi.GetComponent<Weapon>().AttackTarget(this.target.Get(smi));
		}).OnAnimQueueComplete(this.attack.pst);
		this.attack.pst.PlayAnim((AttackStates.Instance smi) => smi.def.pstAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Attack, false);
	}

	// Token: 0x04000251 RID: 593
	public StateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.TargetParameter target;

	// Token: 0x04000252 RID: 594
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.ApproachSubState<AttackableBase> approach;

	// Token: 0x04000253 RID: 595
	public CellOffset[] cellOffsets;

	// Token: 0x04000254 RID: 596
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State waitBeforeAttack;

	// Token: 0x04000255 RID: 597
	public AttackStates.AttackingStates attack;

	// Token: 0x04000256 RID: 598
	public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State behaviourcomplete;

	// Token: 0x02000FE0 RID: 4064
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007ABF RID: 31423 RVA: 0x0030284C File Offset: 0x00300A4C
		public Def(string pre_anim = "eat_pre", string pst_anim = "eat_pst", CellOffset[] cell_offsets = null)
		{
			this.preAnim = pre_anim;
			this.pstAnim = pst_anim;
			if (cell_offsets != null)
			{
				this.cellOffsets = cell_offsets;
			}
		}

		// Token: 0x04005BAC RID: 23468
		public string preAnim;

		// Token: 0x04005BAD RID: 23469
		public string pstAnim;

		// Token: 0x04005BAE RID: 23470
		public CellOffset[] cellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1)
		};
	}

	// Token: 0x02000FE1 RID: 4065
	public class AttackingStates : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State
	{
		// Token: 0x04005BAF RID: 23471
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pre;

		// Token: 0x04005BB0 RID: 23472
		public GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.State pst;
	}

	// Token: 0x02000FE2 RID: 4066
	public new class Instance : GameStateMachine<AttackStates, AttackStates.Instance, IStateMachineTarget, AttackStates.Def>.GameInstance
	{
		// Token: 0x06007AC1 RID: 31425 RVA: 0x003028D1 File Offset: 0x00300AD1
		public Instance(Chore<AttackStates.Instance> chore, AttackStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Attack);
		}
	}
}
