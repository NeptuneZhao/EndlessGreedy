using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DFB RID: 3579
public class OptionSelector : MonoBehaviour
{
	// Token: 0x06007191 RID: 29073 RVA: 0x002AFA51 File Offset: 0x002ADC51
	private void Start()
	{
		this.selectedItem.GetComponent<KButton>().onBtnClick += this.OnClick;
	}

	// Token: 0x06007192 RID: 29074 RVA: 0x002AFA6F File Offset: 0x002ADC6F
	public void Initialize(object id)
	{
		this.id = id;
	}

	// Token: 0x06007193 RID: 29075 RVA: 0x002AFA78 File Offset: 0x002ADC78
	private void OnClick(KKeyCode button)
	{
		if (button == KKeyCode.Mouse0)
		{
			this.OnChangePriority(this.id, 1);
			return;
		}
		if (button != KKeyCode.Mouse1)
		{
			return;
		}
		this.OnChangePriority(this.id, -1);
	}

	// Token: 0x06007194 RID: 29076 RVA: 0x002AFAB0 File Offset: 0x002ADCB0
	public void ConfigureItem(bool disabled, OptionSelector.DisplayOptionInfo display_info)
	{
		HierarchyReferences component = this.selectedItem.GetComponent<HierarchyReferences>();
		KImage kimage = component.GetReference("BG") as KImage;
		if (display_info.bgOptions == null)
		{
			kimage.gameObject.SetActive(false);
		}
		else
		{
			kimage.sprite = display_info.bgOptions[display_info.bgIndex];
		}
		KImage kimage2 = component.GetReference("FG") as KImage;
		if (display_info.fgOptions == null)
		{
			kimage2.gameObject.SetActive(false);
		}
		else
		{
			kimage2.sprite = display_info.fgOptions[display_info.fgIndex];
		}
		KImage kimage3 = component.GetReference("Fill") as KImage;
		if (kimage3 != null)
		{
			kimage3.enabled = !disabled;
			kimage3.color = display_info.fillColour;
		}
		KImage kimage4 = component.GetReference("Outline") as KImage;
		if (kimage4 != null)
		{
			kimage4.enabled = !disabled;
		}
	}

	// Token: 0x04004E62 RID: 20066
	private object id;

	// Token: 0x04004E63 RID: 20067
	public Action<object, int> OnChangePriority;

	// Token: 0x04004E64 RID: 20068
	[SerializeField]
	private KImage selectedItem;

	// Token: 0x04004E65 RID: 20069
	[SerializeField]
	private KImage itemTemplate;

	// Token: 0x02001EFF RID: 7935
	public class DisplayOptionInfo
	{
		// Token: 0x04008C4D RID: 35917
		public IList<Sprite> bgOptions;

		// Token: 0x04008C4E RID: 35918
		public IList<Sprite> fgOptions;

		// Token: 0x04008C4F RID: 35919
		public int bgIndex;

		// Token: 0x04008C50 RID: 35920
		public int fgIndex;

		// Token: 0x04008C51 RID: 35921
		public Color32 fillColour;
	}
}
