using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D0D RID: 3341
public class PlanCategoryNotifications : MonoBehaviour
{
	// Token: 0x0600680F RID: 26639 RVA: 0x0026E763 File Offset: 0x0026C963
	public void ToggleAttention(bool active)
	{
		if (!this.AttentionImage)
		{
			return;
		}
		this.AttentionImage.gameObject.SetActive(active);
	}

	// Token: 0x04004642 RID: 17986
	public Image AttentionImage;
}
