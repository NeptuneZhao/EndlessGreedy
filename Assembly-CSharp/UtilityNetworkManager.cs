using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B4F RID: 2895
public class UtilityNetworkManager<NetworkType, ItemType> : IUtilityNetworkMgr where NetworkType : UtilityNetwork, new() where ItemType : MonoBehaviour
{
	// Token: 0x1700066D RID: 1645
	// (get) Token: 0x06005662 RID: 22114 RVA: 0x001ED902 File Offset: 0x001EBB02
	public bool IsDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x06005663 RID: 22115 RVA: 0x001ED90C File Offset: 0x001EBB0C
	public UtilityNetworkManager(int game_width, int game_height, int tile_layer)
	{
		this.tileLayer = tile_layer;
		this.networks = new List<UtilityNetwork>();
		this.Initialize(game_width, game_height);
	}

	// Token: 0x06005664 RID: 22116 RVA: 0x001ED998 File Offset: 0x001EBB98
	public void Initialize(int game_width, int game_height)
	{
		this.networks.Clear();
		this.physicalGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.visualGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.stashedVisualGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.physicalNodes = new HashSet<int>();
		this.visualNodes = new HashSet<int>();
		this.visitedCells = new HashSet<int>();
		this.visitedVirtualKeys = new HashSet<object>();
		this.queuedVirtualKeys = new HashSet<object>();
		for (int i = 0; i < this.visualGrid.Length; i++)
		{
			this.visualGrid[i] = new UtilityNetworkGridNode
			{
				networkIdx = -1,
				connections = (UtilityConnections)0
			};
			this.physicalGrid[i] = new UtilityNetworkGridNode
			{
				networkIdx = -1,
				connections = (UtilityConnections)0
			};
		}
	}

	// Token: 0x06005665 RID: 22117 RVA: 0x001EDA70 File Offset: 0x001EBC70
	public void Update()
	{
		if (this.dirty)
		{
			this.dirty = false;
			for (int i = 0; i < this.networks.Count; i++)
			{
				this.networks[i].Reset(this.physicalGrid);
			}
			this.networks.Clear();
			this.virtualKeyToNetworkIdx.Clear();
			this.RebuildNetworks(this.tileLayer, false);
			this.RebuildNetworks(this.tileLayer, true);
			if (this.onNetworksRebuilt != null)
			{
				this.onNetworksRebuilt(this.networks, this.GetNodes(true));
			}
		}
	}

	// Token: 0x06005666 RID: 22118 RVA: 0x001EDB0C File Offset: 0x001EBD0C
	protected UtilityNetworkGridNode[] GetGrid(bool is_physical_building)
	{
		if (!is_physical_building)
		{
			return this.visualGrid;
		}
		return this.physicalGrid;
	}

	// Token: 0x06005667 RID: 22119 RVA: 0x001EDB1E File Offset: 0x001EBD1E
	private HashSet<int> GetNodes(bool is_physical_building)
	{
		if (!is_physical_building)
		{
			return this.visualNodes;
		}
		return this.physicalNodes;
	}

