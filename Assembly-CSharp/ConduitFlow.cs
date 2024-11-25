using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x020007E0 RID: 2016
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{conduitType}")]
public class ConduitFlow : IConduitFlow
{
	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06003790 RID: 14224 RVA: 0x0012ECA4 File Offset: 0x0012CEA4
	// (remove) Token: 0x06003791 RID: 14225 RVA: 0x0012ECDC File Offset: 0x0012CEDC
	public event System.Action onConduitsRebuilt;

	// Token: 0x06003792 RID: 14226 RVA: 0x0012ED14 File Offset: 0x0012CF14
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new ConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x0012ED4C File Offset: 0x0012CF4C
	public void RemoveConduitUpdater(Action<float> callback)
	{
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			if (this.conduitUpdaters[i].callback == callback)
			{
				this.conduitUpdaters.RemoveAt(i);
				this.dirtyConduitUpdaters = true;
				return;
			}
		}
	}

	// Token: 0x06003794 RID: 14228 RVA: 0x0012ED9C File Offset: 0x0012CF9C
	private static ConduitFlow.FlowDirections ComputeFlowDirection(int index)
	{
		return (ConduitFlow.FlowDirections)(1 << index);
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x0012EDA8 File Offset: 0x0012CFA8
	private static ConduitFlow.FlowDirections ComputeNextFlowDirection(ConduitFlow.FlowDirections current)
	{
		switch (current)
		{
		case ConduitFlow.FlowDirections.None:
		case ConduitFlow.FlowDirections.Up:
			return ConduitFlow.FlowDirections.Down;
		case ConduitFlow.FlowDirections.Down:
			return ConduitFlow.FlowDirections.Left;
		case ConduitFlow.FlowDirections.Left:
			return ConduitFlow.FlowDirections.Right;
		case ConduitFlow.FlowDirections.Right:
			return ConduitFlow.FlowDirections.Up;
		}
		global::Debug.Assert(false, "multiple bits are set in 'FlowDirections'...can't compute next direction");
		return ConduitFlow.FlowDirections.Down;
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x0012EDF5 File Offset: 0x0012CFF5
	public static ConduitFlow.FlowDirections Invert(ConduitFlow.FlowDirections directions)
	{
		return ConduitFlow.FlowDirections.All & ~directions;
	}

	// Token: 0x06003797 RID: 14231 RVA: 0x0012EE00 File Offset: 0x0012D000
	public static ConduitFlow.FlowDirections Opposite(ConduitFlow.FlowDirections directions)
	{
		ConduitFlow.FlowDirections result = ConduitFlow.FlowDirections.None;
		if ((directions & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Right;
		}
		else if ((directions & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Left;
		}
		else if ((directions & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Down;
		}
		else if ((directions & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Up;
		}
		return result;
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x0012EE34 File Offset: 0x0012D034
	public ConduitFlow(ConduitType conduit_type, int num_cells, IUtilityNetworkMgr network_mgr, float max_conduit_mass, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.conduitType = conduit_type;
		this.networkMgr = network_mgr;
		this.MaxMass = max_conduit_mass;
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x0012EEE4 File Offset: 0x0012D0E4
	public void Initialize(int num_cells)
	{
		this.grid = new ConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.element = SimHashes.Vacuum;
			this.grid[i].contents.diseaseIdx = byte.MaxValue;
		}
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x0012EF54 File Offset: 0x0012D154
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		int count = this.networks.Count - networks.Count;
		if (0 < this.networks.Count - networks.Count)
		{
			this.networks.RemoveRange(networks.Count, count);
		}
		global::Debug.Assert(this.networks.Count <= networks.Count);
		for (int num = 0; num != networks.Count; num++)
		{
			if (num < this.networks.Count)
			{
				this.networks[num] = new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = this.networks[num].cells
				};
				this.networks[num].cells.Clear();
			}
			else
			{
				this.networks.Add(new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = new List<int>()
				});
			}
		}
		this.build_network_job.Reset(this);
		foreach (ConduitFlow.Network network in this.networks)
		{
			this.build_network_job.Add(new ConduitFlow.BuildNetworkTask(network, this.soaInfo.NumEntries));
		}
		GlobalJobManager.Run(this.build_network_job);
		for (int num2 = 0; num2 != this.build_network_job.Count; num2++)
		{
			this.build_network_job.GetWorkItem(num2).Finish();
		}
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0012F114 File Offset: 0x0012D314
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		ConduitFlow.ConnectContext connectContext = new ConduitFlow.ConnectContext(this);
		this.soaInfo.Clear(this);
		this.replacements.ExceptWith(root_nodes);
		ObjectLayer layer = (this.conduitType == ConduitType.Gas) ? ObjectLayer.GasConduit : ObjectLayer.LiquidConduit;
		foreach (int num in root_nodes)
		{
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				global::Conduit component = gameObject.GetComponent<global::Conduit>();
				if (!(component != null) || !component.IsDisconnected())
				{
					int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
					this.grid[num].conduitIdx = conduitIdx;
					connectContext.cells.Add(num);
				}
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		this.connect_job.Reset(connectContext);
		int num2 = 256;
		for (int i = 0; i < connectContext.cells.Count; i += num2)
		{
			this.connect_job.Add(new ConduitFlow.ConnectTask(i, Mathf.Min(i + num2, connectContext.cells.Count)));
		}
		GlobalJobManager.Run(this.connect_job);
		connectContext.Finish();
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0012F27C File Offset: 0x0012D47C
	private ConduitFlow.FlowDirections GetDirection(ConduitFlow.Conduit conduit, ConduitFlow.Conduit target_conduit)
	{
		global::Debug.Assert(conduit.idx != -1);
		global::Debug.Assert(target_conduit.idx != -1);
		ConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Right;
		}
		return ConduitFlow.FlowDirections.None;
	}

	// Token: 0x0600379D RID: 14237 RVA: 0x0012F300 File Offset: 0x0012D500
	public int ComputeUpdateOrder(int cell)
	{
		foreach (ConduitFlow.Network network in this.networks)
		{
			int num = network.cells.IndexOf(cell);
			if (num != -1)
			{
				return num;
			}
		}
		return -1;
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x0012F364 File Offset: 0x0012D564
	public ConduitFlow.ConduitContents GetContents(int cell)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell].contents;
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError(string.Format("unexpected temperature {0}", contents.temperature));
		}
		return contents;
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x0012F3EC File Offset: 0x0012D5EC
	public void SetContents(int cell, ConduitFlow.ConduitContents contents)
	{
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x0012F43D File Offset: 0x0012D63D
	public static int GetCellFromDirection(int cell, ConduitFlow.FlowDirections direction)
	{
		switch (direction)
		{
		case ConduitFlow.FlowDirections.Down:
			return Grid.CellBelow(cell);
		case ConduitFlow.FlowDirections.Left:
			return Grid.CellLeft(cell);
		case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
			break;
		case ConduitFlow.FlowDirections.Right:
			return Grid.CellRight(cell);
		default:
			if (direction == ConduitFlow.FlowDirections.Up)
			{
				return Grid.CellAbove(cell);
			}
			break;
		}
		return -1;
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x0012F47C File Offset: 0x0012D67C
	public void Sim200ms(float dt)
	{
		if (dt <= 0f)
		{
			return;
		}
		this.elapsedTime += dt;
		if (this.elapsedTime < 1f)
		{
			return;
		}
		this.elapsedTime -= 1f;
		float obj = 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.PooledList pooledList = ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.Allocate();
		pooledList.Capacity = Mathf.Max(pooledList.Capacity, this.networks.Count);
		foreach (ConduitFlow.Network network in this.networks)
		{
			pooledList.Add(new ConduitFlow.UpdateNetworkTask(network));
		}
		int num = 0;
		while (num != 4 && pooledList.Count != 0)
		{
			this.update_networks_job.Reset(this);
			foreach (ConduitFlow.UpdateNetworkTask work_item in pooledList)
			{
				this.update_networks_job.Add(work_item);
			}
			GlobalJobManager.Run(this.update_networks_job);
			pooledList.Clear();
			for (int num2 = 0; num2 != this.update_networks_job.Count; num2++)
			{
				ConduitFlow.UpdateNetworkTask workItem = this.update_networks_job.GetWorkItem(num2);
				if (workItem.continue_updating && num != 3)
				{
					pooledList.Add(workItem);
				}
				else
				{
					workItem.Finish(this);
				}
			}
			num++;
		}
		pooledList.Recycle();
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((ConduitFlow.ConduitUpdater a, ConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x0012F68C File Offset: 0x0012D88C
	private float ComputeMovableMass(ConduitFlow.GridNode grid_node)
	{
		ConduitFlow.ConduitContents contents = grid_node.contents;
		if (contents.element == SimHashes.Vacuum)
		{
			return 0f;
		}
		return contents.movable_mass;
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x0012F6BC File Offset: 0x0012D8BC
	private bool UpdateConduit(ConduitFlow.Conduit conduit)
	{
		bool result = false;
		int cell = this.soaInfo.GetCell(conduit.idx);
		ConduitFlow.GridNode gridNode = this.grid[cell];
		float num = this.ComputeMovableMass(gridNode);
		ConduitFlow.FlowDirections permittedFlowDirections = this.soaInfo.GetPermittedFlowDirections(conduit.idx);
		ConduitFlow.FlowDirections flowDirections = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		if (num <= 0f)
		{
			for (int num2 = 0; num2 != 4; num2++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection.idx != -1);
					if ((this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx) & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None)
					{
						this.soaInfo.SetPullDirection(conduitFromDirection.idx, flowDirections);
					}
				}
			}
		}
		else
		{
			for (int num3 = 0; num3 != 4; num3++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection2.idx != -1);
					ConduitFlow.FlowDirections srcFlowDirection = this.soaInfo.GetSrcFlowDirection(conduitFromDirection2.idx);
					bool flag = (srcFlowDirection & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None;
					if (srcFlowDirection != ConduitFlow.FlowDirections.None && !flag)
					{
						result = true;
					}
					else
					{
						int cell2 = this.soaInfo.GetCell(conduitFromDirection2.idx);
						global::Debug.Assert(cell2 != -1);
						ConduitFlow.ConduitContents contents = this.grid[cell2].contents;
						bool flag2 = contents.element == SimHashes.Vacuum || contents.element == gridNode.contents.element;
						float effectiveCapacity = contents.GetEffectiveCapacity(this.MaxMass);
						bool flag3 = flag2 && effectiveCapacity > 0f;
						float num4 = Mathf.Min(num, effectiveCapacity);
						if (flag && flag3)
						{
							this.soaInfo.SetPullDirection(conduitFromDirection2.idx, flowDirections);
						}
						if (num4 > 0f && flag3)
						{
							this.soaInfo.SetTargetFlowDirection(conduit.idx, flowDirections);
							global::Debug.Assert(gridNode.contents.temperature > 0f);
							contents.temperature = GameUtil.GetFinalTemperature(gridNode.contents.temperature, num4, contents.temperature, contents.mass);
							contents.AddMass(num4);
							contents.element = gridNode.contents.element;
							int num5 = (int)(num4 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
							if (num5 != 0)
							{
								SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(gridNode.contents.diseaseIdx, num5, contents.diseaseIdx, contents.diseaseCount);
								contents.diseaseIdx = diseaseInfo.idx;
								contents.diseaseCount = diseaseInfo.count;
							}
							this.grid[cell2].contents = contents;
							global::Debug.Assert(num4 <= gridNode.contents.mass);
							float num6 = gridNode.contents.mass - num4;
							num -= num4;
							if (num6 <= 0f)
							{
								global::Debug.Assert(num <= 0f);
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref gridNode.contents);
								gridNode.contents = ConduitFlow.ConduitContents.Empty;
							}
							else
							{
								int num7 = (int)(num6 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
								global::Debug.Assert(num7 >= 0);
								ConduitFlow.ConduitContents contents2 = gridNode.contents;
								contents2.RemoveMass(num6);
								contents2.diseaseCount -= num7;
								gridNode.contents.RemoveMass(num4);
								gridNode.contents.diseaseCount = num7;
								if (num7 == 0)
								{
									gridNode.contents.diseaseIdx = byte.MaxValue;
								}
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref contents2);
							}
							this.grid[cell].contents = gridNode.contents;
							result = (0f < this.ComputeMovableMass(gridNode));
							break;
						}
					}
				}
			}
		}
		ConduitFlow.FlowDirections srcFlowDirection2 = this.soaInfo.GetSrcFlowDirection(conduit.idx);
		ConduitFlow.FlowDirections pullDirection = this.soaInfo.GetPullDirection(conduit.idx);
		if (srcFlowDirection2 == ConduitFlow.FlowDirections.None || (ConduitFlow.Opposite(srcFlowDirection2) & pullDirection) != ConduitFlow.FlowDirections.None)
		{
			this.soaInfo.SetPullDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			this.soaInfo.SetSrcFlowDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			for (int num8 = 0; num8 != 2; num8++)
			{
				ConduitFlow.FlowDirections flowDirections2 = srcFlowDirection2;
				for (int num9 = 0; num9 != 4; num9++)
				{
					flowDirections2 = ConduitFlow.ComputeNextFlowDirection(flowDirections2);
					ConduitFlow.Conduit conduitFromDirection3 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections2);
					if (conduitFromDirection3.idx != -1 && (this.soaInfo.GetPermittedFlowDirections(conduitFromDirection3.idx) & ConduitFlow.Opposite(flowDirections2)) != ConduitFlow.FlowDirections.None)
					{
						int cell3 = this.soaInfo.GetCell(conduitFromDirection3.idx);
						ConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
						float num10 = (num8 == 0) ? contents3.movable_mass : contents3.mass;
						if (0f < num10)
						{
							this.soaInfo.SetSrcFlowDirection(conduit.idx, flowDirections2);
							break;
						}
					}
				}
				if (this.soaInfo.GetSrcFlowDirection(conduit.idx) != ConduitFlow.FlowDirections.None)
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x060037A4 RID: 14244 RVA: 0x0012FC1B File Offset: 0x0012DE1B
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x060037A5 RID: 14245 RVA: 0x0012FC34 File Offset: 0x0012DE34
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x060037A6 RID: 14246 RVA: 0x0012FC47 File Offset: 0x0012DE47
	public float GetAmountAllowedForMerging(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massDesiredtoBeMoved)
	{
		return Mathf.Min(massDesiredtoBeMoved, this.MaxMass - to.mass);
	}

	// Token: 0x060037A7 RID: 14247 RVA: 0x0012FC5D File Offset: 0x0012DE5D
	public bool CanMergeContents(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massToMove)
	{
		return (from.element == to.element || to.element == SimHashes.Vacuum || massToMove <= 0f) && this.GetAmountAllowedForMerging(from, to, massToMove) > 0f;
	}

	// Token: 0x060037A8 RID: 14248 RVA: 0x0012FC98 File Offset: 0x0012DE98
	public float AddElement(int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			return 0f;
		}
		ConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.element != element && contents.element != SimHashes.Vacuum && mass > 0f)
		{
			return 0f;
		}
		float num = Mathf.Min(mass, this.MaxMass - contents.mass);
		float num2 = num / mass;
		if (num <= 0f)
		{
			return 0f;
		}
		contents.temperature = GameUtil.GetFinalTemperature(temperature, num, contents.temperature, contents.mass);
		contents.AddMass(num);
		contents.element = element;
		contents.ConsolidateMass();
		int num3 = (int)(num2 * (float)disease_count);
		if (num3 > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, num3, contents.diseaseIdx, contents.diseaseCount);
			contents.diseaseIdx = diseaseInfo.idx;
			contents.diseaseCount = diseaseInfo.count;
		}
		this.SetContents(cell_idx, contents);
		return num;
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x0012FD98 File Offset: 0x0012DF98
	public ConduitFlow.ConduitContents RemoveElement(int cell, float delta)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.ConduitContents.Empty;
		}
		return this.RemoveElement(conduit, delta);
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x0012FDC4 File Offset: 0x0012DFC4
	public ConduitFlow.ConduitContents RemoveElement(ConduitFlow.Conduit conduit, float delta)
	{
		ConduitFlow.ConduitContents contents = conduit.GetContents(this);
		float num = Mathf.Min(contents.mass, delta);
		float num2 = contents.mass - num;
		if (num2 <= 0f)
		{
			conduit.SetContents(this, ConduitFlow.ConduitContents.Empty);
			return contents;
		}
		ConduitFlow.ConduitContents result = contents;
		result.RemoveMass(num2);
		int num3 = (int)(num2 / contents.mass * (float)contents.diseaseCount);
		result.diseaseCount = contents.diseaseCount - num3;
		ConduitFlow.ConduitContents contents2 = contents;
		contents2.RemoveMass(num);
		contents2.diseaseCount = num3;
		if (num3 <= 0)
		{
			contents2.diseaseIdx = byte.MaxValue;
			contents2.diseaseCount = 0;
		}
		conduit.SetContents(this, contents2);
		return result;
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x0012FE74 File Offset: 0x0012E074
	public ConduitFlow.FlowDirections GetPermittedFlow(int cell)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.FlowDirections.None;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x0012FEA5 File Offset: 0x0012E0A5
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x0012FEC0 File Offset: 0x0012E0C0
	public ConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return ConduitFlow.Conduit.Invalid;
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x0012FEF8 File Offset: 0x0012E0F8
	private void DumpPipeContents(int cell, ConduitFlow.ConduitContents contents)
	{
		if (contents.element != SimHashes.Vacuum && contents.mass > 0f)
		{
			SimMessages.AddRemoveSubstance(cell, contents.element, CellEventLogger.Instance.ConduitFlowEmptyConduit, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, true, -1);
			this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
		}
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x0012FF5D File Offset: 0x0012E15D
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell, this.grid[cell].contents);
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x0012FF86 File Offset: 0x0012E186
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x0012FF95 File Offset: 0x0012E195
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x0012FFB5 File Offset: 0x0012E1B5
	[Conditional("CHECK_NAN")]
	private void Validate(ConduitFlow.ConduitContents contents)
	{
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError("zero degree pipe contents");
		}
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x0012FFDC File Offset: 0x0012E1DC
	[OnSerializing]
	private void OnSerializing()
	{
		int numEntries = this.soaInfo.NumEntries;
		if (numEntries > 0)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[numEntries];
			this.serializedIdx = new int[numEntries];
			for (int i = 0; i < numEntries; i++)
			{
				ConduitFlow.Conduit conduit = this.soaInfo.GetConduit(i);
				ConduitFlow.ConduitContents contents = conduit.GetContents(this);
				this.serializedIdx[i] = this.soaInfo.GetCell(conduit.idx);
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(contents);
			}
			return;
		}
		this.serializedContents = null;
		this.versionedSerializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x00130074 File Offset: 0x0012E274
	[OnSerialized]
	private void OnSerialized()
	{
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x0013008C File Offset: 0x0012E28C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializedContents != null)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[this.serializedContents.Length];
			for (int i = 0; i < this.serializedContents.Length; i++)
			{
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(this.serializedContents[i]);
			}
			this.serializedContents = null;
		}
		if (this.versionedSerializedContents == null)
		{
			return;
		}
		for (int j = 0; j < this.versionedSerializedContents.Length; j++)
		{
			int num = this.serializedIdx[j];
			ConduitFlow.SerializedContents serializedContents = this.versionedSerializedContents[j];
			ConduitFlow.ConduitContents conduitContents = (serializedContents.mass <= 0f) ? ConduitFlow.ConduitContents.Empty : new ConduitFlow.ConduitContents(serializedContents.element, Math.Min(this.MaxMass, serializedContents.mass), serializedContents.temperature, byte.MaxValue, 0);
			if (0 < serializedContents.diseaseCount || serializedContents.diseaseHash != 0)
			{
				conduitContents.diseaseIdx = Db.Get().Diseases.GetIndex(serializedContents.diseaseHash);
				conduitContents.diseaseCount = ((conduitContents.diseaseIdx == byte.MaxValue) ? 0 : serializedContents.diseaseCount);
			}
			if (float.IsNaN(conduitContents.temperature) || (conduitContents.temperature <= 0f && conduitContents.element != SimHashes.Vacuum) || 10000f < conduitContents.temperature)
			{
				Vector2I vector2I = Grid.CellToXY(num);
				DeserializeWarnings.Instance.PipeContentsTemperatureIsNan.Warn(string.Format("Invalid pipe content temperature of {0} detected. Resetting temperature. (x={1}, y={2}, cell={3})", new object[]
				{
					conduitContents.temperature,
					vector2I.x,
					vector2I.y,
					num
				}), null);
				conduitContents.temperature = ElementLoader.FindElementByHash(conduitContents.element).defaultValues.temperature;
			}
			this.SetContents(num, conduitContents);
		}
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x00130280 File Offset: 0x0012E480
	public UtilityNetwork GetNetwork(ConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x001302AB File Offset: 0x0012E4AB
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x001302B8 File Offset: 0x0012E4B8
	public bool IsConduitFull(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return this.MaxMass - contents.mass <= 0f;
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x001302F0 File Offset: 0x0012E4F0
	public bool IsConduitEmpty(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.mass <= 0f;
	}

	// Token: 0x060037BA RID: 14266 RVA: 0x00130320 File Offset: 0x0012E520
	public void FreezeConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-700727624, null);
		}
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x0013037C File Offset: 0x0012E57C
	public void MeltConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-1152799878, null);
		}
	}

	// Token: 0x04002181 RID: 8577
	public const float MAX_LIQUID_MASS = 10f;

	// Token: 0x04002182 RID: 8578
	public const float MAX_GAS_MASS = 1f;

	// Token: 0x04002183 RID: 8579
	public ConduitType conduitType;

	// Token: 0x04002184 RID: 8580
	private float MaxMass = 10f;

	// Token: 0x04002185 RID: 8581
	private const float PERCENT_MAX_MASS_FOR_STATE_CHANGE_DAMAGE = 0.1f;

	// Token: 0x04002186 RID: 8582
	public const float TickRate = 1f;

	// Token: 0x04002187 RID: 8583
	public const float WaitTime = 1f;

	// Token: 0x04002188 RID: 8584
	private float elapsedTime;

	// Token: 0x04002189 RID: 8585
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x0400218A RID: 8586
	public ConduitFlow.SOAInfo soaInfo = new ConduitFlow.SOAInfo();

	// Token: 0x0400218C RID: 8588
	private bool dirtyConduitUpdaters;

	// Token: 0x0400218D RID: 8589
	private List<ConduitFlow.ConduitUpdater> conduitUpdaters = new List<ConduitFlow.ConduitUpdater>();

	// Token: 0x0400218E RID: 8590
	private ConduitFlow.GridNode[] grid;

	// Token: 0x0400218F RID: 8591
	[Serialize]
	public int[] serializedIdx;

	// Token: 0x04002190 RID: 8592
	[Serialize]
	public ConduitFlow.ConduitContents[] serializedContents;

	// Token: 0x04002191 RID: 8593
	[Serialize]
	public ConduitFlow.SerializedContents[] versionedSerializedContents;

	// Token: 0x04002192 RID: 8594
	private IUtilityNetworkMgr networkMgr;

	// Token: 0x04002193 RID: 8595
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04002194 RID: 8596
	private const int FLOW_DIRECTION_COUNT = 4;

	// Token: 0x04002195 RID: 8597
	private List<ConduitFlow.Network> networks = new List<ConduitFlow.Network>();

	// Token: 0x04002196 RID: 8598
	private WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow> build_network_job = new WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow>();

	// Token: 0x04002197 RID: 8599
	private WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext> connect_job = new WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext>();

	// Token: 0x04002198 RID: 8600
	private WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow> update_networks_job = new WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow>();

	// Token: 0x020016A0 RID: 5792
	[DebuggerDisplay("{NumEntries}")]
	public class SOAInfo
	{
		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06009309 RID: 37641 RVA: 0x003576F9 File Offset: 0x003558F9
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x0600930A RID: 37642 RVA: 0x00357708 File Offset: 0x00355908
		public int AddConduit(ConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			ConduitFlow.Conduit item = new ConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new ConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(ConduitFlow.ConduitFlowInfo.DEFAULT);
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(conduit_go);
			HandleVector<int>.Handle handle2 = Game.Instance.conduitTemperatureManager.Allocate(manager.conduitType, count, handle, ref contents);
			HandleVector<int>.Handle item2 = Game.Instance.conduitDiseaseManager.Allocate(handle2, ref contents);
			this.cells.Add(cell);
			this.diseaseContentsVisible.Add(false);
			this.structureTemperatureHandles.Add(handle);
			this.temperatureHandles.Add(handle2);
			this.diseaseHandles.Add(item2);
			this.conduitGOs.Add(conduit_go);
			this.permittedFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.srcFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.pullDirections.Add(ConduitFlow.FlowDirections.None);
			this.targetFlowDirections.Add(ConduitFlow.FlowDirections.None);
			return count;
		}

		// Token: 0x0600930B RID: 37643 RVA: 0x00357850 File Offset: 0x00355A50
		public void Clear(ConduitFlow manager)
		{
			if (this.clearJob.Count == 0)
			{
				this.clearJob.Reset(this);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishTemperatureToSim>(this.publishTemperatureToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToSim>(this.publishDiseaseToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.ResetConduit>(this.resetConduit);
			}
			this.clearPermanentDiseaseContainer.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToSim.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToSim.Initialize(this.conduits.Count, manager);
			this.resetConduit.Initialize(this.conduits.Count, manager);
			this.clearPermanentDiseaseContainer.Run(this);
			GlobalJobManager.Run(this.clearJob);
			for (int num = 0; num != this.conduits.Count; num++)
			{
				Game.Instance.conduitDiseaseManager.Free(this.diseaseHandles[num]);
			}
			for (int num2 = 0; num2 != this.conduits.Count; num2++)
			{
				Game.Instance.conduitTemperatureManager.Free(this.temperatureHandles[num2]);
			}
			this.cells.Clear();
			this.diseaseContentsVisible.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.pullDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.diseaseHandles.Clear();
			this.temperatureHandles.Clear();
			this.structureTemperatureHandles.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x0600930C RID: 37644 RVA: 0x00357A19 File Offset: 0x00355C19
		public ConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x0600930D RID: 37645 RVA: 0x00357A27 File Offset: 0x00355C27
		public ConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x0600930E RID: 37646 RVA: 0x00357A35 File Offset: 0x00355C35
		public void SetConduitConnections(int idx, ConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x0600930F RID: 37647 RVA: 0x00357A44 File Offset: 0x00355C44
		public float GetConduitTemperature(int idx)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
			global::Debug.Assert(!float.IsNaN(temperature));
			return temperature;
		}

		// Token: 0x06009310 RID: 37648 RVA: 0x00357A7C File Offset: 0x00355C7C
		public void SetConduitTemperatureData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			Game.Instance.conduitTemperatureManager.SetData(handle, ref contents);
		}

		// Token: 0x06009311 RID: 37649 RVA: 0x00357AA8 File Offset: 0x00355CA8
		public ConduitDiseaseManager.Data GetDiseaseData(int idx)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			return Game.Instance.conduitDiseaseManager.GetData(handle);
		}

		// Token: 0x06009312 RID: 37650 RVA: 0x00357AD4 File Offset: 0x00355CD4
		public void SetDiseaseData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			Game.Instance.conduitDiseaseManager.SetData(handle, ref contents);
		}

		// Token: 0x06009313 RID: 37651 RVA: 0x00357AFF File Offset: 0x00355CFF
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x06009314 RID: 37652 RVA: 0x00357B10 File Offset: 0x00355D10
		public void ForcePermanentDiseaseContainer(int idx, bool force_on)
		{
			if (this.diseaseContentsVisible[idx] != force_on)
			{
				this.diseaseContentsVisible[idx] = force_on;
				GameObject gameObject = this.conduitGOs[idx];
				if (gameObject == null)
				{
					return;
				}
				gameObject.GetComponent<PrimaryElement>().ForcePermanentDiseaseContainer(force_on);
			}
		}

		// Token: 0x06009315 RID: 37653 RVA: 0x00357B5C File Offset: 0x00355D5C
		public ConduitFlow.Conduit GetConduitFromDirection(int idx, ConduitFlow.FlowDirections direction)
		{
			ConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case ConduitFlow.FlowDirections.Down:
				if (conduitConnections.down == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.down];
			case ConduitFlow.FlowDirections.Left:
				if (conduitConnections.left == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.left];
			case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
				break;
			case ConduitFlow.FlowDirections.Right:
				if (conduitConnections.right == -1)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.right];
			default:
				if (direction == ConduitFlow.FlowDirections.Up)
				{
					if (conduitConnections.up == -1)
					{
						return ConduitFlow.Conduit.Invalid;
					}
					return this.conduits[conduitConnections.up];
				}
				break;
			}
			return ConduitFlow.Conduit.Invalid;
		}

		// Token: 0x06009316 RID: 37654 RVA: 0x00357C20 File Offset: 0x00355E20
		public void BeginFrame(ConduitFlow manager)
		{
			if (this.beginFrameJob.Count == 0)
			{
				this.beginFrameJob.Reset(this);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InitializeContentsTask>(this.initializeContents);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InvalidateLastFlow>(this.invalidateLastFlow);
			}
			this.initializeContents.Initialize(this.conduits.Count, manager);
			this.invalidateLastFlow.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.beginFrameJob);
		}

		// Token: 0x06009317 RID: 37655 RVA: 0x00357CA4 File Offset: 0x00355EA4
		public void EndFrame(ConduitFlow manager)
		{
			if (this.endFrameJob.Count == 0)
			{
				this.endFrameJob.Reset(this);
				this.endFrameJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToGame>(this.publishDiseaseToGame);
			}
			this.publishTemperatureToGame.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToGame.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToGame.Run(this);
			GlobalJobManager.Run(this.endFrameJob);
		}

		// Token: 0x06009318 RID: 37656 RVA: 0x00357D20 File Offset: 0x00355F20
		public void UpdateFlowDirection(ConduitFlow manager)
		{
			if (this.updateFlowDirectionJob.Count == 0)
			{
				this.updateFlowDirectionJob.Reset(this);
				this.updateFlowDirectionJob.Add<ConduitFlow.SOAInfo.FlowThroughVacuum>(this.flowThroughVacuum);
			}
			this.flowThroughVacuum.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.updateFlowDirectionJob);
		}

		// Token: 0x06009319 RID: 37657 RVA: 0x00357D79 File Offset: 0x00355F79
		public void ResetLastFlowInfo(int idx)
		{
			this.lastFlowInfo[idx] = ConduitFlow.ConduitFlowInfo.DEFAULT;
		}

		// Token: 0x0600931A RID: 37658 RVA: 0x00357D8C File Offset: 0x00355F8C
		public void SetLastFlowInfo(int idx, ConduitFlow.FlowDirections direction, ref ConduitFlow.ConduitContents contents)
		{
			if (this.lastFlowInfo[idx].direction == ConduitFlow.FlowDirections.None)
			{
				this.lastFlowInfo[idx] = new ConduitFlow.ConduitFlowInfo
				{
					direction = direction,
					contents = contents
				};
			}
		}

		// Token: 0x0600931B RID: 37659 RVA: 0x00357DD6 File Offset: 0x00355FD6
		public ConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x0600931C RID: 37660 RVA: 0x00357DE4 File Offset: 0x00355FE4
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x0600931D RID: 37661 RVA: 0x00357DF2 File Offset: 0x00355FF2
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x0600931E RID: 37662 RVA: 0x00357E00 File Offset: 0x00356000
		public void SetPermittedFlowDirections(int idx, ConduitFlow.FlowDirections permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x0600931F RID: 37663 RVA: 0x00357E10 File Offset: 0x00356010
		public ConduitFlow.FlowDirections AddPermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] |= delta;
		}

		// Token: 0x06009320 RID: 37664 RVA: 0x00357E3C File Offset: 0x0035603C
		public ConduitFlow.FlowDirections RemovePermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] &= ~delta;
		}

		// Token: 0x06009321 RID: 37665 RVA: 0x00357E67 File Offset: 0x00356067
		public ConduitFlow.FlowDirections GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x06009322 RID: 37666 RVA: 0x00357E75 File Offset: 0x00356075
		public void SetTargetFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x06009323 RID: 37667 RVA: 0x00357E84 File Offset: 0x00356084
		public ConduitFlow.FlowDirections GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x06009324 RID: 37668 RVA: 0x00357E92 File Offset: 0x00356092
		public void SetSrcFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x06009325 RID: 37669 RVA: 0x00357EA1 File Offset: 0x003560A1
		public ConduitFlow.FlowDirections GetPullDirection(int idx)
		{
			return this.pullDirections[idx];
		}

		// Token: 0x06009326 RID: 37670 RVA: 0x00357EAF File Offset: 0x003560AF
		public void SetPullDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.pullDirections[idx] = directions;
		}

		// Token: 0x06009327 RID: 37671 RVA: 0x00357EBE File Offset: 0x003560BE
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x06009328 RID: 37672 RVA: 0x00357ECC File Offset: 0x003560CC
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x0400703A RID: 28730
		private List<ConduitFlow.Conduit> conduits = new List<ConduitFlow.Conduit>();

		// Token: 0x0400703B RID: 28731
		private List<ConduitFlow.ConduitConnections> conduitConnections = new List<ConduitFlow.ConduitConnections>();

		// Token: 0x0400703C RID: 28732
		private List<ConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<ConduitFlow.ConduitFlowInfo>();

		// Token: 0x0400703D RID: 28733
		private List<ConduitFlow.ConduitContents> initialContents = new List<ConduitFlow.ConduitContents>();

		// Token: 0x0400703E RID: 28734
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x0400703F RID: 28735
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x04007040 RID: 28736
		private List<int> cells = new List<int>();

		// Token: 0x04007041 RID: 28737
		private List<ConduitFlow.FlowDirections> permittedFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007042 RID: 28738
		private List<ConduitFlow.FlowDirections> srcFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007043 RID: 28739
		private List<ConduitFlow.FlowDirections> pullDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007044 RID: 28740
		private List<ConduitFlow.FlowDirections> targetFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04007045 RID: 28741
		private List<HandleVector<int>.Handle> structureTemperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04007046 RID: 28742
		private List<HandleVector<int>.Handle> temperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04007047 RID: 28743
		private List<HandleVector<int>.Handle> diseaseHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04007048 RID: 28744
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer> clearPermanentDiseaseContainer = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer>();

		// Token: 0x04007049 RID: 28745
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim> publishTemperatureToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim>();

		// Token: 0x0400704A RID: 28746
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim> publishDiseaseToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim>();

		// Token: 0x0400704B RID: 28747
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit> resetConduit = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit>();

		// Token: 0x0400704C RID: 28748
		private ConduitFlow.SOAInfo.ConduitJob clearJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x0400704D RID: 28749
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask> initializeContents = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask>();

		// Token: 0x0400704E RID: 28750
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow> invalidateLastFlow = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow>();

		// Token: 0x0400704F RID: 28751
		private ConduitFlow.SOAInfo.ConduitJob beginFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04007050 RID: 28752
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame> publishTemperatureToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame>();

		// Token: 0x04007051 RID: 28753
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame> publishDiseaseToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame>();

		// Token: 0x04007052 RID: 28754
		private ConduitFlow.SOAInfo.ConduitJob endFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04007053 RID: 28755
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum> flowThroughVacuum = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum>();

		// Token: 0x04007054 RID: 28756
		private ConduitFlow.SOAInfo.ConduitJob updateFlowDirectionJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x02002565 RID: 9573
		private abstract class ConduitTask : DivisibleTask<ConduitFlow.SOAInfo>
		{
			// Token: 0x0600BEAE RID: 48814 RVA: 0x003D9687 File Offset: 0x003D7887
			public ConduitTask(string name) : base(name)
			{
			}

			// Token: 0x0400A6B4 RID: 42676
			public ConduitFlow manager;
		}

		// Token: 0x02002566 RID: 9574
		private class ConduitTaskDivision<Task> : TaskDivision<Task, ConduitFlow.SOAInfo> where Task : ConduitFlow.SOAInfo.ConduitTask, new()
		{
			// Token: 0x0600BEAF RID: 48815 RVA: 0x003D9690 File Offset: 0x003D7890
			public void Initialize(int conduitCount, ConduitFlow manager)
			{
				base.Initialize(conduitCount);
				Task[] tasks = this.tasks;
				for (int i = 0; i < tasks.Length; i++)
				{
					tasks[i].manager = manager;
				}
			}
		}

		// Token: 0x02002567 RID: 9575
		private class ConduitJob : WorkItemCollection<ConduitFlow.SOAInfo.ConduitTask, ConduitFlow.SOAInfo>
		{
			// Token: 0x0600BEB1 RID: 48817 RVA: 0x003D96D4 File Offset: 0x003D78D4
			public void Add<Task>(ConduitFlow.SOAInfo.ConduitTaskDivision<Task> taskDivision) where Task : ConduitFlow.SOAInfo.ConduitTask, new()
			{
				foreach (Task task in taskDivision.tasks)
				{
					base.Add(task);
				}
			}
		}

		// Token: 0x02002568 RID: 9576
		private class ClearPermanentDiseaseContainer : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEB3 RID: 48819 RVA: 0x003D9712 File Offset: 0x003D7912
			public ClearPermanentDiseaseContainer() : base("ClearPermanentDiseaseContainer")
			{
			}

			// Token: 0x0600BEB4 RID: 48820 RVA: 0x003D9720 File Offset: 0x003D7920
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.ForcePermanentDiseaseContainer(num, false);
				}
			}
		}

		// Token: 0x02002569 RID: 9577
		private class PublishTemperatureToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEB5 RID: 48821 RVA: 0x003D974B File Offset: 0x003D794B
			public PublishTemperatureToSim() : base("PublishTemperatureToSim")
			{
			}

			// Token: 0x0600BEB6 RID: 48822 RVA: 0x003D9758 File Offset: 0x003D7958
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.temperatureHandles[num];
					if (handle.IsValid())
					{
						float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
						this.manager.grid[soaInfo.cells[num]].contents.temperature = temperature;
					}
				}
			}
		}

		// Token: 0x0200256A RID: 9578
		private class PublishDiseaseToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEB7 RID: 48823 RVA: 0x003D97C9 File Offset: 0x003D79C9
			public PublishDiseaseToSim() : base("PublishDiseaseToSim")
			{
			}

			// Token: 0x0600BEB8 RID: 48824 RVA: 0x003D97D8 File Offset: 0x003D79D8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.diseaseHandles[num];
					if (handle.IsValid())
					{
						ConduitDiseaseManager.Data data = Game.Instance.conduitDiseaseManager.GetData(handle);
						int num2 = soaInfo.cells[num];
						this.manager.grid[num2].contents.diseaseIdx = data.diseaseIdx;
						this.manager.grid[num2].contents.diseaseCount = data.diseaseCount;
					}
				}
			}
		}

		// Token: 0x0200256B RID: 9579
		private class ResetConduit : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEB9 RID: 48825 RVA: 0x003D9874 File Offset: 0x003D7A74
			public ResetConduit() : base("ResetConduitTask")
			{
			}

			// Token: 0x0600BEBA RID: 48826 RVA: 0x003D9884 File Offset: 0x003D7A84
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					this.manager.grid[soaInfo.cells[num]].conduitIdx = -1;
				}
			}
		}

		// Token: 0x0200256C RID: 9580
		private class InitializeContentsTask : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEBB RID: 48827 RVA: 0x003D98C9 File Offset: 0x003D7AC9
			public InitializeContentsTask() : base("SetInitialContents")
			{
			}

			// Token: 0x0600BEBC RID: 48828 RVA: 0x003D98D8 File Offset: 0x003D7AD8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					int num2 = soaInfo.cells[num];
					ConduitFlow.ConduitContents conduitContents = soaInfo.conduits[num].GetContents(this.manager);
					if (conduitContents.mass <= 0f)
					{
						conduitContents = ConduitFlow.ConduitContents.Empty;
					}
					soaInfo.initialContents[num] = conduitContents;
					this.manager.grid[num2].contents = conduitContents;
				}
			}
		}

		// Token: 0x0200256D RID: 9581
		private class InvalidateLastFlow : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEBD RID: 48829 RVA: 0x003D995B File Offset: 0x003D7B5B
			public InvalidateLastFlow() : base("InvalidateLastFlow")
			{
			}

			// Token: 0x0600BEBE RID: 48830 RVA: 0x003D9968 File Offset: 0x003D7B68
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.lastFlowInfo[num] = ConduitFlow.ConduitFlowInfo.DEFAULT;
				}
			}
		}

		// Token: 0x0200256E RID: 9582
		private class PublishTemperatureToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEBF RID: 48831 RVA: 0x003D999C File Offset: 0x003D7B9C
			public PublishTemperatureToGame() : base("PublishTemperatureToGame")
			{
			}

			// Token: 0x0600BEC0 RID: 48832 RVA: 0x003D99AC File Offset: 0x003D7BAC
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitTemperatureManager.SetData(soaInfo.temperatureHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x0200256F RID: 9583
		private class PublishDiseaseToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEC1 RID: 48833 RVA: 0x003D9A0B File Offset: 0x003D7C0B
			public PublishDiseaseToGame() : base("PublishDiseaseToGame")
			{
			}

			// Token: 0x0600BEC2 RID: 48834 RVA: 0x003D9A18 File Offset: 0x003D7C18
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitDiseaseManager.SetData(soaInfo.diseaseHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x02002570 RID: 9584
		private class FlowThroughVacuum : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x0600BEC3 RID: 48835 RVA: 0x003D9A77 File Offset: 0x003D7C77
			public FlowThroughVacuum() : base("FlowThroughVacuum")
			{
			}

			// Token: 0x0600BEC4 RID: 48836 RVA: 0x003D9A84 File Offset: 0x003D7C84
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					ConduitFlow.Conduit conduit = soaInfo.conduits[num];
					int cell = conduit.GetCell(this.manager);
					if (this.manager.grid[cell].contents.element == SimHashes.Vacuum)
					{
						soaInfo.srcFlowDirections[conduit.idx] = ConduitFlow.FlowDirections.None;
					}
				}
			}
		}
	}

	// Token: 0x020016A1 RID: 5793
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x04007055 RID: 28757
		public ConduitFlowPriority priority;

		// Token: 0x04007056 RID: 28758
		public Action<float> callback;
	}

	// Token: 0x020016A2 RID: 5794
	[DebuggerDisplay("conduit {conduitIdx}:{contents.element}")]
	public struct GridNode
	{
		// Token: 0x04007057 RID: 28759
		public int conduitIdx;

		// Token: 0x04007058 RID: 28760
		public ConduitFlow.ConduitContents contents;
	}

	// Token: 0x020016A3 RID: 5795
	public struct SerializedContents
	{
		// Token: 0x0600932A RID: 37674 RVA: 0x00358018 File Offset: 0x00356218
		public SerializedContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			this.element = element;
			this.mass = mass;
			this.temperature = temperature;
			this.diseaseHash = ((disease_idx != byte.MaxValue) ? Db.Get().Diseases[(int)disease_idx].id.GetHashCode() : 0);
			this.diseaseCount = disease_count;
			if (this.diseaseCount <= 0)
			{
				this.diseaseHash = 0;
			}
		}

		// Token: 0x0600932B RID: 37675 RVA: 0x00358085 File Offset: 0x00356285
		public SerializedContents(ConduitFlow.ConduitContents src)
		{
			this = new ConduitFlow.SerializedContents(src.element, src.mass, src.temperature, src.diseaseIdx, src.diseaseCount);
		}

		// Token: 0x04007059 RID: 28761
		public SimHashes element;

		// Token: 0x0400705A RID: 28762
		public float mass;

		// Token: 0x0400705B RID: 28763
		public float temperature;

		// Token: 0x0400705C RID: 28764
		public int diseaseHash;

		// Token: 0x0400705D RID: 28765
		public int diseaseCount;
	}

	// Token: 0x020016A4 RID: 5796
	[Flags]
	public enum FlowDirections : byte
	{
		// Token: 0x0400705F RID: 28767
		None = 0,
		// Token: 0x04007060 RID: 28768
		Down = 1,
		// Token: 0x04007061 RID: 28769
		Left = 2,
		// Token: 0x04007062 RID: 28770
		Right = 4,
		// Token: 0x04007063 RID: 28771
		Up = 8,
		// Token: 0x04007064 RID: 28772
		All = 15
	}

	// Token: 0x020016A5 RID: 5797
	[DebuggerDisplay("conduits l:{left}, r:{right}, u:{up}, d:{down}")]
	public struct ConduitConnections
	{
		// Token: 0x04007065 RID: 28773
		public int left;

		// Token: 0x04007066 RID: 28774
		public int right;

		// Token: 0x04007067 RID: 28775
		public int up;

		// Token: 0x04007068 RID: 28776
		public int down;

		// Token: 0x04007069 RID: 28777
		public static readonly ConduitFlow.ConduitConnections DEFAULT = new ConduitFlow.ConduitConnections
		{
			left = -1,
			right = -1,
			up = -1,
			down = -1
		};
	}

	// Token: 0x020016A6 RID: 5798
	[DebuggerDisplay("{direction}:{contents.element}")]
	public struct ConduitFlowInfo
	{
		// Token: 0x0400706A RID: 28778
		public ConduitFlow.FlowDirections direction;

		// Token: 0x0400706B RID: 28779
		public ConduitFlow.ConduitContents contents;

		// Token: 0x0400706C RID: 28780
		public static readonly ConduitFlow.ConduitFlowInfo DEFAULT = new ConduitFlow.ConduitFlowInfo
		{
			direction = ConduitFlow.FlowDirections.None,
			contents = ConduitFlow.ConduitContents.Empty
		};
	}

	// Token: 0x020016A7 RID: 5799
	[DebuggerDisplay("conduit {idx}")]
	[Serializable]
	public struct Conduit : IEquatable<ConduitFlow.Conduit>
	{
		// Token: 0x0600932E RID: 37678 RVA: 0x00358117 File Offset: 0x00356317
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x0600932F RID: 37679 RVA: 0x00358120 File Offset: 0x00356320
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(ConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x06009330 RID: 37680 RVA: 0x00358133 File Offset: 0x00356333
		public void SetPermittedFlowDirections(ConduitFlow.FlowDirections permitted, ConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x06009331 RID: 37681 RVA: 0x00358147 File Offset: 0x00356347
		public ConduitFlow.FlowDirections GetTargetFlowDirection(ConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x06009332 RID: 37682 RVA: 0x0035815A File Offset: 0x0035635A
		public void SetTargetFlowDirection(ConduitFlow.FlowDirections directions, ConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x06009333 RID: 37683 RVA: 0x00358170 File Offset: 0x00356370
		public ConduitFlow.ConduitContents GetContents(ConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			contents.temperature = soaInfo.GetConduitTemperature(this.idx);
			ConduitDiseaseManager.Data diseaseData = soaInfo.GetDiseaseData(this.idx);
			contents.diseaseIdx = diseaseData.diseaseIdx;
			contents.diseaseCount = diseaseData.diseaseCount;
			return contents;
		}

		// Token: 0x06009334 RID: 37684 RVA: 0x003581E4 File Offset: 0x003563E4
		public void SetContents(ConduitFlow manager, ConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			soaInfo.SetConduitTemperatureData(this.idx, ref contents);
			soaInfo.ForcePermanentDiseaseContainer(this.idx, contents.diseaseIdx != byte.MaxValue);
			soaInfo.SetDiseaseData(this.idx, ref contents);
		}

		// Token: 0x06009335 RID: 37685 RVA: 0x00358252 File Offset: 0x00356452
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(ConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x06009336 RID: 37686 RVA: 0x00358265 File Offset: 0x00356465
		public ConduitFlow.ConduitContents GetInitialContents(ConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x06009337 RID: 37687 RVA: 0x00358278 File Offset: 0x00356478
		public int GetCell(ConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x06009338 RID: 37688 RVA: 0x0035828B File Offset: 0x0035648B
		public bool Equals(ConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x0400706D RID: 28781
		public static readonly ConduitFlow.Conduit Invalid = new ConduitFlow.Conduit(-1);

		// Token: 0x0400706E RID: 28782
		public readonly int idx;
	}

	// Token: 0x020016A8 RID: 5800
	[DebuggerDisplay("{element} M:{mass} T:{temperature}")]
	public struct ConduitContents
	{
		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x0600933A RID: 37690 RVA: 0x003582A8 File Offset: 0x003564A8
		public float mass
		{
			get
			{
				return this.initial_mass + this.added_mass - this.removed_mass;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x0600933B RID: 37691 RVA: 0x003582BE File Offset: 0x003564BE
		public float movable_mass
		{
			get
			{
				return this.initial_mass - this.removed_mass;
			}
		}

		// Token: 0x0600933C RID: 37692 RVA: 0x003582D0 File Offset: 0x003564D0
		public ConduitContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			global::Debug.Assert(!float.IsNaN(temperature));
			this.element = element;
			this.initial_mass = mass;
			this.added_mass = 0f;
			this.removed_mass = 0f;
			this.temperature = temperature;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x0600933D RID: 37693 RVA: 0x00358326 File Offset: 0x00356526
		public void ConsolidateMass()
		{
			this.initial_mass += this.added_mass;
			this.added_mass = 0f;
			this.initial_mass -= this.removed_mass;
			this.removed_mass = 0f;
		}

		// Token: 0x0600933E RID: 37694 RVA: 0x00358364 File Offset: 0x00356564
		public float GetEffectiveCapacity(float maximum_capacity)
		{
			float mass = this.mass;
			return Mathf.Max(0f, maximum_capacity - mass);
		}

		// Token: 0x0600933F RID: 37695 RVA: 0x00358385 File Offset: 0x00356585
		public void AddMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			this.added_mass += amount;
		}

		// Token: 0x06009340 RID: 37696 RVA: 0x003583A8 File Offset: 0x003565A8
		public float RemoveMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			float result = 0f;
			float num = this.mass - amount;
			if (num < 0f)
			{
				amount += num;
				result = -num;
				global::Debug.Assert(false);
			}
			this.removed_mass += amount;
			return result;
		}

		// Token: 0x0400706F RID: 28783
		public SimHashes element;

		// Token: 0x04007070 RID: 28784
		private float initial_mass;

		// Token: 0x04007071 RID: 28785
		private float added_mass;

		// Token: 0x04007072 RID: 28786
		private float removed_mass;

		// Token: 0x04007073 RID: 28787
		public float temperature;

		// Token: 0x04007074 RID: 28788
		public byte diseaseIdx;

		// Token: 0x04007075 RID: 28789
		public int diseaseCount;

		// Token: 0x04007076 RID: 28790
		public static readonly ConduitFlow.ConduitContents Empty = new ConduitFlow.ConduitContents
		{
			element = SimHashes.Vacuum,
			initial_mass = 0f,
			added_mass = 0f,
			removed_mass = 0f,
			temperature = 0f,
			diseaseIdx = byte.MaxValue,
			diseaseCount = 0
		};
	}

	// Token: 0x020016A9 RID: 5801
	[DebuggerDisplay("{network.ConduitType}:{cells.Count}")]
	private struct Network
	{
		// Token: 0x04007077 RID: 28791
		public List<int> cells;

		// Token: 0x04007078 RID: 28792
		public FlowUtilityNetwork network;
	}

	// Token: 0x020016AA RID: 5802
	private struct BuildNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x06009342 RID: 37698 RVA: 0x00358468 File Offset: 0x00356668
		public BuildNetworkTask(ConduitFlow.Network network, int conduit_count)
		{
			this.network = network;
			this.distance_nodes = QueuePool<ConduitFlow.BuildNetworkTask.DistanceNode, ConduitFlow>.Allocate();
			this.distances_via_sources = DictionaryPool<int, int, ConduitFlow>.Allocate();
			this.from_sources = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.distances_via_sinks = DictionaryPool<int, int, ConduitFlow>.Allocate();
			this.from_sinks = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.from_sources_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
			this.from_sinks_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
		}

		// Token: 0x06009343 RID: 37699 RVA: 0x003584D8 File Offset: 0x003566D8
		public void Finish()
		{
			this.distances_via_sinks.Recycle();
			this.distances_via_sources.Recycle();
			this.distance_nodes.Recycle();
			this.from_sources.Recycle();
			this.from_sinks.Recycle();
			this.from_sources_graph.Recycle();
			this.from_sinks_graph.Recycle();
		}

		// Token: 0x06009344 RID: 37700 RVA: 0x00358534 File Offset: 0x00356734
		private void ComputeFlow(ConduitFlow outer)
		{
			this.from_sources_graph.Build(outer, this.network.network.sources, this.network.network.sinks, true);
			this.from_sinks_graph.Build(outer, this.network.network.sinks, this.network.network.sources, false);
			this.from_sources_graph.Merge(this.from_sinks_graph);
			this.from_sources_graph.BreakCycles();
			this.from_sources_graph.WriteFlow(false);
			this.from_sinks_graph.WriteFlow(true);
		}

		// Token: 0x06009345 RID: 37701 RVA: 0x003585D0 File Offset: 0x003567D0
		private void ComputeOrder(ConduitFlow outer)
		{
			foreach (int cell in this.from_sources_graph.sources)
			{
				this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
				{
					cell = cell,
					distance = 0
				});
			}
			using (HashSet<int>.Enumerator enumerator = this.from_sources_graph.dead_ends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell2 = enumerator.Current;
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = cell2,
						distance = 0
					});
				}
				goto IL_21D;
			}
			IL_B3:
			ConduitFlow.BuildNetworkTask.DistanceNode distanceNode = this.distance_nodes.Dequeue();
			int conduitIdx = outer.grid[distanceNode.cell].conduitIdx;
			if (conduitIdx != -1)
			{
				this.distances_via_sources[distanceNode.cell] = distanceNode.distance;
				ConduitFlow.ConduitConnections conduitConnections = outer.soaInfo.GetConduitConnections(conduitIdx);
				ConduitFlow.FlowDirections permittedFlowDirections = outer.soaInfo.GetPermittedFlowDirections(conduitIdx);
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.up),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.down),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.left),
						distance = distanceNode.distance + 1
					});
				}
				if ((permittedFlowDirections & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections.right),
						distance = distanceNode.distance + 1
					});
				}
			}
			IL_21D:
			if (this.distance_nodes.Count != 0)
			{
				goto IL_B3;
			}
			this.from_sources.AddRange(this.distances_via_sources);
			this.from_sources.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => b.Value - a.Value);
			this.distance_nodes.Clear();
			foreach (int cell3 in this.from_sinks_graph.sources)
			{
				this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
				{
					cell = cell3,
					distance = 0
				});
			}
			using (HashSet<int>.Enumerator enumerator = this.from_sinks_graph.dead_ends.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell4 = enumerator.Current;
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = cell4,
						distance = 0
					});
				}
				goto IL_508;
			}
			IL_32A:
			ConduitFlow.BuildNetworkTask.DistanceNode distanceNode2 = this.distance_nodes.Dequeue();
			int conduitIdx2 = outer.grid[distanceNode2.cell].conduitIdx;
			if (conduitIdx2 != -1)
			{
				if (!this.distances_via_sources.ContainsKey(distanceNode2.cell))
				{
					this.distances_via_sinks[distanceNode2.cell] = distanceNode2.distance;
				}
				ConduitFlow.ConduitConnections conduitConnections2 = outer.soaInfo.GetConduitConnections(conduitIdx2);
				if (conduitConnections2.up != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.up) & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.up),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.down != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.down) & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.down),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.left != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.left) & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.left),
						distance = distanceNode2.distance + 1
					});
				}
				if (conduitConnections2.right != -1 && (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.right) & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
				{
					this.distance_nodes.Enqueue(new ConduitFlow.BuildNetworkTask.DistanceNode
					{
						cell = outer.soaInfo.GetCell(conduitConnections2.right),
						distance = distanceNode2.distance + 1
					});
				}
			}
			IL_508:
			if (this.distance_nodes.Count == 0)
			{
				this.from_sinks.AddRange(this.distances_via_sinks);
				this.from_sinks.Sort((KeyValuePair<int, int> a, KeyValuePair<int, int> b) => a.Value - b.Value);
				this.network.cells.Capacity = Mathf.Max(this.network.cells.Capacity, this.from_sources.Count + this.from_sinks.Count);
				foreach (KeyValuePair<int, int> keyValuePair in this.from_sources)
				{
					this.network.cells.Add(keyValuePair.Key);
				}
				foreach (KeyValuePair<int, int> keyValuePair2 in this.from_sinks)
				{
					this.network.cells.Add(keyValuePair2.Key);
				}
				return;
			}
			goto IL_32A;
		}

		// Token: 0x06009346 RID: 37702 RVA: 0x00358C48 File Offset: 0x00356E48
		public void Run(ConduitFlow outer)
		{
			this.ComputeFlow(outer);
			this.ComputeOrder(outer);
		}

		// Token: 0x04007079 RID: 28793
		private ConduitFlow.Network network;

		// Token: 0x0400707A RID: 28794
		private QueuePool<ConduitFlow.BuildNetworkTask.DistanceNode, ConduitFlow>.PooledQueue distance_nodes;

		// Token: 0x0400707B RID: 28795
		private DictionaryPool<int, int, ConduitFlow>.PooledDictionary distances_via_sources;

		// Token: 0x0400707C RID: 28796
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sources;

		// Token: 0x0400707D RID: 28797
		private DictionaryPool<int, int, ConduitFlow>.PooledDictionary distances_via_sinks;

		// Token: 0x0400707E RID: 28798
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sinks;

		// Token: 0x0400707F RID: 28799
		private ConduitFlow.BuildNetworkTask.Graph from_sources_graph;

		// Token: 0x04007080 RID: 28800
		private ConduitFlow.BuildNetworkTask.Graph from_sinks_graph;

		// Token: 0x02002571 RID: 9585
		[DebuggerDisplay("cell {cell}:{distance}")]
		private struct DistanceNode
		{
			// Token: 0x0400A6B5 RID: 42677
			public int cell;

			// Token: 0x0400A6B6 RID: 42678
			public int distance;
		}

		// Token: 0x02002572 RID: 9586
		[DebuggerDisplay("vertices:{vertex_cells.Count}, edges:{edges.Count}")]
		private struct Graph
		{
			// Token: 0x0600BEC5 RID: 48837 RVA: 0x003D9AF8 File Offset: 0x003D7CF8
			public Graph(FlowUtilityNetwork network)
			{
				this.conduit_flow = null;
				this.vertex_cells = HashSetPool<int, ConduitFlow>.Allocate();
				this.edges = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.cycles = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.bfs_traversal = QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.visited = HashSetPool<int, ConduitFlow>.Allocate();
				this.pseudo_sources = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.sources = HashSetPool<int, ConduitFlow>.Allocate();
				this.sinks = HashSetPool<int, ConduitFlow>.Allocate();
				this.dfs_path = HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dfs_traversal = ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dead_ends = HashSetPool<int, ConduitFlow>.Allocate();
				this.cycle_vertices = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
			}

			// Token: 0x0600BEC6 RID: 48838 RVA: 0x003D9B90 File Offset: 0x003D7D90
			public void Recycle()
			{
				this.vertex_cells.Recycle();
				this.edges.Recycle();
				this.cycles.Recycle();
				this.bfs_traversal.Recycle();
				this.visited.Recycle();
				this.pseudo_sources.Recycle();
				this.sources.Recycle();
				this.sinks.Recycle();
				this.dfs_path.Recycle();
				this.dfs_traversal.Recycle();
				this.dead_ends.Recycle();
				this.cycle_vertices.Recycle();
			}

			// Token: 0x0600BEC7 RID: 48839 RVA: 0x003D9C24 File Offset: 0x003D7E24
			public void Build(ConduitFlow conduit_flow, List<FlowUtilityNetwork.IItem> sources, List<FlowUtilityNetwork.IItem> sinks, bool are_dead_ends_pseudo_sources)
			{
				this.conduit_flow = conduit_flow;
				this.sources.Clear();
				for (int i = 0; i < sources.Count; i++)
				{
					int cell = sources[i].Cell;
					if (conduit_flow.grid[cell].conduitIdx != -1)
					{
						this.sources.Add(cell);
					}
				}
				this.sinks.Clear();
				for (int j = 0; j < sinks.Count; j++)
				{
					int cell2 = sinks[j].Cell;
					if (conduit_flow.grid[cell2].conduitIdx != -1)
					{
						this.sinks.Add(cell2);
					}
				}
				global::Debug.Assert(this.bfs_traversal.Count == 0);
				this.visited.Clear();
				foreach (int num in this.sources)
				{
					this.bfs_traversal.Enqueue(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = num,
						direction = ConduitFlow.FlowDirections.None
					});
					this.visited.Add(num);
				}
				this.pseudo_sources.Clear();
				this.dead_ends.Clear();
				this.cycles.Clear();
				while (this.bfs_traversal.Count != 0)
				{
					ConduitFlow.BuildNetworkTask.Graph.Vertex node = this.bfs_traversal.Dequeue();
					this.vertex_cells.Add(node.cell);
					ConduitFlow.FlowDirections flowDirections = ConduitFlow.FlowDirections.None;
					int num2 = 4;
					if (node.direction != ConduitFlow.FlowDirections.None)
					{
						flowDirections = ConduitFlow.Opposite(node.direction);
						num2 = 3;
					}
					int conduitIdx = conduit_flow.grid[node.cell].conduitIdx;
					for (int num3 = 0; num3 != num2; num3++)
					{
						flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
						ConduitFlow.Conduit conduitFromDirection = conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, flowDirections);
						ConduitFlow.BuildNetworkTask.Graph.Vertex new_node = this.WalkPath(conduitIdx, conduitFromDirection.idx, flowDirections, are_dead_ends_pseudo_sources);
						if (new_node.is_valid)
						{
							ConduitFlow.BuildNetworkTask.Graph.Edge item = new ConduitFlow.BuildNetworkTask.Graph.Edge
							{
								vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
								{
									new ConduitFlow.BuildNetworkTask.Graph.Vertex
									{
										cell = node.cell,
										direction = flowDirections
									},
									new_node
								}
							};
							if (new_node.cell == node.cell)
							{
								this.cycles.Add(item);
							}
							else if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == new_node.cell && edge.vertices[1].cell == node.cell) && !this.edges.Contains(item))
							{
								this.edges.Add(item);
								if (this.visited.Add(new_node.cell))
								{
									if (this.IsSink(new_node.cell))
									{
										this.pseudo_sources.Add(new_node);
									}
									else
									{
										this.bfs_traversal.Enqueue(new_node);
									}
								}
							}
						}
					}
					if (this.bfs_traversal.Count == 0)
					{
						foreach (ConduitFlow.BuildNetworkTask.Graph.Vertex item2 in this.pseudo_sources)
						{
							this.bfs_traversal.Enqueue(item2);
						}
						this.pseudo_sources.Clear();
					}
				}
			}

			// Token: 0x0600BEC8 RID: 48840 RVA: 0x003D9FF0 File Offset: 0x003D81F0
			private bool IsEndpoint(int cell)
			{
				global::Debug.Assert(cell != -1);
				return this.conduit_flow.grid[cell].conduitIdx == -1 || this.sources.Contains(cell) || this.sinks.Contains(cell) || this.dead_ends.Contains(cell);
			}

			// Token: 0x0600BEC9 RID: 48841 RVA: 0x003DA04C File Offset: 0x003D824C
			private bool IsSink(int cell)
			{
				return this.sinks.Contains(cell);
			}

			// Token: 0x0600BECA RID: 48842 RVA: 0x003DA05C File Offset: 0x003D825C
			private bool IsJunction(int cell)
			{
				global::Debug.Assert(cell != -1);
				ConduitFlow.GridNode gridNode = this.conduit_flow.grid[cell];
				global::Debug.Assert(gridNode.conduitIdx != -1);
				ConduitFlow.ConduitConnections conduitConnections = this.conduit_flow.soaInfo.GetConduitConnections(gridNode.conduitIdx);
				return 2 < this.JunctionValue(conduitConnections.down) + this.JunctionValue(conduitConnections.left) + this.JunctionValue(conduitConnections.up) + this.JunctionValue(conduitConnections.right);
			}

			// Token: 0x0600BECB RID: 48843 RVA: 0x003DA0E5 File Offset: 0x003D82E5
			private int JunctionValue(int conduit)
			{
				if (conduit != -1)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x0600BECC RID: 48844 RVA: 0x003DA0F0 File Offset: 0x003D82F0
			private ConduitFlow.BuildNetworkTask.Graph.Vertex WalkPath(int root_conduit, int conduit, ConduitFlow.FlowDirections direction, bool are_dead_ends_pseudo_sources)
			{
				if (conduit == -1)
				{
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				int cell;
				for (;;)
				{
					cell = this.conduit_flow.soaInfo.GetCell(conduit);
					if (this.IsEndpoint(cell) || this.IsJunction(cell))
					{
						break;
					}
					direction = ConduitFlow.Opposite(direction);
					bool flag = true;
					for (int num = 0; num != 3; num++)
					{
						direction = ConduitFlow.ComputeNextFlowDirection(direction);
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduit, direction);
						if (conduitFromDirection.idx != -1)
						{
							conduit = conduitFromDirection.idx;
							flag = false;
							break;
						}
					}
					if (flag)
					{
						goto Block_4;
					}
				}
				return new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					cell = cell,
					direction = direction
				};
				Block_4:
				if (are_dead_ends_pseudo_sources)
				{
					this.pseudo_sources.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = cell,
						direction = ConduitFlow.ComputeNextFlowDirection(direction)
					});
					this.dead_ends.Add(cell);
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				ConduitFlow.BuildNetworkTask.Graph.Vertex result = default(ConduitFlow.BuildNetworkTask.Graph.Vertex);
				result.cell = cell;
				direction = (result.direction = ConduitFlow.Opposite(ConduitFlow.ComputeNextFlowDirection(direction)));
				return result;
			}

			// Token: 0x0600BECD RID: 48845 RVA: 0x003DA1FC File Offset: 0x003D83FC
			public void Merge(ConduitFlow.BuildNetworkTask.Graph inverted_graph)
			{
				using (List<ConduitFlow.BuildNetworkTask.Graph.Edge>.Enumerator enumerator = inverted_graph.edges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge = enumerator.Current;
						ConduitFlow.BuildNetworkTask.Graph.Edge candidate = inverted_edge.Invert();
						if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.Equals(inverted_edge) || edge.Equals(candidate)))
						{
							this.edges.Add(candidate);
							this.vertex_cells.Add(candidate.vertices[0].cell);
							this.vertex_cells.Add(candidate.vertices[1].cell);
						}
					}
				}
				int num = 1000;
				for (int num2 = 0; num2 != num; num2++)
				{
					global::Debug.Assert(num2 != num - 1);
					bool flag = false;
					using (HashSet<int>.Enumerator enumerator2 = this.vertex_cells.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int cell = enumerator2.Current;
							if (!this.IsSink(cell) && !this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == cell))
							{
								int num3 = inverted_graph.edges.FindIndex((ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge) => inverted_edge.vertices[1].cell == cell);
								if (num3 != -1)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge3 = inverted_graph.edges[num3];
									for (int num4 = 0; num4 != this.edges.Count; num4++)
									{
										ConduitFlow.BuildNetworkTask.Graph.Edge edge2 = this.edges[num4];
										if (edge2.vertices[0].cell == edge3.vertices[0].cell && edge2.vertices[1].cell == edge3.vertices[1].cell)
										{
											this.edges[num4] = edge2.Invert();
										}
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}

			// Token: 0x0600BECE RID: 48846 RVA: 0x003DA460 File Offset: 0x003D8660
			public void BreakCycles()
			{
				this.visited.Clear();
				foreach (int num in this.vertex_cells)
				{
					if (!this.visited.Contains(num))
					{
						this.dfs_path.Clear();
						this.dfs_traversal.Clear();
						this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
						{
							cell = num,
							parent = null
						});
						while (this.dfs_traversal.Count != 0)
						{
							ConduitFlow.BuildNetworkTask.Graph.DFSNode dfsnode = this.dfs_traversal[this.dfs_traversal.Count - 1];
							this.dfs_traversal.RemoveAt(this.dfs_traversal.Count - 1);
							bool flag = false;
							for (ConduitFlow.BuildNetworkTask.Graph.DFSNode parent = dfsnode.parent; parent != null; parent = parent.parent)
							{
								if (parent.cell == dfsnode.cell)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								for (int num2 = this.edges.Count - 1; num2 != -1; num2--)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge = this.edges[num2];
									if (edge.vertices[0].cell == dfsnode.parent.cell && edge.vertices[1].cell == dfsnode.cell)
									{
										this.cycles.Add(edge);
										this.edges.RemoveAt(num2);
									}
								}
							}
							else if (this.visited.Add(dfsnode.cell))
							{
								foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.edges)
								{
									if (edge2.vertices[0].cell == dfsnode.cell)
									{
										this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
										{
											cell = edge2.vertices[1].cell,
											parent = dfsnode
										});
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x0600BECF RID: 48847 RVA: 0x003DA6B0 File Offset: 0x003D88B0
			public void WriteFlow(bool cycles_only = false)
			{
				if (!cycles_only)
				{
					foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge in this.edges)
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator = edge.Iter(this.conduit_flow);
						while (vertexIterator.IsValid())
						{
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertexIterator.cell].conduitIdx, vertexIterator.direction);
							vertexIterator.Next();
						}
					}
				}
				foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.cycles)
				{
					this.cycle_vertices.Clear();
					ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator2 = edge2.Iter(this.conduit_flow);
					vertexIterator2.Next();
					while (vertexIterator2.IsValid())
					{
						this.cycle_vertices.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
						{
							cell = vertexIterator2.cell,
							direction = vertexIterator2.direction
						});
						vertexIterator2.Next();
					}
					if (this.cycle_vertices.Count > 1)
					{
						int i = 0;
						int num = this.cycle_vertices.Count - 1;
						ConduitFlow.FlowDirections direction = edge2.vertices[0].direction;
						while (i <= num)
						{
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex = this.cycle_vertices[i];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex.cell].conduitIdx, ConduitFlow.Opposite(direction));
							direction = vertex.direction;
							i++;
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex2 = this.cycle_vertices[num];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex2.cell].conduitIdx, vertex2.direction);
							num--;
						}
						this.dead_ends.Add(this.cycle_vertices[i].cell);
						this.dead_ends.Add(this.cycle_vertices[num].cell);
					}
				}
			}

			// Token: 0x0400A6B7 RID: 42679
			private ConduitFlow conduit_flow;

			// Token: 0x0400A6B8 RID: 42680
			private HashSetPool<int, ConduitFlow>.PooledHashSet vertex_cells;

			// Token: 0x0400A6B9 RID: 42681
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList edges;

			// Token: 0x0400A6BA RID: 42682
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList cycles;

			// Token: 0x0400A6BB RID: 42683
			private QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledQueue bfs_traversal;

			// Token: 0x0400A6BC RID: 42684
			private HashSetPool<int, ConduitFlow>.PooledHashSet visited;

			// Token: 0x0400A6BD RID: 42685
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList pseudo_sources;

			// Token: 0x0400A6BE RID: 42686
			public HashSetPool<int, ConduitFlow>.PooledHashSet sources;

			// Token: 0x0400A6BF RID: 42687
			private HashSetPool<int, ConduitFlow>.PooledHashSet sinks;

			// Token: 0x0400A6C0 RID: 42688
			private HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledHashSet dfs_path;

			// Token: 0x0400A6C1 RID: 42689
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledList dfs_traversal;

			// Token: 0x0400A6C2 RID: 42690
			public HashSetPool<int, ConduitFlow>.PooledHashSet dead_ends;

			// Token: 0x0400A6C3 RID: 42691
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList cycle_vertices;

			// Token: 0x02003539 RID: 13625
			[DebuggerDisplay("{cell}:{direction}")]
			public struct Vertex : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Vertex>
			{
				// Token: 0x17000C37 RID: 3127
				// (get) Token: 0x0600DF3E RID: 57150 RVA: 0x0043234C File Offset: 0x0043054C
				public bool is_valid
				{
					get
					{
						return this.cell != -1;
					}
				}

				// Token: 0x0600DF3F RID: 57151 RVA: 0x0043235A File Offset: 0x0043055A
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Vertex rhs)
				{
					return this.direction == rhs.direction && this.cell == rhs.cell;
				}

				// Token: 0x0400D7AF RID: 55215
				public ConduitFlow.FlowDirections direction;

				// Token: 0x0400D7B0 RID: 55216
				public int cell;

				// Token: 0x0400D7B1 RID: 55217
				public static ConduitFlow.BuildNetworkTask.Graph.Vertex INVALID = new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					direction = ConduitFlow.FlowDirections.None,
					cell = -1
				};
			}

			// Token: 0x0200353A RID: 13626
			[DebuggerDisplay("{vertices[0].cell}:{vertices[0].direction} -> {vertices[1].cell}:{vertices[1].direction}")]
			public struct Edge : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Edge>
			{
				// Token: 0x17000C38 RID: 3128
				// (get) Token: 0x0600DF41 RID: 57153 RVA: 0x004323A7 File Offset: 0x004305A7
				public bool is_valid
				{
					get
					{
						return this.vertices != null;
					}
				}

				// Token: 0x0600DF42 RID: 57154 RVA: 0x004323B4 File Offset: 0x004305B4
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Edge rhs)
				{
					if (this.vertices == null)
					{
						return rhs.vertices == null;
					}
					return rhs.vertices != null && (this.vertices.Length == rhs.vertices.Length && this.vertices.Length == 2 && this.vertices[0].Equals(rhs.vertices[0])) && this.vertices[1].Equals(rhs.vertices[1]);
				}

				// Token: 0x0600DF43 RID: 57155 RVA: 0x00432438 File Offset: 0x00430638
				public ConduitFlow.BuildNetworkTask.Graph.Edge Invert()
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge
					{
						vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
						{
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[1].cell,
								direction = ConduitFlow.Opposite(this.vertices[1].direction)
							},
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[0].cell,
								direction = ConduitFlow.Opposite(this.vertices[0].direction)
							}
						}
					};
				}

				// Token: 0x0600DF44 RID: 57156 RVA: 0x004324E5 File Offset: 0x004306E5
				public ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator Iter(ConduitFlow conduit_flow)
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator(conduit_flow, this);
				}

				// Token: 0x0400D7B2 RID: 55218
				public ConduitFlow.BuildNetworkTask.Graph.Vertex[] vertices;

				// Token: 0x0400D7B3 RID: 55219
				public static readonly ConduitFlow.BuildNetworkTask.Graph.Edge INVALID = new ConduitFlow.BuildNetworkTask.Graph.Edge
				{
					vertices = null
				};

				// Token: 0x02003856 RID: 14422
				[DebuggerDisplay("{cell}:{direction}")]
				public struct VertexIterator
				{
					// Token: 0x0600E4FC RID: 58620 RVA: 0x00440017 File Offset: 0x0043E217
					public VertexIterator(ConduitFlow conduit_flow, ConduitFlow.BuildNetworkTask.Graph.Edge edge)
					{
						this.conduit_flow = conduit_flow;
						this.edge = edge;
						this.cell = edge.vertices[0].cell;
						this.direction = edge.vertices[0].direction;
					}

					// Token: 0x0600E4FD RID: 58621 RVA: 0x00440058 File Offset: 0x0043E258
					public void Next()
					{
						int conduitIdx = this.conduit_flow.grid[this.cell].conduitIdx;
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, this.direction);
						global::Debug.Assert(conduitFromDirection.idx != -1);
						this.cell = conduitFromDirection.GetCell(this.conduit_flow);
						if (this.cell == this.edge.vertices[1].cell)
						{
							return;
						}
						this.direction = ConduitFlow.Opposite(this.direction);
						bool flag = false;
						for (int num = 0; num != 3; num++)
						{
							this.direction = ConduitFlow.ComputeNextFlowDirection(this.direction);
							if (this.conduit_flow.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.direction).idx != -1)
							{
								flag = true;
								break;
							}
						}
						global::Debug.Assert(flag);
						if (!flag)
						{
							this.cell = this.edge.vertices[1].cell;
						}
					}

					// Token: 0x0600E4FE RID: 58622 RVA: 0x00440159 File Offset: 0x0043E359
					public bool IsValid()
					{
						return this.cell != this.edge.vertices[1].cell;
					}

					// Token: 0x0400DF9D RID: 57245
					public int cell;

					// Token: 0x0400DF9E RID: 57246
					public ConduitFlow.FlowDirections direction;

					// Token: 0x0400DF9F RID: 57247
					private ConduitFlow conduit_flow;

					// Token: 0x0400DFA0 RID: 57248
					private ConduitFlow.BuildNetworkTask.Graph.Edge edge;
				}
			}

			// Token: 0x0200353B RID: 13627
			[DebuggerDisplay("cell:{cell}, parent:{parent == null ? -1 : parent.cell}")]
			private class DFSNode
			{
				// Token: 0x0400D7B4 RID: 55220
				public int cell;

				// Token: 0x0400D7B5 RID: 55221
				public ConduitFlow.BuildNetworkTask.Graph.DFSNode parent;
			}
		}
	}

	// Token: 0x020016AB RID: 5803
	private struct ConnectContext
	{
		// Token: 0x06009347 RID: 37703 RVA: 0x00358C58 File Offset: 0x00356E58
		public ConnectContext(ConduitFlow outer)
		{
			this.outer = outer;
			this.cells = ListPool<int, ConduitFlow>.Allocate();
			this.cells.Capacity = Mathf.Max(this.cells.Capacity, outer.soaInfo.NumEntries);
		}

		// Token: 0x06009348 RID: 37704 RVA: 0x00358C92 File Offset: 0x00356E92
		public void Finish()
		{
			this.cells.Recycle();
		}

		// Token: 0x04007081 RID: 28801
		public ListPool<int, ConduitFlow>.PooledList cells;

		// Token: 0x04007082 RID: 28802
		public ConduitFlow outer;
	}

	// Token: 0x020016AC RID: 5804
	private struct ConnectTask : IWorkItem<ConduitFlow.ConnectContext>
	{
		// Token: 0x06009349 RID: 37705 RVA: 0x00358C9F File Offset: 0x00356E9F
		public ConnectTask(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x0600934A RID: 37706 RVA: 0x00358CB0 File Offset: 0x00356EB0
		public void Run(ConduitFlow.ConnectContext context)
		{
			for (int num = this.start; num != this.end; num++)
			{
				int num2 = context.cells[num];
				int conduitIdx = context.outer.grid[num2].conduitIdx;
				if (conduitIdx != -1)
				{
					UtilityConnections connections = context.outer.networkMgr.GetConnections(num2, true);
					if (connections != (UtilityConnections)0)
					{
						ConduitFlow.ConduitConnections @default = ConduitFlow.ConduitConnections.DEFAULT;
						int num3 = num2 - 1;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
						{
							@default.left = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 + 1;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
						{
							@default.right = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 - Grid.WidthInCells;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
						{
							@default.down = context.outer.grid[num3].conduitIdx;
						}
						num3 = num2 + Grid.WidthInCells;
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
						{
							@default.up = context.outer.grid[num3].conduitIdx;
						}
						context.outer.soaInfo.SetConduitConnections(conduitIdx, @default);
					}
				}
			}
		}

		// Token: 0x04007083 RID: 28803
		private int start;

		// Token: 0x04007084 RID: 28804
		private int end;
	}

	// Token: 0x020016AD RID: 5805
	private class UpdateNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x0600934B RID: 37707 RVA: 0x00358E03 File Offset: 0x00357003
		// (set) Token: 0x0600934C RID: 37708 RVA: 0x00358E0B File Offset: 0x0035700B
		public bool continue_updating { get; private set; }

		// Token: 0x0600934D RID: 37709 RVA: 0x00358E14 File Offset: 0x00357014
		public UpdateNetworkTask(ConduitFlow.Network network)
		{
			this.continue_updating = true;
			this.network = network;
		}

		// Token: 0x0600934E RID: 37710 RVA: 0x00358E2C File Offset: 0x0035702C
		public void Run(ConduitFlow conduit_flow)
		{
			global::Debug.Assert(this.continue_updating);
			this.continue_updating = false;
			foreach (int num in this.network.cells)
			{
				int conduitIdx = conduit_flow.grid[num].conduitIdx;
				if (conduit_flow.UpdateConduit(conduit_flow.soaInfo.GetConduit(conduitIdx)))
				{
					this.continue_updating = true;
				}
			}
		}

		// Token: 0x0600934F RID: 37711 RVA: 0x00358EBC File Offset: 0x003570BC
		public void Finish(ConduitFlow conduit_flow)
		{
			foreach (int num in this.network.cells)
			{
				ConduitFlow.ConduitContents contents = conduit_flow.grid[num].contents;
				contents.ConsolidateMass();
				conduit_flow.grid[num].contents = contents;
			}
		}

		// Token: 0x04007085 RID: 28805
		private ConduitFlow.Network network;
	}
}
