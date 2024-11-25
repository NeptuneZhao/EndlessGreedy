using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200077E RID: 1918
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Switch")]
public class Switch : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06003418 RID: 13336 RVA: 0x0011C987 File Offset: 0x0011AB87
	public bool IsSwitchedOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06003419 RID: 13337 RVA: 0x0011C990 File Offset: 0x0011AB90
	// (remove) Token: 0x0600341A RID: 13338 RVA: 0x0011C9C8 File Offset: 0x0011ABC8
	public event Action<bool> OnToggle;

	// Token: 0x0600341B RID: 13339 RVA: 0x0011C9FD File Offset: 0x0011ABFD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.switchedOn = this.defaultState;
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x0011CA14 File Offset: 0x0011AC14
	protected override void OnSpawn()
	{
		this.openToggleIndex = this.openSwitch.SetTarget(this);
		if (this.OnToggle != null)
		{
			this.OnToggle(this.switchedOn);
		}
		if (this.manuallyControlled)
		{
			base.Subscribe<Switch>(493375141, Switch.OnRefreshUserMenuDelegate);
		}
		this.UpdateSwitchStatus();
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x0011CA6B File Offset: 0x0011AC6B
	public void HandleToggle()
	{
		this.Toggle();
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x0011CA73 File Offset: 0x0011AC73
	public bool IsHandlerOn()
	{
		return this.switchedOn;
	}

	// Token: 0x0600341F RID: 13343 RVA: 0x0011CA7B File Offset: 0x0011AC7B
	private void OnMinionToggle()
	{
		if (!DebugHandler.InstantBuildMode)
		{
			this.openSwitch.Toggle(this.openToggleIndex);
			return;
		}
		this.Toggle();
	}

	// Token: 0x06003420 RID: 13344 RVA: 0x0011CA9C File Offset: 0x0011AC9C
	protected virtual void Toggle()
	{
		this.SetState(!this.switchedOn);
	}

	// Token: 0x06003421 RID: 13345 RVA: 0x0011CAB0 File Offset: 0x0011ACB0
	protected virtual void SetState(bool on)
	{
		if (this.switchedOn != on)
		{
			this.switchedOn = on;
			this.UpdateSwitchStatus();
			if (this.OnToggle != null)
			{
				this.OnToggle(this.switchedOn);
			}
			if (this.manuallyControlled)
			{
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
		}
	}

	// Token: 0x06003422 RID: 13346 RVA: 0x0011CB0C File Offset: 0x0011AD0C
	protected virtual void OnRefreshUserMenu(object data)
	{
		LocString loc_string = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF : BUILDINGS.PREFABS.SWITCH.TURN_ON;
		LocString loc_string2 = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF_TOOLTIP : BUILDINGS.PREFABS.SWITCH.TURN_ON_TOOLTIP;
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_power", loc_string, new System.Action(this.OnMinionToggle), global::Action.ToggleEnabled, null, null, null, loc_string2, true), 1f);
	}

	// Token: 0x06003423 RID: 13347 RVA: 0x0011CB88 File Offset: 0x0011AD88
	protected virtual void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.SwitchStatusActive : Db.Get().BuildingStatusItems.SwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001ECD RID: 7885
	[SerializeField]
	public bool manuallyControlled = true;

	// Token: 0x04001ECE RID: 7886
	[SerializeField]
	public bool defaultState = true;

	// Token: 0x04001ECF RID: 7887
	[Serialize]
	protected bool switchedOn = true;

	// Token: 0x04001ED0 RID: 7888
	[MyCmpAdd]
	private Toggleable openSwitch;

	// Token: 0x04001ED1 RID: 7889
	private int openToggleIndex;

	// Token: 0x04001ED3 RID: 7891
	private static readonly EventSystem.IntraObjectHandler<Switch> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Switch>(delegate(Switch component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
