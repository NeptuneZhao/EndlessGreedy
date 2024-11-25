using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000772 RID: 1906
public class SteamTurbine : Generator
{
	// Token: 0x1700036C RID: 876
	// (get) Token: 0x0600336F RID: 13167 RVA: 0x00119DE5 File Offset: 0x00117FE5
	// (set) Token: 0x06003370 RID: 13168 RVA: 0x00119DED File Offset: 0x00117FED
	public int BlockedInputs { get; private set; }

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06003371 RID: 13169 RVA: 0x00119DF6 File Offset: 0x00117FF6
	public int TotalInputs
	{
		get
		{
			return this.srcCells.Length;
		}
	}

	// Token: 0x06003372 RID: 13170 RVA: 0x00119E00 File Offset: 0x00118000
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Power", this);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(SteamTurbine.OnSimEmittedCallback), this, "SteamTurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -2));
		}
		this.smi = new SteamTurbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x06003373 RID: 13171 RVA: 0x00119EE0 File Offset: 0x001180E0
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
	}

	// Token: 0x06003374 RID: 13172 RVA: 0x00119F30 File Offset: 0x00118130
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "SteamTurbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x06003375 RID: 13173 RVA: 0x00119F84 File Offset: 0x00118184
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(SteamTurbine.OnSimConsumeCallback), this, "SteamTurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x06003376 RID: 13174 RVA: 0x00119FF2 File Offset: 0x001181F2
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((SteamTurbine)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x06003377 RID: 13175 RVA: 0x0011A000 File Offset: 0x00118200
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minConvertMass && this.simEmitCBHandle.IsValid())
			{
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				this.gasStorage.AddGasChunk(this.srcElem, this.storedMass, this.storedTemperature, this.diseaseIdx, this.diseaseCount, true, true);
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x0011A10E File Offset: 0x0011830E
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((SteamTurbine)data).OnSimEmitted(info);
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x0011A11C File Offset: 0x0011831C
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded != 1)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, info.mass, info.temperature);
			this.storedMass += info.mass;
			if (info.diseaseIdx != 255)
			{
				SimUtil.DiseaseInfo a = new SimUtil.DiseaseInfo
				{
					idx = this.diseaseIdx,
					count = this.diseaseCount
				};
				SimUtil.DiseaseInfo b = new SimUtil.DiseaseInfo
				{
					idx = info.diseaseIdx,
					count = info.diseaseCount
				};
				SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(a, b);
				this.diseaseIdx = diseaseInfo.idx;
				this.diseaseCount = diseaseInfo.count;
			}
		}
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x0011A1E0 File Offset: 0x001183E0
	public static void InitializeStatusItems()
	{
		SteamTurbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem = new StatusItem("TURBINE_PARTIALLY_BLOCKED_INPUT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolvePartialBlockedStatus);
		SteamTurbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientMassStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.buildingTooHotItem = new StatusItem("TURBINE_TOO_HOT", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.buildingTooHotItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.activeWattageStatusItem = new StatusItem("TURBINE_ACTIVE_WATTAGE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.activeWattageStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveWattageStatus);
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x0011A38C File Offset: 0x0011858C
	private static string ResolveWattageStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		float num = Game.Instance.accumulators.GetAverageRate(steamTurbine.accumulator) / steamTurbine.WattageRating;
		return str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Max_Wattage}", GameUtil.GetFormattedWattage(steamTurbine.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Efficiency}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None)).Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x0011A420 File Offset: 0x00118620
	private static string ResolvePartialBlockedStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		return str.Replace("{Blocked}", steamTurbine.BlockedInputs.ToString()).Replace("{Total}", steamTurbine.TotalInputs.ToString());
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x0011A468 File Offset: 0x00118668
	private static string ResolveStrings(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		str = str.Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
		str = str.Replace("{Dest_Element}", ElementLoader.FindElementByHash(steamTurbine.destElem).name);
		str = str.Replace("{Overheat_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.maxBuildingTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Active_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Min_Mass}", GameUtil.GetFormattedMass(steamTurbine.requiredMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		return str;
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x0011A50F File Offset: 0x0011870F
	public void SetStorage(Storage steamStorage, Storage waterStorage)
	{
		this.gasStorage = steamStorage;
		this.liquidStorage = waterStorage;
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x0011A520 File Offset: 0x00118720
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		if (this.gasStorage != null && this.gasStorage.items.Count > 0)
		{
			GameObject gameObject = this.gasStorage.FindFirst(ElementLoader.FindElementByHash(this.srcElem).tag);
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2 = 0.1f;
				if (component.Mass > num2)
				{
					num2 = Mathf.Min(component.Mass, this.pumpKGRate * dt);
					num = Mathf.Min(this.JoulesToGenerate(component) * (num2 / this.pumpKGRate), base.WattageRating * dt);
					float num3 = this.HeatFromCoolingSteam(component) * (num2 / component.Mass);
					float num4 = num2 / component.Mass;
					int num5 = Mathf.RoundToInt((float)component.DiseaseCount * num4);
					component.Mass -= num2;
					component.ModifyDiseaseCount(-num5, "SteamTurbine.EnergySim200ms");
					float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
					this.lastSampleTime = Time.time;
					GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, num3 * this.wasteHeatToTurbinePercent, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, display_dt);
					this.liquidStorage.AddLiquid(this.destElem, num2, this.outputElementTemperature, component.DiseaseIdx, num5, true, true);
				}
			}
		}
		num = Mathf.Clamp(num, 0f, base.WattageRating);
		Game.Instance.accumulators.Accumulate(this.accumulator, num);
		if (num > 0f)
		{
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating);
		this.meter.SetSymbolTint(SteamTurbine.TINT_SYMBOL, Color.Lerp(Color.red, Color.green, Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating));
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x0011A770 File Offset: 0x00118970
	public float HeatFromCoolingSteam(PrimaryElement steam)
	{
		float temperature = steam.Temperature;
		return -GameUtil.CalculateEnergyDeltaForElement(steam, temperature, this.outputElementTemperature);
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x0011A794 File Offset: 0x00118994
	public float JoulesToGenerate(PrimaryElement steam)
	{
		float num = (steam.Temperature - this.outputElementTemperature) / (this.idealSourceElementTemperature - this.outputElementTemperature);
		return base.WattageRating * (float)Math.Pow((double)num, 1.0);
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06003382 RID: 13186 RVA: 0x0011A7D5 File Offset: 0x001189D5
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04001E64 RID: 7780
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001E65 RID: 7781
	public SimHashes srcElem;

	// Token: 0x04001E66 RID: 7782
	public SimHashes destElem;

	// Token: 0x04001E67 RID: 7783
	public float requiredMass = 0.001f;

	// Token: 0x04001E68 RID: 7784
	public float minActiveTemperature = 398.15f;

	// Token: 0x04001E69 RID: 7785
	public float idealSourceElementTemperature = 473.15f;

	// Token: 0x04001E6A RID: 7786
	public float maxBuildingTemperature = 373.15f;

	// Token: 0x04001E6B RID: 7787
	public float outputElementTemperature = 368.15f;

	// Token: 0x04001E6C RID: 7788
	public float minConvertMass;

	// Token: 0x04001E6D RID: 7789
	public float pumpKGRate;

	// Token: 0x04001E6E RID: 7790
	public float maxSelfHeat;

	// Token: 0x04001E6F RID: 7791
	public float wasteHeatToTurbinePercent;

	// Token: 0x04001E70 RID: 7792
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x04001E71 RID: 7793
	[Serialize]
	private float storedMass;

	// Token: 0x04001E72 RID: 7794
	[Serialize]
	private float storedTemperature;

	// Token: 0x04001E73 RID: 7795
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x04001E74 RID: 7796
	[Serialize]
	private int diseaseCount;

	// Token: 0x04001E75 RID: 7797
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x04001E76 RID: 7798
	private static StatusItem inputPartiallyBlockedStatusItem;

	// Token: 0x04001E77 RID: 7799
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x04001E78 RID: 7800
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x04001E79 RID: 7801
	private static StatusItem activeWattageStatusItem;

	// Token: 0x04001E7A RID: 7802
	private static StatusItem buildingTooHotItem;

	// Token: 0x04001E7B RID: 7803
	private static StatusItem activeStatusItem;

	// Token: 0x04001E7D RID: 7805
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x04001E7E RID: 7806
	private MeterController meter;

	// Token: 0x04001E7F RID: 7807
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x04001E80 RID: 7808
	private SteamTurbine.Instance smi;

	// Token: 0x04001E81 RID: 7809
	private int[] srcCells;

	// Token: 0x04001E82 RID: 7810
	private Storage gasStorage;

	// Token: 0x04001E83 RID: 7811
	private Storage liquidStorage;

	// Token: 0x04001E84 RID: 7812
	private ElementConsumer consumer;

	// Token: 0x04001E85 RID: 7813
	private Guid statusHandle;

	// Token: 0x04001E86 RID: 7814
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001E87 RID: 7815
	private float lastSampleTime = -1f;

	// Token: 0x0200160D RID: 5645
	public class States : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine>
	{
		// Token: 0x060090B1 RID: 37041 RVA: 0x0034CAC4 File Offset: 0x0034ACC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			SteamTurbine.InitializeStatusItems();
			default_state = this.operational;
			this.root.Update("UpdateBlocked", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateBlocked(dt);
			}, UpdateRate.SIM_200ms, false);
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.active, (SteamTurbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null);
			this.operational.DefaultState(this.operational.active).EventTransition(GameHashes.OperationalChanged, this.inoperational, (SteamTurbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.active.Update("UpdateActive", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SteamTurbine.Instance smi) => SteamTurbine.activeStatusItem, (SteamTurbine.Instance smi) => smi.master).Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.tooHot.Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.TOOHOT_ANIMS, KAnim.PlayMode.Loop);
			});
		}

		// Token: 0x04006E78 RID: 28280
		public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State inoperational;

		// Token: 0x04006E79 RID: 28281
		public SteamTurbine.States.OperationalStates operational;

		// Token: 0x04006E7A RID: 28282
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x04006E7B RID: 28283
		private static readonly HashedString[] TOOHOT_ANIMS = new HashedString[]
		{
			"working_pre"
		};

		// Token: 0x0200253B RID: 9531
		public class OperationalStates : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State
		{
			// Token: 0x0400A5CF RID: 42447
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State idle;

			// Token: 0x0400A5D0 RID: 42448
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State active;

			// Token: 0x0400A5D1 RID: 42449
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State tooHot;
		}
	}

	// Token: 0x0200160E RID: 5646
	public class Instance : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.GameInstance
	{
		// Token: 0x060090B4 RID: 37044 RVA: 0x0034CD58 File Offset: 0x0034AF58
		public Instance(SteamTurbine master) : base(master)
		{
		}

		// Token: 0x060090B5 RID: 37045 RVA: 0x0034CDB0 File Offset: 0x0034AFB0
		public void UpdateBlocked(float dt)
		{
			base.master.BlockedInputs = 0;
			for (int i = 0; i < base.master.TotalInputs; i++)
			{
				int num = base.master.srcCells[i];
				Element element = Grid.Element[num];
				if (element.IsLiquid || element.IsSolid)
				{
					SteamTurbine master = base.master;
					int blockedInputs = master.BlockedInputs;
					master.BlockedInputs = blockedInputs + 1;
				}
			}
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputBlockedStatusItem, base.master.BlockedInputs == base.master.TotalInputs, this.inputBlockedHandle, component);
			this.inputPartiallyBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputPartiallyBlockedStatusItem, base.master.BlockedInputs > 0 && base.master.BlockedInputs < base.master.TotalInputs, this.inputPartiallyBlockedHandle, component);
		}

		// Token: 0x060090B6 RID: 37046 RVA: 0x0034CE94 File Offset: 0x0034B094
		public void UpdateState(float dt)
		{
			bool flag = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature);
			bool flag2 = this.IsTooHot(ref this.buildingTooHot);
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (flag2)
			{
				if (currentState != base.sm.operational.tooHot)
				{
					base.smi.GoTo(base.sm.operational.tooHot);
					return;
				}
			}
			else if (flag)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x060090B7 RID: 37047 RVA: 0x0034CF63 File Offset: 0x0034B163
		private bool IsTooHot(ref bool building_too_hot)
		{
			building_too_hot = (base.gameObject.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxBuildingTemperature);
			return building_too_hot;
		}

		// Token: 0x060090B8 RID: 37048 RVA: 0x0034CF8C File Offset: 0x0034B18C
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num3 = base.master.srcCells[i];
				float b = Grid.Mass[num3];
				if (Grid.Element[num3].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
					float b2 = Grid.Temperature[num3];
					num2 = Mathf.Max(num2, b2);
				}
			}
			insufficient_mass = (num < base.master.requiredMass);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x060090B9 RID: 37049 RVA: 0x0034D03C File Offset: 0x0034B23C
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.insufficientMassHandle = this.UpdateStatusItem(SteamTurbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(SteamTurbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
			this.buildingTooHotHandle = this.UpdateStatusItem(SteamTurbine.buildingTooHotItem, this.buildingTooHot, this.buildingTooHotHandle, component);
			StatusItem status_item = base.master.operational.IsActive ? SteamTurbine.activeWattageStatusItem : Db.Get().BuildingStatusItems.GeneratorOffline;
			this.activeWattageHandle = component.SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, base.master);
		}

		// Token: 0x060090BA RID: 37050 RVA: 0x0034D0F8 File Offset: 0x0034B2F8
		private Guid UpdateStatusItem(StatusItem item, bool show, Guid current_handle, KSelectable ksel)
		{
			Guid result = current_handle;
			if (show != (current_handle != Guid.Empty))
			{
				if (show)
				{
					result = ksel.AddStatusItem(item, base.master);
				}
				else
				{
					result = ksel.RemoveStatusItem(current_handle, false);
				}
			}
			return result;
		}

		// Token: 0x060090BB RID: 37051 RVA: 0x0034D134 File Offset: 0x0034B334
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.buildingTooHotHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
			component.RemoveStatusItem(this.activeWattageHandle, false);
		}

		// Token: 0x04006E7C RID: 28284
		public bool insufficientMass;

		// Token: 0x04006E7D RID: 28285
		public bool insufficientTemperature;

		// Token: 0x04006E7E RID: 28286
		public bool buildingTooHot;

		// Token: 0x04006E7F RID: 28287
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x04006E80 RID: 28288
		private Guid inputPartiallyBlockedHandle = Guid.Empty;

		// Token: 0x04006E81 RID: 28289
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x04006E82 RID: 28290
		private Guid insufficientTemperatureHandle = Guid.Empty;

		// Token: 0x04006E83 RID: 28291
		private Guid buildingTooHotHandle = Guid.Empty;

		// Token: 0x04006E84 RID: 28292
		private Guid activeWattageHandle = Guid.Empty;
	}
}
