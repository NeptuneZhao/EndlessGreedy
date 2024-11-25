using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A9C RID: 2716
public class SimulatedTemperatureAdjuster
{
	// Token: 0x06004FE9 RID: 20457 RVA: 0x001CBE7C File Offset: 0x001CA07C
	public SimulatedTemperatureAdjuster(float simulated_temperature, float heat_capacity, float thermal_conductivity, Storage storage)
	{
		this.temperature = simulated_temperature;
		this.heatCapacity = heat_capacity;
		this.thermalConductivity = thermal_conductivity;
		this.storage = storage;
		storage.gameObject.Subscribe(824508782, new Action<object>(this.OnActivechanged));
		storage.gameObject.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Operational component = storage.gameObject.GetComponent<Operational>();
		this.OnActivechanged(component);
	}

	// Token: 0x06004FEA RID: 20458 RVA: 0x001CBEFC File Offset: 0x001CA0FC
	public List<Descriptor> GetDescriptors()
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.temperature);
	}

	// Token: 0x06004FEB RID: 20459 RVA: 0x001CBF0C File Offset: 0x001CA10C
	public static List<Descriptor> GetDescriptors(float temperature)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x06004FEC RID: 20460 RVA: 0x001CBF5C File Offset: 0x001CA15C
	private void Register(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			this.OnItemSimRegistered(stt);
		}
	}

	// Token: 0x06004FED RID: 20461 RVA: 0x001CBFC4 File Offset: 0x001CA1C4
	private void Unregister(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, 0f, 0f, 0f);
		}
	}

	// Token: 0x06004FEE RID: 20462 RVA: 0x001CC01C File Offset: 0x001CA21C
	private void OnItemSimRegistered(SimTemperatureTransfer stt)
	{
		if (stt == null)
		{
			return;
		}
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			float num = this.temperature;
			float heat_capacity = this.heatCapacity;
			float thermal_conductivity = this.thermalConductivity;
			if (!this.active)
			{
				num = 0f;
				heat_capacity = 0f;
				thermal_conductivity = 0f;
			}
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, num, heat_capacity, thermal_conductivity);
		}
	}

	// Token: 0x06004FEF RID: 20463 RVA: 0x001CC080 File Offset: 0x001CA280
	private void OnActivechanged(object data)
	{
		Operational operational = (Operational)data;
		this.active = operational.IsActive;
		if (this.active)
		{
			using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					if (gameObject != null)
					{
						SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
						this.OnItemSimRegistered(component);
					}
				}
				return;
			}
		}
		foreach (GameObject gameObject2 in this.storage.items)
		{
			if (gameObject2 != null)
			{
				SimTemperatureTransfer component2 = gameObject2.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component2);
			}
		}
	}

	// Token: 0x06004FF0 RID: 20464 RVA: 0x001CC160 File Offset: 0x001CA360
	public void CleanUp()
	{
		this.storage.gameObject.Unsubscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component);
			}
		}
	}

	// Token: 0x06004FF1 RID: 20465 RVA: 0x001CC1EC File Offset: 0x001CA3EC
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2 == null)
		{
			return;
		}
		if (this.active && component2.storage == this.storage)
		{
			this.Register(component);
			return;
		}
		this.Unregister(component);
	}

	// Token: 0x04003516 RID: 13590
	private float temperature;

	// Token: 0x04003517 RID: 13591
	private float heatCapacity;

	// Token: 0x04003518 RID: 13592
	private float thermalConductivity;

	// Token: 0x04003519 RID: 13593
	private bool active;

	// Token: 0x0400351A RID: 13594
	private Storage storage;
}
