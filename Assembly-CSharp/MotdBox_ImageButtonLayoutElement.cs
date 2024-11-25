using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE9 RID: 3305
public class MotdBox_ImageButtonLayoutElement : LayoutElement
{
	// Token: 0x06006668 RID: 26216 RVA: 0x002641E0 File Offset: 0x002623E0
	private void UpdateState()
	{
		MotdBox_ImageButtonLayoutElement.Style style = this.style;
		if (style == MotdBox_ImageButtonLayoutElement.Style.WidthExpandsBasedOnHeight)
		{
			this.flexibleHeight = 1f;
			this.preferredHeight = -1f;
			this.minHeight = -1f;
			this.flexibleWidth = 0f;
			this.preferredWidth = this.rectTransform().sizeDelta.y * this.heightToWidthRatio;
			this.minWidth = this.preferredWidth;
			this.ignoreLayout = false;
			return;
		}
		if (style != MotdBox_ImageButtonLayoutElement.Style.HeightExpandsBasedOnWidth)
		{
			return;
		}
		this.flexibleWidth = 1f;
		this.preferredWidth = -1f;
		this.minWidth = -1f;
		this.flexibleHeight = 0f;
		this.preferredHeight = this.rectTransform().sizeDelta.x / this.heightToWidthRatio;
		this.minHeight = this.preferredHeight;
		this.ignoreLayout = false;
	}

	// Token: 0x06006669 RID: 26217 RVA: 0x002642B5 File Offset: 0x002624B5
	protected override void OnTransformParentChanged()
	{
		this.UpdateState();
		base.OnTransformParentChanged();
	}

	// Token: 0x0600666A RID: 26218 RVA: 0x002642C3 File Offset: 0x002624C3
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateState();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04004518 RID: 17688
	[SerializeField]
	private float heightToWidthRatio;

	// Token: 0x04004519 RID: 17689
	[SerializeField]
	private MotdBox_ImageButtonLayoutElement.Style style;

	// Token: 0x02001DF3 RID: 7667
	private enum Style
	{
		// Token: 0x040088C5 RID: 35013
		WidthExpandsBasedOnHeight,
		// Token: 0x040088C6 RID: 35014
		HeightExpandsBasedOnWidth
	}
}
