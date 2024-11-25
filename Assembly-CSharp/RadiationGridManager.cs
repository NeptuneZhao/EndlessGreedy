using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A40 RID: 2624
public static class RadiationGridManager
{
	// Token: 0x06004C02 RID: 19458 RVA: 0x001B1E7C File Offset: 0x001B007C
	public static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06004C03 RID: 19459 RVA: 0x001B1E99 File Offset: 0x001B0099
	public static void Initialise()
	{
		RadiationGridManager.emitters = new List<RadiationGridEmitter>();
	}

	// Token: 0x06004C04 RID: 19460 RVA: 0x001B1EA5 File Offset: 0x001B00A5
	public static void Shutdown()
	{
		RadiationGridManager.emitters.Clear();
	}

	// Token: 0x06004C05 RID: 19461 RVA: 0x001B1EB4 File Offset: 0x001B00B4
	public static void Refresh()
	{
		for (int i = 0; i < RadiationGridManager.emitters.Count; i++)
		{
			if (RadiationGridManager.emitters[i].enabled)
			{
				RadiationGridManager.emitters[i].Emit();
			}
		}
	}

	// Token: 0x04003277 RID: 12919
	public const float STANDARD_MASS_FALLOFF = 1000000f;

	// Token: 0x04003278 RID: 12920
	public const int RADIATION_LINGER_RATE = 4;

	// Token: 0x04003279 RID: 12921
	public static List<RadiationGridEmitter> emitters = new List<RadiationGridEmitter>();

	// Token: 0x0400327A RID: 12922
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x0400327B RID: 12923
	public static int[] previewLux;
}
