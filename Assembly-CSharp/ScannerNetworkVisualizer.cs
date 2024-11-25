using System;
using UnityEngine;

// Token: 0x020005B6 RID: 1462
[AddComponentMenu("KMonoBehaviour/scripts/ScannerNetworkVisualizer")]
public class ScannerNetworkVisualizer : KMonoBehaviour
{
	// Token: 0x060022EF RID: 8943 RVA: 0x000C30F7 File Offset: 0x000C12F7
	protected override void OnSpawn()
	{
		Components.ScannerVisualizers.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000C310F File Offset: 0x000C130F
	protected override void OnCleanUp()
	{
		Components.ScannerVisualizers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x040013D5 RID: 5077
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040013D6 RID: 5078
	public int RangeMin;

	// Token: 0x040013D7 RID: 5079
	public int RangeMax;
}
