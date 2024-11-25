using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D3E RID: 3390
public class SideDetailsScreen : KScreen
{
	// Token: 0x06006A89 RID: 27273 RVA: 0x002822F5 File Offset: 0x002804F5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SideDetailsScreen.Instance = this;
		this.Initialize();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06006A8A RID: 27274 RVA: 0x00282315 File Offset: 0x00280515
	protected override void OnForcedCleanUp()
	{
		SideDetailsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06006A8B RID: 27275 RVA: 0x00282324 File Offset: 0x00280524
	private void Initialize()
	{
		if (this.screens == null)
		{
			return;
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.screenMap = new Dictionary<string, SideTargetScreen>();
		List<SideTargetScreen> list = new List<SideTargetScreen>();
		foreach (SideTargetScreen sideTargetScreen in this.screens)
		{
			SideTargetScreen sideTargetScreen2 = Util.KInstantiateUI<SideTargetScreen>(sideTargetScreen.gameObject, this.body.gameObject, false);
			sideTargetScreen2.gameObject.SetActive(false);
			list.Add(sideTargetScreen2);
		}
		list.ForEach(delegate(SideTargetScreen s)
		{
			this.screenMap.Add(s.name, s);
		});
		this.backButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x06006A8C RID: 27276 RVA: 0x002823E8 File Offset: 0x002805E8
	public void SetTitle(string newTitle)
	{
		this.title.text = newTitle;
	}

	// Token: 0x06006A8D RID: 27277 RVA: 0x002823F8 File Offset: 0x002805F8
	public void SetScreen(string screenName, object content, float x)
	{
		if (!this.screenMap.ContainsKey(screenName))
		{
			global::Debug.LogError("Tried to open a screen that does exist on the manager!");
			return;
		}
		if (content == null)
		{
			global::Debug.LogError("Tried to set " + screenName + " with null content!");
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		Rect rect = this.rectTransform.rect;
		this.rectTransform.offsetMin = new Vector2(x, this.rectTransform.offsetMin.y);
		this.rectTransform.offsetMax = new Vector2(x + rect.width, this.rectTransform.offsetMax.y);
		if (this.activeScreen != null)
		{
			this.activeScreen.gameObject.SetActive(false);
		}
		this.activeScreen = this.screenMap[screenName];
		this.activeScreen.gameObject.SetActive(true);
		this.SetTitle(this.activeScreen.displayName);
		this.activeScreen.SetTarget(content);
	}

	// Token: 0x0400489F RID: 18591
	[SerializeField]
	private List<SideTargetScreen> screens;

	// Token: 0x040048A0 RID: 18592
	[SerializeField]
	private LocText title;

	// Token: 0x040048A1 RID: 18593
	[SerializeField]
	private KButton backButton;

	// Token: 0x040048A2 RID: 18594
	[SerializeField]
	private RectTransform body;

	// Token: 0x040048A3 RID: 18595
	private RectTransform rectTransform;

	// Token: 0x040048A4 RID: 18596
	private Dictionary<string, SideTargetScreen> screenMap;

	// Token: 0x040048A5 RID: 18597
	private SideTargetScreen activeScreen;

	// Token: 0x040048A6 RID: 18598
	public static SideDetailsScreen Instance;
}
