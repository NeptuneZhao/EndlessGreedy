using System;

// Token: 0x02000489 RID: 1161
public class NavTableValidator
{
	// Token: 0x0600191B RID: 6427 RVA: 0x00086A74 File Offset: 0x00084C74
	protected bool IsClear(int cell, CellOffset[] bounding_offsets, bool is_dupe)
	{
		foreach (CellOffset offset in bounding_offsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			if (!Grid.IsWorldValidCell(cell2) || !NavTableValidator.IsCellPassable(cell2, is_dupe))
			{
				return false;
			}
			int num = Grid.CellAbove(cell2);
			if (Grid.IsValidCell(num) && Grid.Element[num].IsUnstable)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x00086AD4 File Offset: 0x00084CD4
	protected static bool IsCellPassable(int cell, bool is_dupe)
	{
		Grid.BuildFlags buildFlags = Grid.BuildMasks[cell] & ~(Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.FakeFloor);
		if (buildFlags == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
		{
			return true;
		}
		if (is_dupe)
		{
			return (buildFlags & Grid.BuildFlags.DupeImpassable) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) && ((buildFlags & Grid.BuildFlags.Solid) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || (buildFlags & Grid.BuildFlags.DupePassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor));
		}
		return (buildFlags & (Grid.BuildFlags.Solid | Grid.BuildFlags.CritterImpassable)) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x00086B11 File Offset: 0x00084D11
	public virtual void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
	{
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x00086B13 File Offset: 0x00084D13
	public virtual void Clear()
	{
	}

	// Token: 0x04000E13 RID: 3603
	public Action<int> onDirty;
}
