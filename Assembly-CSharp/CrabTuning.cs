using System;
using System.Collections.Generic;
using TUNING;

// Token: 0x02000097 RID: 151
public static class CrabTuning
{
	// Token: 0x060002DE RID: 734 RVA: 0x0001534F File Offset: 0x0001354F
	public static bool IsReadyToMolt(MoltDropperMonitor.Instance smi)
	{
		return CrabTuning.IsValidTimeToDrop(smi) && CrabTuning.IsValidDropCell(smi) && !smi.prefabID.HasTag(GameTags.Creatures.Hungry) && smi.prefabID.HasTag(GameTags.Creatures.Happy);
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00015385 File Offset: 0x00013585
	public static bool IsValidTimeToDrop(MoltDropperMonitor.Instance smi)
	{
		return !smi.spawnedThisCycle && (smi.timeOfLastDrop <= 0f || GameClock.Instance.GetTime() - smi.timeOfLastDrop > 600f);
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x000153B8 File Offset: 0x000135B8
	public static bool IsValidDropCell(MoltDropperMonitor.Instance smi)
	{
		int num = Grid.PosToCell(smi.transform.GetPosition());
		return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Ethanol;
	}

	// Token: 0x040001C1 RID: 449
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_BASE = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.97f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.01f
		}
	};

	// Token: 0x040001C2 RID: 450
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_WOOD = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.65f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.02f
		}
	};

	// Token: 0x040001C3 RID: 451
	public static List<FertilityMonitor.BreedingChance> EGG_CHANCES_FRESH = new List<FertilityMonitor.BreedingChance>
	{
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabEgg".ToTag(),
			weight = 0.32f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabWoodEgg".ToTag(),
			weight = 0.02f
		},
		new FertilityMonitor.BreedingChance
		{
			egg = "CrabFreshWaterEgg".ToTag(),
			weight = 0.65f
		}
	};

	// Token: 0x040001C4 RID: 452
	public static float STANDARD_CALORIES_PER_CYCLE = 100000f;

	// Token: 0x040001C5 RID: 453
	public static float STANDARD_STARVE_CYCLES = 10f;

	// Token: 0x040001C6 RID: 454
	public static float STANDARD_STOMACH_SIZE = CrabTuning.STANDARD_CALORIES_PER_CYCLE * CrabTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001C7 RID: 455
	public static int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER3;

	// Token: 0x040001C8 RID: 456
	public static float EGG_MASS = 2f;

	// Token: 0x040001C9 RID: 457
	public static CellOffset[] DEFEND_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(-1, 1)
	};
}
