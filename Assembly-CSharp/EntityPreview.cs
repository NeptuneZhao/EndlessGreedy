using System;
using UnityEngine;

// Token: 0x02000806 RID: 2054
[AddComponentMenu("KMonoBehaviour/scripts/EntityPreview")]
public class EntityPreview : KMonoBehaviour
{
	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x060038BC RID: 14524 RVA: 0x00135ABD File Offset: 0x00133CBD
	// (set) Token: 0x060038BD RID: 14525 RVA: 0x00135AC5 File Offset: 0x00133CC5
	public bool Valid { get; private set; }

	// Token: 0x060038BE RID: 14526 RVA: 0x00135AD0 File Offset: 0x00133CD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnAreaChanged));
		if (this.objectLayer != ObjectLayer.NumLayers)
		{
			this.objectPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnAreaChanged));
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "EntityPreview.OnSpawn");
		this.OnAreaChanged(null);
	}

	// Token: 0x060038BF RID: 14527 RVA: 0x00135B98 File Offset: 0x00133D98
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x060038C0 RID: 14528 RVA: 0x00135BE7 File Offset: 0x00133DE7
	private void OnCellChange()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.occupyArea.GetExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, this.occupyArea.GetExtents());
		this.OnAreaChanged(null);
	}

	// Token: 0x060038C1 RID: 14529 RVA: 0x00135C26 File Offset: 0x00133E26
	public void SetSolid()
	{
		this.occupyArea.ApplyToCells = true;
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x00135C34 File Offset: 0x00133E34
	private void OnAreaChanged(object obj)
	{
		this.UpdateValidity();
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x00135C3C File Offset: 0x00133E3C
	public void UpdateValidity()
	{
		bool valid = this.Valid;
		this.Valid = this.occupyArea.TestArea(Grid.PosToCell(this), this, EntityPreview.ValidTestDelegate);
		if (this.Valid)
		{
			this.animController.TintColour = Color.white;
		}
		else
		{
			this.animController.TintColour = Color.red;
		}
		if (valid != this.Valid)
		{
			base.Trigger(-1820564715, this.Valid);
		}
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x00135CC0 File Offset: 0x00133EC0
	private static bool ValidTest(int cell, object data)
	{
		EntityPreview entityPreview = (EntityPreview)data;
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && (entityPreview.objectLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)entityPreview.objectLayer] == entityPreview.gameObject || Grid.Objects[cell, (int)entityPreview.objectLayer] == null);
	}

	// Token: 0x0400221C RID: 8732
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x0400221D RID: 8733
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x0400221E RID: 8734
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400221F RID: 8735
	public ObjectLayer objectLayer = ObjectLayer.NumLayers;

	// Token: 0x04002221 RID: 8737
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04002222 RID: 8738
	private HandleVector<int>.Handle objectPartitionerEntry;

	// Token: 0x04002223 RID: 8739
	private static readonly Func<int, object, bool> ValidTestDelegate = (int cell, object data) => EntityPreview.ValidTest(cell, data);
}
