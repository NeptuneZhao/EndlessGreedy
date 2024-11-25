using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C9A RID: 3226
public class LockerNavigator : KModalScreen
{
	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x06006343 RID: 25411 RVA: 0x0024F992 File Offset: 0x0024DB92
	public GameObject ContentSlot
	{
		get
		{
			return this.slot.gameObject;
		}
	}

	// Token: 0x06006344 RID: 25412 RVA: 0x0024F99F File Offset: 0x0024DB9F
	protected override void OnActivate()
	{
		LockerNavigator.Instance = this;
		this.Show(false);
		this.backButton.onClick += this.OnClickBack;
	}

	// Token: 0x06006345 RID: 25413 RVA: 0x0024F9C5 File Offset: 0x0024DBC5
	public override float GetSortKey()
	{
		return 41f;
	}

	// Token: 0x06006346 RID: 25414 RVA: 0x0024F9CC File Offset: 0x0024DBCC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.PopScreen();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006347 RID: 25415 RVA: 0x0024F9EE File Offset: 0x0024DBEE
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (!show)
		{
			this.PopAllScreens();
		}
		StreamedTextures.SetBundlesLoaded(show);
	}

	// Token: 0x06006348 RID: 25416 RVA: 0x0024FA06 File Offset: 0x0024DC06
	private void OnClickBack()
	{
		this.PopScreen();
	}

	// Token: 0x06006349 RID: 25417 RVA: 0x0024FA10 File Offset: 0x0024DC10
	public void PushScreen(GameObject screen, System.Action onClose = null)
	{
		if (screen == null)
		{
			return;
		}
		if (this.navigationHistory.Count == 0)
		{
			this.Show(true);
			if (!LockerNavigator.didDisplayDataCollectionWarningPopupOnce && KPrivacyPrefs.instance.disableDataCollection)
			{
				LockerNavigator.MakeDataCollectionWarningPopup(base.gameObject.transform.parent.gameObject);
				LockerNavigator.didDisplayDataCollectionWarningPopupOnce = true;
			}
		}
		if (this.navigationHistory.Count > 0 && screen == this.navigationHistory[this.navigationHistory.Count - 1].screen)
		{
			return;
		}
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(false);
		}
		this.navigationHistory.Add(new LockerNavigator.HistoryEntry(screen, onClose));
		this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.RefreshButtons();
	}

	// Token: 0x0600634A RID: 25418 RVA: 0x0024FB28 File Offset: 0x0024DD28
	public bool PopScreen()
	{
		while (this.preventScreenPop.Count > 0)
		{
			int index = this.preventScreenPop.Count - 1;
			Func<bool> func = this.preventScreenPop[index];
			this.preventScreenPop.RemoveAt(index);
			if (func())
			{
				return true;
			}
		}
		int index2 = this.navigationHistory.Count - 1;
		LockerNavigator.HistoryEntry historyEntry = this.navigationHistory[index2];
		historyEntry.screen.SetActive(false);
		if (historyEntry.onClose.IsSome())
		{
			historyEntry.onClose.Unwrap()();
		}
		this.navigationHistory.RemoveAt(index2);
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
			this.RefreshButtons();
			return true;
		}
		this.Show(false);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "initial", true);
		return false;
	}

	// Token: 0x0600634B RID: 25419 RVA: 0x0024FC24 File Offset: 0x0024DE24
	public void PopAllScreens()
	{
		if (this.navigationHistory.Count == 0 && this.preventScreenPop.Count == 0)
		{
			return;
		}
		int num = 0;
		while (this.PopScreen())
		{
			if (num > 100)
			{
				DebugUtil.DevAssert(false, string.Format("Can't close all LockerNavigator screens, hit limit of trying to close {0} screens", 100), null);
				return;
			}
			num++;
		}
	}

	// Token: 0x0600634C RID: 25420 RVA: 0x0024FC7A File Offset: 0x0024DE7A
	private void RefreshButtons()
	{
		this.backButton.isInteractable = true;
	}

	// Token: 0x0600634D RID: 25421 RVA: 0x0024FC88 File Offset: 0x0024DE88
	public void ShowDialogPopup(Action<InfoDialogScreen> configureDialogFn)
	{
		InfoDialogScreen dialog = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.ContentSlot, false);
		configureDialogFn(dialog);
		dialog.Activate();
		dialog.gameObject.AddOrGet<LayoutElement>().ignoreLayout = true;
		dialog.gameObject.AddOrGet<RectTransform>().Fill();
		Func<bool> preventScreenPopFn = delegate()
		{
			dialog.Deactivate();
			return true;
		};
		this.preventScreenPop.Add(preventScreenPopFn);
		InfoDialogScreen dialog2 = dialog;
		dialog2.onDeactivateFn = (System.Action)Delegate.Combine(dialog2.onDeactivateFn, new System.Action(delegate()
		{
			this.preventScreenPop.Remove(preventScreenPopFn);
		}));
	}

	// Token: 0x0600634E RID: 25422 RVA: 0x0024FD50 File Offset: 0x0024DF50
	public static void MakeDataCollectionWarningPopup(GameObject fullscreenParent)
	{
		Action<InfoDialogScreen> <>9__2;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.HEADER).AddPlainText(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BODY).AddOption(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OK, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, true);
			string text = UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OPEN_SETTINGS;
			Action<InfoDialogScreen> action;
			if ((action = <>9__2) == null)
			{
				action = (<>9__2 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					LockerNavigator.Instance.PopAllScreens();
					LockerMenuScreen.Instance.Show(false);
					Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, fullscreenParent, true).ShowMetricsScreen();
				});
			}
			infoDialogScreen.AddOption(text, action, false);
		});
	}

	// Token: 0x04004353 RID: 17235
	public static LockerNavigator Instance;

	// Token: 0x04004354 RID: 17236
	[SerializeField]
	private RectTransform slot;

	// Token: 0x04004355 RID: 17237
	[SerializeField]
	private KButton backButton;

	// Token: 0x04004356 RID: 17238
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004357 RID: 17239
	[SerializeField]
	public GameObject kleiInventoryScreen;

	// Token: 0x04004358 RID: 17240
	[SerializeField]
	public GameObject duplicantCatalogueScreen;

	// Token: 0x04004359 RID: 17241
	[SerializeField]
	public GameObject outfitDesignerScreen;

	// Token: 0x0400435A RID: 17242
	[SerializeField]
	public GameObject outfitBrowserScreen;

	// Token: 0x0400435B RID: 17243
	[SerializeField]
	public GameObject joyResponseDesignerScreen;

	// Token: 0x0400435C RID: 17244
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x0400435D RID: 17245
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x0400435E RID: 17246
	private List<LockerNavigator.HistoryEntry> navigationHistory = new List<LockerNavigator.HistoryEntry>();

	// Token: 0x0400435F RID: 17247
	private Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();

	// Token: 0x04004360 RID: 17248
	private static bool didDisplayDataCollectionWarningPopupOnce;

	// Token: 0x04004361 RID: 17249
	public List<Func<bool>> preventScreenPop = new List<Func<bool>>();

	// Token: 0x02001D81 RID: 7553
	public readonly struct HistoryEntry
	{
		// Token: 0x0600A8DD RID: 43229 RVA: 0x0039E342 File Offset: 0x0039C542
		public HistoryEntry(GameObject screen, System.Action onClose = null)
		{
			this.screen = screen;
			this.onClose = onClose;
		}

		// Token: 0x04008784 RID: 34692
		public readonly GameObject screen;

		// Token: 0x04008785 RID: 34693
		public readonly Option<System.Action> onClose;
	}
}
