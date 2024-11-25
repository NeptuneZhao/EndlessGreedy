using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C4F RID: 3151
[AddComponentMenu("KMonoBehaviour/scripts/GoodnessSlider")]
public class GoodnessSlider : KMonoBehaviour
{
	// Token: 0x060060D5 RID: 24789 RVA: 0x00240419 File Offset: 0x0023E619
	protected override void OnSpawn()
	{
		base.Spawn();
		this.UpdateValues();
	}

	// Token: 0x060060D6 RID: 24790 RVA: 0x00240428 File Offset: 0x0023E628
	public void UpdateValues()
	{
		this.text.color = (this.fill.color = this.gradient.Evaluate(this.slider.value));
		for (int i = 0; i < this.gradient.colorKeys.Length; i++)
		{
			if (this.gradient.colorKeys[i].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
			if (i == this.gradient.colorKeys.Length - 1 && this.gradient.colorKeys[i - 1].time < this.slider.value)
			{
				this.text.text = this.names[i];
			}
		}
	}

	// Token: 0x04004173 RID: 16755
	public Image icon;

	// Token: 0x04004174 RID: 16756
	public Text text;

	// Token: 0x04004175 RID: 16757
	public Slider slider;

	// Token: 0x04004176 RID: 16758
	public Image fill;

	// Token: 0x04004177 RID: 16759
	public Gradient gradient;

	// Token: 0x04004178 RID: 16760
	public string[] names;
}
