using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000BA6 RID: 2982
public class OverlayMenu : KIconToggleMenu
{
	// Token: 0x06005A56 RID: 23126 RVA: 0x0020C441 File Offset: 0x0020A641
	public static void DestroyInstance()
	{
		OverlayMenu.Instance = null;
	}

	// Token: 0x06005A57 RID: 23127 RVA: 0x0020C44C File Offset: 0x0020A64C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		OverlayMenu.Instance = this;
		this.InitializeToggles();
		base.Setup(this.overlayToggleInfos);
		Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		KInputManager.InputChange.AddListener(new UnityAction(this.Refresh));
		base.onSelect += this.OnToggleSelect;
	}

	// Token: 0x06005A58 RID: 23128 RVA: 0x0020C4D7 File Offset: 0x0020A6D7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshButtons();
	}

	// Token: 0x06005A59 RID: 23129 RVA: 0x0020C4E5 File Offset: 0x0020A6E5
	public void Refresh()
	{
		this.RefreshButtons();
	}

	// Token: 0x06005A5A RID: 23130 RVA: 0x0020C4F0 File Offset: 0x0020A6F0
	protected override void RefreshButtons()
	{
		base.RefreshButtons();
		if (Research.Instance == null)
		{
			return;
		}
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.overlayToggleInfos)
		{
			OverlayMenu.OverlayToggleInfo overlayToggleInfo = (OverlayMenu.OverlayToggleInfo)toggleInfo;
			toggleInfo.toggle.gameObject.SetActive(overlayToggleInfo.IsUnlocked());
			toggleInfo.tooltip = GameUtil.ReplaceHotkeyString(overlayToggleInfo.originalToolTipText, toggleInfo.hotKey);
		}
	}

	// Token: 0x06005A5B RID: 23131 RVA: 0x0020C584 File Offset: 0x0020A784
	private void OnResearchComplete(object data)
	{
		this.RefreshButtons();
	}

	// Token: 0x06005A5C RID: 23132 RVA: 0x0020C58C File Offset: 0x0020A78C
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.Refresh));
		base.OnForcedCleanUp();
	}

	// Token: 0x06005A5D RID: 23133 RVA: 0x0020C5AA File Offset: 0x0020A7AA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(1798162660, new Action<object>(this.OnOverlayChanged));
	}

	// Token: 0x06005A5E RID: 23134 RVA: 0x0020C5CD File Offset: 0x0020A7CD
	private void InitializeToggleGroups()
	{
	}

	// Token: 0x06005A5F RID: 23135 RVA: 0x0020C5D0 File Offset: 0x0020A7D0
	private void InitializeToggles()
	{
		this.overlayToggleInfos = new List<KIconToggleMenu.ToggleInfo>
		{
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.OXYGEN.BUTTON, "overlay_oxygen", OverlayModes.Oxygen.ID, "", global::Action.Overlay1, UI.TOOLTIPS.OXYGENOVERLAYSTRING, UI.OVERLAYS.OXYGEN.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ELECTRICAL.BUTTON, "overlay_power", OverlayModes.Power.ID, "", global::Action.Overlay2, UI.TOOLTIPS.POWEROVERLAYSTRING, UI.OVERLAYS.ELECTRICAL.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TEMPERATURE.BUTTON, "overlay_temperature", OverlayModes.Temperature.ID, "", global::Action.Overlay3, UI.TOOLTIPS.TEMPERATUREOVERLAYSTRING, UI.OVERLAYS.TEMPERATURE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TILEMODE.BUTTON, "overlay_materials", OverlayModes.TileMode.ID, "", global::Action.Overlay4, UI.TOOLTIPS.TILEMODE_OVERLAY_STRING, UI.OVERLAYS.TILEMODE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIGHTING.BUTTON, "overlay_lights", OverlayModes.Light.ID, "", global::Action.Overlay5, UI.TOOLTIPS.LIGHTSOVERLAYSTRING, UI.OVERLAYS.LIGHTING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIQUIDPLUMBING.BUTTON, "overlay_liquidvent", OverlayModes.LiquidConduits.ID, "", global::Action.Overlay6, UI.TOOLTIPS.LIQUIDVENTOVERLAYSTRING, UI.OVERLAYS.LIQUIDPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.GASPLUMBING.BUTTON, "overlay_gasvent", OverlayModes.GasConduits.ID, "", global::Action.Overlay7, UI.TOOLTIPS.GASVENTOVERLAYSTRING, UI.OVERLAYS.GASPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DECOR.BUTTON, "overlay_decor", OverlayModes.Decor.ID, "", global::Action.Overlay8, UI.TOOLTIPS.DECOROVERLAYSTRING, UI.OVERLAYS.DECOR.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DISEASE.BUTTON, "overlay_disease", OverlayModes.Disease.ID, "", global::Action.Overlay9, UI.TOOLTIPS.DISEASEOVERLAYSTRING, UI.OVERLAYS.DISEASE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CROPS.BUTTON, "overlay_farming", OverlayModes.Crop.ID, "", global::Action.Overlay10, UI.TOOLTIPS.CROPS_OVERLAY_STRING, UI.OVERLAYS.CROPS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ROOMS.BUTTON, "overlay_rooms", OverlayModes.Rooms.ID, "", global::Action.Overlay11, UI.TOOLTIPS.ROOMSOVERLAYSTRING, UI.OVERLAYS.ROOMS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.SUIT.BUTTON, "overlay_suit", OverlayModes.Suit.ID, "SuitsOverlay", global::Action.Overlay12, UI.TOOLTIPS.SUITOVERLAYSTRING, UI.OVERLAYS.SUIT.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LOGIC.BUTTON, "overlay_logic", OverlayModes.Logic.ID, "AutomationOverlay", global::Action.Overlay13, UI.TOOLTIPS.LOGICOVERLAYSTRING, UI.OVERLAYS.LOGIC.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CONVEYOR.BUTTON, "overlay_conveyor", OverlayModes.SolidConveyor.ID, "ConveyorOverlay", global::Action.Overlay14, UI.TOOLTIPS.CONVEYOR_OVERLAY_STRING, UI.OVERLAYS.CONVEYOR.BUTTON)
		};
		if (Sim.IsRadiationEnabled())
		{
			this.overlayToggleInfos.Add(new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.RADIATION.BUTTON, "overlay_radiation", OverlayModes.Radiation.ID, "", global::Action.Overlay15, UI.TOOLTIPS.RADIATIONOVERLAYSTRING, UI.OVERLAYS.RADIATION.BUTTON));
		}
	}

	// Token: 0x06005A60 RID: 23136 RVA: 0x0020C970 File Offset: 0x0020AB70
	private void OnToggleSelect(KIconToggleMenu.ToggleInfo toggle_info)
	{
		if (SimDebugView.Instance.GetMode() == ((OverlayMenu.OverlayToggleInfo)toggle_info).simView)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			return;
		}
		if (((OverlayMenu.OverlayToggleInfo)toggle_info).IsUnlocked())
		{
			OverlayScreen.Instance.ToggleOverlay(((OverlayMenu.OverlayToggleInfo)toggle_info).simView, true);
		}
	}

	// Token: 0x06005A61 RID: 23137 RVA: 0x0020C9D0 File Offset: 0x0020ABD0
	private void OnOverlayChanged(object overlay_data)
	{
		HashedString y = (HashedString)overlay_data;
		for (int i = 0; i < this.overlayToggleInfos.Count; i++)
		{
			this.overlayToggleInfos[i].toggle.isOn = (((OverlayMenu.OverlayToggleInfo)this.overlayToggleInfos[i]).simView == y);
		}
	}

	// Token: 0x06005A62 RID: 23138 RVA: 0x0020CA2C File Offset: 0x0020AC2C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && e.TryConsume(global::Action.Escape))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06005A63 RID: 23139 RVA: 0x0020CA80 File Offset: 0x0020AC80
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x04003B5F RID: 15199
	public static OverlayMenu Instance;

	// Token: 0x04003B60 RID: 15200
	private List<KIconToggleMenu.ToggleInfo> overlayToggleInfos;

	// Token: 0x04003B61 RID: 15201
	private UnityAction inputChangeReceiver;

	// Token: 0x02001C1A RID: 7194
	private class OverlayToggleGroup : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x0600A55B RID: 42331 RVA: 0x0038F9E1 File Offset: 0x0038DBE1
		public OverlayToggleGroup(string text, string icon_name, List<OverlayMenu.OverlayToggleInfo> toggle_group, string required_tech_item = "", global::Action hot_key = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hot_key, tooltip, tooltip_header)
		{
			this.toggleInfoGroup = toggle_group;
		}

		// Token: 0x0600A55C RID: 42332 RVA: 0x0038F9F9 File Offset: 0x0038DBF9
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);
		}

		// Token: 0x0600A55D RID: 42333 RVA: 0x0038FA26 File Offset: 0x0038DC26
		public OverlayMenu.OverlayToggleInfo GetActiveToggleInfo()
		{
			return this.toggleInfoGroup[this.activeToggleInfo];
		}

		// Token: 0x040081DA RID: 33242
		public List<OverlayMenu.OverlayToggleInfo> toggleInfoGroup;

		// Token: 0x040081DB RID: 33243
		public string requiredTechItem;

		// Token: 0x040081DC RID: 33244
		[SerializeField]
		private int activeToggleInfo;
	}

	// Token: 0x02001C1B RID: 7195
	private class OverlayToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x0600A55E RID: 42334 RVA: 0x0038FA39 File Offset: 0x0038DC39
		public OverlayToggleInfo(string text, string icon_name, HashedString sim_view, string required_tech_item = "", global::Action hotKey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hotKey, tooltip, tooltip_header)
		{
			this.originalToolTipText = tooltip;
			tooltip = GameUtil.ReplaceHotkeyString(tooltip, hotKey);
			this.simView = sim_view;
			this.requiredTechItem = required_tech_item;
		}

		// Token: 0x0600A55F RID: 42335 RVA: 0x0038FA6C File Offset: 0x0038DC6C
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem) || Game.Instance.SandboxModeActive;
		}

		// Token: 0x040081DD RID: 33245
		public HashedString simView;

		// Token: 0x040081DE RID: 33246
		public string requiredTechItem;

		// Token: 0x040081DF RID: 33247
		public string originalToolTipText;
	}
}
