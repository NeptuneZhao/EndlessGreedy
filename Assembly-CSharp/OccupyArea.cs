using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x020009CC RID: 2508
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/OccupyArea")]
public class OccupyArea : KMonoBehaviour
{
	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x060048D8 RID: 18648 RVA: 0x001A05E1 File Offset: 0x0019E7E1
	public CellOffset[] OccupiedCellsOffsets
	{
		get
		{
			this.UpdateRotatedCells();
			return this._RotatedOccupiedCellsOffsets;
		}
	}

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x060048D9 RID: 18649 RVA: 0x001A05EF File Offset: 0x0019E7EF
	// (set) Token: 0x060048DA RID: 18650 RVA: 0x001A05F7 File Offset: 0x0019E7F7
	public bool ApplyToCells
	{
		get
		{
			return this.applyToCells;
		}
		set
		{
			if (value != this.applyToCells)
			{
				if (value)
				{
					this.UpdateOccupiedArea();
				}
				else
				{
					this.ClearOccupiedArea();
				}
				this.applyToCells = value;
			}
		}
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x001A061A File Offset: 0x0019E81A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.applyToCells)
		{
			this.UpdateOccupiedArea();
		}
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x001A0630 File Offset: 0x0019E830
	private void ValidatePosition()
	{
		if (!Grid.IsValidCell(Grid.PosToCell(this)))
		{
			global::Debug.LogWarning(base.name + " is outside the grid! DELETING!");
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x001A065F File Offset: 0x0019E85F
	[OnSerializing]
	private void OnSerializing()
	{
		this.ValidatePosition();
	}

	// Token: 0x060048DE RID: 18654 RVA: 0x001A0667 File Offset: 0x0019E867
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ValidatePosition();
	}

	// Token: 0x060048DF RID: 18655 RVA: 0x001A0670 File Offset: 0x0019E870
	public int GetOffsetCellWithRotation(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x001A06AB File Offset: 0x0019E8AB
	public void SetCellOffsets(CellOffset[] cells)
	{
		this._UnrotatedOccupiedCellsOffsets = cells;
		this._RotatedOccupiedCellsOffsets = cells;
		this.UpdateRotatedCells();
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x001A06C4 File Offset: 0x0019E8C4
	private void UpdateRotatedCells()
	{
		if (this.rotatable != null && this.appliedOrientation != this.rotatable.Orientation)
		{
			this._RotatedOccupiedCellsOffsets = new CellOffset[this._UnrotatedOccupiedCellsOffsets.Length];
			for (int i = 0; i < this._UnrotatedOccupiedCellsOffsets.Length; i++)
			{
				CellOffset offset = this._UnrotatedOccupiedCellsOffsets[i];
				this._RotatedOccupiedCellsOffsets[i] = this.rotatable.GetRotatedCellOffset(offset);
			}
			this.appliedOrientation = this.rotatable.Orientation;
		}
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x001A0750 File Offset: 0x0019E950
	public bool CheckIsOccupying(int checkCell)
	{
		int num = Grid.PosToCell(base.gameObject);
		if (checkCell == num)
		{
			return true;
		}
		foreach (CellOffset offset in this.OccupiedCellsOffsets)
		{
			if (Grid.OffsetCell(num, offset) == checkCell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x001A0799 File Offset: 0x0019E999
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearOccupiedArea();
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x001A07A8 File Offset: 0x0019E9A8
	private void ClearOccupiedArea()
	{
		if (this.occupiedGridCells == null)
		{
			return;
		}
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				foreach (int cell in this.occupiedGridCells)
				{
					if (Grid.Objects[cell, (int)objectLayer] == base.gameObject)
					{
						Grid.Objects[cell, (int)objectLayer] = null;
					}
				}
			}
		}
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x001A0824 File Offset: 0x0019EA24
	public void UpdateOccupiedArea()
	{
		if (this.objectLayers.Length == 0)
		{
			return;
		}
		if (this.occupiedGridCells == null)
		{
			this.occupiedGridCells = new int[this.OccupiedCellsOffsets.Length];
		}
		this.ClearOccupiedArea();
		int cell = Grid.PosToCell(base.gameObject);
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				for (int j = 0; j < this.OccupiedCellsOffsets.Length; j++)
				{
					CellOffset offset = this.OccupiedCellsOffsets[j];
					int num = Grid.OffsetCell(cell, offset);
					Grid.Objects[num, (int)objectLayer] = base.gameObject;
					this.occupiedGridCells[j] = num;
				}
			}
		}
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x001A08D4 File Offset: 0x0019EAD4
	public int GetWidthInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.x);
			num2 = Math.Max(num2, cellOffset.x);
		}
		return num2 - num + 1;
	}

	// Token: 0x060048E7 RID: 18663 RVA: 0x001A092C File Offset: 0x0019EB2C
	public int GetHeightInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.y);
			num2 = Math.Max(num2, cellOffset.y);
		}
		return num2 - num + 1;
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x001A0984 File Offset: 0x0019EB84
	public Extents GetExtents()
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets);
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x001A099C File Offset: 0x0019EB9C
	public Extents GetExtents(Orientation orientation)
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets, orientation);
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x001A09B8 File Offset: 0x0019EBB8
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.PosToCell(base.gameObject);
		if (this.OccupiedCellsOffsets != null)
		{
			foreach (CellOffset offset in this.OccupiedCellsOffsets)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one);
			}
		}
		if (this.AboveOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset2 in this.AboveOccupiedCellOffsets)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset2)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
		if (this.BelowOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset3 in this.BelowOccupiedCellOffsets)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset3)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x001A0B30 File Offset: 0x0019ED30
	public bool CanOccupyArea(int rootCell, ObjectLayer layer)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int cell = Grid.OffsetCell(rootCell, offset);
			if (Grid.Objects[cell, (int)layer] != null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060048EC RID: 18668 RVA: 0x001A0B7C File Offset: 0x0019ED7C
	public bool TestArea(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int arg = Grid.OffsetCell(rootCell, offset);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060048ED RID: 18669 RVA: 0x001A0BC0 File Offset: 0x0019EDC0
	public bool TestAreaAbove(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.AboveOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y + 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.AboveOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.AboveOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.AboveOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060048EE RID: 18670 RVA: 0x001A0C70 File Offset: 0x0019EE70
	public bool TestAreaBelow(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.BelowOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y - 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.BelowOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.BelowOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.BelowOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04002FAD RID: 12205
	private CellOffset[] AboveOccupiedCellOffsets;

	// Token: 0x04002FAE RID: 12206
	private CellOffset[] BelowOccupiedCellOffsets;

	// Token: 0x04002FAF RID: 12207
	private int[] occupiedGridCells;

	// Token: 0x04002FB0 RID: 12208
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002FB1 RID: 12209
	private Orientation appliedOrientation;

	// Token: 0x04002FB2 RID: 12210
	public CellOffset[] _UnrotatedOccupiedCellsOffsets;

	// Token: 0x04002FB3 RID: 12211
	public CellOffset[] _RotatedOccupiedCellsOffsets;

	// Token: 0x04002FB4 RID: 12212
	public ObjectLayer[] objectLayers = new ObjectLayer[0];

	// Token: 0x04002FB5 RID: 12213
	[SerializeField]
	private bool applyToCells = true;
}
