using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A1 RID: 161
public static class LightBugTuning
{
	// Token: 0x040001EE RID: 494
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001EF RID: 495
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_ORANGE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F0 RID: 496
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PURPLE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugOrangeEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F1 RID: 497
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_PINK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPurpleEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F2 RID: 498
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLUE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugPinkEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F3 RID: 499
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BLACK = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlueEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugBlackEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F4 RID: 500
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_CRYSTAL = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugCrystalEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "LightBugEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001F5 RID: 501
	public static float STANDARD_CALORIES_PER_CYCLE = 40000f;

	// Token: 0x040001F6 RID: 502
	public static float STANDARD_STARVE_CYCLES = 8f;

	// Token: 0x040001F7 RID: 503
	public static float STANDARD_STOMACH_SIZE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE * LightBugTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001F8 RID: 504
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001F9 RID: 505
	public static float EGG_MASS = 0.2f;
}
