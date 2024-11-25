using System;
using TUNING;
using UnityEngine;

// Token: 0x020000A6 RID: 166
public static class MooTuning
{
	// Token: 0x04000206 RID: 518
	public static readonly float STANDARD_LIFESPAN = 75f;

	// Token: 0x04000207 RID: 519
	public static readonly float STANDARD_CALORIES_PER_CYCLE = 200000f;

	// Token: 0x04000208 RID: 520
	public static readonly float STANDARD_STARVE_CYCLES = 6f;

	// Token: 0x04000209 RID: 521
	public static readonly float STANDARD_STOMACH_SIZE = MooTuning.STANDARD_CALORIES_PER_CYCLE * MooTuning.STANDARD_STARVE_CYCLES;

	// Token: 0x0400020A RID: 522
	public static readonly int PEN_SIZE_PER_CREATURE = CREATURES.SPACE_REQUIREMENTS.TIER4;

	// Token: 0x0400020B RID: 523
	private static readonly float BECKONS_PER_LIFESPAN = 4f;

	// Token: 0x0400020C RID: 524
	private static readonly float BECKON_FUDGE_CYCLES = 11f;

	// Token: 0x0400020D RID: 525
	private static readonly float BECKON_CYCLES = Mathf.Floor((MooTuning.STANDARD_LIFESPAN - MooTuning.BECKON_FUDGE_CYCLES) / MooTuning.BECKONS_PER_LIFESPAN);

	// Token: 0x0400020E RID: 526
	public static readonly float WELLFED_EFFECT = 100f / (600f * MooTuning.BECKON_CYCLES);

	// Token: 0x0400020F RID: 527
	public static readonly float WELLFED_CALORIES_PER_CYCLE = MooTuning.STANDARD_CALORIES_PER_CYCLE * 0.9f;

	// Token: 0x04000210 RID: 528
	public static readonly float ELIGIBLE_MILKING_PERCENTAGE = 1f;

	// Token: 0x04000211 RID: 529
	public static readonly float MILK_PER_CYCLE = 50f;

	// Token: 0x04000212 RID: 530
	private static readonly float CYCLES_UNTIL_MILKING = 4f;

	// Token: 0x04000213 RID: 531
	public static readonly float MILK_CAPACITY = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x04000214 RID: 532
	public static readonly float MILK_AMOUNT_AT_MILKING = MooTuning.MILK_PER_CYCLE * MooTuning.CYCLES_UNTIL_MILKING;

	// Token: 0x04000215 RID: 533
	public static readonly float MILK_PRODUCTION_PERCENTAGE_PER_SECOND = 100f / (600f * MooTuning.CYCLES_UNTIL_MILKING);
}
