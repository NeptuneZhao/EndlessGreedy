using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005F4 RID: 1524
public class UserMenu
{
	// Token: 0x060024FF RID: 9471 RVA: 0x000CEF2B File Offset: 0x000CD12B
	public void Refresh(GameObject go)
	{
		Game.Instance.Trigger(1980521255, go);
	}

	// Token: 0x06002500 RID: 9472 RVA: 0x000CEF40 File Offset: 0x000CD140
	public void AddButton(GameObject go, KIconButtonMenu.ButtonInfo button, float sort_order = 1f)
	{
		if (button.onClick != null)
		{
			System.Action callback = button.onClick;
			button.onClick = delegate()
			{
				callback();
				Game.Instance.Trigger(1980521255, go);
			};
		}
		this.buttons.Add(new KeyValuePair<KIconButtonMenu.ButtonInfo, float>(button, sort_order));
	}

	// Token: 0x06002501 RID: 9473 RVA: 0x000CEF92 File Offset: 0x000CD192
	public void AddSlider(GameObject go, UserMenu.SliderInfo slider)
	{
		this.sliders.Add(slider);
	}

	// Token: 0x06002502 RID: 9474 RVA: 0x000CEFA0 File Offset: 0x000CD1A0
	public void AppendToScreen(GameObject go, UserMenuScreen screen)
	{
		this.buttons.Clear();
		this.sliders.Clear();
		go.Trigger(493375141, null);
		if (this.buttons.Count > 0)
		{
			this.buttons.Sort(delegate(KeyValuePair<KIconButtonMenu.ButtonInfo, float> x, KeyValuePair<KIconButtonMenu.ButtonInfo, float> y)
			{
				if (x.Value == y.Value)
				{
					return 0;
				}
				if (x.Value > y.Value)
				{
					return 1;
				}
				return -1;
			});
			for (int i = 0; i < this.buttons.Count; i++)
			{
				this.sortedButtons.Add(this.buttons[i].Key);
			}
			screen.AddButtons(this.sortedButtons);
			this.sortedButtons.Clear();
		}
		if (this.sliders.Count > 0)
		{
			screen.AddSliders(this.sliders);
		}
	}

	// Token: 0x040014F0 RID: 5360
	public const float DECONSTRUCT_PRIORITY = 0f;

	// Token: 0x040014F1 RID: 5361
	public const float DRAWPATHS_PRIORITY = 0.1f;

	// Token: 0x040014F2 RID: 5362
	public const float FOLLOWCAM_PRIORITY = 0.3f;

	// Token: 0x040014F3 RID: 5363
	public const float SETDIRECTION_PRIORITY = 0.4f;

	// Token: 0x040014F4 RID: 5364
	public const float AUTOBOTTLE_PRIORITY = 0.4f;

	// Token: 0x040014F5 RID: 5365
	public const float AUTOREPAIR_PRIORITY = 0.5f;

	// Token: 0x040014F6 RID: 5366
	public const float DEFAULT_PRIORITY = 1f;

	// Token: 0x040014F7 RID: 5367
	public const float SUITEQUIP_PRIORITY = 2f;

	// Token: 0x040014F8 RID: 5368
	public const float AUTODISINFECT_PRIORITY = 10f;

	// Token: 0x040014F9 RID: 5369
	public const float ROCKETUSAGERESTRICTION_PRIORITY = 11f;

	// Token: 0x040014FA RID: 5370
	private List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>> buttons = new List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>>();

	// Token: 0x040014FB RID: 5371
	private List<UserMenu.SliderInfo> sliders = new List<UserMenu.SliderInfo>();

	// Token: 0x040014FC RID: 5372
	private List<KIconButtonMenu.ButtonInfo> sortedButtons = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x020013DA RID: 5082
	public class SliderInfo
	{
		// Token: 0x04006826 RID: 26662
		public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

		// Token: 0x04006827 RID: 26663
		public MinMaxSlider.Mode mode;

		// Token: 0x04006828 RID: 26664
		public Slider.Direction direction;

		// Token: 0x04006829 RID: 26665
		public bool interactable = true;

		// Token: 0x0400682A RID: 26666
		public bool lockRange;

		// Token: 0x0400682B RID: 26667
		public string toolTip;

		// Token: 0x0400682C RID: 26668
		public string toolTipMin;

		// Token: 0x0400682D RID: 26669
		public string toolTipMax;

		// Token: 0x0400682E RID: 26670
		public float minLimit;

		// Token: 0x0400682F RID: 26671
		public float maxLimit = 100f;

		// Token: 0x04006830 RID: 26672
		public float currentMinValue = 10f;

		// Token: 0x04006831 RID: 26673
		public float currentMaxValue = 90f;

		// Token: 0x04006832 RID: 26674
		public GameObject sliderGO;

		// Token: 0x04006833 RID: 26675
		public Action<MinMaxSlider> onMinChange;

		// Token: 0x04006834 RID: 26676
		public Action<MinMaxSlider> onMaxChange;
	}
}
