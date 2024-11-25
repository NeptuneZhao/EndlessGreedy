using System;

// Token: 0x0200069D RID: 1693
public interface ISecondaryOutput
{
	// Token: 0x06002A87 RID: 10887
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06002A88 RID: 10888
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
