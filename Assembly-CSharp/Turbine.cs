using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02000790 RID: 1936
[AddComponentMenu("KMonoBehaviour/scripts/Turbine")]
public class Turbine : KMonoBehaviour
{
	// Token: 0x060034E6 RID: 13542 RVA: 0x00120464 File Offset: 0x0011E664
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(Turbine.OnSimEmittedCallback), this, "TurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		this.destCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -1));
			this.destCells[i] = Grid.OffsetCell(cell, new CellOffset(x, def.HeightInCells - 1));
		}
		this.smi = new Turbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x060034E7 RID: 13543 RVA: 0x00120540 File Offset: 0x0011E740
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
		this.smi.UpdateMeter();
	}

	// Token: 0x060034E8 RID: 13544 RVA: 0x0012059C File Offset: 0x0011E79C
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "Turbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x060034E9 RID: 13545 RVA: 0x001205F0 File Offset: 0x0011E7F0
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(Turbine.OnSimConsumeCallback), this, "TurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x060034EA RID: 13546 RVA: 0x0012065E File Offset: 0x0011E85E
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((Turbine)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x0012066C File Offset: 0x0011E86C
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minEmitMass && this.simEmitCBHandle.IsValid())
			{
				float mass = this.storedMass / (float)this.destCells.Length;
				int disease_count = this.diseaseCount / this.destCells.Length;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				int[] array = this.destCells;
				for (int i = 0; i < array.Length; i++)
				{
					SimMessages.EmitMass(array[i], mass_cb_info.elemIdx, mass, this.emitTemperature, this.diseaseIdx, disease_count, this.simEmitCBHandle.index);
				}
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x001207B6 File Offset: 0x0011E9B6
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((Turbine)data).OnSimEmitted(info);
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x001207C4 File Offset: 0x0011E9C4
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

	// Token: 0x060034EE RID: 13550 RVA: 0x00120888 File Offset: 0x0011EA88
	public static void InitializeStatusItems()
	{
		Turbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.outputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_OUTPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.spinningUpStatusItem = new StatusItem("TURBINE_SPINNING_UP", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = string.Format(str, (int)turbine.currentRPM);
			return str;
		};
		Turbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientMassStatusItem.resolveTooltipCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = str.Replace("{MASS}", GameUtil.GetFormattedMass(turbine.requiredMassFlowDifferential, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
			return str;
		};
		Turbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(Turbine.ResolveStrings);
		Turbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(Turbine.ResolveStrings);
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x00120A04 File Offset: 0x0011EC04
	private static string ResolveStrings(string str, object data)
	{
		Turbine turbine = (Turbine)data;
		str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
		str = str.Replace("{ACTIVE_TEMPERATURE}", GameUtil.GetFormattedTemperature(turbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		return str;
	}

	// Token: 0x04001F4E RID: 8014
	public SimHashes srcElem;

	// Token: 0x04001F4F RID: 8015
	public float requiredMassFlowDifferential = 3f;

	// Token: 0x04001F50 RID: 8016
	public float activePercent = 0.75f;

	// Token: 0x04001F51 RID: 8017
	public float minEmitMass;

	// Token: 0x04001F52 RID: 8018
	public float minActiveTemperature = 400f;

	// Token: 0x04001F53 RID: 8019
	public float emitTemperature = 300f;

	// Token: 0x04001F54 RID: 8020
	public float maxRPM;

	// Token: 0x04001F55 RID: 8021
	public float rpmAcceleration;

	// Token: 0x04001F56 RID: 8022
	public float rpmDeceleration;

	// Token: 0x04001F57 RID: 8023
	public float minGenerationRPM;

	// Token: 0x04001F58 RID: 8024
	public float pumpKGRate;

	// Token: 0x04001F59 RID: 8025
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x04001F5A RID: 8026
	[Serialize]
	private float storedMass;

	// Token: 0x04001F5B RID: 8027
	[Serialize]
	private float storedTemperature;

	// Token: 0x04001F5C RID: 8028
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x04001F5D RID: 8029
	[Serialize]
	private int diseaseCount;

	// Token: 0x04001F5E RID: 8030
	[MyCmpGet]
	private Generator generator;

	// Token: 0x04001F5F RID: 8031
	[Serialize]
	private float currentRPM;

	// Token: 0x04001F60 RID: 8032
	private int[] srcCells;

	// Token: 0x04001F61 RID: 8033
	private int[] destCells;

	// Token: 0x04001F62 RID: 8034
	private Turbine.Instance smi;

	// Token: 0x04001F63 RID: 8035
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x04001F64 RID: 8036
	private static StatusItem outputBlockedStatusItem;

	// Token: 0x04001F65 RID: 8037
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x04001F66 RID: 8038
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x04001F67 RID: 8039
	private static StatusItem activeStatusItem;

	// Token: 0x04001F68 RID: 8040
	private static StatusItem spinningUpStatusItem;

	// Token: 0x04001F69 RID: 8041
	private MeterController meter;

	// Token: 0x04001F6A RID: 8042
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x02001645 RID: 5701
	public class States : GameStateMachine<Turbine.States, Turbine.Instance, Turbine>
	{
		// Token: 0x060091AB RID: 37291 RVA: 0x00350EB4 File Offset: 0x0034F0B4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Turbine.InitializeStatusItems();
			default_state = this.operational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.spinningUp, (Turbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null).Enter(delegate(Turbine.Instance smi)
			{
				smi.master.currentRPM = 0f;
				smi.UpdateMeter();
			});
			this.operational.DefaultState(this.operational.spinningUp).EventTransition(GameHashes.OperationalChanged, this.inoperational, (Turbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(Turbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(Turbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.spinningUp.ToggleStatusItem((Turbine.Instance smi) => Turbine.spinningUpStatusItem, (Turbine.Instance smi) => smi.master).QueueAnim("buildup", true, null);
			this.operational.active.Update("UpdateActive", delegate(Turbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((Turbine.Instance smi) => Turbine.activeStatusItem, (Turbine.Instance smi) => smi.master).Enter(delegate(Turbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(Turbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Turbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x04006F2A RID: 28458
		public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State inoperational;

		// Token: 0x04006F2B RID: 28459
		public Turbine.States.OperationalStates operational;

		// Token: 0x04006F2C RID: 28460
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x02002551 RID: 9553
		public class OperationalStates : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State
		{
			// Token: 0x0400A64B RID: 42571
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State idle;

			// Token: 0x0400A64C RID: 42572
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State spinningUp;

			// Token: 0x0400A64D RID: 42573
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State active;
		}
	}

	// Token: 0x02001646 RID: 5702
	public class Instance : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.GameInstance
	{
		// Token: 0x060091AE RID: 37294 RVA: 0x00351141 File Offset: 0x0034F341
		public Instance(Turbine master) : base(master)
		{
		}

		// Token: 0x060091AF RID: 37295 RVA: 0x00351178 File Offset: 0x0034F378
		public void UpdateState(float dt)
		{
			float num = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature) ? base.master.rpmAcceleration : (-base.master.rpmDeceleration);
			base.master.currentRPM = Mathf.Clamp(base.master.currentRPM + dt * num, 0f, base.master.maxRPM);
			this.UpdateMeter();
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (base.master.currentRPM >= base.master.minGenerationRPM)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
				}
				base.smi.master.generator.GenerateJoules(base.smi.master.generator.WattageRating * dt, false);
				return;
			}
			if (base.master.currentRPM > 0f)
			{
				if (currentState != base.sm.operational.spinningUp)
				{
					base.smi.GoTo(base.sm.operational.spinningUp);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x060091B0 RID: 37296 RVA: 0x003512E0 File Offset: 0x0034F4E0
		public void UpdateMeter()
		{
			if (base.master.meter != null)
			{
				float num = Mathf.Clamp01(base.master.currentRPM / base.master.maxRPM);
				base.master.meter.SetPositionPercent(num);
				base.master.meter.SetSymbolTint(Turbine.TINT_SYMBOL, (num >= base.master.activePercent) ? Color.green : Color.red);
			}
		}

		// Token: 0x060091B1 RID: 37297 RVA: 0x00351364 File Offset: 0x0034F564
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = float.PositiveInfinity;
			this.isInputBlocked = false;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num4 = base.master.srcCells[i];
				float b = Grid.Mass[num4];
				if (Grid.Element[num4].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
				}
				float b2 = Grid.Temperature[num4];
				num2 = Mathf.Max(num2, b2);
				ushort index = Grid.ElementIdx[num4];
				Element element = ElementLoader.elements[(int)index];
				if (element.IsLiquid || element.IsSolid)
				{
					this.isInputBlocked = true;
				}
			}
			this.isOutputBlocked = false;
			for (int j = 0; j < base.master.destCells.Length; j++)
			{
				int i2 = base.master.destCells[j];
				float b3 = Grid.Mass[i2];
				num3 = Mathf.Min(num3, b3);
				ushort index2 = Grid.ElementIdx[i2];
				Element element2 = ElementLoader.elements[(int)index2];
				if (element2.IsLiquid || element2.IsSolid)
				{
					this.isOutputBlocked = true;
				}
			}
			insufficient_mass = (num - num3 < base.master.requiredMassFlowDifferential);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x060091B2 RID: 37298 RVA: 0x003514E0 File Offset: 0x0034F6E0
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(Turbine.inputBlockedStatusItem, this.isInputBlocked, this.inputBlockedHandle, component);
			this.outputBlockedHandle = this.UpdateStatusItem(Turbine.outputBlockedStatusItem, this.isOutputBlocked, this.outputBlockedHandle, component);
			this.insufficientMassHandle = this.UpdateStatusItem(Turbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(Turbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
		}

		// Token: 0x060091B3 RID: 37299 RVA: 0x0035156C File Offset: 0x0034F76C
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

		// Token: 0x060091B4 RID: 37300 RVA: 0x003515A8 File Offset: 0x0034F7A8
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.inputBlockedHandle, false);
			component.RemoveStatusItem(this.outputBlockedHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
		}

		// Token: 0x04006F2D RID: 28461
		public bool isInputBlocked;

		// Token: 0x04006F2E RID: 28462
		public bool isOutputBlocked;

		// Token: 0x04006F2F RID: 28463
		public bool insufficientMass;

		// Token: 0x04006F30 RID: 28464
		public bool insufficientTemperature;

		// Token: 0x04006F31 RID: 28465
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x04006F32 RID: 28466
		private Guid outputBlockedHandle = Guid.Empty;

		// Token: 0x04006F33 RID: 28467
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x04006F34 RID: 28468
		private Guid insufficientTemperatureHandle = Guid.Empty;
	}
}
