using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020009B7 RID: 2487
[SkipSaveFileSerialization]
public class Narcolepsy : StateMachineComponent<Narcolepsy.StatesInstance>
{
	// Token: 0x06004859 RID: 18521 RVA: 0x0019EC76 File Offset: 0x0019CE76
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x0019EC83 File Offset: 0x0019CE83
	public bool IsNarcolepsing()
	{
		return base.smi.IsNarcolepsing();
	}

	// Token: 0x04002F75 RID: 12149
	public static readonly Chore.Precondition IsNarcolepsingPrecondition = new Chore.Precondition
	{
		id = "IsNarcolepsingPrecondition",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NARCOLEPSING,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			return component != null && component.IsNarcolepsing();
		}
	};

	// Token: 0x020019BD RID: 6589
	public class StatesInstance : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.GameInstance
	{
		// Token: 0x06009DEB RID: 40427 RVA: 0x00376630 File Offset: 0x00374830
		public StatesInstance(Narcolepsy master) : base(master)
		{
		}

		// Token: 0x06009DEC RID: 40428 RVA: 0x0037663C File Offset: 0x0037483C
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x06009DED RID: 40429 RVA: 0x00376660 File Offset: 0x00374860
		public bool IsNarcolepsing()
		{
			return this.GetCurrentState() == base.sm.sleepy;
		}

		// Token: 0x06009DEE RID: 40430 RVA: 0x00376675 File Offset: 0x00374875
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "NarcolepticSleep";
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}
	}

	// Token: 0x020019BE RID: 6590
	public class States : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy>
	{
		// Token: 0x06009DEF RID: 40431 RVA: 0x003766A0 File Offset: 0x003748A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Enter("ScheduleNextSleep", delegate(Narcolepsy.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_INTERVAL_MIN, TRAITS.NARCOLEPSY_INTERVAL_MAX), this.sleepy);
			});
			this.sleepy.Enter("Is Already Sleeping Check", delegate(Narcolepsy.StatesInstance smi)
			{
				if (smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping())
				{
					smi.GoTo(this.idle);
					return;
				}
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_SLEEPDURATION_MIN, TRAITS.NARCOLEPSY_SLEEPDURATION_MAX), this.idle);
			}).ToggleUrge(Db.Get().Urges.Narcolepsy).ToggleChore(new Func<Narcolepsy.StatesInstance, Chore>(this.CreateNarcolepsyChore), this.idle);
		}

		// Token: 0x06009DF0 RID: 40432 RVA: 0x00376730 File Offset: 0x00374930
		private Chore CreateNarcolepsyChore(Narcolepsy.StatesInstance smi)
		{
			GameObject bed = smi.CreateFloorLocator();
			SleepChore sleepChore = new SleepChore(Db.Get().ChoreTypes.Narcolepsy, smi.master, bed, true, false);
			sleepChore.AddPrecondition(Narcolepsy.IsNarcolepsingPrecondition, null);
			return sleepChore;
		}

		// Token: 0x06009DF1 RID: 40433 RVA: 0x0037676D File Offset: 0x0037496D
		private float GetNewInterval(float min, float max)
		{
			Mathf.Min(Mathf.Max(Util.GaussianRandom(max - min, 1f), min), max);
			return UnityEngine.Random.Range(min, max);
		}

		// Token: 0x04007A99 RID: 31385
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State idle;

		// Token: 0x04007A9A RID: 31386
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State sleepy;
	}
}
