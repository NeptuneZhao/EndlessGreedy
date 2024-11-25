using System;
using UnityEngine;

// Token: 0x020006A3 RID: 1699
[AddComponentMenu("KMonoBehaviour/scripts/ConduitSecondaryOutput")]
public class ConduitSecondaryOutput : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x06002AAF RID: 10927 RVA: 0x000F0AAC File Offset: 0x000EECAC
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x000F0ABC File Offset: 0x000EECBC
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.portInfo.conduitType)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04001891 RID: 6289
	[SerializeField]
	public ConduitPortInfo portInfo;
}
