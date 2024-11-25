using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006AB RID: 1707
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitTemperatureSensor : ConduitThresholdSensor, IThresholdSwitch
{
	// Token: 0x06002AEF RID: 10991 RVA: 0x000F177C File Offset: 0x000EF97C
	private void GetContentsTemperature(out float temperature, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			temperature = contents.temperature;
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		if (pickupable != null && pickupable.PrimaryElement.Mass > 0f)
		{
			temperature = pickupable.PrimaryElement.Temperature;
			hasMass = true;
			return;
		}
		temperature = 0f;
		hasMass = false;
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06002AF0 RID: 10992 RVA: 0x000F181C File Offset: 0x000EFA1C
	public override float CurrentValue
	{
		get
		{
			float num;
			bool flag;
			this.GetContentsTemperature(out num, out flag);
			if (flag)
			{
				this.lastValue = num;
			}
			return this.lastValue;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x000F1843 File Offset: 0x000EFA43
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06002AF2 RID: 10994 RVA: 0x000F184B File Offset: 0x000EFA4B
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06002AF3 RID: 10995 RVA: 0x000F1853 File Offset: 0x000EFA53
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06002AF4 RID: 10996 RVA: 0x000F1861 File Offset: 0x000EFA61
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x000F186F File Offset: 0x000EFA6F
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06002AF6 RID: 10998 RVA: 0x000F1876 File Offset: 0x000EFA76
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x000F187D File Offset: 0x000EFA7D
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000F1889 File Offset: 0x000EFA89
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.CONTENT_TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x000F1895 File Offset: 0x000EFA95
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x000F18A1 File Offset: 0x000EFAA1
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x000F18A9 File Offset: 0x000EFAA9
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x000F18B4 File Offset: 0x000EFAB4
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

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000F18F4 File Offset: 0x000EFAF4
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06002AFE RID: 11006 RVA: 0x000F18F7 File Offset: 0x000EFAF7
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06002AFF RID: 11007 RVA: 0x000F18FC File Offset: 0x000EFAFC
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

	// Token: 0x040018AE RID: 6318
	public float rangeMin;

	// Token: 0x040018AF RID: 6319
	public float rangeMax = 373.15f;

	// Token: 0x040018B0 RID: 6320
	[Serialize]
	private float lastValue;
}
