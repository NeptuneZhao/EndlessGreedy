using System;
using UnityEngine;

// Token: 0x020008FD RID: 2301
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Insulator")]
public class Insulator : KMonoBehaviour
{
	// Token: 0x06004220 RID: 16928 RVA: 0x0017804A File Offset: 0x0017624A
	protected override void OnSpawn()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x0017807C File Offset: 0x0017627C
	protected override void OnCleanUp()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), 1f);
	}

	// Token: 0x04002BD3 RID: 11219
	[MyCmpReq]
	private Building building;

	// Token: 0x04002BD4 RID: 11220
	[SerializeField]
	public CellOffset offset = CellOffset.none;
}
