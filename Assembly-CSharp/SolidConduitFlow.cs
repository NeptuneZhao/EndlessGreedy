using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

// Token: 0x02000AAB RID: 2731
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitFlow : IConduitFlow
{
	// Token: 0x0600506A RID: 20586 RVA: 0x001CE075 File Offset: 0x001CC275
	public SolidConduitFlow.SOAInfo GetSOAInfo()
	{
		return this.soaInfo;
	}

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x0600506B RID: 20587 RVA: 0x001CE080 File Offset: 0x001CC280
	// (remove) Token: 0x0600506C RID: 20588 RVA: 0x001CE0B8 File Offset: 0x001CC2B8
	public event System.Action onConduitsRebuilt;

	// Token: 0x0600506D RID: 20589 RVA: 0x001CE0F0 File Offset: 0x001CC2F0
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new SolidConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x0600506E RID: 20590 RVA: 0x001CE128 File Offset: 0x001CC328
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

	// Token: 0x0600506F RID: 20591 RVA: 0x001CE178 File Offset: 0x001CC378
	public static int FlowBit(SolidConduitFlow.FlowDirection direction)
	{
		return 1 << direction - SolidConduitFlow.FlowDirection.Left;
	}

	// Token: 0x06005070 RID: 20592 RVA: 0x001CE184 File Offset: 0x001CC384
	public SolidConduitFlow(int num_cells, IUtilityNetworkMgr network_mgr, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.networkMgr = network_mgr;
		this.maskedOverlayLayer = LayerMask.NameToLayer("MaskedOverlay");
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x06005071 RID: 20593 RVA: 0x001CE234 File Offset: 0x001CC434
	public void Initialize(int num_cells)
	{
		this.grid = new SolidConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.pickupableHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x06005072 RID: 20594 RVA: 0x001CE288 File Offset: 0x001CC488
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			FlowUtilityNetwork network = (FlowUtilityNetwork)utilityNetwork;
			this.ScanNetworkSources(network);
		}
		this.RefreshPaths();
	}

	// Token: 0x06005073 RID: 20595 RVA: 0x001CE2E4 File Offset: 0x001CC4E4
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		this.soaInfo.Clear(this);
		this.pathList.Clear();
		ObjectLayer layer = ObjectLayer.SolidConduit;
		foreach (int num in root_nodes)
		{
			if (this.replacements.Contains(num))
			{
				this.replacements.Remove(num);
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
				this.grid[num].conduitIdx = conduitIdx;
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		foreach (int num2 in root_nodes)
		{
			UtilityConnections connections = this.networkMgr.GetConnections(num2, true);
			if (connections != (UtilityConnections)0 && this.grid[num2].conduitIdx != -1)
			{
				int conduitIdx2 = this.grid[num2].conduitIdx;
				SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduitIdx2);
				int num3 = num2 - 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					conduitConnections.left = this.grid[num3].conduitIdx;
				}
				num3 = num2 + 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					conduitConnections.right = this.grid[num3].conduitIdx;
				}
				num3 = num2 - Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					conduitConnections.down = this.grid[num3].conduitIdx;
				}
				num3 = num2 + Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					conduitConnections.up = this.grid[num3].conduitIdx;
				}
				this.soaInfo.SetConduitConnections(conduitIdx2, conduitConnections);
			}
		}
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x06005074 RID: 20596 RVA: 0x001CE52C File Offset: 0x001CC72C
	public void ScanNetworkSources(FlowUtilityNetwork network)
	{
		if (network == null)
		{
			return;
		}
		for (int i = 0; i < network.sources.Count; i++)
		{
			FlowUtilityNetwork.IItem item = network.sources[i];
			this.path.Clear();
			this.visited.Clear();
			this.FindSinks(i, item.Cell);
		}
	}

	// Token: 0x06005075 RID: 20597 RVA: 0x001CE584 File Offset: 0x001CC784
	public void RefreshPaths()
	{
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				SolidConduitFlow.Conduit conduit = list[i];
				SolidConduitFlow.Conduit target_conduit = list[i + 1];
				if (conduit.GetTargetFlowDirection(this) == SolidConduitFlow.FlowDirection.None)
				{
					SolidConduitFlow.FlowDirection direction = this.GetDirection(conduit, target_conduit);
					conduit.SetTargetFlowDirection(direction, this);
				}
			}
		}
	}

	// Token: 0x06005076 RID: 20598 RVA: 0x001CE618 File Offset: 0x001CC818
	private void FindSinks(int source_idx, int cell)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.FindSinksInternal(source_idx, gridNode.conduitIdx);
		}
	}

	// Token: 0x06005077 RID: 20599 RVA: 0x001CE648 File Offset: 0x001CC848
	private void FindSinksInternal(int source_idx, int conduit_idx)
	{
		if (this.visited.Contains(conduit_idx))
		{
			return;
		}
		this.visited.Add(conduit_idx);
		SolidConduitFlow.Conduit conduit = this.soaInfo.GetConduit(conduit_idx);
		if (conduit.GetPermittedFlowDirections(this) == -1)
		{
			return;
		}
		this.path.Add(conduit);
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)this.networkMgr.GetEndpoint(this.soaInfo.GetCell(conduit_idx));
		if (item != null && item.EndpointType == Endpoint.Sink)
		{
			this.FoundSink(source_idx);
		}
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit_idx);
		if (conduitConnections.down != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.down);
		}
		if (conduitConnections.left != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.left);
		}
		if (conduitConnections.right != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.right);
		}
		if (conduitConnections.up != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.up);
		}
		if (this.path.Count > 0)
		{
			this.path.RemoveAt(this.path.Count - 1);
		}
	}

	// Token: 0x06005078 RID: 20600 RVA: 0x001CE754 File Offset: 0x001CC954
	private SolidConduitFlow.FlowDirection GetDirection(SolidConduitFlow.Conduit conduit, SolidConduitFlow.Conduit target_conduit)
	{
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Right;
		}
		return SolidConduitFlow.FlowDirection.None;
	}

	// Token: 0x06005079 RID: 20601 RVA: 0x001CE7B4 File Offset: 0x001CC9B4
	private void FoundSink(int source_idx)
	{
		for (int i = 0; i < this.path.Count - 1; i++)
		{
			SolidConduitFlow.FlowDirection direction = this.GetDirection(this.path[i], this.path[i + 1]);
			SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(direction);
			int cellFromDirection = SolidConduitFlow.GetCellFromDirection(this.soaInfo.GetCell(this.path[i].idx), direction2);
			SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(this.path[i].idx, direction2);
			if (i == 0 || (this.path[i].GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0 || (cellFromDirection != this.soaInfo.GetCell(this.path[i - 1].idx) && (this.soaInfo.GetSrcFlowIdx(this.path[i].idx) == source_idx || (conduitFromDirection.GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0)))
			{
				int permittedFlowDirections = this.path[i].GetPermittedFlowDirections(this);
				this.soaInfo.SetSrcFlowIdx(this.path[i].idx, source_idx);
				this.path[i].SetPermittedFlowDirections(permittedFlowDirections | SolidConduitFlow.FlowBit(direction), this);
				this.path[i].SetTargetFlowDirection(direction, this);
			}
		}
		for (int j = 1; j < this.path.Count; j++)
		{
			SolidConduitFlow.FlowDirection direction3 = this.GetDirection(this.path[j], this.path[j - 1]);
			this.soaInfo.SetSrcFlowDirection(this.path[j].idx, direction3);
		}
		List<SolidConduitFlow.Conduit> list = new List<SolidConduitFlow.Conduit>(this.path);
		list.Reverse();
		this.TryAdd(list);
	}

	// Token: 0x0600507A RID: 20602 RVA: 0x001CE9A4 File Offset: 0x001CCBA4
	private void TryAdd(List<SolidConduitFlow.Conduit> new_path)
	{
		Predicate<SolidConduitFlow.Conduit> <>9__0;
		Predicate<SolidConduitFlow.Conduit> <>9__1;
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			if (list.Count >= new_path.Count)
			{
				bool flag = false;
				List<SolidConduitFlow.Conduit> list2 = list;
				Predicate<SolidConduitFlow.Conduit> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[0].idx));
				}
				int num = list2.FindIndex(match);
				List<SolidConduitFlow.Conduit> list3 = list;
				Predicate<SolidConduitFlow.Conduit> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[new_path.Count - 1].idx));
				}
				int num2 = list3.FindIndex(match2);
				if (num != -1 && num2 != -1)
				{
					flag = true;
					int i = num;
					int num3 = 0;
					while (i < num2)
					{
						if (list[i].idx != new_path[num3].idx)
						{
							flag = false;
							break;
						}
						i++;
						num3++;
					}
				}
				if (flag)
				{
					return;
				}
			}
		}
		for (int j = this.pathList.Count - 1; j >= 0; j--)
		{
			if (this.pathList[j].Count <= 0)
			{
				this.pathList.RemoveAt(j);
			}
		}
		for (int k = this.pathList.Count - 1; k >= 0; k--)
		{
			List<SolidConduitFlow.Conduit> old_path = this.pathList[k];
			if (new_path.Count >= old_path.Count)
			{
				bool flag2 = false;
				int num4 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[0].idx);
				int num5 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[old_path.Count - 1].idx);
				if (num4 != -1 && num5 != -1)
				{
					flag2 = true;
					int l = num4;
					int num6 = 0;
					while (l < num5)
					{
						if (new_path[l].idx != old_path[num6].idx)
						{
							flag2 = false;
							break;
						}
						l++;
						num6++;
					}
				}
				if (flag2)
				{
					this.pathList.RemoveAt(k);
				}
			}
		}
		foreach (List<SolidConduitFlow.Conduit> list4 in this.pathList)
		{
			for (int m = new_path.Count - 1; m >= 0; m--)
			{
				SolidConduitFlow.Conduit new_conduit = new_path[m];
				if (list4.FindIndex((SolidConduitFlow.Conduit t) => t.idx == new_conduit.idx) != -1 && Mathf.IsPowerOfTwo(this.soaInfo.GetPermittedFlowDirections(new_conduit.idx)))
				{
					new_path.RemoveAt(m);
				}
			}
		}
		this.pathList.Add(new_path);
	}

	// Token: 0x0600507B RID: 20603 RVA: 0x001CECC0 File Offset: 0x001CCEC0
	public SolidConduitFlow.ConduitContents GetContents(int cell)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		return contents;
	}

	// Token: 0x0600507C RID: 20604 RVA: 0x001CED14 File Offset: 0x001CCF14
	private void SetContents(int cell, SolidConduitFlow.ConduitContents contents)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x0600507D RID: 20605 RVA: 0x001CED68 File Offset: 0x001CCF68
	public void SetContents(int cell, Pickupable pickupable)
	{
		SolidConduitFlow.ConduitContents contents = new SolidConduitFlow.ConduitContents
		{
			pickupableHandle = HandleVector<int>.InvalidHandle
		};
		if (pickupable != null)
		{
			KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
			SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
			{
				kbac = component,
				pickupable = pickupable
			};
			contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
			KBatchedAnimController component2 = pickupable.GetComponent<KBatchedAnimController>();
			component2.enabled = false;
			component2.enabled = true;
			pickupable.Trigger(856640610, true);
		}
		this.SetContents(cell, contents);
	}

	// Token: 0x0600507E RID: 20606 RVA: 0x001CEDF5 File Offset: 0x001CCFF5
	public static int GetCellFromDirection(int cell, SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return Grid.CellLeft(cell);
		case SolidConduitFlow.FlowDirection.Right:
			return Grid.CellRight(cell);
		case SolidConduitFlow.FlowDirection.Up:
			return Grid.CellAbove(cell);
		case SolidConduitFlow.FlowDirection.Down:
			return Grid.CellBelow(cell);
		default:
			return -1;
		}
	}

	// Token: 0x0600507F RID: 20607 RVA: 0x001CEE2E File Offset: 0x001CD02E
	public static SolidConduitFlow.FlowDirection InverseFlow(SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return SolidConduitFlow.FlowDirection.Right;
		case SolidConduitFlow.FlowDirection.Right:
			return SolidConduitFlow.FlowDirection.Left;
		case SolidConduitFlow.FlowDirection.Up:
			return SolidConduitFlow.FlowDirection.Down;
		case SolidConduitFlow.FlowDirection.Down:
			return SolidConduitFlow.FlowDirection.Up;
		default:
			return SolidConduitFlow.FlowDirection.None;
		}
	}

	// Token: 0x06005080 RID: 20608 RVA: 0x001CEE54 File Offset: 0x001CD054
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
		float obj = 1f;
		this.elapsedTime -= 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			foreach (SolidConduitFlow.Conduit conduit in list)
			{
				this.UpdateConduit(conduit);
			}
		}
		this.soaInfo.UpdateFlowDirection(this);
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((SolidConduitFlow.ConduitUpdater a, SolidConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x06005081 RID: 20609 RVA: 0x001CEFAC File Offset: 0x001CD1AC
	public void RenderEveryTick(float dt)
	{
		for (int i = 0; i < this.GetSOAInfo().NumEntries; i++)
		{
			SolidConduitFlow.Conduit conduit = this.GetSOAInfo().GetConduit(i);
			SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this);
			if (lastFlowInfo.direction != SolidConduitFlow.FlowDirection.None)
			{
				int cell = conduit.GetCell(this);
				int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
				SolidConduitFlow.ConduitContents contents = this.GetContents(cellFromDirection);
				if (contents.pickupableHandle.IsValid())
				{
					Vector3 a = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					Vector3 b = Grid.CellToPosCCC(cellFromDirection, Grid.SceneLayer.SolidConduitContents);
					Vector3 position = Vector3.Lerp(a, b, this.ContinuousLerpPercent);
					Pickupable pickupable = this.GetPickupable(contents.pickupableHandle);
					if (pickupable != null)
					{
						pickupable.transform.SetPosition(position);
					}
				}
			}
		}
	}

	// Token: 0x06005082 RID: 20610 RVA: 0x001CF06C File Offset: 0x001CD26C
	private void UpdateConduit(SolidConduitFlow.Conduit conduit)
	{
		if (this.soaInfo.GetUpdated(conduit.idx))
		{
			return;
		}
		if (this.soaInfo.GetSrcFlowDirection(conduit.idx) == SolidConduitFlow.FlowDirection.None)
		{
			this.soaInfo.SetSrcFlowDirection(conduit.idx, conduit.GetNextFlowSource(this));
		}
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		if (!contents.pickupableHandle.IsValid())
		{
			return;
		}
		SolidConduitFlow.FlowDirection targetFlowDirection = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, targetFlowDirection);
		if (conduitFromDirection.idx == -1)
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		int cell2 = this.soaInfo.GetCell(conduitFromDirection.idx);
		SolidConduitFlow.ConduitContents contents2 = this.grid[cell2].contents;
		if (contents2.pickupableHandle.IsValid())
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		if ((this.soaInfo.GetPermittedFlowDirections(conduit.idx) & SolidConduitFlow.FlowBit(targetFlowDirection)) != 0)
		{
			bool flag = false;
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx));
				if (conduitFromDirection2.idx == conduit.idx)
				{
					flag = true;
					break;
				}
				if (conduitFromDirection2.idx != -1)
				{
					int cell3 = this.soaInfo.GetCell(conduitFromDirection2.idx);
					SolidConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
					if (contents3.pickupableHandle.IsValid())
					{
						break;
					}
				}
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
			if (flag && !contents2.pickupableHandle.IsValid())
			{
				SolidConduitFlow.ConduitContents contents4 = this.RemoveFromGrid(conduit);
				this.AddToGrid(cell2, contents4);
				this.soaInfo.SetLastFlowInfo(conduit.idx, this.soaInfo.GetTargetFlowDirection(conduit.idx));
				this.soaInfo.SetUpdated(conduitFromDirection.idx, true);
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
		}
		this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06005083 RID: 20611 RVA: 0x001CF2D5 File Offset: 0x001CD4D5
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06005084 RID: 20612 RVA: 0x001CF2EE File Offset: 0x001CD4EE
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x06005085 RID: 20613 RVA: 0x001CF301 File Offset: 0x001CD501
	private void AddToGrid(int cell_idx, SolidConduitFlow.ConduitContents contents)
	{
		this.grid[cell_idx].contents = contents;
	}

	// Token: 0x06005086 RID: 20614 RVA: 0x001CF318 File Offset: 0x001CD518
	private SolidConduitFlow.ConduitContents RemoveFromGrid(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.ConduitContents contents2 = SolidConduitFlow.ConduitContents.EmptyContents();
		this.grid[cell].contents = contents2;
		return contents;
	}

	// Token: 0x06005087 RID: 20615 RVA: 0x001CF360 File Offset: 0x001CD560
	public void AddPickupable(int cell_idx, Pickupable pickupable)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			global::Debug.LogWarning("No conduit in cell: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		SolidConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.pickupableHandle.IsValid())
		{
			global::Debug.LogWarning("Conduit already full: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
		SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
		{
			kbac = component,
			pickupable = pickupable
		};
		contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
		if (this.viewingConduits)
		{
			this.ApplyOverlayVisualization(component);
		}
		if (pickupable.storage)
		{
			pickupable.storage.Remove(pickupable.gameObject, true);
		}
		pickupable.Trigger(856640610, true);
		this.SetContents(cell_idx, contents);
	}

	// Token: 0x06005088 RID: 20616 RVA: 0x001CF458 File Offset: 0x001CD658
	public Pickupable RemovePickupable(int cell_idx)
	{
		Pickupable pickupable = null;
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell_idx);
		if (conduit.idx != -1)
		{
			SolidConduitFlow.ConduitContents conduitContents = this.RemoveFromGrid(conduit);
			if (conduitContents.pickupableHandle.IsValid())
			{
				SolidConduitFlow.StoredInfo data = this.conveyorPickupables.GetData(conduitContents.pickupableHandle);
				this.ClearOverlayVisualization(data.kbac);
				pickupable = data.pickupable;
				if (pickupable)
				{
					pickupable.Trigger(856640610, false);
				}
				this.freedHandles.Add(conduitContents.pickupableHandle);
			}
		}
		return pickupable;
	}

	// Token: 0x06005089 RID: 20617 RVA: 0x001CF4E0 File Offset: 0x001CD6E0
	public int GetPermittedFlow(int cell)
	{
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return 0;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x0600508A RID: 20618 RVA: 0x001CF511 File Offset: 0x001CD711
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x0600508B RID: 20619 RVA: 0x001CF52C File Offset: 0x001CD72C
	public SolidConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return SolidConduitFlow.Conduit.Invalid();
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x0600508C RID: 20620 RVA: 0x001CF564 File Offset: 0x001CD764
	private void DumpPipeContents(int cell)
	{
		Pickupable pickupable = this.RemovePickupable(cell);
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x0600508D RID: 20621 RVA: 0x001CF58D File Offset: 0x001CD78D
	private void DumpPickupable(Pickupable pickupable)
	{
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x0600508E RID: 20622 RVA: 0x001CF5A3 File Offset: 0x001CD7A3
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell);
	}

	// Token: 0x0600508F RID: 20623 RVA: 0x001CF5BB File Offset: 0x001CD7BB
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x06005090 RID: 20624 RVA: 0x001CF5CC File Offset: 0x001CD7CC
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		SolidConduitFlow.ConduitContents contents = SolidConduitFlow.ConduitContents.EmptyContents();
		this.SetContents(cell, contents);
	}

	// Token: 0x06005091 RID: 20625 RVA: 0x001CF5FC File Offset: 0x001CD7FC
	public UtilityNetwork GetNetwork(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x06005092 RID: 20626 RVA: 0x001CF627 File Offset: 0x001CD827
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x06005093 RID: 20627 RVA: 0x001CF634 File Offset: 0x001CD834
	public bool IsConduitFull(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.pickupableHandle.IsValid();
	}

	// Token: 0x06005094 RID: 20628 RVA: 0x001CF660 File Offset: 0x001CD860
	public bool IsConduitEmpty(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return !contents.pickupableHandle.IsValid();
	}

	// Token: 0x06005095 RID: 20629 RVA: 0x001CF690 File Offset: 0x001CD890
	public void Initialize()
	{
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			OverlayScreen instance2 = OverlayScreen.Instance;
			instance2.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance2.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		}
	}

	// Token: 0x06005096 RID: 20630 RVA: 0x001CF6F8 File Offset: 0x001CD8F8
	private void OnOverlayChanged(HashedString mode)
	{
		bool flag = mode == OverlayModes.SolidConveyor.ID;
		if (flag == this.viewingConduits)
		{
			return;
		}
		this.viewingConduits = flag;
		int layer = this.viewingConduits ? this.maskedOverlayLayer : Game.PickupableLayer;
		Color32 tintColour = this.viewingConduits ? SolidConduitFlow.OverlayColour : SolidConduitFlow.NormalColour;
		List<SolidConduitFlow.StoredInfo> dataList = this.conveyorPickupables.GetDataList();
		for (int i = 0; i < dataList.Count; i++)
		{
			SolidConduitFlow.StoredInfo storedInfo = dataList[i];
			if (storedInfo.kbac != null)
			{
				storedInfo.kbac.SetLayer(layer);
				storedInfo.kbac.TintColour = tintColour;
			}
		}
	}

	// Token: 0x06005097 RID: 20631 RVA: 0x001CF7A1 File Offset: 0x001CD9A1
	private void ApplyOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(this.maskedOverlayLayer);
		kbac.TintColour = SolidConduitFlow.OverlayColour;
	}

	// Token: 0x06005098 RID: 20632 RVA: 0x001CF7C4 File Offset: 0x001CD9C4
	private void ClearOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(Game.PickupableLayer);
		kbac.TintColour = SolidConduitFlow.NormalColour;
	}

	// Token: 0x06005099 RID: 20633 RVA: 0x001CF7E8 File Offset: 0x001CD9E8
	public Pickupable GetPickupable(HandleVector<int>.Handle h)
	{
		Pickupable result = null;
		if (h.IsValid())
		{
			result = this.conveyorPickupables.GetData(h).pickupable;
		}
		return result;
	}

	// Token: 0x04003572 RID: 13682
	public const float MAX_SOLID_MASS = 20f;

	// Token: 0x04003573 RID: 13683
	public const float TickRate = 1f;

	// Token: 0x04003574 RID: 13684
	public const float WaitTime = 1f;

	// Token: 0x04003575 RID: 13685
	private float elapsedTime;

	// Token: 0x04003576 RID: 13686
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x04003577 RID: 13687
	private KCompactedVector<SolidConduitFlow.StoredInfo> conveyorPickupables = new KCompactedVector<SolidConduitFlow.StoredInfo>(0);

	// Token: 0x04003578 RID: 13688
	private List<HandleVector<int>.Handle> freedHandles = new List<HandleVector<int>.Handle>();

	// Token: 0x04003579 RID: 13689
	private SolidConduitFlow.SOAInfo soaInfo = new SolidConduitFlow.SOAInfo();

	// Token: 0x0400357B RID: 13691
	private bool dirtyConduitUpdaters;

	// Token: 0x0400357C RID: 13692
	private List<SolidConduitFlow.ConduitUpdater> conduitUpdaters = new List<SolidConduitFlow.ConduitUpdater>();

	// Token: 0x0400357D RID: 13693
	private SolidConduitFlow.GridNode[] grid;

	// Token: 0x0400357E RID: 13694
	public IUtilityNetworkMgr networkMgr;

	// Token: 0x0400357F RID: 13695
	private HashSet<int> visited = new HashSet<int>();

	// Token: 0x04003580 RID: 13696
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04003581 RID: 13697
	private List<SolidConduitFlow.Conduit> path = new List<SolidConduitFlow.Conduit>();

	// Token: 0x04003582 RID: 13698
	private List<List<SolidConduitFlow.Conduit>> pathList = new List<List<SolidConduitFlow.Conduit>>();

	// Token: 0x04003583 RID: 13699
	public static readonly SolidConduitFlow.ConduitContents emptyContents = new SolidConduitFlow.ConduitContents
	{
		pickupableHandle = HandleVector<int>.InvalidHandle
	};

	// Token: 0x04003584 RID: 13700
	private int maskedOverlayLayer;

	// Token: 0x04003585 RID: 13701
	private bool viewingConduits;

	// Token: 0x04003586 RID: 13702
	private static readonly Color32 NormalColour = Color.white;

	// Token: 0x04003587 RID: 13703
	private static readonly Color32 OverlayColour = new Color(0.25f, 0.25f, 0.25f, 0f);

	// Token: 0x02001ADC RID: 6876
	private struct StoredInfo
	{
		// Token: 0x04007DEE RID: 32238
		public KBatchedAnimController kbac;

		// Token: 0x04007DEF RID: 32239
		public Pickupable pickupable;
	}

	// Token: 0x02001ADD RID: 6877
	public class SOAInfo
	{
		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x0600A151 RID: 41297 RVA: 0x00382CB9 File Offset: 0x00380EB9
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x0600A152 RID: 41298 RVA: 0x00382CC6 File Offset: 0x00380EC6
		public List<int> Cells
		{
			get
			{
				return this.cells;
			}
		}

		// Token: 0x0600A153 RID: 41299 RVA: 0x00382CD0 File Offset: 0x00380ED0
		public int AddConduit(SolidConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			SolidConduitFlow.Conduit item = new SolidConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new SolidConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(new SolidConduitFlow.ConduitFlowInfo
			{
				direction = SolidConduitFlow.FlowDirection.None
			});
			this.cells.Add(cell);
			this.updated.Add(false);
			this.diseaseContentsVisible.Add(false);
			this.conduitGOs.Add(conduit_go);
			this.srcFlowIdx.Add(-1);
			this.permittedFlowDirections.Add(0);
			this.srcFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			this.targetFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			return count;
		}

		// Token: 0x0600A154 RID: 41300 RVA: 0x00382DD0 File Offset: 0x00380FD0
		public void Clear(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.ForcePermanentDiseaseContainer(i, false);
				int num = this.cells[i];
				SolidConduitFlow.ConduitContents contents = manager.grid[num].contents;
				manager.grid[num].contents = contents;
				manager.grid[num].conduitIdx = -1;
			}
			this.cells.Clear();
			this.updated.Clear();
			this.diseaseContentsVisible.Clear();
			this.srcFlowIdx.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x0600A155 RID: 41301 RVA: 0x00382EC2 File Offset: 0x003810C2
		public SolidConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x0600A156 RID: 41302 RVA: 0x00382ED0 File Offset: 0x003810D0
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x0600A157 RID: 41303 RVA: 0x00382EDE File Offset: 0x003810DE
		public SolidConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x0600A158 RID: 41304 RVA: 0x00382EEC File Offset: 0x003810EC
		public void SetConduitConnections(int idx, SolidConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x0600A159 RID: 41305 RVA: 0x00382EFC File Offset: 0x003810FC
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

		// Token: 0x0600A15A RID: 41306 RVA: 0x00382F48 File Offset: 0x00381148
		public SolidConduitFlow.Conduit GetConduitFromDirection(int idx, SolidConduitFlow.FlowDirection direction)
		{
			SolidConduitFlow.Conduit result = SolidConduitFlow.Conduit.Invalid();
			SolidConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case SolidConduitFlow.FlowDirection.Left:
				result = ((conduitConnections.left != -1) ? this.conduits[conduitConnections.left] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Right:
				result = ((conduitConnections.right != -1) ? this.conduits[conduitConnections.right] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Up:
				result = ((conduitConnections.up != -1) ? this.conduits[conduitConnections.up] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Down:
				result = ((conduitConnections.down != -1) ? this.conduits[conduitConnections.down] : SolidConduitFlow.Conduit.Invalid());
				break;
			}
			return result;
		}

		// Token: 0x0600A15B RID: 41307 RVA: 0x00383014 File Offset: 0x00381214
		public void BeginFrame(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.updated[i] = false;
				SolidConduitFlow.ConduitContents contents = this.conduits[i].GetContents(manager);
				this.initialContents[i] = contents;
				this.lastFlowInfo[i] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				int num = this.cells[i];
				manager.grid[num].contents = contents;
			}
			for (int j = 0; j < manager.freedHandles.Count; j++)
			{
				HandleVector<int>.Handle handle = manager.freedHandles[j];
				manager.conveyorPickupables.Free(handle);
			}
			manager.freedHandles.Clear();
		}

		// Token: 0x0600A15C RID: 41308 RVA: 0x003830E6 File Offset: 0x003812E6
		public void EndFrame(SolidConduitFlow manager)
		{
		}

		// Token: 0x0600A15D RID: 41309 RVA: 0x003830E8 File Offset: 0x003812E8
		public void UpdateFlowDirection(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				SolidConduitFlow.Conduit conduit = this.conduits[i];
				if (!this.updated[i])
				{
					int cell = conduit.GetCell(manager);
					SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
					if (!contents.pickupableHandle.IsValid())
					{
						this.srcFlowDirections[conduit.idx] = conduit.GetNextFlowSource(manager);
					}
				}
			}
		}

		// Token: 0x0600A15E RID: 41310 RVA: 0x00383168 File Offset: 0x00381368
		public void MarkConduitEmpty(int idx, SolidConduitFlow manager)
		{
			if (this.lastFlowInfo[idx].direction != SolidConduitFlow.FlowDirection.None)
			{
				this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				SolidConduitFlow.Conduit conduit = this.conduits[idx];
				this.targetFlowDirections[idx] = conduit.GetNextFlowTarget(manager);
				int num = this.cells[idx];
				manager.grid[num].contents = SolidConduitFlow.ConduitContents.EmptyContents();
			}
		}

		// Token: 0x0600A15F RID: 41311 RVA: 0x003831EC File Offset: 0x003813EC
		public void SetLastFlowInfo(int idx, SolidConduitFlow.FlowDirection direction)
		{
			this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
			{
				direction = direction
			};
		}

		// Token: 0x0600A160 RID: 41312 RVA: 0x00383216 File Offset: 0x00381416
		public SolidConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x0600A161 RID: 41313 RVA: 0x00383224 File Offset: 0x00381424
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x0600A162 RID: 41314 RVA: 0x00383232 File Offset: 0x00381432
		public int GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x0600A163 RID: 41315 RVA: 0x00383240 File Offset: 0x00381440
		public void SetPermittedFlowDirections(int idx, int permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x0600A164 RID: 41316 RVA: 0x0038324F File Offset: 0x0038144F
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x0600A165 RID: 41317 RVA: 0x0038325D File Offset: 0x0038145D
		public void SetTargetFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x0600A166 RID: 41318 RVA: 0x0038326C File Offset: 0x0038146C
		public int GetSrcFlowIdx(int idx)
		{
			return this.srcFlowIdx[idx];
		}

		// Token: 0x0600A167 RID: 41319 RVA: 0x0038327A File Offset: 0x0038147A
		public void SetSrcFlowIdx(int idx, int new_src_idx)
		{
			this.srcFlowIdx[idx] = new_src_idx;
		}

		// Token: 0x0600A168 RID: 41320 RVA: 0x00383289 File Offset: 0x00381489
		public SolidConduitFlow.FlowDirection GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x0600A169 RID: 41321 RVA: 0x00383297 File Offset: 0x00381497
		public void SetSrcFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x0600A16A RID: 41322 RVA: 0x003832A6 File Offset: 0x003814A6
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x0600A16B RID: 41323 RVA: 0x003832B4 File Offset: 0x003814B4
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x0600A16C RID: 41324 RVA: 0x003832C3 File Offset: 0x003814C3
		public bool GetUpdated(int idx)
		{
			return this.updated[idx];
		}

		// Token: 0x0600A16D RID: 41325 RVA: 0x003832D1 File Offset: 0x003814D1
		public void SetUpdated(int idx, bool is_updated)
		{
			this.updated[idx] = is_updated;
		}

		// Token: 0x04007DF0 RID: 32240
		private List<SolidConduitFlow.Conduit> conduits = new List<SolidConduitFlow.Conduit>();

		// Token: 0x04007DF1 RID: 32241
		private List<SolidConduitFlow.ConduitConnections> conduitConnections = new List<SolidConduitFlow.ConduitConnections>();

		// Token: 0x04007DF2 RID: 32242
		private List<SolidConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<SolidConduitFlow.ConduitFlowInfo>();

		// Token: 0x04007DF3 RID: 32243
		private List<SolidConduitFlow.ConduitContents> initialContents = new List<SolidConduitFlow.ConduitContents>();

		// Token: 0x04007DF4 RID: 32244
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x04007DF5 RID: 32245
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x04007DF6 RID: 32246
		private List<bool> updated = new List<bool>();

		// Token: 0x04007DF7 RID: 32247
		private List<int> cells = new List<int>();

		// Token: 0x04007DF8 RID: 32248
		private List<int> permittedFlowDirections = new List<int>();

		// Token: 0x04007DF9 RID: 32249
		private List<int> srcFlowIdx = new List<int>();

		// Token: 0x04007DFA RID: 32250
		private List<SolidConduitFlow.FlowDirection> srcFlowDirections = new List<SolidConduitFlow.FlowDirection>();

		// Token: 0x04007DFB RID: 32251
		private List<SolidConduitFlow.FlowDirection> targetFlowDirections = new List<SolidConduitFlow.FlowDirection>();
	}

	// Token: 0x02001ADE RID: 6878
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x04007DFC RID: 32252
		public ConduitFlowPriority priority;

		// Token: 0x04007DFD RID: 32253
		public Action<float> callback;
	}

	// Token: 0x02001ADF RID: 6879
	public struct GridNode
	{
		// Token: 0x04007DFE RID: 32254
		public int conduitIdx;

		// Token: 0x04007DFF RID: 32255
		public SolidConduitFlow.ConduitContents contents;
	}

	// Token: 0x02001AE0 RID: 6880
	public enum FlowDirection
	{
		// Token: 0x04007E01 RID: 32257
		Blocked = -1,
		// Token: 0x04007E02 RID: 32258
		None,
		// Token: 0x04007E03 RID: 32259
		Left,
		// Token: 0x04007E04 RID: 32260
		Right,
		// Token: 0x04007E05 RID: 32261
		Up,
		// Token: 0x04007E06 RID: 32262
		Down,
		// Token: 0x04007E07 RID: 32263
		Num
	}

	// Token: 0x02001AE1 RID: 6881
	public struct ConduitConnections
	{
		// Token: 0x04007E08 RID: 32264
		public int left;

		// Token: 0x04007E09 RID: 32265
		public int right;

		// Token: 0x04007E0A RID: 32266
		public int up;

		// Token: 0x04007E0B RID: 32267
		public int down;
	}

	// Token: 0x02001AE2 RID: 6882
	public struct ConduitFlowInfo
	{
		// Token: 0x04007E0C RID: 32268
		public SolidConduitFlow.FlowDirection direction;
	}

	// Token: 0x02001AE3 RID: 6883
	[Serializable]
	public struct Conduit : IEquatable<SolidConduitFlow.Conduit>
	{
		// Token: 0x0600A16F RID: 41327 RVA: 0x00383377 File Offset: 0x00381577
		public static SolidConduitFlow.Conduit Invalid()
		{
			return new SolidConduitFlow.Conduit(-1);
		}

		// Token: 0x0600A170 RID: 41328 RVA: 0x0038337F File Offset: 0x0038157F
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x0600A171 RID: 41329 RVA: 0x00383388 File Offset: 0x00381588
		public int GetPermittedFlowDirections(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x0600A172 RID: 41330 RVA: 0x0038339B File Offset: 0x0038159B
		public void SetPermittedFlowDirections(int permitted, SolidConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x0600A173 RID: 41331 RVA: 0x003833AF File Offset: 0x003815AF
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x0600A174 RID: 41332 RVA: 0x003833C2 File Offset: 0x003815C2
		public void SetTargetFlowDirection(SolidConduitFlow.FlowDirection directions, SolidConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x0600A175 RID: 41333 RVA: 0x003833D8 File Offset: 0x003815D8
		public SolidConduitFlow.ConduitContents GetContents(SolidConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			return manager.grid[cell].contents;
		}

		// Token: 0x0600A176 RID: 41334 RVA: 0x00383408 File Offset: 0x00381608
		public void SetContents(SolidConduitFlow manager, SolidConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			if (contents.pickupableHandle.IsValid())
			{
				Pickupable pickupable = manager.GetPickupable(contents.pickupableHandle);
				if (pickupable != null)
				{
					pickupable.transform.parent = null;
					Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					pickupable.transform.SetPosition(position);
					KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
					component.GetBatchInstanceData().ClearOverrideTransformMatrix();
					component.SetSceneLayer(Grid.SceneLayer.SolidConduitContents);
				}
			}
		}

		// Token: 0x0600A177 RID: 41335 RVA: 0x00383498 File Offset: 0x00381698
		public SolidConduitFlow.FlowDirection GetNextFlowSource(SolidConduitFlow manager)
		{
			if (manager.soaInfo.GetPermittedFlowDirections(this.idx) == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			SolidConduitFlow.FlowDirection flowDirection = manager.soaInfo.GetSrcFlowDirection(this.idx);
			if (flowDirection == SolidConduitFlow.FlowDirection.None)
			{
				flowDirection = SolidConduitFlow.FlowDirection.Down;
			}
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.FlowDirection flowDirection2 = (flowDirection + i - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.Conduit conduitFromDirection = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection2);
				if (conduitFromDirection.idx != -1)
				{
					SolidConduitFlow.ConduitContents contents = manager.grid[conduitFromDirection.GetCell(manager)].contents;
					if (contents.pickupableHandle.IsValid())
					{
						int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection.idx);
						if (permittedFlowDirections != -1)
						{
							SolidConduitFlow.FlowDirection direction = SolidConduitFlow.InverseFlow(flowDirection2);
							if (manager.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, direction).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit(direction)) != 0)
							{
								return flowDirection2;
							}
						}
					}
				}
			}
			for (int j = 0; j < 5; j++)
			{
				SolidConduitFlow.FlowDirection flowDirection3 = (manager.soaInfo.GetTargetFlowDirection(this.idx) + j - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(flowDirection3);
				SolidConduitFlow.Conduit conduitFromDirection2 = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection3);
				if (conduitFromDirection2.idx != -1)
				{
					int permittedFlowDirections2 = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection2.idx);
					if (permittedFlowDirections2 != -1 && (permittedFlowDirections2 & SolidConduitFlow.FlowBit(direction2)) != 0)
					{
						return flowDirection3;
					}
				}
			}
			return SolidConduitFlow.FlowDirection.None;
		}

		// Token: 0x0600A178 RID: 41336 RVA: 0x003835FC File Offset: 0x003817FC
		public SolidConduitFlow.FlowDirection GetNextFlowTarget(SolidConduitFlow manager)
		{
			int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(this.idx);
			if (permittedFlowDirections == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			for (int i = 0; i < 5; i++)
			{
				int num = (manager.soaInfo.GetTargetFlowDirection(this.idx) + i - SolidConduitFlow.FlowDirection.Left + 1) % 5 + 1;
				if (manager.soaInfo.GetConduitFromDirection(this.idx, (SolidConduitFlow.FlowDirection)num).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit((SolidConduitFlow.FlowDirection)num)) != 0)
				{
					return (SolidConduitFlow.FlowDirection)num;
				}
			}
			return SolidConduitFlow.FlowDirection.Blocked;
		}

		// Token: 0x0600A179 RID: 41337 RVA: 0x00383670 File Offset: 0x00381870
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x0600A17A RID: 41338 RVA: 0x00383683 File Offset: 0x00381883
		public SolidConduitFlow.ConduitContents GetInitialContents(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x0600A17B RID: 41339 RVA: 0x00383696 File Offset: 0x00381896
		public int GetCell(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x0600A17C RID: 41340 RVA: 0x003836A9 File Offset: 0x003818A9
		public bool Equals(SolidConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x04007E0D RID: 32269
		public int idx;
	}

	// Token: 0x02001AE4 RID: 6884
	[DebuggerDisplay("{pickupable}")]
	public struct ConduitContents
	{
		// Token: 0x0600A17D RID: 41341 RVA: 0x003836B9 File Offset: 0x003818B9
		public ConduitContents(HandleVector<int>.Handle pickupable_handle)
		{
			this.pickupableHandle = pickupable_handle;
		}

		// Token: 0x0600A17E RID: 41342 RVA: 0x003836C4 File Offset: 0x003818C4
		public static SolidConduitFlow.ConduitContents EmptyContents()
		{
			return new SolidConduitFlow.ConduitContents
			{
				pickupableHandle = HandleVector<int>.InvalidHandle
			};
		}

		// Token: 0x04007E0E RID: 32270
		public HandleVector<int>.Handle pickupableHandle;
	}
}
