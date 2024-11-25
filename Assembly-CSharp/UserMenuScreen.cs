using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000BB9 RID: 3001
public class UserMenuScreen : KIconButtonMenu
{
	// Token: 0x06005B33 RID: 23347 RVA: 0x00213954 File Offset: 0x00211B54
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
		this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
		this.buttonParent.transform.SetAsLastSibling();
	}

	// Token: 0x06005B34 RID: 23348 RVA: 0x002139B3 File Offset: 0x00211BB3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(1980521255, new Action<object>(this.OnUIRefresh));
		KInputManager.InputChange.AddListener(new UnityAction(base.RefreshButtonTooltip));
	}

	// Token: 0x06005B35 RID: 23349 RVA: 0x002139ED File Offset: 0x00211BED
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(base.RefreshButtonTooltip));
		base.OnForcedCleanUp();
	}

	// Token: 0x06005B36 RID: 23350 RVA: 0x00213A0B File Offset: 0x00211C0B
	public void SetSelected(GameObject go)
	{
		this.ClearPrioritizable();
		this.selected = go;
		this.RefreshPrioritizable();
	}

	// Token: 0x06005B37 RID: 23351 RVA: 0x00213A20 File Offset: 0x00211C20
	private void ClearPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Remove(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
			}
		}
	}

	// Token: 0x06005B38 RID: 23352 RVA: 0x00213A74 File Offset: 0x00211C74
	private void RefreshPrioritizable()
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null && component.IsPrioritizable())
			{
				Prioritizable prioritizable = component;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.OnPriorityChanged));
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.SetScreenPriority(component.GetMasterPriority(), false);
				return;
			}
			this.priorityScreen.gameObject.SetActive(false);
		}
	}

	// Token: 0x06005B39 RID: 23353 RVA: 0x00213B04 File Offset: 0x00211D04
	public void Refresh(GameObject go)
	{
		if (go != this.selected)
		{
			return;
		}
		this.buttonInfos.Clear();
		this.slidersInfos.Clear();
		Game.Instance.userMenu.AppendToScreen(go, this);
		base.SetButtons(this.buttonInfos);
		base.RefreshButtons();
		this.RefreshSliders();
		this.ClearPrioritizable();
		this.RefreshPrioritizable();
		if ((this.sliders == null || this.sliders.Count == 0) && (this.buttonInfos == null || this.buttonInfos.Count == 0) && !this.priorityScreen.gameObject.activeSelf)
		{
			base.transform.parent.gameObject.SetActive(false);
			return;
		}
		base.transform.parent.gameObject.SetActive(true);
	}

	// Token: 0x06005B3A RID: 23354 RVA: 0x00213BD4 File Offset: 0x00211DD4
	public void AddSliders(IList<UserMenu.SliderInfo> sliders)
	{
		this.slidersInfos.AddRange(sliders);
	}

	// Token: 0x06005B3B RID: 23355 RVA: 0x00213BE2 File Offset: 0x00211DE2
	public void AddButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttonInfos.AddRange(buttons);
	}

	// Token: 0x06005B3C RID: 23356 RVA: 0x00213BF0 File Offset: 0x00211DF0
	private void OnUIRefresh(object data)
	{
		this.Refresh(data as GameObject);
	}

	// Token: 0x06005B3D RID: 23357 RVA: 0x00213C00 File Offset: 0x00211E00
	public void RefreshSliders()
	{
		if (this.sliders != null)
		{
			for (int i = 0; i < this.sliders.Count; i++)
			{
				UnityEngine.Object.Destroy(this.sliders[i].gameObject);
			}
			this.sliders = null;
		}
		if (this.slidersInfos == null || this.slidersInfos.Count == 0)
		{
			return;
		}
		this.sliders = new List<MinMaxSlider>();
		for (int j = 0; j < this.slidersInfos.Count; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.sliderPrefab.gameObject, Vector3.zero, Quaternion.identity);
			this.slidersInfos[j].sliderGO = gameObject;
			MinMaxSlider component = gameObject.GetComponent<MinMaxSlider>();
			this.sliders.Add(component);
			Transform parent = (this.sliderParent != null) ? this.sliderParent.transform : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = "Slider";
			if (component.toolTip)
			{
				component.toolTip.toolTip = this.slidersInfos[j].toolTip;
			}
			component.lockType = this.slidersInfos[j].lockType;
			component.interactable = this.slidersInfos[j].interactable;
			component.minLimit = this.slidersInfos[j].minLimit;
			component.maxLimit = this.slidersInfos[j].maxLimit;
			component.currentMinValue = this.slidersInfos[j].currentMinValue;
			component.currentMaxValue = this.slidersInfos[j].currentMaxValue;
			component.onMinChange = this.slidersInfos[j].onMinChange;
			component.onMaxChange = this.slidersInfos[j].onMaxChange;
			component.direction = this.slidersInfos[j].direction;
			component.SetMode(this.slidersInfos[j].mode);
			component.SetMinMaxValue(this.slidersInfos[j].currentMinValue, this.slidersInfos[j].currentMaxValue, this.slidersInfos[j].minLimit, this.slidersInfos[j].maxLimit);
		}
	}

	// Token: 0x06005B3E RID: 23358 RVA: 0x00213E64 File Offset: 0x00212064
	private void OnPriorityClicked(PrioritySetting priority)
	{
		if (this.selected != null)
		{
			Prioritizable component = this.selected.GetComponent<Prioritizable>();
			if (component != null)
			{
				component.SetMasterPriority(priority);
			}
		}
	}

	// Token: 0x06005B3F RID: 23359 RVA: 0x00213E9B File Offset: 0x0021209B
	private void OnPriorityChanged(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x04003C1D RID: 15389
	private GameObject selected;

	// Token: 0x04003C1E RID: 15390
	public MinMaxSlider sliderPrefab;

	// Token: 0x04003C1F RID: 15391
	public GameObject sliderParent;

	// Token: 0x04003C20 RID: 15392
	public PriorityScreen priorityScreenPrefab;

	// Token: 0x04003C21 RID: 15393
	public GameObject priorityScreenParent;

	// Token: 0x04003C22 RID: 15394
	private List<MinMaxSlider> sliders = new List<MinMaxSlider>();

	// Token: 0x04003C23 RID: 15395
	private List<UserMenu.SliderInfo> slidersInfos = new List<UserMenu.SliderInfo>();

	// Token: 0x04003C24 RID: 15396
	private List<KIconButtonMenu.ButtonInfo> buttonInfos = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x04003C25 RID: 15397
	private PriorityScreen priorityScreen;
}
