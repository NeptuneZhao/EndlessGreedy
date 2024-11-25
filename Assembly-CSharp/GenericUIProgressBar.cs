using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C4E RID: 3150
[AddComponentMenu("KMonoBehaviour/scripts/GenericUIProgressBar")]
public class GenericUIProgressBar : KMonoBehaviour
{
	// Token: 0x060060D2 RID: 24786 RVA: 0x002403B5 File Offset: 0x0023E5B5
	public void SetMaxValue(float max)
	{
		this.maxValue = max;
	}

	// Token: 0x060060D3 RID: 24787 RVA: 0x002403C0 File Offset: 0x0023E5C0
	public void SetFillPercentage(float value)
	{
		this.fill.fillAmount = value;
		this.label.text = Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value)) + "/" + this.maxValue.ToString();
	}

	// Token: 0x04004170 RID: 16752
	public Image fill;

	// Token: 0x04004171 RID: 16753
	public LocText label;

	// Token: 0x04004172 RID: 16754
	private float maxValue;
}
