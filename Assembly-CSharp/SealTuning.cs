using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000B0 RID: 176
public static class SealTuning
{
	// Token: 0x04000236 RID: 566
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SealEgg".ToTag(),
			weight = 1f
		}
	};

	// Token: 0x04000237 RID: 567
	public const float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000238 RID: 568
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000239 RID: 569
	public const float STANDARD_STOMACH_SIZE = 1000000f;

	// Token: 0x0400023A RID: 570
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400023B RID: 571
	public static float EGG_MASS = 2f;
}
