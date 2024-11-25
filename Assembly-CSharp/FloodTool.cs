using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090F RID: 2319
public class FloodTool : InterfaceTool
{
	// Token: 0x0600430C RID: 17164 RVA: 0x0017D3D0 File Offset: 0x0017B5D0
	public HashSet<int> Flood(int startCell)
	{
		HashSet<int> visited_cells = new HashSet<int>();
		HashSet<int> hashSet = new HashSet<int>();
		GameUtil.FloodFillConditional(startCell, this.floodCriteria, visited_cells, hashSet);
		return hashSet;
	}

	// Token: 0x0600430D RID: 17165 RVA: 0x0017D3F8 File Offset: 0x0017B5F8
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.paintArea(this.Flood(Grid.PosToCell(cursor_pos)));
	}

	// Token: 0x0600430E RID: 17166 RVA: 0x0017D418 File Offset: 0x0017B618
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.mouseCell = Grid.PosToCell(cursor_pos);
	}

	// Token: 0x04002C27 RID: 11303
	public Func<int, bool> floodCriteria;

	// Token: 0x04002C28 RID: 11304
	public Action<HashSet<int>> paintArea;

	// Token: 0x04002C29 RID: 11305
	protected Color32 areaColour = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04002C2A RID: 11306
	protected int mouseCell = -1;
}
