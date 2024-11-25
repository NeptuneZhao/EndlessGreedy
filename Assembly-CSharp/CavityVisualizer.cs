using System;
using System.Collections.Generic;
using ProcGenGame;
using UnityEngine;

// Token: 0x02000E01 RID: 3585
[AddComponentMenu("KMonoBehaviour/scripts/CavityVisualizer")]
public class CavityVisualizer : KMonoBehaviour
{
	// Token: 0x060071C0 RID: 29120 RVA: 0x002B1098 File Offset: 0x002AF298
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
		{
			foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
			{
				foreach (int item in hashSet)
				{
					this.cavityCells.Add(item);
				}
			}
		}
	}

	// Token: 0x060071C1 RID: 29121 RVA: 0x002B1170 File Offset: 0x002AF370
	private void OnDrawGizmosSelected()
	{
		if (this.drawCavity)
		{
			Color[] array = new Color[]
			{
				Color.blue,
				Color.yellow
			};
			int num = 0;
			foreach (TerrainCell key in MobSpawning.NaturalCavities.Keys)
			{
				Gizmos.color = array[num % array.Length];
				Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.125f);
				num++;
				foreach (HashSet<int> hashSet in MobSpawning.NaturalCavities[key])
				{
					foreach (int cell in hashSet)
					{
						Gizmos.DrawCube(Grid.CellToPos(cell) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
					}
				}
			}
		}
		if (this.spawnCells != null && this.drawSpawnCells)
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
			foreach (int cell2 in this.spawnCells)
			{
				Gizmos.DrawCube(Grid.CellToPos(cell2) + (Vector3.right / 2f + Vector3.up / 2f), Vector3.one);
			}
		}
	}

	// Token: 0x04004E77 RID: 20087
	public List<int> cavityCells = new List<int>();

	// Token: 0x04004E78 RID: 20088
	public List<int> spawnCells = new List<int>();

	// Token: 0x04004E79 RID: 20089
	public bool drawCavity = true;

	// Token: 0x04004E7A RID: 20090
	public bool drawSpawnCells = true;
}
