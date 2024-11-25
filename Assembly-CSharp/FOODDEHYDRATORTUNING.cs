using System;

// Token: 0x020001D3 RID: 467
public class FOODDEHYDRATORTUNING
{
	// Token: 0x0400064B RID: 1611
	public const float INTERNAL_WORK_TIME = 250f;

	// Token: 0x0400064C RID: 1612
	public const float DUPE_WORK_TIME = 50f;

	// Token: 0x0400064D RID: 1613
	public const float GAS_CONSUMPTION_PER_SECOND = 0.020000001f;

	// Token: 0x0400064E RID: 1614
	public const float REQUIRED_FUEL_AMOUNT = 5.0000005f;

	// Token: 0x0400064F RID: 1615
	public const float CO2_EMIT_RATE = 0.0050000004f;

	// Token: 0x04000650 RID: 1616
	public const float CO2_EMIT_TEMPERATURE = 348.15f;

	// Token: 0x04000651 RID: 1617
	public const float PLASTIC_KG = 12f;

	// Token: 0x04000652 RID: 1618
	public const float WATER_OUTPUT_KG = 6f;

	// Token: 0x04000653 RID: 1619
	public const float FOOD_PACKETS = 6f;

	// Token: 0x04000654 RID: 1620
	public const float KCAL_PER_PACKET = 1000f;

	// Token: 0x04000655 RID: 1621
	public const float FOOD_KCAL = 6000000f;

	// Token: 0x04000656 RID: 1622
	public static Tag FUEL_TAG = SimHashes.Methane.CreateTag();
}
