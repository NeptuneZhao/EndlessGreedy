using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000AB RID: 171
public static class PacuTuning
{
	// Token: 0x04000222 RID: 546
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000223 RID: 547
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_TROPICAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000224 RID: 548
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_CLEANER = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuCleanerEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "PacuTropicalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000225 RID: 549
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x04000226 RID: 550
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000227 RID: 551
	public static float STANDARD_STOMACH_SIZE = PacuTuning.STANDARD_CALORIES_PER_CYCLE * PacuTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000228 RID: 552
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER2;

	// Token: 0x04000229 RID: 553
	public static float EGG_MASS = 4f;
}
