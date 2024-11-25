using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000750 RID: 1872
[SerializationConfig(MemberSerialization.OptIn)]
public class PressureSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060031E5 RID: 12773 RVA: 0x001127C0 File Offset: 0x001109C0
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

	// Token: 0x060031E6 RID: 12774 RVA: 0x00112888 File Offset: 0x00110A88
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x060031E7 RID: 12775 RVA: 0x001128DB File Offset: 0x00110ADB
	// (set) Token: 0x060031E8 RID: 12776 RVA: 0x001128E3 File Offset: 0x00110AE3
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

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x060031E9 RID: 12777 RVA: 0x001128EC File Offset: 0x00110AEC
	// (set) Token: 0x060031EA RID: 12778 RVA: 0x001128F4 File Offset: 0x00110AF4
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

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x060031EB RID: 12779 RVA: 0x00112900 File Offset: 0x00110B00
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

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x060031EC RID: 12780 RVA: 0x00112931 File Offset: 0x00110B31
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x060031ED RID: 12781 RVA: 0x00112939 File Offset: 0x00110B39
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x00112941 File Offset: 0x00110B41
	public float GetRangeMinInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMin;
		}
		return this.rangeMin * 1000f;
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x0011295F File Offset: 0x00110B5F
	public float GetRangeMaxInputField()
	{
		if (this.desiredState != Element.State.Gas)
		{
			return this.rangeMax;
		}
		return this.rangeMax * 1000f;
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x060031F0 RID: 12784 RVA: 0x0011297D File Offset: 0x00110B7D
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x060031F1 RID: 12785 RVA: 0x00112984 File Offset: 0x00110B84
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x060031F2 RID: 12786 RVA: 0x0011298B File Offset: 0x00110B8B
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x060031F3 RID: 12787 RVA: 0x00112997 File Offset: 0x00110B97
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x001129A4 File Offset: 0x00110BA4
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

	// Token: 0x060031F5 RID: 12789 RVA: 0x001129D0 File Offset: 0x00110BD0
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

	// Token: 0x060031F6 RID: 12790 RVA: 0x001129FA File Offset: 0x00110BFA
	public float ProcessedInputValue(float input)
	{
		if (this.desiredState == Element.State.Gas)
		{
			input /= 1000f;
		}
		return input;
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x00112A0F File Offset: 0x00110C0F
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(this.desiredState == Element.State.Gas);
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x060031F8 RID: 12792 RVA: 0x00112A1F File Offset: 0x00110C1F
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700033A RID: 826
	// (get) Token: 0x060031F9 RID: 12793 RVA: 0x00112A22 File Offset: 0x00110C22
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x060031FA RID: 12794 RVA: 0x00112A25 File Offset: 0x00110C25
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001D63 RID: 7523
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04001D64 RID: 7524
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04001D65 RID: 7525
	public float rangeMin;

	// Token: 0x04001D66 RID: 7526
	public float rangeMax = 1f;

	// Token: 0x04001D67 RID: 7527
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04001D68 RID: 7528
	private const int WINDOW_SIZE = 8;

	// Token: 0x04001D69 RID: 7529
	private float[] samples = new float[8];

	// Token: 0x04001D6A RID: 7530
	private int sampleIdx;
}
