using System;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
[AddComponentMenu("KMonoBehaviour/scripts/RangeVisualizer")]
public class RangeVisualizer : KMonoBehaviour
{
	// Token: 0x04001365 RID: 4965
	public Vector2I OriginOffset;

	// Token: 0x04001366 RID: 4966
	public Vector2I RangeMin;

	// Token: 0x04001367 RID: 4967
	public Vector2I RangeMax;

	// Token: 0x04001368 RID: 4968
	public bool TestLineOfSight = true;

	// Token: 0x04001369 RID: 4969
	public bool BlockingTileVisible;

	// Token: 0x0400136A RID: 4970
	public Func<int, bool> BlockingVisibleCb;

	// Token: 0x0400136B RID: 4971
	public Func<int, bool> BlockingCb = new Func<int, bool>(Grid.IsSolidCell);

	// Token: 0x0400136C RID: 4972
	public bool AllowLineOfSightInvalidCells;
}
