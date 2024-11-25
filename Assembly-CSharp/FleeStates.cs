﻿using System;
using STRINGS;

// Token: 0x020000D6 RID: 214
public class FleeStates : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>
{
	// Token: 0x060003E9 RID: 1001 RVA: 0x0001FE80 File Offset: 0x0001E080
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.plan;
		GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State state = this.root.Enter("SetFleeTarget", delegate(FleeStates.Instance smi)
		{
			this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi.GetSMI<ThreatMonitor.Instance>().MainThreat), smi, false);
		});
		string name = CREATURES.STATUSITEMS.FLEEING.NAME;
		string tooltip = CREATURES.STATUSITEMS.FLEEING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.plan.Enter(delegate(FleeStates.Instance smi)
		{
			ThreatMonitor.Instance smi2 = smi.master.gameObject.GetSMI<ThreatMonitor.Instance>();
			this.fleeToTarget.Set(CreatureHelpers.GetFleeTargetLocatorObject(smi.master.gameObject, smi2.MainThreat), smi, false);
			if (this.fleeToTarget.Get(smi) != null)
			{
				smi.GoTo(this.approach);
				return;
			}
			smi.GoTo(this.cower);
		});
		this.approach.InitializeStates(this.mover, this.fleeToTarget, this.cower, this.cower, null, NavigationTactics.ReduceTravelDistance).Enter(delegate(FleeStates.Instance smi)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, CREATURES.STATUSITEMS.FLEEING.NAME.text, smi.master.transform, 1.5f, false);
		});
		this.cower.Enter(delegate(FleeStates.Instance smi)
		{
			string s = "DEFAULT COWER ANIMATION";
			if (smi.Get<KBatchedAnimController>().HasAnimation("cower"))
			{
				s = "cower";
			}
			else if (smi.Get<KBatchedAnimController>().HasAnimation("idle"))
			{
				s = "idle";
			}
			else if (smi.Get<KBatchedAnimController>().HasAnimation("idle_loop"))
			{
				s = "idle_loop";
			}
			smi.Get<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		}).ScheduleGoTo(2f, this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Flee, false);
	}

	// Token: 0x040002AE RID: 686
	private StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter mover;

	// Token: 0x040002AF RID: 687
	public StateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.TargetParameter fleeToTarget;

	// Token: 0x040002B0 RID: 688
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State plan;

	// Token: 0x040002B1 RID: 689
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.ApproachSubState<IApproachable> approach;

	// Token: 0x040002B2 RID: 690
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State cower;

	// Token: 0x040002B3 RID: 691
	public GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.State behaviourcomplete;

	// Token: 0x02001049 RID: 4169
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200104A RID: 4170
	public new class Instance : GameStateMachine<FleeStates, FleeStates.Instance, IStateMachineTarget, FleeStates.Def>.GameInstance
	{
		// Token: 0x06007BA5 RID: 31653 RVA: 0x00303DEE File Offset: 0x00301FEE
		public Instance(Chore<FleeStates.Instance> chore, FleeStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Flee);
			base.sm.mover.Set(base.GetComponent<Navigator>(), base.smi);
		}
	}
}
