using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimTemperatureTransfer")]
public class SimTemperatureTransfer : KMonoBehaviour
{
	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06004FD1 RID: 20433 RVA: 0x001CB5E3 File Offset: 0x001C97E3
	// (set) Token: 0x06004FD2 RID: 20434 RVA: 0x001CB5EB File Offset: 0x001C97EB
	public float SurfaceArea
	{
		get
		{
			return this.surfaceArea;
		}
		set
		{
			this.surfaceArea = value;
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06004FD3 RID: 20435 RVA: 0x001CB5F4 File Offset: 0x001C97F4
	// (set) Token: 0x06004FD4 RID: 20436 RVA: 0x001CB5FC File Offset: 0x001C97FC
	public float Thickness
	{
		get
		{
			return this.thickness;
		}
		set
		{
			this.thickness = value;
		}
	}

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x001CB605 File Offset: 0x001C9805
	// (set) Token: 0x06004FD6 RID: 20438 RVA: 0x001CB60D File Offset: 0x001C980D
	public float GroundTransferScale
	{
		get
		{
			return this.groundTransferScale;
		}
		set
		{
			this.groundTransferScale = value;
		}
	}

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x001CB616 File Offset: 0x001C9816
	public int SimHandle
	{
		get
		{
			return this.simHandle;
		}
	}

	// Token: 0x06004FD8 RID: 20440 RVA: 0x001CB61E File Offset: 0x001C981E
	public static void ClearInstanceMap()
	{
		SimTemperatureTransfer.handleInstanceMap.Clear();
	}

	// Token: 0x06004FD9 RID: 20441 RVA: 0x001CB62C File Offset: 0x001C982C
	public static void DoOreMeltTransition(int sim_handle)
	{
		SimTemperatureTransfer simTemperatureTransfer = null;
		if (!SimTemperatureTransfer.handleInstanceMap.TryGetValue(sim_handle, out simTemperatureTransfer))
		{
			return;
		}
		if (simTemperatureTransfer == null)
		{
			return;
		}
		if (simTemperatureTransfer.HasTag(GameTags.Sealed))
		{
			return;
		}
		PrimaryElement primaryElement = simTemperatureTransfer.pe;
		Element element = primaryElement.Element;
		bool flag = primaryElement.Temperature >= element.highTemp;
		bool flag2 = primaryElement.Temperature <= element.lowTemp;
		if (!flag && !flag2)
		{
			return;
		}
		if (flag && element.highTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (flag2 && element.lowTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (primaryElement.Mass > 0f)
		{
			int gameCell = Grid.PosToCell(simTemperatureTransfer.transform.GetPosition());
			float num = primaryElement.Mass;
			int num2 = primaryElement.DiseaseCount;
			SimHashes new_element = flag ? element.highTempTransitionTarget : element.lowTempTransitionTarget;
			SimHashes simHashes = flag ? element.highTempTransitionOreID : element.lowTempTransitionOreID;
			float num3 = flag ? element.highTempTransitionOreMassConversion : element.lowTempTransitionOreMassConversion;
			if (simHashes != (SimHashes)0)
			{
				float num4 = num * num3;
				int num5 = (int)((float)num2 * num3);
				if (num4 > 0.001f)
				{
					num -= num4;
					num2 -= num5;
					Element element2 = ElementLoader.FindElementByHash(simHashes);
					if (element2.IsSolid)
					{
						GameObject obj = element2.substance.SpawnResource(simTemperatureTransfer.transform.GetPosition(), num4, primaryElement.Temperature, primaryElement.DiseaseIdx, num5, true, false, true);
						element2.substance.ActivateSubstanceGameObject(obj, primaryElement.DiseaseIdx, num5);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, element2.id, CellEventLogger.Instance.OreMelted, num4, primaryElement.Temperature, primaryElement.DiseaseIdx, num5, true, -1);
					}
				}
			}
			SimMessages.AddRemoveSubstance(gameCell, new_element, CellEventLogger.Instance.OreMelted, num, primaryElement.Temperature, primaryElement.DiseaseIdx, num2, true, -1);
		}
		simTemperatureTransfer.OnCleanUp();
		Util.KDestroyGameObject(simTemperatureTransfer.gameObject);
	}

	// Token: 0x06004FDA RID: 20442 RVA: 0x001CB814 File Offset: 0x001C9A14
	protected override void OnPrefabInit()
	{
		this.pe.sttOptimizationHook = this;
		this.pe.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(SimTemperatureTransfer.OnGetTemperature);
		this.pe.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(SimTemperatureTransfer.OnSetTemperature);
		PrimaryElement primaryElement = this.pe;
		primaryElement.onDataChanged = (Action<PrimaryElement>)Delegate.Combine(primaryElement.onDataChanged, new Action<PrimaryElement>(this.OnDataChanged));
	}

	// Token: 0x06004FDB RID: 20443 RVA: 0x001CB884 File Offset: 0x001C9A84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Element element = this.pe.Element;
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "SimTemperatureTransfer.OnSpawn");
		if (!Grid.IsValidCell(Grid.PosToCell(this)) || this.pe.Element.HasTag(GameTags.Special) || element.specificHeatCapacity == 0f)
		{
			base.enabled = false;
		}
		this.SimRegister();
	}

	// Token: 0x06004FDC RID: 20444 RVA: 0x001CB903 File Offset: 0x001C9B03
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimTemperatureTransfer.OnSetTemperature(this.pe, this.pe.Temperature);
		}
	}

	// Token: 0x06004FDD RID: 20445 RVA: 0x001CB934 File Offset: 0x001C9B34
	protected override void OnCmpDisable()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			float temperature = this.pe.Temperature;
			this.pe.InternalTemperature = this.pe.Temperature;
			SimMessages.SetElementChunkData(this.simHandle, temperature, 0f);
		}
		base.OnCmpDisable();
	}

	// Token: 0x06004FDE RID: 20446 RVA: 0x001CB988 File Offset: 0x001C9B88
	private void OnCellChanged()
	{
		int cell = Grid.PosToCell(this);
		if (!Grid.IsValidCell(cell))
		{
			base.enabled = false;
			return;
		}
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimMessages.MoveElementChunk(this.simHandle, cell);
			return;
		}
		this.forceDataSyncOnRegister = true;
	}

	// Token: 0x06004FDF RID: 20447 RVA: 0x001CB9D3 File Offset: 0x001C9BD3
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
		this.SimUnregister();
		base.OnForcedCleanUp();
	}

	// Token: 0x06004FE0 RID: 20448 RVA: 0x001CBA00 File Offset: 0x001C9C00
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimTemperatureTransfer sttOptimizationHook = primary_element.sttOptimizationHook;
		float result;
		if (Sim.IsValidHandle(sttOptimizationHook.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(sttOptimizationHook.simHandle);
			result = Game.Instance.simData.elementChunks[handleIndex].temperature;
			sttOptimizationHook.deltaKJ = Game.Instance.simData.elementChunks[handleIndex].deltaKJ;
		}
		else
		{
			result = primary_element.InternalTemperature;
		}
		return result;
	}

	// Token: 0x06004FE1 RID: 20449 RVA: 0x001CBA7C File Offset: 0x001C9C7C
	private unsafe static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "STT.OnSetTemperature - Tried to set <= 0 degree temperature", null);
			temperature = 293f;
		}
		primary_element.InternalTemperature = temperature;
		SimTemperatureTransfer sttOptimizationHook = primary_element.sttOptimizationHook;
		if (Sim.IsValidHandle(sttOptimizationHook.simHandle))
		{
			float mass = primary_element.Mass;
			float heat_capacity = (mass >= 0.01f) ? (mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(sttOptimizationHook.simHandle, temperature, heat_capacity);
			int handleIndex = Sim.GetHandleIndex(sttOptimizationHook.simHandle);
			Game.Instance.simData.elementChunks[handleIndex].temperature = temperature;
		}
	}

	// Token: 0x06004FE2 RID: 20450 RVA: 0x001CBB1C File Offset: 0x001C9D1C
	private void OnDataChanged(PrimaryElement primary_element)
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			float heat_capacity = (primary_element.Mass >= 0.01f) ? (primary_element.Mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(this.simHandle, primary_element.Temperature, heat_capacity);
			return;
		}
		this.forceDataSyncOnRegister = true;
	}

	// Token: 0x06004FE3 RID: 20451 RVA: 0x001CBB78 File Offset: 0x001C9D78
	protected void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1 && base.enabled && this.pe.Mass > 0f && !this.pe.Element.IsTemperatureInsulated)
		{
			int gameCell = Grid.PosToCell(base.transform.GetPosition());
			this.simHandle = -2;
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(SimTemperatureTransfer.OnSimRegisteredCallback), this, "SimTemperatureTransfer.SimRegister");
			float num = this.pe.InternalTemperature;
			if (num <= 0f)
			{
				this.pe.InternalTemperature = 293f;
				num = 293f;
			}
			this.forceDataSyncOnRegister = false;
			SimMessages.AddElementChunk(gameCell, this.pe.ElementID, this.pe.Mass, num, this.surfaceArea, this.thickness, this.groundTransferScale, handle.index);
		}
	}

	// Token: 0x06004FE4 RID: 20452 RVA: 0x001CBC74 File Offset: 0x001C9E74
	protected unsafe void SimUnregister()
	{
		if (this.simHandle != -1 && !KMonoBehaviour.isLoadingScene)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				int handleIndex = Sim.GetHandleIndex(this.simHandle);
				this.pe.InternalTemperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
				SimMessages.RemoveElementChunk(this.simHandle, -1);
				SimTemperatureTransfer.handleInstanceMap.Remove(this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x06004FE5 RID: 20453 RVA: 0x001CBCF7 File Offset: 0x001C9EF7
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((SimTemperatureTransfer)data).OnSimRegistered(handle);
	}

	// Token: 0x06004FE6 RID: 20454 RVA: 0x001CBD08 File Offset: 0x001C9F08
	private unsafe void OnSimRegistered(int handle)
	{
		if (this != null && this.simHandle == -2)
		{
			this.simHandle = handle;
			int handleIndex = Sim.GetHandleIndex(handle);
			float temperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
			float internalTemperature = this.pe.InternalTemperature;
			if (temperature <= 0f)
			{
				KCrashReporter.Assert(false, "Bad temperature", null);
			}
			SimTemperatureTransfer.handleInstanceMap[this.simHandle] = this;
			if (this.forceDataSyncOnRegister || Mathf.Abs(temperature - internalTemperature) > 0.1f)
			{
				float heat_capacity = (this.pe.Mass >= 0.01f) ? (this.pe.Mass * this.pe.Element.specificHeatCapacity) : 0f;
				SimMessages.SetElementChunkData(this.simHandle, internalTemperature, heat_capacity);
				SimMessages.MoveElementChunk(this.simHandle, Grid.PosToCell(this));
				Game.Instance.simData.elementChunks[handleIndex].temperature = internalTemperature;
			}
			if (this.onSimRegistered != null)
			{
				this.onSimRegistered(this);
			}
			if (!base.enabled)
			{
				this.OnCmpDisable();
				return;
			}
		}
		else
		{
			SimMessages.RemoveElementChunk(handle, -1);
		}
	}

	// Token: 0x0400350B RID: 13579
	[MyCmpReq]
	public PrimaryElement pe;

	// Token: 0x0400350C RID: 13580
	private const float SIM_FREEZE_SPAWN_ORE_PERCENT = 0.8f;

	// Token: 0x0400350D RID: 13581
	public const float MIN_MASS_FOR_TEMPERATURE_TRANSFER = 0.01f;

	// Token: 0x0400350E RID: 13582
	public float deltaKJ;

	// Token: 0x0400350F RID: 13583
	public Action<SimTemperatureTransfer> onSimRegistered;

	// Token: 0x04003510 RID: 13584
	protected int simHandle = -1;

	// Token: 0x04003511 RID: 13585
	protected bool forceDataSyncOnRegister;

	// Token: 0x04003512 RID: 13586
	[SerializeField]
	protected float surfaceArea = 10f;

	// Token: 0x04003513 RID: 13587
	[SerializeField]
	protected float thickness = 0.01f;

	// Token: 0x04003514 RID: 13588
	[SerializeField]
	protected float groundTransferScale = 0.0625f;

	// Token: 0x04003515 RID: 13589
	private static Dictionary<int, SimTemperatureTransfer> handleInstanceMap = new Dictionary<int, SimTemperatureTransfer>();
}
