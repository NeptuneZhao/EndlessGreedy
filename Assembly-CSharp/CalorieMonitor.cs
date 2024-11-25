using System;
using Klei.AI;
using TUNING;

// Token: 0x02000970 RID: 2416
public class CalorieMonitor : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance>
{
	// Token: 0x060046D6 RID: 18134 RVA: 0x00194F3C File Offset: 0x0019313C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Transition(this.hungry, (CalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_200ms);
		this.hungry.DefaultState(this.hungry.normal).Transition(this.satisfied, (CalorieMonitor.Instance smi) => smi.IsSatisfied(), UpdateRate.SIM_200ms).EventTransition(GameHashes.BeginChore, this.eating, (CalorieMonitor.Instance smi) => smi.IsEating());
		this.hungry.working.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.normal, (CalorieMonitor.Instance smi) => smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null);
		this.hungry.normal.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.working, (CalorieMonitor.Instance smi) => !smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.hungry.starving.Transition(this.hungry.normal, (CalorieMonitor.Instance smi) => !smi.IsStarving(), UpdateRate.SIM_200ms).Transition(this.depleted, (CalorieMonitor.Instance smi) => smi.IsDepleted(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Starving, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.eating.EventTransition(GameHashes.EndChore, this.satisfied, (CalorieMonitor.Instance smi) => !smi.IsEating());
		this.depleted.ToggleTag(GameTags.CaloriesDepleted).Enter(delegate(CalorieMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x04002E20 RID: 11808
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002E21 RID: 11809
	public CalorieMonitor.HungryState hungry;

	// Token: 0x04002E22 RID: 11810
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State eating;

	// Token: 0x04002E23 RID: 11811
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x04002E24 RID: 11812
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State depleted;

	// Token: 0x02001902 RID: 6402
	public class HungryState : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400781E RID: 30750
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x0400781F RID: 30751
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x04007820 RID: 30752
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State starving;
	}

	// Token: 0x02001903 RID: 6403
	public new class Instance : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009AD6 RID: 39638 RVA: 0x0036E499 File Offset: 0x0036C699
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
		}

		// Token: 0x06009AD7 RID: 39639 RVA: 0x0036E4C2 File Offset: 0x0036C6C2
		private float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06009AD8 RID: 39640 RVA: 0x0036E4DB File Offset: 0x0036C6DB
		public bool IsEatTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
		}

		// Token: 0x06009AD9 RID: 39641 RVA: 0x0036E4FC File Offset: 0x0036C6FC
		public bool IsHungry()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.HUNGRY_THRESHOLD;
		}

		// Token: 0x06009ADA RID: 39642 RVA: 0x0036E515 File Offset: 0x0036C715
		public bool IsStarving()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.STARVING_THRESHOLD;
		}

		// Token: 0x06009ADB RID: 39643 RVA: 0x0036E52E File Offset: 0x0036C72E
		public bool IsSatisfied()
		{
			return this.GetCalories0to1() > DUPLICANTSTATS.STANDARD.BaseStats.SATISFIED_THRESHOLD;
		}

		// Token: 0x06009ADC RID: 39644 RVA: 0x0036E548 File Offset: 0x0036C748
		public bool IsEating()
		{
			ChoreDriver component = base.master.GetComponent<ChoreDriver>();
			return component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x06009ADD RID: 39645 RVA: 0x0036E58C File Offset: 0x0036C78C
		public bool IsDepleted()
		{
			return this.calories.value <= 0f;
		}

		// Token: 0x06009ADE RID: 39646 RVA: 0x0036E5A3 File Offset: 0x0036C7A3
		public bool ShouldExitInfirmary()
		{
			return !this.IsStarving();
		}

		// Token: 0x06009ADF RID: 39647 RVA: 0x0036E5AE File Offset: 0x0036C7AE
		public void Kill()
		{
			if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
			{
				base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
			}
		}

		// Token: 0x04007821 RID: 30753
		public AmountInstance calories;
	}
}
