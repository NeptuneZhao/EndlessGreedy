using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using STRINGS;
using UnityEngine;

// Token: 0x020007B1 RID: 1969
public class CircuitManager
{
	// Token: 0x060035D1 RID: 13777 RVA: 0x00124AEE File Offset: 0x00122CEE
	public void Connect(Generator generator)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.generators.Add(generator);
		this.dirty = true;
	}

	// Token: 0x060035D2 RID: 13778 RVA: 0x00124B0C File Offset: 0x00122D0C
	public void Disconnect(Generator generator)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.generators.Remove(generator);
		this.dirty = true;
	}

	// Token: 0x060035D3 RID: 13779 RVA: 0x00124B2A File Offset: 0x00122D2A
	public void Connect(IEnergyConsumer consumer)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.consumers.Add(consumer);
		this.dirty = true;
	}

	// Token: 0x060035D4 RID: 13780 RVA: 0x00124B48 File Offset: 0x00122D48
	public void Disconnect(IEnergyConsumer consumer, bool isDestroy)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.consumers.Remove(consumer);
		if (!isDestroy)
		{
			consumer.SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
		}
		this.dirty = true;
	}

	// Token: 0x060035D5 RID: 13781 RVA: 0x00124B70 File Offset: 0x00122D70
	public void Connect(WireUtilityNetworkLink bridge)
	{
		this.bridges.Add(bridge);
		this.dirty = true;
	}

	// Token: 0x060035D6 RID: 13782 RVA: 0x00124B86 File Offset: 0x00122D86
	public void Disconnect(WireUtilityNetworkLink bridge)
	{
		this.bridges.Remove(bridge);
		this.dirty = true;
	}

	// Token: 0x060035D7 RID: 13783 RVA: 0x00124B9C File Offset: 0x00122D9C
	public float GetPowerDraw(ushort circuitID, Generator generator)
	{
		float result = 0f;
		if ((int)circuitID < this.circuitInfo.Count)
		{
			CircuitManager.CircuitInfo value = this.circuitInfo[(int)circuitID];
			this.circuitInfo[(int)circuitID] = value;
			this.circuitInfo[(int)circuitID] = value;
		}
		return result;
	}

	// Token: 0x060035D8 RID: 13784 RVA: 0x00124BE4 File Offset: 0x00122DE4
	public ushort GetCircuitID(int cell)
	{
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return (ushort)((networkForCell == null) ? 65535 : networkForCell.id);
	}

	// Token: 0x060035D9 RID: 13785 RVA: 0x00124C14 File Offset: 0x00122E14
	public ushort GetVirtualCircuitID(object virtualKey)
	{
		UtilityNetwork networkForVirtualKey = Game.Instance.electricalConduitSystem.GetNetworkForVirtualKey(virtualKey);
		return (ushort)((networkForVirtualKey == null) ? 65535 : networkForVirtualKey.id);
	}

	// Token: 0x060035DA RID: 13786 RVA: 0x00124C43 File Offset: 0x00122E43
	public ushort GetCircuitID(ICircuitConnected ent)
	{
		if (!ent.IsVirtual)
		{
			return this.GetCircuitID(ent.PowerCell);
		}
		return this.GetVirtualCircuitID(ent.VirtualCircuitKey);
	}

	// Token: 0x060035DB RID: 13787 RVA: 0x00124C66 File Offset: 0x00122E66
	public void Sim200msFirst(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x060035DC RID: 13788 RVA: 0x00124C6F File Offset: 0x00122E6F
	public void RenderEveryTick(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x060035DD RID: 13789 RVA: 0x00124C78 File Offset: 0x00122E78
	private void Refresh(float dt)
	{
		UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem = Game.Instance.electricalConduitSystem;
		if (electricalConduitSystem.IsDirty || this.dirty)
		{
			electricalConduitSystem.Update();
			IList<UtilityNetwork> networks = electricalConduitSystem.GetNetworks();
			while (this.circuitInfo.Count < networks.Count)
			{
				CircuitManager.CircuitInfo circuitInfo = new CircuitManager.CircuitInfo
				{
					generators = new List<Generator>(),
					consumers = new List<IEnergyConsumer>(),
					batteries = new List<Battery>(),
					inputTransformers = new List<Battery>(),
					outputTransformers = new List<Generator>()
				};
				circuitInfo.bridgeGroups = new List<WireUtilityNetworkLink>[5];
				for (int i = 0; i < circuitInfo.bridgeGroups.Length; i++)
				{
					circuitInfo.bridgeGroups[i] = new List<WireUtilityNetworkLink>();
				}
				this.circuitInfo.Add(circuitInfo);
			}
			this.Rebuild();
		}
	}

	// Token: 0x060035DE RID: 13790 RVA: 0x00124D58 File Offset: 0x00122F58
	public void Rebuild()
	{
		for (int i = 0; i < this.circuitInfo.Count; i++)
		{
			CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[i];
			circuitInfo.generators.Clear();
			circuitInfo.consumers.Clear();
			circuitInfo.batteries.Clear();
			circuitInfo.inputTransformers.Clear();
			circuitInfo.outputTransformers.Clear();
			circuitInfo.minBatteryPercentFull = 1f;
			for (int j = 0; j < circuitInfo.bridgeGroups.Length; j++)
			{
				circuitInfo.bridgeGroups[j].Clear();
			}
			this.circuitInfo[i] = circuitInfo;
		}
		this.consumersShadow.AddRange(this.consumers);
		foreach (IEnergyConsumer energyConsumer in this.consumersShadow)
		{
			ushort circuitID = this.GetCircuitID(energyConsumer);
			if (circuitID != 65535)
			{
				Battery battery = energyConsumer as Battery;
				if (battery != null)
				{
					CircuitManager.CircuitInfo circuitInfo2 = this.circuitInfo[(int)circuitID];
					if (battery.powerTransformer != null)
					{
						circuitInfo2.inputTransformers.Add(battery);
					}
					else
					{
						circuitInfo2.batteries.Add(battery);
						circuitInfo2.minBatteryPercentFull = Mathf.Min(this.circuitInfo[(int)circuitID].minBatteryPercentFull, battery.PercentFull);
					}
					this.circuitInfo[(int)circuitID] = circuitInfo2;
				}
				else
				{
					this.circuitInfo[(int)circuitID].consumers.Add(energyConsumer);
				}
			}
		}
		this.consumersShadow.Clear();
		for (int k = 0; k < this.circuitInfo.Count; k++)
		{
			this.circuitInfo[k].consumers.Sort((IEnergyConsumer a, IEnergyConsumer b) => a.WattsNeededWhenActive.CompareTo(b.WattsNeededWhenActive));
		}
		foreach (Generator generator in this.generators)
		{
			ushort circuitID2 = this.GetCircuitID(generator);
			if (circuitID2 != 65535)
			{
				if (generator.GetType() == typeof(PowerTransformer))
				{
					this.circuitInfo[(int)circuitID2].outputTransformers.Add(generator);
				}
				else
				{
					this.circuitInfo[(int)circuitID2].generators.Add(generator);
				}
			}
		}
		foreach (WireUtilityNetworkLink wireUtilityNetworkLink in this.bridges)
		{
			ushort circuitID3 = this.GetCircuitID(wireUtilityNetworkLink);
			if (circuitID3 != 65535)
			{
				Wire.WattageRating maxWattageRating = wireUtilityNetworkLink.GetMaxWattageRating();
				this.circuitInfo[(int)circuitID3].bridgeGroups[(int)maxWattageRating].Add(wireUtilityNetworkLink);
			}
		}
		this.dirty = false;
	}

	// Token: 0x060035DF RID: 13791 RVA: 0x00125030 File Offset: 0x00123230
	private float GetBatteryJoulesAvailable(List<Battery> batteries, out int num_powered)
	{
		float result = 0f;
		num_powered = 0;
		for (int i = 0; i < batteries.Count; i++)
		{
			if (batteries[i].JoulesAvailable > 0f)
			{
				result = batteries[i].JoulesAvailable;
				num_powered = batteries.Count - i;
				break;
			}
		}
		return result;
	}

	// Token: 0x060035E0 RID: 13792 RVA: 0x00125084 File Offset: 0x00123284
	public void Sim200msLast(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 0.2f)
		{
			return;
		}
		this.elapsedTime -= 0.2f;
		for (int i = 0; i < this.circuitInfo.Count; i++)
		{
			CircuitManager.CircuitInfo circuitInfo = this.circuitInfo[i];
			circuitInfo.wattsUsed = 0f;
			this.activeGenerators.Clear();
			List<Generator> list = circuitInfo.generators;
			List<IEnergyConsumer> list2 = circuitInfo.consumers;
			List<Battery> batteries = circuitInfo.batteries;
			List<Generator> outputTransformers = circuitInfo.outputTransformers;
			batteries.Sort((Battery a, Battery b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			bool flag = false;
			bool flag2 = list.Count > 0;
			for (int j = 0; j < list.Count; j++)
			{
				Generator generator = list[j];
				if (generator.JoulesAvailable > 0f)
				{
					flag = true;
					this.activeGenerators.Add(generator);
				}
			}
			this.activeGenerators.Sort((Generator a, Generator b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			if (!flag)
			{
				for (int k = 0; k < outputTransformers.Count; k++)
				{
					if (outputTransformers[k].JoulesAvailable > 0f)
					{
						flag = true;
					}
				}
			}
			float num = 1f;
			for (int l = 0; l < batteries.Count; l++)
			{
				Battery battery = batteries[l];
				if (battery.JoulesAvailable > 0f)
				{
					flag = true;
				}
				num = Mathf.Min(num, battery.PercentFull);
			}
			for (int m = 0; m < circuitInfo.inputTransformers.Count; m++)
			{
				Battery battery2 = circuitInfo.inputTransformers[m];
				num = Mathf.Min(num, battery2.PercentFull);
			}
			circuitInfo.minBatteryPercentFull = num;
			if (flag)
			{
				for (int n = 0; n < list2.Count; n++)
				{
					IEnergyConsumer energyConsumer = list2[n];
					float num2 = energyConsumer.WattsUsed * 0.2f;
					if (num2 > 0f)
					{
						bool flag3 = false;
						for (int num3 = 0; num3 < this.activeGenerators.Count; num3++)
						{
							Generator g = this.activeGenerators[num3];
							num2 = this.PowerFromGenerator(num2, g, energyConsumer);
							if (num2 <= 0f)
							{
								flag3 = true;
								break;
							}
						}
						if (!flag3)
						{
							for (int num4 = 0; num4 < outputTransformers.Count; num4++)
							{
								Generator g2 = outputTransformers[num4];
								num2 = this.PowerFromGenerator(num2, g2, energyConsumer);
								if (num2 <= 0f)
								{
									flag3 = true;
									break;
								}
							}
						}
						if (!flag3)
						{
							num2 = this.PowerFromBatteries(num2, batteries, energyConsumer);
							flag3 = (num2 <= 0.01f);
						}
						if (flag3)
						{
							circuitInfo.wattsUsed += energyConsumer.WattsUsed;
						}
						else
						{
							circuitInfo.wattsUsed += energyConsumer.WattsUsed - num2 / 0.2f;
						}
						energyConsumer.SetConnectionStatus(flag3 ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
					}
					else
					{
						energyConsumer.SetConnectionStatus(flag ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
					}
				}
			}
			else if (flag2)
			{
				for (int num5 = 0; num5 < list2.Count; num5++)
				{
					list2[num5].SetConnectionStatus(CircuitManager.ConnectionStatus.Unpowered);
				}
			}
			else
			{
				for (int num6 = 0; num6 < list2.Count; num6++)
				{
					list2[num6].SetConnectionStatus(CircuitManager.ConnectionStatus.NotConnected);
				}
			}
			this.circuitInfo[i] = circuitInfo;
		}
		for (int num7 = 0; num7 < this.circuitInfo.Count; num7++)
		{
			CircuitManager.CircuitInfo circuitInfo2 = this.circuitInfo[num7];
			circuitInfo2.batteries.Sort((Battery a, Battery b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable));
			circuitInfo2.inputTransformers.Sort((Battery a, Battery b) => (a.Capacity - a.JoulesAvailable).CompareTo(b.Capacity - b.JoulesAvailable));
			circuitInfo2.generators.Sort((Generator a, Generator b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			float num8 = 0f;
			this.ChargeTransformers<Generator>(circuitInfo2.inputTransformers, circuitInfo2.generators, ref num8);
			this.ChargeTransformers<Generator>(circuitInfo2.inputTransformers, circuitInfo2.outputTransformers, ref num8);
			float num9 = 0f;
			this.ChargeBatteries(circuitInfo2.batteries, circuitInfo2.generators, ref num9);
			this.ChargeBatteries(circuitInfo2.batteries, circuitInfo2.outputTransformers, ref num9);
			circuitInfo2.minBatteryPercentFull = 1f;
			for (int num10 = 0; num10 < circuitInfo2.batteries.Count; num10++)
			{
				float percentFull = circuitInfo2.batteries[num10].PercentFull;
				if (percentFull < circuitInfo2.minBatteryPercentFull)
				{
					circuitInfo2.minBatteryPercentFull = percentFull;
				}
			}
			for (int num11 = 0; num11 < circuitInfo2.inputTransformers.Count; num11++)
			{
				float percentFull2 = circuitInfo2.inputTransformers[num11].PercentFull;
				if (percentFull2 < circuitInfo2.minBatteryPercentFull)
				{
					circuitInfo2.minBatteryPercentFull = percentFull2;
				}
			}
			circuitInfo2.wattsUsed += num8 / 0.2f;
			this.circuitInfo[num7] = circuitInfo2;
		}
		for (int num12 = 0; num12 < this.circuitInfo.Count; num12++)
		{
			CircuitManager.CircuitInfo circuitInfo3 = this.circuitInfo[num12];
			circuitInfo3.batteries.Sort((Battery a, Battery b) => a.JoulesAvailable.CompareTo(b.JoulesAvailable));
			float num13 = 0f;
			this.ChargeTransformers<Battery>(circuitInfo3.inputTransformers, circuitInfo3.batteries, ref num13);
			circuitInfo3.wattsUsed += num13 / 0.2f;
			this.circuitInfo[num12] = circuitInfo3;
		}
		for (int num14 = 0; num14 < this.circuitInfo.Count; num14++)
		{
			CircuitManager.CircuitInfo circuitInfo4 = this.circuitInfo[num14];
			bool is_connected_to_something_useful = circuitInfo4.generators.Count + circuitInfo4.consumers.Count + circuitInfo4.outputTransformers.Count > 0;
			this.UpdateBatteryConnectionStatus(circuitInfo4.batteries, is_connected_to_something_useful, num14);
			bool flag4 = circuitInfo4.generators.Count > 0 || circuitInfo4.outputTransformers.Count > 0;
			if (!flag4)
			{
				using (List<Battery>.Enumerator enumerator = circuitInfo4.batteries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.JoulesAvailable > 0f)
						{
							flag4 = true;
							break;
						}
					}
				}
			}
			this.UpdateBatteryConnectionStatus(circuitInfo4.inputTransformers, flag4, num14);
			this.circuitInfo[num14] = circuitInfo4;
			for (int num15 = 0; num15 < circuitInfo4.generators.Count; num15++)
			{
				Generator generator2 = circuitInfo4.generators[num15];
				ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -generator2.JoulesAvailable, StringFormatter.Replace(BUILDINGS.PREFABS.GENERATOR.OVERPRODUCTION, "{Generator}", generator2.gameObject.GetProperName()), null);
			}
		}
		for (int num16 = 0; num16 < this.circuitInfo.Count; num16++)
		{
			CircuitManager.CircuitInfo circuitInfo5 = this.circuitInfo[num16];
			this.CheckCircuitOverloaded(0.2f, num16, circuitInfo5.wattsUsed);
		}
	}

	// Token: 0x060035E1 RID: 13793 RVA: 0x00125818 File Offset: 0x00123A18
	private float PowerFromBatteries(float joules_needed, List<Battery> batteries, IEnergyConsumer c)
	{
		int num2;
		do
		{
			float num = this.GetBatteryJoulesAvailable(batteries, out num2) * (float)num2;
			float num3 = (num < joules_needed) ? num : joules_needed;
			joules_needed -= num3;
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num3, c.Name, null);
			float joules = num3 / (float)num2;
			for (int i = batteries.Count - num2; i < batteries.Count; i++)
			{
				batteries[i].ConsumeEnergy(joules);
			}
		}
		while (joules_needed >= 0.01f && num2 > 0);
		return joules_needed;
	}

	// Token: 0x060035E2 RID: 13794 RVA: 0x00125894 File Offset: 0x00123A94
	private float PowerFromGenerator(float joules_needed, Generator g, IEnergyConsumer c)
	{
		float num = Mathf.Min(g.JoulesAvailable, joules_needed);
		joules_needed -= num;
		g.ApplyDeltaJoules(-num, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, -num, c.Name, null);
		return joules_needed;
	}

	// Token: 0x060035E3 RID: 13795 RVA: 0x001258D4 File Offset: 0x00123AD4
	private void ChargeBatteries(List<Battery> sink_batteries, List<Generator> source_generators, ref float joules_used)
	{
		if (sink_batteries.Count == 0)
		{
			return;
		}
		foreach (Generator generator in source_generators)
		{
			for (bool flag = true; flag && generator.JoulesAvailable >= 1f; flag = this.ChargeBatteriesFromGenerator(sink_batteries, generator, ref joules_used))
			{
			}
		}
	}

	// Token: 0x060035E4 RID: 13796 RVA: 0x00125944 File Offset: 0x00123B44
	private bool ChargeBatteriesFromGenerator(List<Battery> sink_batteries, Generator source_generator, ref float joules_used)
	{
		float num = source_generator.JoulesAvailable;
		float num2 = 0f;
		for (int i = 0; i < sink_batteries.Count; i++)
		{
			Battery battery = sink_batteries[i];
			if (battery != null && source_generator != null && battery.gameObject != source_generator.gameObject)
			{
				float num3 = battery.Capacity - battery.JoulesAvailable;
				if (num3 > 0f)
				{
					float num4 = Mathf.Min(num3, num / (float)(sink_batteries.Count - i));
					battery.AddEnergy(num4);
					num -= num4;
					num2 += num4;
				}
			}
		}
		if (num2 > 0f)
		{
			source_generator.ApplyDeltaJoules(-num2, false);
			joules_used += num2;
			return true;
		}
		return false;
	}

	// Token: 0x060035E5 RID: 13797 RVA: 0x001259F4 File Offset: 0x00123BF4
	private void UpdateBatteryConnectionStatus(List<Battery> batteries, bool is_connected_to_something_useful, int circuit_id)
	{
		foreach (Battery battery in batteries)
		{
			if (!(battery == null))
			{
				if (battery.powerTransformer == null)
				{
					battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.NotConnected);
				}
				else if ((int)this.GetCircuitID(battery) == circuit_id)
				{
					battery.SetConnectionStatus(is_connected_to_something_useful ? CircuitManager.ConnectionStatus.Powered : CircuitManager.ConnectionStatus.Unpowered);
				}
			}
		}
	}

	// Token: 0x060035E6 RID: 13798 RVA: 0x00125A78 File Offset: 0x00123C78
	private void ChargeTransformer<T>(Battery sink_transformer, List<T> source_energy_producers, ref float joules_used) where T : IEnergyProducer
	{
		if (source_energy_producers.Count <= 0)
		{
			return;
		}
		float num = Mathf.Min(sink_transformer.Capacity - sink_transformer.JoulesAvailable, sink_transformer.ChargeCapacity);
		if (num <= 0f)
		{
			return;
		}
		float num2 = num;
		float num3 = 0f;
		for (int i = 0; i < source_energy_producers.Count; i++)
		{
			T t = source_energy_producers[i];
			if (t.JoulesAvailable > 0f)
			{
				float num4 = Mathf.Min(t.JoulesAvailable, num2 / (float)(source_energy_producers.Count - i));
				t.ConsumeEnergy(num4);
				num2 -= num4;
				num3 += num4;
			}
		}
		sink_transformer.AddEnergy(num3);
		joules_used += num3;
	}

	// Token: 0x060035E7 RID: 13799 RVA: 0x00125B2C File Offset: 0x00123D2C
	private void ChargeTransformers<T>(List<Battery> sink_transformers, List<T> source_energy_producers, ref float joules_used) where T : IEnergyProducer
	{
		foreach (Battery sink_transformer in sink_transformers)
		{
			this.ChargeTransformer<T>(sink_transformer, source_energy_producers, ref joules_used);
		}
	}

	// Token: 0x060035E8 RID: 13800 RVA: 0x00125B7C File Offset: 0x00123D7C
	private void CheckCircuitOverloaded(float dt, int id, float watts_used)
	{
		UtilityNetwork networkByID = Game.Instance.electricalConduitSystem.GetNetworkByID(id);
		if (networkByID != null)
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)networkByID;
			if (electricalUtilityNetwork != null)
			{
				electricalUtilityNetwork.UpdateOverloadTime(dt, watts_used, this.circuitInfo[id].bridgeGroups);
			}
		}
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x00125BC0 File Offset: 0x00123DC0
	public float GetWattsUsedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		return this.circuitInfo[(int)circuitID].wattsUsed;
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x00125BE4 File Offset: 0x00123DE4
	public float GetWattsNeededWhenActive(ushort originCircuitId)
	{
		if (originCircuitId == 65535)
		{
			return -1f;
		}
		HashSet<ushort> hashSet = new HashSet<ushort>();
		HashSet<ushort> hashSet2 = new HashSet<ushort>();
		HashSet<ushort> hashSet3 = new HashSet<ushort>();
		hashSet2.Add(originCircuitId);
		int num = 0;
		while (hashSet2.Count > 0)
		{
			num++;
			if (num > 100)
			{
				break;
			}
			foreach (ushort num2 in hashSet2)
			{
				if (num2 >= 0 && (int)num2 < this.circuitInfo.Count)
				{
					foreach (Battery battery in this.circuitInfo[(int)num2].inputTransformers)
					{
						ushort circuitID = battery.powerTransformer.CircuitID;
						if (battery.powerTransformer.CircuitID != 65535)
						{
							hashSet3.Add(circuitID);
						}
					}
					hashSet.Add(num2);
				}
			}
			hashSet2.Clear();
			foreach (ushort item in hashSet3)
			{
				if (!hashSet.Contains(item))
				{
					hashSet2.Add(item);
				}
			}
			hashSet3.Clear();
		}
		HashSet<ushort> hashSet4 = hashSet;
		Dictionary<ushort, float> dictionary = new Dictionary<ushort, float>();
		foreach (ushort num3 in hashSet4)
		{
			if (num3 >= 0 && (int)num3 < this.circuitInfo.Count)
			{
				float num4 = 0f;
				foreach (IEnergyConsumer energyConsumer in this.circuitInfo[(int)num3].consumers)
				{
					num4 += energyConsumer.WattsNeededWhenActive;
				}
				dictionary.Add(num3, num4);
			}
		}
		Dictionary<ushort, float> dictionary2 = new Dictionary<ushort, float>();
		foreach (Battery battery2 in this.circuitInfo[(int)originCircuitId].inputTransformers)
		{
			float b;
			dictionary.TryGetValue(battery2.powerTransformer.CircuitID, out b);
			float b2 = Mathf.Min(battery2.powerTransformer.WattageRating, b);
			float a;
			dictionary2.TryGetValue(battery2.powerTransformer.CircuitID, out a);
			dictionary2[battery2.powerTransformer.CircuitID] = Mathf.Max(a, b2);
		}
		float num5;
		dictionary.TryGetValue(originCircuitId, out num5);
		foreach (KeyValuePair<ushort, float> keyValuePair in dictionary2)
		{
			ushort num6;
			float num7;
			keyValuePair.Deconstruct(out num6, out num7);
			float num8 = num7;
			num5 += num8;
		}
		return num5;
	}

	// Token: 0x060035EB RID: 13803 RVA: 0x00125F24 File Offset: 0x00124124
	public float GetWattsGeneratedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		float num = 0f;
		foreach (Generator generator in this.circuitInfo[(int)circuitID].generators)
		{
			if (!(generator == null) && generator.IsProducingPower())
			{
				num += generator.WattageRating;
			}
		}
		return num;
	}

	// Token: 0x060035EC RID: 13804 RVA: 0x00125FAC File Offset: 0x001241AC
	public float GetPotentialWattsGeneratedByCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return -1f;
		}
		float num = 0f;
		foreach (Generator generator in this.circuitInfo[(int)circuitID].generators)
		{
			num += generator.WattageRating;
		}
		return num;
	}

	// Token: 0x060035ED RID: 13805 RVA: 0x00126024 File Offset: 0x00124224
	public float GetJoulesAvailableOnCircuit(ushort circuitID)
	{
		int num;
		return this.GetBatteryJoulesAvailable(this.GetBatteriesOnCircuit(circuitID), out num) * (float)num;
	}

	// Token: 0x060035EE RID: 13806 RVA: 0x00126043 File Offset: 0x00124243
	public ReadOnlyCollection<Generator> GetGeneratorsOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].generators.AsReadOnly();
	}

	// Token: 0x060035EF RID: 13807 RVA: 0x00126065 File Offset: 0x00124265
	public ReadOnlyCollection<IEnergyConsumer> GetConsumersOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].consumers.AsReadOnly();
	}

	// Token: 0x060035F0 RID: 13808 RVA: 0x00126087 File Offset: 0x00124287
	public ReadOnlyCollection<Battery> GetTransformersOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].inputTransformers.AsReadOnly();
	}

	// Token: 0x060035F1 RID: 13809 RVA: 0x001260A9 File Offset: 0x001242A9
	public List<Battery> GetBatteriesOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return null;
		}
		return this.circuitInfo[(int)circuitID].batteries;
	}

	// Token: 0x060035F2 RID: 13810 RVA: 0x001260C6 File Offset: 0x001242C6
	public float GetMinBatteryPercentFullOnCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return 0f;
		}
		return this.circuitInfo[(int)circuitID].minBatteryPercentFull;
	}

	// Token: 0x060035F3 RID: 13811 RVA: 0x001260E7 File Offset: 0x001242E7
	public bool HasBatteries(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].batteries.Count + this.circuitInfo[(int)circuitID].inputTransformers.Count > 0;
	}

	// Token: 0x060035F4 RID: 13812 RVA: 0x00126123 File Offset: 0x00124323
	public bool HasGenerators(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].generators.Count + this.circuitInfo[(int)circuitID].outputTransformers.Count > 0;
	}

	// Token: 0x060035F5 RID: 13813 RVA: 0x0012615F File Offset: 0x0012435F
	public bool HasGenerators()
	{
		return this.generators.Count > 0;
	}

	// Token: 0x060035F6 RID: 13814 RVA: 0x0012616F File Offset: 0x0012436F
	public bool HasConsumers(ushort circuitID)
	{
		return circuitID != ushort.MaxValue && this.circuitInfo[(int)circuitID].consumers.Count > 0;
	}

	// Token: 0x060035F7 RID: 13815 RVA: 0x00126194 File Offset: 0x00124394
	public float GetMaxSafeWattageForCircuit(ushort circuitID)
	{
		if (circuitID == 65535)
		{
			return 0f;
		}
		ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkByID((int)circuitID) as ElectricalUtilityNetwork;
		if (electricalUtilityNetwork == null)
		{
			return 0f;
		}
		return electricalUtilityNetwork.GetMaxSafeWattage();
	}

	// Token: 0x04002011 RID: 8209
	public const ushort INVALID_ID = 65535;

	// Token: 0x04002012 RID: 8210
	private const int SimUpdateSortKey = 1000;

	// Token: 0x04002013 RID: 8211
	private const float MIN_POWERED_THRESHOLD = 0.01f;

	// Token: 0x04002014 RID: 8212
	private bool dirty = true;

	// Token: 0x04002015 RID: 8213
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x04002016 RID: 8214
	private HashSet<IEnergyConsumer> consumers = new HashSet<IEnergyConsumer>();

	// Token: 0x04002017 RID: 8215
	private HashSet<WireUtilityNetworkLink> bridges = new HashSet<WireUtilityNetworkLink>();

	// Token: 0x04002018 RID: 8216
	private float elapsedTime;

	// Token: 0x04002019 RID: 8217
	private List<CircuitManager.CircuitInfo> circuitInfo = new List<CircuitManager.CircuitInfo>();

	// Token: 0x0400201A RID: 8218
	private List<IEnergyConsumer> consumersShadow = new List<IEnergyConsumer>();

	// Token: 0x0400201B RID: 8219
	private List<Generator> activeGenerators = new List<Generator>();

	// Token: 0x02001667 RID: 5735
	private struct CircuitInfo
	{
		// Token: 0x04006F8A RID: 28554
		public List<Generator> generators;

		// Token: 0x04006F8B RID: 28555
		public List<IEnergyConsumer> consumers;

		// Token: 0x04006F8C RID: 28556
		public List<Battery> batteries;

		// Token: 0x04006F8D RID: 28557
		public List<Battery> inputTransformers;

		// Token: 0x04006F8E RID: 28558
		public List<Generator> outputTransformers;

		// Token: 0x04006F8F RID: 28559
		public List<WireUtilityNetworkLink>[] bridgeGroups;

		// Token: 0x04006F90 RID: 28560
		public float minBatteryPercentFull;

		// Token: 0x04006F91 RID: 28561
		public float wattsUsed;
	}

	// Token: 0x02001668 RID: 5736
	public enum ConnectionStatus
	{
		// Token: 0x04006F93 RID: 28563
		NotConnected,
		// Token: 0x04006F94 RID: 28564
		Unpowered,
		// Token: 0x04006F95 RID: 28565
		Powered
	}
}
