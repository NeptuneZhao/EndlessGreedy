using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C00 RID: 3072
public interface ICodexWidget
{
	// Token: 0x06005E38 RID: 24120
	void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);
}
