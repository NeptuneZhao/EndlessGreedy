using System;
using System.Collections.Generic;

// Token: 0x020000B4 RID: 180
public static class StaterpillarTuning
{
	// Token: 0x04000244 RID: 580
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarGasEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarLiquidEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000245 RID: 581
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_GAS = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarGasEgg".ToTag(),
			weight = 0.66f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarLiquidEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x04000246 RID: 582
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_LIQUID = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarGasEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "StaterpillarLiquidEgg".ToTag(),
			weight = 0.66f
		}
	};

	// Token: 0x04000247 RID: 583
	public static float STANDARD_CALORIES_PER_CYCLE = 2000000f;

	// Token: 0x04000248 RID: 584
	public static float STANDARD_STARVE_CYCLES = 5f;

	// Token: 0x04000249 RID: 585
	public static float STANDARD_STOMACH_SIZE = StaterpillarTuning.STANDARD_CALORIES_PER_CYCLE * StaterpillarTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400024A RID: 586
	public static float POOP_CONVERSTION_RATE = 0.05f;

	// Token: 0x0400024B RID: 587
	public static float EGG_MASS = 2f;
}
