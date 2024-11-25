using System;
using UnityEngine;

// Token: 0x02000498 RID: 1176
public class DrawNavGridQuery : PathFinderQuery
{
	// Token: 0x06001969 RID: 6505 RVA: 0x00087ECE File Offset: 0x000860CE
	public DrawNavGridQuery Reset(MinionBrain brain)
	{
		return this;
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x00087ED4 File Offset: 0x000860D4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (parent_cell == Grid.InvalidCell || (int)Grid.WorldIdx[parent_cell] != ClusterManager.Instance.activeWorldId || (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			return false;
		}
		GL.Color(Color.white);
		GL.Vertex(Grid.CellToPosCCC(parent_cell, Grid.SceneLayer.Move));
		GL.Vertex(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
		return false;
	}
}
