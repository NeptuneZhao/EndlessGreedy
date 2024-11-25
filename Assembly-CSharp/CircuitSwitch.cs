using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000694 RID: 1684
[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06002A06 RID: 10758 RVA: 0x000ECCA4 File Offset: 0x000EAEA4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CircuitSwitch>(-905833192, CircuitSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x000ECCC0 File Offset: 0x000EAEC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.CircuitOnToggle;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		Wire wire = (gameObject != null) ? gameObject.GetComponent<Wire>() : null;
		if (wire == null)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
		this.AttachWire(wire);
		this.wasOn = this.switchedOn;
		this.UpdateCircuit(true);
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x000ECD90 File Offset: 0x000EAF90
	protected override void OnCleanUp()
	{
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		bool switchedOn = this.switchedOn;
		this.switchedOn = true;
		this.UpdateCircuit(false);
		this.switchedOn = switchedOn;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x000ECDD4 File Offset: 0x000EAFD4
	private void OnCopySettings(object data)
	{
		CircuitSwitch component = ((GameObject)data).GetComponent<CircuitSwitch>();
		if (component != null)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateCircuit(true);
		}
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x000ECE0C File Offset: 0x000EB00C
	public bool IsConnected()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<IDisconnectable>() != null;
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x000ECE50 File Offset: 0x000EB050
	private void CircuitOnToggle(bool on)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x000ECE5C File Offset: 0x000EB05C
	public void AttachWire(Wire wire)
	{
		if (wire == this.attachedWire)
		{
			return;
		}
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		this.attachedWire = wire;
		if (this.attachedWire != null)
		{
			this.SubscribeToWire(this.attachedWire);
			this.UpdateCircuit(true);
			this.wireConnectedGUID = base.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID, false);
			return;
		}
		if (this.wireConnectedGUID == Guid.Empty)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x000ECF06 File Offset: 0x000EB106
	private void OnWireDestroyed(object data)
	{
		if (this.attachedWire != null)
		{
			this.attachedWire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		}
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x000ECF32 File Offset: 0x000EB132
	private void OnWireStateChanged(object data)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x000ECF3C File Offset: 0x000EB13C
	private void SubscribeToWire(Wire wire)
	{
		wire.Subscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Subscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Subscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x06002A10 RID: 10768 RVA: 0x000ECF94 File Offset: 0x000EB194
	private void UnsubscribeFromWire(Wire wire)
	{
		wire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Unsubscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Unsubscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x000ECFE8 File Offset: 0x000EB1E8
	private void UpdateCircuit(bool should_update_anim = true)
	{
		if (this.attachedWire != null)
		{
			if (this.switchedOn)
			{
				this.attachedWire.Connect();
			}
			else
			{
				this.attachedWire.Disconnect();
			}
		}
		if (should_update_anim && this.wasOn != this.switchedOn)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x000ED0AF File Offset: 0x000EB2AF
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x000ED0E3 File Offset: 0x000EB2E3
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000ED0EB File Offset: 0x000EB2EB
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x000ED0F3 File Offset: 0x000EB2F3
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06002A16 RID: 10774 RVA: 0x000ED0FB File Offset: 0x000EB2FB
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.SWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06002A17 RID: 10775 RVA: 0x000ED102 File Offset: 0x000EB302
	// (set) Token: 0x06002A18 RID: 10776 RVA: 0x000ED10A File Offset: 0x000EB30A
	public bool ToggleRequested { get; set; }

	// Token: 0x04001835 RID: 6197
	[SerializeField]
	public ObjectLayer objectLayer;

	// Token: 0x04001836 RID: 6198
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001837 RID: 6199
	private static readonly EventSystem.IntraObjectHandler<CircuitSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CircuitSwitch>(delegate(CircuitSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001838 RID: 6200
	private Wire attachedWire;

	// Token: 0x04001839 RID: 6201
	private Guid wireConnectedGUID;

	// Token: 0x0400183A RID: 6202
	private bool wasOn;
}
