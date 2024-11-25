using System;
using System.Collections.Generic;

// Token: 0x02000A7F RID: 2687
public class ScenePartitioner : ISim1000ms
{
	// Token: 0x06004ECA RID: 20170 RVA: 0x001C58C0 File Offset: 0x001C3AC0
	public ScenePartitioner(int node_size, int layer_count, int scene_width, int scene_height)
	{
		this.nodeSize = node_size;
		int num = scene_width / node_size;
		int num2 = scene_height / node_size;
		this.nodes = new ScenePartitioner.ScenePartitionerNode[layer_count, num2, num];
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					this.nodes[i, j, k].entries = new HashSet<ScenePartitionerEntry>();
				}
			}
		}
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06004ECB RID: 20171 RVA: 0x001C5980 File Offset: 0x001C3B80
	public void FreeResources()
	{
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[i, j, k].entries)
					{
						if (scenePartitionerEntry != null)
						{
							scenePartitionerEntry.partitioner = null;
							scenePartitionerEntry.obj = null;
						}
					}
					this.nodes[i, j, k].entries.Clear();
				}
			}
		}
		this.nodes = null;
	}

	// Token: 0x06004ECC RID: 20172 RVA: 0x001C5A60 File Offset: 0x001C3C60
	[Obsolete]
	public ScenePartitionerLayer CreateMask(HashedString name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x001C5B00 File Offset: 0x001C3D00
	public ScenePartitionerLayer CreateMask(string name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		HashCache.Get().Add(name);
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x001C5BB8 File Offset: 0x001C3DB8
	private int ClampNodeX(int x)
	{
		return Math.Min(Math.Max(x, 0), this.nodes.GetLength(2) - 1);
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001C5BD4 File Offset: 0x001C3DD4
	private int ClampNodeY(int y)
	{
		return Math.Min(Math.Max(y, 0), this.nodes.GetLength(1) - 1);
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001C5BF0 File Offset: 0x001C3DF0
	private Extents GetNodeExtents(int x, int y, int width, int height)
	{
		Extents extents = default(Extents);
		extents.x = this.ClampNodeX(x / this.nodeSize);
		extents.y = this.ClampNodeY(y / this.nodeSize);
		extents.width = 1 + this.ClampNodeX((x + width) / this.nodeSize) - extents.x;
		extents.height = 1 + this.ClampNodeY((y + height) / this.nodeSize) - extents.y;
		return extents;
	}

	// Token: 0x06004ED1 RID: 20177 RVA: 0x001C5C71 File Offset: 0x001C3E71
	private Extents GetNodeExtents(ScenePartitionerEntry entry)
	{
		return this.GetNodeExtents(entry.x, entry.y, entry.width, entry.height);
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x001C5C94 File Offset: 0x001C3E94
	private void Insert(ScenePartitionerEntry entry)
	{
		if (entry.obj == null)
		{
			Debug.LogWarning("Trying to put null go into scene partitioner");
			return;
		}
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
				this.nodes[layer, i, j].entries.Add(entry);
			}
		}
	}

	// Token: 0x06004ED3 RID: 20179 RVA: 0x001C5E8C File Offset: 0x001C408C
	private void Widthdraw(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (this.nodes[layer, i, j].entries.Remove(entry) && !this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001C6054 File Offset: 0x001C4254
	public ScenePartitionerEntry Add(ScenePartitionerEntry entry)
	{
		this.Insert(entry);
		return entry;
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001C605E File Offset: 0x001C425E
	public void UpdatePosition(int x, int y, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = x;
		entry.y = y;
		this.Insert(entry);
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x001C607C File Offset: 0x001C427C
	public void UpdatePosition(Extents e, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = e.x;
		entry.y = e.y;
		entry.width = e.width;
		entry.height = e.height;
		this.Insert(entry);
	}

	// Token: 0x06004ED7 RID: 20183 RVA: 0x001C60BC File Offset: 0x001C42BC
	public void Remove(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
		entry.obj = null;
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x001C626C File Offset: 0x001C446C
	public void Sim1000ms(float dt)
	{
		foreach (ScenePartitioner.DirtyNode dirtyNode in this.dirtyNodes)
		{
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries.RemoveWhere(ScenePartitioner.removeCallback);
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].dirty = false;
		}
		this.dirtyNodes.Clear();
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x001C6314 File Offset: 0x001C4514
	public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.queryId++;
		foreach (int cell in cells)
		{
			int x = 0;
			int y = 0;
			Grid.CellToXY(cell, out x, out y);
			this.GatherEntries(x, y, 1, 1, layer, event_data, pooledList, this.queryId);
		}
		this.RunLayerGlobalEvent(cells, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x06004EDA RID: 20186 RVA: 0x001C63A0 File Offset: 0x001C45A0
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.GatherEntries(x, y, width, height, layer, event_data, pooledList);
		this.RunLayerGlobalEvent(x, y, width, height, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x06004EDB RID: 20187 RVA: 0x001C63E4 File Offset: 0x001C45E4
	private void RunLayerGlobalEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			foreach (int arg in cells)
			{
				layer.OnEvent(arg, event_data);
			}
		}
	}

	// Token: 0x06004EDC RID: 20188 RVA: 0x001C643C File Offset: 0x001C463C
	private void RunLayerGlobalEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			for (int i = y; i < y + height; i++)
			{
				for (int j = x; j < x + width; j++)
				{
					int num = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(num))
					{
						layer.OnEvent(num, event_data);
					}
				}
			}
		}
	}

	// Token: 0x06004EDD RID: 20189 RVA: 0x001C6490 File Offset: 0x001C4690
	private void RunEntries(List<ScenePartitionerEntry> gathered_entries, object event_data)
	{
		for (int i = 0; i < gathered_entries.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = gathered_entries[i];
			if (scenePartitionerEntry.obj != null && scenePartitionerEntry.eventCallback != null)
			{
				scenePartitionerEntry.eventCallback(event_data);
			}
		}
	}

	// Token: 0x06004EDE RID: 20190 RVA: 0x001C64D4 File Offset: 0x001C46D4
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries)
	{
		int query_id = this.queryId + 1;
		this.queryId = query_id;
		this.GatherEntries(x, y, width, height, layer, event_data, gathered_entries, query_id);
	}

	// Token: 0x06004EDF RID: 20191 RVA: 0x001C6504 File Offset: 0x001C4704
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries, int query_id)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer2, i, j].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.queryId != this.queryId)
					{
						if (scenePartitionerEntry.obj == null)
						{
							pooledList.Add(scenePartitionerEntry);
						}
						else if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1)
						{
							scenePartitionerEntry.queryId = this.queryId;
							gathered_entries.Add(scenePartitionerEntry);
						}
					}
				}
				this.nodes[layer2, i, j].entries.ExceptWith(pooledList);
				pooledList.Recycle();
			}
		}
	}

	// Token: 0x06004EE0 RID: 20192 RVA: 0x001C66AC File Offset: 0x001C48AC
	public IEnumerable<object> AsyncSafeEnumerate(int x, int y, int width, int height, ScenePartitionerLayer layer)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int max_y = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num = Math.Max(nodeExtents.y, 0);
		int start_x = Math.Max(nodeExtents.x, 0);
		int max_x = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer_idx = layer.layer;
		int num2;
		for (int node_y = num; node_y < max_y; node_y = num2)
		{
			for (int node_x = start_x; node_x < max_x; node_x = num2)
			{
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer_idx, node_y, node_x].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.obj != null && x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1)
					{
						yield return scenePartitionerEntry.obj;
					}
				}
				HashSet<ScenePartitionerEntry>.Enumerator enumerator = default(HashSet<ScenePartitionerEntry>.Enumerator);
				num2 = node_x + 1;
			}
			num2 = node_y + 1;
		}
		yield break;
		yield break;
	}

	// Token: 0x06004EE1 RID: 20193 RVA: 0x001C66E1 File Offset: 0x001C48E1
	public void Cleanup()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06004EE2 RID: 20194 RVA: 0x001C66F0 File Offset: 0x001C48F0
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		int x_bottomLeft = 0;
		int y_bottomLeft = 0;
		Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
		List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
		foreach (ScenePartitionerLayer layer in this.toggledLayers)
		{
			list.Clear();
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, layer, list);
			if (list.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04003465 RID: 13413
	public List<ScenePartitionerLayer> layers = new List<ScenePartitionerLayer>();

	// Token: 0x04003466 RID: 13414
	private int nodeSize;

	// Token: 0x04003467 RID: 13415
	private List<ScenePartitioner.DirtyNode> dirtyNodes = new List<ScenePartitioner.DirtyNode>();

	// Token: 0x04003468 RID: 13416
	private ScenePartitioner.ScenePartitionerNode[,,] nodes;

	// Token: 0x04003469 RID: 13417
	private int queryId;

	// Token: 0x0400346A RID: 13418
	private static readonly Predicate<ScenePartitionerEntry> removeCallback = (ScenePartitionerEntry entry) => entry == null || entry.obj == null;

	// Token: 0x0400346B RID: 13419
	public HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

	// Token: 0x02001AB3 RID: 6835
	private struct ScenePartitionerNode
	{
		// Token: 0x04007D57 RID: 32087
		public HashSet<ScenePartitionerEntry> entries;

		// Token: 0x04007D58 RID: 32088
		public bool dirty;
	}

	// Token: 0x02001AB4 RID: 6836
	private struct DirtyNode
	{
		// Token: 0x04007D59 RID: 32089
		public int layer;

		// Token: 0x04007D5A RID: 32090
		public int x;

		// Token: 0x04007D5B RID: 32091
		public int y;
	}
}