	// Token: 0x06005668 RID: 22120 RVA: 0x001EDB30 File Offset: 0x001EBD30
	public void ClearCell(int cell, bool is_physical_building)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
		HashSet<int> nodes = this.GetNodes(is_physical_building);
		UtilityConnections connections = grid[cell].connections;
		grid[cell].connections = (UtilityConnections)0;
		Vector2I vector2I = Grid.CellToXY(cell);
		if (vector2I.x > 0 && (connections & UtilityConnections.Left) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array = grid;
			int num = Grid.CellLeft(cell);
			array[num].connections = (array[num].connections & ~UtilityConnections.Right);
		}
		if (vector2I.x < Grid.WidthInCells - 1 && (connections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array2 = grid;
			int num2 = Grid.CellRight(cell);
			array2[num2].connections = (array2[num2].connections & ~UtilityConnections.Left);
		}
		if (vector2I.y > 0 && (connections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array3 = grid;
			int num3 = Grid.CellBelow(cell);
			array3[num3].connections = (array3[num3].connections & ~UtilityConnections.Up);
		}
		if (vector2I.y < Grid.HeightInCells - 1 && (connections & UtilityConnections.Up) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array4 = grid;
			int num4 = Grid.CellAbove(cell);
			array4[num4].connections = (array4[num4].connections & ~UtilityConnections.Down);
		}
		nodes.Remove(cell);
		if (is_physical_building)
		{
			this.dirty = true;
			this.ClearCell(cell, false);
		}
	}

	// Token: 0x06005669 RID: 22121 RVA: 0x001EDC30 File Offset: 0x001EBE30
	private void QueueCellForVisit(UtilityNetworkGridNode[] grid, int dest_cell, UtilityConnections direction)
	{
		if (!Grid.IsValidCell(dest_cell))
		{
			return;
		}
		if (this.visitedCells.Contains(dest_cell))
		{
			return;
		}
		if (direction != (UtilityConnections)0 && (grid[dest_cell].connections & direction.InverseDirection()) == (UtilityConnections)0)
		{
			return;
		}
		if (Grid.Objects[dest_cell, this.tileLayer] != null)
		{
			this.visitedCells.Add(dest_cell);
			this.queued.Enqueue(dest_cell);
		}
	}

	// Token: 0x0600566A RID: 22122 RVA: 0x001EDCA0 File Offset: 0x001EBEA0
	public void ForceRebuildNetworks()
	{
		this.dirty = true;
	}

	// Token: 0x0600566B RID: 22123 RVA: 0x001EDCAC File Offset: 0x001EBEAC
	public void AddToNetworks(int cell, object item, bool is_endpoint)
	{
		if (item != null)
		{
			if (is_endpoint)
			{
				if (this.endpoints.ContainsKey(cell))
				{
					global::Debug.LogWarning(string.Format("Cell {0} already has a utility network endpoint assigned. Adding {1} will stomp previous endpoint, destroying the object that's already there.", cell, item.ToString()));
					KMonoBehaviour kmonoBehaviour = this.endpoints[cell] as KMonoBehaviour;
					if (kmonoBehaviour != null)
					{
						Util.KDestroyGameObject(kmonoBehaviour);
					}
				}
				this.endpoints[cell] = item;
			}
			else
			{
				if (this.items.ContainsKey(cell))
				{
					global::Debug.LogWarning(string.Format("Cell {0} already has a utility network connector assigned. Adding {1} will stomp previous item, destroying the object that's already there.", cell, item.ToString()));
					KMonoBehaviour kmonoBehaviour2 = this.items[cell] as KMonoBehaviour;
					if (kmonoBehaviour2 != null)
					{
						Util.KDestroyGameObject(kmonoBehaviour2);
					}
				}
				this.items[cell] = item;
			}
		}
		this.dirty = true;
	}

	// Token: 0x0600566C RID: 22124 RVA: 0x001EDD7C File Offset: 0x001EBF7C
	public void AddToVirtualNetworks(object key, object item, bool is_endpoint)
	{
		if (item != null)
		{
			if (is_endpoint)
			{
				if (!this.virtualEndpoints.ContainsKey(key))
				{
					this.virtualEndpoints[key] = new List<object>();
				}
				this.virtualEndpoints[key].Add(item);
			}
			else
			{
				if (!this.virtualItems.ContainsKey(key))
				{
					this.virtualItems[key] = new List<object>();
				}
				this.virtualItems[key].Add(item);
			}
		}
		this.dirty = true;
	}

	// Token: 0x0600566D RID: 22125 RVA: 0x001EDDFC File Offset: 0x001EBFFC
	private unsafe void Reconnect(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		int* ptr3 = stackalloc int[(UIntPtr)16];
		int num = 0;
		if (vector2I.y < Grid.HeightInCells - 1)
		{
			ptr[num] = Grid.CellAbove(cell);
			ptr2[num] = 4;
			ptr3[num] = 8;
			num++;
		}
		if (vector2I.y > 0)
		{
			ptr[num] = Grid.CellBelow(cell);
			ptr2[num] = 8;
			ptr3[num] = 4;
			num++;
		}
		if (vector2I.x > 0)
		{
			ptr[num] = Grid.CellLeft(cell);
			ptr2[num] = 1;
			ptr3[num] = 2;
			num++;
		}
		if (vector2I.x < Grid.WidthInCells - 1)
		{
			ptr[num] = Grid.CellRight(cell);
			ptr2[num] = 2;
			ptr3[num] = 1;
			num++;
		}
		UtilityConnections connections = this.physicalGrid[cell].connections;
		UtilityConnections connections2 = this.visualGrid[cell].connections;
		for (int i = 0; i < num; i++)
		{
			int num2 = ptr[i];
			UtilityConnections utilityConnections = (UtilityConnections)ptr2[i];
			UtilityConnections utilityConnections2 = (UtilityConnections)ptr3[i];
			if ((connections & utilityConnections) != (UtilityConnections)0)
			{
				if (this.physicalNodes.Contains(num2))
				{
					UtilityNetworkGridNode[] array = this.physicalGrid;
					int num3 = num2;
					array[num3].connections = (array[num3].connections | utilityConnections2);
				}
				if (this.visualNodes.Contains(num2))
				{
					UtilityNetworkGridNode[] array2 = this.visualGrid;
					int num4 = num2;
					array2[num4].connections = (array2[num4].connections | utilityConnections2);
				}
			}
			else if ((connections2 & utilityConnections) != (UtilityConnections)0 && (this.physicalNodes.Contains(num2) || this.visualNodes.Contains(num2)))
			{
				UtilityNetworkGridNode[] array3 = this.visualGrid;
				int num5 = num2;
				array3[num5].connections = (array3[num5].connections | utilityConnections2);
			}
		}
	}

	// Token: 0x0600566E RID: 22126 RVA: 0x001EDFDC File Offset: 0x001EC1DC
	public void RemoveFromVirtualNetworks(object key, object item, bool is_endpoint)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.dirty = true;
		if (item != null)
		{
			if (is_endpoint)
			{
				this.virtualEndpoints[key].Remove(item);
				if (this.virtualEndpoints[key].Count == 0)
				{
					this.virtualEndpoints.Remove(key);
				}
			}
			else
			{
				this.virtualItems[key].Remove(item);
				if (this.virtualItems[key].Count == 0)
				{
					this.virtualItems.Remove(key);
				}
			}
			UtilityNetwork networkForVirtualKey = this.GetNetworkForVirtualKey(key);
			if (networkForVirtualKey != null)
			{
				networkForVirtualKey.RemoveItem(item);
			}
		}
	}

