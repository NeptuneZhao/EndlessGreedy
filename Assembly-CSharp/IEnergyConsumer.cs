using System;

// Token: 0x0200087D RID: 2173
public interface IEnergyConsumer : ICircuitConnected
{
	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06003CCD RID: 15565
	float WattsUsed { get; }

	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x06003CCE RID: 15566
	float WattsNeededWhenActive { get; }

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06003CCF RID: 15567
	int PowerSortOrder { get; }

	// Token: 0x06003CD0 RID: 15568
	void SetConnectionStatus(CircuitManager.ConnectionStatus status);

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06003CD1 RID: 15569
	string Name { get; }

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x06003CD2 RID: 15570
	bool IsConnected { get; }

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x06003CD3 RID: 15571
	bool IsPowered { get; }
}
