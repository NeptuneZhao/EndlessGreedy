using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C86 RID: 3206
[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour
{
	// Token: 0x060062A2 RID: 25250 RVA: 0x0024D328 File Offset: 0x0024B528
	protected override void OnRectTransformDimensionsChange()
	{
		this.Layout();
	}

	// Token: 0x060062A3 RID: 25251 RVA: 0x0024D330 File Offset: 0x0024B530
	public void Layout()
	{
		KleiPermitDioramaVisScaler.Layout(this.root, this.scaleTarget, this.slot);
	}

	// Token: 0x060062A4 RID: 25252 RVA: 0x0024D34C File Offset: 0x0024B54C
	public static void Layout(RectTransform root, RectTransform scaleTarget, RectTransform slot)
	{
		float aspectRatio = 2.125f;
		AspectRatioFitter aspectRatioFitter = slot.FindOrAddComponent<AspectRatioFitter>();
		aspectRatioFitter.aspectRatio = aspectRatio;
		aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
		float num = 1700f;
		float a = Mathf.Max(0.1f, root.rect.width) / num;
		float num2 = 800f;
		float b = Mathf.Max(0.1f, root.rect.height) / num2;
		float d = Mathf.Max(a, b);
		scaleTarget.localScale = Vector3.one * d;
		scaleTarget.sizeDelta = new Vector2(1700f, 800f);
		scaleTarget.anchorMin = Vector2.one * 0.5f;
		scaleTarget.anchorMax = Vector2.one * 0.5f;
		scaleTarget.pivot = Vector2.one * 0.5f;
		scaleTarget.anchoredPosition = Vector2.zero;
	}

	// Token: 0x040042F1 RID: 17137
	public const float REFERENCE_WIDTH = 1700f;

	// Token: 0x040042F2 RID: 17138
	public const float REFERENCE_HEIGHT = 800f;

	// Token: 0x040042F3 RID: 17139
	[SerializeField]
	private RectTransform root;

	// Token: 0x040042F4 RID: 17140
	[SerializeField]
	private RectTransform scaleTarget;

	// Token: 0x040042F5 RID: 17141
	[SerializeField]
	private RectTransform slot;
}
