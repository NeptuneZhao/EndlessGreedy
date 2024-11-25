using System;

// Token: 0x020004A1 RID: 1185
public class SafeCellQuery : PathFinderQuery
{
	// Token: 0x0600198A RID: 6538 RVA: 0x0008890D File Offset: 0x00086B0D
	public SafeCellQuery Reset(MinionBrain brain, bool avoid_light)
	{
		this.brain = brain;
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetCellFlags = (SafeCellQuery.SafeFlags)0;
		this.avoid_light = avoid_light;
		return this;
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x0008893C File Offset: 0x00086B3C
	public static SafeCellQuery.SafeFlags GetFlags(int cell, MinionBrain brain, bool avoid_light = false)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(num))
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.Solid[cell] || Grid.Solid[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		bool flag = brain.IsCellClear(cell);
		bool flag2 = !Grid.Element[cell].IsLiquid;
		bool flag3 = !Grid.Element[num].IsLiquid;
		bool flag4 = Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f;
		bool flag5 = Grid.Radiation[cell] < 250f;
		bool flag6 = brain.OxygenBreather == null || brain.OxygenBreather.IsBreathableElementAtCell(cell, Grid.DefaultOffset);
		bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
		bool flag8 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
		bool flag9 = !avoid_light || SleepChore.IsDarkAtCell(cell);
		if (cell == Grid.PosToCell(brain))
		{
			flag6 = (brain.OxygenBreather == null || !brain.OxygenBreather.IsSuffocating);
		}
		SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags)0;
		if (flag)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsClear;
		}
		if (flag4)
		{
			safeFlags |= SafeCellQuery.SafeFlags.CorrectTemperature;
		}
		if (flag5)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotRadiated;
		}
		if (flag6)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsBreathable;
		}
		if (flag7)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLadder;
		}
		if (flag8)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotTube;
		}
		if (flag2)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquid;
		}
		if (flag3)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
		}
		if (flag9)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsLightOk;
		}
		return safeFlags;
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x00088B0C File Offset: 0x00086D0C
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light);
		bool flag = flags > this.targetCellFlags;
		bool flag2 = flags == this.targetCellFlags && cost < this.targetCost;
		if (flag || flag2)
		{
			this.targetCellFlags = flags;
			this.targetCost = cost;
			this.targetCell = cell;
		}
		return false;
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x00088B65 File Offset: 0x00086D65
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000E85 RID: 3717
	private MinionBrain brain;

	// Token: 0x04000E86 RID: 3718
	private int targetCell;

	// Token: 0x04000E87 RID: 3719
	private int targetCost;

	// Token: 0x04000E88 RID: 3720
	public SafeCellQuery.SafeFlags targetCellFlags;

	// Token: 0x04000E89 RID: 3721
	private bool avoid_light;

	// Token: 0x0200126F RID: 4719
	public enum SafeFlags
	{
		// Token: 0x0400636C RID: 25452
		IsClear = 1,
		// Token: 0x0400636D RID: 25453
		IsLightOk,
		// Token: 0x0400636E RID: 25454
		IsNotLadder = 4,
		// Token: 0x0400636F RID: 25455
		IsNotTube = 8,
		// Token: 0x04006370 RID: 25456
		CorrectTemperature = 16,
		// Token: 0x04006371 RID: 25457
		IsNotRadiated = 32,
		// Token: 0x04006372 RID: 25458
		IsBreathable = 64,
		// Token: 0x04006373 RID: 25459
		IsNotLiquidOnMyFace = 128,
		// Token: 0x04006374 RID: 25460
		IsNotLiquid = 256
	}
}
