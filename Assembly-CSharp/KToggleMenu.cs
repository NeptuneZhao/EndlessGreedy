using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C77 RID: 3191
public class KToggleMenu : KScreen
{
	// Token: 0x1400002A RID: 42
	// (add) Token: 0x0600620D RID: 25101 RVA: 0x0024991C File Offset: 0x00247B1C
	// (remove) Token: 0x0600620E RID: 25102 RVA: 0x00249954 File Offset: 0x00247B54
	public event KToggleMenu.OnSelect onSelect;

	// Token: 0x0600620F RID: 25103 RVA: 0x00249989 File Offset: 0x00247B89
	public void Setup(IList<KToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x06006210 RID: 25104 RVA: 0x00249998 File Offset: 0x00247B98
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x06006211 RID: 25105 RVA: 0x002499A0 File Offset: 0x00247BA0
	private void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				UnityEngine.Object.Destroy(ktoggle.gameObject);
			}
		}
		this.toggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform parent = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			if (toggleInfo == null)
			{
				this.toggles.Add(null);
			}
			else
			{
				KToggle ktoggle2 = UnityEngine.Object.Instantiate<KToggle>(this.prefab, Vector3.zero, Quaternion.identity);
				ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
				ktoggle2.transform.SetParent(parent, false);
				ktoggle2.group = this.group;
				ktoggle2.onClick += delegate()
				{
					this.OnClick(idx);
				};
				ktoggle2.GetComponentsInChildren<Text>(true)[0].text = toggleInfo.text;
				toggleInfo.toggle = ktoggle2;
				this.toggles.Add(ktoggle2);
			}
		}
	}

	// Token: 0x06006212 RID: 25106 RVA: 0x00249B18 File Offset: 0x00247D18
	public int GetSelected()
	{
		return KToggleMenu.selected;
	}

	// Token: 0x06006213 RID: 25107 RVA: 0x00249B1F File Offset: 0x00247D1F
	private void OnClick(int i)
	{
		UISounds.PlaySound(UISounds.Sound.ClickObject);
		if (this.onSelect == null)
		{
			return;
		}
		this.onSelect(this.toggleInfo[i]);
	}

	// Token: 0x06006214 RID: 25108 RVA: 0x00249B48 File Offset: 0x00247D48
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			global::Action hotKey = this.toggleInfo[i].hotKey;
			if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
			{
				this.toggles[i].Click();
				return;
			}
		}
	}

	// Token: 0x0400427C RID: 17020
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x0400427D RID: 17021
	[SerializeField]
	private KToggle prefab;

	// Token: 0x0400427E RID: 17022
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04004280 RID: 17024
	protected IList<KToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x04004281 RID: 17025
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x04004282 RID: 17026
	private static int selected = -1;

	// Token: 0x02001D5D RID: 7517
	// (Invoke) Token: 0x0600A869 RID: 43113
	public delegate void OnSelect(KToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001D5E RID: 7518
	public class ToggleInfo
	{
		// Token: 0x0600A86C RID: 43116 RVA: 0x0039C94A File Offset: 0x0039AB4A
		public ToggleInfo(string text, object user_data = null, global::Action hotKey = global::Action.NumActions)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotKey;
		}

		// Token: 0x0400871D RID: 34589
		public string text;

		// Token: 0x0400871E RID: 34590
		public object userData;

		// Token: 0x0400871F RID: 34591
		public KToggle toggle;

		// Token: 0x04008720 RID: 34592
		public global::Action hotKey;
	}
}
