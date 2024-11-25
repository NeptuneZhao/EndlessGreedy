using System;
using TUNING;

// Token: 0x020009A4 RID: 2468
public class StressBehaviourMonitor : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance>
{
	// Token: 0x060047E3 RID: 18403 RVA: 0x0019BA34 File Offset: 0x00199C34
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.satisfied.EventTransition(GameHashes.Stressed, this.stressed, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed());
		this.stressed.DefaultState(this.stressed.tierOne).ToggleExpression(Db.Get().Expressions.Unhappy, null).ToggleAnims((StressBehaviourMonitor.Instance smi) => smi.tierOneLocoAnim).Transition(this.satisfied, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && !smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed(), UpdateRate.SIM_200ms);
		this.stressed.tierOne.DefaultState(this.stressed.tierOne.actingOut).EventTransition(GameHashes.StressedHadEnough, this.stressed.tierTwo, null);
		this.stressed.tierOne.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierOneStressChore(), this.stressed.tierOne.reprieve);
		this.stressed.tierOne.reprieve.ScheduleGoTo(30f, this.stressed.tierOne.actingOut);
		this.stressed.tierTwo.DefaultState(this.stressed.tierTwo.actingOut).Update(delegate(StressBehaviourMonitor.Instance smi, float dt)
		{
			smi.sm.timeInTierTwoStressResponse.Set(smi.sm.timeInTierTwoStressResponse.Get(smi) + dt, smi, false);
		}, UpdateRate.SIM_200ms, false).Exit("ResetStress", delegate(StressBehaviourMonitor.Instance smi)
		{
			Db.Get().Amounts.Stress.Lookup(smi.gameObject).SetValue(STRESS.ACTING_OUT_RESET);
		});
		this.stressed.tierTwo.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierTwoStressChore(), this.stressed.tierTwo.reprieve);
		this.stressed.tierTwo.reprieve.ToggleChore((StressBehaviourMonitor.Instance smi) => new StressIdleChore(smi.master), null).Enter(delegate(StressBehaviourMonitor.Instance smi)
		{
			if (smi.sm.timeInTierTwoStressResponse.Get(smi) >= 150f)
			{
				smi.sm.timeInTierTwoStressResponse.Set(0f, smi, false);
				smi.GoTo(this.stressed);
			}
		}).ScheduleGoTo((StressBehaviourMonitor.Instance smi) => smi.tierTwoReprieveDuration, this.stressed.tierTwo);
	}

	// Token: 0x04002F0B RID: 12043
	public StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeInTierTwoStressResponse;

	// Token: 0x04002F0C RID: 12044
	public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002F0D RID: 12045
	public StressBehaviourMonitor.StressedState stressed;

	// Token: 0x0200198B RID: 6539
	public class StressedState : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040079C8 RID: 31176
		public StressBehaviourMonitor.TierOneStates tierOne;

		// Token: 0x040079C9 RID: 31177
		public StressBehaviourMonitor.TierTwoStates tierTwo;
	}

	// Token: 0x0200198C RID: 6540
	public class TierOneStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040079CA RID: 31178
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x040079CB RID: 31179
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x0200198D RID: 6541
	public class TierTwoStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040079CC RID: 31180
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x040079CD RID: 31181
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x0200198E RID: 6542
	public new class Instance : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009D0F RID: 40207 RVA: 0x00373E22 File Offset: 0x00372022
		public Instance(IStateMachineTarget master, Func<ChoreProvider, Chore> tier_one_stress_chore_creator, Func<ChoreProvider, Chore> tier_two_stress_chore_creator, string tier_one_loco_anim, float tier_two_reprieve_duration = 3f) : base(master)
		{
			this.tierOneLocoAnim = tier_one_loco_anim;
			this.tierTwoReprieveDuration = tier_two_reprieve_duration;
			this.tierOneStressChoreCreator = tier_one_stress_chore_creator;
			this.tierTwoStressChoreCreator = tier_two_stress_chore_creator;
		}

		// Token: 0x06009D10 RID: 40208 RVA: 0x00373E54 File Offset: 0x00372054
		public Chore CreateTierOneStressChore()
		{
			return this.tierOneStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x06009D11 RID: 40209 RVA: 0x00373E67 File Offset: 0x00372067
		public Chore CreateTierTwoStressChore()
		{
			return this.tierTwoStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x040079CE RID: 31182
		public Func<ChoreProvider, Chore> tierOneStressChoreCreator;

		// Token: 0x040079CF RID: 31183
		public Func<ChoreProvider, Chore> tierTwoStressChoreCreator;

		// Token: 0x040079D0 RID: 31184
		public string tierOneLocoAnim = "";

		// Token: 0x040079D1 RID: 31185
		public float tierTwoReprieveDuration;
	}
}
