using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DAD RID: 3501
public class SingleSliderSideScreen : SideScreenContent
{
	// Token: 0x06006E98 RID: 28312 RVA: 0x00298B0C File Offset: 0x00296D0C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x06006E99 RID: 28313 RVA: 0x00298B48 File Offset: 0x00296D48
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		ISingleSliderControl singleSliderControl = target.GetComponent<ISingleSliderControl>();
		singleSliderControl = ((singleSliderControl != null) ? singleSliderControl : target.GetSMI<ISingleSliderControl>());
		return singleSliderControl != null && !component.IsPrefabID("HydrogenGenerator".ToTag()) && !component.IsPrefabID("MethaneGenerator".ToTag()) && !component.IsPrefabID("PetroleumGenerator".ToTag()) && !component.IsPrefabID("DevGenerator".ToTag()) && !component.HasTag(GameTags.DeadReactor) && singleSliderControl.GetSliderMin(0) != singleSliderControl.GetSliderMax(0);
	}

	// Token: 0x06006E9A RID: 28314 RVA: 0x00298BE0 File Offset: 0x00296DE0
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ISingleSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<ISingleSliderControl>();
			if (this.target == null)
			{
				global::Debug.LogError("The gameObject received does not contain a ISingleSliderControl implementation");
				return;
			}
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x04004B6A RID: 19306
	private ISingleSliderControl target;

	// Token: 0x04004B6B RID: 19307
	public List<SliderSet> sliderSets;
}
