using System;
using System.Collections.Generic;
using Klei.AI;
using TemplateClasses;
using UnityEngine;

// Token: 0x02000C2B RID: 3115
public class DebugBaseTemplateButton : KScreen
{
	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06005F77 RID: 24439 RVA: 0x00236E55 File Offset: 0x00235055
	// (set) Token: 0x06005F78 RID: 24440 RVA: 0x00236E5C File Offset: 0x0023505C
	public static DebugBaseTemplateButton Instance { get; private set; }

	// Token: 0x06005F79 RID: 24441 RVA: 0x00236E64 File Offset: 0x00235064
	public static void DestroyInstance()
	{
		DebugBaseTemplateButton.Instance = null;
	}

	// Token: 0x06005F7A RID: 24442 RVA: 0x00236E6C File Offset: 0x0023506C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugBaseTemplateButton.Instance = this;
		base.gameObject.SetActive(false);
		this.SetupLocText();
		base.ConsumeMouseScroll = true;
		KInputTextField kinputTextField = this.nameField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.nameField.onEndEdit.AddListener(delegate(string <p0>)
		{
			base.isEditing = false;
		});
		this.nameField.onValueChanged.AddListener(delegate(string <p0>)
		{
			Util.ScrubInputField(this.nameField, true, false);
		});
	}

	// Token: 0x06005F7B RID: 24443 RVA: 0x00236EFD File Offset: 0x002350FD
	protected override void OnActivate()
	{
		base.OnActivate();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06005F7C RID: 24444 RVA: 0x00236F0C File Offset: 0x0023510C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.saveBaseButton != null)
		{
			this.saveBaseButton.onClick -= this.OnClickSaveBase;
			this.saveBaseButton.onClick += this.OnClickSaveBase;
		}
		if (this.clearButton != null)
		{
			this.clearButton.onClick -= this.OnClickClear;
			this.clearButton.onClick += this.OnClickClear;
		}
		if (this.AddSelectionButton != null)
		{
			this.AddSelectionButton.onClick -= this.OnClickAddSelection;
			this.AddSelectionButton.onClick += this.OnClickAddSelection;
		}
		if (this.RemoveSelectionButton != null)
		{
			this.RemoveSelectionButton.onClick -= this.OnClickRemoveSelection;
			this.RemoveSelectionButton.onClick += this.OnClickRemoveSelection;
		}
		if (this.clearSelectionButton != null)
		{
			this.clearSelectionButton.onClick -= this.OnClickClearSelection;
			this.clearSelectionButton.onClick += this.OnClickClearSelection;
		}
		if (this.MoveButton != null)
		{
			this.MoveButton.onClick -= this.OnClickMove;
			this.MoveButton.onClick += this.OnClickMove;
		}
		if (this.DestroyButton != null)
		{
			this.DestroyButton.onClick -= this.OnClickDestroySelection;
			this.DestroyButton.onClick += this.OnClickDestroySelection;
		}
		if (this.DeconstructButton != null)
		{
			this.DeconstructButton.onClick -= this.OnClickDeconstructSelection;
			this.DeconstructButton.onClick += this.OnClickDeconstructSelection;
		}
	}

	// Token: 0x06005F7D RID: 24445 RVA: 0x002370FF File Offset: 0x002352FF
	private void SetupLocText()
	{
	}

	// Token: 0x06005F7E RID: 24446 RVA: 0x00237101 File Offset: 0x00235301
	private void OnClickDestroySelection()
	{
		DebugTool.Instance.Activate(DebugTool.Type.Destroy);
	}

	// Token: 0x06005F7F RID: 24447 RVA: 0x0023710E File Offset: 0x0023530E
	private void OnClickDeconstructSelection()
	{
		DebugTool.Instance.Activate(DebugTool.Type.Deconstruct);
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x0023711B File Offset: 0x0023531B
	private void OnClickMove()
	{
		DebugTool.Instance.DeactivateTool(null);
		this.moveAsset = this.GetSelectionAsAsset();
		StampTool.Instance.Activate(this.moveAsset, false, false);
	}

	// Token: 0x06005F81 RID: 24449 RVA: 0x00237146 File Offset: 0x00235346
	private void OnClickAddSelection()
	{
		DebugTool.Instance.Activate(DebugTool.Type.AddSelection);
	}

	// Token: 0x06005F82 RID: 24450 RVA: 0x00237153 File Offset: 0x00235353
	private void OnClickRemoveSelection()
	{
		DebugTool.Instance.Activate(DebugTool.Type.RemoveSelection);
	}

	// Token: 0x06005F83 RID: 24451 RVA: 0x00237160 File Offset: 0x00235360
	private void OnClickClearSelection()
	{
		this.ClearSelection();
		this.nameField.text = "";
	}

	// Token: 0x06005F84 RID: 24452 RVA: 0x00237178 File Offset: 0x00235378
	private void OnClickClear()
	{
		DebugTool.Instance.Activate(DebugTool.Type.Clear);
	}

	// Token: 0x06005F85 RID: 24453 RVA: 0x00237185 File Offset: 0x00235385
	protected override void OnDeactivate()
	{
		if (DebugTool.Instance != null)
		{
			DebugTool.Instance.DeactivateTool(null);
		}
		base.OnDeactivate();
	}

	// Token: 0x06005F86 RID: 24454 RVA: 0x002371A5 File Offset: 0x002353A5
	protected override void OnDisable()
	{
		if (DebugTool.Instance != null)
		{
			DebugTool.Instance.DeactivateTool(null);
		}
	}

	// Token: 0x06005F87 RID: 24455 RVA: 0x002371C0 File Offset: 0x002353C0
	private TemplateContainer GetSelectionAsAsset()
	{
		List<Cell> list = new List<Cell>();
		List<Prefab> list2 = new List<Prefab>();
		List<Prefab> list3 = new List<Prefab>();
		List<Prefab> list4 = new List<Prefab>();
		List<Prefab> otherEntities = new List<Prefab>();
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		float num = 0f;
		float num2 = 0f;
		foreach (int cell in this.SelectedCells)
		{
			num += (float)Grid.CellToXY(cell).x;
			num2 += (float)Grid.CellToXY(cell).y;
		}
		float x = num / (float)this.SelectedCells.Count;
		float y;
		num2 = (y = num2 / (float)this.SelectedCells.Count);
		int rootX;
		int rootY;
		Grid.CellToXY(Grid.PosToCell(new Vector3(x, y, 0f)), out rootX, out rootY);
		for (int i = 0; i < this.SelectedCells.Count; i++)
		{
			int i2 = this.SelectedCells[i];
			int num3;
			int num4;
			Grid.CellToXY(this.SelectedCells[i], out num3, out num4);
			Element element = ElementLoader.elements[(int)Grid.ElementIdx[i2]];
			string diseaseName = (Grid.DiseaseIdx[i2] != byte.MaxValue) ? Db.Get().Diseases[(int)Grid.DiseaseIdx[i2]].Id : null;
			int num5 = Grid.DiseaseCount[i2];
			if (num5 <= 0)
			{
				num5 = 0;
				diseaseName = null;
			}
			list.Add(new Cell(num3 - rootX, num4 - rootY, element.id, Grid.Temperature[i2], Grid.Mass[i2], diseaseName, num5, Grid.PreventFogOfWarReveal[this.SelectedCells[i]]));
		}
		for (int j = 0; j < Components.BuildingCompletes.Count; j++)
		{
			BuildingComplete buildingComplete = Components.BuildingCompletes[j];
			if (!hashSet.Contains(buildingComplete.gameObject))
			{
				int num6 = Grid.PosToCell(buildingComplete);
				int num7;
				int num8;
				Grid.CellToXY(num6, out num7, out num8);
				if (this.SaveAllBuildings || this.SelectedCells.Contains(num6))
				{
					int[] placementCells = buildingComplete.PlacementCells;
					string text;
					for (int k = 0; k < placementCells.Length; k++)
					{
						int num9 = placementCells[k];
						int xplace;
						int yplace;
						Grid.CellToXY(num9, out xplace, out yplace);
						text = ((Grid.DiseaseIdx[num9] != byte.MaxValue) ? Db.Get().Diseases[(int)Grid.DiseaseIdx[num9]].Id : null);
						if (list.Find((Cell c) => c.location_x == xplace - rootX && c.location_y == yplace - rootY) == null)
						{
							list.Add(new Cell(xplace - rootX, yplace - rootY, Grid.Element[num9].id, Grid.Temperature[num9], Grid.Mass[num9], text, Grid.DiseaseCount[num9], false));
						}
					}
					Orientation rotation = Orientation.Neutral;
					Rotatable component = buildingComplete.gameObject.GetComponent<Rotatable>();
					if (component != null)
					{
						rotation = component.GetOrientation();
					}
					SimHashes element2 = SimHashes.Void;
					float num10 = 280f;
					text = null;
					int disease_count = 0;
					PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
					if (component2 != null)
					{
						element2 = component2.ElementID;
						num10 = component2.Temperature;
						text = ((component2.DiseaseIdx != byte.MaxValue) ? Db.Get().Diseases[(int)component2.DiseaseIdx].Id : null);
						disease_count = component2.DiseaseCount;
					}
					List<Prefab.template_amount_value> list5 = new List<Prefab.template_amount_value>();
					List<Prefab.template_amount_value> list6 = new List<Prefab.template_amount_value>();
					foreach (AmountInstance amountInstance in buildingComplete.gameObject.GetAmounts())
					{
						list5.Add(new Prefab.template_amount_value(amountInstance.amount.Id, amountInstance.value));
					}
					Battery component3 = buildingComplete.GetComponent<Battery>();
					if (component3 != null)
					{
						float joulesAvailable = component3.JoulesAvailable;
						list6.Add(new Prefab.template_amount_value("joulesAvailable", joulesAvailable));
					}
					Unsealable component4 = buildingComplete.GetComponent<Unsealable>();
					if (component4 != null)
					{
						float value = (float)(component4.facingRight ? 1 : 0);
						list6.Add(new Prefab.template_amount_value("sealedDoorDirection", value));
					}
					LogicSwitch component5 = buildingComplete.GetComponent<LogicSwitch>();
					if (component5 != null)
					{
						float value2 = (float)(component5.IsSwitchedOn ? 1 : 0);
						list6.Add(new Prefab.template_amount_value("switchSetting", value2));
					}
					int connections = 0;
					IHaveUtilityNetworkMgr component6 = buildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
					if (component6 != null)
					{
						connections = (int)component6.GetNetworkManager().GetConnections(num6, true);
					}
					string facadeIdId = null;
					BuildingFacade component7 = buildingComplete.GetComponent<BuildingFacade>();
					if (component7 != null)
					{
						facadeIdId = component7.CurrentFacade;
					}
					num7 -= rootX;
					num8 -= rootY;
					num10 = Mathf.Clamp(num10, 1f, 99999f);
					Prefab prefab = new Prefab(buildingComplete.PrefabID().Name, Prefab.Type.Building, num7, num8, element2, num10, 0f, text, disease_count, rotation, list5.ToArray(), list6.ToArray(), connections, facadeIdId);
					Storage component8 = buildingComplete.gameObject.GetComponent<Storage>();
					if (component8 != null)
					{
						foreach (GameObject gameObject in component8.items)
						{
							float units = 0f;
							SimHashes element3 = SimHashes.Vacuum;
							float temp = 280f;
							string disease = null;
							int disease_count2 = 0;
							bool isOre = false;
							PrimaryElement component9 = gameObject.GetComponent<PrimaryElement>();
							if (component9 != null)
							{
								units = component9.Units;
								element3 = component9.ElementID;
								temp = component9.Temperature;
								disease = ((component9.DiseaseIdx != byte.MaxValue) ? Db.Get().Diseases[(int)component9.DiseaseIdx].Id : null);
								disease_count2 = component9.DiseaseCount;
							}
							global::Rottable.Instance smi = gameObject.gameObject.GetSMI<global::Rottable.Instance>();
							if (gameObject.GetComponent<ElementChunk>() != null)
							{
								isOre = true;
							}
							StorageItem storageItem = new StorageItem(gameObject.PrefabID().Name, units, temp, element3, disease, disease_count2, isOre);
							if (smi != null)
							{
								storageItem.rottable.rotAmount = smi.RotValue;
							}
							prefab.AssignStorage(storageItem);
							hashSet.Add(gameObject);
						}
					}
					list2.Add(prefab);
					hashSet.Add(buildingComplete.gameObject);
				}
			}
		}
		for (int l = 0; l < Components.Pickupables.Count; l++)
		{
			if (Components.Pickupables[l].gameObject.activeSelf)
			{
				Pickupable pickupable = Components.Pickupables[l];
				if (!hashSet.Contains(pickupable.gameObject))
				{
					int num11 = Grid.PosToCell(pickupable);
					if ((this.SaveAllPickups || this.SelectedCells.Contains(num11)) && !Components.Pickupables[l].gameObject.GetComponent<MinionBrain>())
					{
						int num12;
						int num13;
						Grid.CellToXY(num11, out num12, out num13);
						num12 -= rootX;
						num13 -= rootY;
						SimHashes element4 = SimHashes.Void;
						float temperature = 280f;
						float units2 = 1f;
						string disease2 = null;
						int disease_count3 = 0;
						float rotAmount = 0f;
						global::Rottable.Instance smi2 = pickupable.gameObject.GetSMI<global::Rottable.Instance>();
						if (smi2 != null)
						{
							rotAmount = smi2.RotValue;
						}
						PrimaryElement component10 = pickupable.gameObject.GetComponent<PrimaryElement>();
						if (component10 != null)
						{
							element4 = component10.ElementID;
							units2 = component10.Units;
							temperature = component10.Temperature;
							disease2 = ((component10.DiseaseIdx != byte.MaxValue) ? Db.Get().Diseases[(int)component10.DiseaseIdx].Id : null);
							disease_count3 = component10.DiseaseCount;
						}
						if (pickupable.gameObject.GetComponent<ElementChunk>() != null)
						{
							Prefab item = new Prefab(pickupable.PrefabID().Name, Prefab.Type.Ore, num12, num13, element4, temperature, units2, disease2, disease_count3, Orientation.Neutral, null, null, 0, null);
							list4.Add(item);
						}
						else
						{
							list3.Add(new Prefab(pickupable.PrefabID().Name, Prefab.Type.Pickupable, num12, num13, element4, temperature, units2, disease2, disease_count3, Orientation.Neutral, null, null, 0, null)
							{
								rottable = new TemplateClasses.Rottable(),
								rottable = 
								{
									rotAmount = rotAmount
								}
							});
						}
						hashSet.Add(pickupable.gameObject);
					}
				}
			}
		}
		this.GetEntities<Crop>(Components.Crops.Items, rootX, rootY, ref list4, ref otherEntities, ref hashSet);
		this.GetEntities<Health>(Components.Health.Items, rootX, rootY, ref list4, ref otherEntities, ref hashSet);
		this.GetEntities<Harvestable>(Components.Harvestables.Items, rootX, rootY, ref list4, ref otherEntities, ref hashSet);
		this.GetEntities<Edible>(Components.Edibles.Items, rootX, rootY, ref list4, ref otherEntities, ref hashSet);
		this.GetEntities<Geyser>(rootX, rootY, ref list4, ref otherEntities, ref hashSet);
		this.GetEntities<OccupyArea>(rootX, rootY, ref list4, ref otherEntities, ref hashSet);
		this.GetEntities<FogOfWarMask>(rootX, rootY, ref list4, ref otherEntities, ref hashSet);
		TemplateContainer templateContainer = new TemplateContainer();
		templateContainer.Init(list, list2, list3, list4, otherEntities);
		return templateContainer;
	}

	// Token: 0x06005F88 RID: 24456 RVA: 0x00237BF8 File Offset: 0x00235DF8
	private void GetEntities<T>(int rootX, int rootY, ref List<Prefab> _primaryElementOres, ref List<Prefab> _otherEntities, ref HashSet<GameObject> _excludeEntities)
	{
		object[] array = UnityEngine.Object.FindObjectsOfType(typeof(T));
		object[] component_collection = array;
		this.GetEntities<object>(component_collection, rootX, rootY, ref _primaryElementOres, ref _otherEntities, ref _excludeEntities);
	}

	// Token: 0x06005F89 RID: 24457 RVA: 0x00237C28 File Offset: 0x00235E28
	private void GetEntities<T>(IEnumerable<T> component_collection, int rootX, int rootY, ref List<Prefab> _primaryElementOres, ref List<Prefab> _otherEntities, ref HashSet<GameObject> _excludeEntities)
	{
		foreach (T t in component_collection)
		{
			if (!_excludeEntities.Contains((t as KMonoBehaviour).gameObject) && (t as KMonoBehaviour).gameObject.activeSelf)
			{
				int num = Grid.PosToCell(t as KMonoBehaviour);
				if (this.SelectedCells.Contains(num) && !(t as KMonoBehaviour).gameObject.GetComponent<MinionBrain>())
				{
					Orientation rotation = Orientation.Neutral;
					Rotatable component = (t as KMonoBehaviour).GetComponent<Rotatable>();
					if (component != null)
					{
						rotation = component.Orientation;
					}
					int num2;
					int num3;
					Grid.CellToXY(num, out num2, out num3);
					num2 -= rootX;
					num3 -= rootY;
					SimHashes simHashes = SimHashes.Void;
					float num4 = 280f;
					float num5 = 1f;
					string text = null;
					int num6 = 0;
					PrimaryElement component2 = (t as KMonoBehaviour).gameObject.GetComponent<PrimaryElement>();
					if (component2 != null)
					{
						simHashes = component2.ElementID;
						num5 = component2.Units;
						num4 = component2.Temperature;
						text = ((component2.DiseaseIdx != byte.MaxValue) ? Db.Get().Diseases[(int)component2.DiseaseIdx].Id : null);
						num6 = component2.DiseaseCount;
					}
					List<Prefab.template_amount_value> list = new List<Prefab.template_amount_value>();
					if ((t as KMonoBehaviour).gameObject.GetAmounts() != null)
					{
						foreach (AmountInstance amountInstance in (t as KMonoBehaviour).gameObject.GetAmounts())
						{
							list.Add(new Prefab.template_amount_value(amountInstance.amount.Id, amountInstance.value));
						}
					}
					if ((t as KMonoBehaviour).gameObject.GetComponent<ElementChunk>() != null)
					{
						string name = (t as KMonoBehaviour).PrefabID().Name;
						Prefab.Type type = Prefab.Type.Ore;
						int loc_x = num2;
						int loc_y = num3;
						SimHashes element = simHashes;
						float temperature = num4;
						float units = num5;
						string disease = text;
						int disease_count = num6;
						Prefab.template_amount_value[] amount_values = list.ToArray();
						Prefab item = new Prefab(name, type, loc_x, loc_y, element, temperature, units, disease, disease_count, rotation, amount_values, null, 0, null);
						_primaryElementOres.Add(item);
						_excludeEntities.Add((t as KMonoBehaviour).gameObject);
					}
					else
					{
						string name2 = (t as KMonoBehaviour).PrefabID().Name;
						Prefab.Type type2 = Prefab.Type.Other;
						int loc_x2 = num2;
						int loc_y2 = num3;
						SimHashes element2 = simHashes;
						float temperature2 = num4;
						float units2 = num5;
						string disease2 = text;
						int disease_count2 = num6;
						Prefab.template_amount_value[] amount_values = list.ToArray();
						Prefab item = new Prefab(name2, type2, loc_x2, loc_y2, element2, temperature2, units2, disease2, disease_count2, rotation, amount_values, null, 0, null);
						_otherEntities.Add(item);
						_excludeEntities.Add((t as KMonoBehaviour).gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06005F8A RID: 24458 RVA: 0x00237F30 File Offset: 0x00236130
	private void OnClickSaveBase()
	{
		TemplateContainer selectionAsAsset = this.GetSelectionAsAsset();
		if (this.SelectedCells.Count <= 0)
		{
			global::Debug.LogWarning("No cells selected. Use buttons above to select the area you want to save.");
			return;
		}
		this.SaveName = this.nameField.text;
		if (this.SaveName == null || this.SaveName == "")
		{
			global::Debug.LogWarning("Invalid save name. Please enter a name in the input field.");
			return;
		}
		selectionAsAsset.SaveToYaml(this.SaveName);
		TemplateCache.Clear();
		TemplateCache.Init();
		PasteBaseTemplateScreen.Instance.RefreshStampButtons();
	}

	// Token: 0x06005F8B RID: 24459 RVA: 0x00237FB4 File Offset: 0x002361B4
	public void ClearSelection()
	{
		for (int i = this.SelectedCells.Count - 1; i >= 0; i--)
		{
			this.RemoveFromSelection(this.SelectedCells[i]);
		}
	}

	// Token: 0x06005F8C RID: 24460 RVA: 0x00237FEB File Offset: 0x002361EB
	public void DestroySelection()
	{
	}

	// Token: 0x06005F8D RID: 24461 RVA: 0x00237FED File Offset: 0x002361ED
	public void DeconstructSelection()
	{
	}

	// Token: 0x06005F8E RID: 24462 RVA: 0x00237FF0 File Offset: 0x002361F0
	public void AddToSelection(int cell)
	{
		if (!this.SelectedCells.Contains(cell))
		{
			GameObject gameObject = Util.KInstantiate(this.Placer, null, null);
			Grid.Objects[cell, 7] = gameObject;
			Vector3 vector = Grid.CellToPosCBC(cell, this.visualizerLayer);
			float num = -0.15f;
			vector.z += num;
			gameObject.transform.SetPosition(vector);
			this.SelectedCells.Add(cell);
		}
	}

	// Token: 0x06005F8F RID: 24463 RVA: 0x00238064 File Offset: 0x00236264
	public void RemoveFromSelection(int cell)
	{
		if (this.SelectedCells.Contains(cell))
		{
			GameObject gameObject = Grid.Objects[cell, 7];
			if (gameObject != null)
			{
				gameObject.DeleteObject();
			}
			this.SelectedCells.Remove(cell);
		}
	}

	// Token: 0x04004048 RID: 16456
	private bool SaveAllBuildings;

	// Token: 0x04004049 RID: 16457
	private bool SaveAllPickups;

	// Token: 0x0400404A RID: 16458
	public KButton saveBaseButton;

	// Token: 0x0400404B RID: 16459
	public KButton clearButton;

	// Token: 0x0400404C RID: 16460
	private TemplateContainer pasteAndSelectAsset;

	// Token: 0x0400404D RID: 16461
	public KButton AddSelectionButton;

	// Token: 0x0400404E RID: 16462
	public KButton RemoveSelectionButton;

	// Token: 0x0400404F RID: 16463
	public KButton clearSelectionButton;

	// Token: 0x04004050 RID: 16464
	public KButton DestroyButton;

	// Token: 0x04004051 RID: 16465
	public KButton DeconstructButton;

	// Token: 0x04004052 RID: 16466
	public KButton MoveButton;

	// Token: 0x04004053 RID: 16467
	public TemplateContainer moveAsset;

	// Token: 0x04004054 RID: 16468
	public KInputTextField nameField;

	// Token: 0x04004055 RID: 16469
	private string SaveName = "enter_template_name";

	// Token: 0x04004056 RID: 16470
	public GameObject Placer;

	// Token: 0x04004057 RID: 16471
	public Grid.SceneLayer visualizerLayer = Grid.SceneLayer.Move;

	// Token: 0x04004058 RID: 16472
	public List<int> SelectedCells = new List<int>();
}
