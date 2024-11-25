using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B23 RID: 2851
public class StructureTemperatureComponents : KGameObjectSplitComponentManager<StructureTemperatureHeader, StructureTemperaturePayload>
{
	// Token: 0x060054E1 RID: 21729 RVA: 0x001E50CC File Offset: 0x001E32CC
	public HandleVector<int>.Handle Add(GameObject go)
	{
		StructureTemperaturePayload structureTemperaturePayload = new StructureTemperaturePayload(go);
		return base.Add(go, new StructureTemperatureHeader
		{
			dirty = false,
			simHandle = -1,
			isActiveBuilding = false
		}, ref structureTemperaturePayload);
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x001E510B File Offset: 0x001E330B
	public static void ClearInstanceMap()
	{
		StructureTemperatureComponents.handleInstanceMap.Clear();
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x001E5120 File Offset: 0x001E3320
	protected override void OnPrefabInit(HandleVector<int>.Handle handle)
	{
		this.InitializeStatusItem();
		base.OnPrefabInit(handle);
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(StructureTemperatureComponents.OnGetTemperature);
		structureTemperaturePayload.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(StructureTemperatureComponents.OnSetTemperature);
		new_data.isActiveBuilding = (structureTemperaturePayload.building.Def.SelfHeatKilowattsWhenActive != 0f || structureTemperaturePayload.ExhaustKilowatts != 0f);
		base.SetHeader(handle, new_data);
	}

	// Token: 0x060054E5 RID: 21733 RVA: 0x001E51B0 File Offset: 0x001E33B0
	private void InitializeStatusItem()
	{
		if (this.operatingEnergyStatusItem != null)
		{
			return;
		}
		this.operatingEnergyStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.operatingEnergyStatusItem.resolveStringCallback = delegate(string str, object ev_data)
		{
			int key = (int)ev_data;
			HandleVector<int>.Handle handle = StructureTemperatureComponents.handleInstanceMap[key];
			StructureTemperaturePayload payload = base.GetPayload(handle);
			if (str != BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP)
			{
				try
				{
					return string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				}
				catch (Exception obj)
				{
					global::Debug.LogWarning(obj);
					global::Debug.LogWarning(BUILDING.STATUSITEMS.OPERATINGENERGY.TOOLTIP);
					global::Debug.LogWarning(str);
					return str;
				}
			}
			string text = "";
			foreach (StructureTemperaturePayload.EnergySource energySource in payload.energySourcesKW)
			{
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, energySource.source, GameUtil.GetFormattedHeatEnergy(energySource.value * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
			}
			str = string.Format(str, GameUtil.GetFormattedHeatEnergy(payload.TotalEnergyProducedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S), text);
			return str;
		};
	}

	// Token: 0x060054E6 RID: 21734 RVA: 0x001E5208 File Offset: 0x001E3408
	protected override void OnSpawn(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		if (structureTemperaturePayload.operational != null && structureTemperatureHeader.isActiveBuilding)
		{
			structureTemperaturePayload.primaryElement.Subscribe(824508782, delegate(object ev_data)
			{
				StructureTemperatureComponents.OnActiveChanged(handle);
			});
		}
		structureTemperaturePayload.maxTemperature = ((structureTemperaturePayload.overheatable != null) ? structureTemperaturePayload.overheatable.OverheatTemperature : 10000f);
		if (structureTemperaturePayload.maxTemperature <= 0f)
		{
			global::Debug.LogError("invalid max temperature");
		}
		base.SetPayload(handle, ref structureTemperaturePayload);
		this.SimRegister(handle, ref structureTemperatureHeader, ref structureTemperaturePayload);
	}

	// Token: 0x060054E7 RID: 21735 RVA: 0x001E52C4 File Offset: 0x001E34C4
	private static void OnActiveChanged(HandleVector<int>.Handle handle)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = structureTemperaturePayload.Temperature;
		new_data.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, new_data);
	}

	// Token: 0x060054E8 RID: 21736 RVA: 0x001E5307 File Offset: 0x001E3507
	protected override void OnCleanUp(HandleVector<int>.Handle handle)
	{
		this.SimUnregister(handle);
		base.OnCleanUp(handle);
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x001E5318 File Offset: 0x001E3518
	public override void Sim200ms(float dt)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		List<StructureTemperatureHeader> list;
		List<StructureTemperaturePayload> list2;
		base.GetDataLists(out list, out list2);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList.Capacity = Math.Max(pooledList.Capacity, list.Count);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList2 = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList2.Capacity = Math.Max(pooledList2.Capacity, list.Count);
		ListPool<int, StructureTemperatureComponents>.PooledList pooledList3 = ListPool<int, StructureTemperatureComponents>.Allocate();
		pooledList3.Capacity = Math.Max(pooledList3.Capacity, list.Count);
		for (int num4 = 0; num4 != list.Count; num4++)
		{
			StructureTemperatureHeader structureTemperatureHeader = list[num4];
			if (Sim.IsValidHandle(structureTemperatureHeader.simHandle))
			{
				pooledList.Add(num4);
				if (structureTemperatureHeader.dirty)
				{
					pooledList2.Add(num4);
					structureTemperatureHeader.dirty = false;
					list[num4] = structureTemperatureHeader;
				}
				if (structureTemperatureHeader.isActiveBuilding)
				{
					pooledList3.Add(num4);
				}
			}
		}
		foreach (int index in pooledList2)
		{
			StructureTemperaturePayload structureTemperaturePayload = list2[index];
			StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
		}
		foreach (int index2 in pooledList2)
		{
			if (list2[index2].pendingEnergyModifications != 0f)
			{
				StructureTemperaturePayload structureTemperaturePayload2 = list2[index2];
				SimMessages.ModifyBuildingEnergy(structureTemperaturePayload2.simHandleCopy, structureTemperaturePayload2.pendingEnergyModifications, 0f, 10000f);
				structureTemperaturePayload2.pendingEnergyModifications = 0f;
				list2[index2] = structureTemperaturePayload2;
			}
		}
		foreach (int index3 in pooledList3)
		{
			StructureTemperaturePayload structureTemperaturePayload3 = list2[index3];
			if (structureTemperaturePayload3.operational == null || structureTemperaturePayload3.operational.IsActive)
			{
				num++;
				if (!structureTemperaturePayload3.isActiveStatusItemSet)
				{
					num3++;
					structureTemperaturePayload3.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, this.operatingEnergyStatusItem, structureTemperaturePayload3.simHandleCopy);
					structureTemperaturePayload3.isActiveStatusItemSet = true;
				}
				structureTemperaturePayload3.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload3.energySourcesKW, structureTemperaturePayload3.OperatingKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING);
				if (structureTemperaturePayload3.ExhaustKilowatts != 0f)
				{
					num2++;
					StructureTemperatureComponents.ExhaustHeat(structureTemperaturePayload3.GetExtents(), structureTemperaturePayload3.ExhaustKilowatts, structureTemperaturePayload3.maxTemperature, dt);
					structureTemperaturePayload3.energySourcesKW = this.AccumulateProducedEnergyKW(structureTemperaturePayload3.energySourcesKW, structureTemperaturePayload3.ExhaustKilowatts, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING);
				}
			}
			else if (structureTemperaturePayload3.isActiveStatusItemSet)
			{
				num3++;
				structureTemperaturePayload3.primaryElement.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.OperatingEnergy, null, null);
				structureTemperaturePayload3.isActiveStatusItemSet = false;
			}
			list2[index3] = structureTemperaturePayload3;
		}
		pooledList3.Recycle();
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x001E5688 File Offset: 0x001E3888
	public static void ExhaustHeat(Extents extents, float kw, float maxTemperature, float dt)
	{
		int num = extents.width * extents.height;
		float num2 = kw * dt / (float)num;
		for (int i = 0; i < extents.height; i++)
		{
			int num3 = extents.y + i;
			for (int j = 0; j < extents.width; j++)
			{
				int num4 = extents.x + j;
				int num5 = num3 * Grid.WidthInCells + num4;
				float num6 = Mathf.Min(Grid.Mass[num5], 1.5f) / 1.5f;
				float kilojoules = num2 * num6;
				SimMessages.ModifyEnergy(num5, kilojoules, maxTemperature, SimMessages.EnergySourceID.StructureTemperature);
			}
		}
	}

	// Token: 0x060054EB RID: 21739 RVA: 0x001E5724 File Offset: 0x001E3924
	private static void UpdateSimState(ref StructureTemperaturePayload payload)
	{
		DebugUtil.Assert(Sim.IsValidHandle(payload.simHandleCopy));
		float internalTemperature = payload.primaryElement.InternalTemperature;
		BuildingDef def = payload.building.Def;
		float mass = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		float overheat_temperature = (payload.overheatable != null) ? payload.overheatable.OverheatTemperature : 10000f;
		if (!payload.enabled || payload.bypass)
		{
			mass = 0f;
		}
		Extents extents = payload.GetExtents();
		ushort idx = payload.primaryElement.Element.idx;
		if (payload.heatEffect != null)
		{
			float num = (payload.operational == null || payload.operational.IsActive) ? payload.ExhaustKilowatts : 0f;
			payload.heatEffect.SetHeatBeingProducedValue(Mathf.Clamp(operatingKilowatts + num, 0f, float.MaxValue));
		}
		SimMessages.ModifyBuildingHeatExchange(payload.simHandleCopy, extents, mass, internalTemperature, def.ThermalConductivity, overheat_temperature, operatingKilowatts, idx);
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x001E582C File Offset: 0x001E3A2C
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
		float result;
		if (Sim.IsValidHandle(payload.simHandleCopy) && payload.enabled)
		{
			if (!payload.bypass)
			{
				int handleIndex = Sim.GetHandleIndex(payload.simHandleCopy);
				result = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
			}
			else
			{
				int i = Grid.PosToCell(payload.primaryElement.transform.GetPosition());
				result = Grid.Temperature[i];
			}
		}
		else
		{
			result = payload.primaryElement.InternalTemperature;
		}
		return result;
	}

	// Token: 0x060054ED RID: 21741 RVA: 0x001E58D8 File Offset: 0x001E3AD8
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(primary_element.gameObject);
		StructureTemperatureHeader structureTemperatureHeader;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out structureTemperatureHeader, out structureTemperaturePayload);
		structureTemperaturePayload.primaryElement.InternalTemperature = temperature;
		structureTemperatureHeader.dirty = true;
		GameComps.StructureTemperatures.SetHeader(handle, structureTemperatureHeader);
		if (!structureTemperatureHeader.isActiveBuilding && Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
		{
			StructureTemperatureComponents.UpdateSimState(ref structureTemperaturePayload);
			if (structureTemperaturePayload.pendingEnergyModifications != 0f)
			{
				SimMessages.ModifyBuildingEnergy(structureTemperaturePayload.simHandleCopy, structureTemperaturePayload.pendingEnergyModifications, 0f, 10000f);
				structureTemperaturePayload.pendingEnergyModifications = 0f;
				GameComps.StructureTemperatures.SetPayload(handle, ref structureTemperaturePayload);
			}
		}
	}

	// Token: 0x060054EE RID: 21742 RVA: 0x001E5984 File Offset: 0x001E3B84
	public void ProduceEnergy(HandleVector<int>.Handle handle, float delta_kilojoules, string source, float display_dt)
	{
		StructureTemperaturePayload payload = base.GetPayload(handle);
		if (Sim.IsValidHandle(payload.simHandleCopy))
		{
			SimMessages.ModifyBuildingEnergy(payload.simHandleCopy, delta_kilojoules, 0f, 10000f);
		}
		else
		{
			payload.pendingEnergyModifications += delta_kilojoules;
			StructureTemperatureHeader header = base.GetHeader(handle);
			header.dirty = true;
			base.SetHeader(handle, header);
		}
		payload.energySourcesKW = this.AccumulateProducedEnergyKW(payload.energySourcesKW, delta_kilojoules / display_dt, source);
		base.SetPayload(handle, ref payload);
	}

	// Token: 0x060054EF RID: 21743 RVA: 0x001E5A04 File Offset: 0x001E3C04
	private List<StructureTemperaturePayload.EnergySource> AccumulateProducedEnergyKW(List<StructureTemperaturePayload.EnergySource> sources, float kw, string source)
	{
		if (sources == null)
		{
			sources = new List<StructureTemperaturePayload.EnergySource>();
		}
		bool flag = false;
		for (int i = 0; i < sources.Count; i++)
		{
			if (sources[i].source == source)
			{
				sources[i].Accumulate(kw);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			sources.Add(new StructureTemperaturePayload.EnergySource(kw, source));
		}
		return sources;
	}

	// Token: 0x060054F0 RID: 21744 RVA: 0x001E5A64 File Offset: 0x001E3C64
	public static void DoStateTransition(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			StructureTemperatureComponents.DoMelt(GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement);
		}
	}

	// Token: 0x060054F1 RID: 21745 RVA: 0x001E5AA0 File Offset: 0x001E3CA0
	public static void DoMelt(PrimaryElement primary_element)
	{
		Element element = primary_element.Element;
		if (element.highTempTransitionTarget != SimHashes.Unobtanium)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(primary_element.transform.GetPosition()), element.highTempTransitionTarget, CellEventLogger.Instance.OreMelted, primary_element.Mass, primary_element.Element.highTemp, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
			Building.CreateBuildingMeltedNotification(primary_element.gameObject);
			Util.KDestroyGameObject(primary_element.gameObject);
		}
	}

	// Token: 0x060054F2 RID: 21746 RVA: 0x001E5B1C File Offset: 0x001E3D1C
	public static void DoOverheat(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(1832602615, null);
		}
	}

	// Token: 0x060054F3 RID: 21747 RVA: 0x001E5B64 File Offset: 0x001E3D64
	public static void DoNoLongerOverheated(int sim_handle)
	{
		HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
		if (StructureTemperatureComponents.handleInstanceMap.TryGetValue(sim_handle, out invalidHandle))
		{
			GameComps.StructureTemperatures.GetPayload(invalidHandle).primaryElement.gameObject.Trigger(171119937, null);
		}
	}

	// Token: 0x060054F4 RID: 21748 RVA: 0x001E5BA9 File Offset: 0x001E3DA9
	public bool IsEnabled(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).enabled;
	}

	// Token: 0x060054F5 RID: 21749 RVA: 0x001E5BB8 File Offset: 0x001E3DB8
	private void Enable(HandleVector<int>.Handle handle, bool isEnabled)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.enabled = isEnabled;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x060054F6 RID: 21750 RVA: 0x001E5BEA File Offset: 0x001E3DEA
	public void Enable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, true);
	}

	// Token: 0x060054F7 RID: 21751 RVA: 0x001E5BF4 File Offset: 0x001E3DF4
	public void Disable(HandleVector<int>.Handle handle)
	{
		this.Enable(handle, false);
	}

	// Token: 0x060054F8 RID: 21752 RVA: 0x001E5BFE File Offset: 0x001E3DFE
	public bool IsBypassed(HandleVector<int>.Handle handle)
	{
		return base.GetPayload(handle).bypass;
	}

	// Token: 0x060054F9 RID: 21753 RVA: 0x001E5C0C File Offset: 0x001E3E0C
	private void Bypass(HandleVector<int>.Handle handle, bool bypass)
	{
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		base.GetData(handle, out new_data, out structureTemperaturePayload);
		new_data.dirty = true;
		structureTemperaturePayload.bypass = bypass;
		base.SetData(handle, new_data, ref structureTemperaturePayload);
	}

	// Token: 0x060054FA RID: 21754 RVA: 0x001E5C3E File Offset: 0x001E3E3E
	public void Bypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, true);
	}

	// Token: 0x060054FB RID: 21755 RVA: 0x001E5C48 File Offset: 0x001E3E48
	public void UnBypass(HandleVector<int>.Handle handle)
	{
		this.Bypass(handle, false);
	}

	// Token: 0x060054FC RID: 21756 RVA: 0x001E5C54 File Offset: 0x001E3E54
	protected void SimRegister(HandleVector<int>.Handle handle, ref StructureTemperatureHeader header, ref StructureTemperaturePayload payload)
	{
		if (payload.simHandleCopy != -1)
		{
			return;
		}
		PrimaryElement primaryElement = payload.primaryElement;
		if (primaryElement.Mass <= 0f)
		{
			return;
		}
		if (primaryElement.Element.IsTemperatureInsulated)
		{
			return;
		}
		payload.simHandleCopy = -2;
		string dbg_name = primaryElement.name;
		HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle2 = Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			StructureTemperatureComponents.OnSimRegistered(handle, sim_handle, dbg_name);
		}, null, "StructureTemperature.SimRegister");
		BuildingDef def = primaryElement.GetComponent<Building>().Def;
		float internalTemperature = primaryElement.InternalTemperature;
		float massForTemperatureModification = def.MassForTemperatureModification;
		float operatingKilowatts = payload.OperatingKilowatts;
		Extents extents = payload.GetExtents();
		ushort idx = primaryElement.Element.idx;
		SimMessages.AddBuildingHeatExchange(extents, massForTemperatureModification, internalTemperature, def.ThermalConductivity, operatingKilowatts, idx, handle2.index);
		header.simHandle = payload.simHandleCopy;
		base.SetData(handle, header, ref payload);
	}

	// Token: 0x060054FD RID: 21757 RVA: 0x001E5D44 File Offset: 0x001E3F44
	private static void OnSimRegistered(HandleVector<int>.Handle handle, int sim_handle, string dbg_name)
	{
		if (!GameComps.StructureTemperatures.IsValid(handle))
		{
			return;
		}
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy == -2)
		{
			StructureTemperatureComponents.handleInstanceMap[sim_handle] = handle;
			new_data.simHandle = sim_handle;
			structureTemperaturePayload.simHandleCopy = sim_handle;
			GameComps.StructureTemperatures.SetData(handle, new_data, ref structureTemperaturePayload);
			structureTemperaturePayload.primaryElement.Trigger(-1555603773, sim_handle);
			int cell = Grid.PosToCell(structureTemperaturePayload.building.transform.GetPosition());
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Created, structureTemperaturePayload.building, sim_handle));
			return;
		}
		SimMessages.RemoveBuildingHeatExchange(sim_handle, -1);
	}

	// Token: 0x060054FE RID: 21758 RVA: 0x001E5E0C File Offset: 0x001E400C
	protected unsafe void SimUnregister(HandleVector<int>.Handle handle)
	{
		if (!GameComps.StructureTemperatures.IsVersionValid(handle))
		{
			KCrashReporter.Assert(false, "Handle version mismatch in StructureTemperature.SimUnregister", null);
			return;
		}
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		StructureTemperatureHeader new_data;
		StructureTemperaturePayload structureTemperaturePayload;
		GameComps.StructureTemperatures.GetData(handle, out new_data, out structureTemperaturePayload);
		if (structureTemperaturePayload.simHandleCopy != -1)
		{
			int cell = Grid.PosToCell(structureTemperaturePayload.building);
			GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.contactConductiveLayer, new StructureToStructureTemperature.BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType.Destroyed, structureTemperaturePayload.building, structureTemperaturePayload.simHandleCopy));
			if (Sim.IsValidHandle(structureTemperaturePayload.simHandleCopy))
			{
				int handleIndex = Sim.GetHandleIndex(structureTemperaturePayload.simHandleCopy);
				structureTemperaturePayload.primaryElement.InternalTemperature = Game.Instance.simData.buildingTemperatures[handleIndex].temperature;
				SimMessages.RemoveBuildingHeatExchange(structureTemperaturePayload.simHandleCopy, -1);
				StructureTemperatureComponents.handleInstanceMap.Remove(structureTemperaturePayload.simHandleCopy);
			}
			structureTemperaturePayload.simHandleCopy = -1;
			new_data.simHandle = -1;
			base.SetData(handle, new_data, ref structureTemperaturePayload);
		}
	}

	// Token: 0x040037A9 RID: 14249
	private const float MAX_PRESSURE = 1.5f;

	// Token: 0x040037AA RID: 14250
	private static Dictionary<int, HandleVector<int>.Handle> handleInstanceMap = new Dictionary<int, HandleVector<int>.Handle>();

	// Token: 0x040037AB RID: 14251
	private StatusItem operatingEnergyStatusItem;
}
