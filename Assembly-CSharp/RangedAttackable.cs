using System;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public class RangedAttackable : AttackableBase
{
	// Token: 0x0600226B RID: 8811 RVA: 0x000BFB24 File Offset: 0x000BDD24
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600226C RID: 8812 RVA: 0x000BFB2C File Offset: 0x000BDD2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.preferUnreservedCell = true;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x000BFB46 File Offset: 0x000BDD46
	public new int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000BFB50 File Offset: 0x000BDD50
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 0.5f, 0.5f, 0.15f);
		foreach (CellOffset offset in base.GetOffsets())
		{
			Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0f) + Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset)), Vector3.one);
		}
	}
}
