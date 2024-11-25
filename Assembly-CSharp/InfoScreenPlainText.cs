using System;
using UnityEngine;

// Token: 0x02000C69 RID: 3177
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenPlainText")]
public class InfoScreenPlainText : KMonoBehaviour
{
	// Token: 0x06006174 RID: 24948 RVA: 0x002446A1 File Offset: 0x002428A1
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x04004210 RID: 16912
	[SerializeField]
	private LocText locText;
}
