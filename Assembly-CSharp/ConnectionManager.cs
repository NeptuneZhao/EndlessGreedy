using System;
using KSerialization;
using STRINGS;

// Token: 0x020006E2 RID: 1762
public class ConnectionManager : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06002CC1 RID: 11457 RVA: 0x000FB774 File Offset: 0x000F9974
	// (set) Token: 0x06002CC2 RID: 11458 RVA: 0x000FB77C File Offset: 0x000F997C
	public bool IsConnected
	{
		get
		{
			return this.connected;
		}
		set
		{
			this.connected = value;
			this.connectedMeter.SetPositionPercent(value ? 1f : 0f);
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000FB79F File Offset: 0x000F999F
	public bool WaitingForToggle
	{
		get
		{
			return this.toggleQueued;
		}
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x000FB7A7 File Offset: 0x000F99A7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleIdx = this.toggleable.SetTarget(this);
		base.Subscribe<ConnectionManager>(493375141, ConnectionManager.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x000FB7D4 File Offset: 0x000F99D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.toggleQueued)
		{
			this.OnMenuToggle();
		}
		this.connectedMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_connected_target", "meter_connected", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.CONNECTED_SYMBOLS);
		this.connectedMeter.SetPositionPercent(this.IsConnected ? 1f : 0f);
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x000FB837 File Offset: 0x000F9A37
	public void HandleToggle()
	{
		this.toggleQueued = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000FB851 File Offset: 0x000F9A51
	private void OnToggle()
	{
		this.IsConnected = !this.IsConnected;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x000FB878 File Offset: 0x000F9A78
	private void OnMenuToggle()
	{
		if (!this.toggleable.IsToggleQueued(this.toggleIdx))
		{
			if (this.IsConnected)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.toggleQueued = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.toggleQueued = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.toggleable.Toggle(this.toggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x000FB8FC File Offset: 0x000F9AFC
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showButton)
		{
			return;
		}
		bool isConnected = this.IsConnected;
		bool flag = this.toggleable.IsToggleQueued(this.toggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isConnected && !flag) || (!isConnected && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x000FB9C0 File Offset: 0x000F9BC0
	bool IToggleHandler.IsHandlerOn()
	{
		return this.IsConnected;
	}

	// Token: 0x040019D2 RID: 6610
	[MyCmpAdd]
	private ToggleGeothermalVentConnection toggleable;

	// Token: 0x040019D3 RID: 6611
	[MyCmpGet]
	private GeothermalVent vent;

	// Token: 0x040019D4 RID: 6612
	private int toggleIdx;

	// Token: 0x040019D5 RID: 6613
	private MeterController connectedMeter;

	// Token: 0x040019D6 RID: 6614
	public bool showButton;

	// Token: 0x040019D7 RID: 6615
	[Serialize]
	private bool connected;

	// Token: 0x040019D8 RID: 6616
	[Serialize]
	private bool toggleQueued;

	// Token: 0x040019D9 RID: 6617
	private static readonly EventSystem.IntraObjectHandler<ConnectionManager> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ConnectionManager>(delegate(ConnectionManager component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
