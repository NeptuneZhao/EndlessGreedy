using System;
using UnityEngine;

// Token: 0x020005BC RID: 1468
[AddComponentMenu("KMonoBehaviour/scripts/SkyVisibilityVisualizer")]
public class SkyVisibilityVisualizer : KMonoBehaviour
{
	// Token: 0x06002303 RID: 8963 RVA: 0x000C342E File Offset: 0x000C162E
	private static bool HasSkyVisibility(int cell)
	{
		return Grid.ExposedToSunlight[cell] >= 1;
	}

	// Token: 0x040013DF RID: 5087
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040013E0 RID: 5088
	public bool TwoWideOrgin;

	// Token: 0x040013E1 RID: 5089
	public int RangeMin;

	// Token: 0x040013E2 RID: 5090
	public int RangeMax;

	// Token: 0x040013E3 RID: 5091
	public int ScanVerticalStep;

	// Token: 0x040013E4 RID: 5092
	public bool SkipOnModuleInteriors;

	// Token: 0x040013E5 RID: 5093
	public bool AllOrNothingVisibility;

	// Token: 0x040013E6 RID: 5094
	public Func<int, bool> SkyVisibilityCb = new Func<int, bool>(SkyVisibilityVisualizer.HasSkyVisibility);
}
