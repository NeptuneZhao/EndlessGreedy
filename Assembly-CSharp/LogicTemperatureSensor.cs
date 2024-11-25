using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200071A RID: 1818
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTemperatureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06002FDA RID: 12250 RVA: 0x00109B18 File Offset: 0x00107D18
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x06002FDB RID: 12251 RVA: 0x00109B3D File Offset: 0x00107D3D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTemperatureSensor>(-905833192, LogicTemperatureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002FDC RID: 12252 RVA: 0x00109B58 File Offset: 0x00107D58
	private void OnCopySettings(object data)
	{
		LogicTemperatureSensor component = ((GameObject)data).GetComponent<LogicTemperatureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002FDD RID: 12253 RVA: 0x00109B94 File Offset: 0x00107D94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x00109BE8 File Offset: 0x00107DE8
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.temperatures[this.simUpdateCounter] = Grid.Temperature[i];
				this.simUpdateCounter++;
			}
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageTemp = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageTemp += this.temperatures[j];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp <= this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp >= this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x00109CFF File Offset: 0x00107EFF
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x00109D07 File Offset: 0x00107F07
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x00109D16 File Offset: 0x00107F16
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x00109D34 File Offset: 0x00107F34
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

	// Token: 0x06002FE3 RID: 12259 RVA: 0x00109DBC File Offset: 0x00107FBC
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x00109E0F File Offset: 0x0010800F
	// (set) Token: 0x06002FE5 RID: 12261 RVA: 0x00109E17 File Offset: 0x00108017
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
			this.dirty = true;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x00109E27 File Offset: 0x00108027
	// (set) Token: 0x06002FE7 RID: 12263 RVA: 0x00109E2F File Offset: 0x0010802F
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x00109E3F File Offset: 0x0010803F
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x00109E47 File Offset: 0x00108047
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06002FEA RID: 12266 RVA: 0x00109E4F File Offset: 0x0010804F
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x00109E57 File Offset: 0x00108057
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x00109E65 File Offset: 0x00108065
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x06002FED RID: 12269 RVA: 0x00109E73 File Offset: 0x00108073
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x06002FEE RID: 12270 RVA: 0x00109E7A File Offset: 0x0010807A
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x06002FEF RID: 12271 RVA: 0x00109E81 File Offset: 0x00108081
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x00109E8D File Offset: 0x0010808D
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x00109E99 File Offset: 0x00108099
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, true);
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x00109EA5 File Offset: 0x001080A5
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002FF3 RID: 12275 RVA: 0x00109EAD File Offset: 0x001080AD
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x00109EB8 File Offset: 0x001080B8
	public LocString ThresholdValueUnits()
	{
		LocString result = null;
		switch (GameUtil.temperatureUnit)
		{
		case GameUtil.TemperatureUnit.Celsius:
			result = UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
			break;
		case GameUtil.TemperatureUnit.Fahrenheit:
			result = UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
			break;
		case GameUtil.TemperatureUnit.Kelvin:
			result = UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
			break;
		}
		return result;
	}

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x06002FF5 RID: 12277 RVA: 0x00109EF8 File Offset: 0x001080F8
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x00109EFB File Offset: 0x001080FB
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x00109F00 File Offset: 0x00108100
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(25f, 260f),
				new NonLinearSlider.Range(50f, 400f),
				new NonLinearSlider.Range(12f, 1500f),
				new NonLinearSlider.Range(13f, 10000f)
			};
		}
	}

	// Token: 0x04001C22 RID: 7202
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001C23 RID: 7203
	private int simUpdateCounter;

	// Token: 0x04001C24 RID: 7204
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04001C25 RID: 7205
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001C26 RID: 7206
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001C27 RID: 7207
	public float minTemp;

	// Token: 0x04001C28 RID: 7208
	public float maxTemp = 373.15f;

	// Token: 0x04001C29 RID: 7209
	private const int NumFrameDelay = 8;

	// Token: 0x04001C2A RID: 7210
	private float[] temperatures = new float[8];

	// Token: 0x04001C2B RID: 7211
	private float averageTemp;

	// Token: 0x04001C2C RID: 7212
	private bool wasOn;

	// Token: 0x04001C2D RID: 7213
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C2E RID: 7214
	private static readonly EventSystem.IntraObjectHandler<LogicTemperatureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTemperatureSensor>(delegate(LogicTemperatureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
