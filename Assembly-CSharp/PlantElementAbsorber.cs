using System;

// Token: 0x020009E8 RID: 2536
public struct PlantElementAbsorber
{
	// Token: 0x0600498C RID: 18828 RVA: 0x001A52C5 File Offset: 0x001A34C5
	public void Clear()
	{
		this.storage = null;
		this.consumedElements = null;
	}

	// Token: 0x0400301D RID: 12317
	public Storage storage;

	// Token: 0x0400301E RID: 12318
	public PlantElementAbsorber.LocalInfo localInfo;

	// Token: 0x0400301F RID: 12319
	public HandleVector<int>.Handle[] accumulators;

	// Token: 0x04003020 RID: 12320
	public PlantElementAbsorber.ConsumeInfo[] consumedElements;

	// Token: 0x020019EB RID: 6635
	public struct ConsumeInfo
	{
		// Token: 0x06009E71 RID: 40561 RVA: 0x00377F2B File Offset: 0x0037612B
		public ConsumeInfo(Tag tag, float mass_consumption_rate)
		{
			this.tag = tag;
			this.massConsumptionRate = mass_consumption_rate;
		}

		// Token: 0x04007ADB RID: 31451
		public Tag tag;

		// Token: 0x04007ADC RID: 31452
		public float massConsumptionRate;
	}

	// Token: 0x020019EC RID: 6636
	public struct LocalInfo
	{
		// Token: 0x04007ADD RID: 31453
		public Tag tag;

		// Token: 0x04007ADE RID: 31454
		public float massConsumptionRate;
	}
}
