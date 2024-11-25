using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x020000A4 RID: 164
public static class MoleTuning
{
	// Token: 0x040001FB RID: 507
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.98f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001FC RID: 508
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_DELICACY = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "MoleDelicacyEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040001FD RID: 509
	public static float STANDARD_CALORIES_PER_CYCLE = 4800000f;

	// Token: 0x040001FE RID: 510
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001FF RID: 511
	public static float STANDARD_STOMACH_SIZE = MoleTuning.STANDARD_CALORIES_PER_CYCLE * MoleTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x04000200 RID: 512
	public static float DELICACY_STOMACH_SIZE = MoleTuning.STANDARD_STOMACH_SIZE / 2f;

	// Token: 0x04000201 RID: 513
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER2;

	// Token: 0x04000202 RID: 514
	public static float EGG_MASS = 2f;

	// Token: 0x04000203 RID: 515
	public static int DEPTH_TO_HIDE = 2;

	// Token: 0x04000204 RID: 516
	public static HashedString[] GINGER_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger",
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};
}
