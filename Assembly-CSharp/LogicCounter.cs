using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000703 RID: 1795
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCounter : Switch, ISaveLoadable
{
	// Token: 0x06002E16 RID: 11798 RVA: 0x00102B25 File Offset: 0x00100D25
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicCounter>(-905833192, LogicCounter.OnCopySettingsDelegate);
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x00102B40 File Offset: 0x00100D40
	private void OnCopySettings(object data)
	{
		LogicCounter component = ((GameObject)data).GetComponent<LogicCounter>();
		if (component != null)
		{
			this.maxCount = component.maxCount;
			this.resetCountAtMax = component.resetCountAtMax;
			this.advancedMode = component.advancedMode;
		}
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x00102B88 File Offset: 0x00100D88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		if (this.maxCount == 0)
		{
			this.maxCount = 10;
		}
		base.Subscribe<LogicCounter>(-801688580, LogicCounter.OnLogicValueChangedDelegate);
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", component.FlipY ? "meter_dn" : "meter_up", Meter.Offset.UserSpecified, Grid.SceneLayer.LogicGatesFront, Vector3.zero, null);
		this.UpdateMeter();
	}

	// Token: 0x06002E19 RID: 11801 RVA: 0x00102C4D File Offset: 0x00100E4D
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x00102C7A File Offset: 0x00100E7A
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x00102C89 File Offset: 0x00100E89
	public void UpdateLogicCircuit()
	{
		if (this.receivedFirstSignal)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicCounter.OUTPUT_PORT_ID, this.switchedOn ? 1 : 0);
		}
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x00102CB0 File Offset: 0x00100EB0
	public void UpdateMeter()
	{
		float num = (float)(this.currentCount % (this.advancedMode ? this.maxCount : 10));
		this.meter.SetPositionPercent(num / 9f);
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x00102CEC File Offset: 0x00100EEC
	public void UpdateVisualState(bool force = false)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (!this.receivedFirstSignal)
		{
			component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		if (this.wasOn != this.switchedOn || force)
		{
			int num = (this.switchedOn ? 4 : 0) + (this.wasResetting ? 2 : 0) + (this.wasIncrementing ? 1 : 0);
			this.wasOn = this.switchedOn;
			component.Play("on_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06002E1E RID: 11806 RVA: 0x00102D94 File Offset: 0x00100F94
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == LogicCounter.INPUT_PORT_ID)
		{
			int newValue = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue))
			{
				if (!this.wasIncrementing)
				{
					this.wasIncrementing = true;
					if (!this.wasResetting)
					{
						if (this.currentCount == this.maxCount || this.currentCount >= 10)
						{
							this.currentCount = 0;
						}
						this.currentCount++;
						this.UpdateMeter();
						this.SetCounterState();
						if (this.currentCount == this.maxCount && this.resetCountAtMax)
						{
							this.resetRequested = true;
						}
					}
				}
			}
			else
			{
				this.wasIncrementing = false;
			}
		}
		else
		{
			if (!(logicValueChanged.portID == LogicCounter.RESET_PORT_ID))
			{
				return;
			}
			int newValue2 = logicValueChanged.newValue;
			this.receivedFirstSignal = true;
			if (LogicCircuitNetwork.IsBitActive(0, newValue2))
			{
				if (!this.wasResetting)
				{
					this.wasResetting = true;
					this.ResetCounter();
				}
			}
			else
			{
				this.wasResetting = false;
			}
		}
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002E1F RID: 11807 RVA: 0x00102EAC File Offset: 0x001010AC
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x06002E20 RID: 11808 RVA: 0x00102EFF File Offset: 0x001010FF
	public void ResetCounter()
	{
		this.resetRequested = false;
		this.currentCount = 0;
		this.SetState(false);
		if (this.advancedMode)
		{
			this.pulsingActive = false;
			this.pulseTicksRemaining = 0;
		}
		this.UpdateVisualState(true);
		this.UpdateMeter();
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002E21 RID: 11809 RVA: 0x00102F40 File Offset: 0x00101140
	public void LogicTick()
	{
		if (this.resetRequested)
		{
			this.ResetCounter();
		}
		if (this.pulsingActive)
		{
			this.pulseTicksRemaining--;
			if (this.pulseTicksRemaining <= 0)
			{
				this.pulsingActive = false;
				this.SetState(false);
				this.UpdateVisualState(false);
				this.UpdateMeter();
				this.UpdateLogicCircuit();
			}
		}
	}

	// Token: 0x06002E22 RID: 11810 RVA: 0x00102F9C File Offset: 0x0010119C
	public void SetCounterState()
	{
		this.SetState(this.advancedMode ? (this.currentCount % this.maxCount == 0) : (this.currentCount == this.maxCount));
		if (this.advancedMode && this.currentCount % this.maxCount == 0)
		{
			this.pulsingActive = true;
			this.pulseTicksRemaining = 2;
		}
	}

	// Token: 0x04001AE2 RID: 6882
	[Serialize]
	public int maxCount;

	// Token: 0x04001AE3 RID: 6883
	[Serialize]
	public int currentCount;

	// Token: 0x04001AE4 RID: 6884
	[Serialize]
	public bool resetCountAtMax;

	// Token: 0x04001AE5 RID: 6885
	[Serialize]
	public bool advancedMode;

	// Token: 0x04001AE6 RID: 6886
	private bool wasOn;

	// Token: 0x04001AE7 RID: 6887
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001AE8 RID: 6888
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001AE9 RID: 6889
	private static readonly EventSystem.IntraObjectHandler<LogicCounter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicCounter>(delegate(LogicCounter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001AEA RID: 6890
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicCounterInput");

	// Token: 0x04001AEB RID: 6891
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicCounterReset");

	// Token: 0x04001AEC RID: 6892
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicCounterOutput");

	// Token: 0x04001AED RID: 6893
	private bool resetRequested;

	// Token: 0x04001AEE RID: 6894
	[Serialize]
	private bool wasResetting;

	// Token: 0x04001AEF RID: 6895
	[Serialize]
	private bool wasIncrementing;

	// Token: 0x04001AF0 RID: 6896
	[Serialize]
	public bool receivedFirstSignal;

	// Token: 0x04001AF1 RID: 6897
	private bool pulsingActive;

	// Token: 0x04001AF2 RID: 6898
	private const int pulseLength = 1;

	// Token: 0x04001AF3 RID: 6899
	private int pulseTicksRemaining;

	// Token: 0x04001AF4 RID: 6900
	private MeterController meter;
}
