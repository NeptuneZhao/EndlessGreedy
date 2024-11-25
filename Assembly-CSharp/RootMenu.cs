using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000BB0 RID: 2992
public class RootMenu : KScreen
{
	// Token: 0x06005A9E RID: 23198 RVA: 0x0020D9DA File Offset: 0x0020BBDA
	public static void DestroyInstance()
	{
		RootMenu.Instance = null;
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x06005A9F RID: 23199 RVA: 0x0020D9E2 File Offset: 0x0020BBE2
	// (set) Token: 0x06005AA0 RID: 23200 RVA: 0x0020D9E9 File Offset: 0x0020BBE9
	public static RootMenu Instance { get; private set; }

	// Token: 0x06005AA1 RID: 23201 RVA: 0x0020D9F1 File Offset: 0x0020BBF1
	public override float GetSortKey()
	{
		return -1f;
	}

	// Token: 0x06005AA2 RID: 23202 RVA: 0x0020D9F8 File Offset: 0x0020BBF8
	protected override void OnPrefabInit()
	{
		RootMenu.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		base.Subscribe(Game.Instance.gameObject, -809948329, new Action<object>(this.OnBuildingStatechanged));
		base.OnPrefabInit();
	}

	// Token: 0x06005AA3 RID: 23203 RVA: 0x0020DA78 File Offset: 0x0020BC78
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.detailsScreen = Util.KInstantiateUI(this.detailsScreenPrefab, base.gameObject, true).GetComponent<DetailsScreen>();
		this.detailsScreen.gameObject.SetActive(true);
		this.userMenuParent = this.detailsScreen.UserMenuPanel.gameObject;
		this.userMenu = Util.KInstantiateUI(this.userMenuPrefab.gameObject, this.userMenuParent, false).GetComponent<UserMenuScreen>();
		this.detailsScreen.gameObject.SetActive(false);
		this.userMenu.gameObject.SetActive(false);
	}

	// Token: 0x06005AA4 RID: 23204 RVA: 0x0020DB13 File Offset: 0x0020BD13
	private void OnClickCommon()
	{
		this.CloseSubMenus();
	}

	// Token: 0x06005AA5 RID: 23205 RVA: 0x0020DB1B File Offset: 0x0020BD1B
	public void AddSubMenu(KScreen sub_menu)
	{
		if (sub_menu.activateOnSpawn)
		{
			sub_menu.Show(true);
		}
		this.subMenus.Add(sub_menu);
	}

	// Token: 0x06005AA6 RID: 23206 RVA: 0x0020DB38 File Offset: 0x0020BD38
	public void RemoveSubMenu(KScreen sub_menu)
	{
		this.subMenus.Remove(sub_menu);
	}

	// Token: 0x06005AA7 RID: 23207 RVA: 0x0020DB48 File Offset: 0x0020BD48
	private void CloseSubMenus()
	{
		foreach (KScreen kscreen in this.subMenus)
		{
			if (kscreen != null)
			{
				if (kscreen.activateOnSpawn)
				{
					kscreen.gameObject.SetActive(false);
				}
				else
				{
					kscreen.Deactivate();
				}
			}
		}
		this.subMenus.Clear();
	}

