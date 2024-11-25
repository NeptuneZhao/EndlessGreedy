using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000710 RID: 1808
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicLightSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002F12 RID: 12050 RVA: 0x00107840 File Offset: 0x00105A40
	private void OnCopySettings(object data)
	{
		LogicLightSensor component = ((GameObject)data).GetComponent<LogicLightSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x0010787A File Offset: 0x00105A7A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicLightSensor>(-905833192, LogicLightSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x00107893 File Offset: 0x00105A93
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x001078C8 File Offset: 0x00105AC8
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 4)
		{
			this.levels[this.simUpdateCounter] = (float)Grid.LightIntensity[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageBrightness = 0f;
		for (int i = 0; i < 4; i++)
		{
			this.averageBrightness += this.levels[i];
		}
		this.averageBrightness /= 4f;
		if (this.activateOnBrighterThan)
		{
			if ((this.averageBrightness > this.thresholdBrightness && !base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageBrightness > this.thresholdBrightness && base.IsSwitchedOn) || (this.averageBrightness < this.thresholdBrightness && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x001079BD File Offset: 0x00105BBD
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x001079CC File Offset: 0x00105BCC
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x001079EC File Offset: 0x00105BEC
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

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06002F19 RID: 12057 RVA: 0x00107A73 File Offset: 0x00105C73
	// (set) Token: 0x06002F1A RID: 12058 RVA: 0x00107A7B File Offset: 0x00105C7B
	public float Threshold
	{
		get
		{
			return this.thresholdBrightness;
		}
		set
		{
			this.thresholdBrightness = value;
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06002F1B RID: 12059 RVA: 0x00107A84 File Offset: 0x00105C84
	// (set) Token: 0x06002F1C RID: 12060 RVA: 0x00107A8C File Offset: 0x00105C8C
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnBrighterThan;
		}
		set
		{
			this.activateOnBrighterThan = value;
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06002F1D RID: 12061 RVA: 0x00107A95 File Offset: 0x00105C95
	public float CurrentValue
	{
		get
		{
			return this.averageBrightness;
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06002F1E RID: 12062 RVA: 0x00107A9D File Offset: 0x00105C9D
	public float RangeMin
	{
		get
		{
			return this.minBrightness;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06002F1F RID: 12063 RVA: 0x00107AA5 File Offset: 0x00105CA5
	public float RangeMax
	{
		get
		{
			return this.maxBrightness;
		}
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x00107AAD File Offset: 0x00105CAD
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x00107AB5 File Offset: 0x00105CB5
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06002F22 RID: 12066 RVA: 0x00107ABD File Offset: 0x00105CBD
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.BRIGHTNESSSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06002F23 RID: 12067 RVA: 0x00107AC4 File Offset: 0x00105CC4
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS;
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06002F24 RID: 12068 RVA: 0x00107ACB File Offset: 0x00105CCB
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06002F25 RID: 12069 RVA: 0x00107AD7 File Offset: 0x00105CD7
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.BRIGHTNESS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002F26 RID: 12070 RVA: 0x00107AE3 File Offset: 0x00105CE3
	public string Format(float value, bool units)
	{
		if (units)
		{
			return GameUtil.GetFormattedLux((int)value);
		}
		return string.Format("{0}", (int)value);
	}

	// Token: 0x06002F27 RID: 12071 RVA: 0x00107B01 File Offset: 0x00105D01
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x00107B09 File Offset: 0x00105D09
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x00107B0C File Offset: 0x00105D0C
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.LIGHT.LUX;
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06002F2A RID: 12074 RVA: 0x00107B13 File Offset: 0x00105D13
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06002F2B RID: 12075 RVA: 0x00107B16 File Offset: 0x00105D16
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06002F2C RID: 12076 RVA: 0x00107B19 File Offset: 0x00105D19
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x00107B28 File Offset: 0x00105D28
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001BBA RID: 7098
	private int simUpdateCounter;

	// Token: 0x04001BBB RID: 7099
	[Serialize]
	public float thresholdBrightness = 280f;

	// Token: 0x04001BBC RID: 7100
	[Serialize]
	public bool activateOnBrighterThan = true;

	// Token: 0x04001BBD RID: 7101
	public float minBrightness;

	// Token: 0x04001BBE RID: 7102
	public float maxBrightness = 15000f;

	// Token: 0x04001BBF RID: 7103
	private const int NumFrameDelay = 4;

	// Token: 0x04001BC0 RID: 7104
	private float[] levels = new float[4];

	// Token: 0x04001BC1 RID: 7105
	private float averageBrightness;

	// Token: 0x04001BC2 RID: 7106
	private bool wasOn;

	// Token: 0x04001BC3 RID: 7107
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BC4 RID: 7108
	private static readonly EventSystem.IntraObjectHandler<LogicLightSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicLightSensor>(delegate(LogicLightSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
