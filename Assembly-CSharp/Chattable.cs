using System;
using UnityEngine;

// Token: 0x02000538 RID: 1336
[AddComponentMenu("KMonoBehaviour/scripts/Chattable")]
public class Chattable : KMonoBehaviour, IApproachable
{
	// Token: 0x06001E75 RID: 7797 RVA: 0x000A9A31 File Offset: 0x000A7C31
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Chat;
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x000A9A38 File Offset: 0x000A7C38
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
