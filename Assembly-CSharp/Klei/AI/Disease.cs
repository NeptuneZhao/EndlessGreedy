﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F48 RID: 3912
	[DebuggerDisplay("{base.Id}")]
	public abstract class Disease : Resource
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06007864 RID: 30820 RVA: 0x002F9101 File Offset: 0x002F7301
		public new string Name
		{
			get
			{
				return Strings.Get(this.name);
			}
		}

		// Token: 0x06007865 RID: 30821 RVA: 0x002F9114 File Offset: 0x002F7314
		public Disease(string id, float strength, Disease.RangeInfo temperature_range, Disease.RangeInfo temperature_half_lives, Disease.RangeInfo pressure_range, Disease.RangeInfo pressure_half_lives, float radiation_kill_rate, bool statsOnly) : base(id, null, null)
		{
			this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
			this.id = id;
			this.temperatureRange = temperature_range;
			this.temperatureHalfLives = temperature_half_lives;
			this.pressureRange = pressure_range;
			this.pressureHalfLives = pressure_half_lives;
			this.radiationKillRate = radiation_kill_rate;
			this.PopulateElemGrowthInfo();
			this.ApplyRules();
			if (!statsOnly)
			{
				DiseaseVisualization.Info info = Assets.instance.DiseaseVisualization.GetInfo(id);
				this.overlayColourName = info.overlayColourName;
				string str = Strings.Get("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".LEGEND_HOVERTEXT").ToString();
				this.overlayLegendHovertext = str + DUPLICANTS.DISEASES.LEGEND_POSTAMBLE;
				Attribute attribute = new Attribute(id + "Min", "Minimum" + id.ToString(), "", "", 0f, Attribute.Display.Normal, false, null, null, null);
				Attribute attribute2 = new Attribute(id + "Max", "Maximum" + id.ToString(), "", "", 10000000f, Attribute.Display.Normal, false, null, null, null);
				this.amountDeltaAttribute = new Attribute(id + "Delta", id.ToString(), "", "", 0f, Attribute.Display.Normal, false, null, null, null);
				this.amount = new Amount(id, id + " " + DUPLICANTS.DISEASES.GERMS, id + " " + DUPLICANTS.DISEASES.GERMS, attribute, attribute2, this.amountDeltaAttribute, false, Units.Flat, 0.01f, true, null, null);
				Db.Get().Attributes.Add(attribute);
				Db.Get().Attributes.Add(attribute2);
				Db.Get().Attributes.Add(this.amountDeltaAttribute);
				this.cureSpeedBase = new Attribute(id + "CureSpeed", false, Attribute.Display.Normal, false, 0f, null, null, null, null);
				this.cureSpeedBase.BaseValue = 1f;
				this.cureSpeedBase.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
				Db.Get().Attributes.Add(this.cureSpeedBase);
			}
		}

		// Token: 0x06007866 RID: 30822 RVA: 0x002F9360 File Offset: 0x002F7560
		protected virtual void PopulateElemGrowthInfo()
		{
			this.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
			this.AddGrowthRule(new GrowthRule
			{
				underPopulationDeathRate = new float?(0f),
				minCountPerKG = new float?((float)100),
				populationHalfLife = new float?(float.PositiveInfinity),
				maxCountPerKG = new float?((float)1000),
				overPopulationHalfLife = new float?(float.PositiveInfinity),
				minDiffusionCount = new int?(1000),
				diffusionScale = new float?(0.001f),
				minDiffusionInfestationTickCount = new byte?((byte)1)
			});
			this.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
			this.AddExposureRule(new ExposureRule
			{
				populationHalfLife = new float?(float.PositiveInfinity)
			});
		}

		// Token: 0x06007867 RID: 30823 RVA: 0x002F9434 File Offset: 0x002F7634
		protected void AddGrowthRule(GrowthRule g)
		{
			if (this.growthRules == null)
			{
				this.growthRules = new List<GrowthRule>();
				global::Debug.Assert(g.GetType() == typeof(GrowthRule), "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.underPopulationDeathRate != null, "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.populationHalfLife != null, "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.overPopulationHalfLife != null, "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.diffusionScale != null, "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.minCountPerKG != null, "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.maxCountPerKG != null, "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.minDiffusionCount != null, "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.minDiffusionInfestationTickCount != null, "First rule must be a fully defined base rule.");
			}
			else
			{
				global::Debug.Assert(g.GetType() != typeof(GrowthRule), "Subsequent rules should not be base rules");
			}
			this.growthRules.Add(g);
		}

		// Token: 0x06007868 RID: 30824 RVA: 0x002F954C File Offset: 0x002F774C
		protected void AddExposureRule(ExposureRule g)
		{
			if (this.exposureRules == null)
			{
				this.exposureRules = new List<ExposureRule>();
				global::Debug.Assert(g.GetType() == typeof(ExposureRule), "First rule must be a fully defined base rule.");
				global::Debug.Assert(g.populationHalfLife != null, "First rule must be a fully defined base rule.");
			}
			else
			{
				global::Debug.Assert(g.GetType() != typeof(ExposureRule), "Subsequent rules should not be base rules");
			}
			this.exposureRules.Add(g);
		}

		// Token: 0x06007869 RID: 30825 RVA: 0x002F95D0 File Offset: 0x002F77D0
		public CompositeGrowthRule GetGrowthRuleForElement(Element e)
		{
			CompositeGrowthRule compositeGrowthRule = new CompositeGrowthRule();
			if (this.growthRules != null)
			{
				for (int i = 0; i < this.growthRules.Count; i++)
				{
					if (this.growthRules[i].Test(e))
					{
						compositeGrowthRule.Overlay(this.growthRules[i]);
					}
				}
			}
			return compositeGrowthRule;
		}

		// Token: 0x0600786A RID: 30826 RVA: 0x002F9628 File Offset: 0x002F7828
		public CompositeExposureRule GetExposureRuleForElement(Element e)
		{
			CompositeExposureRule compositeExposureRule = new CompositeExposureRule();
			if (this.exposureRules != null)
			{
				for (int i = 0; i < this.exposureRules.Count; i++)
				{
					if (this.exposureRules[i].Test(e))
					{
						compositeExposureRule.Overlay(this.exposureRules[i]);
					}
				}
			}
			return compositeExposureRule;
		}

		// Token: 0x0600786B RID: 30827 RVA: 0x002F9680 File Offset: 0x002F7880
		public TagGrowthRule GetGrowthRuleForTag(Tag t)
		{
			if (this.growthRules != null)
			{
				for (int i = 0; i < this.growthRules.Count; i++)
				{
					TagGrowthRule tagGrowthRule = this.growthRules[i] as TagGrowthRule;
					if (tagGrowthRule != null && tagGrowthRule.tag == t)
					{
						return tagGrowthRule;
					}
				}
			}
			return null;
		}

		// Token: 0x0600786C RID: 30828 RVA: 0x002F96D4 File Offset: 0x002F78D4
		protected void ApplyRules()
		{
			if (this.growthRules != null)
			{
				for (int i = 0; i < this.growthRules.Count; i++)
				{
					this.growthRules[i].Apply(this.elemGrowthInfo);
				}
			}
			if (this.exposureRules != null)
			{
				for (int j = 0; j < this.exposureRules.Count; j++)
				{
					this.exposureRules[j].Apply(this.elemExposureInfo);
				}
			}
		}

		// Token: 0x0600786D RID: 30829 RVA: 0x002F974C File Offset: 0x002F794C
		protected void InitializeElemGrowthArray(ref ElemGrowthInfo[] infoArray, ElemGrowthInfo default_value)
		{
			List<Element> elements = ElementLoader.elements;
			infoArray = new ElemGrowthInfo[elements.Count];
			for (int i = 0; i < elements.Count; i++)
			{
				infoArray[i] = default_value;
			}
			infoArray[(int)ElementLoader.GetElementIndex(SimHashes.Polypropylene)] = new ElemGrowthInfo
			{
				underPopulationDeathRate = 2.6666667f,
				populationHalfLife = 10f,
				overPopulationHalfLife = 10f,
				minCountPerKG = 0f,
				maxCountPerKG = float.PositiveInfinity,
				minDiffusionCount = int.MaxValue,
				diffusionScale = 1f,
				minDiffusionInfestationTickCount = byte.MaxValue
			};
			infoArray[(int)ElementLoader.GetElementIndex(SimHashes.Vacuum)] = new ElemGrowthInfo
			{
				underPopulationDeathRate = 0f,
				populationHalfLife = 0f,
				overPopulationHalfLife = 0f,
				minCountPerKG = 0f,
				maxCountPerKG = float.PositiveInfinity,
				diffusionScale = 0f,
				minDiffusionInfestationTickCount = byte.MaxValue
			};
		}

		// Token: 0x0600786E RID: 30830 RVA: 0x002F9870 File Offset: 0x002F7A70
		protected void InitializeElemExposureArray(ref ElemExposureInfo[] infoArray, ElemExposureInfo default_value)
		{
			List<Element> elements = ElementLoader.elements;
			infoArray = new ElemExposureInfo[elements.Count];
			for (int i = 0; i < elements.Count; i++)
			{
				infoArray[i] = default_value;
			}
		}

		// Token: 0x0600786F RID: 30831 RVA: 0x002F98AC File Offset: 0x002F7AAC
		public float GetGrowthRateForTags(HashSet<Tag> tags, bool overpopulated)
		{
			float num = 1f;
			if (this.growthRules != null)
			{
				for (int i = 0; i < this.growthRules.Count; i++)
				{
					TagGrowthRule tagGrowthRule = this.growthRules[i] as TagGrowthRule;
					if (tagGrowthRule != null && tags.Contains(tagGrowthRule.tag))
					{
						num *= Disease.HalfLifeToGrowthRate((overpopulated ? tagGrowthRule.overPopulationHalfLife : tagGrowthRule.populationHalfLife).Value, 1f);
					}
				}
			}
			return num;
		}

		// Token: 0x06007870 RID: 30832 RVA: 0x002F9928 File Offset: 0x002F7B28
		public static float HalfLifeToGrowthRate(float half_life_in_seconds, float dt)
		{
			float result;
			if (half_life_in_seconds == 0f)
			{
				result = 0f;
			}
			else if (half_life_in_seconds == float.PositiveInfinity)
			{
				result = 1f;
			}
			else
			{
				float num = half_life_in_seconds / dt;
				result = Mathf.Pow(2f, -1f / num);
			}
			return result;
		}

		// Token: 0x06007871 RID: 30833 RVA: 0x002F9974 File Offset: 0x002F7B74
		public static float GrowthRateToHalfLife(float growth_rate)
		{
			float result;
			if (growth_rate == 0f)
			{
				result = 0f;
			}
			else if (growth_rate == 1f)
			{
				result = float.PositiveInfinity;
			}
			else
			{
				result = Mathf.Log(2f, growth_rate);
			}
			return result;
		}

		// Token: 0x06007872 RID: 30834 RVA: 0x002F99B4 File Offset: 0x002F7BB4
		public float CalculateTemperatureHalfLife(float temperature)
		{
			return Disease.CalculateRangeHalfLife(temperature, ref this.temperatureRange, ref this.temperatureHalfLives);
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x002F99C8 File Offset: 0x002F7BC8
		public static float CalculateRangeHalfLife(float range_value, ref Disease.RangeInfo range, ref Disease.RangeInfo half_lives)
		{
			int num = 3;
			int num2 = 3;
			for (int i = 0; i < 4; i++)
			{
				if (range_value <= range.GetValue(i))
				{
					num = i - 1;
					num2 = i;
					break;
				}
			}
			if (num < 0)
			{
				num = num2;
			}
			float value = half_lives.GetValue(num);
			float value2 = half_lives.GetValue(num2);
			if (num == 1 && num2 == 2)
			{
				return float.PositiveInfinity;
			}
			if (float.IsInfinity(value) || float.IsInfinity(value2))
			{
				return float.PositiveInfinity;
			}
			float value3 = range.GetValue(num);
			float value4 = range.GetValue(num2);
			float t = 0f;
			float num3 = value4 - value3;
			if (num3 > 0f)
			{
				t = (range_value - value3) / num3;
			}
			return Mathf.Lerp(value, value2, t);
		}

		// Token: 0x06007874 RID: 30836 RVA: 0x002F9A70 File Offset: 0x002F7C70
		public List<Descriptor> GetQuantitativeDescriptors()
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.TEMPERATURE_RANGE, GameUtil.GetFormattedTemperature(this.temperatureRange.minViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(this.temperatureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.TEMPERATURE_RANGE_TOOLTIP, new object[]
			{
				GameUtil.GetFormattedTemperature(this.temperatureRange.minViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
				GameUtil.GetFormattedTemperature(this.temperatureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
				GameUtil.GetFormattedTemperature(this.temperatureRange.minGrowth, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
				GameUtil.GetFormattedTemperature(this.temperatureRange.maxGrowth, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)
			}), Descriptor.DescriptorType.Information, false));
			list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.PRESSURE_RANGE, GameUtil.GetFormattedMass(this.pressureRange.minViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.pressureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.PRESSURE_RANGE_TOOLTIP, new object[]
			{
				GameUtil.GetFormattedMass(this.pressureRange.minViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
				GameUtil.GetFormattedMass(this.pressureRange.maxViable, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
				GameUtil.GetFormattedMass(this.pressureRange.minGrowth, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
				GameUtil.GetFormattedMass(this.pressureRange.maxGrowth, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			}), Descriptor.DescriptorType.Information, false));
			List<GrowthRule> list2 = new List<GrowthRule>();
			List<GrowthRule> list3 = new List<GrowthRule>();
			List<GrowthRule> list4 = new List<GrowthRule>();
			List<GrowthRule> list5 = new List<GrowthRule>();
			List<GrowthRule> list6 = new List<GrowthRule>();
			foreach (GrowthRule growthRule in this.growthRules)
			{
				if (growthRule.populationHalfLife != null && growthRule.Name() != null)
				{
					if (growthRule.populationHalfLife.Value < 0f)
					{
						list2.Add(growthRule);
					}
					else if (growthRule.populationHalfLife.Value == float.PositiveInfinity)
					{
						list3.Add(growthRule);
					}
					else if (growthRule.populationHalfLife.Value >= 12000f)
					{
						list4.Add(growthRule);
					}
					else if (growthRule.populationHalfLife.Value >= 1200f)
					{
						list5.Add(growthRule);
					}
					else
					{
						list6.Add(growthRule);
					}
				}
			}
			list.AddRange(this.BuildGrowthInfoDescriptors(list2, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWS_ON, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWS_ON_TOOLTIP, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWS_TOOLTIP));
			list.AddRange(this.BuildGrowthInfoDescriptors(list3, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.NEUTRAL_ON, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.NEUTRAL_ON_TOOLTIP, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.NEUTRAL_TOOLTIP));
			list.AddRange(this.BuildGrowthInfoDescriptors(list4, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_SLOWLY_ON, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_SLOWLY_ON_TOOLTIP, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_SLOWLY_TOOLTIP));
			list.AddRange(this.BuildGrowthInfoDescriptors(list5, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_ON, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_ON_TOOLTIP, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_TOOLTIP));
			list.AddRange(this.BuildGrowthInfoDescriptors(list6, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_QUICKLY_ON, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_QUICKLY_ON_TOOLTIP, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.DIES_QUICKLY_TOOLTIP));
			return list;
		}

		// Token: 0x06007875 RID: 30837 RVA: 0x002F9DE8 File Offset: 0x002F7FE8
		private List<Descriptor> BuildGrowthInfoDescriptors(List<GrowthRule> rules, string section_text, string section_tooltip, string item_tooltip)
		{
			List<Descriptor> list = new List<Descriptor>();
			if (rules.Count > 0)
			{
				list.Add(new Descriptor(section_text, section_tooltip, Descriptor.DescriptorType.Information, false));
				for (int i = 0; i < rules.Count; i++)
				{
					list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.GROWTH_FORMAT, rules[i].Name()), string.Format(item_tooltip, GameUtil.GetFormattedCycles(Mathf.Abs(rules[i].populationHalfLife.Value), "F1", false)), Descriptor.DescriptorType.Information, false));
				}
			}
			return list;
		}

		// Token: 0x040059ED RID: 23021
		private StringKey name;

		// Token: 0x040059EE RID: 23022
		public HashedString id;

		// Token: 0x040059EF RID: 23023
		public float strength;

		// Token: 0x040059F0 RID: 23024
		public Disease.RangeInfo temperatureRange;

		// Token: 0x040059F1 RID: 23025
		public Disease.RangeInfo temperatureHalfLives;

		// Token: 0x040059F2 RID: 23026
		public Disease.RangeInfo pressureRange;

		// Token: 0x040059F3 RID: 23027
		public Disease.RangeInfo pressureHalfLives;

		// Token: 0x040059F4 RID: 23028
		public List<GrowthRule> growthRules;

		// Token: 0x040059F5 RID: 23029
		public List<ExposureRule> exposureRules;

		// Token: 0x040059F6 RID: 23030
		public ElemGrowthInfo[] elemGrowthInfo;

		// Token: 0x040059F7 RID: 23031
		public ElemExposureInfo[] elemExposureInfo;

		// Token: 0x040059F8 RID: 23032
		public string overlayColourName;

		// Token: 0x040059F9 RID: 23033
		public string overlayLegendHovertext;

		// Token: 0x040059FA RID: 23034
		public float radiationKillRate;

		// Token: 0x040059FB RID: 23035
		public Amount amount;

		// Token: 0x040059FC RID: 23036
		public Attribute amountDeltaAttribute;

		// Token: 0x040059FD RID: 23037
		public Attribute cureSpeedBase;

		// Token: 0x040059FE RID: 23038
		public static readonly ElemGrowthInfo DEFAULT_GROWTH_INFO = new ElemGrowthInfo
		{
			underPopulationDeathRate = 0f,
			populationHalfLife = float.PositiveInfinity,
			overPopulationHalfLife = float.PositiveInfinity,
			minCountPerKG = 0f,
			maxCountPerKG = float.PositiveInfinity,
			minDiffusionCount = 0,
			diffusionScale = 1f,
			minDiffusionInfestationTickCount = byte.MaxValue
		};

		// Token: 0x040059FF RID: 23039
		public static ElemExposureInfo DEFAULT_EXPOSURE_INFO = new ElemExposureInfo
		{
			populationHalfLife = float.PositiveInfinity
		};

		// Token: 0x02002337 RID: 9015
		public struct RangeInfo
		{
			// Token: 0x0600B5F6 RID: 46582 RVA: 0x003C8D00 File Offset: 0x003C6F00
			public RangeInfo(float min_viable, float min_growth, float max_growth, float max_viable)
			{
				this.minViable = min_viable;
				this.minGrowth = min_growth;
				this.maxGrowth = max_growth;
				this.maxViable = max_viable;
			}

			// Token: 0x0600B5F7 RID: 46583 RVA: 0x003C8D1F File Offset: 0x003C6F1F
			public void Write(BinaryWriter writer)
			{
				writer.Write(this.minViable);
				writer.Write(this.minGrowth);
				writer.Write(this.maxGrowth);
				writer.Write(this.maxViable);
			}

			// Token: 0x0600B5F8 RID: 46584 RVA: 0x003C8D51 File Offset: 0x003C6F51
			public float GetValue(int idx)
			{
				switch (idx)
				{
				case 0:
					return this.minViable;
				case 1:
					return this.minGrowth;
				case 2:
					return this.maxGrowth;
				case 3:
					return this.maxViable;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}

			// Token: 0x0600B5F9 RID: 46585 RVA: 0x003C8D8C File Offset: 0x003C6F8C
			public static Disease.RangeInfo Idempotent()
			{
				return new Disease.RangeInfo(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
			}

			// Token: 0x04009E11 RID: 40465
			public float minViable;

			// Token: 0x04009E12 RID: 40466
			public float minGrowth;

			// Token: 0x04009E13 RID: 40467
			public float maxGrowth;

			// Token: 0x04009E14 RID: 40468
			public float maxViable;
		}
	}
}
