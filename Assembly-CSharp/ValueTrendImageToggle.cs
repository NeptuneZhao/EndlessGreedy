using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DE9 RID: 3561
public class ValueTrendImageToggle : MonoBehaviour
{
	// Token: 0x06007117 RID: 28951 RVA: 0x002ACC28 File Offset: 0x002AAE28
	public void SetValue(AmountInstance ainstance)
	{
		float delta = ainstance.GetDelta();
		Sprite sprite = null;
		if (ainstance.paused || delta == 0f)
		{
			this.targetImage.gameObject.SetActive(false);
		}
		else
		{
			this.targetImage.gameObject.SetActive(true);
			if (delta <= -ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Down_Three;
			}
			else if (delta <= -ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Down_Two;
			}
			else if (delta <= 0f)
			{
				sprite = this.Down_One;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Up_Three;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Up_Two;
			}
			else if (delta > 0f)
			{
				sprite = this.Up_One;
			}
		}
		this.targetImage.sprite = sprite;
	}

	// Token: 0x04004DBE RID: 19902
	public Image targetImage;

	// Token: 0x04004DBF RID: 19903
	public Sprite Up_One;

	// Token: 0x04004DC0 RID: 19904
	public Sprite Up_Two;

	// Token: 0x04004DC1 RID: 19905
	public Sprite Up_Three;

	// Token: 0x04004DC2 RID: 19906
	public Sprite Down_One;

	// Token: 0x04004DC3 RID: 19907
	public Sprite Down_Two;

	// Token: 0x04004DC4 RID: 19908
	public Sprite Down_Three;

	// Token: 0x04004DC5 RID: 19909
	public Sprite Zero;
}
