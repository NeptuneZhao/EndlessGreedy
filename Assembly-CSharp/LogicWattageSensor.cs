using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200071D RID: 1821
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicWattageSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x0600300F RID: 12303 RVA: 0x0010A460 File Offset: 0x00108660
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicWattageSensor>(-905833192, LogicWattageSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06003010 RID: 12304 RVA: 0x0010A47C File Offset: 0x0010867C
	private void OnCopySettings(object data)
	{
		LogicWattageSensor component = ((GameObject)data).GetComponent<LogicWattageSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x0010A4B6 File Offset: 0x001086B6
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x0010A4EC File Offset: 0x001086EC
	public void Sim200ms(float dt)
	{
		this.currentWattage = Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(Grid.PosToCell(this)));
		this.currentWattage = Mathf.Max(0f, this.currentWattage);
		if (this.activateOnHigherThan)
		{
			if ((this.currentWattage > this.thresholdWattage && !base.IsSwitchedOn) || (this.currentWattage <= this.thresholdWattage && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.currentWattage >= this.thresholdWattage && base.IsSwitchedOn) || (this.currentWattage < this.thresholdWattage && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x0010A5A6 File Offset: 0x001087A6
	public float GetWattageUsed()
	{
		return this.currentWattage;
	}

	// Token: 0x06003014 RID: 12308 RVA: 0x0010A5AE File Offset: 0x001087AE
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06003015 RID: 12309 RVA: 0x0010A5BD File Offset: 0x001087BD
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x0010A5DC File Offset: 0x001087DC
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

	// Token: 0x06003017 RID: 12311 RVA: 0x0010A664 File Offset: 0x00108864
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06003018 RID: 12312 RVA: 0x0010A6B7 File Offset: 0x001088B7
	// (set) Token: 0x06003019 RID: 12313 RVA: 0x0010A6BF File Offset: 0x001088BF
	public float Threshold
	{
		get
		{
			return this.thresholdWattage;
		}
		set
		{
			this.thresholdWattage = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x0600301A RID: 12314 RVA: 0x0010A6CF File Offset: 0x001088CF
	// (set) Token: 0x0600301B RID: 12315 RVA: 0x0010A6D7 File Offset: 0x001088D7
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnHigherThan;
		}
		set
		{
			this.activateOnHigherThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x17000301 RID: 769
	// (get) Token: 0x0600301C RID: 12316 RVA: 0x0010A6E7 File Offset: 0x001088E7
	public float CurrentValue
	{
		get
		{
			return this.GetWattageUsed();
		}
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x0600301D RID: 12317 RVA: 0x0010A6EF File Offset: 0x001088EF
	public float RangeMin
	{
		get
		{
			return this.minWattage;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x0600301E RID: 12318 RVA: 0x0010A6F7 File Offset: 0x001088F7
	public float RangeMax
	{
		get
		{
			return this.maxWattage;
		}
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x0010A6FF File Offset: 0x001088FF
	public float GetRangeMinInputField()
	{
		return this.minWattage;
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x0010A707 File Offset: 0x00108907
	public float GetRangeMaxInputField()
	{
		return this.maxWattage;
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06003021 RID: 12321 RVA: 0x0010A70F File Offset: 0x0010890F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.WATTAGESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06003022 RID: 12322 RVA: 0x0010A716 File Offset: 0x00108916
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06003023 RID: 12323 RVA: 0x0010A71D File Offset: 0x0010891D
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06003024 RID: 12324 RVA: 0x0010A729 File Offset: 0x00108929
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x0010A735 File Offset: 0x00108935
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Watts, units);
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x0010A73F File Offset: 0x0010893F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x0010A747 File Offset: 0x00108947
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x0010A74A File Offset: 0x0010894A
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.ELECTRICAL.WATT;
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06003029 RID: 12329 RVA: 0x0010A751 File Offset: 0x00108951
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x0600302A RID: 12330 RVA: 0x0010A754 File Offset: 0x00108954
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x0600302B RID: 12331 RVA: 0x0010A758 File Offset: 0x00108958
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(5f, 5f),
				new NonLinearSlider.Range(35f, 1000f),
				new NonLinearSlider.Range(50f, 3000f),
				new NonLinearSlider.Range(10f, this.maxWattage)
			};
		}
	}

	// Token: 0x04001C3B RID: 7227
	[Serialize]
	public float thresholdWattage;

	// Token: 0x04001C3C RID: 7228
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x04001C3D RID: 7229
	[Serialize]
	public bool dirty = true;

	// Token: 0x04001C3E RID: 7230
	private readonly float minWattage;

	// Token: 0x04001C3F RID: 7231
	private readonly float maxWattage = 1.5f * Wire.GetMaxWattageAsFloat(Wire.WattageRating.Max50000);

	// Token: 0x04001C40 RID: 7232
	private float currentWattage;

	// Token: 0x04001C41 RID: 7233
	private bool wasOn;

	// Token: 0x04001C42 RID: 7234
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001C43 RID: 7235
	private static readonly EventSystem.IntraObjectHandler<LogicWattageSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicWattageSensor>(delegate(LogicWattageSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
