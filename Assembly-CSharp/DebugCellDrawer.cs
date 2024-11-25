using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000831 RID: 2097
[AddComponentMenu("KMonoBehaviour/scripts/DebugCellDrawer")]
public class DebugCellDrawer : KMonoBehaviour
{
	// Token: 0x06003A45 RID: 14917 RVA: 0x0013E48C File Offset: 0x0013C68C
	private void Update()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			if (this.cells[i] != PathFinder.InvalidCell)
			{
				DebugExtension.DebugPoint(Grid.CellToPosCCF(this.cells[i], Grid.SceneLayer.Background), 1f, 0f, true);
			}
		}
	}

	// Token: 0x04002306 RID: 8966
	public List<int> cells;
}
