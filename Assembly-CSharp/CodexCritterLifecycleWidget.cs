using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C14 RID: 3092
public class CodexCritterLifecycleWidget : CodexWidget<CodexCritterLifecycleWidget>
{
	// Token: 0x06005EC1 RID: 24257 RVA: 0x002333AC File Offset: 0x002315AC
	private CodexCritterLifecycleWidget()
	{
	}

	// Token: 0x06005EC2 RID: 24258 RVA: 0x002333B4 File Offset: 0x002315B4
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("EggIcon").sprite = null;
		component.GetReference<Image>("EggIcon").color = Color.white;
		component.GetReference<LocText>("EggToBabyLabel").text = "";
		component.GetReference<Image>("BabyIcon").sprite = null;
		component.GetReference<Image>("BabyIcon").color = Color.white;
		component.GetReference<LocText>("BabyToAdultLabel").text = "";
		component.GetReference<Image>("AdultIcon").sprite = null;
		component.GetReference<Image>("AdultIcon").color = Color.white;
	}
}
