using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000784 RID: 1924
[SerializationConfig(MemberSerialization.OptIn)]
public class TemperatureControlledSwitch : CircuitSwitch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06003458 RID: 13400 RVA: 0x0011DA5C File Offset: 0x0011BC5C
	public float StructureTemperature
	{
		get
		{
			return GameComps.StructureTemperatures.GetPayload(this.structureTemperature).Temperature;
		}
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x0011DA81 File Offset: 0x0011BC81
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x0011DAA0 File Offset: 0x0011BCA0
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8)
		{
			this.temperatures[this.simUpdateCounter] = Grid.Temperature[Grid.PosToCell(this)];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.averageTemp = 0f;
		for (int i = 0; i < 8; i++)
		{
			this.averageTemp += this.temperatures[i];
		}
		this.averageTemp /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageTemp > this.thresholdTemperature && !base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageTemp > this.thresholdTemperature && base.IsSwitchedOn) || (this.averageTemp < this.thresholdTemperature && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x0011DB94 File Offset: 0x0011BD94
	public float GetTemperature()
	{
		return this.averageTemp;
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x0600345C RID: 13404 RVA: 0x0011DB9C File Offset: 0x0011BD9C
	// (set) Token: 0x0600345D RID: 13405 RVA: 0x0011DBA4 File Offset: 0x0011BDA4
	public float Threshold
	{
		get
		{
			return this.thresholdTemperature;
		}
		set
		{
			this.thresholdTemperature = value;
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x0600345E RID: 13406 RVA: 0x0011DBAD File Offset: 0x0011BDAD
	// (set) Token: 0x0600345F RID: 13407 RVA: 0x0011DBB5 File Offset: 0x0011BDB5
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06003460 RID: 13408 RVA: 0x0011DBBE File Offset: 0x0011BDBE
	public float CurrentValue
	{
		get
		{
			return this.GetTemperature();
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06003461 RID: 13409 RVA: 0x0011DBC6 File Offset: 0x0011BDC6
	public float RangeMin
	{
		get
		{
			return this.minTemp;
		}
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06003462 RID: 13410 RVA: 0x0011DBCE File Offset: 0x0011BDCE
	public float RangeMax
	{
		get
		{
			return this.maxTemp;
		}
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x0011DBD6 File Offset: 0x0011BDD6
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x0011DBE4 File Offset: 0x0011BDE4
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06003465 RID: 13413 RVA: 0x0011DBF2 File Offset: 0x0011BDF2
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.TEMPERATURESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06003466 RID: 13414 RVA: 0x0011DBF9 File Offset: 0x0011BDF9
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE;
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06003467 RID: 13415 RVA: 0x0011DC00 File Offset: 0x0011BE00
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06003468 RID: 13416 RVA: 0x0011DC0C File Offset: 0x0011BE0C
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TEMPERATURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x0011DC18 File Offset: 0x0011BE18
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, units, false);
	}

	// Token: 0x0600346A RID: 13418 RVA: 0x0011DC24 File Offset: 0x0011BE24
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x0011DC2C File Offset: 0x0011BE2C
	public float ProcessedInputValue(float input)
	{
		return GameUtil.GetTemperatureConvertedToKelvin(input);
	}

	// Token: 0x0600346C RID: 13420 RVA: 0x0011DC34 File Offset: 0x0011BE34
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

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x0600346D RID: 13421 RVA: 0x0011DC74 File Offset: 0x0011BE74
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.InputField;
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x0600346E RID: 13422 RVA: 0x0011DC77 File Offset: 0x0011BE77
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000396 RID: 918
	// (get) Token: 0x0600346F RID: 13423 RVA: 0x0011DC7A File Offset: 0x0011BE7A
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x04001EE9 RID: 7913
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04001EEA RID: 7914
	private int simUpdateCounter;

	// Token: 0x04001EEB RID: 7915
	[Serialize]
	public float thresholdTemperature = 280f;

	// Token: 0x04001EEC RID: 7916
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04001EED RID: 7917
	public float minTemp;

	// Token: 0x04001EEE RID: 7918
	public float maxTemp = 373.15f;

	// Token: 0x04001EEF RID: 7919
	private const int NumFrameDelay = 8;

	// Token: 0x04001EF0 RID: 7920
	private float[] temperatures = new float[8];

	// Token: 0x04001EF1 RID: 7921
	private float averageTemp;
}
