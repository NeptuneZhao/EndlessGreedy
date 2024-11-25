using System;
using UnityEngine;

// Token: 0x0200058F RID: 1423
[AddComponentMenu("KMonoBehaviour/scripts/AntiCluster")]
public class MoverLayerOccupier : KMonoBehaviour, ISim200ms
{
	// Token: 0x0600214B RID: 8523 RVA: 0x000BA8FC File Offset: 0x000B8AFC
	private void RefreshCellOccupy()
	{
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in this.cellOffsets)
		{
			int current_cell = Grid.OffsetCell(cell, offset);
			if (this.previousCell != Grid.InvalidCell)
			{
				int previous_cell = Grid.OffsetCell(this.previousCell, offset);
				this.UpdateCell(previous_cell, current_cell);
			}
			else
			{
				this.UpdateCell(this.previousCell, current_cell);
			}
		}
		this.previousCell = cell;
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x000BA972 File Offset: 0x000B8B72
	public void Sim200ms(float dt)
	{
		this.RefreshCellOccupy();
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x000BA97C File Offset: 0x000B8B7C
	private void UpdateCell(int previous_cell, int current_cell)
	{
		foreach (ObjectLayer layer in this.objectLayers)
		{
			if (previous_cell != Grid.InvalidCell && previous_cell != current_cell && Grid.Objects[previous_cell, (int)layer] == base.gameObject)
			{
				Grid.Objects[previous_cell, (int)layer] = null;
			}
			GameObject gameObject = Grid.Objects[current_cell, (int)layer];
			if (gameObject == null)
			{
				Grid.Objects[current_cell, (int)layer] = base.gameObject;
			}
			else
			{
				KPrefabID component = base.GetComponent<KPrefabID>();
				KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
				if (component.InstanceID > component2.InstanceID)
				{
					Grid.Objects[current_cell, (int)layer] = base.gameObject;
				}
			}
		}
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x000BAA34 File Offset: 0x000B8C34
	private void CleanUpOccupiedCells()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		foreach (CellOffset offset in this.cellOffsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			foreach (ObjectLayer layer in this.objectLayers)
			{
				if (Grid.Objects[cell2, (int)layer] == base.gameObject)
				{
					Grid.Objects[cell2, (int)layer] = null;
				}
			}
		}
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x000BAAC4 File Offset: 0x000B8CC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.RefreshCellOccupy();
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x000BAAD2 File Offset: 0x000B8CD2
	protected override void OnCleanUp()
	{
		this.CleanUpOccupiedCells();
		base.OnCleanUp();
	}

	// Token: 0x040012AB RID: 4779
	private int previousCell = Grid.InvalidCell;

	// Token: 0x040012AC RID: 4780
	public ObjectLayer[] objectLayers;

	// Token: 0x040012AD RID: 4781
	public CellOffset[] cellOffsets;
}
