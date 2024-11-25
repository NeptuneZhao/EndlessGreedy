using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class StaterpillarCellQuery : PathFinderQuery
{
	// Token: 0x0600198F RID: 6543 RVA: 0x00088B78 File Offset: 0x00086D78
	public StaterpillarCellQuery Reset(int max_results, GameObject tester, ObjectLayer conduitLayer)
	{
		this.max_results = max_results;
		this.tester = tester;
		this.result_cells.Clear();
		ObjectLayer objectLayer;
		if (conduitLayer <= ObjectLayer.LiquidConduit)
		{
			if (conduitLayer == ObjectLayer.GasConduit)
			{
				objectLayer = ObjectLayer.GasConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.LiquidConduit)
			{
				objectLayer = ObjectLayer.LiquidConduitConnection;
				goto IL_4A;
			}
		}
		else
		{
			if (conduitLayer == ObjectLayer.SolidConduit)
			{
				objectLayer = ObjectLayer.SolidConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.Wire)
			{
				objectLayer = ObjectLayer.WireConnectors;
				goto IL_4A;
			}
		}
		objectLayer = conduitLayer;
		IL_4A:
		this.connectorLayer = objectLayer;
		return this;
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x00088BD7 File Offset: 0x00086DD7
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidRoofCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x00088C14 File Offset: 0x00086E14
	private bool CheckValidRoofCell(int testCell)
	{
		if (!this.tester.GetComponent<Navigator>().NavGrid.NavTable.IsValid(testCell, NavType.Ceiling))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Down);
		return !Grid.ObjectLayers[1].ContainsKey(testCell) && !Grid.ObjectLayers[1].ContainsKey(cellInDirection) && !Grid.Objects[cellInDirection, (int)this.connectorLayer] && Grid.IsValidBuildingCell(testCell) && Grid.IsValidCell(cellInDirection) && Grid.IsValidBuildingCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection);
	}

	// Token: 0x04000E8A RID: 3722
	public List<int> result_cells = new List<int>();

	// Token: 0x04000E8B RID: 3723
	private int max_results;

	// Token: 0x04000E8C RID: 3724
	private GameObject tester;

	// Token: 0x04000E8D RID: 3725
	private ObjectLayer connectorLayer;
}
