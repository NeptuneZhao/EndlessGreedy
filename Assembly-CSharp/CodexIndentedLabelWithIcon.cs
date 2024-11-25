using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C08 RID: 3080
public class CodexIndentedLabelWithIcon : CodexWidget<CodexIndentedLabelWithIcon>
{
	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x06005E79 RID: 24185 RVA: 0x0023153B File Offset: 0x0022F73B
	// (set) Token: 0x06005E7A RID: 24186 RVA: 0x00231543 File Offset: 0x0022F743
	public CodexImage icon { get; set; }

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x06005E7B RID: 24187 RVA: 0x0023154C File Offset: 0x0022F74C
	// (set) Token: 0x06005E7C RID: 24188 RVA: 0x00231554 File Offset: 0x0022F754
	public CodexText label { get; set; }

	// Token: 0x06005E7D RID: 24189 RVA: 0x0023155D File Offset: 0x0022F75D
	public CodexIndentedLabelWithIcon()
	{
	}

	// Token: 0x06005E7E RID: 24190 RVA: 0x00231565 File Offset: 0x0022F765
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06005E7F RID: 24191 RVA: 0x00231587 File Offset: 0x0022F787
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06005E80 RID: 24192 RVA: 0x002315B0 File Offset: 0x0022F7B0
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		Image componentInChildren = contentGameObject.GetComponentInChildren<Image>();
		this.icon.ConfigureImage(componentInChildren);
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
	}
}
