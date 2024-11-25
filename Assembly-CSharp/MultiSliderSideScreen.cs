using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D82 RID: 3458
public class MultiSliderSideScreen : SideScreenContent
{
	// Token: 0x06006CCF RID: 27855 RVA: 0x0028EEF4 File Offset: 0x0028D0F4
	public override bool IsValidForTarget(GameObject target)
	{
		IMultiSliderControl component = target.GetComponent<IMultiSliderControl>();
		return component != null && component.SidescreenEnabled();
	}

	// Token: 0x06006CD0 RID: 27856 RVA: 0x0028EF13 File Offset: 0x0028D113
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IMultiSliderControl>();
		this.titleKey = this.target.SidescreenTitleKey;
		this.Refresh();
	}

	// Token: 0x06006CD1 RID: 27857 RVA: 0x0028EF4C File Offset: 0x0028D14C
	private void Refresh()
	{
		while (this.liveSliders.Count < this.target.sliderControls.Length)
		{
			GameObject gameObject = Util.KInstantiateUI(this.sliderPrefab.gameObject, this.sliderContainer.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			SliderSet sliderSet = new SliderSet();
			sliderSet.valueSlider = component.GetReference<KSlider>("Slider");
			sliderSet.numberInput = component.GetReference<KNumberInputField>("NumberInputField");
			if (sliderSet.numberInput != null)
			{
				sliderSet.numberInput.Activate();
			}
			sliderSet.targetLabel = component.GetReference<LocText>("TargetLabel");
			sliderSet.unitsLabel = component.GetReference<LocText>("UnitsLabel");
			sliderSet.minLabel = component.GetReference<LocText>("MinLabel");
			sliderSet.maxLabel = component.GetReference<LocText>("MaxLabel");
			sliderSet.SetupSlider(this.liveSliders.Count);
			this.liveSliders.Add(gameObject);
			this.sliderSets.Add(sliderSet);
		}
		for (int i = 0; i < this.liveSliders.Count; i++)
		{
			if (i >= this.target.sliderControls.Length)
			{
				this.liveSliders[i].SetActive(false);
			}
			else
			{
				if (!this.liveSliders[i].activeSelf)
				{
					this.liveSliders[i].SetActive(true);
					this.liveSliders[i].gameObject.SetActive(true);
				}
				this.sliderSets[i].SetTarget(this.target.sliderControls[i], i);
			}
		}
	}

	// Token: 0x04004A35 RID: 18997
	public LayoutElement sliderPrefab;

	// Token: 0x04004A36 RID: 18998
	public RectTransform sliderContainer;

	// Token: 0x04004A37 RID: 18999
	private IMultiSliderControl target;

	// Token: 0x04004A38 RID: 19000
	private List<GameObject> liveSliders = new List<GameObject>();

	// Token: 0x04004A39 RID: 19001
	private List<SliderSet> sliderSets = new List<SliderSet>();
}
