using System;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
public class VerticalModuleTiler : KMonoBehaviour
{
	// Token: 0x06001539 RID: 5433 RVA: 0x00074820 File Offset: 0x00072A20
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (this.manageTopCap)
		{
			this.topCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.topCapStr);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.bottomCapStr);
		}
		this.PostReorderMove();
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x00074894 File Offset: 0x00072A94
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x000748AC File Offset: 0x00072AAC
	public void PostReorderMove()
	{
		this.dirty = true;
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x000748B5 File Offset: 0x00072AB5
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

	// Token: 0x0600153D RID: 5437 RVA: 0x000748E4 File Offset: 0x00072AE4
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellTop = this.GetCellTop();
		int cellBottom = this.GetCellBottom();
		if (Grid.IsValidCell(cellTop))
		{
			if (this.HasWideNeighbor(cellTop))
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (Grid.IsValidCell(cellBottom))
		{
			if (this.HasWideNeighbor(cellBottom))
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (this.manageTopCap)
		{
			this.topCapWide.Enable(this.topCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide.Enable(this.bottomCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x00074988 File Offset: 0x00072B88
	private int GetCellTop()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x000749CC File Offset: 0x00072BCC
	private int GetCellBottom()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 - 1);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x00074A08 File Offset: 0x00072C08
	private bool HasWideNeighbor(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.GetComponent<ReorderableBuilding>() != null && component.GetComponent<Building>().Def.WidthInCells >= 5)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x00074A68 File Offset: 0x00072C68
	private void LateUpdate()
	{
		if (this.animController.Offset != this.m_previousAnimControllerOffset)
		{
			this.m_previousAnimControllerOffset = this.animController.Offset;
			this.bottomCapWide.Dirty();
			this.topCapWide.Dirty();
		}
		if (this.dirty)
		{
			if (this.partitionerEntry != HandleVector<int>.InvalidHandle)
			{
				GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			}
			OccupyArea component = base.GetComponent<OccupyArea>();
			if (component != null)
			{
				this.extents = component.GetExtents();
			}
			Extents extents = new Extents(this.extents.x, this.extents.y - 1, this.extents.width, this.extents.height + 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("VerticalModuleTiler.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
			this.UpdateEndCaps();
			this.dirty = false;
		}
	}

	// Token: 0x04000C01 RID: 3073
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04000C02 RID: 3074
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04000C03 RID: 3075
	private Extents extents;

	// Token: 0x04000C04 RID: 3076
	private VerticalModuleTiler.AnimCapType topCapSetting;

	// Token: 0x04000C05 RID: 3077
	private VerticalModuleTiler.AnimCapType bottomCapSetting;

	// Token: 0x04000C06 RID: 3078
	private bool manageTopCap = true;

	// Token: 0x04000C07 RID: 3079
	private bool manageBottomCap = true;

	// Token: 0x04000C08 RID: 3080
	private KAnimSynchronizedController topCapWide;

	// Token: 0x04000C09 RID: 3081
	private KAnimSynchronizedController bottomCapWide;

	// Token: 0x04000C0A RID: 3082
	private static readonly string topCapStr = "#cap_top_5";

	// Token: 0x04000C0B RID: 3083
	private static readonly string bottomCapStr = "#cap_bottom_5";

	// Token: 0x04000C0C RID: 3084
	private bool dirty;

	// Token: 0x04000C0D RID: 3085
	[MyCmpGet]
	private KAnimControllerBase animController;

	// Token: 0x04000C0E RID: 3086
	private Vector3 m_previousAnimControllerOffset;

	// Token: 0x02001161 RID: 4449
	private enum AnimCapType
	{
		// Token: 0x04005FF8 RID: 24568
		ThreeWide,
		// Token: 0x04005FF9 RID: 24569
		FiveWide
	}
}
