using System;
using System.Collections.Generic;
using KSerialization.Converters;
using UnityEngine;

// Token: 0x02000C02 RID: 3074
public class ContentContainer
{
	// Token: 0x06005E41 RID: 24129 RVA: 0x002310E8 File Offset: 0x0022F2E8
	public ContentContainer()
	{
		this.content = new List<ICodexWidget>();
	}

	// Token: 0x06005E42 RID: 24130 RVA: 0x002310FB File Offset: 0x0022F2FB
	public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
	{
		this.content = content;
		this.contentLayout = contentLayout;
	}

	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x06005E43 RID: 24131 RVA: 0x00231111 File Offset: 0x0022F311
	// (set) Token: 0x06005E44 RID: 24132 RVA: 0x00231119 File Offset: 0x0022F319
	public List<ICodexWidget> content { get; set; }

	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x06005E45 RID: 24133 RVA: 0x00231122 File Offset: 0x0022F322
	// (set) Token: 0x06005E46 RID: 24134 RVA: 0x0023112A File Offset: 0x0022F32A
	public string lockID { get; set; }

	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x06005E47 RID: 24135 RVA: 0x00231133 File Offset: 0x0022F333
	// (set) Token: 0x06005E48 RID: 24136 RVA: 0x0023113B File Offset: 0x0022F33B
	[StringEnumConverter]
	public ContentContainer.ContentLayout contentLayout { get; set; }

	// Token: 0x170006FE RID: 1790
	// (get) Token: 0x06005E49 RID: 24137 RVA: 0x00231144 File Offset: 0x0022F344
	// (set) Token: 0x06005E4A RID: 24138 RVA: 0x0023114C File Offset: 0x0022F34C
	public bool showBeforeGeneratedContent { get; set; }

	// Token: 0x04003F19 RID: 16153
	public GameObject go;

	// Token: 0x02001CF6 RID: 7414
	public enum ContentLayout
	{
		// Token: 0x040085A6 RID: 34214
		Vertical,
		// Token: 0x040085A7 RID: 34215
		Horizontal,
		// Token: 0x040085A8 RID: 34216
		Grid,
		// Token: 0x040085A9 RID: 34217
		GridTwoColumn,
		// Token: 0x040085AA RID: 34218
		GridTwoColumnTall
	}
}
