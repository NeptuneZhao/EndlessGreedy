using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C0C RID: 3084
public class CodexLabelWithLargeIcon : CodexLabelWithIcon
{
	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x06005E8D RID: 24205 RVA: 0x00231776 File Offset: 0x0022F976
	// (set) Token: 0x06005E8E RID: 24206 RVA: 0x0023177E File Offset: 0x0022F97E
	public string linkID { get; set; }

	// Token: 0x06005E8F RID: 24207 RVA: 0x00231787 File Offset: 0x0022F987
	public CodexLabelWithLargeIcon()
	{
	}

	// Token: 0x06005E90 RID: 24208 RVA: 0x00231790 File Offset: 0x0022F990
	public CodexLabelWithLargeIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, string targetEntrylinkID) : base(text, style, coloredSprite, 128, 128)
	{
		base.icon = new CodexImage(128, 128, coloredSprite);
		base.label = new CodexText(text, style, null);
		this.linkID = targetEntrylinkID;
	}

	// Token: 0x06005E91 RID: 24209 RVA: 0x002317DC File Offset: 0x0022F9DC
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.icon.ConfigureImage(contentGameObject.GetComponentsInChildren<Image>()[1]);
		if (base.icon.preferredWidth != -1 && base.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentsInChildren<Image>()[1].GetComponent<LayoutElement>();
			component.minWidth = (float)base.icon.preferredHeight;
			component.minHeight = (float)base.icon.preferredWidth;
			component.preferredHeight = (float)base.icon.preferredHeight;
			component.preferredWidth = (float)base.icon.preferredWidth;
		}
		base.label.text = UI.StripLinkFormatting(base.label.text);
		base.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		contentGameObject.GetComponent<KButton>().ClearOnClick();
		contentGameObject.GetComponent<KButton>().onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(this.linkID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
	}
}
