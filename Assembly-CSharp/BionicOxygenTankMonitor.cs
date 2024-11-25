using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000969 RID: 2409
public class BionicOxygenTankMonitor : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>
{
	// Token: 0x06004694 RID: 18068 RVA: 0x00193A64 File Offset: 0x00191C64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.fistSpawn;
		this.fistSpawn.ParamTransition<bool>(this.HasSpawnedBefore, this.safe, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.IsTrue).Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.StartWithFullTank));
		this.safe.Transition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)), UpdateRate.SIM_200ms);
		this.lowOxygen.Transition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe), UpdateRate.SIM_200ms).Transition(this.critical, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical), UpdateRate.SIM_200ms).DefaultState(this.lowOxygen.idle);
		this.lowOxygen.idle.EventTransition(GameHashes.ScheduleBlocksChanged, this.lowOxygen.scheduleSearch, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule)).EventTransition(GameHashes.ScheduleChanged, this.lowOxygen.scheduleSearch, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule));
		this.lowOxygen.scheduleSearch.EventTransition(GameHashes.ScheduleBlocksChanged, this.lowOxygen.idle, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule))).EventTransition(GameHashes.ScheduleChanged, this.lowOxygen.idle, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenSourceItemsBySchedule))).Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).ToggleUrge(Db.Get().Urges.FindOxygenRefill).ToggleRecurringChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master), null).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors));
		this.critical.ToggleUrge(Db.Get().Urges.FindOxygenRefill).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors)).DefaultState(this.critical.enableSensors);
		this.critical.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo(this.critical.seekingOxygenSourceMode);
		this.critical.seekingOxygenSourceMode.Transition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical)), UpdateRate.SIM_200ms).OnSignal(this.ClosestOxygenSourceChanged, this.critical.environmentAbsorbMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.NoOxygenSourceAvailable)).OnSignal(this.OxygenSourceItemLostSignal, this.critical.environmentAbsorbMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.NoOxygenSourceAvailable)).ToggleRecurringChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master), null);
		this.critical.environmentAbsorbMode.DefaultState(this.critical.environmentAbsorbMode.running);
		this.critical.environmentAbsorbMode.running.ToggleChore((BionicOxygenTankMonitor.Instance smi) => new BionicMassOxygenAbsorbChore(smi.master), this.critical.environmentAbsorbMode.success, this.critical.environmentAbsorbMode.failed);
		this.critical.environmentAbsorbMode.failed.EnterTransition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.seekingOxygenSourceMode);
		this.critical.environmentAbsorbMode.success.EnterTransition(this.lowOxygen, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.seekingOxygenSourceMode);
	}

	// Token: 0x06004695 RID: 18069 RVA: 0x00193E05 File Offset: 0x00192005
	public static bool IsAllowedToSeekOxygenSourceItemsBySchedule(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.IsAllowedToSeekOxygenBySchedule;
	}

	// Token: 0x06004696 RID: 18070 RVA: 0x00193E0D File Offset: 0x0019200D
	public static bool AreOxygenLevelsSafe(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage >= 0.5f;
	}

	// Token: 0x06004697 RID: 18071 RVA: 0x00193E1F File Offset: 0x0019201F
	public static bool AreOxygenLevelsCritical(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage < 0.2f;
	}

	// Token: 0x06004698 RID: 18072 RVA: 0x00193E2E File Offset: 0x0019202E
	public static bool IsThereAnOxygenSourceItemAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() != null;
	}

	// Token: 0x06004699 RID: 18073 RVA: 0x00193E3C File Offset: 0x0019203C
	public static bool NoOxygenSourceAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() == null;
	}

	// Token: 0x0600469A RID: 18074 RVA: 0x00193E4A File Offset: 0x0019204A
	public static void StartWithFullTank(BionicOxygenTankMonitor.Instance smi)
	{
		smi.AddFirstTimeSpawnedOxygen();
	}

	// Token: 0x0600469B RID: 18075 RVA: 0x00193E52 File Offset: 0x00192052
	public static void EnableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(true);
	}

	// Token: 0x0600469C RID: 18076 RVA: 0x00193E5B File Offset: 0x0019205B
	public static void DisableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(false);
	}

	// Token: 0x04002DF5 RID: 11765
	public const SimHashes INITIAL_TANK_ELEMENT = SimHashes.Oxygen;

	// Token: 0x04002DF6 RID: 11766
	public static readonly Tag INITIAL_TANK_ELEMENT_TAG = SimHashes.Oxygen.CreateTag();

	// Token: 0x04002DF7 RID: 11767
	public const float SAFE_TRESHOLD = 0.5f;

	// Token: 0x04002DF8 RID: 11768
	public const float CRITICAL_TRESHOLD = 0.2f;

	// Token: 0x04002DF9 RID: 11769
	public const float OXYGEN_TANK_CAPACITY_IN_SECONDS = 1800f;

	// Token: 0x04002DFA RID: 11770
	public static readonly float OXYGEN_TANK_CAPACITY_KG = 1800f * DUPLICANTSTATS.BIONICS.BaseStats.OXYGEN_USED_PER_SECOND;

	// Token: 0x04002DFB RID: 11771
	public static float INITIAL_OXYGEN_TEMP = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;

	// Token: 0x04002DFC RID: 11772
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State fistSpawn;

	// Token: 0x04002DFD RID: 11773
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State safe;

	// Token: 0x04002DFE RID: 11774
	public BionicOxygenTankMonitor.LowOxygenStates lowOxygen;

	// Token: 0x04002DFF RID: 11775
	public BionicOxygenTankMonitor.CriticalStates critical;

	// Token: 0x04002E00 RID: 11776
	private StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.BoolParameter HasSpawnedBefore;

	// Token: 0x04002E01 RID: 11777
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal OxygenSourceItemLostSignal;

	// Token: 0x04002E02 RID: 11778
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal ClosestOxygenSourceChanged;

	// Token: 0x020018E4 RID: 6372
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018E5 RID: 6373
	public class EnvironmentAbsorbStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x040077C4 RID: 30660
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State running;

		// Token: 0x040077C5 RID: 30661
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State success;

		// Token: 0x040077C6 RID: 30662
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State failed;
	}

	// Token: 0x020018E6 RID: 6374
	public class CriticalStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x040077C7 RID: 30663
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State enableSensors;

		// Token: 0x040077C8 RID: 30664
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State seekingOxygenSourceMode;

		// Token: 0x040077C9 RID: 30665
		public BionicOxygenTankMonitor.EnvironmentAbsorbStates environmentAbsorbMode;
	}

	// Token: 0x020018E7 RID: 6375
	public class LowOxygenStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x040077CA RID: 30666
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State idle;

		// Token: 0x040077CB RID: 30667
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State scheduleSearch;
	}

	// Token: 0x020018E8 RID: 6376
	public new class Instance : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.GameInstance, OxygenBreather.IGasProvider
	{
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06009A4F RID: 39503 RVA: 0x0036CE31 File Offset: 0x0036B031
		public bool IsAllowedToSeekOxygenBySchedule
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06009A50 RID: 39504 RVA: 0x0036CE4D File Offset: 0x0036B04D
		public bool IsEmpty
		{
			get
			{
				return this.AvailableOxygen == 0f;
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06009A51 RID: 39505 RVA: 0x0036CE5C File Offset: 0x0036B05C
		public float OxygenPercentage
		{
			get
			{
				return this.AvailableOxygen / this.storage.capacityKg;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06009A52 RID: 39506 RVA: 0x0036CE70 File Offset: 0x0036B070
		public float AvailableOxygen
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.Breathable);
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06009A53 RID: 39507 RVA: 0x0036CE82 File Offset: 0x0036B082
		public float SpaceAvailableInTank
		{
			get
			{
				return this.storage.capacityKg - this.AvailableOxygen;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06009A55 RID: 39509 RVA: 0x0036CE9F File Offset: 0x0036B09F
		// (set) Token: 0x06009A54 RID: 39508 RVA: 0x0036CE96 File Offset: 0x0036B096
		public Storage storage { get; private set; }

		// Token: 0x06009A56 RID: 39510 RVA: 0x0036CEA8 File Offset: 0x0036B0A8
		public Instance(IStateMachineTarget master, BionicOxygenTankMonitor.Def def) : base(master, def)
		{
			Sensors component = base.GetComponent<Sensors>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.oxygenSourceSensors = new ClosestPickupableSensor<Pickupable>[]
			{
				component.GetSensor<ClosestOxygenCanisterSensor>(),
				component.GetSensor<ClosestOxyliteSensor>()
			};
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.OnItemChanged = (Action<Pickupable>)Delegate.Combine(closestPickupableSensor.OnItemChanged, new Action<Pickupable>(this.OnOxygenSourceSensorItemChanged));
			}
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicOxygenTankStorage);
			this.oxygenTankAmountInstance = Db.Get().Amounts.BionicOxygenTank.Lookup(base.gameObject);
			this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
		}

		// Token: 0x06009A57 RID: 39511 RVA: 0x0036CFC6 File Offset: 0x0036B1C6
		public Pickupable GetClosestOxygenSource()
		{
			return this.closestOxygenSource;
		}

		// Token: 0x06009A58 RID: 39512 RVA: 0x0036CFCE File Offset: 0x0036B1CE
		private void OnOxygenSourceSensorItemChanged(object o)
		{
			this.CompareOxygenSources();
		}

		// Token: 0x06009A59 RID: 39513 RVA: 0x0036CFD6 File Offset: 0x0036B1D6
		private void OnOxygenTankStorageChanged(object o)
		{
			this.RefreshAmountInstance();
		}

		// Token: 0x06009A5A RID: 39514 RVA: 0x0036CFDE File Offset: 0x0036B1DE
		public void RefreshAmountInstance()
		{
			this.oxygenTankAmountInstance.SetValue(this.AvailableOxygen);
		}

		// Token: 0x06009A5B RID: 39515 RVA: 0x0036CFF4 File Offset: 0x0036B1F4
		public void AddFirstTimeSpawnedOxygen()
		{
			this.storage.AddElement(SimHashes.Oxygen, this.storage.capacityKg - this.AvailableOxygen, BionicOxygenTankMonitor.INITIAL_OXYGEN_TEMP, byte.MaxValue, 0, false, true);
			base.sm.HasSpawnedBefore.Set(true, this, false);
		}

		// Token: 0x06009A5C RID: 39516 RVA: 0x0036D048 File Offset: 0x0036B248
		private void CompareOxygenSources()
		{
			Pickupable item = this.closestOxygenSource;
			float num = 2.1474836E+09f;
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				int itemNavCost = closestPickupableSensor.GetItemNavCost();
				if ((float)itemNavCost < num)
				{
					num = (float)itemNavCost;
					item = closestPickupableSensor.GetItem();
				}
			}
			bool flag = item != this.closestOxygenSource;
			this.closestOxygenSource = item;
			if (flag)
			{
				base.sm.ClosestOxygenSourceChanged.Trigger(this);
			}
		}

		// Token: 0x06009A5D RID: 39517 RVA: 0x0036D0BD File Offset: 0x0036B2BD
		public float AddGas(Sim.MassConsumedCallback mass_cb_info)
		{
			return this.AddGas(ElementLoader.elements[(int)mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
		}

		// Token: 0x06009A5E RID: 39518 RVA: 0x0036D0F4 File Offset: 0x0036B2F4
		public float AddGas(SimHashes element, float mass, float temperature, byte disseaseIDX = 255, int _disseaseCount = 0)
		{
			float num = Mathf.Min(mass, this.SpaceAvailableInTank);
			float result = mass - num;
			float num2 = num / mass;
			int disease_count = Mathf.CeilToInt((float)_disseaseCount * num2);
			this.storage.AddElement(element, num, temperature, disseaseIDX, disease_count, false, true);
			return result;
		}

		// Token: 0x06009A5F RID: 39519 RVA: 0x0036D134 File Offset: 0x0036B334
		public void SetOxygenSourceSensorsActiveState(bool shouldItBeActive)
		{
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.SetActive(shouldItBeActive);
				if (shouldItBeActive)
				{
					closestPickupableSensor.Update();
				}
			}
		}

		// Token: 0x06009A60 RID: 39520 RVA: 0x0036D170 File Offset: 0x0036B370
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.IsEmpty)
			{
				return false;
			}
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out num2);
			Game.Instance.accumulators.Accumulate(oxygen_breather.O2Accumulator, num);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, -num, oxygen_breather.GetProperName(), null);
			return true;
		}

		// Token: 0x06009A61 RID: 39521 RVA: 0x0036D1CA File Offset: 0x0036B3CA
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06009A62 RID: 39522 RVA: 0x0036D1CC File Offset: 0x0036B3CC
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x06009A63 RID: 39523 RVA: 0x0036D1CE File Offset: 0x0036B3CE
		public bool IsLowOxygen()
		{
			return this.IsEmpty;
		}

		// Token: 0x06009A64 RID: 39524 RVA: 0x0036D1D6 File Offset: 0x0036B3D6
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x06009A65 RID: 39525 RVA: 0x0036D1D9 File Offset: 0x0036B3D9
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x06009A66 RID: 39526 RVA: 0x0036D1DC File Offset: 0x0036B3DC
		protected override void OnCleanUp()
		{
			if (this.storage != null)
			{
				Storage storage = this.storage;
				storage.OnStorageChange = (Action<GameObject>)Delegate.Remove(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x040077CC RID: 30668
		public AttributeInstance airConsumptionRate;

		// Token: 0x040077CD RID: 30669
		private Schedulable schedulable;

		// Token: 0x040077CE RID: 30670
		private AmountInstance oxygenTankAmountInstance;

		// Token: 0x040077CF RID: 30671
		private ClosestPickupableSensor<Pickupable>[] oxygenSourceSensors;

		// Token: 0x040077D0 RID: 30672
		private Pickupable closestOxygenSource;
	}
}
