using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DEB RID: 3563
[AddComponentMenu("KMonoBehaviour/scripts/VideoOverlay")]
public class VideoOverlay : KMonoBehaviour
{
	// Token: 0x06007120 RID: 28960 RVA: 0x002ACDF4 File Offset: 0x002AAFF4
	public void SetText(List<string> strings)
	{
		if (strings.Count != this.textFields.Count)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				base.name,
				"expects",
				this.textFields.Count,
				"strings passed to it, got",
				strings.Count
			});
		}
		for (int i = 0; i < this.textFields.Count; i++)
		{
			this.textFields[i].text = strings[i];
		}
	}

	// Token: 0x04004DC8 RID: 19912
	public List<LocText> textFields;
}
