using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000714 RID: 1812
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicPressureSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002F5E RID: 12126 RVA: 0x0010861B File Offset: 0x0010681B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicPressureSensor>(-905833192, LogicPressureSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002F5F RID: 12127 RVA: 0x00108634 File Offset: 0x00106834
	private void OnCopySettings(object data)
	{
		LogicPressureSensor component = ((GameObject)data).GetComponent<LogicPressureSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002F60 RID: 12128 RVA: 0x0010866E File Offset: 0x0010686E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002F61 RID: 12129 RVA: 0x001086A4 File Offset: 0x001068A4
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			float num2 = Grid.Element[num].IsState(this.desiredState) ? Grid.Mass[num] : 0f;
			this.samples[this.sampleIdx] = num2;
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002F62 RID: 12130 RVA: 0x0010876C File Offset: 0x0010696C
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06002F63 RID: 12131 RVA: 0x0010877B File Offset: 0x0010697B
	// (set) Token: 0x06002F64 RID: 12132 RVA: 0x00108783 File Offset: 0x00106983
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06002F65 RID: 12133 RVA: 0x0010878C File Offset: 0x0010698C
	// (set) Token: 0x06002F66 RID: 12134 RVA: 0x00108794 File Offset: 0x00106994
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06002F67 RID: 12135 RVA: 0x001087A0 File Offset: 0x001069A0
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06002F68 RID: 12136 RVA: 0x001087D1 File Offset: 0x001069D1
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170002D8 RID: 728
	// (get) Token: 0x06002F69 RID: 12137 RVA: 0x001087D9 File Offset: 0x001069D9
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06002F6A RID: 12138 RVA: 0x001087E1 File Offset: 0x001069E1
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x06002F6B RID: 12139 RVA: 0x001087FF File Offset: 0x001069FF
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x170002D9 RID: 729
	// (get) Token: 0x06002F6C RID: 12140 RVA: 0x0010881D File Offset: 0x00106A1D
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170002DA RID: 730
	// (get) Token: 0x06002F6D RID: 12141 RVA: 0x00108824 File Offset: 0x00106A24
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002DB RID: 731
	// (get) Token: 0x06002F6E RID: 12142 RVA: 0x00108830 File Offset: 0x00106A30
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002F6F RID: 12143 RVA: 0x0010883C File Offset: 0x00106A3C
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat;
		if (this.desiredState == Element.State.Gas)
		{
			massFormat = GameUtil.MetricMassFormat.Gram;
		}
		else
		{
			massFormat = GameUtil.MetricMassFormat.Kilogram;
		}
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x06002F70 RID: 12144 RVA: 0x00108868 File Offset: 0x00106A68
	public float ProcessedSliderValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input = Mathf.Round(input * 1000f) / 1000f;
		}
		else
		{
			input = Mathf.Round(input);
		}
		return input;
	}

	// Token: 0x06002F71 RID: 12145 RVA: 0x00108892 File Offset: 0x00106A92
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x06002F72 RID: 12146 RVA: 0x001088A7 File Offset: 0x00106AA7
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06002F73 RID: 12147 RVA: 0x001088B7 File Offset: 0x00106AB7
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06002F74 RID: 12148 RVA: 0x001088BE File Offset: 0x00106ABE
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06002F75 RID: 12149 RVA: 0x001088C1 File Offset: 0x00106AC1
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06002F76 RID: 12150 RVA: 0x001088C4 File Offset: 0x00106AC4
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06002F77 RID: 12151 RVA: 0x001088D1 File Offset: 0x00106AD1
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x001088F0 File Offset: 0x00106AF0
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

	// Token: 0x06002F79 RID: 12153 RVA: 0x00108978 File Offset: 0x00106B78
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001BE3 RID: 7139
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001BE4 RID: 7140
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001BE5 RID: 7141
	private bool wasOn;

	// Token: 0x04001BE6 RID: 7142
	public float rangeMin;

	// Token: 0x04001BE7 RID: 7143
	public float rangeMax = 1f;

	// Token: 0x04001BE8 RID: 7144
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04001BE9 RID: 7145
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001BEA RID: 7146
	private float[] samples = new float[8];

	// Token: 0x04001BEB RID: 7147
	private int sampleIdx;

	// Token: 0x04001BEC RID: 7148
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BED RID: 7149
	private static readonly EventSystem.IntraObjectHandler<LogicPressureSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicPressureSensor>(delegate(LogicPressureSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
