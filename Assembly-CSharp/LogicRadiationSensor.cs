using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000715 RID: 1813
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicRadiationSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06002F7C RID: 12156 RVA: 0x00108A14 File Offset: 0x00106C14
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRadiationSensor>(-905833192, LogicRadiationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x00108A30 File Offset: 0x00106C30
	private void OnCopySettings(object data)
	{
		LogicRadiationSensor component = ((GameObject)data).GetComponent<LogicRadiationSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06002F7E RID: 12158 RVA: 0x00108A6A File Offset: 0x00106C6A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06002F7F RID: 12159 RVA: 0x00108AA0 File Offset: 0x00106CA0
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			this.radHistory[this.simUpdateCounter] = Grid.Radiation[i];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageRads = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageRads += this.radHistory[j];
		}
		this.averageRads /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageRads > this.thresholdRads && !base.IsSwitchedOn) || (this.averageRads <= this.thresholdRads && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageRads >= this.thresholdRads && base.IsSwitchedOn) || (this.averageRads < this.thresholdRads && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06002F80 RID: 12160 RVA: 0x00108BA5 File Offset: 0x00106DA5
	public float GetAverageRads()
	{
		return this.averageRads;
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x00108BAD File Offset: 0x00106DAD
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06002F82 RID: 12162 RVA: 0x00108BBC File Offset: 0x00106DBC
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06002F83 RID: 12163 RVA: 0x00108BDC File Offset: 0x00106DDC
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

	// Token: 0x06002F84 RID: 12164 RVA: 0x00108C64 File Offset: 0x00106E64
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06002F85 RID: 12165 RVA: 0x00108CB7 File Offset: 0x00106EB7
	// (set) Token: 0x06002F86 RID: 12166 RVA: 0x00108CBF File Offset: 0x00106EBF
	public float Threshold
	{
		get
		{
			return this.thresholdRads;
		}
		set
		{
			this.thresholdRads = value;
			this.dirty = true;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06002F87 RID: 12167 RVA: 0x00108CCF File Offset: 0x00106ECF
	// (set) Token: 0x06002F88 RID: 12168 RVA: 0x00108CD7 File Offset: 0x00106ED7
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

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06002F89 RID: 12169 RVA: 0x00108CE7 File Offset: 0x00106EE7
	public float CurrentValue
	{
		get
		{
			return this.GetAverageRads();
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06002F8A RID: 12170 RVA: 0x00108CEF File Offset: 0x00106EEF
	public float RangeMin
	{
		get
		{
			return this.minRads;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06002F8B RID: 12171 RVA: 0x00108CF7 File Offset: 0x00106EF7
	public float RangeMax
	{
		get
		{
			return this.maxRads;
		}
	}

	// Token: 0x06002F8C RID: 12172 RVA: 0x00108CFF File Offset: 0x00106EFF
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06002F8D RID: 12173 RVA: 0x00108D0D File Offset: 0x00106F0D
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06002F8E RID: 12174 RVA: 0x00108D1B File Offset: 0x00106F1B
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.RADIATIONSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06002F8F RID: 12175 RVA: 0x00108D22 File Offset: 0x00106F22
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06002F90 RID: 12176 RVA: 0x00108D29 File Offset: 0x00106F29
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06002F91 RID: 12177 RVA: 0x00108D35 File Offset: 0x00106F35
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002F92 RID: 12178 RVA: 0x00108D41 File Offset: 0x00106F41
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06002F93 RID: 12179 RVA: 0x00108D4A File Offset: 0x00106F4A
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002F94 RID: 12180 RVA: 0x00108D52 File Offset: 0x00106F52
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06002F95 RID: 12181 RVA: 0x00108D55 File Offset: 0x00106F55
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06002F96 RID: 12182 RVA: 0x00108D61 File Offset: 0x00106F61
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06002F97 RID: 12183 RVA: 0x00108D64 File Offset: 0x00106F64
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06002F98 RID: 12184 RVA: 0x00108D68 File Offset: 0x00106F68
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(50f, 200f),
				new NonLinearSlider.Range(25f, 1000f),
				new NonLinearSlider.Range(25f, 5000f)
			};
		}
	}

	// Token: 0x04001BEE RID: 7150
	private int simUpdateCounter;

	// Token: 0x04001BEF RID: 7151
	[Serialize]
	public float thresholdRads = 280f;

	// Token: 0x04001BF0 RID: 7152
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001BF1 RID: 7153
	[Serialize]
	private bool dirty = true;

	// Token: 0x04001BF2 RID: 7154
	public float minRads;

	// Token: 0x04001BF3 RID: 7155
	public float maxRads = 5000f;

	// Token: 0x04001BF4 RID: 7156
	private const int NumFrameDelay = 8;

	// Token: 0x04001BF5 RID: 7157
	private float[] radHistory = new float[8];

	// Token: 0x04001BF6 RID: 7158
	private float averageRads;

	// Token: 0x04001BF7 RID: 7159
	private bool wasOn;

	// Token: 0x04001BF8 RID: 7160
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001BF9 RID: 7161
	private static readonly EventSystem.IntraObjectHandler<LogicRadiationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRadiationSensor>(delegate(LogicRadiationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
