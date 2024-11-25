using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C2D RID: 3117
public class DebugPaintElementScreen : KScreen
{
	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06005F98 RID: 24472 RVA: 0x0023812D File Offset: 0x0023632D
	// (set) Token: 0x06005F99 RID: 24473 RVA: 0x00238134 File Offset: 0x00236334
	public static DebugPaintElementScreen Instance { get; private set; }

	// Token: 0x06005F9A RID: 24474 RVA: 0x0023813C File Offset: 0x0023633C
	public static void DestroyInstance()
	{
		DebugPaintElementScreen.Instance = null;
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x00238144 File Offset: 0x00236344
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugPaintElementScreen.Instance = this;
		this.SetupLocText();
		this.inputFields.Add(this.massPressureInput);
		this.inputFields.Add(this.temperatureInput);
		this.inputFields.Add(this.diseaseCountInput);
		this.inputFields.Add(this.filterInput);
		foreach (KInputTextField kinputTextField in this.inputFields)
		{
			kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
			{
				base.isEditing = true;
			}));
			kinputTextField.onEndEdit.AddListener(delegate(string value)
			{
				base.isEditing = false;
			});
		}
		this.temperatureInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeTemperature();
		});
		this.massPressureInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnChangeMassPressure();
		});
		this.diseaseCountInput.onEndEdit.AddListener(delegate(string value)
		{
			this.OnDiseaseCountChange();
		});
		base.gameObject.SetActive(false);
		this.activateOnSpawn = true;
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x0023828C File Offset: 0x0023648C
	private void SetupLocText()
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("Title").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TITLE;
		component.GetReference<LocText>("ElementLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		component.GetReference<LocText>("MassLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.MASS_KG;
		component.GetReference<LocText>("TemperatureLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.TEMPERATURE_KELVIN;
		component.GetReference<LocText>("DiseaseLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		component.GetReference<LocText>("DiseaseCountLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE_COUNT;
		component.GetReference<LocText>("AddFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ADD_FOW_MASK;
		component.GetReference<LocText>("RemoveFoWMaskLabel").text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.REMOVE_FOW_MASK;
		this.elementButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.ELEMENT;
		this.diseaseButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.DISEASE;
		this.paintButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.PAINT;
		this.fillButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.FILL;
		this.spawnButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SPAWN_ALL;
		this.sampleButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.SAMPLE;
		this.storeButton.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.STORE;
		this.affectBuildings.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.BUILDINGS;
		this.affectCells.transform.parent.GetComponentsInChildren<LocText>()[0].text = UI.DEBUG_TOOLS.PAINT_ELEMENTS_SCREEN.CELLS;
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x00238480 File Offset: 0x00236680
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.element = SimHashes.Ice;
		this.diseaseIdx = byte.MaxValue;
		this.ConfigureElements();
		List<string> list = new List<string>();
		list.Insert(0, "None");
		foreach (Disease disease in Db.Get().Diseases.resources)
		{
			list.Add(disease.Name);
		}
		this.diseasePopup.SetOptions(list.ToArray());
		KPopupMenu kpopupMenu = this.diseasePopup;
		kpopupMenu.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu.OnSelect, new Action<string, int>(this.OnSelectDisease));
		this.SelectDiseaseOption((int)this.diseaseIdx);
		this.paintButton.onClick += this.OnClickPaint;
		this.fillButton.onClick += this.OnClickFill;
		this.sampleButton.onClick += this.OnClickSample;
		this.storeButton.onClick += this.OnClickStore;
		if (SaveGame.Instance.worldGenSpawner.SpawnsRemain())
		{
			this.spawnButton.onClick += this.OnClickSpawn;
		}
		KPopupMenu kpopupMenu2 = this.elementPopup;
		kpopupMenu2.OnSelect = (Action<string, int>)Delegate.Combine(kpopupMenu2.OnSelect, new Action<string, int>(this.OnSelectElement));
		this.elementButton.onClick += this.elementPopup.OnClick;
		this.diseaseButton.onClick += this.diseasePopup.OnClick;
	}

	// Token: 0x06005F9E RID: 24478 RVA: 0x0023863C File Offset: 0x0023683C
	private void FilterElements(string filterValue)
	{
		if (string.IsNullOrEmpty(filterValue))
		{
			foreach (KButtonMenu.ButtonInfo buttonInfo in this.elementPopup.GetButtons())
			{
				buttonInfo.uibutton.gameObject.SetActive(true);
			}
			return;
		}
		filterValue = this.filter.ToLower();
		foreach (KButtonMenu.ButtonInfo buttonInfo2 in this.elementPopup.GetButtons())
		{
			buttonInfo2.uibutton.gameObject.SetActive(buttonInfo2.text.ToLower().Contains(filterValue));
		}
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x00238708 File Offset: 0x00236908
	private void ConfigureElements()
	{
		if (this.filter != null)
		{
			this.filter = this.filter.ToLower();
		}
		List<DebugPaintElementScreen.ElemDisplayInfo> list = new List<DebugPaintElementScreen.ElemDisplayInfo>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.name != "Element Not Loaded" && element.substance != null && element.substance.showInEditor && (string.IsNullOrEmpty(this.filter) || element.name.ToLower().Contains(this.filter)))
			{
				list.Add(new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element.id,
					displayStr = element.name + " (" + element.GetStateString() + ")"
				});
			}
		}
		list.Sort((DebugPaintElementScreen.ElemDisplayInfo a, DebugPaintElementScreen.ElemDisplayInfo b) => a.displayStr.CompareTo(b.displayStr));
		if (string.IsNullOrEmpty(this.filter))
		{
			SimHashes[] array = new SimHashes[]
			{
				SimHashes.SlimeMold,
				SimHashes.Vacuum,
				SimHashes.Dirt,
				SimHashes.CarbonDioxide,
				SimHashes.Water,
				SimHashes.Oxygen
			};
			for (int i = 0; i < array.Length; i++)
			{
				Element element2 = ElementLoader.FindElementByHash(array[i]);
				list.Insert(0, new DebugPaintElementScreen.ElemDisplayInfo
				{
					id = element2.id,
					displayStr = element2.name + " (" + element2.GetStateString() + ")"
				});
			}
		}
		this.options_list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (DebugPaintElementScreen.ElemDisplayInfo elemDisplayInfo in list)
		{
			list2.Add(elemDisplayInfo.displayStr);
			this.options_list.Add(elemDisplayInfo.id.ToString());
		}
		this.elementPopup.SetOptions(list2);
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].id == this.element)
			{
				this.elementPopup.SelectOption(list2[j], j);
			}
		}
		this.elementPopup.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0f, 1f);
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x00238988 File Offset: 0x00236B88
	private void OnClickSpawn()
	{
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.SetDiscovered(true);
		}
		SaveGame.Instance.worldGenSpawner.SpawnEverything();
		this.spawnButton.GetComponent<KButton>().isInteractable = false;
	}

	// Token: 0x06005FA1 RID: 24481 RVA: 0x002389F8 File Offset: 0x00236BF8
	private void OnClickPaint()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.ReplaceSubstance);
	}

	// Token: 0x06005FA2 RID: 24482 RVA: 0x00238A1D File Offset: 0x00236C1D
	private void OnClickStore()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.StoreSubstance);
	}

	// Token: 0x06005FA3 RID: 24483 RVA: 0x00238A42 File Offset: 0x00236C42
	private void OnClickSample()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		this.OnChangeFOWReveal();
		DebugTool.Instance.Activate(DebugTool.Type.Sample);
	}

	// Token: 0x06005FA4 RID: 24484 RVA: 0x00238A67 File Offset: 0x00236C67
	private void OnClickFill()
	{
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
		this.OnDiseaseCountChange();
		DebugTool.Instance.Activate(DebugTool.Type.FillReplaceSubstance);
	}

	// Token: 0x06005FA5 RID: 24485 RVA: 0x00238A86 File Offset: 0x00236C86
	private void OnSelectElement(string str, int index)
	{
		this.element = (SimHashes)Enum.Parse(typeof(SimHashes), this.options_list[index]);
		this.elementButton.GetComponentInChildren<LocText>().text = str;
	}

	// Token: 0x06005FA6 RID: 24486 RVA: 0x00238ABF File Offset: 0x00236CBF
	private void OnSelectElement(SimHashes element)
	{
		this.element = element;
		this.elementButton.GetComponentInChildren<LocText>().text = ElementLoader.FindElementByHash(element).name;
	}

	// Token: 0x06005FA7 RID: 24487 RVA: 0x00238AE4 File Offset: 0x00236CE4
	private void OnSelectDisease(string str, int index)
	{
		this.diseaseIdx = byte.MaxValue;
		for (int i = 0; i < Db.Get().Diseases.Count; i++)
		{
			if (Db.Get().Diseases[i].Name == str)
			{
				this.diseaseIdx = (byte)i;
			}
		}
		this.SelectDiseaseOption((int)this.diseaseIdx);
	}

	// Token: 0x06005FA8 RID: 24488 RVA: 0x00238B48 File Offset: 0x00236D48
	private void SelectDiseaseOption(int diseaseIdx)
	{
		if (diseaseIdx == 255)
		{
			this.diseaseButton.GetComponentInChildren<LocText>().text = "None";
			return;
		}
		string name = Db.Get().Diseases[diseaseIdx].Name;
		this.diseaseButton.GetComponentInChildren<LocText>().text = name;
	}

	// Token: 0x06005FA9 RID: 24489 RVA: 0x00238B9C File Offset: 0x00236D9C
	private void OnChangeFOWReveal()
	{
		if (this.paintPreventFOWReveal.isOn)
		{
			this.paintAllowFOWReveal.isOn = false;
		}
		if (this.paintAllowFOWReveal.isOn)
		{
			this.paintPreventFOWReveal.isOn = false;
		}
		this.set_prevent_fow_reveal = this.paintPreventFOWReveal.isOn;
		this.set_allow_fow_reveal = this.paintAllowFOWReveal.isOn;
	}

	// Token: 0x06005FAA RID: 24490 RVA: 0x00238C00 File Offset: 0x00236E00
	public void OnChangeMassPressure()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.massPressureInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.massPressureInput.text = "1";
		}
		this.mass = num;
	}

	// Token: 0x06005FAB RID: 24491 RVA: 0x00238C60 File Offset: 0x00236E60
	public void OnChangeTemperature()
	{
		float num;
		try
		{
			num = Convert.ToSingle(this.temperatureInput.text);
		}
		catch
		{
			num = -1f;
		}
		if (num <= 0f)
		{
			num = 1f;
			this.temperatureInput.text = "1";
		}
		this.temperature = num;
	}

	// Token: 0x06005FAC RID: 24492 RVA: 0x00238CC0 File Offset: 0x00236EC0
	public void OnDiseaseCountChange()
	{
		int num;
		int.TryParse(this.diseaseCountInput.text, out num);
		if (num < 0)
		{
			num = 0;
			this.diseaseCountInput.text = "0";
		}
		this.diseaseCount = num;
	}

	// Token: 0x06005FAD RID: 24493 RVA: 0x00238CFD File Offset: 0x00236EFD
	public void OnElementsFilterEdited(string new_filter)
	{
		this.filter = (string.IsNullOrEmpty(this.filterInput.text) ? null : this.filterInput.text);
		this.FilterElements(this.filter);
	}

	// Token: 0x06005FAE RID: 24494 RVA: 0x00238D34 File Offset: 0x00236F34
	public void SampleCell(int cell)
	{
		this.massPressureInput.text = (Grid.Pressure[cell] * 0.010000001f).ToString();
		this.temperatureInput.text = Grid.Temperature[cell].ToString();
		this.OnSelectElement(ElementLoader.GetElementID(Grid.Element[cell].tag));
		this.OnChangeMassPressure();
		this.OnChangeTemperature();
	}

	// Token: 0x0400405D RID: 16477
	[Header("Current State")]
	public SimHashes element;

	// Token: 0x0400405E RID: 16478
	[NonSerialized]
	public float mass = 1000f;

	// Token: 0x0400405F RID: 16479
	[NonSerialized]
	public float temperature = -1f;

	// Token: 0x04004060 RID: 16480
	[NonSerialized]
	public bool set_prevent_fow_reveal;

	// Token: 0x04004061 RID: 16481
	[NonSerialized]
	public bool set_allow_fow_reveal;

	// Token: 0x04004062 RID: 16482
	[NonSerialized]
	public int diseaseCount;

	// Token: 0x04004063 RID: 16483
	public byte diseaseIdx;

	// Token: 0x04004064 RID: 16484
	[Header("Popup Buttons")]
	[SerializeField]
	private KButton elementButton;

	// Token: 0x04004065 RID: 16485
	[SerializeField]
	private KButton diseaseButton;

	// Token: 0x04004066 RID: 16486
	[Header("Popup Menus")]
	[SerializeField]
	private KPopupMenu elementPopup;

	// Token: 0x04004067 RID: 16487
	[SerializeField]
	private KPopupMenu diseasePopup;

	// Token: 0x04004068 RID: 16488
	[Header("Value Inputs")]
	[SerializeField]
	private KInputTextField massPressureInput;

	// Token: 0x04004069 RID: 16489
	[SerializeField]
	private KInputTextField temperatureInput;

	// Token: 0x0400406A RID: 16490
	[SerializeField]
	private KInputTextField diseaseCountInput;

	// Token: 0x0400406B RID: 16491
	[SerializeField]
	private KInputTextField filterInput;

	// Token: 0x0400406C RID: 16492
	[Header("Tool Buttons")]
	[SerializeField]
	private KButton paintButton;

	// Token: 0x0400406D RID: 16493
	[SerializeField]
	private KButton fillButton;

	// Token: 0x0400406E RID: 16494
	[SerializeField]
	private KButton sampleButton;

	// Token: 0x0400406F RID: 16495
	[SerializeField]
	private KButton spawnButton;

	// Token: 0x04004070 RID: 16496
	[SerializeField]
	private KButton storeButton;

	// Token: 0x04004071 RID: 16497
	[Header("Parameter Toggles")]
	public Toggle paintElement;

	// Token: 0x04004072 RID: 16498
	public Toggle paintMass;

	// Token: 0x04004073 RID: 16499
	public Toggle paintTemperature;

	// Token: 0x04004074 RID: 16500
	public Toggle paintDisease;

	// Token: 0x04004075 RID: 16501
	public Toggle paintDiseaseCount;

	// Token: 0x04004076 RID: 16502
	public Toggle affectBuildings;

	// Token: 0x04004077 RID: 16503
	public Toggle affectCells;

	// Token: 0x04004078 RID: 16504
	public Toggle paintPreventFOWReveal;

	// Token: 0x04004079 RID: 16505
	public Toggle paintAllowFOWReveal;

	// Token: 0x0400407A RID: 16506
	private List<KInputTextField> inputFields = new List<KInputTextField>();

	// Token: 0x0400407B RID: 16507
	private List<string> options_list = new List<string>();

	// Token: 0x0400407C RID: 16508
	private string filter;

	// Token: 0x02001D13 RID: 7443
	private struct ElemDisplayInfo
	{
		// Token: 0x040085EA RID: 34282
		public SimHashes id;

		// Token: 0x040085EB RID: 34283
		public string displayStr;
	}
}
