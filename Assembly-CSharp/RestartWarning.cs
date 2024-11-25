using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A5B RID: 2651
public class RestartWarning : MonoBehaviour
{
	// Token: 0x06004CEB RID: 19691 RVA: 0x001B7626 File Offset: 0x001B5826
	private void Update()
	{
		if (RestartWarning.ShouldWarn)
		{
			this.text.enabled = true;
			this.image.enabled = true;
		}
	}

	// Token: 0x0400331D RID: 13085
	public static bool ShouldWarn;

	// Token: 0x0400331E RID: 13086
	public LocText text;

	// Token: 0x0400331F RID: 13087
	public Image image;
}
