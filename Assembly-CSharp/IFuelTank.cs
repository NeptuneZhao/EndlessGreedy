using System;

// Token: 0x020008F4 RID: 2292
public interface IFuelTank
{
	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x060041E1 RID: 16865
	IStorage Storage { get; }

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x060041E2 RID: 16866
	bool ConsumeFuelOnLand { get; }

	// Token: 0x060041E3 RID: 16867
	void DEBUG_FillTank();
}
