using System;
using UnityEngine;

// Token: 0x020006A2 RID: 1698
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryInput")]
public class ConduitSecondaryInput : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x06002AAC RID: 10924 RVA: 0x000F0A73 File Offset: 0x000EEC73
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x000F0A83 File Offset: 0x000EEC83
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001890 RID: 6288
	[SerializeField]
	public ConduitPortInfo portInfo;
}
