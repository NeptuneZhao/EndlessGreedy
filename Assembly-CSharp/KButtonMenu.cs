using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C70 RID: 3184
public class KButtonMenu : KScreen
{
	// Token: 0x060061C2 RID: 25026 RVA: 0x00248029 File Offset: 0x00246229
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = this.ShouldConsumeMouseScroll;
		this.RefreshButtons();
	}

	// Token: 0x060061C3 RID: 25027 RVA: 0x0024803D File Offset: 0x0024623D
	public void SetButtons(IList<KButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x060061C4 RID: 25028 RVA: 0x00248054 File Offset: 0x00246254
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
		if (this.buttons == null)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KButtonMenu.ButtonInfo binfo = this.buttons[j];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
			this.buttonObjects[j] = gameObject;
			Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = binfo.text + "Button";
			LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>(true);
			if (componentsInChildren != null)
			{
				foreach (LocText locText in componentsInChildren)
				{
					locText.text = ((locText.name == "Hotkey") ? GameUtil.GetActionString(binfo.shortcutKey) : binfo.text);
					locText.color = (binfo.isEnabled ? new Color(1f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f));
				}
			}
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (binfo.toolTip != null && binfo.toolTip != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = binfo.toolTip;
			}
			KButtonMenu screen = this;
			KButton button = gameObject.GetComponent<KButton>();
			button.isInteractable = binfo.isEnabled;
			if (binfo.popupOptions == null && binfo.onPopulatePopup == null)
			{
				UnityAction onClick = binfo.onClick;
				System.Action value = delegate()
				{
					onClick();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
				};
				button.onClick += value;
			}
			else
			{
				button.onClick += delegate()
				{
					this.SetupPopupMenu(binfo, button);
				};
			}
			binfo.uibutton = button;
			KButtonMenu.ButtonInfo.HoverCallback onHover = binfo.onHover;
		}
		this.Update();
	}

	// Token: 0x060061C5 RID: 25029 RVA: 0x00248308 File Offset: 0x00246508
	protected Button.ButtonClickedEvent SetupPopupMenu(KButtonMenu.ButtonInfo binfo, KButton button)
	{
		Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
		UnityAction unityAction = delegate()
		{
			List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
			if (binfo.onPopulatePopup != null)
			{
				binfo.popupOptions = binfo.onPopulatePopup();
			}
			string[] popupOptions = binfo.popupOptions;
			for (int i = 0; i < popupOptions.Length; i++)
			{
				string delegate_str2 = popupOptions[i];
				string delegate_str = delegate_str2;
				list.Add(new KButtonMenu.ButtonInfo(delegate_str, delegate()
				{
					binfo.onPopupClick(delegate_str);
					if (!this.keepMenuOpen)
					{
						this.Deactivate();
					}
				}, global::Action.NumActions, null, null, null, true, null, null, null));
			}
			KButtonMenu component = Util.KInstantiate(ScreenPrefabs.Instance.ButtonGrid.gameObject, null, null).GetComponent<KButtonMenu>();
			component.SetButtons(list.ToArray());
			RootMenu.Instance.AddSubMenu(component);
			Game.Instance.LocalPlayer.ScreenManager.ActivateScreen(component.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			Vector3 b = default(Vector3);
			if (Util.IsOnLeftSideOfScreen(button.transform.GetPosition()))
			{
				b.x = button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			else
			{
				b.x = -button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			component.transform.SetPosition(button.transform.GetPosition() + b);
		};
		binfo.onClick = unityAction;
		buttonClickedEvent.AddListener(unityAction);
		return buttonClickedEvent;
	}

	// Token: 0x060061C6 RID: 25030 RVA: 0x00248358 File Offset: 0x00246558
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060061C7 RID: 25031 RVA: 0x002483D1 File Offset: 0x002465D1
	protected override void OnPrefabInit()
	{
		base.Subscribe<KButtonMenu>(315865555, KButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x060061C8 RID: 25032 RVA: 0x002483E4 File Offset: 0x002465E4
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x060061C9 RID: 25033 RVA: 0x002483F8 File Offset: 0x002465F8
	protected override void OnDeactivate()
	{
	}

	// Token: 0x060061CA RID: 25034 RVA: 0x002483FC File Offset: 0x002465FC
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

	// Token: 0x04004249 RID: 16969
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x0400424A RID: 16970
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x0400424B RID: 16971
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x0400424C RID: 16972
	public GameObject buttonPrefab;

	// Token: 0x0400424D RID: 16973
	public bool ShouldConsumeMouseScroll;

	// Token: 0x0400424E RID: 16974
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x0400424F RID: 16975
	protected GameObject go;

	// Token: 0x04004250 RID: 16976
	protected IList<KButtonMenu.ButtonInfo> buttons;

	// Token: 0x04004251 RID: 16977
	private static readonly EventSystem.IntraObjectHandler<KButtonMenu> OnSetActivatorDelegate = new EventSystem.IntraObjectHandler<KButtonMenu>(delegate(KButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001D4F RID: 7503
	public class ButtonInfo
	{
		// Token: 0x0600A846 RID: 43078 RVA: 0x0039C36C File Offset: 0x0039A56C
		public ButtonInfo(string text = null, UnityAction on_click = null, global::Action shortcut_key = global::Action.NumActions, KButtonMenu.ButtonInfo.HoverCallback on_hover = null, string tool_tip = null, GameObject visualizer = null, bool is_enabled = true, string[] popup_options = null, Action<string> on_popup_click = null, Func<string[]> on_populate_popup = null)
		{
			this.text = text;
			this.shortcutKey = shortcut_key;
			this.onClick = on_click;
			this.onHover = on_hover;
			this.visualizer = visualizer;
			this.toolTip = tool_tip;
			this.isEnabled = is_enabled;
			this.uibutton = null;
			this.popupOptions = popup_options;
			this.onPopupClick = on_popup_click;
			this.onPopulatePopup = on_populate_popup;
		}

		// Token: 0x0600A847 RID: 43079 RVA: 0x0039C3DC File Offset: 0x0039A5DC
		public ButtonInfo(string text, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.userData = userData;
			this.visualizer = null;
			this.uibutton = null;
		}

		// Token: 0x0600A848 RID: 43080 RVA: 0x0039C42C File Offset: 0x0039A62C
		public ButtonInfo(string text, GameObject visualizer, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.visualizer = visualizer;
			this.userData = userData;
			this.uibutton = null;
		}

		// Token: 0x040086E1 RID: 34529
		public string text;

		// Token: 0x040086E2 RID: 34530
		public global::Action shortcutKey;

		// Token: 0x040086E3 RID: 34531
		public GameObject visualizer;

		// Token: 0x040086E4 RID: 34532
		public UnityAction onClick;

		// Token: 0x040086E5 RID: 34533
		public KButtonMenu.ButtonInfo.HoverCallback onHover;

		// Token: 0x040086E6 RID: 34534
		public FMODAsset clickSound;

		// Token: 0x040086E7 RID: 34535
		public KButton uibutton;

		// Token: 0x040086E8 RID: 34536
		public string toolTip;

		// Token: 0x040086E9 RID: 34537
		public bool isEnabled = true;

		// Token: 0x040086EA RID: 34538
		public string[] popupOptions;

		// Token: 0x040086EB RID: 34539
		public Action<string> onPopupClick;

		// Token: 0x040086EC RID: 34540
		public Func<string[]> onPopulatePopup;

		// Token: 0x040086ED RID: 34541
		public object userData;

		// Token: 0x02002654 RID: 9812
		// (Invoke) Token: 0x0600C204 RID: 49668
		public delegate void HoverCallback(GameObject hoverTarget);

		// Token: 0x02002655 RID: 9813
		// (Invoke) Token: 0x0600C208 RID: 49672
		public delegate void Callback();
	}
}
