using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000985 RID: 2437
public class GunkMonitor : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>
{
	// Token: 0x0600472A RID: 18218 RVA: 0x00197368 File Offset: 0x00195568
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EnterTransition(this.mildUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.IsGunkLevelsOverMildUrgeThreshold)).OnSignal(this.GunkChangedSignal, this.mildUrge, new Func<GunkMonitor.Instance, bool>(GunkMonitor.IsGunkLevelsOverMildUrgeThreshold));
		this.mildUrge.EnterTransition(this.criticalUrge, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold)).ToggleThought(Db.Get().Thoughts.ExpellGunkDesire, null).OnSignal(this.GunkChangedSignal, this.criticalUrge, new Func<GunkMonitor.Instance, bool>(GunkMonitor.IsGunkLevelsOverCriticalUrgeThreshold)).OnSignal(this.GunkMaxedOutSignal, this.criticalUrge).OnSignal(this.GunkEmptiedSignal, this.idle).DefaultState(this.mildUrge.prevented);
		this.mildUrge.prevented.EventTransition(GameHashes.ScheduleBlocksChanged, this.mildUrge.allowed, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling)).EventTransition(GameHashes.ScheduleChanged, this.mildUrge.allowed, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling));
		this.mildUrge.allowed.EventTransition(GameHashes.ScheduleBlocksChanged, this.mildUrge.prevented, GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Not(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling))).EventTransition(GameHashes.ScheduleChanged, this.mildUrge.prevented, GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Not(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.ScheduleAllowsExpelling))).ToggleUrge(Db.Get().Urges.Pee).ToggleUrge(Db.Get().Urges.GunkPee);
		this.criticalUrge.EnterTransition(this.cantHold, new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Transition.ConditionCallback(GunkMonitor.CanNotHoldGunkAnymore)).OnSignal(this.GunkMaxedOutSignal, this.cantHold).OnSignal(this.GunkEmptiedSignal, this.idle).ToggleUrge(Db.Get().Urges.GunkPee).ToggleUrge(Db.Get().Urges.Pee).ToggleEffect("GunkSick").ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
		this.cantHold.ToggleUrge(Db.Get().Urges.GunkPee).ToggleThought(Db.Get().Thoughts.ExpellingGunk, null).ToggleChore((GunkMonitor.Instance smi) => new BionicGunkSpillChore(smi.master), this.emptyRemaining);
		this.emptyRemaining.Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ExpellAllGunk)).Enter(new StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State.Callback(GunkMonitor.ApplyGunkHungoverEffect)).GoTo(this.idle);
	}

	// Token: 0x0600472B RID: 18219 RVA: 0x00197642 File Offset: 0x00195842
	public static bool IsGunkLevelsOverCriticalUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.DesperetlySeekForGunkToiletTreshold;
	}

	// Token: 0x0600472C RID: 18220 RVA: 0x0019765A File Offset: 0x0019585A
	public static bool IsGunkLevelsOverMildUrgeThreshold(GunkMonitor.Instance smi)
	{
		return smi.CurrentGunkPercentage >= smi.def.SeekForGunkToiletTreshold_InSchedule;
	}

	// Token: 0x0600472D RID: 18221 RVA: 0x00197672 File Offset: 0x00195872
	public static bool ScheduleAllowsExpelling(GunkMonitor.Instance smi)
	{
		return smi.DoesCurrentScheduleAllowsGunkToilet;
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x0019767A File Offset: 0x0019587A
	public static bool DoesNotWantToExpellGunk(GunkMonitor.Instance smi)
	{
		return !GunkMonitor.IsGunkLevelsOverMildUrgeThreshold(smi);
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x00197685 File Offset: 0x00195885
	public static bool CanNotHoldGunkAnymore(GunkMonitor.Instance smi)
	{
		return smi.IsGunkBuildupAtMax;
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x0019768D File Offset: 0x0019588D
	public static void ExpellAllGunk(GunkMonitor.Instance smi)
	{
		smi.ExpellAllGunk(null);
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x00197696 File Offset: 0x00195896
	public static void ApplyGunkHungoverEffect(GunkMonitor.Instance smi)
	{
		smi.GetComponent<Effects>().Add("GunkHungover", true);
	}

	// Token: 0x04002E6D RID: 11885
	public static readonly float GUNK_CAPACITY = 50f;

	// Token: 0x04002E6E RID: 11886
	public const string GUNK_FULL_EFFECT_NAME = "GunkSick";

	// Token: 0x04002E6F RID: 11887
	public const string GUNK_HUNGOVER_EFFECT_NAME = "GunkHungover";

	// Token: 0x04002E70 RID: 11888
	public static SimHashes GunkElement = SimHashes.LiquidGunk;

	// Token: 0x04002E71 RID: 11889
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State idle;

	// Token: 0x04002E72 RID: 11890
	public GunkMonitor.MildUrgeStates mildUrge;

	// Token: 0x04002E73 RID: 11891
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State criticalUrge;

	// Token: 0x04002E74 RID: 11892
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State cantHold;

	// Token: 0x04002E75 RID: 11893
	public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State emptyRemaining;

	// Token: 0x04002E76 RID: 11894
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal GunkChangedSignal;

	// Token: 0x04002E77 RID: 11895
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal GunkMaxedOutSignal;

	// Token: 0x04002E78 RID: 11896
	public StateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.Signal GunkEmptiedSignal;

	// Token: 0x0200193C RID: 6460
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040078E3 RID: 30947
		public float SeekForGunkToiletTreshold_InSchedule = 0.6f;

		// Token: 0x040078E4 RID: 30948
		public float DesperetlySeekForGunkToiletTreshold = 0.9f;
	}

	// Token: 0x0200193D RID: 6461
	public class MildUrgeStates : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State
	{
		// Token: 0x040078E5 RID: 30949
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State allowed;

		// Token: 0x040078E6 RID: 30950
		public GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.State prevented;
	}

	// Token: 0x0200193E RID: 6462
	public new class Instance : GameStateMachine<GunkMonitor, GunkMonitor.Instance, IStateMachineTarget, GunkMonitor.Def>.GameInstance
	{
		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06009BCD RID: 39885 RVA: 0x00370EF2 File Offset: 0x0036F0F2
		public bool HasGunk
		{
			get
			{
				return this.CurrentGunkMass > 0f;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06009BCE RID: 39886 RVA: 0x00370F01 File Offset: 0x0036F101
		public bool IsGunkBuildupAtMax
		{
			get
			{
				return this.CurrentGunkPercentage >= 1f;
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06009BCF RID: 39887 RVA: 0x00370F13 File Offset: 0x0036F113
		public float CurrentGunkMass
		{
			get
			{
				if (this.gunkAmount != null)
				{
					return this.gunkAmount.value;
				}
				return 0f;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06009BD0 RID: 39888 RVA: 0x00370F2E File Offset: 0x0036F12E
		public float CurrentGunkPercentage
		{
			get
			{
				return this.CurrentGunkMass / this.gunkAmount.GetMax();
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06009BD1 RID: 39889 RVA: 0x00370F42 File Offset: 0x0036F142
		public bool DoesCurrentScheduleAllowsGunkToilet
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat) || this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
			}
		}

		// Token: 0x06009BD2 RID: 39890 RVA: 0x00370F7C File Offset: 0x0036F17C
		public Instance(IStateMachineTarget master, GunkMonitor.Def def) : base(master, def)
		{
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.gunkAmount = Db.Get().Amounts.BionicGunk.Lookup(base.gameObject);
			this.oilAmount = Db.Get().Amounts.BionicOil.Lookup(base.gameObject);
			AmountInstance amountInstance = this.oilAmount;
			amountInstance.OnValueChanged = (Action<float>)Delegate.Combine(amountInstance.OnValueChanged, new Action<float>(this.OnOilValueChanged));
			AmountInstance amountInstance2 = this.gunkAmount;
			amountInstance2.OnValueChanged = (Action<float>)Delegate.Combine(amountInstance2.OnValueChanged, new Action<float>(this.OnGunkValueChanged));
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x06009BD3 RID: 39891 RVA: 0x0037104B File Offset: 0x0036F24B
		private void OnMaxGunkBuildupReached()
		{
			base.sm.GunkMaxedOutSignal.Trigger(base.smi);
		}

		// Token: 0x06009BD4 RID: 39892 RVA: 0x00371063 File Offset: 0x0036F263
		private void OnGunkEmptied()
		{
			base.sm.GunkEmptiedSignal.Trigger(base.smi);
		}

		// Token: 0x06009BD5 RID: 39893 RVA: 0x0037107B File Offset: 0x0036F27B
		private void OnGunkValueChanged(float delta)
		{
			base.sm.GunkChangedSignal.Trigger(base.smi);
		}

		// Token: 0x06009BD6 RID: 39894 RVA: 0x00371094 File Offset: 0x0036F294
		private void OnOilValueChanged(float delta)
		{
			float num = (delta < 0f) ? Mathf.Abs(delta) : 0f;
			float gunkMassValue = Mathf.Clamp(this.CurrentGunkMass + num, 0f, this.gunkAmount.GetMax());
			this.SetGunkMassValue(gunkMassValue);
		}

		// Token: 0x06009BD7 RID: 39895 RVA: 0x003710DC File Offset: 0x0036F2DC
		public void SetGunkMassValue(float value)
		{
			bool flag = this.CurrentGunkMass != value;
			this.gunkAmount.SetValue(value);
			if (flag)
			{
				if (this.CurrentGunkMass <= 0f)
				{
					this.OnGunkEmptied();
					return;
				}
				if (this.IsGunkBuildupAtMax)
				{
					this.OnMaxGunkBuildupReached();
					return;
				}
				base.sm.GunkChangedSignal.Trigger(this);
			}
		}

		// Token: 0x06009BD8 RID: 39896 RVA: 0x00371138 File Offset: 0x0036F338
		public void ExpellGunk(float mass, Storage targetStorage = null)
		{
			if (this.HasGunk)
			{
				float currentGunkMass = this.CurrentGunkMass;
				float num = Mathf.Min(mass, this.CurrentGunkMass);
				num = Mathf.Max(num, Mathf.Epsilon);
				int gameCell = Grid.PosToCell(base.transform.position);
				byte index = Db.Get().Diseases.GetIndex(DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE);
				float num2 = num / GunkMonitor.GUNK_CAPACITY;
				Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
				if (equippable != null)
				{
					equippable.GetComponent<Storage>().AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
				}
				else if (targetStorage != null)
				{
					targetStorage.AddLiquid(GunkMonitor.GunkElement, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), false, true);
				}
				else
				{
					SimMessages.AddRemoveSubstance(gameCell, GunkMonitor.GunkElement, CellEventLogger.Instance.Vomit, num, this.bodyTemperature.value, index, (int)((float)DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE * num2), true, -1);
				}
				if (Sim.IsRadiationEnabled())
				{
					MinionIdentity component = base.transform.GetComponent<MinionIdentity>();
					AmountInstance amountInstance = Db.Get().Amounts.RadiationBalance.Lookup(component);
					RadiationMonitor.Instance smi = component.GetSMI<RadiationMonitor.Instance>();
					float num3 = DUPLICANTSTATS.STANDARD.BaseStats.BLADDER_INCREASE_PER_SECOND / DUPLICANTSTATS.BIONICS.BaseStats.BLADDER_INCREASE_PER_SECOND;
					float num4 = Math.Min(amountInstance.value, 100f * num3 * smi.difficultySettingMod * num2);
					if (num4 >= 1f)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, Math.Floor((double)num4).ToString() + UI.UNITSUFFIXES.RADIATION.RADS, component.transform, Vector3.up * 2f, 1.5f, false, false);
					}
					amountInstance.ApplyDelta(-num4);
				}
				this.SetGunkMassValue(Mathf.Clamp(this.CurrentGunkMass - num, 0f, this.gunkAmount.GetMax()));
			}
		}

		// Token: 0x06009BD9 RID: 39897 RVA: 0x00371367 File Offset: 0x0036F567
		public void ExpellAllGunk(Storage targetStorage = null)
		{
			this.ExpellGunk(this.CurrentGunkMass, targetStorage);
		}

		// Token: 0x040078E7 RID: 30951
		private AmountInstance oilAmount;

		// Token: 0x040078E8 RID: 30952
		private AmountInstance gunkAmount;

		// Token: 0x040078E9 RID: 30953
		private AmountInstance bodyTemperature;

		// Token: 0x040078EA RID: 30954
		private Schedulable schedulable;
	}
}
