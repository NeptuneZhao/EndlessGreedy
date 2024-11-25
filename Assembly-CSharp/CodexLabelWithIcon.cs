using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C0B RID: 3083
public class CodexLabelWithIcon : CodexWidget<CodexLabelWithIcon>
{
	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x06005E85 RID: 24197 RVA: 0x00231666 File Offset: 0x0022F866
	// (set) Token: 0x06005E86 RID: 24198 RVA: 0x0023166E File Offset: 0x0022F86E
	public CodexImage icon { get; set; }

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x06005E87 RID: 24199 RVA: 0x00231677 File Offset: 0x0022F877
	// (set) Token: 0x06005E88 RID: 24200 RVA: 0x0023167F File Offset: 0x0022F87F
	public CodexText label { get; set; }

	// Token: 0x06005E89 RID: 24201 RVA: 0x00231688 File Offset: 0x0022F888
	public CodexLabelWithIcon()
	{
	}

	// Token: 0x06005E8A RID: 24202 RVA: 0x00231690 File Offset: 0x0022F890
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06005E8B RID: 24203 RVA: 0x002316B2 File Offset: 0x0022F8B2
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06005E8C RID: 24204 RVA: 0x002316D8 File Offset: 0x0022F8D8
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.icon.ConfigureImage(contentGameObject.GetComponentInChildren<Image>());
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentInChildren<Image>().GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
	}
}
