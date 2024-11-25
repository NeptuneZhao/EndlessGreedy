using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C73 RID: 3187
public class KIconToggleMenu : KScreen
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x060061E1 RID: 25057 RVA: 0x00248E90 File Offset: 0x00247090
	// (remove) Token: 0x060061E2 RID: 25058 RVA: 0x00248EC8 File Offset: 0x002470C8
	public event KIconToggleMenu.OnSelect onSelect;

	// Token: 0x060061E3 RID: 25059 RVA: 0x00248EFD File Offset: 0x002470FD
	public void Setup(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x060061E4 RID: 25060 RVA: 0x00248F0C File Offset: 0x0024710C
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x060061E5 RID: 25061 RVA: 0x00248F14 File Offset: 0x00247114
	protected virtual void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				if (!this.dontDestroyToggles.Contains(ktoggle))
				{
					UnityEngine.Object.Destroy(ktoggle.gameObject);
				}
				else
				{
					ktoggle.ClearOnClick();
				}
			}
		}
		this.toggles.Clear();
		this.dontDestroyToggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform transform = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KIconToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			KToggle ktoggle2;
			if (toggleInfo.instanceOverride != null)
			{
				ktoggle2 = toggleInfo.instanceOverride;
				this.dontDestroyToggles.Add(ktoggle2);
			}
			else if (toggleInfo.prefabOverride)
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(toggleInfo.prefabOverride.gameObject, transform.gameObject, true);
			}
			else
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(this.prefab.gameObject, transform.gameObject, true);
			}
			ktoggle2.Deselect();
			ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
			ktoggle2.group = this.group;
			ktoggle2.onClick += delegate()
			{
				this.OnClick(idx);
			};
			LocText componentInChildren = ktoggle2.transform.GetComponentInChildren<LocText>();
			if (componentInChildren != null)
			{
				componentInChildren.SetText(toggleInfo.text);
			}
			if (toggleInfo.getSpriteCB != null)
			{
				ktoggle2.fgImage.sprite = toggleInfo.getSpriteCB();
			}
			else if (toggleInfo.icon != null)
			{
				ktoggle2.fgImage.sprite = Assets.GetSprite(toggleInfo.icon);
			}
			toggleInfo.SetToggle(ktoggle2);
			this.toggles.Add(ktoggle2);
		}
	}

	// Token: 0x060061E6 RID: 25062 RVA: 0x0024913C File Offset: 0x0024733C
	public Sprite GetIcon(string name)
	{
		foreach (Sprite sprite in this.icons)
		{
			if (sprite.name == name)
			{
				return sprite;
			}
		}
		return null;
	}

	// Token: 0x060061E7 RID: 25063 RVA: 0x00249174 File Offset: 0x00247374
	public virtual void ClearSelection()
	{
		if (this.toggles == null)
		{
			return;
		}
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.Deselect();
			ktoggle.ClearAnimState();
		}
		this.selected = -1;
	}

	// Token: 0x060061E8 RID: 25064 RVA: 0x002491DC File Offset: 0x002473DC
	private void OnClick(int i)
	{
		if (this.onSelect == null)
		{
			return;
		}
		this.selected = i;
		this.onSelect(this.toggleInfo[i]);
		if (!this.toggles[i].isOn)
		{
			this.selected = -1;
		}
		for (int j = 0; j < this.toggles.Count; j++)
		{
			if (j != this.selected)
			{
				this.toggles[j].isOn = false;
			}
		}
	}

	// Token: 0x060061E9 RID: 25065 RVA: 0x0024925C File Offset: 0x0024745C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		if (this.toggleInfo == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			if (this.toggles[i].isActiveAndEnabled)
			{
				global::Action hotKey = this.toggleInfo[i].hotKey;
				if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
				{
					if (this.selected != i || this.repeatKeyDownToggles)
					{
						this.toggles[i].Click();
						if (this.selected == i)
						{
							this.toggles[i].Deselect();
						}
						this.selected = i;
						return;
					}
					break;
				}
			}
		}
	}

	// Token: 0x060061EA RID: 25066 RVA: 0x0024930E File Offset: 0x0024750E
	public virtual void Close()
	{
		this.ClearSelection();
		this.Show(false);
	}

	// Token: 0x04004264 RID: 16996
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x04004265 RID: 16997
	[SerializeField]
	private KToggle prefab;

	// Token: 0x04004266 RID: 16998
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04004267 RID: 16999
	[SerializeField]
	private Sprite[] icons;

	// Token: 0x04004268 RID: 17000
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04004269 RID: 17001
	[SerializeField]
	public TextStyleSetting ToggleToolTipHeaderTextStyleSetting;

	// Token: 0x0400426A RID: 17002
	[SerializeField]
	protected bool repeatKeyDownToggles = true;

	// Token: 0x0400426B RID: 17003
	protected KToggle currentlySelectedToggle;

	// Token: 0x0400426D RID: 17005
	protected IList<KIconToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x0400426E RID: 17006
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x0400426F RID: 17007
	private List<KToggle> dontDestroyToggles = new List<KToggle>();

	// Token: 0x04004270 RID: 17008
	protected int selected = -1;

	// Token: 0x02001D59 RID: 7513
	// (Invoke) Token: 0x0600A85D RID: 43101
	public delegate void OnSelect(KIconToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001D5A RID: 7514
	public class ToggleInfo
	{
		// Token: 0x0600A860 RID: 43104 RVA: 0x0039C828 File Offset: 0x0039AA28
		public ToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "")
		{
			this.text = text;
			this.userData = user_data;
			this.icon = icon;
			this.hotKey = hotkey;
			this.tooltip = tooltip;
			this.tooltipHeader = tooltip_header;
			this.getTooltipText = new ToolTip.ComplexTooltipDelegate(this.DefaultGetTooltipText);
		}

		// Token: 0x0600A861 RID: 43105 RVA: 0x0039C87B File Offset: 0x0039AA7B
		public ToggleInfo(string text, object user_data, global::Action hotkey, Func<Sprite> get_sprite_cb)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotkey;
			this.getSpriteCB = get_sprite_cb;
		}

		// Token: 0x0600A862 RID: 43106 RVA: 0x0039C8A0 File Offset: 0x0039AAA0
		public virtual void SetToggle(KToggle toggle)
		{
			this.toggle = toggle;
			toggle.GetComponent<ToolTip>().OnComplexToolTip = this.getTooltipText;
		}

		// Token: 0x0600A863 RID: 43107 RVA: 0x0039C8BC File Offset: 0x0039AABC
		protected virtual List<global::Tuple<string, TextStyleSetting>> DefaultGetTooltipText()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (this.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		}

		// Token: 0x0400870D RID: 34573
		public string text;

		// Token: 0x0400870E RID: 34574
		public object userData;

		// Token: 0x0400870F RID: 34575
		public string icon;

		// Token: 0x04008710 RID: 34576
		public string tooltip;

		// Token: 0x04008711 RID: 34577
		public string tooltipHeader;

		// Token: 0x04008712 RID: 34578
		public KToggle toggle;

		// Token: 0x04008713 RID: 34579
		public global::Action hotKey;

		// Token: 0x04008714 RID: 34580
		public ToolTip.ComplexTooltipDelegate getTooltipText;

		// Token: 0x04008715 RID: 34581
		public Func<Sprite> getSpriteCB;

		// Token: 0x04008716 RID: 34582
		public KToggle prefabOverride;

		// Token: 0x04008717 RID: 34583
		public KToggle instanceOverride;
	}
}
