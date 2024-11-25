using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x0200009F RID: 159
public static class HatchTuning
{
	// Token: 0x040001E5 RID: 485
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001E6 RID: 486
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HARD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001E7 RID: 487
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_VEGGIE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchVeggieEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040001E8 RID: 488
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_METAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchEgg".ToTag(),
			weight = 0.11f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchHardEgg".ToTag(),
			weight = 0.22f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "HatchMetalEgg".ToTag(),
			weight = 0.67f
		}
	};

	// Token: 0x040001E9 RID: 489
	public static float STANDARD_CALORIES_PER_CYCLE = 700000f;

	// Token: 0x040001EA RID: 490
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001EB RID: 491
	public static float STANDARD_STOMACH_SIZE = HatchTuning.STANDARD_CALORIES_PER_CYCLE * HatchTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001EC RID: 492
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001ED RID: 493
	public static float EGG_MASS = 2f;
}
