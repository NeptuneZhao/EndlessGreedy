using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000C72 RID: 3186
public class KIconButtonMenu : KScreen
{
	// Token: 0x060061D5 RID: 25045 RVA: 0x00248784 File Offset: 0x00246984
	protected override void OnActivate()
	{
		base.OnActivate();
		this.RefreshButtons();
	}

	// Token: 0x060061D6 RID: 25046 RVA: 0x00248792 File Offset: 0x00246992
	public void SetButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x060061D7 RID: 25047 RVA: 0x002487AC File Offset: 0x002469AC
	public void RefreshButtonTooltip()
	{
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (buttonInfo.buttonGo == null || buttonInfo == null)
			{
				return;
			}
			ToolTip componentInChildren = buttonInfo.buttonGo.GetComponentInChildren<ToolTip>();
			if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = buttonInfo.GetTooltipText();
				LocText componentInChildren2 = buttonInfo.buttonGo.GetComponentInChildren<LocText>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.text = buttonInfo.text;
				}
			}
		}
	}

	// Token: 0x060061D8 RID: 25048 RVA: 0x00248850 File Offset: 0x00246A50
	public virtual void RefreshButtons()
	{
		if (this.buttonObjects != null)
		{
			for (int i = 0; i < this.buttonObjects.Length; i++)
			{
				UnityEngine.Object.Destroy(this.buttonObjects[i]);
			}
			this.buttonObjects = null;
		}
		if (this.buttons == null || this.buttons.Count == 0)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[j];
			if (buttonInfo != null)
			{
				GameObject binstance = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
				buttonInfo.buttonGo = binstance;
				this.buttonObjects[j] = binstance;
				Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
				binstance.transform.SetParent(parent, false);
				binstance.SetActive(true);
				binstance.name = buttonInfo.text + "Button";
				KButton component = binstance.GetComponent<KButton>();
				if (component != null && buttonInfo.onClick != null)
				{
					component.onClick += buttonInfo.onClick;
				}
				Image image = null;
				if (component)
				{
					image = component.fgImage;
				}
				if (image != null)
				{
					image.gameObject.SetActive(false);
					foreach (Sprite sprite in this.icons)
					{
						if (sprite != null && sprite.name == buttonInfo.iconName)
						{
							image.sprite = sprite;
							image.gameObject.SetActive(true);
							break;
						}
					}
				}
				if (buttonInfo.texture != null)
				{
					RawImage componentInChildren = binstance.GetComponentInChildren<RawImage>();
					if (componentInChildren != null)
					{
						componentInChildren.gameObject.SetActive(true);
						componentInChildren.texture = buttonInfo.texture;
					}
				}
				ToolTip componentInChildren2 = binstance.GetComponentInChildren<ToolTip>();
				if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren2 != null)
				{
					componentInChildren2.toolTip = buttonInfo.GetTooltipText();
					LocText componentInChildren3 = binstance.GetComponentInChildren<LocText>();
					if (componentInChildren3 != null)
					{
						componentInChildren3.text = buttonInfo.text;
					}
				}
				if (buttonInfo.onToolTip != null)
				{
					componentInChildren2.OnToolTip = buttonInfo.onToolTip;
				}
				KIconButtonMenu screen = this;
				System.Action onClick = buttonInfo.onClick;
				System.Action value = delegate()
				{
					onClick.Signal();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
					if (binstance != null)
					{
						KToggle component3 = binstance.GetComponent<KToggle>();
						if (component3 != null)
						{
							this.SelectToggle(component3);
						}
					}
				};
				KToggle componentInChildren4 = binstance.GetComponentInChildren<KToggle>();
				if (componentInChildren4 != null)
				{
					ToggleGroup component2 = base.GetComponent<ToggleGroup>();
					if (component2 == null)
					{
						component2 = this.externalToggleGroup;
					}
					componentInChildren4.group = component2;
					componentInChildren4.onClick += value;
					Navigation navigation = componentInChildren4.navigation;
					navigation.mode = (this.automaticNavigation ? Navigation.Mode.Automatic : Navigation.Mode.None);
					componentInChildren4.navigation = navigation;
				}
				else
				{
					KBasicToggle componentInChildren5 = binstance.GetComponentInChildren<KBasicToggle>();
					if (componentInChildren5 != null)
					{
						componentInChildren5.onClick += value;
					}
				}
				if (component != null)
				{
					component.isInteractable = buttonInfo.isInteractable;
				}
				buttonInfo.onCreate.Signal(buttonInfo);
			}
		}
		this.Update();
	}

	// Token: 0x060061D9 RID: 25049 RVA: 0x00248BC0 File Offset: 0x00246DC0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		if (!base.gameObject.activeSelf || !base.enabled)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060061DA RID: 25050 RVA: 0x00248C4F File Offset: 0x00246E4F
	protected override void OnPrefabInit()
	{
		base.Subscribe<KIconButtonMenu>(315865555, KIconButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x060061DB RID: 25051 RVA: 0x00248C62 File Offset: 0x00246E62
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x060061DC RID: 25052 RVA: 0x00248C78 File Offset: 0x00246E78
	private void Update()
	{
		if (!this.followGameObject || this.go == null || base.canvas == null)
		{
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(this.go.transform.GetPosition());
		RectTransform component = base.GetComponent<RectTransform>();
		RectTransform component2 = base.canvas.GetComponent<RectTransform>();
		if (component != null)
		{
			component.anchoredPosition = new Vector2(vector.x * component2.sizeDelta.x - component2.sizeDelta.x * 0.5f, vector.y * component2.sizeDelta.y - component2.sizeDelta.y * 0.5f);
		}
	}

	// Token: 0x060061DD RID: 25053 RVA: 0x00248D34 File Offset: 0x00246F34
	protected void SelectToggle(KToggle selectedToggle)
	{
		if (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.enabled)
		{
			return;
		}
		if (this.currentlySelectedToggle == selectedToggle)
		{
			this.currentlySelectedToggle = null;
		}
		else
		{
			this.currentlySelectedToggle = selectedToggle;
		}
		GameObject[] array = this.buttonObjects;
		for (int i = 0; i < array.Length; i++)
		{
			KToggle component = array[i].GetComponent<KToggle>();
			if (component != null)
			{
				if (component == this.currentlySelectedToggle)
				{
					component.Select();
					component.isOn = true;
				}
				else
				{
					component.Deselect();
					component.isOn = false;
				}
			}
		}
	}

	// Token: 0x060061DE RID: 25054 RVA: 0x00248DCC File Offset: 0x00246FCC
	public void ClearSelection()
	{
		foreach (GameObject gameObject in this.buttonObjects)
		{
			KToggle component = gameObject.GetComponent<KToggle>();
			if (component != null)
			{
				component.Deselect();
				component.isOn = false;
			}
			else
			{
				KBasicToggle component2 = gameObject.GetComponent<KBasicToggle>();
				if (component2 != null)
				{
					component2.isOn = false;
				}
			}
			ImageToggleState component3 = gameObject.GetComponent<ImageToggleState>();
			if (component3.GetIsActive())
			{
				component3.SetInactive();
			}
		}
		ToggleGroup component4 = base.GetComponent<ToggleGroup>();
		if (component4 != null)
		{
			component4.SetAllTogglesOff(true);
		}
		this.SelectToggle(null);
	}

	// Token: 0x04004256 RID: 16982
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x04004257 RID: 16983
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x04004258 RID: 16984
	[SerializeField]
	protected bool automaticNavigation = true;

	// Token: 0x04004259 RID: 16985
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x0400425A RID: 16986
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x0400425B RID: 16987
	[SerializeField]
	protected Sprite[] icons;

	// Token: 0x0400425C RID: 16988
	[SerializeField]
	private ToggleGroup externalToggleGroup;

	// Token: 0x0400425D RID: 16989
	protected KToggle currentlySelectedToggle;

	// Token: 0x0400425E RID: 16990
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x0400425F RID: 16991
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04004260 RID: 16992
	private UnityAction inputChangeReceiver;

	// Token: 0x04004261 RID: 16993
	protected GameObject go;

	// Token: 0x04004262 RID: 16994
	protected IList<KIconButtonMenu.ButtonInfo> buttons;

	// Token: 0x04004263 RID: 16995
	private static readonly global::EventSystem.IntraObjectHandler<KIconButtonMenu> OnSetActivatorDelegate = new global::EventSystem.IntraObjectHandler<KIconButtonMenu>(delegate(KIconButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001D56 RID: 7510
	public class ButtonInfo
	{
		// Token: 0x0600A855 RID: 43093 RVA: 0x0039C6F8 File Offset: 0x0039A8F8
		public ButtonInfo(string iconName = "", string text = "", System.Action on_click = null, global::Action shortcutKey = global::Action.NumActions, Action<GameObject> on_refresh = null, Action<KIconButtonMenu.ButtonInfo> on_create = null, Texture texture = null, string tooltipText = "", bool is_interactable = true)
		{
			this.iconName = iconName;
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = on_click;
			this.onCreate = on_create;
			this.texture = texture;
			this.tooltipText = tooltipText;
			this.isInteractable = is_interactable;
		}

		// Token: 0x0600A856 RID: 43094 RVA: 0x0039C748 File Offset: 0x0039A948
		public string GetTooltipText()
		{
			string text = (this.tooltipText == "") ? this.text : this.tooltipText;
			if (this.shortcutKey != global::Action.NumActions)
			{
				text = GameUtil.ReplaceHotkeyString(text, this.shortcutKey);
			}
			return text;
		}

		// Token: 0x040086FC RID: 34556
		public string iconName;

		// Token: 0x040086FD RID: 34557
		public string text;

		// Token: 0x040086FE RID: 34558
		public string tooltipText;

		// Token: 0x040086FF RID: 34559
		public string[] multiText;

		// Token: 0x04008700 RID: 34560
		public global::Action shortcutKey;

		// Token: 0x04008701 RID: 34561
		public bool isInteractable;

		// Token: 0x04008702 RID: 34562
		public Action<KIconButtonMenu.ButtonInfo> onCreate;

		// Token: 0x04008703 RID: 34563
		public System.Action onClick;

		// Token: 0x04008704 RID: 34564
		public Func<string> onToolTip;

		// Token: 0x04008705 RID: 34565
		public GameObject buttonGo;

		// Token: 0x04008706 RID: 34566
		public object userData;

		// Token: 0x04008707 RID: 34567
		public Texture texture;

		// Token: 0x02002656 RID: 9814
		// (Invoke) Token: 0x0600C20C RID: 49676
		public delegate void Callback();
	}
}