	// Token: 0x0600566F RID: 22127 RVA: 0x001EE078 File Offset: 0x001EC278
	public void RemoveFromNetworks(int cell, object item, bool is_endpoint)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.dirty = true;
		if (item != null)
		{
			if (is_endpoint)
			{
				this.endpoints.Remove(cell);
				int networkIdx = this.physicalGrid[cell].networkIdx;
				if (networkIdx != -1)
				{
					this.networks[networkIdx].RemoveItem(item);
					return;
				}
			}
			else
			{
				int networkIdx2 = this.physicalGrid[cell].networkIdx;
				this.physicalGrid[cell].connections = (UtilityConnections)0;
				this.physicalGrid[cell].networkIdx = -1;
				this.items.Remove(cell);
				this.Disconnect(cell);
				object item2;
				if (this.endpoints.TryGetValue(cell, out item2) && networkIdx2 != -1)
				{
					this.networks[networkIdx2].DisconnectItem(item2);
				}
			}
		}
	}

	// Token: 0x06005670 RID: 22128 RVA: 0x001EE148 File Offset: 0x001EC348
	private unsafe void Disconnect(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		int num = 0;
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		if (vector2I.y < Grid.HeightInCells - 1)
		{
			ptr[num] = Grid.CellAbove(cell);
			ptr2[num] = -9;
			num++;
		}
		if (vector2I.y > 0)
		{
			ptr[num] = Grid.CellBelow(cell);
			ptr2[num] = -5;
			num++;
		}
		if (vector2I.x > 0)
		{
			ptr[num] = Grid.CellLeft(cell);
			ptr2[num] = -3;
			num++;
		}
		if (vector2I.x < Grid.WidthInCells - 1)
		{
			ptr[num] = Grid.CellRight(cell);
			ptr2[num] = -2;
			num++;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = ptr[i];
			int num3 = ptr2[i];
			int connections = (int)(this.physicalGrid[num2].connections & (UtilityConnections)num3);
			this.physicalGrid[num2].connections = (UtilityConnections)connections;
		}
	}

	// Token: 0x06005671 RID: 22129 RVA: 0x001EE254 File Offset: 0x001EC454
	private unsafe void RebuildNetworks(int layer, bool is_physical)
	{
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical);
		HashSet<int> nodes = this.GetNodes(is_physical);
		this.visitedCells.Clear();
		this.visitedVirtualKeys.Clear();
		this.queuedVirtualKeys.Clear();
		this.queued.Clear();
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		foreach (int num in nodes)
		{
			UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
			if (!this.visitedCells.Contains(num))
			{
				this.queued.Enqueue(num);
				this.visitedCells.Add(num);
				NetworkType networkType = Activator.CreateInstance<NetworkType>();
				networkType.id = this.networks.Count;
				this.networks.Add(networkType);
				while (this.queued.Count > 0)
				{
					int num2 = this.queued.Dequeue();
					utilityNetworkGridNode = grid[num2];
					object obj = null;
					object obj2 = null;
					if (is_physical)
					{
						if (this.items.TryGetValue(num2, out obj))
						{
							if (obj is IDisconnectable && (obj as IDisconnectable).IsDisconnected())
							{
								continue;
							}
							if (obj != null)
							{
								networkType.AddItem(obj);
							}
						}
						if (this.endpoints.TryGetValue(num2, out obj2) && obj2 != null)
						{
							networkType.AddItem(obj2);
						}
					}
					grid[num2].networkIdx = networkType.id;
					if (obj != null && obj2 != null)
					{
						networkType.ConnectItem(obj2);
					}
					Vector2I vector2I = Grid.CellToXY(num2);
					int num3 = 0;
					if (vector2I.x > 0)
					{
						ptr[num3] = Grid.CellLeft(num2);
						ptr2[num3] = 1;
						num3++;
					}
					if (vector2I.x < Grid.WidthInCells - 1)
					{
						ptr[num3] = Grid.CellRight(num2);
						ptr2[num3] = 2;
						num3++;
					}
					if (vector2I.y > 0)
					{
						ptr[num3] = Grid.CellBelow(num2);
						ptr2[num3] = 8;
						num3++;
					}
					if (vector2I.y < Grid.HeightInCells - 1)
					{
						ptr[num3] = Grid.CellAbove(num2);
						ptr2[num3] = 4;
						num3++;
					}
					for (int i = 0; i < num3; i++)
					{
						int num4 = ptr2[i];
						if ((utilityNetworkGridNode.connections & (UtilityConnections)num4) != (UtilityConnections)0)
						{
							int dest_cell = ptr[i];
							this.QueueCellForVisit(grid, dest_cell, (UtilityConnections)num4);
						}
					}
					int dest_cell2;
					if (this.links.TryGetValue(num2, out dest_cell2))
					{
						this.QueueCellForVisit(grid, dest_cell2, (UtilityConnections)0);
					}
					object obj3;
					if (this.semiVirtualLinks.TryGetValue(num2, out obj3) && !this.visitedVirtualKeys.Contains(obj3))
					{
						this.visitedVirtualKeys.Add(obj3);
						this.virtualKeyToNetworkIdx[obj3] = networkType.id;
						if (this.virtualItems.ContainsKey(obj3))
						{
							foreach (object item in this.virtualItems[obj3])
							{
								networkType.AddItem(item);
								networkType.ConnectItem(item);
							}
						}
						if (this.virtualEndpoints.ContainsKey(obj3))
						{
							foreach (object item2 in this.virtualEndpoints[obj3])
							{
								networkType.AddItem(item2);
								networkType.ConnectItem(item2);
							}
						}
						foreach (KeyValuePair<int, object> keyValuePair in this.semiVirtualLinks)
						{
							if (keyValuePair.Value == obj3)
							{
								this.QueueCellForVisit(grid, keyValuePair.Key, (UtilityConnections)0);
							}
						}
					}
				}
			}
		}
		foreach (KeyValuePair<object, List<object>> keyValuePair2 in this.virtualItems)
		{
			if (!this.visitedVirtualKeys.Contains(keyValuePair2.Key))
			{
				NetworkType networkType2 = Activator.CreateInstance<NetworkType>();
				networkType2.id = this.networks.Count;
				this.visitedVirtualKeys.Add(keyValuePair2.Key);
				this.virtualKeyToNetworkIdx[keyValuePair2.Key] = networkType2.id;
				foreach (object item3 in keyValuePair2.Value)
				{
					networkType2.AddItem(item3);
					networkType2.ConnectItem(item3);
				}
				foreach (object item4 in this.virtualEndpoints[keyValuePair2.Key])
				{
					networkType2.AddItem(item4);
					networkType2.ConnectItem(item4);
				}
				this.networks.Add(networkType2);
			}
		}
		foreach (KeyValuePair<object, List<object>> keyValuePair3 in this.virtualEndpoints)
		{
			if (!this.visitedVirtualKeys.Contains(keyValuePair3.Key))
			{
				NetworkType networkType3 = Activator.CreateInstance<NetworkType>();
				networkType3.id = this.networks.Count;
				this.visitedVirtualKeys.Add(keyValuePair3.Key);
				this.virtualKeyToNetworkIdx[keyValuePair3.Key] = networkType3.id;
				foreach (object item5 in this.virtualEndpoints[keyValuePair3.Key])
				{
					networkType3.AddItem(item5);
					networkType3.ConnectItem(item5);
				}
				this.networks.Add(networkType3);
			}
		}
	}

	// Token: 0x06005672 RID: 22130 RVA: 0x001EE9A8 File Offset: 0x001ECBA8
	public UtilityNetwork GetNetworkForVirtualKey(object key)
	{
		int index;
		if (this.virtualKeyToNetworkIdx.TryGetValue(key, out index))
		{
			return this.networks[index];
		}
		return null;
	}

	// Token: 0x06005673 RID: 22131 RVA: 0x001EE9D4 File Offset: 0x001ECBD4
	public UtilityNetwork GetNetworkByID(int id)
	{
		UtilityNetwork result = null;
		if (0 <= id && id < this.networks.Count)
		{
			result = this.networks[id];
		}
		return result;
	}

	// Token: 0x06005674 RID: 22132 RVA: 0x001EEA04 File Offset: 0x001ECC04
	public UtilityNetwork GetNetworkForCell(int cell)
	{
		UtilityNetwork result = null;
		if (Grid.IsValidCell(cell) && 0 <= this.physicalGrid[cell].networkIdx && this.physicalGrid[cell].networkIdx < this.networks.Count)
		{
			result = this.networks[this.physicalGrid[cell].networkIdx];
		}
		return result;
	}

	// Token: 0x06005675 RID: 22133 RVA: 0x001EEA6C File Offset: 0x001ECC6C
	public UtilityNetwork GetNetworkForDirection(int cell, Direction direction)
	{
		cell = Grid.GetCellInDirection(cell, direction);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		UtilityNetworkGridNode utilityNetworkGridNode = this.GetGrid(true)[cell];
		UtilityNetwork result = null;
		if (utilityNetworkGridNode.networkIdx != -1 && utilityNetworkGridNode.networkIdx < this.networks.Count)
		{
			result = this.networks[utilityNetworkGridNode.networkIdx];
		}
		return result;
	}

	// Token: 0x06005676 RID: 22134 RVA: 0x001EEACC File Offset: 0x001ECCCC
	private UtilityConnections GetNeighboursAsConnections(int cell, HashSet<int> nodes)
	{
		UtilityConnections utilityConnections = (UtilityConnections)0;
		Vector2I vector2I = Grid.CellToXY(cell);
		if (vector2I.x > 0 && nodes.Contains(Grid.CellLeft(cell)))
		{
			utilityConnections |= UtilityConnections.Left;
		}
		if (vector2I.x < Grid.WidthInCells - 1 && nodes.Contains(Grid.CellRight(cell)))
		{
			utilityConnections |= UtilityConnections.Right;
		}
		if (vector2I.y > 0 && nodes.Contains(Grid.CellBelow(cell)))
		{
			utilityConnections |= UtilityConnections.Down;
		}
		if (vector2I.y < Grid.HeightInCells - 1 && nodes.Contains(Grid.CellAbove(cell)))
		{
			utilityConnections |= UtilityConnections.Up;
		}
		return utilityConnections;
	}

	// Token: 0x06005677 RID: 22135 RVA: 0x001EEB5C File Offset: 0x001ECD5C
	public virtual void SetConnections(UtilityConnections connections, int cell, bool is_physical_building)
	{
		HashSet<int> nodes = this.GetNodes(is_physical_building);
		nodes.Add(cell);
		this.visualGrid[cell].connections = connections;
		if (is_physical_building)
		{
			this.dirty = true;
			UtilityConnections connections2 = is_physical_building ? (connections & this.GetNeighboursAsConnections(cell, nodes)) : connections;
			this.physicalGrid[cell].connections = connections2;
		}
		this.Reconnect(cell);
	}

	// Token: 0x06005678 RID: 22136 RVA: 0x001EEBC0 File Offset: 0x001ECDC0
	public UtilityConnections GetConnections(int cell, bool is_physical_building)
	{
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
		UtilityConnections utilityConnections = grid[cell].connections;
		if (!is_physical_building)
		{
			grid = this.GetGrid(true);
			utilityConnections |= grid[cell].connections;
		}
		return utilityConnections;
	}

	// Token: 0x06005679 RID: 22137 RVA: 0x001EEC00 File Offset: 0x001ECE00
	public UtilityConnections GetDisplayConnections(int cell)
	{
		UtilityConnections utilityConnections = (UtilityConnections)0;
		UtilityNetworkGridNode[] grid = this.GetGrid(false);
		UtilityConnections utilityConnections2 = utilityConnections | grid[cell].connections;
		grid = this.GetGrid(true);
		return utilityConnections2 | grid[cell].connections;
	}

	// Token: 0x0600567A RID: 22138 RVA: 0x001EEC38 File Offset: 0x001ECE38
	public virtual bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason)
	{
		fail_reason = null;
		return true;
	}

	// Token: 0x0600567B RID: 22139 RVA: 0x001EEC40 File Offset: 0x001ECE40
	public void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building)
	{
		string text;
		if (this.CanAddConnection(new_connection, cell, is_physical_building, out text))
		{
			if (is_physical_building)
			{
				this.dirty = true;
			}
			UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
			UtilityConnections connections = grid[cell].connections;
			grid[cell].connections = (connections | new_connection);
		}
	}

	// Token: 0x0600567C RID: 22140 RVA: 0x001EEC86 File Offset: 0x001ECE86
	public void StashVisualGrids()
	{
		Array.Copy(this.visualGrid, this.stashedVisualGrid, this.visualGrid.Length);
	}

	// Token: 0x0600567D RID: 22141 RVA: 0x001EECA1 File Offset: 0x001ECEA1
	public void UnstashVisualGrids()
	{
		Array.Copy(this.stashedVisualGrid, this.visualGrid, this.visualGrid.Length);
	}

	// Token: 0x0600567E RID: 22142 RVA: 0x001EECBC File Offset: 0x001ECEBC
	public string GetVisualizerString(int cell)
	{
		UtilityConnections displayConnections = this.GetDisplayConnections(cell);
		return this.GetVisualizerString(displayConnections);
	}

	// Token: 0x0600567F RID: 22143 RVA: 0x001EECD8 File Offset: 0x001ECED8
	public string GetVisualizerString(UtilityConnections connections)
	{
		string text = "";
		if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
		{
			text += "L";
		}
		if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			text += "R";
		}
		if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
		{
			text += "U";
		}
		if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			text += "D";
		}
		if (text == "")
		{
			text = "None";
		}
		return text;
	}

	// Token: 0x06005680 RID: 22144 RVA: 0x001EED44 File Offset: 0x001ECF44
	public object GetEndpoint(int cell)
	{
		object result = null;
		this.endpoints.TryGetValue(cell, out result);
		return result;
	}

	// Token: 0x06005681 RID: 22145 RVA: 0x001EED63 File Offset: 0x001ECF63
	public void AddSemiVirtualLink(int cell1, object virtualKey)
	{
		global::Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
		this.semiVirtualLinks[cell1] = virtualKey;
		this.dirty = true;
	}

	// Token: 0x06005682 RID: 22146 RVA: 0x001EED87 File Offset: 0x001ECF87
	public void RemoveSemiVirtualLink(int cell1, object virtualKey)
	{
		global::Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
		this.semiVirtualLinks.Remove(cell1);
		this.dirty = true;
	}

	// Token: 0x06005683 RID: 22147 RVA: 0x001EEDAB File Offset: 0x001ECFAB
	public void AddLink(int cell1, int cell2)
	{
		this.links[cell1] = cell2;
		this.links[cell2] = cell1;
		this.dirty = true;
	}

	// Token: 0x06005684 RID: 22148 RVA: 0x001EEDCE File Offset: 0x001ECFCE
	public void RemoveLink(int cell1, int cell2)
	{
		this.links.Remove(cell1);
		this.links.Remove(cell2);
		this.dirty = true;
	}

	// Token: 0x06005685 RID: 22149 RVA: 0x001EEDF1 File Offset: 0x001ECFF1
	public void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener)
	{
		this.onNetworksRebuilt = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Combine(this.onNetworksRebuilt, listener);
	}

	// Token: 0x06005686 RID: 22150 RVA: 0x001EEE0A File Offset: 0x001ED00A
	public void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener)
	{
		this.onNetworksRebuilt = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Remove(this.onNetworksRebuilt, listener);
	}

	// Token: 0x06005687 RID: 22151 RVA: 0x001EEE23 File Offset: 0x001ED023
	public IList<UtilityNetwork> GetNetworks()
	{
		return this.networks;
	}

	// Token: 0x04003886 RID: 14470
	private Dictionary<int, object> items = new Dictionary<int, object>();

	// Token: 0x04003887 RID: 14471
	private Dictionary<int, object> endpoints = new Dictionary<int, object>();

	// Token: 0x04003888 RID: 14472
	private Dictionary<object, List<object>> virtualItems = new Dictionary<object, List<object>>();

	// Token: 0x04003889 RID: 14473
	private Dictionary<object, List<object>> virtualEndpoints = new Dictionary<object, List<object>>();

	// Token: 0x0400388A RID: 14474
	private Dictionary<int, int> links = new Dictionary<int, int>();

	// Token: 0x0400388B RID: 14475
	private Dictionary<int, object> semiVirtualLinks = new Dictionary<int, object>();

	// Token: 0x0400388C RID: 14476
	private List<UtilityNetwork> networks;

	// Token: 0x0400388D RID: 14477
	private Dictionary<object, int> virtualKeyToNetworkIdx = new Dictionary<object, int>();

	// Token: 0x0400388E RID: 14478
	private HashSet<int> visitedCells;

	// Token: 0x0400388F RID: 14479
	private HashSet<object> visitedVirtualKeys;

	// Token: 0x04003890 RID: 14480
	private HashSet<object> queuedVirtualKeys;

	// Token: 0x04003891 RID: 14481
	private Action<IList<UtilityNetwork>, ICollection<int>> onNetworksRebuilt;

	// Token: 0x04003892 RID: 14482
	private Queue<int> queued = new Queue<int>();

	// Token: 0x04003893 RID: 14483
	protected UtilityNetworkGridNode[] visualGrid;

	// Token: 0x04003894 RID: 14484
	private UtilityNetworkGridNode[] stashedVisualGrid;

	// Token: 0x04003895 RID: 14485
	protected UtilityNetworkGridNode[] physicalGrid;

	// Token: 0x04003896 RID: 14486
	protected HashSet<int> physicalNodes;

	// Token: 0x04003897 RID: 14487
	protected HashSet<int> visualNodes;

	// Token: 0x04003898 RID: 14488
	private bool dirty;

	// Token: 0x04003899 RID: 14489
	private int tileLayer = -1;
}
