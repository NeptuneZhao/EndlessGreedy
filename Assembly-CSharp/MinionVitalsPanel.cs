﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CE5 RID: 3301
[AddComponentMenu("KMonoBehaviour/scripts/MinionVitalsPanel")]
public class MinionVitalsPanel : CollapsibleDetailContentPanel
{
	// Token: 0x06006618 RID: 26136 RVA: 0x00261610 File Offset: 0x0025F810
	public void Init()
	{
		this.AddAmountLine(Db.Get().Amounts.HitPoints, null);
		this.AddAmountLine(Db.Get().Amounts.BionicInternalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.BionicOil, null);
		this.AddAmountLine(Db.Get().Amounts.BionicGunk, null);
		this.AddAttributeLine(Db.Get().CritterAttributes.Happiness, null);
		this.AddAmountLine(Db.Get().Amounts.Wildness, null);
		this.AddAmountLine(Db.Get().Amounts.Incubation, null);
		this.AddAmountLine(Db.Get().Amounts.Viability, null);
		this.AddAmountLine(Db.Get().Amounts.PowerCharge, null);
		this.AddAmountLine(Db.Get().Amounts.Fertility, null);
		this.AddAmountLine(Db.Get().Amounts.Beckoning, null);
		this.AddAmountLine(Db.Get().Amounts.Age, null);
		this.AddAmountLine(Db.Get().Amounts.Stress, null);
		this.AddAttributeLine(Db.Get().Attributes.QualityOfLife, null);
		this.AddAmountLine(Db.Get().Amounts.Bladder, null);
		this.AddAmountLine(Db.Get().Amounts.Breath, null);
		this.AddAmountLine(Db.Get().Amounts.BionicOxygenTank, null);
		this.AddAmountLine(Db.Get().Amounts.Stamina, null);
		this.AddAttributeLine(Db.Get().CritterAttributes.Metabolism, null);
		this.AddAmountLine(Db.Get().Amounts.Calories, null);
		this.AddAmountLine(Db.Get().Amounts.ScaleGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.MilkProduction, null);
		this.AddAmountLine(Db.Get().Amounts.ElementGrowth, null);
		this.AddAmountLine(Db.Get().Amounts.Temperature, null);
		this.AddAmountLine(Db.Get().Amounts.CritterTemperature, null);
		this.AddAmountLine(Db.Get().Amounts.Decor, null);
		this.AddAmountLine(Db.Get().Amounts.InternalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalChemicalBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalBioBattery, null);
		this.AddAmountLine(Db.Get().Amounts.InternalElectroBank, null);
		if (DlcManager.FeatureRadiationEnabled())
		{
			this.AddAmountLine(Db.Get().Amounts.RadiationBalance, null);
		}
		this.AddCheckboxLine(Db.Get().Amounts.AirPressure, this.conditionsContainerNormal, (GameObject go) => this.GetAirPressureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().pressure_sensitive)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_pressure(go), (GameObject go) => this.GetAirPressureTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetAtmosphereLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<PressureVulnerable>() != null && go.GetComponent<PressureVulnerable>().safe_atmospheres.Count > 0)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_atmosphere(go), (GameObject go) => this.GetAtmosphereTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Temperature, this.conditionsContainerNormal, (GameObject go) => this.GetInternalTemperatureLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<TemperatureVulnerable>() != null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_temperature(go), (GameObject go) => this.GetInternalTemperatureTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Fertilization, this.conditionsContainerAdditional, (GameObject go) => this.GetFertilizationLabel(go), delegate(GameObject go)
		{
			if (go.GetComponent<ReceptacleMonitor>() == null)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			}
			if (go.GetComponent<ReceptacleMonitor>().Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
		}, (GameObject go) => this.check_fertilizer(go), (GameObject go) => this.GetFertilizationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Irrigation, this.conditionsContainerAdditional, (GameObject go) => this.GetIrrigationLabel(go), delegate(GameObject go)
		{
			ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
			if (!(component != null) || !component.Replanted)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Diminished;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
		}, (GameObject go) => this.check_irrigation(go), (GameObject go) => this.GetIrrigationTooltip(go));
		this.AddCheckboxLine(Db.Get().Amounts.Illumination, this.conditionsContainerNormal, (GameObject go) => this.GetIlluminationLabel(go), (GameObject go) => MinionVitalsPanel.CheckboxLineDisplayType.Normal, (GameObject go) => this.check_illumination(go), (GameObject go) => this.GetIlluminationTooltip(go));
		this.AddCheckboxLine(null, this.conditionsContainerNormal, (GameObject go) => this.GetRadiationLabel(go), delegate(GameObject go)
		{
			AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
			if (attributeInstance != null && attributeInstance.GetTotalValue() > 0f)
			{
				return MinionVitalsPanel.CheckboxLineDisplayType.Normal;
			}
			return MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
		}, (GameObject go) => this.check_radiation(go), (GameObject go) => this.GetRadiationTooltip(go));
	}

	// Token: 0x06006619 RID: 26137 RVA: 0x00261B44 File Offset: 0x0025FD44
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Init();
	}

	// Token: 0x0600661A RID: 26138 RVA: 0x00261B52 File Offset: 0x0025FD52
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x0600661B RID: 26139 RVA: 0x00261B66 File Offset: 0x0025FD66
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600661C RID: 26140 RVA: 0x00261B7C File Offset: 0x0025FD7C
	private void AddAmountLine(Amount amount, Func<AmountInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(amount.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AmountLine item = default(MinionVitalsPanel.AmountLine);
		item.amount = amount;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		item.imageToggle = gameObject.GetComponentInChildren<ValueTrendImageToggle>();
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AmountInstance, string>(amount.GetTooltip));
		this.amountsLines.Add(item);
	}

	// Token: 0x0600661D RID: 26141 RVA: 0x00261C34 File Offset: 0x0025FE34
	private void AddAttributeLine(Klei.AI.Attribute attribute, Func<AttributeInstance, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.LineItemPrefab, this.Content.gameObject, false);
		gameObject.GetComponentInChildren<Image>().sprite = Assets.GetSprite(attribute.uiSprite);
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.AttributeLine item = default(MinionVitalsPanel.AttributeLine);
		item.attribute = attribute;
		item.go = gameObject;
		item.locText = gameObject.GetComponentInChildren<LocText>();
		item.toolTip = gameObject.GetComponentInChildren<ToolTip>();
		gameObject.GetComponentInChildren<ValueTrendImageToggle>().gameObject.SetActive(false);
		item.toolTipFunc = ((tooltip_func != null) ? tooltip_func : new Func<AttributeInstance, string>(attribute.GetTooltip));
		this.attributesLines.Add(item);
	}

	// Token: 0x0600661E RID: 26142 RVA: 0x00261CF0 File Offset: 0x0025FEF0
	private void AddCheckboxLine(Amount amount, Transform parentContainer, Func<GameObject, string> label_text_func, Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition, Func<GameObject, bool> checkbox_value_func, Func<GameObject, string> tooltip_func = null)
	{
		GameObject gameObject = Util.KInstantiateUI(this.CheckboxLinePrefab, this.Content.gameObject, false);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		gameObject.GetComponent<ToolTip>().refreshWhileHovering = true;
		gameObject.SetActive(true);
		MinionVitalsPanel.CheckboxLine checkboxLine = default(MinionVitalsPanel.CheckboxLine);
		checkboxLine.go = gameObject;
		checkboxLine.parentContainer = parentContainer;
		checkboxLine.amount = amount;
		checkboxLine.locText = (component.GetReference("Label") as LocText);
		checkboxLine.get_value = checkbox_value_func;
		checkboxLine.display_condition = display_condition;
		checkboxLine.label_text_func = label_text_func;
		checkboxLine.go.name = "Checkbox_";
		if (amount != null)
		{
			GameObject go = checkboxLine.go;
			go.name += amount.Name;
		}
		else
		{
			GameObject go2 = checkboxLine.go;
			go2.name += "Unnamed";
		}
		if (tooltip_func != null)
		{
			checkboxLine.tooltip = tooltip_func;
			ToolTip tt = checkboxLine.go.GetComponent<ToolTip>();
			tt.refreshWhileHovering = true;
			tt.OnToolTip = delegate()
			{
				tt.ClearMultiStringTooltip();
				tt.AddMultiStringTooltip(tooltip_func(this.lastSelectedEntity), null);
				return "";
			};
		}
		this.checkboxLines.Add(checkboxLine);
	}

	// Token: 0x0600661F RID: 26143 RVA: 0x00261E36 File Offset: 0x00260036
	private void ShouldShowVitalsPanel(GameObject selectedEntity)
	{
	}

	// Token: 0x06006620 RID: 26144 RVA: 0x00261E38 File Offset: 0x00260038
	public void Refresh(GameObject selectedEntity)
	{
		if (selectedEntity == null)
		{
			return;
		}
		if (selectedEntity.gameObject == null)
		{
			return;
		}
		this.lastSelectedEntity = selectedEntity;
		WiltCondition component = selectedEntity.GetComponent<WiltCondition>();
		MinionIdentity component2 = selectedEntity.GetComponent<MinionIdentity>();
		CreatureBrain component3 = selectedEntity.GetComponent<CreatureBrain>();
		IncubationMonitor.Instance smi = selectedEntity.GetSMI<IncubationMonitor.Instance>();
		object[] array = new object[]
		{
			component,
			component2,
			component3,
			smi
		};
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			base.SetActive(false);
			return;
		}
		base.SetActive(true);
		base.SetTitle((component == null) ? UI.DETAILTABS.SIMPLEINFO.GROUPNAME_CONDITION : UI.DETAILTABS.SIMPLEINFO.GROUPNAME_REQUIREMENTS);
		Amounts amounts = selectedEntity.GetAmounts();
		Attributes attributes = selectedEntity.GetAttributes();
		if (amounts == null || attributes == null)
		{
			return;
		}
		if (component == null)
		{
			this.conditionsContainerNormal.gameObject.SetActive(false);
			this.conditionsContainerAdditional.gameObject.SetActive(false);
			foreach (MinionVitalsPanel.AmountLine amountLine in this.amountsLines)
			{
				bool flag2 = amountLine.TryUpdate(amounts);
				if (amountLine.go.activeSelf != flag2)
				{
					amountLine.go.SetActive(flag2);
				}
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine in this.attributesLines)
			{
				bool flag3 = attributeLine.TryUpdate(attributes);
				if (attributeLine.go.activeSelf != flag3)
				{
					attributeLine.go.SetActive(flag3);
				}
			}
		}
		bool flag4 = false;
		for (int j = 0; j < this.checkboxLines.Count; j++)
		{
			MinionVitalsPanel.CheckboxLine checkboxLine = this.checkboxLines[j];
			MinionVitalsPanel.CheckboxLineDisplayType checkboxLineDisplayType = MinionVitalsPanel.CheckboxLineDisplayType.Hidden;
			if (this.checkboxLines[j].amount != null)
			{
				for (int k = 0; k < amounts.Count; k++)
				{
					AmountInstance amountInstance = amounts[k];
					if (checkboxLine.amount == amountInstance.amount)
					{
						checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
						break;
					}
				}
			}
			else
			{
				checkboxLineDisplayType = checkboxLine.display_condition(selectedEntity.gameObject);
			}
			if (checkboxLineDisplayType != MinionVitalsPanel.CheckboxLineDisplayType.Hidden)
			{
				checkboxLine.locText.SetText(checkboxLine.label_text_func(selectedEntity.gameObject));
				if (!checkboxLine.go.activeSelf)
				{
					checkboxLine.go.SetActive(true);
				}
				GameObject gameObject = checkboxLine.go.GetComponent<HierarchyReferences>().GetReference("Check").gameObject;
				gameObject.SetActive(checkboxLine.get_value(selectedEntity.gameObject));
				if (checkboxLine.go.transform.parent != checkboxLine.parentContainer)
				{
					checkboxLine.go.transform.SetParent(checkboxLine.parentContainer);
					checkboxLine.go.transform.localScale = Vector3.one;
				}
				if (checkboxLine.parentContainer == this.conditionsContainerAdditional)
				{
					flag4 = true;
				}
				if (checkboxLineDisplayType == MinionVitalsPanel.CheckboxLineDisplayType.Normal)
				{
					if (checkboxLine.get_value(selectedEntity.gameObject))
					{
						checkboxLine.locText.color = Color.black;
						gameObject.transform.parent.GetComponent<Image>().color = Color.black;
					}
					else
					{
						Color color = new Color(0.99215686f, 0f, 0.101960786f);
						checkboxLine.locText.color = color;
						gameObject.transform.parent.GetComponent<Image>().color = color;
					}
				}
				else
				{
					checkboxLine.locText.color = Color.grey;
					gameObject.transform.parent.GetComponent<Image>().color = Color.grey;
				}
			}
			else if (checkboxLine.go.activeSelf)
			{
				checkboxLine.go.SetActive(false);
			}
		}
		if (component != null)
		{
			IManageGrowingStates manageGrowingStates = component.GetComponent<IManageGrowingStates>();
			manageGrowingStates = ((manageGrowingStates != null) ? manageGrowingStates : component.GetSMI<IManageGrowingStates>());
			bool flag5 = component.HasTag(GameTags.Decoration);
			this.conditionsContainerNormal.gameObject.SetActive(true);
			this.conditionsContainerAdditional.gameObject.SetActive(!flag5);
			if (manageGrowingStates == null)
			{
				float num = 1f;
				LocText reference = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference.text = "";
				reference.text = (flag5 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_DECOR.BASE, Array.Empty<object>()) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 0.25f * 100f)));
				reference.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD_INSTANT.TOOLTIP, Array.Empty<object>()));
				LocText reference2 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				ReceptacleMonitor component4 = selectedEntity.GetComponent<ReceptacleMonitor>();
				reference2.color = ((component4 == null || component4.Replanted) ? Color.black : Color.grey);
				reference2.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.BASE, Util.FormatTwoDecimalPlace(num * 100f));
				reference2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC_INSTANT.TOOLTIP, Array.Empty<object>()));
			}
			else
			{
				LocText reference3 = this.conditionsContainerNormal.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference3.text = "";
				reference3.text = string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime(), "F1", false));
				reference3.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.WILD.TOOLTIP, GameUtil.GetFormattedCycles(manageGrowingStates.WildGrowthTime(), "F1", false)));
				LocText reference4 = this.conditionsContainerAdditional.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
				reference4.color = (selectedEntity.GetComponent<ReceptacleMonitor>().Replanted ? Color.black : Color.grey);
				reference4.text = "";
				reference4.text = (flag4 ? string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)) : string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.DOMESTIC.BASE, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)));
				reference4.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(UI.VITALSSCREEN.CONDITIONS_GROWING.ADDITIONAL_DOMESTIC.TOOLTIP, GameUtil.GetFormattedCycles(manageGrowingStates.DomesticGrowthTime(), "F1", false)));
			}
			foreach (MinionVitalsPanel.AmountLine amountLine2 in this.amountsLines)
			{
				amountLine2.go.SetActive(false);
			}
			foreach (MinionVitalsPanel.AttributeLine attributeLine2 in this.attributesLines)
			{
				attributeLine2.go.SetActive(false);
			}
		}
	}

	// Token: 0x06006621 RID: 26145 RVA: 0x0026257C File Offset: 0x0026077C
	private string GetAirPressureTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_PRESSURE.text.Replace("{pressure}", GameUtil.GetFormattedMass(component.GetExternalPressure(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06006622 RID: 26146 RVA: 0x002625C8 File Offset: 0x002607C8
	private string GetInternalTemperatureTooltip(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		if (component == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_TEMPERATURE.text.Replace("{temperature}", GameUtil.GetFormattedTemperature(component.InternalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x06006623 RID: 26147 RVA: 0x00262610 File Offset: 0x00260810
	private string GetFertilizationTooltip(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_FERTILIZER.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06006624 RID: 26148 RVA: 0x00262654 File Offset: 0x00260854
	private string GetIrrigationTooltip(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		if (smi == null)
		{
			return "";
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_IRRIGATION.text.Replace("{mass}", GameUtil.GetFormattedMass(smi.total_fertilizer_available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06006625 RID: 26149 RVA: 0x00262698 File Offset: 0x00260898
	private string GetIlluminationTooltip(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		if (illuminationTracker == null)
		{
			return "";
		}
		return illuminationTracker.GetIlluminationUITooltip();
	}

	// Token: 0x06006626 RID: 26150 RVA: 0x002626C8 File Offset: 0x002608C8
	private string GetRadiationTooltip(GameObject go)
	{
		int num = Grid.PosToCell(go);
		float rads = Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f;
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		MutantPlant component = go.GetComponent<MutantPlant>();
		bool flag = component != null && component.IsOriginal;
		string text;
		if (attributeInstance.GetTotalValue() == 0f)
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION_NO_MIN.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		else
		{
			text = UI.TOOLTIPS.VITALS_CHECKBOX_RADIATION.Replace("{rads}", GameUtil.GetFormattedRads(rads, GameUtil.TimeSlice.None)).Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		if (flag)
		{
			text += UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP;
		}
		return text;
	}

	// Token: 0x06006627 RID: 26151 RVA: 0x002627D0 File Offset: 0x002609D0
	private string GetReceptacleTooltip(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		if (component == null)
		{
			return "";
		}
		if (component.HasOperationalReceptacle())
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL;
	}

	// Token: 0x06006628 RID: 26152 RVA: 0x00262810 File Offset: 0x00260A10
	private string GetAtmosphereTooltip(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		if (component != null && component.currentAtmoElement != null)
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE.text.Replace("{element}", component.currentAtmoElement.name);
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ATMOSPHERE;
	}

	// Token: 0x06006629 RID: 26153 RVA: 0x00262860 File Offset: 0x00260A60
	private string GetAirPressureLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.AirPressure.Name,
			"\n    • ",
			GameUtil.GetFormattedMass(component.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, false, "{0:0.#}"),
			" - ",
			GameUtil.GetFormattedMass(component.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Gram, true, "{0:0.#}")
		});
	}

	// Token: 0x0600662A RID: 26154 RVA: 0x002628D4 File Offset: 0x00260AD4
	private string GetInternalTemperatureLabel(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return string.Concat(new string[]
		{
			Db.Get().Amounts.Temperature.Name,
			"\n    • ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false),
			" - ",
			GameUtil.GetFormattedTemperature(component.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)
		});
	}

	// Token: 0x0600662B RID: 26155 RVA: 0x00262940 File Offset: 0x00260B40
	private string GetFertilizationLabel(GameObject go)
	{
		StateMachine<FertilizationMonitor, FertilizationMonitor.Instance, IStateMachineTarget, FertilizationMonitor.Def>.GenericInstance smi = go.GetSMI<FertilizationMonitor.Instance>();
		string text = Db.Get().Amounts.Fertilization.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				" ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x0600662C RID: 26156 RVA: 0x002629F8 File Offset: 0x00260BF8
	private string GetIrrigationLabel(GameObject go)
	{
		StateMachine<IrrigationMonitor, IrrigationMonitor.Instance, IStateMachineTarget, IrrigationMonitor.Def>.GenericInstance smi = go.GetSMI<IrrigationMonitor.Instance>();
		string text = Db.Get().Amounts.Irrigation.Name;
		float totalValue = go.GetAttributes().Get(Db.Get().PlantAttributes.FertilizerUsageMod).GetTotalValue();
		foreach (PlantElementAbsorber.ConsumeInfo consumeInfo in smi.def.consumedElements)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n    • ",
				ElementLoader.GetElement(consumeInfo.tag).name,
				": ",
				GameUtil.GetFormattedMass(consumeInfo.massConsumptionRate * totalValue, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
			});
		}
		return text;
	}

	// Token: 0x0600662D RID: 26157 RVA: 0x00262AB0 File Offset: 0x00260CB0
	private string GetIlluminationLabel(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		return illuminationTracker.GetIlluminationUILabel();
	}

	// Token: 0x0600662E RID: 26158 RVA: 0x00262AD4 File Offset: 0x00260CD4
	private string GetAtmosphereLabel(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		string text = UI.VITALSSCREEN.ATMOSPHERE_CONDITION;
		foreach (Element element in component.safe_atmospheres)
		{
			text = text + "\n    • " + element.name;
		}
		return text;
	}

	// Token: 0x0600662F RID: 26159 RVA: 0x00262B44 File Offset: 0x00260D44
	private string GetRadiationLabel(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		AttributeInstance attributeInstance2 = go.GetAttributes().Get(Db.Get().PlantAttributes.MaxRadiationThreshold);
		if (attributeInstance.GetTotalValue() == 0f)
		{
			return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_NO_MIN_RADIATION_FMT.Replace("{maxRads}", attributeInstance2.GetFormattedValue());
		}
		return UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION + "\n    • " + UI.GAMEOBJECTEFFECTS.AMBIENT_RADIATION_FMT.Replace("{minRads}", attributeInstance.GetFormattedValue()).Replace("{maxRads}", attributeInstance2.GetFormattedValue());
	}

	// Token: 0x06006630 RID: 26160 RVA: 0x00262BF8 File Offset: 0x00260DF8
	private bool check_pressure(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.ExternalPressureState == PressureVulnerable.PressureState.Normal;
	}

	// Token: 0x06006631 RID: 26161 RVA: 0x00262C20 File Offset: 0x00260E20
	private bool check_temperature(GameObject go)
	{
		TemperatureVulnerable component = go.GetComponent<TemperatureVulnerable>();
		return !(component != null) || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
	}

	// Token: 0x06006632 RID: 26162 RVA: 0x00262C48 File Offset: 0x00260E48
	private bool check_irrigation(GameObject go)
	{
		IrrigationMonitor.Instance smi = go.GetSMI<IrrigationMonitor.Instance>();
		return smi == null || (!smi.IsInsideState(smi.sm.replanted.starved) && !smi.IsInsideState(smi.sm.wild));
	}

	// Token: 0x06006633 RID: 26163 RVA: 0x00262C90 File Offset: 0x00260E90
	private bool check_illumination(GameObject go)
	{
		IIlluminationTracker illuminationTracker = go.GetComponent<IIlluminationTracker>();
		if (illuminationTracker == null)
		{
			illuminationTracker = go.GetSMI<IIlluminationTracker>();
		}
		return illuminationTracker == null || illuminationTracker.ShouldIlluminationUICheckboxBeChecked();
	}

	// Token: 0x06006634 RID: 26164 RVA: 0x00262CBC File Offset: 0x00260EBC
	private bool check_radiation(GameObject go)
	{
		AttributeInstance attributeInstance = go.GetAttributes().Get(Db.Get().PlantAttributes.MinRadiationThreshold);
		if (attributeInstance != null && attributeInstance.GetTotalValue() != 0f)
		{
			int num = Grid.PosToCell(go);
			return (Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f) >= attributeInstance.GetTotalValue();
		}
		return true;
	}

	// Token: 0x06006635 RID: 26165 RVA: 0x00262D24 File Offset: 0x00260F24
	private bool check_receptacle(GameObject go)
	{
		ReceptacleMonitor component = go.GetComponent<ReceptacleMonitor>();
		return !(component == null) && component.HasOperationalReceptacle();
	}

	// Token: 0x06006636 RID: 26166 RVA: 0x00262D4C File Offset: 0x00260F4C
	private bool check_fertilizer(GameObject go)
	{
		FertilizationMonitor.Instance smi = go.GetSMI<FertilizationMonitor.Instance>();
		return smi == null || smi.sm.hasCorrectFertilizer.Get(smi);
	}

	// Token: 0x06006637 RID: 26167 RVA: 0x00262D78 File Offset: 0x00260F78
	private bool check_atmosphere(GameObject go)
	{
		PressureVulnerable component = go.GetComponent<PressureVulnerable>();
		return !(component != null) || component.testAreaElementSafe;
	}

	// Token: 0x040044F4 RID: 17652
	public GameObject LineItemPrefab;

	// Token: 0x040044F5 RID: 17653
	public GameObject CheckboxLinePrefab;

	// Token: 0x040044F6 RID: 17654
	private GameObject lastSelectedEntity;

	// Token: 0x040044F7 RID: 17655
	public List<MinionVitalsPanel.AmountLine> amountsLines = new List<MinionVitalsPanel.AmountLine>();

	// Token: 0x040044F8 RID: 17656
	public List<MinionVitalsPanel.AttributeLine> attributesLines = new List<MinionVitalsPanel.AttributeLine>();

	// Token: 0x040044F9 RID: 17657
	public List<MinionVitalsPanel.CheckboxLine> checkboxLines = new List<MinionVitalsPanel.CheckboxLine>();

	// Token: 0x040044FA RID: 17658
	public Transform conditionsContainerNormal;

	// Token: 0x040044FB RID: 17659
	public Transform conditionsContainerAdditional;

	// Token: 0x02001DE6 RID: 7654
	[DebuggerDisplay("{amount.Name}")]
	public struct AmountLine
	{
		// Token: 0x0600A9FD RID: 43517 RVA: 0x003A03F4 File Offset: 0x0039E5F4
		public bool TryUpdate(Amounts amounts)
		{
			foreach (AmountInstance amountInstance in amounts)
			{
				if (this.amount == amountInstance.amount && !amountInstance.hide)
				{
					this.locText.SetText(this.amount.GetDescription(amountInstance));
					this.toolTip.toolTip = this.toolTipFunc(amountInstance);
					this.imageToggle.SetValue(amountInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008880 RID: 34944
		public Amount amount;

		// Token: 0x04008881 RID: 34945
		public GameObject go;

		// Token: 0x04008882 RID: 34946
		public ValueTrendImageToggle imageToggle;

		// Token: 0x04008883 RID: 34947
		public LocText locText;

		// Token: 0x04008884 RID: 34948
		public ToolTip toolTip;

		// Token: 0x04008885 RID: 34949
		public Func<AmountInstance, string> toolTipFunc;
	}

	// Token: 0x02001DE7 RID: 7655
	[DebuggerDisplay("{attribute.Name}")]
	public struct AttributeLine
	{
		// Token: 0x0600A9FE RID: 43518 RVA: 0x003A048C File Offset: 0x0039E68C
		public bool TryUpdate(Attributes attributes)
		{
			foreach (AttributeInstance attributeInstance in attributes)
			{
				if (this.attribute == attributeInstance.modifier && !attributeInstance.hide)
				{
					this.locText.SetText(this.attribute.GetDescription(attributeInstance));
					this.toolTip.toolTip = this.toolTipFunc(attributeInstance);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008886 RID: 34950
		public Klei.AI.Attribute attribute;

		// Token: 0x04008887 RID: 34951
		public GameObject go;

		// Token: 0x04008888 RID: 34952
		public LocText locText;

		// Token: 0x04008889 RID: 34953
		public ToolTip toolTip;

		// Token: 0x0400888A RID: 34954
		public Func<AttributeInstance, string> toolTipFunc;
	}

	// Token: 0x02001DE8 RID: 7656
	public struct CheckboxLine
	{
		// Token: 0x0400888B RID: 34955
		public Amount amount;

		// Token: 0x0400888C RID: 34956
		public GameObject go;

		// Token: 0x0400888D RID: 34957
		public LocText locText;

		// Token: 0x0400888E RID: 34958
		public Func<GameObject, string> tooltip;

		// Token: 0x0400888F RID: 34959
		public Func<GameObject, bool> get_value;

		// Token: 0x04008890 RID: 34960
		public Func<GameObject, MinionVitalsPanel.CheckboxLineDisplayType> display_condition;

		// Token: 0x04008891 RID: 34961
		public Func<GameObject, string> label_text_func;

		// Token: 0x04008892 RID: 34962
		public Transform parentContainer;
	}

	// Token: 0x02001DE9 RID: 7657
	public enum CheckboxLineDisplayType
	{
		// Token: 0x04008894 RID: 34964
		Normal,
		// Token: 0x04008895 RID: 34965
		Diminished,
		// Token: 0x04008896 RID: 34966
		Hidden
	}
}
