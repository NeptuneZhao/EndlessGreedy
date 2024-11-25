using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BFD RID: 3069
public class CodexImageLayoutMB : UIBehaviour
{
	// Token: 0x06005E16 RID: 24086 RVA: 0x0022F0EC File Offset: 0x0022D2EC
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (this.image.preserveAspect && this.image.sprite != null && this.image.sprite)
		{
			float num = this.image.sprite.rect.height / this.image.sprite.rect.width;
			this.layoutElement.preferredHeight = num * this.rectTransform.sizeDelta.x;
			this.layoutElement.minHeight = this.layoutElement.preferredHeight;
			return;
		}
		this.layoutElement.preferredHeight = -1f;
		this.layoutElement.preferredWidth = -1f;
		this.layoutElement.minHeight = -1f;
		this.layoutElement.minWidth = -1f;
		this.layoutElement.flexibleHeight = -1f;
		this.layoutElement.flexibleWidth = -1f;
		this.layoutElement.ignoreLayout = false;
	}

	// Token: 0x04003EDB RID: 16091
	public RectTransform rectTransform;

	// Token: 0x04003EDC RID: 16092
	public LayoutElement layoutElement;

	// Token: 0x04003EDD RID: 16093
	public Image image;
}
