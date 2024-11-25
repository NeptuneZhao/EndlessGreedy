using System;
using UnityEngine;

// Token: 0x02000DA2 RID: 3490
public class SideScreen : KScreen
{
	// Token: 0x06006E37 RID: 28215 RVA: 0x002978A2 File Offset: 0x00295AA2
	public void SetContent(SideScreenContent sideScreenContent, GameObject target)
	{
		if (sideScreenContent.transform.parent != this.contentBody.transform)
		{
			sideScreenContent.transform.SetParent(this.contentBody.transform);
		}
		sideScreenContent.SetTarget(target);
	}

	// Token: 0x04004B3E RID: 19262
	[SerializeField]
	private GameObject contentBody;
}
