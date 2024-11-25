using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D1A RID: 3354
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class RawImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x060068C0 RID: 26816 RVA: 0x00273994 File Offset: 0x00271B94
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.texture != null)
		{
			base.aspectRatio = (float)this.targetImage.texture.width / (float)this.targetImage.texture.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x060068C1 RID: 26817 RVA: 0x002739F7 File Offset: 0x00271BF7
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x060068C2 RID: 26818 RVA: 0x00273A05 File Offset: 0x00271C05
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x040046E3 RID: 18147
	[SerializeField]
	private RawImage targetImage;
}
