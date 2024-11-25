using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000DE8 RID: 3560
[Serializable]
public class UnitConfigurationScreen
{
	// Token: 0x06007110 RID: 28944 RVA: 0x002AC870 File Offset: 0x002AAA70
	public void Init()
	{
		this.celsiusToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.celsiusToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS_TOOLTIP;
		this.celsiusToggle.GetComponentInChildren<KButton>().onClick += this.OnCelsiusClicked;
		this.celsiusToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.CELSIUS;
		this.kelvinToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.kelvinToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN_TOOLTIP;
		this.kelvinToggle.GetComponentInChildren<KButton>().onClick += this.OnKelvinClicked;
		this.kelvinToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.KELVIN;
		this.fahrenheitToggle = Util.KInstantiateUI(this.toggleUnitPrefab, this.toggleGroup, true);
		this.fahrenheitToggle.GetComponentInChildren<ToolTip>().toolTip = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT_TOOLTIP;
		this.fahrenheitToggle.GetComponentInChildren<KButton>().onClick += this.OnFahrenheitClicked;
		this.fahrenheitToggle.GetComponentInChildren<LocText>().text = UI.FRONTEND.UNIT_OPTIONS_SCREEN.FAHRENHEIT;
		this.DisplayCurrentUnit();
	}

	// Token: 0x06007111 RID: 28945 RVA: 0x002AC9BC File Offset: 0x002AABBC
	private void DisplayCurrentUnit()
	{
		GameUtil.TemperatureUnit @int = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt(UnitConfigurationScreen.TemperatureUnitKey, 0);
		if (@int == GameUtil.TemperatureUnit.Celsius)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			return;
		}
		if (@int != GameUtil.TemperatureUnit.Kelvin)
		{
			this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
			this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
			return;
		}
		this.celsiusToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
		this.kelvinToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(true);
		this.fahrenheitToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(false);
	}

	// Token: 0x06007112 RID: 28946 RVA: 0x002ACB04 File Offset: 0x002AAD04
	private void OnCelsiusClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Celsius;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Celsius);
		}
	}

	// Token: 0x06007113 RID: 28947 RVA: 0x002ACB5C File Offset: 0x002AAD5C
	private void OnKelvinClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Kelvin;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Kelvin);
		}
	}

	// Token: 0x06007114 RID: 28948 RVA: 0x002ACBB4 File Offset: 0x002AADB4
	private void OnFahrenheitClicked()
	{
		GameUtil.temperatureUnit = GameUtil.TemperatureUnit.Fahrenheit;
		KPlayerPrefs.SetInt(UnitConfigurationScreen.TemperatureUnitKey, GameUtil.temperatureUnit.GetHashCode());
		this.DisplayCurrentUnit();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(999382396, GameUtil.TemperatureUnit.Fahrenheit);
		}
	}

	// Token: 0x04004DB7 RID: 19895
	[SerializeField]
	private GameObject toggleUnitPrefab;

	// Token: 0x04004DB8 RID: 19896
	[SerializeField]
	private GameObject toggleGroup;

	// Token: 0x04004DB9 RID: 19897
	private GameObject celsiusToggle;

	// Token: 0x04004DBA RID: 19898
	private GameObject kelvinToggle;

	// Token: 0x04004DBB RID: 19899
	private GameObject fahrenheitToggle;

	// Token: 0x04004DBC RID: 19900
	public static readonly string TemperatureUnitKey = "TemperatureUnit";

	// Token: 0x04004DBD RID: 19901
	public static readonly string MassUnitKey = "MassUnit";
}
