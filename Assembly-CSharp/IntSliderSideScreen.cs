using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D75 RID: 3445
public class IntSliderSideScreen : SideScreenContent
{
	// Token: 0x06006C5A RID: 27738 RVA: 0x0028C130 File Offset: 0x0028A330
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
			this.sliderSets[i].valueSlider.wholeNumbers = true;
		}
	}

	// Token: 0x06006C5B RID: 27739 RVA: 0x0028C182 File Offset: 0x0028A382
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IIntSliderControl>() != null || target.GetSMI<IIntSliderControl>() != null;
	}

	// Token: 0x06006C5C RID: 27740 RVA: 0x0028C198 File Offset: 0x0028A398
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IIntSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IIntSliderControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a Manual Generator component");
			return;
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x040049E2 RID: 18914
	private IIntSliderControl target;

	// Token: 0x040049E3 RID: 18915
	public List<SliderSet> sliderSets;
}
