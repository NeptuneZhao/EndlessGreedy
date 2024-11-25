using System;

// Token: 0x02000D51 RID: 3409
public interface IUserControlledCapacity
{
	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x06006B4C RID: 27468
	// (set) Token: 0x06006B4D RID: 27469
	float UserMaxCapacity { get; set; }

	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x06006B4E RID: 27470
	float AmountStored { get; }

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06006B4F RID: 27471
	float MinCapacity { get; }

	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x06006B50 RID: 27472
	float MaxCapacity { get; }

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x06006B51 RID: 27473
	bool WholeValues { get; }

	// Token: 0x1700078C RID: 1932
	// (get) Token: 0x06006B52 RID: 27474
	LocString CapacityUnits { get; }
}
