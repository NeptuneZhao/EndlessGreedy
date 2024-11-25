using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C06 RID: 3078
public class CodexDividerLine : CodexWidget<CodexDividerLine>
{
	// Token: 0x06005E75 RID: 24181 RVA: 0x00231517 File Offset: 0x0022F717
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		contentGameObject.GetComponent<LayoutElement>().minWidth = 530f;
	}
}
