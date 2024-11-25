using System;

// Token: 0x02000093 RID: 147
public static class BeeHiveTuning
{
	// Token: 0x040001A3 RID: 419
	public static float ORE_DELIVERY_AMOUNT = 1f;

	// Token: 0x040001A4 RID: 420
	public static float KG_ORE_EATEN_PER_CYCLE = BeeHiveTuning.ORE_DELIVERY_AMOUNT * 10f;

	// Token: 0x040001A5 RID: 421
	public static float STANDARD_CALORIES_PER_CYCLE = 1500000f;

	// Token: 0x040001A6 RID: 422
	public static float STANDARD_STARVE_CYCLES = 30f;

	// Token: 0x040001A7 RID: 423
	public static float STANDARD_STOMACH_SIZE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE * BeeHiveTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x040001A8 RID: 424
	public static float CALORIES_PER_KG_OF_ORE = BeeHiveTuning.STANDARD_CALORIES_PER_CYCLE / BeeHiveTuning.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040001A9 RID: 425
	public static float POOP_CONVERSTION_RATE = 0.9f;

	// Token: 0x040001AA RID: 426
	public static Tag CONSUMED_ORE = SimHashes.UraniumOre.CreateTag();

	// Token: 0x040001AB RID: 427
	public static Tag PRODUCED_ORE = SimHashes.EnrichedUranium.CreateTag();

	// Token: 0x040001AC RID: 428
	public static float HIVE_GROWTH_TIME = 2f;

	// Token: 0x040001AD RID: 429
	public static float WASTE_DROPPED_ON_DEATH = 5f;

	// Token: 0x040001AE RID: 430
	public static int GERMS_DROPPED_ON_DEATH = 10000;
}
