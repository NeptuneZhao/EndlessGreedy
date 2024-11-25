using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000723 RID: 1827
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/ManualGenerator")]
public class ManualGenerator : RemoteWorkable, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06003053 RID: 12371 RVA: 0x0010B22F File Offset: 0x0010942F
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06003054 RID: 12372 RVA: 0x0010B236 File Offset: 0x00109436
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06003055 RID: 12373 RVA: 0x0010B242 File Offset: 0x00109442
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x0010B245 File Offset: 0x00109445
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x0010B24C File Offset: 0x0010944C
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x0010B253 File Offset: 0x00109453
	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	// Token: 0x06003059 RID: 12377 RVA: 0x0010B261 File Offset: 0x00109461
	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	// Token: 0x0600305A RID: 12378 RVA: 0x0010B270 File Offset: 0x00109470
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP";
	}

	// Token: 0x0600305B RID: 12379 RVA: 0x0010B277 File Offset: 0x00109477
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP"), this.batteryRefillPercent * 100f);
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x0600305C RID: 12380 RVA: 0x0010B29E File Offset: 0x0010949E
	public bool IsPowered
	{
		get
		{
			return this.operational.IsActive;
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x0600305D RID: 12381 RVA: 0x0010B2AB File Offset: 0x001094AB
	public override Chore RemoteDockChore
	{
		get
		{
			return this.chore;
		}
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x0010B2B3 File Offset: 0x001094B3
	private ManualGenerator()
	{
		this.showProgressBar = false;
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x0010B2D0 File Offset: 0x001094D0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualGenerator>(-592767678, ManualGenerator.OnOperationalChangedDelegate);
		base.Subscribe<ManualGenerator>(824508782, ManualGenerator.OnActiveChangedDelegate);
		base.Subscribe<ManualGenerator>(-905833192, ManualGenerator.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.GeneratingPower;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		EnergyGenerator.EnsureStatusItemAvailable();
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x0010B378 File Offset: 0x00109578
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		foreach (KAnimHashedString symbol in ManualGenerator.symbol_names)
		{
			component.SetSymbolVisiblity(symbol, false);
		}
		Building component2 = base.GetComponent<Building>();
		this.powerCell = component2.GetPowerOutputCell();
		this.OnActiveChanged(null);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_generatormanual_kanim")
		};
		this.smi = new ManualGenerator.GeneratePowerSM.Instance(this);
		this.smi.StartSM();
		Game.Instance.energySim.AddManualGenerator(this);
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x0010B422 File Offset: 0x00109622
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveManualGenerator(this);
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x0010B44A File Offset: 0x0010964A
	protected void OnActiveChanged(object is_active)
	{
		if (this.operational.IsActive)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ManualGeneratorChargingUp, null);
		}
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x0010B484 File Offset: 0x00109684
	private void OnCopySettings(object data)
	{
		GameObject gameObject = data as GameObject;
		if (gameObject != null)
		{
			ManualGenerator component = gameObject.GetComponent<ManualGenerator>();
			if (component != null)
			{
				this.batteryRefillPercent = component.batteryRefillPercent;
			}
		}
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x0010B4B8 File Offset: 0x001096B8
	public void EnergySim200ms(float dt)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.operational.IsActive)
		{
			this.generator.GenerateJoules(this.generator.WattageRating * dt, false);
			component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this.generator);
			return;
		}
		this.generator.ResetJoules();
		component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.GeneratorOffline, null);
		if (this.operational.IsOperational)
		{
			CircuitManager circuitManager = Game.Instance.circuitManager;
			if (circuitManager == null)
			{
				return;
			}
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag = circuitManager.HasBatteries(circuitID);
			bool flag2 = false;
			if (!flag && circuitManager.HasConsumers(circuitID))
			{
				flag2 = true;
			}
			else if (flag)
			{
				if (this.batteryRefillPercent <= 0f && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) <= 0f)
				{
					flag2 = true;
				}
				else if (circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < this.batteryRefillPercent)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				if (this.chore == null && this.smi.GetCurrentState() == this.smi.sm.on)
				{
					this.chore = new WorkChore<ManualGenerator>(Db.Get().ChoreTypes.GeneratePower, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				}
			}
			else if (this.chore != null)
			{
				this.chore.Cancel("No refill needed");
				this.chore = null;
			}
			component.ToggleStatusItem(EnergyGenerator.BatteriesSufficientlyFull, !flag2, null);
		}
	}

	// Token: 0x06003065 RID: 12389 RVA: 0x0010B654 File Offset: 0x00109854
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06003066 RID: 12390 RVA: 0x0010B66C File Offset: 0x0010986C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		CircuitManager circuitManager = Game.Instance.circuitManager;
		bool flag = false;
		if (circuitManager != null)
		{
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag2 = circuitManager.HasBatteries(circuitID);
			flag = ((flag2 && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < 1f) || (!flag2 && circuitManager.HasConsumers(circuitID)));
		}
		AttributeLevels component = worker.GetComponent<AttributeLevels>();
		if (component != null)
		{
			component.AddExperience(Db.Get().Attributes.Athletics.Id, dt, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
		return !flag;
	}

	// Token: 0x06003067 RID: 12391 RVA: 0x0010B6F8 File Offset: 0x001098F8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06003068 RID: 12392 RVA: 0x0010B70E File Offset: 0x0010990E
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		if (this.chore != null)
		{
			this.chore.Cancel("complete");
			this.chore = null;
		}
	}

	// Token: 0x06003069 RID: 12393 RVA: 0x0010B73C File Offset: 0x0010993C
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x0600306A RID: 12394 RVA: 0x0010B73F File Offset: 0x0010993F
	private void OnOperationalChanged(object data)
	{
		if (!this.buildingEnabledButton.IsEnabled)
		{
			this.generator.ResetJoules();
		}
	}

	// Token: 0x04001C51 RID: 7249
	[Serialize]
	[SerializeField]
	private float batteryRefillPercent = 0.5f;

	// Token: 0x04001C52 RID: 7250
	private const float batteryStopRunningPercent = 1f;

	// Token: 0x04001C53 RID: 7251
	[MyCmpReq]
	private Generator generator;

	// Token: 0x04001C54 RID: 7252
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001C55 RID: 7253
	[MyCmpGet]
	private BuildingEnabledButton buildingEnabledButton;

	// Token: 0x04001C56 RID: 7254
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C57 RID: 7255
	private Chore chore;

	// Token: 0x04001C58 RID: 7256
	private int powerCell;

	// Token: 0x04001C59 RID: 7257
	private ManualGenerator.GeneratePowerSM.Instance smi;

	// Token: 0x04001C5A RID: 7258
	private static readonly KAnimHashedString[] symbol_names = new KAnimHashedString[]
	{
		"meter",
		"meter_target",
		"meter_fill",
		"meter_frame",
		"meter_light",
		"meter_tubing"
	};

	// Token: 0x04001C5B RID: 7259
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001C5C RID: 7260
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x04001C5D RID: 7261
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001566 RID: 5478
	public class GeneratePowerSM : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance>
	{
		// Token: 0x06008E41 RID: 36417 RVA: 0x003427E8 File Offset: 0x003409E8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.on.EventTransition(GameHashes.OperationalChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsActive).PlayAnim("on");
			this.working.DefaultState(this.working.pre);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => this.masterTarget.Get(smi) != null && !smi.master.GetComponent<Operational>().IsActive);
		}

		// Token: 0x04006CA8 RID: 27816
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State off;

		// Token: 0x04006CA9 RID: 27817
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State on;

		// Token: 0x04006CAA RID: 27818
		public ManualGenerator.GeneratePowerSM.WorkingStates working;

		// Token: 0x020024FF RID: 9471
		public class WorkingStates : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x0400A49F RID: 42143
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pre;

			// Token: 0x0400A4A0 RID: 42144
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State loop;

			// Token: 0x0400A4A1 RID: 42145
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x02002500 RID: 9472
		public new class Instance : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x0600BCAF RID: 48303 RVA: 0x003D6602 File Offset: 0x003D4802
			public Instance(IStateMachineTarget master) : base(master)
			{
			}
		}
	}
}
