using System;

// Token: 0x0200080C RID: 2060
public interface IManageGrowingStates
{
	// Token: 0x060038E7 RID: 14567
	float TimeUntilNextHarvest();

	// Token: 0x060038E8 RID: 14568
	float PercentGrown();

	// Token: 0x060038E9 RID: 14569
	Crop GetGropComponent();

	// Token: 0x060038EA RID: 14570
	void OverrideMaturityLevel(float percentage);

	// Token: 0x060038EB RID: 14571
	float DomesticGrowthTime();

	// Token: 0x060038EC RID: 14572
	float WildGrowthTime();
}
