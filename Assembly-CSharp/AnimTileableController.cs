using System;
using UnityEngine;

// Token: 0x0200064A RID: 1610
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/AnimTileableController")]
public class AnimTileableController : KMonoBehaviour
{
	// Token: 0x0600275D RID: 10077 RVA: 0x000E0141 File Offset: 0x000DE341
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				base.GetComponent<KPrefabID>().PrefabTag
			};
		}
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x000E0178 File Offset: 0x000DE378
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		else
		{
			Building component2 = base.GetComponent<Building>();
			this.extents = component2.GetExtents();
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		KBatchedAnimController component3 = base.GetComponent<KBatchedAnimController>();
		this.left = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.leftName);
		this.right = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.rightName);
		this.top = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.topName);
		this.bottom = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.bottomName);
		this.UpdateEndCaps();
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x000E0297 File Offset: 0x000DE497
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x000E02B0 File Offset: 0x000DE4B0
	private void UpdateEndCaps()
	{
		int cell = Grid.PosToCell(this);
		bool enable = true;
		bool enable2 = true;
		bool enable3 = true;
		bool enable4 = true;
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset rotatedCellOffset = new CellOffset(this.extents.x - num - 1, 0);
		CellOffset rotatedCellOffset2 = new CellOffset(this.extents.x - num + this.extents.width, 0);
		CellOffset rotatedCellOffset3 = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		CellOffset rotatedCellOffset4 = new CellOffset(0, this.extents.y - num2 - 1);
		Rotatable component = base.GetComponent<Rotatable>();
		if (component)
		{
			rotatedCellOffset = component.GetRotatedCellOffset(rotatedCellOffset);
			rotatedCellOffset2 = component.GetRotatedCellOffset(rotatedCellOffset2);
			rotatedCellOffset3 = component.GetRotatedCellOffset(rotatedCellOffset3);
			rotatedCellOffset4 = component.GetRotatedCellOffset(rotatedCellOffset4);
		}
		int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num4 = Grid.OffsetCell(cell, rotatedCellOffset2);
		int num5 = Grid.OffsetCell(cell, rotatedCellOffset3);
		int num6 = Grid.OffsetCell(cell, rotatedCellOffset4);
		if (Grid.IsValidCell(num3))
		{
			enable = !this.HasTileableNeighbour(num3);
		}
		if (Grid.IsValidCell(num4))
		{
			enable2 = !this.HasTileableNeighbour(num4);
		}
		if (Grid.IsValidCell(num5))
		{
			enable3 = !this.HasTileableNeighbour(num5);
		}
		if (Grid.IsValidCell(num6))
		{
			enable4 = !this.HasTileableNeighbour(num6);
		}
		this.left.Enable(enable);
		this.right.Enable(enable2);
		this.top.Enable(enable3);
		this.bottom.Enable(enable4);
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x000E0434 File Offset: 0x000DE634
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x000E047F File Offset: 0x000DE67F
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x040016AA RID: 5802
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040016AB RID: 5803
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x040016AC RID: 5804
	public Tag[] tags;

	// Token: 0x040016AD RID: 5805
	private Extents extents;

	// Token: 0x040016AE RID: 5806
	public string leftName = "#cap_left";

	// Token: 0x040016AF RID: 5807
	public string rightName = "#cap_right";

	// Token: 0x040016B0 RID: 5808
	public string topName = "#cap_top";

	// Token: 0x040016B1 RID: 5809
	public string bottomName = "#cap_bottom";

	// Token: 0x040016B2 RID: 5810
	private KAnimSynchronizedController left;

	// Token: 0x040016B3 RID: 5811
	private KAnimSynchronizedController right;

	// Token: 0x040016B4 RID: 5812
	private KAnimSynchronizedController top;

	// Token: 0x040016B5 RID: 5813
	private KAnimSynchronizedController bottom;
}
