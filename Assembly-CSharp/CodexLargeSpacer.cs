using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C09 RID: 3081
public class CodexLargeSpacer : CodexWidget<CodexLargeSpacer>
{
	// Token: 0x06005E81 RID: 24193 RVA: 0x0023164B File Offset: 0x0022F84B
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
