using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class SleepChore : Chore<SleepChore.StatesInstance>
{
	// Token: 0x0600176F RID: 5999 RVA: 0x0007F168 File Offset: 0x0007D368
	public static void DisplayCustomStatusItemsWhenAsleep(SleepChore.StatesInstance smi)
	{
		if (smi.optional_StatusItemsDisplayedWhileAsleep == null)
		{
			return;
		}
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		for (int i = 0; i < smi.optional_StatusItemsDisplayedWhileAsleep.Length; i++)
		{
			StatusItem status_item = smi.optional_StatusItemsDisplayedWhileAsleep[i];
			component.AddStatusItem(status_item, null);
		}
	}

	// Token: 0x06001770 RID: 6000 RVA: 0x0007F1B0 File Offset: 0x0007D3B0
	public static void RemoveCustomStatusItemsWhenAsleep(SleepChore.StatesInstance smi)
	{
		if (smi.optional_StatusItemsDisplayedWhileAsleep == null)
		{
			return;
		}
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		for (int i = 0; i < smi.optional_StatusItemsDisplayedWhileAsleep.Length; i++)
		{
			StatusItem status_item = smi.optional_StatusItemsDisplayedWhileAsleep[i];
			component.RemoveStatusItem(status_item, false);
		}
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x0007F1F7 File Offset: 0x0007D3F7
	public SleepChore(ChoreType choreType, IStateMachineTarget target, GameObject bed, bool bedIsLocator, bool isInterruptable) : this(choreType, target, bed, bedIsLocator, isInterruptable, null)
	{
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x0007F208 File Offset: 0x0007D408
	public SleepChore(ChoreType choreType, IStateMachineTarget target, GameObject bed, bool bedIsLocator, bool isInterruptable, StatusItem[] optional_StatusItemsDisplayedWhileAsleep) : base(choreType, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new SleepChore.StatesInstance(this, target.gameObject, bed, bedIsLocator, isInterruptable);
		base.smi.optional_StatusItemsDisplayedWhileAsleep = optional_StatusItemsDisplayedWhileAsleep;
		if (isInterruptable)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		}
		this.AddPrecondition(SleepChore.IsOkayTimeToSleep, null);
		Operational component = bed.GetComponent<Operational>();
		if (component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		}
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x0007F298 File Offset: 0x0007D498
	public static Sleepable GetSafeFloorLocator(GameObject sleeper)
	{
		int num = sleeper.GetComponent<Sensors>().GetSensor<SafeCellSensor>().GetSleepCellQuery();
		if (num == Grid.InvalidCell)
		{
			num = Grid.PosToCell(sleeper.transform.GetPosition());
		}
		return ChoreHelpers.CreateSleepLocator(Grid.CellToPosCBC(num, Grid.SceneLayer.Move)).GetComponent<Sleepable>();
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x0007F2E1 File Offset: 0x0007D4E1
	public static bool IsDarkAtCell(int cell)
	{
		return Grid.LightIntensity[cell] < DUPLICANTSTATS.STANDARD.Light.LOW_LIGHT;
	}

	// Token: 0x04000D1A RID: 3354
	public static readonly Chore.Precondition IsOkayTimeToSleep = new Chore.Precondition
	{
		id = "IsOkayTimeToSleep",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OKAY_TIME_TO_SLEEP,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			bool flag = component != null && component.IsNarcolepsing();
			StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
			bool flag2 = smi != null && smi.NeedsToSleep();
			bool flag3 = ChorePreconditions.instance.IsScheduledTime.fn(ref context, Db.Get().ScheduleBlockTypes.Sleep);
			return flag || flag3 || flag2;
		}
	};

	// Token: 0x020011FF RID: 4607
	public class StatesInstance : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.GameInstance
	{
		// Token: 0x060081D1 RID: 33233 RVA: 0x003196D8 File Offset: 0x003178D8
		public StatesInstance(SleepChore master, GameObject sleeper, GameObject bed, bool bedIsLocator, bool isInterruptable) : base(master)
		{
			base.sm.sleeper.Set(sleeper, base.smi, false);
			base.sm.isInterruptable.Set(isInterruptable, base.smi, false);
			Traits component = sleeper.GetComponent<Traits>();
			if (component != null)
			{
				base.sm.needsNightLight.Set(component.HasTrait("NightLight"), base.smi, false);
			}
			if (bedIsLocator)
			{
				this.AddLocator(bed);
				return;
			}
			base.sm.bed.Set(bed, base.smi, false);
		}

		// Token: 0x060081D2 RID: 33234 RVA: 0x0031978C File Offset: 0x0031798C
		public void CheckLightLevel()
		{
			GameObject go = base.sm.sleeper.Get(base.smi);
			int cell = Grid.PosToCell(go);
			if (Grid.IsValidCell(cell))
			{
				bool flag = SleepChore.IsDarkAtCell(cell);
				if (base.sm.needsNightLight.Get(base.smi))
				{
					if (flag)
					{
						go.Trigger(-1307593733, null);
						return;
					}
				}
				else if (!flag && !this.IsLoudSleeper() && !this.IsGlowStick())
				{
					go.Trigger(-1063113160, null);
				}
			}
		}

		// Token: 0x060081D3 RID: 33235 RVA: 0x00319810 File Offset: 0x00317A10
		public void CheckTemperature()
		{
			GameObject go = base.sm.sleeper.Get(base.smi);
			if (go.GetSMI<ExternalTemperatureMonitor.Instance>().IsTooCold())
			{
				go.Trigger(157165762, null);
			}
		}

		// Token: 0x060081D4 RID: 33236 RVA: 0x0031984D File Offset: 0x00317A4D
		public bool IsLoudSleeper()
		{
			return base.sm.sleeper.Get(base.smi).GetComponent<Snorer>() != null;
		}

		// Token: 0x060081D5 RID: 33237 RVA: 0x00319875 File Offset: 0x00317A75
		public bool IsGlowStick()
		{
			return base.sm.sleeper.Get(base.smi).GetComponent<GlowStick>() != null;
		}

		// Token: 0x060081D6 RID: 33238 RVA: 0x0031989D File Offset: 0x00317A9D
		public void EvaluateSleepQuality()
		{
		}

		// Token: 0x060081D7 RID: 33239 RVA: 0x003198A0 File Offset: 0x00317AA0
		public void AddLocator(GameObject sleepable)
		{
			this.locator = sleepable;
			int i = Grid.PosToCell(this.locator);
			Grid.Reserved[i] = true;
			base.sm.bed.Set(this.locator, this, false);
		}

		// Token: 0x060081D8 RID: 33240 RVA: 0x003198E8 File Offset: 0x00317AE8
		public void DestroyLocator()
		{
			if (this.locator != null)
			{
				Grid.Reserved[Grid.PosToCell(this.locator)] = false;
				ChoreHelpers.DestroyLocator(this.locator);
				base.sm.bed.Set(null, this);
				this.locator = null;
			}
		}

		// Token: 0x060081D9 RID: 33241 RVA: 0x00319940 File Offset: 0x00317B40
		public void SetAnim()
		{
			Sleepable sleepable = base.sm.bed.Get<Sleepable>(base.smi);
			if (sleepable.GetComponent<Building>() == null)
			{
				NavType currentNavType = base.sm.sleeper.Get<Navigator>(base.smi).CurrentNavType;
				string s;
				if (currentNavType != NavType.Ladder)
				{
					if (currentNavType != NavType.Pole)
					{
						s = "anim_sleep_floor_kanim";
					}
					else
					{
						s = "anim_sleep_pole_kanim";
					}
				}
				else
				{
					s = "anim_sleep_ladder_kanim";
				}
				sleepable.overrideAnims = new KAnimFile[]
				{
					Assets.GetAnim(s)
				};
			}
		}

		// Token: 0x04006201 RID: 25089
		public bool hadPeacefulSleep;

		// Token: 0x04006202 RID: 25090
		public bool hadNormalSleep;

		// Token: 0x04006203 RID: 25091
		public bool hadBadSleep;

		// Token: 0x04006204 RID: 25092
		public bool hadTerribleSleep;

		// Token: 0x04006205 RID: 25093
		public int lastEvaluatedDay = -1;

		// Token: 0x04006206 RID: 25094
		public float wakeUpBuffer = 2f;

		// Token: 0x04006207 RID: 25095
		public string stateChangeNoiseSource;

		// Token: 0x04006208 RID: 25096
		public StatusItem[] optional_StatusItemsDisplayedWhileAsleep;

		// Token: 0x04006209 RID: 25097
		private GameObject locator;
	}

	// Token: 0x02001200 RID: 4608
	public class States : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore>
	{
		// Token: 0x060081DA RID: 33242 RVA: 0x003199C8 File Offset: 0x00317BC8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.sleeper);
			this.root.Exit("DestroyLocator", delegate(SleepChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.approach.InitializeStates(this.sleeper, this.bed, this.sleep, null, null, null);
			this.sleep.Enter("SetAnims", delegate(SleepChore.StatesInstance smi)
			{
				smi.SetAnim();
			}).DefaultState(this.sleep.normal).ToggleTag(GameTags.Asleep).DoSleep(this.sleeper, this.bed, this.success, null).Toggle("Custom Status Items", new StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback(SleepChore.DisplayCustomStatusItemsWhenAsleep), new StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback(SleepChore.RemoveCustomStatusItemsWhenAsleep)).TriggerOnExit(GameHashes.SleepFinished, null).EventHandler(GameHashes.SleepDisturbedByLight, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByLight.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByNoise, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByNoise.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByFearOfDark, delegate(SleepChore.StatesInstance smi)
			{
				this.isScaredOfDark.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByMovement, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByMovement.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByCold, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByCold.Set(true, smi, false);
			});
			this.sleep.uninterruptable.DoNothing();
			this.sleep.normal.ParamTransition<bool>(this.isInterruptable, this.sleep.uninterruptable, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsFalse).ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Sleeping, null).QueueAnim("working_loop", true, null).ParamTransition<bool>(this.isDisturbedByNoise, this.sleep.interrupt_noise, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByLight, this.sleep.interrupt_light, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isScaredOfDark, this.sleep.interrupt_scared, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByMovement, this.sleep.interrupt_movement, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByCold, this.sleep.interrupt_cold, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).Update(delegate(SleepChore.StatesInstance smi, float dt)
			{
				smi.CheckLightLevel();
				smi.CheckTemperature();
			}, UpdateRate.SIM_200ms, false);
			this.sleep.interrupt_scared.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByFearOfDark, null).QueueAnim("interrupt_afraid", false, null).OnAnimQueueComplete(this.sleep.interrupt_scared_transition);
			this.sleep.interrupt_scared_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepAfraidOfDark"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isScaredOfDark.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_movement.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByMovement, null).PlayAnim("interrupt_light").OnAnimQueueComplete(this.sleep.interrupt_movement_transition).Enter(delegate(SleepChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.bed.Get(smi);
				if (gameObject != null)
				{
					gameObject.Trigger(-717201811, null);
				}
			});
			this.sleep.interrupt_movement_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepMovement"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByMovement.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_cold.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByCold, null).PlayAnim("interrupt_cold").ToggleThought(Db.Get().Thoughts.Cold, null).OnAnimQueueComplete(this.sleep.interrupt_cold_transition).Enter(delegate(SleepChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.bed.Get(smi);
				if (gameObject != null)
				{
					gameObject.Trigger(157165762, null);
				}
			});
			this.sleep.interrupt_cold_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepCold"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByCold.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_noise.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByNoise, null).QueueAnim("interrupt_light", false, null).OnAnimQueueComplete(this.sleep.interrupt_noise_transition);
			this.sleep.interrupt_noise_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				Effects component = smi.master.GetComponent<Effects>();
				component.Add(Db.Get().effects.Get("TerribleSleep"), true);
				if (component.HasEffect(Db.Get().effects.Get("BadSleep")))
				{
					component.Remove(Db.Get().effects.Get("BadSleep"));
				}
				this.isDisturbedByNoise.Set(false, smi, false);
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				smi.GoTo(state);
			});
			this.sleep.interrupt_light.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByLight, null).QueueAnim("interrupt", false, null).OnAnimQueueComplete(this.sleep.interrupt_light_transition);
			this.sleep.interrupt_light_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleep"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByLight.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.success.Enter(delegate(SleepChore.StatesInstance smi)
			{
				smi.EvaluateSleepQuality();
			}).ReturnSuccess();
		}

		// Token: 0x0400620A RID: 25098
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter sleeper;

		// Token: 0x0400620B RID: 25099
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter bed;

		// Token: 0x0400620C RID: 25100
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isInterruptable;

		// Token: 0x0400620D RID: 25101
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByNoise;

		// Token: 0x0400620E RID: 25102
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByLight;

		// Token: 0x0400620F RID: 25103
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByMovement;

		// Token: 0x04006210 RID: 25104
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByCold;

		// Token: 0x04006211 RID: 25105
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isScaredOfDark;

		// Token: 0x04006212 RID: 25106
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter needsNightLight;

		// Token: 0x04006213 RID: 25107
		public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04006214 RID: 25108
		public SleepChore.States.SleepStates sleep;

		// Token: 0x04006215 RID: 25109
		public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State success;

		// Token: 0x020023EB RID: 9195
		public class SleepStates : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State
		{
			// Token: 0x0400A051 RID: 41041
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition;

			// Token: 0x0400A052 RID: 41042
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition_pre;

			// Token: 0x0400A053 RID: 41043
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State uninterruptable;

			// Token: 0x0400A054 RID: 41044
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State normal;

			// Token: 0x0400A055 RID: 41045
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise;

			// Token: 0x0400A056 RID: 41046
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise_transition;

			// Token: 0x0400A057 RID: 41047
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light;

			// Token: 0x0400A058 RID: 41048
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light_transition;

			// Token: 0x0400A059 RID: 41049
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_scared;

			// Token: 0x0400A05A RID: 41050
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_scared_transition;

			// Token: 0x0400A05B RID: 41051
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_movement;

			// Token: 0x0400A05C RID: 41052
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_movement_transition;

			// Token: 0x0400A05D RID: 41053
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_cold;

			// Token: 0x0400A05E RID: 41054
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_cold_transition;
		}
	}
}
