using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D20 RID: 3360
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeaderRow")]
public class ReportScreenHeaderRow : KMonoBehaviour
{
	// Token: 0x060068FC RID: 26876 RVA: 0x00275024 File Offset: 0x00273224
	public void SetLine(ReportManager.ReportGroup reportGroup)
	{
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		component.minWidth = (component.preferredWidth = this.nameWidth);
		this.spacer.minWidth = this.groupSpacerWidth;
		this.name.text = reportGroup.stringKey;
	}

	// Token: 0x0400471E RID: 18206
	[SerializeField]
	public new LocText name;

	// Token: 0x0400471F RID: 18207
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x04004720 RID: 18208
	[SerializeField]
	private Image bgImage;

	// Token: 0x04004721 RID: 18209
	public float groupSpacerWidth;

	// Token: 0x04004722 RID: 18210
	private float nameWidth = 164f;

	// Token: 0x04004723 RID: 18211
	[SerializeField]
	private Color oddRowColor;
}
