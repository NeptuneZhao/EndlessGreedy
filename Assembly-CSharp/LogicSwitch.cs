using System;
using System.Collections;
using KSerialization;
using UnityEngine;

// Token: 0x02000719 RID: 1817
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06002FC7 RID: 12231 RVA: 0x00109896 File Offset: 0x00107A96
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicSwitch>(-905833192, LogicSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06002FC8 RID: 12232 RVA: 0x001098B0 File Offset: 0x00107AB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wasOn = this.switchedOn;
		this.UpdateLogicCircuit();
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002FC9 RID: 12233 RVA: 0x00109904 File Offset: 0x00107B04
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002FCA RID: 12234 RVA: 0x0010990C File Offset: 0x00107B0C
	private void OnCopySettings(object data)
	{
		LogicSwitch component = ((GameObject)data).GetComponent<LogicSwitch>();
		if (component != null && this.switchedOn != component.switchedOn)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateVisualization();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x06002FCB RID: 12235 RVA: 0x00109954 File Offset: 0x00107B54
	protected override void Toggle()
	{
		base.Toggle();
		this.UpdateVisualization();
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002FCC RID: 12236 RVA: 0x00109968 File Offset: 0x00107B68
	private void UpdateVisualization()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.wasOn != this.switchedOn)
		{
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x001099EA File Offset: 0x00107BEA
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x00109A08 File Offset: 0x00107C08
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSwitchStatusActive : Db.Get().BuildingStatusItems.LogicSwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x00109A5B File Offset: 0x00107C5B
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06002FD0 RID: 12240 RVA: 0x00109A8F File Offset: 0x00107C8F
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06002FD1 RID: 12241 RVA: 0x00109AA5 File Offset: 0x00107CA5
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002FD2 RID: 12242 RVA: 0x00109AB4 File Offset: 0x00107CB4
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06002FD3 RID: 12243 RVA: 0x00109ABC File Offset: 0x00107CBC
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06002FD4 RID: 12244 RVA: 0x00109AC4 File Offset: 0x00107CC4
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x00109ACC File Offset: 0x00107CCC
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x00109AD3 File Offset: 0x00107CD3
	// (set) Token: 0x06002FD7 RID: 12247 RVA: 0x00109ADB File Offset: 0x00107CDB
	public bool ToggleRequested { get; set; }

	// Token: 0x04001C1C RID: 7196
	public static readonly HashedString PORT_ID = "LogicSwitch";

	// Token: 0x04001C1D RID: 7197
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C1E RID: 7198
	private static readonly EventSystem.IntraObjectHandler<LogicSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicSwitch>(delegate(LogicSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001C1F RID: 7199
	private bool wasOn;

	// Token: 0x04001C20 RID: 7200
	private System.Action firstFrameCallback;
}
