using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x0200009B RID: 155
public static class DivergentTuning
{
	// Token: 0x040001D2 RID: 466
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BEETLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001D3 RID: 467
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WORM = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentBeetleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "DivergentWormEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040001D4 RID: 468
	public static int TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION = 2;

	// Token: 0x040001D5 RID: 469
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x040001D6 RID: 470
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001D7 RID: 471
	public static float STANDARD_STOMACH_SIZE = DivergentTuning.STANDARD_CALORIES_PER_CYCLE * DivergentTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001D8 RID: 472
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001D9 RID: 473
	public static int PEN_SIZE_PER_CREATURE_WORM = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x040001DA RID: 474
	public static float EGG_MASS = 2f;
}
