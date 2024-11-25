using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000BD7 RID: 3031
public class StandardAttributeFormatter : IAttributeFormatter
{
	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06005C51 RID: 23633 RVA: 0x0021CB8F File Offset: 0x0021AD8F
	// (set) Token: 0x06005C52 RID: 23634 RVA: 0x0021CB97 File Offset: 0x0021AD97
	public GameUtil.TimeSlice DeltaTimeSlice { get; set; }

	// Token: 0x06005C53 RID: 23635 RVA: 0x0021CBA0 File Offset: 0x0021ADA0
	public StandardAttributeFormatter(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice)
	{
		this.unitClass = unitClass;
		this.DeltaTimeSlice = deltaTimeSlice;
	}

	// Token: 0x06005C54 RID: 23636 RVA: 0x0021CBB6 File Offset: 0x0021ADB6
	public virtual string GetFormattedAttribute(AttributeInstance instance)
	{
		return this.GetFormattedValue(instance.GetTotalDisplayValue(), GameUtil.TimeSlice.None);
	}

	// Token: 0x06005C55 RID: 23637 RVA: 0x0021CBC8 File Offset: 0x0021ADC8
	public virtual string GetFormattedModifier(AttributeModifier modifier)
	{
		return this.GetFormattedValue(modifier.Value, (modifier.OverrideTimeSlice != null) ? modifier.OverrideTimeSlice.Value : this.DeltaTimeSlice);
	}

	// Token: 0x06005C56 RID: 23638 RVA: 0x0021CC08 File Offset: 0x0021AE08
	public virtual string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
	{
		switch (this.unitClass)
		{
		case GameUtil.UnitClass.SimpleInteger:
			return GameUtil.GetFormattedInt(value, timeSlice);
		case GameUtil.UnitClass.Temperature:
			return GameUtil.GetFormattedTemperature(value, timeSlice, (timeSlice == GameUtil.TimeSlice.None) ? GameUtil.TemperatureInterpretation.Absolute : GameUtil.TemperatureInterpretation.Relative, true, false);
		case GameUtil.UnitClass.Mass:
			return GameUtil.GetFormattedMass(value, timeSlice, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		case GameUtil.UnitClass.Calories:
			return GameUtil.GetFormattedCalories(value, timeSlice, true);
		case GameUtil.UnitClass.Percent:
			return GameUtil.GetFormattedPercent(value, timeSlice);
		case GameUtil.UnitClass.Distance:
			return GameUtil.GetFormattedDistance(value);
		case GameUtil.UnitClass.Disease:
			return GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(value), GameUtil.TimeSlice.None);
		case GameUtil.UnitClass.Radiation:
			return GameUtil.GetFormattedRads(value, timeSlice);
		case GameUtil.UnitClass.Energy:
			return GameUtil.GetFormattedJoules(value, "F1", timeSlice);
		case GameUtil.UnitClass.Power:
			return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
		case GameUtil.UnitClass.Lux:
			return GameUtil.GetFormattedLux(Mathf.FloorToInt(value));
		case GameUtil.UnitClass.Time:
			return GameUtil.GetFormattedCycles(value, "F1", false);
		case GameUtil.UnitClass.Seconds:
			return GameUtil.GetFormattedTime(value, "F0");
		case GameUtil.UnitClass.Cycles:
			return GameUtil.GetFormattedCycles(value * 600f, "F1", false);
		}
		return GameUtil.GetFormattedSimple(value, timeSlice, null);
	}

	// Token: 0x06005C57 RID: 23639 RVA: 0x0021CD0E File Offset: 0x0021AF0E
	public virtual string GetTooltipDescription(Klei.AI.Attribute master)
	{
		return master.Description;
	}

	// Token: 0x06005C58 RID: 23640 RVA: 0x0021CD18 File Offset: 0x0021AF18
	public virtual string GetTooltip(Klei.AI.Attribute master, AttributeInstance instance)
	{
		List<AttributeModifier> list = new List<AttributeModifier>();
		for (int i = 0; i < instance.Modifiers.Count; i++)
		{
			list.Add(instance.Modifiers[i]);
		}
		return this.GetTooltip(master, list, instance.GetComponent<AttributeConverters>());
	}

	// Token: 0x06005C59 RID: 23641 RVA: 0x0021CD64 File Offset: 0x0021AF64
	public string GetTooltip(Klei.AI.Attribute master, List<AttributeModifier> modifiers, AttributeConverters converters)
	{
		string text = this.GetTooltipDescription(master);
		text += string.Format(DUPLICANTS.ATTRIBUTES.TOTAL_VALUE, this.GetFormattedValue(AttributeInstance.GetTotalDisplayValue(master, modifiers), GameUtil.TimeSlice.None), master.Name);
		if (master.BaseValue != 0f)
		{
			text += string.Format(DUPLICANTS.ATTRIBUTES.BASE_VALUE, master.BaseValue);
		}
		List<AttributeModifier> list = new List<AttributeModifier>(modifiers);
		list.Sort((AttributeModifier p1, AttributeModifier p2) => p2.Value.CompareTo(p1.Value));
		for (int num = 0; num != list.Count; num++)
		{
			AttributeModifier attributeModifier = list[num];
			string formattedString = attributeModifier.GetFormattedString();
			if (formattedString != null)
			{
				text += string.Format(DUPLICANTS.ATTRIBUTES.MODIFIER_ENTRY, attributeModifier.GetDescription(), formattedString);
			}
		}
		string text2 = "";
		if (converters != null && master.converters.Count > 0)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in converters.converters)
			{
				if (attributeConverterInstance.converter.attribute == master)
				{
					string text3 = attributeConverterInstance.DescriptionFromAttribute(attributeConverterInstance.Evaluate(), attributeConverterInstance.gameObject);
					if (text3 != null)
					{
						text2 = text2 + "\n" + text3;
					}
				}
			}
		}
		if (text2.Length > 0)
		{
			text = text + "\n" + text2;
		}
		return text;
	}

	// Token: 0x04003DA2 RID: 15778
	public GameUtil.UnitClass unitClass;
}
