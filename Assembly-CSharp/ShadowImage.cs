using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D3B RID: 3387
public class ShadowImage : ShadowRect
{
	// Token: 0x06006A81 RID: 27265 RVA: 0x00281CC8 File Offset: 0x0027FEC8
	protected override void MatchRect()
	{
		base.MatchRect();
		if (this.RectMain == null || this.RectShadow == null)
		{
			return;
		}
		if (this.shadowImage == null)
		{
			this.shadowImage = this.RectShadow.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			this.mainImage = this.RectMain.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			if (this.shadowImage != null)
			{
				this.shadowImage.color = Color.clear;
			}
			return;
		}
		if (this.shadowImage == null)
		{
			return;
		}
		if (this.shadowImage.sprite != this.mainImage.sprite)
		{
			this.shadowImage.sprite = this.mainImage.sprite;
		}
		if (this.shadowImage.color != this.shadowColor)
		{
			if (this.shadowImage.sprite != null)
			{
				this.shadowImage.color = this.shadowColor;
				return;
			}
			this.shadowImage.color = Color.clear;
		}
	}

	// Token: 0x04004896 RID: 18582
	private Image shadowImage;

	// Token: 0x04004897 RID: 18583
	private Image mainImage;
}
