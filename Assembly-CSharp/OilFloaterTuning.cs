using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A8 RID: 168
public static class OilFloaterTuning
{
	// Token: 0x04000216 RID: 534
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000217 RID: 535
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_HIGHTEMP = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000218 RID: 536
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DECOR = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterEgg".ToTag(),
			weight = 0.33f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterHighTempEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "OilfloaterDecorEgg".ToTag(),
			weight = 0.66f
		}
	};

	// Token: 0x04000219 RID: 537
	public static float STANDARD_CALORIES_PER_CYCLE = 120000f;

	// Token: 0x0400021A RID: 538
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x0400021B RID: 539
	public static float STANDARD_STOMACH_SIZE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE * OilFloaterTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400021C RID: 540
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x0400021D RID: 541
	public static float EGG_MASS = 2f;
}
