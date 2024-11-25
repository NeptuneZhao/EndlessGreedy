using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C64 RID: 3172
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class ImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x0600614D RID: 24909 RVA: 0x00244070 File Offset: 0x00242270
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.sprite != null)
		{
			base.aspectRatio = this.targetImage.sprite.rect.width / this.targetImage.sprite.rect.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x0600614E RID: 24910 RVA: 0x002440E1 File Offset: 0x002422E1
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x0600614F RID: 24911 RVA: 0x002440EF File Offset: 0x002422EF
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x040041F6 RID: 16886
	[SerializeField]
	private Image targetImage;
}
