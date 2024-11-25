using System;
using Klei.AI;

// Token: 0x020009A2 RID: 2466
public class StaminaMonitor : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance>
{
	// Token: 0x060047D6 RID: 18390 RVA: 0x0019B538 File Offset: 0x00199738
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleStateMachine((StaminaMonitor.Instance smi) => new UrgeMonitor.Instance(smi.master, Db.Get().Urges.Sleep, Db.Get().Amounts.Stamina, Db.Get().ScheduleBlockTypes.Sleep, 100f, 0f, false)).ToggleStateMachine((StaminaMonitor.Instance smi) => new SleepChoreMonitor.Instance(smi.master));
		this.satisfied.Transition(this.sleepy, (StaminaMonitor.Instance smi) => smi.NeedsToSleep() || smi.WantsToSleep(), UpdateRate.SIM_200ms);
		this.sleepy.Update("Check Sleep State", delegate(StaminaMonitor.Instance smi, float dt)
		{
			smi.TryExitSleepState();
		}, UpdateRate.SIM_1000ms, false).DefaultState(this.sleepy.needssleep);
		this.sleepy.needssleep.Transition(this.sleepy.sleeping, (StaminaMonitor.Instance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.Tired, null).ToggleThought(Db.Get().Thoughts.Sleepy, null);
		this.sleepy.sleeping.Enter(delegate(StaminaMonitor.Instance smi)
		{
			smi.CheckDebugFastWorkMode();
		}).Transition(this.satisfied, (StaminaMonitor.Instance smi) => !smi.IsSleeping(), UpdateRate.SIM_200ms);
	}

	// Token: 0x04002F01 RID: 12033
	public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04002F02 RID: 12034
	public StaminaMonitor.SleepyState sleepy;

	// Token: 0x04002F03 RID: 12035
	private const float OUTSIDE_SCHEDULE_STAMINA_THRESHOLD = 0f;

	// Token: 0x02001987 RID: 6535
	public class SleepyState : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040079B9 RID: 31161
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State needssleep;

		// Token: 0x040079BA RID: 31162
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State sleeping;
	}

	// Token: 0x02001988 RID: 6536
	public new class Instance : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06009CF7 RID: 40183 RVA: 0x00373B50 File Offset: 0x00371D50
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stamina = Db.Get().Amounts.Stamina.Lookup(base.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x06009CF8 RID: 40184 RVA: 0x00373B9C File Offset: 0x00371D9C
		public bool NeedsToSleep()
		{
			return this.stamina.value <= 0f;
		}

		// Token: 0x06009CF9 RID: 40185 RVA: 0x00373BB3 File Offset: 0x00371DB3
		public bool WantsToSleep()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x06009CFA RID: 40186 RVA: 0x00373BE3 File Offset: 0x00371DE3
		public void TryExitSleepState()
		{
			if (!this.NeedsToSleep() && !this.WantsToSleep())
			{
				base.smi.GoTo(base.smi.sm.satisfied);
			}
		}

		// Token: 0x06009CFB RID: 40187 RVA: 0x00373C10 File Offset: 0x00371E10
		public bool IsSleeping()
		{
			bool result = false;
			if (this.WantsToSleep() && this.choreDriver.GetComponent<WorkerBase>().GetWorkable() != null)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06009CFC RID: 40188 RVA: 0x00373C42 File Offset: 0x00371E42
		public void CheckDebugFastWorkMode()
		{
			if (Game.Instance.FastWorkersModeActive)
			{
				this.stamina.value = this.stamina.GetMax();
			}
		}

		// Token: 0x06009CFD RID: 40189 RVA: 0x00373C68 File Offset: 0x00371E68
		public bool ShouldExitSleep()
		{
			if (this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return false;
			}
			Narcolepsy component = base.GetComponent<Narcolepsy>();
			return (!(component != null) || !component.IsNarcolepsing()) && this.stamina.value >= this.stamina.GetMax();
		}

		// Token: 0x040079BB RID: 31163
		private ChoreDriver choreDriver;

		// Token: 0x040079BC RID: 31164
		private Schedulable schedulable;

		// Token: 0x040079BD RID: 31165
		public AmountInstance stamina;
	}
}
