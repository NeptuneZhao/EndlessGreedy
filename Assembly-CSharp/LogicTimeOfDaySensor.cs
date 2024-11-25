using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200071B RID: 1819
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimeOfDaySensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x06002FFA RID: 12282 RVA: 0x00109FB8 File Offset: 0x001081B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimeOfDaySensor>(-905833192, LogicTimeOfDaySensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x00109FD4 File Offset: 0x001081D4
	private void OnCopySettings(object data)
	{
		LogicTimeOfDaySensor component = ((GameObject)data).GetComponent<LogicTimeOfDaySensor>();
		if (component != null)
		{
			this.startTime = component.startTime;
			this.duration = component.duration;
		}
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x0010A00E File Offset: 0x0010820E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x0010A044 File Offset: 0x00108244
	public void Sim200ms(float dt)
	{
		float currentCycleAsPercentage = GameClock.Instance.GetCurrentCycleAsPercentage();
		bool state = false;
		if (currentCycleAsPercentage >= this.startTime && currentCycleAsPercentage < this.startTime + this.duration)
		{
			state = true;
		}
		if (currentCycleAsPercentage < this.startTime + this.duration - 1f)
		{
			state = true;
		}
		this.SetState(state);
	}

	// Token: 0x06002FFE RID: 12286 RVA: 0x0010A098 File Offset: 0x00108298
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x0010A0A7 File Offset: 0x001082A7
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x0010A0C8 File Offset: 0x001082C8
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

	// Token: 0x06003001 RID: 12289 RVA: 0x0010A150 File Offset: 0x00108350
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001C2F RID: 7215
	[SerializeField]
	[Serialize]
	public float startTime;

	// Token: 0x04001C30 RID: 7216
	[SerializeField]
	[Serialize]
	public float duration = 1f;

	// Token: 0x04001C31 RID: 7217
	private bool wasOn;

	// Token: 0x04001C32 RID: 7218
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C33 RID: 7219
	private static readonly EventSystem.IntraObjectHandler<LogicTimeOfDaySensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimeOfDaySensor>(delegate(LogicTimeOfDaySensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
