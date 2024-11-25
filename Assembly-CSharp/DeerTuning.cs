using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x02000099 RID: 153
public static class DeerTuning
{
	// Token: 0x040001CA RID: 458
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "WoodDeerEgg".ToTag(),
			weight = 1f
		}
	};

	// Token: 0x040001CB RID: 459
	public const float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040001CC RID: 460
	public const float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001CD RID: 461
	public const float STANDARD_STOMACH_SIZE = 1000000f;

	// Token: 0x040001CE RID: 462
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001CF RID: 463
	public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x040001D0 RID: 464
	public static float EGG_MASS = 2f;

	// Token: 0x040001D1 RID: 465
	public static float DROP_ANTLER_DURATION = 1200f;
}
