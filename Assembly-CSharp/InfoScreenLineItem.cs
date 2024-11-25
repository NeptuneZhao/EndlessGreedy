using System;
using UnityEngine;

// Token: 0x02000C68 RID: 3176
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenLineItem")]
public class InfoScreenLineItem : KMonoBehaviour
{
	// Token: 0x06006171 RID: 24945 RVA: 0x0024467D File Offset: 0x0024287D
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x06006172 RID: 24946 RVA: 0x0024468B File Offset: 0x0024288B
	public void SetTooltip(string tooltip)
	{
		this.toolTip.toolTip = tooltip;
	}

	// Token: 0x0400420C RID: 16908
	[SerializeField]
	private LocText locText;

	// Token: 0x0400420D RID: 16909
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x0400420E RID: 16910
	private string text;

	// Token: 0x0400420F RID: 16911
	private string tooltip;
}
