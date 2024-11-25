using System;
using UnityEngine;

// Token: 0x02000528 RID: 1320
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Approachable")]
public class Approachable : KMonoBehaviour, IApproachable
{
	// Token: 0x06001DAE RID: 7598 RVA: 0x000A49B6 File Offset: 0x000A2BB6
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x000A49BD File Offset: 0x000A2BBD
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}
}
