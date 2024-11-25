using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000B2 RID: 178
public static class SquirrelTuning
{
	// Token: 0x0400023C RID: 572
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x0400023D RID: 573
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HUG = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelEgg".ToTag(),
			weight = 0.35f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "SquirrelHugEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x0400023E RID: 574
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x0400023F RID: 575
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x04000240 RID: 576
	public static float STANDARD_STOMACH_SIZE = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE * SquirrelTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000241 RID: 577
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x04000242 RID: 578
	public static int PEN_SIZE_PER_CREATURE_HUG = CREATURES.SPACE_REQUIREMENTS.TIER1;

	// Token: 0x04000243 RID: 579
	public static float EGG_MASS = 2f;
}
