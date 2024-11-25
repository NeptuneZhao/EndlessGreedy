using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A7E RID: 2686
[AddComponentMenu("KMonoBehaviour/scripts/GameScenePartitioner")]
public class GameScenePartitioner : KMonoBehaviour
{
	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x06004EAC RID: 20140 RVA: 0x001C5129 File Offset: 0x001C3329
	public static GameScenePartitioner Instance
	{
		get
		{
			global::Debug.Assert(GameScenePartitioner.instance != null);
			return GameScenePartitioner.instance;
		}
	}

	// Token: 0x06004EAD RID: 20141 RVA: 0x001C5140 File Offset: 0x001C3340
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GameScenePartitioner.instance == null);
		GameScenePartitioner.instance = this;
		this.partitioner = new ScenePartitioner(16, 66, Grid.WidthInCells, Grid.HeightInCells);
		this.solidChangedLayer = this.partitioner.CreateMask("SolidChanged");
		this.liquidChangedLayer = this.partitioner.CreateMask("LiquidChanged");
		this.digDestroyedLayer = this.partitioner.CreateMask("DigDestroyed");
		this.fogOfWarChangedLayer = this.partitioner.CreateMask("FogOfWarChanged");
		this.decorProviderLayer = this.partitioner.CreateMask("DecorProviders");
		this.attackableEntitiesLayer = this.partitioner.CreateMask("FactionedEntities");
		this.fetchChoreLayer = this.partitioner.CreateMask("FetchChores");
		this.pickupablesLayer = this.partitioner.CreateMask("Pickupables");
		this.storedPickupablesLayer = this.partitioner.CreateMask("StoredPickupables");
		this.pickupablesChangedLayer = this.partitioner.CreateMask("PickupablesChanged");
		this.gasConduitsLayer = this.partitioner.CreateMask("GasConduit");
		this.liquidConduitsLayer = this.partitioner.CreateMask("LiquidConduit");
		this.solidConduitsLayer = this.partitioner.CreateMask("SolidConduit");
		this.noisePolluterLayer = this.partitioner.CreateMask("NoisePolluters");
		this.validNavCellChangedLayer = this.partitioner.CreateMask("validNavCellChangedLayer");
		this.dirtyNavCellUpdateLayer = this.partitioner.CreateMask("dirtyNavCellUpdateLayer");
		this.trapsLayer = this.partitioner.CreateMask("trapsLayer");
		this.floorSwitchActivatorLayer = this.partitioner.CreateMask("FloorSwitchActivatorLayer");
		this.floorSwitchActivatorChangedLayer = this.partitioner.CreateMask("FloorSwitchActivatorChangedLayer");
		this.collisionLayer = this.partitioner.CreateMask("Collision");
		this.lure = this.partitioner.CreateMask("Lure");
		this.plants = this.partitioner.CreateMask("Plants");
		this.industrialBuildings = this.partitioner.CreateMask("IndustrialBuildings");
		this.completeBuildings = this.partitioner.CreateMask("CompleteBuildings");
		this.prioritizableObjects = this.partitioner.CreateMask("PrioritizableObjects");
		this.contactConductiveLayer = this.partitioner.CreateMask("ContactConductiveLayer");
		this.objectLayers = new ScenePartitionerLayer[45];
		for (int i = 0; i < 45; i++)
		{
			ObjectLayer objectLayer = (ObjectLayer)i;
			this.objectLayers[i] = this.partitioner.CreateMask(objectLayer.ToString());
		}
	}

	// Token: 0x06004EAE RID: 20142 RVA: 0x001C53F4 File Offset: 0x001C35F4
	protected override void OnForcedCleanUp()
	{
		GameScenePartitioner.instance = null;
		this.partitioner.FreeResources();
		this.partitioner = null;
		this.solidChangedLayer = null;
		this.liquidChangedLayer = null;
		this.digDestroyedLayer = null;
		this.fogOfWarChangedLayer = null;
		this.decorProviderLayer = null;
		this.attackableEntitiesLayer = null;
		this.fetchChoreLayer = null;
		this.pickupablesLayer = null;
		this.storedPickupablesLayer = null;
		this.pickupablesChangedLayer = null;
		this.gasConduitsLayer = null;
		this.liquidConduitsLayer = null;
		this.solidConduitsLayer = null;
		this.noisePolluterLayer = null;
		this.validNavCellChangedLayer = null;
		this.dirtyNavCellUpdateLayer = null;
		this.trapsLayer = null;
		this.floorSwitchActivatorLayer = null;
		this.floorSwitchActivatorChangedLayer = null;
		this.contactConductiveLayer = null;
		this.objectLayers = null;
	}

	// Token: 0x06004EAF RID: 20143 RVA: 0x001C54AC File Offset: 0x001C36AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		navGrid.OnNavGridUpdateComplete = (Action<IEnumerable<int>>)Delegate.Combine(navGrid.OnNavGridUpdateComplete, new Action<IEnumerable<int>>(this.OnNavGridUpdateComplete));
		NavTable navTable = navGrid.NavTable;
		navTable.OnValidCellChanged = (Action<int, NavType>)Delegate.Combine(navTable.OnValidCellChanged, new Action<int, NavType>(this.OnValidNavCellChanged));
	}

	// Token: 0x06004EB0 RID: 20144 RVA: 0x001C5518 File Offset: 0x001C3718
	public HandleVector<int>.Handle Add(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		ScenePartitionerEntry scenePartitionerEntry = new ScenePartitionerEntry(name, obj, x, y, width, height, layer, this.partitioner, event_callback);
		this.partitioner.Add(scenePartitionerEntry);
		return this.scenePartitionerEntries.Allocate(scenePartitionerEntry);
	}

	// Token: 0x06004EB1 RID: 20145 RVA: 0x001C5558 File Offset: 0x001C3758
	public HandleVector<int>.Handle Add(string name, object obj, Extents extents, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		return this.Add(name, obj, extents.x, extents.y, extents.width, extents.height, layer, event_callback);
	}

	// Token: 0x06004EB2 RID: 20146 RVA: 0x001C558C File Offset: 0x001C378C
	public HandleVector<int>.Handle Add(string name, object obj, int cell, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return this.Add(name, obj, x, y, 1, 1, layer, event_callback);
	}

	// Token: 0x06004EB3 RID: 20147 RVA: 0x001C55B7 File Offset: 0x001C37B7
	public void AddGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Combine(layer.OnEvent, action);
	}

	// Token: 0x06004EB4 RID: 20148 RVA: 0x001C55D0 File Offset: 0x001C37D0
	public void RemoveGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Remove(layer.OnEvent, action);
	}

	// Token: 0x06004EB5 RID: 20149 RVA: 0x001C55E9 File Offset: 0x001C37E9
	public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(cells, layer, event_data);
	}

	// Token: 0x06004EB6 RID: 20150 RVA: 0x001C55F9 File Offset: 0x001C37F9
	public void TriggerEvent(Extents extents, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(extents.x, extents.y, extents.width, extents.height, layer, event_data);
	}

	// Token: 0x06004EB7 RID: 20151 RVA: 0x001C5620 File Offset: 0x001C3820
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(x, y, width, height, layer, event_data);
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x001C5638 File Offset: 0x001C3838
	public void TriggerEvent(int cell, ScenePartitionerLayer layer, object event_data)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		this.TriggerEvent(x, y, 1, 1, layer, event_data);
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x001C565F File Offset: 0x001C385F
	public void GatherEntries(Extents extents, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.GatherEntries(extents.x, extents.y, extents.width, extents.height, layer, gathered_entries);
	}

	// Token: 0x06004EBA RID: 20154 RVA: 0x001C5681 File Offset: 0x001C3881
	public void GatherEntries(int x_bottomLeft, int y_bottomLeft, int width, int height, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.partitioner.GatherEntries(x_bottomLeft, y_bottomLeft, width, height, layer, null, gathered_entries);
	}

	// Token: 0x06004EBB RID: 20155 RVA: 0x001C5698 File Offset: 0x001C3898
	public void Iterate<IteratorType>(int x, int y, int width, int height, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x, y, width, height, layer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = pooledList[i];
			iterator.Iterate(scenePartitionerEntry.obj);
		}
		pooledList.Recycle();
	}

	// Token: 0x06004EBC RID: 20156 RVA: 0x001C56F0 File Offset: 0x001C38F0
	public void Iterate<IteratorType>(int cell, int radius, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.Iterate<IteratorType>(num - radius, num2 - radius, radius * 2, radius * 2, layer, ref iterator);
	}

	// Token: 0x06004EBD RID: 20157 RVA: 0x001C5720 File Offset: 0x001C3920
	public IEnumerable<object> AsyncSafeEnumerate(int x, int y, int width, int height, ScenePartitionerLayer layer)
	{
		return this.partitioner.AsyncSafeEnumerate(x, y, width, height, layer);
	}

	// Token: 0x06004EBE RID: 20158 RVA: 0x001C5734 File Offset: 0x001C3934
	public IEnumerable<object> AsyncSafeEnumerate(int cell, int radius, ScenePartitionerLayer layer)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		return this.AsyncSafeEnumerate(num - radius, num2 - radius, radius * 2, radius * 2, layer);
	}

	// Token: 0x06004EBF RID: 20159 RVA: 0x001C5762 File Offset: 0x001C3962
	private void OnValidNavCellChanged(int cell, NavType nav_type)
	{
		this.changedCells.Add(cell);
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x001C5770 File Offset: 0x001C3970
	private void OnNavGridUpdateComplete(IEnumerable<int> dirty_nav_cells)
	{
		GameScenePartitioner.Instance.TriggerEvent(dirty_nav_cells, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, null);
		if (this.changedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.validNavCellChangedLayer, null);
			this.changedCells.Clear();
		}
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001C57C8 File Offset: 0x001C39C8
	public void UpdatePosition(HandleVector<int>.Handle handle, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.UpdatePosition(handle, vector2I.x, vector2I.y);
	}

	// Token: 0x06004EC2 RID: 20162 RVA: 0x001C57EF File Offset: 0x001C39EF
	public void UpdatePosition(HandleVector<int>.Handle handle, int x, int y)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(x, y);
	}

	// Token: 0x06004EC3 RID: 20163 RVA: 0x001C580E File Offset: 0x001C3A0E
	public void UpdatePosition(HandleVector<int>.Handle handle, Extents ext)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(ext);
	}

	// Token: 0x06004EC4 RID: 20164 RVA: 0x001C582C File Offset: 0x001C3A2C
	public void Free(ref HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).Release();
		this.scenePartitionerEntries.Free(handle);
		handle.Clear();
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x001C5865 File Offset: 0x001C3A65
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.partitioner.Cleanup();
	}

	// Token: 0x06004EC6 RID: 20166 RVA: 0x001C5878 File Offset: 0x001C3A78
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		return this.partitioner.DoDebugLayersContainItemsOnCell(cell);
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001C5886 File Offset: 0x001C3A86
	public List<ScenePartitionerLayer> GetLayers()
	{
		return this.partitioner.layers;
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001C5893 File Offset: 0x001C3A93
	public void SetToggledLayers(HashSet<ScenePartitionerLayer> toggled_layers)
	{
		this.partitioner.toggledLayers = toggled_layers;
	}

	// Token: 0x04003445 RID: 13381
	public ScenePartitionerLayer solidChangedLayer;

	// Token: 0x04003446 RID: 13382
	public ScenePartitionerLayer liquidChangedLayer;

	// Token: 0x04003447 RID: 13383
	public ScenePartitionerLayer digDestroyedLayer;

	// Token: 0x04003448 RID: 13384
	public ScenePartitionerLayer fogOfWarChangedLayer;

	// Token: 0x04003449 RID: 13385
	public ScenePartitionerLayer decorProviderLayer;

	// Token: 0x0400344A RID: 13386
	public ScenePartitionerLayer attackableEntitiesLayer;

	// Token: 0x0400344B RID: 13387
	public ScenePartitionerLayer fetchChoreLayer;

	// Token: 0x0400344C RID: 13388
	public ScenePartitionerLayer pickupablesLayer;

	// Token: 0x0400344D RID: 13389
	public ScenePartitionerLayer storedPickupablesLayer;

	// Token: 0x0400344E RID: 13390
	public ScenePartitionerLayer pickupablesChangedLayer;

	// Token: 0x0400344F RID: 13391
	public ScenePartitionerLayer gasConduitsLayer;

	// Token: 0x04003450 RID: 13392
	public ScenePartitionerLayer liquidConduitsLayer;

	// Token: 0x04003451 RID: 13393
	public ScenePartitionerLayer solidConduitsLayer;

	// Token: 0x04003452 RID: 13394
	public ScenePartitionerLayer wiresLayer;

	// Token: 0x04003453 RID: 13395
	public ScenePartitionerLayer[] objectLayers;

	// Token: 0x04003454 RID: 13396
	public ScenePartitionerLayer noisePolluterLayer;

	// Token: 0x04003455 RID: 13397
	public ScenePartitionerLayer validNavCellChangedLayer;

	// Token: 0x04003456 RID: 13398
	public ScenePartitionerLayer dirtyNavCellUpdateLayer;

	// Token: 0x04003457 RID: 13399
	public ScenePartitionerLayer trapsLayer;

	// Token: 0x04003458 RID: 13400
	public ScenePartitionerLayer floorSwitchActivatorLayer;

	// Token: 0x04003459 RID: 13401
	public ScenePartitionerLayer floorSwitchActivatorChangedLayer;

	// Token: 0x0400345A RID: 13402
	public ScenePartitionerLayer collisionLayer;

	// Token: 0x0400345B RID: 13403
	public ScenePartitionerLayer lure;

	// Token: 0x0400345C RID: 13404
	public ScenePartitionerLayer plants;

	// Token: 0x0400345D RID: 13405
	public ScenePartitionerLayer industrialBuildings;

	// Token: 0x0400345E RID: 13406
	public ScenePartitionerLayer completeBuildings;

	// Token: 0x0400345F RID: 13407
	public ScenePartitionerLayer prioritizableObjects;

	// Token: 0x04003460 RID: 13408
	public ScenePartitionerLayer contactConductiveLayer;

	// Token: 0x04003461 RID: 13409
	private ScenePartitioner partitioner;

	// Token: 0x04003462 RID: 13410
	private static GameScenePartitioner instance;

	// Token: 0x04003463 RID: 13411
	private KCompactedVector<ScenePartitionerEntry> scenePartitionerEntries = new KCompactedVector<ScenePartitionerEntry>(0);

	// Token: 0x04003464 RID: 13412
	private List<int> changedCells = new List<int>();

	// Token: 0x02001AB2 RID: 6834
	public interface Iterator
	{
		// Token: 0x0600A0E6 RID: 41190
		void Iterate(object obj);

		// Token: 0x0600A0E7 RID: 41191
		void Cleanup();
	}
}
