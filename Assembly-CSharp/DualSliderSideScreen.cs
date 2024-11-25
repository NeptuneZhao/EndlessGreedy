using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D67 RID: 3431
public class DualSliderSideScreen : SideScreenContent
{
	// Token: 0x06006C0A RID: 27658 RVA: 0x0028A5F0 File Offset: 0x002887F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x06006C0B RID: 27659 RVA: 0x0028A62B File Offset: 0x0028882B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDualSliderControl>() != null;
	}

	// Token: 0x06006C0C RID: 27660 RVA: 0x0028A638 File Offset: 0x00288838
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IDualSliderControl>();
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

	// Token: 0x040049AA RID: 18858
	private IDualSliderControl target;

	// Token: 0x040049AB RID: 18859
	public List<SliderSet> sliderSets;
}
