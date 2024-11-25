using System;
using Klei;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000740 RID: 1856
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/OilWellCap")]
public class OilWellCap : Workable, ISingleSliderControl, ISliderControl, IElementEmitter
{
	// Token: 0x17000328 RID: 808
	// (get) Token: 0x0600315F RID: 12639 RVA: 0x0011040A File Offset: 0x0010E60A
	public SimHashes Element
	{
		get
		{
			return this.gasElement;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06003160 RID: 12640 RVA: 0x00110412 File Offset: 0x0010E612
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06003161 RID: 12641 RVA: 0x00110429 File Offset: 0x0010E629
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06003162 RID: 12642 RVA: 0x00110430 File Offset: 0x0010E630
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x0011043C File Offset: 0x0010E63C
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x0011043F File Offset: 0x0010E63F
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x00110446 File Offset: 0x0010E646
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x0011044D File Offset: 0x0010E64D
	public float GetSliderValue(int index)
	{
		return this.depressurizePercent * 100f;
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x0011045B File Offset: 0x0010E65B
	public void SetSliderValue(float value, int index)
	{
		this.depressurizePercent = value / 100f;
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x0011046A File Offset: 0x0010E66A
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x00110471 File Offset: 0x0010E671
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP"), this.depressurizePercent * 100f);
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x00110498 File Offset: 0x0010E698
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OilWellCap>(-905833192, OilWellCap.OnCopySettingsDelegate);
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x001104B4 File Offset: 0x0010E6B4
	private void OnCopySettings(object data)
	{
		OilWellCap component = ((GameObject)data).GetComponent<OilWellCap>();
		if (component != null)
		{
			this.depressurizePercent = component.depressurizePercent;
		}
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x001104E4 File Offset: 0x0010E6E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.accumulator = Game.Instance.accumulators.Add("pressuregas", this);
		this.showProgressBar = false;
		base.SetWorkTime(float.PositiveInfinity);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oil_cap_kanim")
		};
		this.workingStatusItem = Db.Get().BuildingStatusItems.ReleasingPressure;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.pressureMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.smi = new OilWellCap.StatesInstance(this);
		this.smi.StartSM();
		this.UpdatePressurePercent();
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x001105F9 File Offset: 0x0010E7F9
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x00110622 File Offset: 0x0010E822
	public void AddGasPressure(float dt)
	{
		this.storage.AddGasChunk(this.gasElement, this.addGasRate * dt, this.gasTemperature, 0, 0, true, true);
		this.UpdatePressurePercent();
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x00110650 File Offset: 0x0010E850
	public void ReleaseGasPressure(float dt)
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.gasElement);
		if (primaryElement != null && primaryElement.Mass > 0f)
		{
			float num = this.releaseGasRate * dt;
			if (base.worker != null)
			{
				num *= this.GetEfficiencyMultiplier(base.worker);
			}
			num = Mathf.Min(num, primaryElement.Mass);
			SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(primaryElement, num / primaryElement.Mass);
			primaryElement.Mass -= num;
			Game.Instance.accumulators.Accumulate(this.accumulator, num);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(this), ElementLoader.GetElementIndex(this.gasElement), null, num, primaryElement.Temperature, percentOfDisease.idx, percentOfDisease.count, true, -1);
		}
		this.UpdatePressurePercent();
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x00110724 File Offset: 0x0010E924
	private void UpdatePressurePercent()
	{
		float num = this.storage.GetMassAvailable(this.gasElement) / this.maxGasPressure;
		num = Mathf.Clamp01(num);
		this.smi.sm.pressurePercent.Set(num, this.smi, false);
		this.pressureMeter.SetPositionPercent(num);
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x0011077B File Offset: 0x0010E97B
	public bool NeedsDepressurizing()
	{
		return this.smi.GetPressurePercent() >= this.depressurizePercent;
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x00110794 File Offset: 0x0010E994
	private WorkChore<OilWellCap> CreateWorkChore()
	{
		this.DepressurizeChore = new WorkChore<OilWellCap>(Db.Get().ChoreTypes.Depressurize, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.DepressurizeChore.AddPrecondition(OilWellCap.AllowedToDepressurize, this);
		return this.DepressurizeChore;
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x001107E4 File Offset: 0x0010E9E4
	private void CancelChore(string reason)
	{
		if (this.DepressurizeChore != null)
		{
			this.DepressurizeChore.Cancel(reason);
		}
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x001107FA File Offset: 0x0010E9FA
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.working.Set(true, this.smi, false);
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x00110821 File Offset: 0x0010EA21
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.working.Set(false, this.smi, false);
		this.DepressurizeChore = null;
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x0011084F File Offset: 0x0010EA4F
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.smi.GetPressurePercent() <= 0f;
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x00110866 File Offset: 0x0010EA66
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.ReleaseGasPressure(60f);
		return true;
	}

	// Token: 0x04001D09 RID: 7433
	private OilWellCap.StatesInstance smi;

	// Token: 0x04001D0A RID: 7434
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001D0B RID: 7435
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04001D0C RID: 7436
	public SimHashes gasElement;

	// Token: 0x04001D0D RID: 7437
	public float gasTemperature;

	// Token: 0x04001D0E RID: 7438
	public float addGasRate = 1f;

	// Token: 0x04001D0F RID: 7439
	public float maxGasPressure = 10f;

	// Token: 0x04001D10 RID: 7440
	public float releaseGasRate = 10f;

	// Token: 0x04001D11 RID: 7441
	[Serialize]
	private float depressurizePercent = 0.75f;

	// Token: 0x04001D12 RID: 7442
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001D13 RID: 7443
	private MeterController pressureMeter;

	// Token: 0x04001D14 RID: 7444
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001D15 RID: 7445
	private static readonly EventSystem.IntraObjectHandler<OilWellCap> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OilWellCap>(delegate(OilWellCap component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001D16 RID: 7446
	private WorkChore<OilWellCap> DepressurizeChore;

	// Token: 0x04001D17 RID: 7447
	private static readonly Chore.Precondition AllowedToDepressurize = new Chore.Precondition
	{
		id = "AllowedToDepressurize",
		description = DUPLICANTS.CHORES.PRECONDITIONS.ALLOWED_TO_DEPRESSURIZE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((OilWellCap)data).NeedsDepressurizing();
		}
	};

	// Token: 0x020015A8 RID: 5544
	public class StatesInstance : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.GameInstance
	{
		// Token: 0x06008F67 RID: 36711 RVA: 0x00347558 File Offset: 0x00345758
		public StatesInstance(OilWellCap master) : base(master)
		{
		}

		// Token: 0x06008F68 RID: 36712 RVA: 0x00347561 File Offset: 0x00345761
		public float GetPressurePercent()
		{
			return base.sm.pressurePercent.Get(base.smi);
		}
	}

	// Token: 0x020015A9 RID: 5545
	public class States : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap>
	{
		// Token: 0x06008F69 RID: 36713 RVA: 0x0034757C File Offset: 0x0034577C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational));
			this.operational.DefaultState(this.operational.idle).ToggleRecurringChore((OilWellCap.StatesInstance smi) => smi.master.CreateWorkChore(), null).EventHandler(GameHashes.WorkChoreDisabled, delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.CancelChore("WorkChoreDisabled");
			});
			this.operational.idle.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational.active, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsAbleToPump));
			this.operational.active.DefaultState(this.operational.active.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).Enter(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.AddGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.active.pre.PlayAnim("working_pre").ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).OnAnimQueueComplete(this.operational.active.loop);
			this.operational.active.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.pressurePercent, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping)).EventTransition(GameHashes.OnStorageChange, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping));
			this.operational.active.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.idle);
			this.operational.overpressure.PlayAnim("over_pressured_pre", KAnim.PlayMode.Once).QueueAnim("over_pressured_loop", true, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WellOverpressure, null).ParamTransition<float>(this.pressurePercent, this.operational.idle, (OilWellCap.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue);
			this.operational.releasing_pressure.DefaultState(this.operational.releasing_pressure.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (OilWellCap.StatesInstance smi) => smi.master);
			this.operational.releasing_pressure.pre.PlayAnim("steam_out_pre").OnAnimQueueComplete(this.operational.releasing_pressure.loop);
			this.operational.releasing_pressure.loop.PlayAnim("steam_out_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).ParamTransition<bool>(this.working, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsFalse).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.ReleaseGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.releasing_pressure.pst.PlayAnim("steam_out_pst").OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x06008F6A RID: 36714 RVA: 0x00347A67 File Offset: 0x00345C67
		private bool IsOperational(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x06008F6B RID: 36715 RVA: 0x00347A79 File Offset: 0x00345C79
		private bool IsAbleToPump(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06008F6C RID: 36716 RVA: 0x00347A9B File Offset: 0x00345C9B
		private bool MustStopPumping(OilWellCap.StatesInstance smi)
		{
			return !smi.master.operational.IsOperational || !smi.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x04006D85 RID: 28037
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.FloatParameter pressurePercent;

		// Token: 0x04006D86 RID: 28038
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.BoolParameter working;

		// Token: 0x04006D87 RID: 28039
		public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State inoperational;

		// Token: 0x04006D88 RID: 28040
		public OilWellCap.States.OperationalStates operational;

		// Token: 0x02002512 RID: 9490
		public class OperationalStates : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State
		{
			// Token: 0x0400A4FC RID: 42236
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State idle;

			// Token: 0x0400A4FD RID: 42237
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState active;

			// Token: 0x0400A4FE RID: 42238
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State overpressure;

			// Token: 0x0400A4FF RID: 42239
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState releasing_pressure;
		}
	}
}
