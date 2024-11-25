﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using UnityEngine;

// Token: 0x02000BC5 RID: 3013
public class AdditionalDetailsPanel : DetailScreenTab
{
	// Token: 0x06005BC4 RID: 23492 RVA: 0x0021740E File Offset: 0x0021560E
	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	// Token: 0x06005BC5 RID: 23493 RVA: 0x00217414 File Offset: 0x00215614
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.detailsPanel = base.CreateCollapsableSection(UI.DETAILTABS.DETAILS.GROUPNAME_DETAILS);
		this.drawer = new DetailsPanelDrawer(this.attributesLabelTemplate, this.detailsPanel.GetComponent<CollapsibleDetailContentPanel>().Content.gameObject);
		this.immuneSystemPanel = base.CreateCollapsableSection(UI.DETAILTABS.DISEASE.CONTRACTION_RATES);
		this.diseaseSourcePanel = base.CreateCollapsableSection(UI.DETAILTABS.DISEASE.DISEASE_SOURCE);
		this.currentGermsPanel = base.CreateCollapsableSection(UI.DETAILTABS.DISEASE.CURRENT_GERMS);
		this.overviewPanel = base.CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.CIRCUITOVERVIEW);
		this.generatorsPanel = base.CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.GENERATORS);
		this.consumersPanel = base.CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.CONSUMERS);
		this.batteriesPanel = base.CreateCollapsableSection(UI.DETAILTABS.ENERGYGENERATOR.BATTERIES);
		base.Subscribe<AdditionalDetailsPanel>(-1514841199, AdditionalDetailsPanel.OnRefreshDataDelegate);
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x0021750E File Offset: 0x0021570E
	private void OnRefreshData(object obj)
	{
		this.Refresh();
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x00217516 File Offset: 0x00215716
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06005BC8 RID: 23496 RVA: 0x0021751E File Offset: 0x0021571E
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		this.Refresh();
	}

	// Token: 0x06005BC9 RID: 23497 RVA: 0x00217530 File Offset: 0x00215730
	private void Refresh()
	{
		AdditionalDetailsPanel.RefreshDetailsPanel(this.detailsPanel, this.selectedTarget);
		AdditionalDetailsPanel.RefreshImuneSystemPanel(this.immuneSystemPanel, this.selectedTarget);
		AdditionalDetailsPanel.RefreshCurrentGermsPanel(this.currentGermsPanel, this.selectedTarget);
		AdditionalDetailsPanel.RefreshDiseaseSourcePanel(this.diseaseSourcePanel, this.selectedTarget);
		AdditionalDetailsPanel.RefreshEnergyOverviewPanel(this.overviewPanel, this.selectedTarget);
		AdditionalDetailsPanel.RefreshEnergyGeneratorPanel(this.generatorsPanel, this.selectedTarget);
		AdditionalDetailsPanel.RefreshEnergyConsumerPanel(this.consumersPanel, this.selectedTarget);
		AdditionalDetailsPanel.RefreshEnergyBatteriesPanel(this.batteriesPanel, this.selectedTarget);
	}

	// Token: 0x06005BCA RID: 23498 RVA: 0x002175C8 File Offset: 0x002157C8
	private static void RefreshDetailsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		PrimaryElement component = targetEntity.GetComponent<PrimaryElement>();
		CellSelectionObject component2 = targetEntity.GetComponent<CellSelectionObject>();
		float mass;
		float temperature;
		Element element;
		byte diseaseIdx;
		int diseaseCount;
		if (component != null)
		{
			mass = component.Mass;
			temperature = component.Temperature;
			element = component.Element;
			diseaseIdx = component.DiseaseIdx;
			diseaseCount = component.DiseaseCount;
		}
		else
		{
			if (!(component2 != null))
			{
				return;
			}
			mass = component2.Mass;
			temperature = component2.temperature;
			element = component2.element;
			diseaseIdx = component2.diseaseIdx;
			diseaseCount = component2.diseaseCount;
		}
		bool flag = element.id == SimHashes.Vacuum || element.id == SimHashes.Void;
		float specificHeatCapacity = element.specificHeatCapacity;
		float highTemp = element.highTemp;
		float lowTemp = element.lowTemp;
		BuildingComplete component3 = targetEntity.GetComponent<BuildingComplete>();
		float num;
		if (component3 != null)
		{
			num = component3.creationTime;
		}
		else
		{
			num = -1f;
		}
		LogicPorts component4 = targetEntity.GetComponent<LogicPorts>();
		EnergyConsumer component5 = targetEntity.GetComponent<EnergyConsumer>();
		Operational component6 = targetEntity.GetComponent<Operational>();
		Battery component7 = targetEntity.GetComponent<Battery>();
		targetPanel.SetLabel("element_name", string.Format(UI.ELEMENTAL.PRIMARYELEMENT.NAME, element.name), string.Format(UI.ELEMENTAL.PRIMARYELEMENT.TOOLTIP, element.name));
		targetPanel.SetLabel("element_mass", string.Format(UI.ELEMENTAL.MASS.NAME, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.ELEMENTAL.MASS.TOOLTIP, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
		if (num > 0f)
		{
			targetPanel.SetLabel("element_age", string.Format(UI.ELEMENTAL.AGE.NAME, Util.FormatTwoDecimalPlace((GameClock.Instance.GetTime() - num) / 600f)), string.Format(UI.ELEMENTAL.AGE.TOOLTIP, Util.FormatTwoDecimalPlace((GameClock.Instance.GetTime() - num) / 600f)));
		}
		int num_cycles = 5;
		float num2;
		float num3;
		float num4;
		if (component6 != null && (component4 != null || component5 != null || component7 != null))
		{
			num2 = component6.GetCurrentCycleUptime();
			num3 = component6.GetLastCycleUptime();
			num4 = component6.GetUptimeOverCycles(num_cycles);
		}
		else
		{
			num2 = -1f;
			num3 = -1f;
			num4 = -1f;
		}
		if (num2 >= 0f)
		{
			string text = UI.ELEMENTAL.UPTIME.NAME;
			text = text.Replace("{0}", "    • ");
			text = text.Replace("{1}", UI.ELEMENTAL.UPTIME.THIS_CYCLE);
			text = text.Replace("{2}", GameUtil.GetFormattedPercent(num2 * 100f, GameUtil.TimeSlice.None));
			text = text.Replace("{3}", UI.ELEMENTAL.UPTIME.LAST_CYCLE);
			text = text.Replace("{4}", GameUtil.GetFormattedPercent(num3 * 100f, GameUtil.TimeSlice.None));
			text = text.Replace("{5}", UI.ELEMENTAL.UPTIME.LAST_X_CYCLES.Replace("{0}", num_cycles.ToString()));
			text = text.Replace("{6}", GameUtil.GetFormattedPercent(num4 * 100f, GameUtil.TimeSlice.None));
			targetPanel.SetLabel("uptime_name", text, "");
		}
		if (!flag)
		{
			bool flag2 = false;
			float num5 = element.thermalConductivity;
			Building component8 = targetEntity.GetComponent<Building>();
			if (component8 != null)
			{
				num5 *= component8.Def.ThermalConductivity;
				flag2 = (component8.Def.ThermalConductivity < 1f);
			}
			string temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
			float shc = specificHeatCapacity * 1f;
			string text2 = string.Format(UI.ELEMENTAL.SHC.NAME, GameUtil.GetDisplaySHC(shc).ToString("0.000"));
			string text3 = UI.ELEMENTAL.SHC.TOOLTIP;
			text3 = text3.Replace("{SPECIFIC_HEAT_CAPACITY}", text2 + GameUtil.GetSHCSuffix());
			text3 = text3.Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
			string text4 = string.Format(UI.ELEMENTAL.THERMALCONDUCTIVITY.NAME, GameUtil.GetDisplayThermalConductivity(num5).ToString("0.000"));
			string text5 = UI.ELEMENTAL.THERMALCONDUCTIVITY.TOOLTIP;
			text5 = text5.Replace("{THERMAL_CONDUCTIVITY}", text4 + GameUtil.GetThermalConductivitySuffix());
			text5 = text5.Replace("{TEMPERATURE_UNIT}", temperatureUnitSuffix);
			targetPanel.SetLabel("temperature", string.Format(UI.ELEMENTAL.TEMPERATURE.NAME, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.ELEMENTAL.TEMPERATURE.TOOLTIP, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
			targetPanel.SetLabel("disease", string.Format(UI.ELEMENTAL.DISEASE.NAME, GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount, false)), string.Format(UI.ELEMENTAL.DISEASE.TOOLTIP, GameUtil.GetFormattedDisease(diseaseIdx, diseaseCount, true)));
			targetPanel.SetLabel("shc", text2, text3);
			targetPanel.SetLabel("tc", text4, text5);
			if (flag2)
			{
				targetPanel.SetLabel("insulated", UI.GAMEOBJECTEFFECTS.INSULATED.NAME, UI.GAMEOBJECTEFFECTS.INSULATED.TOOLTIP);
			}
		}
		if (element.IsSolid)
		{
			targetPanel.SetLabel("melting_point", string.Format(UI.ELEMENTAL.MELTINGPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.ELEMENTAL.MELTINGPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
			targetPanel.SetLabel("melting_point", string.Format(UI.ELEMENTAL.MELTINGPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.ELEMENTAL.MELTINGPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
			if (targetEntity.GetComponent<ElementChunk>() != null)
			{
				AttributeModifier attributeModifier = component.Element.attributeModifiers.Find((AttributeModifier m) => m.AttributeId == Db.Get().BuildingAttributes.OverheatTemperature.Id);
				if (attributeModifier != null)
				{
					targetPanel.SetLabel("overheat", string.Format(UI.ELEMENTAL.OVERHEATPOINT.NAME, attributeModifier.GetFormattedString()), string.Format(UI.ELEMENTAL.OVERHEATPOINT.TOOLTIP, attributeModifier.GetFormattedString()));
				}
			}
		}
		else if (element.IsLiquid)
		{
			targetPanel.SetLabel("freezepoint", string.Format(UI.ELEMENTAL.FREEZEPOINT.NAME, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.ELEMENTAL.FREEZEPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
			targetPanel.SetLabel("vapourizationpoint", string.Format(UI.ELEMENTAL.VAPOURIZATIONPOINT.NAME, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.ELEMENTAL.VAPOURIZATIONPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(highTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
		}
		else if (!flag)
		{
			targetPanel.SetLabel("dewpoint", string.Format(UI.ELEMENTAL.DEWPOINT.NAME, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.ELEMENTAL.DEWPOINT.TOOLTIP, GameUtil.GetFormattedTemperature(lowTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
		}
		if (DlcManager.FeatureRadiationEnabled())
		{
			string formattedPercent = GameUtil.GetFormattedPercent(GameUtil.GetRadiationAbsorptionPercentage(Grid.PosToCell(targetEntity)) * 100f, GameUtil.TimeSlice.None);
			targetPanel.SetLabel("radiationabsorption", string.Format(UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.NAME, formattedPercent), string.Format(UI.DETAILTABS.DETAILS.RADIATIONABSORPTIONFACTOR.TOOLTIP, formattedPercent));
		}
		Attributes attributes = targetEntity.GetAttributes();
		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				AttributeInstance attributeInstance = attributes.AttributeTable[i];
				if (DlcManager.IsDlcListValidForCurrentContent(attributeInstance.Attribute.DLCIds) && (attributeInstance.Attribute.ShowInUI == Klei.AI.Attribute.Display.Details || attributeInstance.Attribute.ShowInUI == Klei.AI.Attribute.Display.Expectation))
				{
					targetPanel.SetLabel(attributeInstance.modifier.Id, attributeInstance.modifier.Name + ": " + attributeInstance.GetFormattedValue(), attributeInstance.GetAttributeValueTooltip());
				}
			}
		}
		List<Descriptor> detailDescriptors = GameUtil.GetDetailDescriptors(GameUtil.GetAllDescriptors(targetEntity, false));
		for (int j = 0; j < detailDescriptors.Count; j++)
		{
			Descriptor descriptor = detailDescriptors[j];
			targetPanel.SetLabel("descriptor_" + j.ToString(), descriptor.text, descriptor.tooltipText);
		}
		targetPanel.Commit();
	}

	// Token: 0x06005BCB RID: 23499 RVA: 0x00217DE0 File Offset: 0x00215FE0
	private static void RefreshDiseaseSourcePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		List<Descriptor> list = GameUtil.GetAllDescriptors(targetEntity, true);
		Sicknesses sicknesses = targetEntity.GetSicknesses();
		if (sicknesses != null)
		{
			for (int i = 0; i < sicknesses.Count; i++)
			{
				list.AddRange(sicknesses[i].GetDescriptors());
			}
		}
		list = list.FindAll((Descriptor e) => e.type == Descriptor.DescriptorType.DiseaseSource);
		if (list.Count > 0)
		{
			for (int j = 0; j < list.Count; j++)
			{
				targetPanel.SetLabel("source_" + j.ToString(), list[j].text, list[j].tooltipText);
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06005BCC RID: 23500 RVA: 0x00217E98 File Offset: 0x00216098
	private static void RefreshCurrentGermsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity != null)
		{
			CellSelectionObject component = targetEntity.GetComponent<CellSelectionObject>();
			if (component != null)
			{
				if (component.diseaseIdx != 255 && component.diseaseCount > 0)
				{
					Disease disease = Db.Get().Diseases[(int)component.diseaseIdx];
					AdditionalDetailsPanel.BuildFactorsStrings(targetPanel, component.diseaseCount, component.element.idx, component.SelectedCell, component.Mass, component.temperature, null, disease, true);
				}
				else
				{
					targetPanel.SetLabel("currentgerms", UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
				}
			}
			else
			{
				PrimaryElement component2 = targetEntity.GetComponent<PrimaryElement>();
				if (component2 != null)
				{
					if (component2.DiseaseIdx != 255 && component2.DiseaseCount > 0)
					{
						Disease disease2 = Db.Get().Diseases[(int)component2.DiseaseIdx];
						int environmentCell = Grid.PosToCell(component2.transform.GetPosition());
						KPrefabID component3 = component2.GetComponent<KPrefabID>();
						AdditionalDetailsPanel.BuildFactorsStrings(targetPanel, component2.DiseaseCount, component2.Element.idx, environmentCell, component2.Mass, component2.Temperature, component3.Tags, disease2, false);
					}
					else
					{
						targetPanel.SetLabel("currentgerms", UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
					}
				}
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06005BCD RID: 23501 RVA: 0x00217FF4 File Offset: 0x002161F4
	private static void RefreshImuneSystemPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		GermExposureMonitor.Instance smi = targetEntity.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			targetPanel.SetLabel("germ_resistance", Db.Get().Attributes.GermResistance.Name + ": " + smi.GetGermResistance().ToString(), DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.DESC);
			for (int i = 0; i < Db.Get().Diseases.Count; i++)
			{
				Disease disease = Db.Get().Diseases[i];
				ExposureType exposureTypeForDisease = GameUtil.GetExposureTypeForDisease(disease);
				Sickness sicknessForDisease = GameUtil.GetSicknessForDisease(disease);
				if (sicknessForDisease != null)
				{
					bool flag = true;
					List<string> list = new List<string>();
					if (exposureTypeForDisease.required_traits != null && exposureTypeForDisease.required_traits.Count > 0)
					{
						for (int j = 0; j < exposureTypeForDisease.required_traits.Count; j++)
						{
							if (!targetEntity.GetComponent<Traits>().HasTrait(exposureTypeForDisease.required_traits[j]))
							{
								list.Add(exposureTypeForDisease.required_traits[j]);
							}
						}
						if (list.Count > 0)
						{
							flag = false;
						}
					}
					bool flag2 = false;
					List<string> list2 = new List<string>();
					if (exposureTypeForDisease.excluded_effects != null && exposureTypeForDisease.excluded_effects.Count > 0)
					{
						for (int k = 0; k < exposureTypeForDisease.excluded_effects.Count; k++)
						{
							if (targetEntity.GetComponent<Effects>().HasEffect(exposureTypeForDisease.excluded_effects[k]))
							{
								list2.Add(exposureTypeForDisease.excluded_effects[k]);
							}
						}
						if (list2.Count > 0)
						{
							flag2 = true;
						}
					}
					bool flag3 = false;
					List<string> list3 = new List<string>();
					if (exposureTypeForDisease.excluded_traits != null && exposureTypeForDisease.excluded_traits.Count > 0)
					{
						for (int l = 0; l < exposureTypeForDisease.excluded_traits.Count; l++)
						{
							if (targetEntity.GetComponent<Traits>().HasTrait(exposureTypeForDisease.excluded_traits[l]))
							{
								list3.Add(exposureTypeForDisease.excluded_traits[l]);
							}
						}
						if (list3.Count > 0)
						{
							flag3 = true;
						}
					}
					string text = "";
					float num;
					if (!flag)
					{
						num = 0f;
						string text2 = "";
						for (int m = 0; m < list.Count; m++)
						{
							if (text2 != "")
							{
								text2 += ", ";
							}
							text2 += Db.Get().traits.Get(list[m]).Name;
						}
						text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_MISSING_REQUIRED_TRAIT, text2);
					}
					else if (flag3)
					{
						num = 0f;
						string text3 = "";
						for (int n = 0; n < list3.Count; n++)
						{
							if (text3 != "")
							{
								text3 += ", ";
							}
							text3 += Db.Get().traits.Get(list3[n]).Name;
						}
						if (text != "")
						{
							text += "\n";
						}
						text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT, text3);
					}
					else if (flag2)
					{
						num = 0f;
						string text4 = "";
						for (int num2 = 0; num2 < list2.Count; num2++)
						{
							if (text4 != "")
							{
								text4 += ", ";
							}
							text4 += Db.Get().effects.Get(list2[num2]).Name;
						}
						if (text != "")
						{
							text += "\n";
						}
						text += string.Format(DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXCLUDED_EFFECT, text4);
					}
					else if (exposureTypeForDisease.infect_immediately)
					{
						num = 1f;
					}
					else
					{
						num = GermExposureMonitor.GetContractionChance(smi.GetResistanceToExposureType(exposureTypeForDisease, 3f));
					}
					string arg = (text != "") ? text : string.Format(DUPLICANTS.DISEASES.CONTRACTION_PROBABILITY, GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None), targetEntity.GetProperName(), sicknessForDisease.Name);
					targetPanel.SetLabel("disease_" + disease.Id, "    • " + disease.Name + ": " + GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None), string.Format(DUPLICANTS.DISEASES.RESISTANCES_PANEL_TOOLTIP, arg, sicknessForDisease.Name));
				}
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06005BCE RID: 23502 RVA: 0x002184A0 File Offset: 0x002166A0
	private static string GetFormattedHalfLife(float hl)
	{
		return AdditionalDetailsPanel.GetFormattedGrowthRate(Disease.HalfLifeToGrowthRate(hl, 600f));
	}

	// Token: 0x06005BCF RID: 23503 RVA: 0x002184B4 File Offset: 0x002166B4
	private static string GetFormattedGrowthRate(float rate)
	{
		if (rate < 1f)
		{
			return string.Format(UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT, GameUtil.GetFormattedPercent(100f * (1f - rate), GameUtil.TimeSlice.None), UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT_TOOLTIP);
		}
		if (rate > 1f)
		{
			return string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT, GameUtil.GetFormattedPercent(100f * (rate - 1f), GameUtil.TimeSlice.None), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT_TOOLTIP);
		}
		return string.Format(UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT, UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT_TOOLTIP);
	}

	// Token: 0x06005BD0 RID: 23504 RVA: 0x00218538 File Offset: 0x00216738
	private static string GetFormattedGrowthEntry(string name, float halfLife, string dyingFormat, string growingFormat, string neutralFormat)
	{
		string format;
		if (halfLife == float.PositiveInfinity)
		{
			format = neutralFormat;
		}
		else if (halfLife > 0f)
		{
			format = dyingFormat;
		}
		else
		{
			format = growingFormat;
		}
		return string.Format(format, name, AdditionalDetailsPanel.GetFormattedHalfLife(halfLife));
	}

	// Token: 0x06005BD1 RID: 23505 RVA: 0x00218570 File Offset: 0x00216770
	private static void BuildFactorsStrings(CollapsibleDetailContentPanel targetPanel, int diseaseCount, ushort elementIdx, int environmentCell, float environmentMass, float temperature, HashSet<Tag> tags, Disease disease, bool isCell = false)
	{
		targetPanel.SetTitle(string.Format(UI.DETAILTABS.DISEASE.CURRENT_GERMS, disease.Name.ToUpper()));
		targetPanel.SetLabel("currentgerms", string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT, disease.Name, GameUtil.GetFormattedDiseaseAmount(diseaseCount, GameUtil.TimeSlice.None)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT_TOOLTIP, GameUtil.GetFormattedDiseaseAmount(diseaseCount, GameUtil.TimeSlice.None)));
		Element e = ElementLoader.elements[(int)elementIdx];
		CompositeGrowthRule growthRuleForElement = disease.GetGrowthRuleForElement(e);
		float tags_multiplier_base = 1f;
		if (tags != null && tags.Count > 0)
		{
			tags_multiplier_base = disease.GetGrowthRateForTags(tags, (float)diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass);
		}
		float num = DiseaseContainers.CalculateDelta(diseaseCount, elementIdx, environmentMass, environmentCell, temperature, tags_multiplier_base, disease, 1f, Sim.IsRadiationEnabled());
		targetPanel.SetLabel("finaldelta", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE, GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE_TOOLTIP, GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.PerSecond, "F0")));
		float num2 = Disease.GrowthRateToHalfLife(1f - num / (float)diseaseCount);
		if (num2 > 0f)
		{
			targetPanel.SetLabel("finalhalflife", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG, GameUtil.GetFormattedCycles(num2, "F1", false)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG_TOOLTIP, GameUtil.GetFormattedCycles(num2, "F1", false)));
		}
		else if (num2 < 0f)
		{
			targetPanel.SetLabel("finalhalflife", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS, GameUtil.GetFormattedCycles(-num2, "F1", false)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS_TOOLTIP, GameUtil.GetFormattedCycles(num2, "F1", false)));
		}
		else
		{
			targetPanel.SetLabel("finalhalflife", UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL_TOOLTIP);
		}
		targetPanel.SetLabel("factors", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TITLE, Array.Empty<object>()), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TOOLTIP);
		bool flag = false;
		if ((float)diseaseCount < growthRuleForElement.minCountPerKG * environmentMass)
		{
			targetPanel.SetLabel("critical_status", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TITLE, AdditionalDetailsPanel.GetFormattedGrowthRate(-growthRuleForElement.underPopulationDeathRate)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TOOLTIP, GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.minCountPerKG * environmentMass), GameUtil.TimeSlice.None), GameUtil.GetFormattedMass(environmentMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), growthRuleForElement.minCountPerKG));
			flag = true;
		}
		else if ((float)diseaseCount > growthRuleForElement.maxCountPerKG * environmentMass)
		{
			targetPanel.SetLabel("critical_status", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TITLE, AdditionalDetailsPanel.GetFormattedHalfLife(growthRuleForElement.overPopulationHalfLife)), string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TOOLTIP, GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.maxCountPerKG * environmentMass), GameUtil.TimeSlice.None), GameUtil.GetFormattedMass(environmentMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), growthRuleForElement.maxCountPerKG));
			flag = true;
		}
		if (!flag)
		{
			targetPanel.SetLabel("substrate", AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
		}
		int num3 = 0;
		if (tags != null)
		{
			foreach (Tag t in tags)
			{
				TagGrowthRule growthRuleForTag = disease.GetGrowthRuleForTag(t);
				if (growthRuleForTag != null)
				{
					targetPanel.SetLabel("tag_" + num3.ToString(), AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), AdditionalDetailsPanel.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
				}
				num3++;
			}
		}
		if (Grid.IsValidCell(environmentCell))
		{
			if (!isCell)
			{
				CompositeExposureRule exposureRuleForElement = disease.GetExposureRuleForElement(Grid.Element[environmentCell]);
				if (exposureRuleForElement != null && exposureRuleForElement.populationHalfLife != float.PositiveInfinity)
				{
					if (exposureRuleForElement.GetHalfLifeForCount(diseaseCount) > 0f)
					{
						targetPanel.SetLabel("environment", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, exposureRuleForElement.Name(), AdditionalDetailsPanel.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.DIE_TOOLTIP);
					}
					else
					{
						targetPanel.SetLabel("environment", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, exposureRuleForElement.Name(), AdditionalDetailsPanel.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.GROW_TOOLTIP);
					}
				}
			}
			if (Sim.IsRadiationEnabled())
			{
				float num4 = Grid.Radiation[environmentCell];
				if (num4 > 0f)
				{
					float num5 = disease.radiationKillRate * num4;
					float hl = (float)diseaseCount * 0.5f / num5;
					targetPanel.SetLabel("radiation", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.TITLE, Mathf.RoundToInt(num4), AdditionalDetailsPanel.GetFormattedHalfLife(hl)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RADIATION.DIE_TOOLTIP);
				}
			}
		}
		float num6 = disease.CalculateTemperatureHalfLife(temperature);
		if (num6 != float.PositiveInfinity)
		{
			if (num6 > 0f)
			{
				targetPanel.SetLabel("temperature", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), AdditionalDetailsPanel.GetFormattedHalfLife(num6)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.DIE_TOOLTIP);
				return;
			}
			targetPanel.SetLabel("temperature", string.Format(UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), AdditionalDetailsPanel.GetFormattedHalfLife(num6)), UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.GROW_TOOLTIP);
		}
	}

	// Token: 0x06005BD2 RID: 23506 RVA: 0x00218B60 File Offset: 0x00216D60
	private static void RefreshEnergyOverviewPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			return;
		}
		if (targetEntity.GetComponent<ICircuitConnected>() != null || targetEntity.GetComponent<Wire>() != null)
		{
			ushort selectedTargetCircuitID = AdditionalDetailsPanel.GetSelectedTargetCircuitID(targetEntity);
			if (selectedTargetCircuitID == 65535)
			{
				targetPanel.SetLabel("nocircuit", UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED, UI.DETAILTABS.ENERGYGENERATOR.DISCONNECTED);
			}
			else
			{
				float joulesAvailableOnCircuit = Game.Instance.circuitManager.GetJoulesAvailableOnCircuit(selectedTargetCircuitID);
				targetPanel.SetLabel("joulesAvailable", string.Format(UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES, GameUtil.GetFormattedJoules(joulesAvailableOnCircuit, "F1", GameUtil.TimeSlice.None)), UI.DETAILTABS.ENERGYGENERATOR.AVAILABLE_JOULES_TOOLTIP);
				float wattsGeneratedByCircuit = Game.Instance.circuitManager.GetWattsGeneratedByCircuit(selectedTargetCircuitID);
				float potentialWattsGeneratedByCircuit = Game.Instance.circuitManager.GetPotentialWattsGeneratedByCircuit(selectedTargetCircuitID);
				string arg;
				if (wattsGeneratedByCircuit == potentialWattsGeneratedByCircuit)
				{
					arg = GameUtil.GetFormattedWattage(wattsGeneratedByCircuit, GameUtil.WattageFormatterUnit.Automatic, true);
				}
				else
				{
					arg = string.Format("{0} / {1}", GameUtil.GetFormattedWattage(wattsGeneratedByCircuit, GameUtil.WattageFormatterUnit.Automatic, true), GameUtil.GetFormattedWattage(potentialWattsGeneratedByCircuit, GameUtil.WattageFormatterUnit.Automatic, true));
				}
				targetPanel.SetLabel("wattageGenerated", string.Format(UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED, arg), UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_GENERATED_TOOLTIP);
				targetPanel.SetLabel("wattageConsumed", string.Format(UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED, GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetWattsUsedByCircuit(selectedTargetCircuitID), GameUtil.WattageFormatterUnit.Automatic, true)), UI.DETAILTABS.ENERGYGENERATOR.WATTAGE_CONSUMED_TOOLTIP);
				targetPanel.SetLabel("potentialWattageConsumed", string.Format(UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED, GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetWattsNeededWhenActive(selectedTargetCircuitID), GameUtil.WattageFormatterUnit.Automatic, true)), UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED_TOOLTIP);
				targetPanel.SetLabel("maxSafeWattage", string.Format(UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE, GameUtil.GetFormattedWattage(Game.Instance.circuitManager.GetMaxSafeWattageForCircuit(selectedTargetCircuitID), GameUtil.WattageFormatterUnit.Automatic, true)), UI.DETAILTABS.ENERGYGENERATOR.MAX_SAFE_WATTAGE_TOOLTIP);
			}
		}
		targetPanel.Commit();
	}

	// Token: 0x06005BD3 RID: 23507 RVA: 0x00218D34 File Offset: 0x00216F34
	private static void RefreshEnergyGeneratorPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			return;
		}
		ushort selectedTargetCircuitID = AdditionalDetailsPanel.GetSelectedTargetCircuitID(targetEntity);
		if (selectedTargetCircuitID == 65535)
		{
			targetPanel.SetActive(false);
			return;
		}
		targetPanel.SetActive(true);
		ReadOnlyCollection<Generator> generatorsOnCircuit = Game.Instance.circuitManager.GetGeneratorsOnCircuit(selectedTargetCircuitID);
		if (generatorsOnCircuit.Count > 0)
		{
			using (IEnumerator<Generator> enumerator = generatorsOnCircuit.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Generator generator = enumerator.Current;
					if (generator != null && generator.GetComponent<Battery>() == null)
					{
						string text;
						if (generator.IsProducingPower())
						{
							text = string.Format("{0}: {1}", generator.GetComponent<KSelectable>().entityName, GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
						}
						else
						{
							text = string.Format("{0}: {1} / {2}", generator.GetComponent<KSelectable>().entityName, GameUtil.GetFormattedWattage(0f, GameUtil.WattageFormatterUnit.Automatic, true), GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
						}
						text = ((generator.gameObject == targetEntity) ? ("<b>" + text + "</b>") : text);
						targetPanel.SetLabel(generator.gameObject.GetInstanceID().ToString(), text, "");
					}
				}
				goto IL_151;
			}
		}
		targetPanel.SetLabel("nogenerators", UI.DETAILTABS.ENERGYGENERATOR.NOGENERATORS, "");
		IL_151:
		targetPanel.Commit();
	}

	// Token: 0x06005BD4 RID: 23508 RVA: 0x00218EA8 File Offset: 0x002170A8
	private static void RefreshEnergyConsumerPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		AdditionalDetailsPanel.<>c__DisplayClass27_0 CS$<>8__locals1;
		CS$<>8__locals1.targetEntity = targetEntity;
		CS$<>8__locals1.targetPanel = targetPanel;
		if (CS$<>8__locals1.targetEntity == null)
		{
			return;
		}
		ushort selectedTargetCircuitID = AdditionalDetailsPanel.GetSelectedTargetCircuitID(CS$<>8__locals1.targetEntity);
		if (selectedTargetCircuitID == 65535)
		{
			CS$<>8__locals1.targetPanel.SetActive(false);
			return;
		}
		CS$<>8__locals1.targetPanel.SetActive(true);
		ReadOnlyCollection<IEnergyConsumer> consumersOnCircuit = Game.Instance.circuitManager.GetConsumersOnCircuit(selectedTargetCircuitID);
		ReadOnlyCollection<Battery> transformersOnCircuit = Game.Instance.circuitManager.GetTransformersOnCircuit(selectedTargetCircuitID);
		if (consumersOnCircuit.Count > 0 || transformersOnCircuit.Count > 0)
		{
			foreach (IEnergyConsumer consumer in consumersOnCircuit)
			{
				AdditionalDetailsPanel.<RefreshEnergyConsumerPanel>g__AddConsumerInfo|27_0(consumer, ref CS$<>8__locals1);
			}
			using (IEnumerator<Battery> enumerator2 = transformersOnCircuit.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Battery consumer2 = enumerator2.Current;
					AdditionalDetailsPanel.<RefreshEnergyConsumerPanel>g__AddConsumerInfo|27_0(consumer2, ref CS$<>8__locals1);
				}
				goto IL_FD;
			}
		}
		CS$<>8__locals1.targetPanel.SetLabel("noconsumers", UI.DETAILTABS.ENERGYGENERATOR.NOCONSUMERS, "");
		IL_FD:
		CS$<>8__locals1.targetPanel.Commit();
	}

	// Token: 0x06005BD5 RID: 23509 RVA: 0x00218FDC File Offset: 0x002171DC
	private static void RefreshEnergyBatteriesPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (targetEntity == null)
		{
			return;
		}
		ushort selectedTargetCircuitID = AdditionalDetailsPanel.GetSelectedTargetCircuitID(targetEntity);
		if (selectedTargetCircuitID == 65535)
		{
			targetPanel.SetActive(false);
			return;
		}
		targetPanel.SetActive(true);
		List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(selectedTargetCircuitID);
		if (batteriesOnCircuit.Count > 0)
		{
			using (List<Battery>.Enumerator enumerator = batteriesOnCircuit.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Battery battery = enumerator.Current;
					if (battery != null)
					{
						string text = string.Format("{0}: {1}", battery.GetComponent<KSelectable>().entityName, GameUtil.GetFormattedJoules(battery.JoulesAvailable, "F1", GameUtil.TimeSlice.None));
						text = ((battery.gameObject == targetEntity) ? ("<b>" + text + "</b>") : text);
						targetPanel.SetLabel(battery.gameObject.GetInstanceID().ToString(), text, "");
					}
				}
				goto IL_103;
			}
		}
		targetPanel.SetLabel("nobatteries", UI.DETAILTABS.ENERGYGENERATOR.NOBATTERIES, "");
		IL_103:
		targetPanel.Commit();
	}

	// Token: 0x06005BD6 RID: 23510 RVA: 0x00219104 File Offset: 0x00217304
	private static ushort GetSelectedTargetCircuitID(GameObject targetEntity)
	{
		CircuitManager circuitManager = Game.Instance.circuitManager;
		ICircuitConnected component = targetEntity.GetComponent<ICircuitConnected>();
		ushort result = ushort.MaxValue;
		if (component != null)
		{
			result = Game.Instance.circuitManager.GetCircuitID(component);
		}
		else if (targetEntity.GetComponent<Wire>() != null)
		{
			int cell = Grid.PosToCell(targetEntity.transform.GetPosition());
			result = Game.Instance.circuitManager.GetCircuitID(cell);
		}
		return result;
	}

	// Token: 0x06005BD9 RID: 23513 RVA: 0x00219194 File Offset: 0x00217394
	[CompilerGenerated]
	internal static void <RefreshEnergyConsumerPanel>g__AddConsumerInfo|27_0(IEnergyConsumer consumer, ref AdditionalDetailsPanel.<>c__DisplayClass27_0 A_1)
	{
		KMonoBehaviour kmonoBehaviour = consumer as KMonoBehaviour;
		if (kmonoBehaviour != null)
		{
			float wattsUsed = consumer.WattsUsed;
			float wattsNeededWhenActive = consumer.WattsNeededWhenActive;
			string arg;
			if (wattsUsed == wattsNeededWhenActive)
			{
				arg = GameUtil.GetFormattedWattage(wattsUsed, GameUtil.WattageFormatterUnit.Automatic, true);
			}
			else
			{
				arg = string.Format("{0} / {1}", GameUtil.GetFormattedWattage(wattsUsed, GameUtil.WattageFormatterUnit.Automatic, true), GameUtil.GetFormattedWattage(wattsNeededWhenActive, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			string text = string.Format("{0}: {1}", consumer.Name, arg);
			text = ((kmonoBehaviour.gameObject == A_1.targetEntity) ? ("<b>" + text + "</b>") : text);
			A_1.targetPanel.SetLabel(kmonoBehaviour.gameObject.GetInstanceID().ToString(), text, "");
		}
	}

	// Token: 0x04003D72 RID: 15730
	public GameObject attributesLabelTemplate;

	// Token: 0x04003D73 RID: 15731
	private CollapsibleDetailContentPanel detailsPanel;

	// Token: 0x04003D74 RID: 15732
	private DetailsPanelDrawer drawer;

	// Token: 0x04003D75 RID: 15733
	private CollapsibleDetailContentPanel immuneSystemPanel;

	// Token: 0x04003D76 RID: 15734
	private CollapsibleDetailContentPanel diseaseSourcePanel;

	// Token: 0x04003D77 RID: 15735
	private CollapsibleDetailContentPanel currentGermsPanel;

	// Token: 0x04003D78 RID: 15736
	private CollapsibleDetailContentPanel overviewPanel;

	// Token: 0x04003D79 RID: 15737
	private CollapsibleDetailContentPanel generatorsPanel;

	// Token: 0x04003D7A RID: 15738
	private CollapsibleDetailContentPanel consumersPanel;

	// Token: 0x04003D7B RID: 15739
	private CollapsibleDetailContentPanel batteriesPanel;

	// Token: 0x04003D7C RID: 15740
	private static readonly EventSystem.IntraObjectHandler<AdditionalDetailsPanel> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<AdditionalDetailsPanel>(delegate(AdditionalDetailsPanel component, object data)
	{
		component.OnRefreshData(data);
	});
}
