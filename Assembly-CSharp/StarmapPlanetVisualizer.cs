using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DCE RID: 3534
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanetVisualizer")]
public class StarmapPlanetVisualizer : KMonoBehaviour
{
	// Token: 0x04004CD8 RID: 19672
	public Image image;

	// Token: 0x04004CD9 RID: 19673
	public LocText label;

	// Token: 0x04004CDA RID: 19674
	public MultiToggle button;

	// Token: 0x04004CDB RID: 19675
	public RectTransform selection;

	// Token: 0x04004CDC RID: 19676
	public GameObject analysisSelection;

	// Token: 0x04004CDD RID: 19677
	public Image unknownBG;

	// Token: 0x04004CDE RID: 19678
	public GameObject rocketIconContainer;
}
