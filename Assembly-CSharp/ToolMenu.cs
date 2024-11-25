using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x02000BB8 RID: 3000
public class ToolMenu : KScreen
{
	// Token: 0x06005B0A RID: 23306 RVA: 0x00211A7D File Offset: 0x0020FC7D
	public static void DestroyInstance()
	{
		ToolMenu.Instance = null;
	}

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06005B0B RID: 23307 RVA: 0x00211A85 File Offset: 0x0020FC85
	public PriorityScreen PriorityScreen
	{
		get
		{
			return this.priorityScreen;
		}
	}

	// Token: 0x06005B0C RID: 23308 RVA: 0x00211A8D File Offset: 0x0020FC8D
	public override float GetSortKey()
	{
		return 5f;
	}

	// Token: 0x06005B0D RID: 23309 RVA: 0x00211A94 File Offset: 0x0020FC94
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ToolMenu.Instance = this;
		Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.Prefab_priorityScreen.gameObject, base.gameObject, false);
		this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), false);
	}

	// Token: 0x06005B0E RID: 23310 RVA: 0x00211AFE File Offset: 0x0020FCFE
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.OnInputChange));
		base.OnForcedCleanUp();
	}

	// Token: 0x06005B0F RID: 23311 RVA: 0x00211B1C File Offset: 0x0020FD1C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		Game.Instance.Unsubscribe(this.refreshScaleHandle);
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x00211B50 File Offset: 0x0020FD50
	private void OnOverlayChanged(object overlay_data)
	{
		HashedString y = (HashedString)overlay_data;
		if (PlayerController.Instance.ActiveTool != null && PlayerController.Instance.ActiveTool.ViewMode != OverlayModes.None.ID && PlayerController.Instance.ActiveTool.ViewMode != y)
		{
			this.ChooseCollection(null, true);
			this.ChooseTool(null);
		}
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x00211BB8 File Offset: 0x0020FDB8
	protected override void OnSpawn()
	{
		this.activateOnSpawn = true;
		base.OnSpawn();
		this.CreateSandBoxTools();
		this.CreateBasicTools();
		this.rows.Add(this.sandboxTools);
		this.rows.Add(this.basicTools);
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.InstantiateCollectionsUI(row);
		});
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildRowToggles(row);
		});
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildToolToggles(row);
		});
		this.ChooseCollection(null, true);
		this.priorityScreen.gameObject.SetActive(false);
		this.ToggleSandboxUI(null);
		KInputManager.InputChange.AddListener(new UnityAction(this.OnInputChange));
		Game.Instance.Subscribe(-1948169901, new Action<object>(this.ToggleSandboxUI));
		this.ResetToolDisplayPlane();
		this.refreshScaleHandle = Game.Instance.Subscribe(-442024484, new Action<object>(this.RefreshScale));
		this.RefreshScale(null);
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x00211CC8 File Offset: 0x0020FEC8
	private void RefreshScale(object data = null)
	{
		int num = 14;
		int num2 = 16;
		foreach (ToolMenu.ToolCollection toolCollection in this.sandboxTools)
		{
			LocText componentInChildren = toolCollection.toggle.GetComponentInChildren<LocText>();
			if (componentInChildren != null)
			{
				componentInChildren.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? num2 : num);
			}
		}
		foreach (ToolMenu.ToolCollection toolCollection2 in this.basicTools)
		{
			LocText componentInChildren2 = toolCollection2.toggle.GetComponentInChildren<LocText>();
			if (componentInChildren2 != null)
			{
				componentInChildren2.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? num2 : num);
			}
		}
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x00211DA4 File Offset: 0x0020FFA4
	public void OnInputChange()
	{
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildRowToggles(row);
		});
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.BuildToolToggles(row);
		});
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x00211DD4 File Offset: 0x0020FFD4
	private void ResetToolDisplayPlane()
	{
		this.toolEffectDisplayPlane = this.CreateToolDisplayPlane("Overlay", World.Instance.transform);
		this.toolEffectDisplayPlaneTexture = this.CreatePlaneTexture(out this.toolEffectDisplayBytes, Grid.WidthInCells, Grid.HeightInCells);
		this.toolEffectDisplayPlane.GetComponent<Renderer>().sharedMaterial = this.toolEffectDisplayMaterial;
		this.toolEffectDisplayPlane.GetComponent<Renderer>().sharedMaterial.mainTexture = this.toolEffectDisplayPlaneTexture;
		this.toolEffectDisplayPlane.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -6f));
		this.RefreshToolDisplayPlaneColor();
	}

	// Token: 0x06005B15 RID: 23317 RVA: 0x00211E80 File Offset: 0x00210080
	private GameObject CreateToolDisplayPlane(string layer, Transform parent)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
		gameObject.name = "toolEffectDisplayPlane";
		gameObject.SetLayerRecursively(LayerMask.NameToLayer(layer));
		UnityEngine.Object.Destroy(gameObject.GetComponent<Collider>());
		if (parent != null)
		{
			gameObject.transform.SetParent(parent);
		}
		gameObject.transform.SetPosition(Vector3.zero);
		gameObject.transform.localScale = new Vector3(Grid.WidthInMeters / -10f, 1f, Grid.HeightInMeters / -10f);
		gameObject.transform.eulerAngles = new Vector3(270f, 0f, 0f);
		gameObject.GetComponent<MeshRenderer>().reflectionProbeUsage = ReflectionProbeUsage.Off;
		return gameObject;
	}

	// Token: 0x06005B16 RID: 23318 RVA: 0x00211F33 File Offset: 0x00210133
	private Texture2D CreatePlaneTexture(out byte[] textureBytes, int width, int height)
	{
		textureBytes = new byte[width * height * 4];
		return new Texture2D(width, height, TextureUtil.TextureFormatToGraphicsFormat(TextureFormat.RGBA32), TextureCreationFlags.None)
		{
			name = "toolEffectDisplayPlane",
			wrapMode = TextureWrapMode.Clamp,
			filterMode = FilterMode.Point
		};
	}

	// Token: 0x06005B17 RID: 23319 RVA: 0x00211F68 File Offset: 0x00210168
	private void Update()
	{
		this.RefreshToolDisplayPlaneColor();
	}

	// Token: 0x06005B18 RID: 23320 RVA: 0x00211F70 File Offset: 0x00210170
	private void RefreshToolDisplayPlaneColor()
	{
		if (PlayerController.Instance.ActiveTool == null || PlayerController.Instance.ActiveTool == SelectTool.Instance)
		{
			this.toolEffectDisplayPlane.SetActive(false);
			return;
		}
		PlayerController.Instance.ActiveTool.GetOverlayColorData(out this.colors);
		Array.Clear(this.toolEffectDisplayBytes, 0, this.toolEffectDisplayBytes.Length);
		if (this.colors != null)
		{
			foreach (ToolMenu.CellColorData cellColorData in this.colors)
			{
				if (Grid.IsValidCell(cellColorData.cell))
				{
					int num = cellColorData.cell * 4;
					if (num >= 0)
					{
						this.toolEffectDisplayBytes[num] = (byte)(Mathf.Min(cellColorData.color.r, 1f) * 255f);
						this.toolEffectDisplayBytes[num + 1] = (byte)(Mathf.Min(cellColorData.color.g, 1f) * 255f);
						this.toolEffectDisplayBytes[num + 2] = (byte)(Mathf.Min(cellColorData.color.b, 1f) * 255f);
						this.toolEffectDisplayBytes[num + 3] = (byte)(Mathf.Min(cellColorData.color.a, 1f) * 255f);
					}
				}
			}
		}
		if (!this.toolEffectDisplayPlane.activeSelf)
		{
			this.toolEffectDisplayPlane.SetActive(true);
		}
		this.toolEffectDisplayPlaneTexture.LoadRawTextureData(this.toolEffectDisplayBytes);
		this.toolEffectDisplayPlaneTexture.Apply();
	}

	// Token: 0x06005B19 RID: 23321 RVA: 0x00212118 File Offset: 0x00210318
	public void ToggleSandboxUI(object data = null)
	{
		this.ClearSelection();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		this.sandboxTools[0].toggle.transform.parent.transform.parent.gameObject.SetActive(Game.Instance.SandboxModeActive);
	}

	// Token: 0x06005B1A RID: 23322 RVA: 0x00212174 File Offset: 0x00210374
	public static ToolMenu.ToolCollection CreateToolCollection(LocString collection_name, string icon_name, global::Action hotkey, string tool_name, LocString tooltip, bool largeIcon)
	{
		ToolMenu.ToolCollection toolCollection = new ToolMenu.ToolCollection(collection_name, icon_name, "", false, global::Action.NumActions, largeIcon);
		new ToolMenu.ToolInfo(collection_name, icon_name, hotkey, tool_name, toolCollection, tooltip, null, null);
		return toolCollection;
	}

	// Token: 0x06005B1B RID: 23323 RVA: 0x002121B8 File Offset: 0x002103B8
	private void CreateSandBoxTools()
	{
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.BRUSH.NAME, "brush", global::Action.SandboxBrush, "SandboxBrushTool", UI.SANDBOXTOOLS.SETTINGS.BRUSH.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SPRINKLE.NAME, "sprinkle", global::Action.SandboxSprinkle, "SandboxSprinkleTool", UI.SANDBOXTOOLS.SETTINGS.SPRINKLE.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.FLOOD.NAME, "flood", global::Action.SandboxFlood, "SandboxFloodTool", UI.SANDBOXTOOLS.SETTINGS.FLOOD.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SAMPLE.NAME, "sample", global::Action.SandboxSample, "SandboxSampleTool", UI.SANDBOXTOOLS.SETTINGS.SAMPLE.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.HEATGUN.NAME, "temperature", global::Action.SandboxHeatGun, "SandboxHeatTool", UI.SANDBOXTOOLS.SETTINGS.HEATGUN.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.STRESSTOOL.NAME, "crew_state_happy", global::Action.SandboxStressTool, "SandboxStressTool", UI.SANDBOXTOOLS.SETTINGS.STRESS.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SPAWNER.NAME, "spawn", global::Action.SandboxSpawnEntity, "SandboxSpawnerTool", UI.SANDBOXTOOLS.SETTINGS.SPAWNER.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.CLEAR_FLOOR.NAME, "clear_floor", global::Action.SandboxClearFloor, "SandboxClearFloorTool", UI.SANDBOXTOOLS.SETTINGS.CLEAR_FLOOR.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.DESTROY.NAME, "destroy", global::Action.SandboxDestroy, "SandboxDestroyerTool", UI.SANDBOXTOOLS.SETTINGS.DESTROY.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.FOW.NAME, "reveal", global::Action.SandboxReveal, "SandboxFOWTool", UI.SANDBOXTOOLS.SETTINGS.FOW.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.CRITTER.NAME, "critter", global::Action.SandboxCritterTool, "SandboxCritterTool", UI.SANDBOXTOOLS.SETTINGS.CRITTER.TOOLTIP, false));
		this.sandboxTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.NAME, "sandbox_storytrait", global::Action.SandboxStoryTraitTool, "SandboxStoryTraitTool", UI.SANDBOXTOOLS.SETTINGS.SPAWN_STORY_TRAIT.TOOLTIP, false));
	}

	// Token: 0x06005B1C RID: 23324 RVA: 0x002123C0 File Offset: 0x002105C0
	private void CreateBasicTools()
	{
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DIG.NAME, "icon_action_dig", global::Action.Dig, "DigTool", UI.TOOLTIPS.DIGBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.CANCEL.NAME, "icon_action_cancel", global::Action.BuildingCancel, "CancelTool", UI.TOOLTIPS.CANCELBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DECONSTRUCT.NAME, "icon_action_deconstruct", global::Action.BuildingDeconstruct, "DeconstructTool", UI.TOOLTIPS.DECONSTRUCTBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.PRIORITIZE.NAME, "icon_action_prioritize", global::Action.Prioritize, "PrioritizeTool", UI.TOOLTIPS.PRIORITIZEBUTTON, true));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DISINFECT.NAME, "icon_action_disinfect", global::Action.Disinfect, "DisinfectTool", UI.TOOLTIPS.DISINFECTBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.MARKFORSTORAGE.NAME, "icon_action_store", global::Action.Clear, "ClearTool", UI.TOOLTIPS.CLEARBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.ATTACK.NAME, "icon_action_attack", global::Action.Attack, "AttackTool", UI.TOOLTIPS.ATTACKBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.MOP.NAME, "icon_action_mop", global::Action.Mop, "MopTool", UI.TOOLTIPS.MOPBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.CAPTURE.NAME, "icon_action_capture", global::Action.Capture, "CaptureTool", UI.TOOLTIPS.CAPTUREBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.HARVEST.NAME, "icon_action_harvest", global::Action.Harvest, "HarvestTool", UI.TOOLTIPS.HARVESTBUTTON, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.EMPTY_PIPE.NAME, "icon_action_empty_pipes", global::Action.EmptyPipe, "EmptyPipeTool", UI.TOOLS.EMPTY_PIPE.TOOLTIP, false));
		this.basicTools.Add(ToolMenu.CreateToolCollection(UI.TOOLS.DISCONNECT.NAME, "icon_action_disconnect", global::Action.Disconnect, "DisconnectTool", UI.TOOLS.DISCONNECT.TOOLTIP, false));
	}

	// Token: 0x06005B1D RID: 23325 RVA: 0x002125C8 File Offset: 0x002107C8
	private void InstantiateCollectionsUI(IList<ToolMenu.ToolCollection> collections)
	{
		GameObject parent = Util.KInstantiateUI(this.prefabToolRow, base.gameObject, true);
		GameObject gameObject = Util.KInstantiateUI(this.largeToolSet, parent, true);
		GameObject gameObject2 = Util.KInstantiateUI(this.smallToolSet, parent, true);
		GameObject gameObject3 = Util.KInstantiateUI(this.smallToolBottomRow, gameObject2, true);
		GameObject gameObject4 = Util.KInstantiateUI(this.smallToolTopRow, gameObject2, true);
		GameObject gameObject5 = Util.KInstantiateUI(this.sandboxToolSet, parent, true);
		bool flag = true;
		int num = 0;
		for (int i = 0; i < collections.Count; i++)
		{
			GameObject parent2;
			if (collections == this.sandboxTools)
			{
				parent2 = gameObject5;
			}
			else if (collections[i].largeIcon)
			{
				parent2 = gameObject;
			}
			else
			{
				parent2 = (flag ? gameObject4 : gameObject3);
				flag = !flag;
				num++;
			}
			ToolMenu.ToolCollection tc = collections[i];
			tc.toggle = Util.KInstantiateUI((collections[i].tools.Count > 1) ? this.collectionIconPrefab : ((collections == this.sandboxTools) ? this.sandboxToolIconPrefab : (collections[i].largeIcon ? this.toolIconLargePrefab : this.toolIconPrefab)), parent2, true);
			KToggle component = tc.toggle.GetComponent<KToggle>();
			component.soundPlayer.Enabled = false;
			component.onClick += delegate()
			{
				if (this.currentlySelectedCollection == tc && tc.tools.Count >= 1)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false));
				}
				this.ChooseCollection(tc, true);
			};
			if (tc.tools != null)
			{
				GameObject gameObject6;
				if (tc.tools.Count < this.smallCollectionMax)
				{
					gameObject6 = Util.KInstantiateUI(this.Prefab_collectionContainer, parent2, true);
					gameObject6.transform.SetSiblingIndex(gameObject6.transform.GetSiblingIndex() - 1);
					gameObject6.transform.localScale = Vector3.one;
					gameObject6.rectTransform().sizeDelta = new Vector2((float)(tc.tools.Count * 75), 50f);
					tc.MaskContainer = gameObject6.GetComponentInChildren<Mask>().gameObject;
					gameObject6.SetActive(false);
				}
				else
				{
					gameObject6 = Util.KInstantiateUI(this.Prefab_collectionContainerWindow, parent2, true);
					gameObject6.transform.localScale = Vector3.one;
					gameObject6.GetComponentInChildren<LocText>().SetText(tc.text.ToUpper());
					tc.MaskContainer = gameObject6.GetComponentInChildren<GridLayoutGroup>().gameObject;
					gameObject6.SetActive(false);
				}
				tc.UIMenuDisplay = gameObject6;
				Action<object> <>9__2;
				for (int j = 0; j < tc.tools.Count; j++)
				{
					ToolMenu.ToolInfo ti = tc.tools[j];
					GameObject gameObject7 = Util.KInstantiateUI((collections == this.sandboxTools) ? this.sandboxToolIconPrefab : (collections[i].largeIcon ? this.toolIconLargePrefab : this.toolIconPrefab), tc.MaskContainer, true);
					gameObject7.name = ti.text;
					ti.toggle = gameObject7.GetComponent<KToggle>();
					if (ti.collection.tools.Count > 1)
					{
						RectTransform rectTransform = ti.toggle.gameObject.GetComponentInChildren<SetTextStyleSetting>().rectTransform();
						if (gameObject7.name.Length > 12)
						{
							rectTransform.GetComponent<SetTextStyleSetting>().SetStyle(this.CategoryLabelTextStyle_LeftAlign);
							rectTransform.anchoredPosition = new Vector2(16f, rectTransform.anchoredPosition.y);
						}
					}
					ti.toggle.onClick += delegate()
					{
						this.ChooseTool(ti);
					};
					ExpandRevealUIContent component2 = tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>();
					Action<object> completeCallback;
					if ((completeCallback = <>9__2) == null)
					{
						completeCallback = (<>9__2 = delegate(object s)
						{
							this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
							tc.UIMenuDisplay.SetActive(false);
						});
					}
					component2.Collapse(completeCallback);
				}
			}
		}
		if (num > 0 && num % 2 == 0)
		{
			gameObject3.GetComponent<HorizontalLayoutGroup>().padding.left = 26;
			gameObject4.GetComponent<HorizontalLayoutGroup>().padding.right = 26;
		}
		if (gameObject.transform.childCount == 0)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		if (gameObject3.transform.childCount == 0 && gameObject4.transform.childCount == 0)
		{
			UnityEngine.Object.Destroy(gameObject2);
		}
		if (gameObject5.transform.childCount == 0)
		{
			UnityEngine.Object.Destroy(gameObject5);
		}
	}

	// Token: 0x06005B1E RID: 23326 RVA: 0x00212A84 File Offset: 0x00210C84
	private void ChooseTool(ToolMenu.ToolInfo tool)
	{
		if (this.currentlySelectedTool == tool)
		{
			return;
		}
		if (this.currentlySelectedTool != tool)
		{
			this.currentlySelectedTool = tool;
			if (this.currentlySelectedTool != null && this.currentlySelectedTool.onSelectCallback != null)
			{
				this.currentlySelectedTool.onSelectCallback(this.currentlySelectedTool);
			}
		}
		if (this.currentlySelectedTool != null)
		{
			this.currentlySelectedCollection = this.currentlySelectedTool.collection;
			foreach (InterfaceTool interfaceTool in PlayerController.Instance.tools)
			{
				if (this.currentlySelectedTool.toolName == interfaceTool.name)
				{
					UISounds.PlaySound(UISounds.Sound.ClickObject);
					this.activeTool = interfaceTool;
					PlayerController.Instance.ActivateTool(interfaceTool);
					break;
				}
			}
		}
		else
		{
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
		}
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.RefreshRowDisplay(row);
		});
	}

	// Token: 0x06005B1F RID: 23327 RVA: 0x00212B68 File Offset: 0x00210D68
	private void RefreshRowDisplay(IList<ToolMenu.ToolCollection> row)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection tc = row[i];
			if (this.currentlySelectedTool != null && this.currentlySelectedTool.collection == tc)
			{
				if (!tc.UIMenuDisplay.activeSelf || tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing)
				{
					if (tc.tools.Count > 1)
					{
						tc.UIMenuDisplay.SetActive(true);
						if (tc.tools.Count < this.smallCollectionMax)
						{
							float speedScale = Mathf.Clamp(1f - (float)tc.tools.Count * 0.15f, 0.5f, 1f);
							tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().speedScale = speedScale;
						}
						tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Expand(delegate(object s)
						{
							this.SetToggleState(tc.toggle.GetComponent<KToggle>(), true);
						});
					}
					else
					{
						this.currentlySelectedTool = tc.tools[0];
					}
				}
			}
			else if (tc.UIMenuDisplay.activeSelf && !tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing && tc.tools.Count > 0)
			{
				tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
				{
					this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
					tc.UIMenuDisplay.SetActive(false);
				});
			}
			for (int j = 0; j < tc.tools.Count; j++)
			{
				if (tc.tools[j] == this.currentlySelectedTool)
				{
					this.SetToggleState(tc.tools[j].toggle, true);
				}
				else
				{
					this.SetToggleState(tc.tools[j].toggle, false);
				}
			}
		}
	}

	// Token: 0x06005B20 RID: 23328 RVA: 0x00212D7E File Offset: 0x00210F7E
	public void TurnLargeCollectionOff()
	{
		if (this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Count > this.smallCollectionMax)
		{
			this.ChooseCollection(null, true);
		}
	}

	// Token: 0x06005B21 RID: 23329 RVA: 0x00212DA8 File Offset: 0x00210FA8
	private void ChooseCollection(ToolMenu.ToolCollection collection, bool autoSelectTool = true)
	{
		if (collection == this.currentlySelectedCollection)
		{
			if (collection != null && collection.tools.Count > 1)
			{
				this.currentlySelectedCollection = null;
				if (this.currentlySelectedTool != null)
				{
					this.ChooseTool(null);
				}
			}
			else if (this.currentlySelectedTool != null && this.currentlySelectedCollection.tools.Contains(this.currentlySelectedTool) && this.currentlySelectedCollection.tools.Count == 1)
			{
				this.currentlySelectedCollection = null;
				this.ChooseTool(null);
			}
		}
		else
		{
			this.currentlySelectedCollection = collection;
		}
		this.rows.ForEach(delegate(List<ToolMenu.ToolCollection> row)
		{
			this.OpenOrCloseCollectionsInRow(row, true);
		});
	}

	// Token: 0x06005B22 RID: 23330 RVA: 0x00212E48 File Offset: 0x00211048
	private void OpenOrCloseCollectionsInRow(IList<ToolMenu.ToolCollection> row, bool autoSelectTool = true)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection tc = row[i];
			if (this.currentlySelectedCollection == tc)
			{
				if ((this.currentlySelectedCollection.tools != null && this.currentlySelectedCollection.tools.Count == 1) || autoSelectTool)
				{
					this.ChooseTool(this.currentlySelectedCollection.tools[0]);
				}
			}
			else if (tc.UIMenuDisplay.activeSelf && !tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapsing)
			{
				tc.UIMenuDisplay.GetComponent<ExpandRevealUIContent>().Collapse(delegate(object s)
				{
					this.SetToggleState(tc.toggle.GetComponent<KToggle>(), false);
					tc.UIMenuDisplay.SetActive(false);
				});
			}
			this.SetToggleState(tc.toggle.GetComponent<KToggle>(), this.currentlySelectedCollection == tc);
		}
	}

	// Token: 0x06005B23 RID: 23331 RVA: 0x00212F42 File Offset: 0x00211142
	private void SetToggleState(KToggle toggle, bool state)
	{
		if (state)
		{
			toggle.Select();
			toggle.isOn = true;
			return;
		}
		toggle.Deselect();
		toggle.isOn = false;
	}

	// Token: 0x06005B24 RID: 23332 RVA: 0x00212F62 File Offset: 0x00211162
	public void ClearSelection()
	{
		if (this.currentlySelectedCollection != null)
		{
			this.ChooseCollection(null, true);
		}
		if (this.currentlySelectedTool != null)
		{
			this.ChooseTool(null);
		}
	}

	// Token: 0x06005B25 RID: 23333 RVA: 0x00212F84 File Offset: 0x00211184
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			if (e.IsAction(global::Action.ToggleSandboxTools))
			{
				if (Application.isEditor)
				{
					DebugUtil.LogArgs(new object[]
					{
						"Force-enabling sandbox mode because we're in editor."
					});
					SaveGame.Instance.sandboxEnabled = true;
				}
				if (SaveGame.Instance.sandboxEnabled)
				{
					Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
					KMonoBehaviour.PlaySound(Game.Instance.SandboxModeActive ? GlobalAssets.GetSound("SandboxTool_Toggle_On", false) : GlobalAssets.GetSound("SandboxTool_Toggle_Off", false));
				}
			}
			foreach (List<ToolMenu.ToolCollection> list in this.rows)
			{
				if (list != this.sandboxTools || Game.Instance.SandboxModeActive)
				{
					int i = 0;
					while (i < list.Count)
					{
						global::Action toolHotkey = list[i].hotkey;
						if (toolHotkey != global::Action.NumActions && e.IsAction(toolHotkey) && (this.currentlySelectedCollection == null || (this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Find((ToolMenu.ToolInfo t) => GameInputMapping.CompareActionKeyCodes(t.hotkey, toolHotkey)) == null)))
						{
							if (this.currentlySelectedCollection != list[i])
							{
								this.ChooseCollection(list[i], false);
								this.ChooseTool(list[i].tools[0]);
								break;
							}
							if (this.currentlySelectedCollection.tools.Count <= 1)
							{
								break;
							}
							e.Consumed = true;
							this.ChooseCollection(null, true);
							this.ChooseTool(null);
							string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
							if (sound != null)
							{
								KMonoBehaviour.PlaySound(sound);
								break;
							}
							break;
						}
						else
						{
							for (int j = 0; j < list[i].tools.Count; j++)
							{
								if ((this.currentlySelectedCollection == null && list[i].tools.Count == 1) || this.currentlySelectedCollection == list[i] || (this.currentlySelectedCollection != null && this.currentlySelectedCollection.tools.Count == 1 && list[i].tools.Count == 1))
								{
									global::Action hotkey = list[i].tools[j].hotkey;
									if (e.IsAction(hotkey) && e.TryConsume(hotkey))
									{
										if (list[i].tools.Count == 1 && this.currentlySelectedCollection != list[i])
										{
											this.ChooseCollection(list[i], false);
										}
										else if (this.currentlySelectedTool != list[i].tools[j])
										{
											this.ChooseTool(list[i].tools[j]);
										}
									}
									else if (GameInputMapping.CompareActionKeyCodes(e.GetAction(), hotkey))
									{
										e.Consumed = true;
									}
								}
							}
							i++;
						}
					}
				}
			}
			if ((this.currentlySelectedTool != null || this.currentlySelectedCollection != null) && !e.Consumed)
			{
				if (e.TryConsume(global::Action.Escape))
				{
					string sound2 = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
					if (sound2 != null)
					{
						KMonoBehaviour.PlaySound(sound2);
					}
					if (this.currentlySelectedCollection != null)
					{
						this.ChooseCollection(null, true);
					}
					if (this.currentlySelectedTool != null)
					{
						this.ChooseTool(null);
					}
					SelectTool.Instance.Activate();
				}
			}
			else if (!PlayerController.Instance.IsUsingDefaultTool() && !e.Consumed && e.TryConsume(global::Action.Escape))
			{
				SelectTool.Instance.Activate();
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005B26 RID: 23334 RVA: 0x0021336C File Offset: 0x0021156C
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			if ((this.currentlySelectedTool != null || this.currentlySelectedCollection != null) && !e.Consumed)
			{
				if (PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
				{
					string sound = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
					if (sound != null)
					{
						KMonoBehaviour.PlaySound(sound);
					}
					if (this.currentlySelectedCollection != null)
					{
						this.ChooseCollection(null, true);
					}
					if (this.currentlySelectedTool != null)
					{
						this.ChooseTool(null);
					}
					SelectTool.Instance.Activate();
				}
			}
			else if (!PlayerController.Instance.IsUsingDefaultTool() && !e.Consumed && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
			{
				SelectTool.Instance.Activate();
				string sound2 = GlobalAssets.GetSound(PlayerController.Instance.ActiveTool.GetDeactivateSound(), false);
				if (sound2 != null)
				{
					KMonoBehaviour.PlaySound(sound2);
				}
			}
		}
		base.OnKeyUp(e);
	}

	// Token: 0x06005B27 RID: 23335 RVA: 0x0021344C File Offset: 0x0021164C
	protected void BuildRowToggles(IList<ToolMenu.ToolCollection> row)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection toolCollection = row[i];
			if (!(toolCollection.toggle == null))
			{
				GameObject toggle = toolCollection.toggle;
				Sprite sprite = Assets.GetSprite(toolCollection.icon);
				if (sprite != null)
				{
					toggle.transform.Find("FG").GetComponent<Image>().sprite = sprite;
				}
				Transform transform = toggle.transform.Find("Text");
				if (transform != null)
				{
					LocText component = transform.GetComponent<LocText>();
					if (component != null)
					{
						component.text = toolCollection.text;
					}
				}
				ToolTip component2 = toggle.GetComponent<ToolTip>();
				if (component2)
				{
					if (row[i].tools.Count == 1)
					{
						string newString = GameUtil.ReplaceHotkeyString(row[i].tools[0].tooltip, row[i].tools[0].hotkey);
						component2.ClearMultiStringTooltip();
						component2.AddMultiStringTooltip(row[i].tools[0].text, this.TooltipHeader);
						component2.AddMultiStringTooltip(newString, this.ToggleToolTipTextStyleSetting);
					}
					else
					{
						string text = row[i].tooltip;
						if (row[i].hotkey != global::Action.NumActions)
						{
							text = GameUtil.ReplaceHotkeyString(text, row[i].hotkey);
						}
						component2.ClearMultiStringTooltip();
						component2.AddMultiStringTooltip(text, this.ToggleToolTipTextStyleSetting);
					}
				}
			}
		}
	}

	// Token: 0x06005B28 RID: 23336 RVA: 0x002135E8 File Offset: 0x002117E8
	protected void BuildToolToggles(IList<ToolMenu.ToolCollection> row)
	{
		for (int i = 0; i < row.Count; i++)
		{
			ToolMenu.ToolCollection toolCollection = row[i];
			if (!(toolCollection.toggle == null))
			{
				for (int j = 0; j < toolCollection.tools.Count; j++)
				{
					GameObject gameObject = toolCollection.tools[j].toggle.gameObject;
					Sprite sprite = Assets.GetSprite(toolCollection.icon);
					if (sprite != null)
					{
						gameObject.transform.Find("FG").GetComponent<Image>().sprite = sprite;
					}
					Transform transform = gameObject.transform.Find("Text");
					if (transform != null)
					{
						LocText component = transform.GetComponent<LocText>();
						if (component != null)
						{
							component.text = toolCollection.tools[j].text;
						}
					}
					ToolTip component2 = gameObject.GetComponent<ToolTip>();
					if (component2)
					{
						string newString = (toolCollection.tools.Count > 1) ? GameUtil.ReplaceHotkeyString(toolCollection.tools[j].tooltip, toolCollection.hotkey, toolCollection.tools[j].hotkey) : GameUtil.ReplaceHotkeyString(toolCollection.tools[j].tooltip, toolCollection.tools[j].hotkey);
						component2.ClearMultiStringTooltip();
						component2.AddMultiStringTooltip(newString, this.ToggleToolTipTextStyleSetting);
					}
				}
			}
		}
	}

	// Token: 0x06005B29 RID: 23337 RVA: 0x00213764 File Offset: 0x00211964
	public bool HasUniqueKeyBindings()
	{
		bool result = true;
		this.boundRootActions.Clear();
		foreach (List<ToolMenu.ToolCollection> list in this.rows)
		{
			foreach (ToolMenu.ToolCollection toolCollection in list)
			{
				if (this.boundRootActions.Contains(toolCollection.hotkey))
				{
					result = false;
					break;
				}
				this.boundRootActions.Add(toolCollection.hotkey);
				this.boundSubgroupActions.Clear();
				foreach (ToolMenu.ToolInfo toolInfo in toolCollection.tools)
				{
					if (this.boundSubgroupActions.Contains(toolInfo.hotkey))
					{
						result = false;
						break;
					}
					this.boundSubgroupActions.Add(toolInfo.hotkey);
				}
			}
		}
		return result;
	}

	// Token: 0x06005B2A RID: 23338 RVA: 0x002138A0 File Offset: 0x00211AA0
	private void OnPriorityClicked(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x04003BF7 RID: 15351
	public static ToolMenu Instance;

	// Token: 0x04003BF8 RID: 15352
	public GameObject Prefab_collectionContainer;

	// Token: 0x04003BF9 RID: 15353
	public GameObject Prefab_collectionContainerWindow;

	// Token: 0x04003BFA RID: 15354
	public PriorityScreen Prefab_priorityScreen;

	// Token: 0x04003BFB RID: 15355
	public GameObject toolIconPrefab;

	// Token: 0x04003BFC RID: 15356
	public GameObject toolIconLargePrefab;

	// Token: 0x04003BFD RID: 15357
	public GameObject sandboxToolIconPrefab;

	// Token: 0x04003BFE RID: 15358
	public GameObject collectionIconPrefab;

	// Token: 0x04003BFF RID: 15359
	public GameObject prefabToolRow;

	// Token: 0x04003C00 RID: 15360
	public GameObject largeToolSet;

	// Token: 0x04003C01 RID: 15361
	public GameObject smallToolSet;

	// Token: 0x04003C02 RID: 15362
	public GameObject smallToolBottomRow;

	// Token: 0x04003C03 RID: 15363
	public GameObject smallToolTopRow;

	// Token: 0x04003C04 RID: 15364
	public GameObject sandboxToolSet;

	// Token: 0x04003C05 RID: 15365
	private PriorityScreen priorityScreen;

	// Token: 0x04003C06 RID: 15366
	public ToolParameterMenu toolParameterMenu;

	// Token: 0x04003C07 RID: 15367
	public GameObject sandboxToolParameterMenu;

	// Token: 0x04003C08 RID: 15368
	private GameObject toolEffectDisplayPlane;

	// Token: 0x04003C09 RID: 15369
	private Texture2D toolEffectDisplayPlaneTexture;

	// Token: 0x04003C0A RID: 15370
	public Material toolEffectDisplayMaterial;

	// Token: 0x04003C0B RID: 15371
	private byte[] toolEffectDisplayBytes;

	// Token: 0x04003C0C RID: 15372
	private List<List<ToolMenu.ToolCollection>> rows = new List<List<ToolMenu.ToolCollection>>();

	// Token: 0x04003C0D RID: 15373
	public List<ToolMenu.ToolCollection> basicTools = new List<ToolMenu.ToolCollection>();

	// Token: 0x04003C0E RID: 15374
	public List<ToolMenu.ToolCollection> sandboxTools = new List<ToolMenu.ToolCollection>();

	// Token: 0x04003C0F RID: 15375
	public ToolMenu.ToolCollection currentlySelectedCollection;

	// Token: 0x04003C10 RID: 15376
	public ToolMenu.ToolInfo currentlySelectedTool;

	// Token: 0x04003C11 RID: 15377
	public InterfaceTool activeTool;

	// Token: 0x04003C12 RID: 15378
	private Coroutine activeOpenAnimationRoutine;

	// Token: 0x04003C13 RID: 15379
	private Coroutine activeCloseAnimationRoutine;

	// Token: 0x04003C14 RID: 15380
	private HashSet<global::Action> boundRootActions = new HashSet<global::Action>();

	// Token: 0x04003C15 RID: 15381
	private HashSet<global::Action> boundSubgroupActions = new HashSet<global::Action>();

	// Token: 0x04003C16 RID: 15382
	private UnityAction inputChangeReceiver;

	// Token: 0x04003C17 RID: 15383
	private int refreshScaleHandle = -1;

	// Token: 0x04003C18 RID: 15384
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04003C19 RID: 15385
	[SerializeField]
	public TextStyleSetting CategoryLabelTextStyle_LeftAlign;

	// Token: 0x04003C1A RID: 15386
	[SerializeField]
	private TextStyleSetting TooltipHeader;

	// Token: 0x04003C1B RID: 15387
	private int smallCollectionMax = 5;

	// Token: 0x04003C1C RID: 15388
	private HashSet<ToolMenu.CellColorData> colors = new HashSet<ToolMenu.CellColorData>();

	// Token: 0x02001C47 RID: 7239
	public class ToolInfo
	{
		// Token: 0x0600A678 RID: 42616 RVA: 0x003976C8 File Offset: 0x003958C8
		public ToolInfo(string text, string icon_name, global::Action hotkey, string ToolName, ToolMenu.ToolCollection toolCollection, string tooltip = "", Action<object> onSelectCallback = null, object toolData = null)
		{
			this.text = text;
			this.icon = icon_name;
			this.hotkey = hotkey;
			this.toolName = ToolName;
			this.collection = toolCollection;
			toolCollection.tools.Add(this);
			this.tooltip = tooltip;
			this.onSelectCallback = onSelectCallback;
			this.toolData = toolData;
		}

		// Token: 0x040082B1 RID: 33457
		public string text;

		// Token: 0x040082B2 RID: 33458
		public string icon;

		// Token: 0x040082B3 RID: 33459
		public global::Action hotkey;

		// Token: 0x040082B4 RID: 33460
		public string toolName;

		// Token: 0x040082B5 RID: 33461
		public ToolMenu.ToolCollection collection;

		// Token: 0x040082B6 RID: 33462
		public string tooltip;

		// Token: 0x040082B7 RID: 33463
		public KToggle toggle;

		// Token: 0x040082B8 RID: 33464
		public Action<object> onSelectCallback;

		// Token: 0x040082B9 RID: 33465
		public object toolData;
	}

	// Token: 0x02001C48 RID: 7240
	public class ToolCollection
	{
		// Token: 0x0600A679 RID: 42617 RVA: 0x00397725 File Offset: 0x00395925
		public ToolCollection(string text, string icon_name, string tooltip = "", bool useInfoMenu = false, global::Action hotkey = global::Action.NumActions, bool largeIcon = false)
		{
			this.text = text;
			this.icon = icon_name;
			this.tooltip = tooltip;
			this.useInfoMenu = useInfoMenu;
			this.hotkey = hotkey;
			this.largeIcon = largeIcon;
		}

		// Token: 0x040082BA RID: 33466
		public string text;

		// Token: 0x040082BB RID: 33467
		public string icon;

		// Token: 0x040082BC RID: 33468
		public string tooltip;

		// Token: 0x040082BD RID: 33469
		public bool useInfoMenu;

		// Token: 0x040082BE RID: 33470
		public bool largeIcon;

		// Token: 0x040082BF RID: 33471
		public GameObject toggle;

		// Token: 0x040082C0 RID: 33472
		public List<ToolMenu.ToolInfo> tools = new List<ToolMenu.ToolInfo>();

		// Token: 0x040082C1 RID: 33473
		public GameObject UIMenuDisplay;

		// Token: 0x040082C2 RID: 33474
		public GameObject MaskContainer;

		// Token: 0x040082C3 RID: 33475
		public global::Action hotkey;
	}

	// Token: 0x02001C49 RID: 7241
	public struct CellColorData
	{
		// Token: 0x0600A67A RID: 42618 RVA: 0x00397765 File Offset: 0x00395965
		public CellColorData(int cell, Color color)
		{
			this.cell = cell;
			this.color = color;
		}

		// Token: 0x040082C4 RID: 33476
		public int cell;

		// Token: 0x040082C5 RID: 33477
		public Color color;
	}
}
