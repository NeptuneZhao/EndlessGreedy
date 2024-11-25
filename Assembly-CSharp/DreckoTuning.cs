using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x0200009D RID: 157
public static class DreckoTuning
{
	// Token: 0x040001DE RID: 478
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001DF RID: 479
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PLASTIC = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DreckoPlasticEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040001E0 RID: 480
	public static float STANDARD_CALORIES_PER_CYCLE = 2000000f;

	// Token: 0x040001E1 RID: 481
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x040001E2 RID: 482
	public static float STANDARD_STOMACH_SIZE = DreckoTuning.STANDARD_CALORIES_PER_CYCLE * DreckoTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001E3 RID: 483
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001E4 RID: 484
	public static float EGG_MASS = 2f;
}