	// Token: 0x06005AA8 RID: 23208 RVA: 0x0020DBC4 File Offset: 0x0020BDC4
	private void OnSelectObject(object data)
	{
		GameObject gameObject = (GameObject)data;
		bool flag = false;
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && !component.IsInitialized())
			{
				return;
			}
			flag = (component != null || CellSelectionObject.IsSelectionObject(gameObject));
		}
		if (gameObject != this.selectedGO)
		{
			if (this.selectedGO != null)
			{
				this.selectedGO.Unsubscribe(1980521255, new Action<object>(this.TriggerRefresh));
			}
			this.selectedGO = null;
			this.CloseSubMenus();
			if (flag)
			{
				this.selectedGO = gameObject;
				this.selectedGO.Subscribe(1980521255, new Action<object>(this.TriggerRefresh));
				this.AddSubMenu(this.detailsScreen);
				this.AddSubMenu(this.userMenu);
			}
			this.userMenu.SetSelected(this.selectedGO);
		}
		this.Refresh();
	}

	// Token: 0x06005AA9 RID: 23209 RVA: 0x0020DCAD File Offset: 0x0020BEAD
	public void TriggerRefresh(object obj)
	{
		this.Refresh();
	}

	// Token: 0x06005AAA RID: 23210 RVA: 0x0020DCB5 File Offset: 0x0020BEB5
	public void Refresh()
	{
		if (this.selectedGO == null)
		{
			return;
		}
		this.detailsScreen.Refresh(this.selectedGO);
		this.userMenu.Refresh(this.selectedGO);
	}

	// Token: 0x06005AAB RID: 23211 RVA: 0x0020DCE8 File Offset: 0x0020BEE8
	private void OnBuildingStatechanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == this.selectedGO)
		{
			this.OnSelectObject(gameObject);
		}
	}

	// Token: 0x06005AAC RID: 23212 RVA: 0x0020DD14 File Offset: 0x0020BF14
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && e.TryConsume(global::Action.Escape) && SelectTool.Instance.enabled)
		{
			if (!this.canTogglePauseScreen)
			{
				return;
			}
			if (this.AreSubMenusOpen())
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
				this.CloseSubMenus();
				SelectTool.Instance.Select(null, false);
			}
			else if (e.IsAction(global::Action.Escape))
			{
				if (!SelectTool.Instance.enabled)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
				}
				if (PlayerController.Instance.IsUsingDefaultTool())
				{
					if (SelectTool.Instance.selected != null)
					{
						SelectTool.Instance.Select(null, false);
					}
					else
					{
						CameraController.Instance.ForcePanningState(false);
						this.TogglePauseScreen();
					}
				}
				else
				{
					Game.Instance.Trigger(288942073, null);
				}
				ToolMenu.Instance.ClearSelection();
				SelectTool.Instance.Activate();
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005AAD RID: 23213 RVA: 0x0020DE16 File Offset: 0x0020C016
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		if (!e.Consumed && e.TryConsume(global::Action.AlternateView) && this.tileScreenInst != null)
		{
			this.tileScreenInst.Deactivate();
			this.tileScreenInst = null;
		}
	}

	// Token: 0x06005AAE RID: 23214 RVA: 0x0020DE51 File Offset: 0x0020C051
	public void TogglePauseScreen()
	{
		PauseScreen.Instance.Show(true);
	}

	// Token: 0x06005AAF RID: 23215 RVA: 0x0020DE5E File Offset: 0x0020C05E
	public void ExternalClose()
	{
		this.OnClickCommon();
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x0020DE66 File Offset: 0x0020C066
	private void OnUIClear(object data)
	{
		this.CloseSubMenus();
		SelectTool.Instance.Select(null, true);
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
			return;
		}
		global::Debug.LogWarning("OnUIClear() Event system is null");
	}

	// Token: 0x06005AB1 RID: 23217 RVA: 0x0020DE9D File Offset: 0x0020C09D
	protected override void OnActivate()
	{
		base.OnActivate();
	}

	// Token: 0x06005AB2 RID: 23218 RVA: 0x0020DEA5 File Offset: 0x0020C0A5
	private bool AreSubMenusOpen()
	{
		return this.subMenus.Count > 0;
	}

	// Token: 0x06005AB3 RID: 23219 RVA: 0x0020DEB8 File Offset: 0x0020C0B8
	private KToggleMenu.ToggleInfo[] GetFillers()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			KPrefabID kprefabID = pickupable.KPrefabID;
			if (kprefabID.HasTag(GameTags.Filler) && hashSet.Add(kprefabID.PrefabTag))
			{
				string text = kprefabID.GetComponent<PrimaryElement>().Element.id.ToString();
				list.Add(new KToggleMenu.ToggleInfo(text, null, global::Action.NumActions));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06005AB4 RID: 23220 RVA: 0x0020DF6C File Offset: 0x0020C16C
	public bool IsBuildingChorePanelActive()
	{
		return this.detailsScreen != null && this.detailsScreen.GetActiveTab() is BuildingChoresPanel;
	}

	// Token: 0x04003BAB RID: 15275
	private DetailsScreen detailsScreen;

	// Token: 0x04003BAC RID: 15276
	private UserMenuScreen userMenu;

	// Token: 0x04003BAD RID: 15277
	[SerializeField]
	private GameObject detailsScreenPrefab;

	// Token: 0x04003BAE RID: 15278
	[SerializeField]
	private UserMenuScreen userMenuPrefab;

	// Token: 0x04003BAF RID: 15279
	private GameObject userMenuParent;

	// Token: 0x04003BB0 RID: 15280
	[SerializeField]
	private TileScreen tileScreen;

	// Token: 0x04003BB2 RID: 15282
	public KScreen buildMenu;

	// Token: 0x04003BB3 RID: 15283
	private List<KScreen> subMenus = new List<KScreen>();

	// Token: 0x04003BB4 RID: 15284
	private TileScreen tileScreenInst;

	// Token: 0x04003BB5 RID: 15285
	public bool canTogglePauseScreen = true;

	// Token: 0x04003BB6 RID: 15286
	public GameObject selectedGO;
}
