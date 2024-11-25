using System;
using UnityEngine;

// Token: 0x02000416 RID: 1046
public static class RectTransformExtensions
{
	// Token: 0x0600163F RID: 5695 RVA: 0x00078220 File Offset: 0x00076420
	public static RectTransform Fill(this RectTransform rectTransform)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		return rectTransform;
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x00078284 File Offset: 0x00076484
	public static RectTransform Fill(this RectTransform rectTransform, Padding padding)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(padding.left, padding.bottom);
		rectTransform.sizeDelta = new Vector2(-padding.right, -padding.top);
		return rectTransform;
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x000782EC File Offset: 0x000764EC
	public static RectTransform Pivot(this RectTransform rectTransform, float x, float y)
	{
		rectTransform.pivot = new Vector2(x, y);
		return rectTransform;
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x000782FC File Offset: 0x000764FC
	public static RectTransform Pivot(this RectTransform rectTransform, Vector2 pivot)
	{
		rectTransform.pivot = pivot;
		return rectTransform;
	}
}
