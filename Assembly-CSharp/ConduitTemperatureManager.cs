using System;
using System.Runtime.InteropServices;

// Token: 0x020007E1 RID: 2017
public class ConduitTemperatureManager
{
	// Token: 0x060037BC RID: 14268 RVA: 0x001303D6 File Offset: 0x0012E5D6
	public ConduitTemperatureManager()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Initialize();
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x001303FB File Offset: 0x0012E5FB
	public void Shutdown()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Shutdown();
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x00130404 File Offset: 0x0012E604
	public HandleVector<int>.Handle Allocate(ConduitType conduit_type, int conduit_idx, HandleVector<int>.Handle conduit_structure_temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(conduit_structure_temperature_handle);
		Element element = payload.primaryElement.Element;
		BuildingDef def = payload.building.Def;
		float conduit_heat_capacity = def.MassForTemperatureModification * element.specificHeatCapacity;
		float conduit_thermal_conductivity = element.thermalConductivity * def.ThermalConductivity;
		int num = ConduitTemperatureManager.ConduitTemperatureManager_Add(contents.temperature, contents.mass, (int)contents.element, payload.simHandleCopy, conduit_heat_capacity, conduit_thermal_conductivity, def.ThermalConductivity < 1f);
		HandleVector<int>.Handle result = default(HandleVector<int>.Handle);
		result.index = num;
		int handleIndex = Sim.GetHandleIndex(num);
		if (handleIndex + 1 > this.temperatures.Length)
		{
			Array.Resize<float>(ref this.temperatures, (handleIndex + 1) * 2);
			Array.Resize<ConduitTemperatureManager.ConduitInfo>(ref this.conduitInfo, (handleIndex + 1) * 2);
		}
		this.temperatures[handleIndex] = contents.temperature;
		this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
		{
			type = conduit_type,
			idx = conduit_idx
		};
		return result;
	}

	// Token: 0x060037BF RID: 14271 RVA: 0x00130508 File Offset: 0x0012E708
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.temperatures[Sim.GetHandleIndex(handle.index)] = contents.temperature;
		ConduitTemperatureManager.ConduitTemperatureManager_Set(handle.index, contents.temperature, contents.mass, (int)contents.element);
	}

	// Token: 0x060037C0 RID: 14272 RVA: 0x00130558 File Offset: 0x0012E758
	public void Free(HandleVector<int>.Handle handle)
	{
		if (handle.IsValid())
		{
			int handleIndex = Sim.GetHandleIndex(handle.index);
			this.temperatures[handleIndex] = -1f;
			this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
			{
				type = ConduitType.None,
				idx = -1
			};
			ConduitTemperatureManager.ConduitTemperatureManager_Remove(handle.index);
		}
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x001305B9 File Offset: 0x0012E7B9
	public void Clear()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Clear();
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x001305C0 File Offset: 0x0012E7C0
	public unsafe void Sim200ms(float dt)
	{
		ConduitTemperatureManager.ConduitTemperatureUpdateData* ptr = (ConduitTemperatureManager.ConduitTemperatureUpdateData*)((void*)ConduitTemperatureManager.ConduitTemperatureManager_Update(dt, (IntPtr)((void*)Game.Instance.simData.buildingTemperatures)));
		int numEntries = ptr->numEntries;
		if (numEntries > 0)
		{
			Marshal.Copy((IntPtr)((void*)ptr->temperatures), this.temperatures, 0, numEntries);
		}
		for (int i = 0; i < ptr->numFrozenHandles; i++)
		{
			int handleIndex = Sim.GetHandleIndex(ptr->frozenHandles[i]);
			ConduitTemperatureManager.ConduitInfo conduitInfo = this.conduitInfo[handleIndex];
			Conduit.GetFlowManager(conduitInfo.type).FreezeConduitContents(conduitInfo.idx);
		}
		for (int j = 0; j < ptr->numMeltedHandles; j++)
		{
			int handleIndex2 = Sim.GetHandleIndex(ptr->meltedHandles[j]);
			ConduitTemperatureManager.ConduitInfo conduitInfo2 = this.conduitInfo[handleIndex2];
			Conduit.GetFlowManager(conduitInfo2.type).MeltConduitContents(conduitInfo2.idx);
		}
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x001306A9 File Offset: 0x0012E8A9
	public float GetTemperature(HandleVector<int>.Handle handle)
	{
		return this.temperatures[Sim.GetHandleIndex(handle.index)];
	}

	// Token: 0x060037C4 RID: 14276
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Initialize();

	// Token: 0x060037C5 RID: 14277
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Shutdown();

	// Token: 0x060037C6 RID: 14278
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Add(float contents_temperature, float contents_mass, int contents_element_hash, int conduit_structure_temperature_handle, float conduit_heat_capacity, float conduit_thermal_conductivity, bool conduit_insulated);

	// Token: 0x060037C7 RID: 14279
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Set(int handle, float contents_temperature, float contents_mass, int contents_element_hash);

	// Token: 0x060037C8 RID: 14280
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Remove(int handle);

	// Token: 0x060037C9 RID: 14281
	[DllImport("SimDLL")]
	private static extern IntPtr ConduitTemperatureManager_Update(float dt, IntPtr building_conductivity_data);

	// Token: 0x060037CA RID: 14282
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Clear();

	// Token: 0x04002199 RID: 8601
	private float[] temperatures = new float[0];

	// Token: 0x0400219A RID: 8602
	private ConduitTemperatureManager.ConduitInfo[] conduitInfo = new ConduitTemperatureManager.ConduitInfo[0];

	// Token: 0x020016AF RID: 5807
	private struct ConduitInfo
	{
		// Token: 0x04007089 RID: 28809
		public ConduitType type;

		// Token: 0x0400708A RID: 28810
		public int idx;
	}

	// Token: 0x020016B0 RID: 5808
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConduitTemperatureUpdateData
	{
		// Token: 0x0400708B RID: 28811
		public int numEntries;

		// Token: 0x0400708C RID: 28812
		public unsafe float* temperatures;

		// Token: 0x0400708D RID: 28813
		public int numFrozenHandles;

		// Token: 0x0400708E RID: 28814
		public unsafe int* frozenHandles;

		// Token: 0x0400708F RID: 28815
		public int numMeltedHandles;

		// Token: 0x04007090 RID: 28816
		public unsafe int* meltedHandles;
	}
}
