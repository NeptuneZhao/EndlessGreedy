using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200071C RID: 1820
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimerSensor : Switch, ISaveLoadable, ISim33ms
{
	// Token: 0x06003004 RID: 12292 RVA: 0x0010A1D2 File Offset: 0x001083D2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimerSensor>(-905833192, LogicTimerSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x0010A1EC File Offset: 0x001083EC
	private void OnCopySettings(object data)
	{
		LogicTimerSensor component = ((GameObject)data).GetComponent<LogicTimerSensor>();
		if (component != null)
		{
			this.onDuration = component.onDuration;
			this.offDuration = component.offDuration;
			this.timeElapsedInCurrentState = component.timeElapsedInCurrentState;
			this.displayCyclesMode = component.displayCyclesMode;
		}
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x0010A23E File Offset: 0x0010843E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x0010A274 File Offset: 0x00108474
	public void Sim33ms(float dt)
	{
		if (this.onDuration == 0f && this.offDuration == 0f)
		{
			return;
		}
		this.timeElapsedInCurrentState += dt;
		bool flag = base.IsSwitchedOn;
		if (flag)
		{
			if (this.timeElapsedInCurrentState >= this.onDuration)
			{
				flag = false;
				this.timeElapsedInCurrentState -= this.onDuration;
			}
		}
		else if (this.timeElapsedInCurrentState >= this.offDuration)
		{
			flag = true;
			this.timeElapsedInCurrentState -= this.offDuration;
		}
		this.SetState(flag);
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x0010A303 File Offset: 0x00108503
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x0010A312 File Offset: 0x00108512
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x0600300A RID: 12298 RVA: 0x0010A330 File Offset: 0x00108530
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0600300B RID: 12299 RVA: 0x0010A3B8 File Offset: 0x001085B8
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x0010A40B File Offset: 0x0010860B
	public void ResetTimer()
	{
		this.SetState(true);
		this.OnSwitchToggled(true);
		this.timeElapsedInCurrentState = 0f;
	}

	// Token: 0x04001C34 RID: 7220
	[Serialize]
	public float onDuration = 10f;

	// Token: 0x04001C35 RID: 7221
	[Serialize]
	public float offDuration = 10f;

	// Token: 0x04001C36 RID: 7222
	[Serialize]
	public bool displayCyclesMode;

	// Token: 0x04001C37 RID: 7223
	private bool wasOn;

	// Token: 0x04001C38 RID: 7224
	[SerializeField]
	[Serialize]
	public float timeElapsedInCurrentState;

	// Token: 0x04001C39 RID: 7225
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C3A RID: 7226
	private static readonly EventSystem.IntraObjectHandler<LogicTimerSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimerSensor>(delegate(LogicTimerSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
