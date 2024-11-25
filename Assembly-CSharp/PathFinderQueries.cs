using System;

// Token: 0x0200048C RID: 1164
public static class PathFinderQueries
{
	// Token: 0x06001932 RID: 6450 RVA: 0x00087408 File Offset: 0x00085608
	public static void Reset()
	{
		PathFinderQueries.cellQuery = new CellQuery();
		PathFinderQueries.cellCostQuery = new CellCostQuery();
		PathFinderQueries.cellArrayQuery = new CellArrayQuery();
		PathFinderQueries.cellOffsetQuery = new CellOffsetQuery();
		PathFinderQueries.safeCellQuery = new SafeCellQuery();
		PathFinderQueries.idleCellQuery = new IdleCellQuery();
		PathFinderQueries.breathableCellQuery = new BreathableCellQuery();
		PathFinderQueries.drawNavGridQuery = new DrawNavGridQuery();
		PathFinderQueries.plantableCellQuery = new PlantableCellQuery();
		PathFinderQueries.mineableCellQuery = new MineableCellQuery();
		PathFinderQueries.staterpillarCellQuery = new StaterpillarCellQuery();
		PathFinderQueries.floorCellQuery = new FloorCellQuery();
		PathFinderQueries.buildingPlacementQuery = new BuildingPlacementQuery();
	}

	// Token: 0x04000E19 RID: 3609
	public static CellQuery cellQuery = new CellQuery();

	// Token: 0x04000E1A RID: 3610
	public static CellCostQuery cellCostQuery = new CellCostQuery();

	// Token: 0x04000E1B RID: 3611
	public static CellArrayQuery cellArrayQuery = new CellArrayQuery();

	// Token: 0x04000E1C RID: 3612
	public static CellOffsetQuery cellOffsetQuery = new CellOffsetQuery();

	// Token: 0x04000E1D RID: 3613
	public static SafeCellQuery safeCellQuery = new SafeCellQuery();

	// Token: 0x04000E1E RID: 3614
	public static IdleCellQuery idleCellQuery = new IdleCellQuery();

	// Token: 0x04000E1F RID: 3615
	public static BreathableCellQuery breathableCellQuery = new BreathableCellQuery();

	// Token: 0x04000E20 RID: 3616
	public static DrawNavGridQuery drawNavGridQuery = new DrawNavGridQuery();

	// Token: 0x04000E21 RID: 3617
	public static PlantableCellQuery plantableCellQuery = new PlantableCellQuery();

	// Token: 0x04000E22 RID: 3618
	public static MineableCellQuery mineableCellQuery = new MineableCellQuery();

	// Token: 0x04000E23 RID: 3619
	public static StaterpillarCellQuery staterpillarCellQuery = new StaterpillarCellQuery();

	// Token: 0x04000E24 RID: 3620
	public static FloorCellQuery floorCellQuery = new FloorCellQuery();

	// Token: 0x04000E25 RID: 3621
	public static BuildingPlacementQuery buildingPlacementQuery = new BuildingPlacementQuery();
}
