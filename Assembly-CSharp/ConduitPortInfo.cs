using System;

// Token: 0x020006A1 RID: 1697
[Serializable]
public class ConduitPortInfo
{
	// Token: 0x06002AAB RID: 10923 RVA: 0x000F0A5D File Offset: 0x000EEC5D
	public ConduitPortInfo(ConduitType type, CellOffset offset)
	{
		this.conduitType = type;
		this.offset = offset;
	}

	// Token: 0x0400188E RID: 6286
	public ConduitType conduitType;

	// Token: 0x0400188F RID: 6287
	public CellOffset offset;
}
