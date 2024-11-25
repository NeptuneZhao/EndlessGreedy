using System;

// Token: 0x0200069E RID: 1694
public interface ISecondaryInput
{
	// Token: 0x06002A89 RID: 10889
	bool HasSecondaryConduitType(ConduitType type);

	// Token: 0x06002A8A RID: 10890
	CellOffset GetSecondaryConduitOffset(ConduitType type);
}
