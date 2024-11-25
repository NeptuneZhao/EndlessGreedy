using System;

// Token: 0x020008CE RID: 2254
public interface IEnergyProducer
{
	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06004004 RID: 16388
	float JoulesAvailable { get; }

	// Token: 0x06004005 RID: 16389
	void ConsumeEnergy(float joules);
}
