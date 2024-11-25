using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C01 RID: 3073
public abstract class CodexWidget<SubClass> : ICodexWidget
{
	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06005E39 RID: 24121 RVA: 0x00231079 File Offset: 0x0022F279
	// (set) Token: 0x06005E3A RID: 24122 RVA: 0x00231081 File Offset: 0x0022F281
	public int preferredWidth { get; set; }

	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x06005E3B RID: 24123 RVA: 0x0023108A File Offset: 0x0022F28A
	// (set) Token: 0x06005E3C RID: 24124 RVA: 0x00231092 File Offset: 0x0022F292
	public int preferredHeight { get; set; }

	// Token: 0x06005E3D RID: 24125 RVA: 0x0023109B File Offset: 0x0022F29B
	protected CodexWidget()
	{
		this.preferredWidth = -1;
		this.preferredHeight = -1;
	}

	// Token: 0x06005E3E RID: 24126 RVA: 0x002310B1 File Offset: 0x0022F2B1
	protected CodexWidget(int preferredWidth, int preferredHeight)
	{
		this.preferredWidth = preferredWidth;
		this.preferredHeight = preferredHeight;
	}

	// Token: 0x06005E3F RID: 24127
	public abstract void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

	// Token: 0x06005E40 RID: 24128 RVA: 0x002310C7 File Offset: 0x0022F2C7
	protected void ConfigurePreferredLayout(GameObject contentGameObject)
	{
		LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
		componentInChildren.preferredHeight = (float)this.preferredHeight;
		componentInChildren.preferredWidth = (float)this.preferredWidth;
	}
}
