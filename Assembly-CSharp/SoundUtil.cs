using System;
using UnityEngine;

// Token: 0x02000AAE RID: 2734
public static class SoundUtil
{
	// Token: 0x060050A1 RID: 20641 RVA: 0x001CFA40 File Offset: 0x001CDC40
	public static float GetLiquidDepth(int cell)
	{
		float num = 0f;
		num += Grid.Mass[cell] * (Grid.Element[cell].IsLiquid ? 1f : 0f);
		int num2 = Grid.CellBelow(cell);
		if (Grid.IsValidCell(num2))
		{
			num += Grid.Mass[num2] * (Grid.Element[num2].IsLiquid ? 1f : 0f);
		}
		return Mathf.Min(num / 1000f, 1f);
	}

	// Token: 0x060050A2 RID: 20642 RVA: 0x001CFAC5 File Offset: 0x001CDCC5
	public static float GetLiquidVolume(float mass)
	{
		return Mathf.Min(mass / 100f, 1f);
	}
}
