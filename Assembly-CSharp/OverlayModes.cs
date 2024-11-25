using System;
using System.Collections.Generic;
using FMOD.Studio;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BA7 RID: 2983
public abstract class OverlayModes
{
	// Token: 0x02001C1C RID: 7196
	public class GasConduits : OverlayModes.ConduitMode
	{
		// Token: 0x0600A560 RID: 42336 RVA: 0x0038FAA7 File Offset: 0x0038DCA7
		public override HashedString ViewMode()
		{
			return OverlayModes.GasConduits.ID;
		}

		// Token: 0x0600A561 RID: 42337 RVA: 0x0038FAAE File Offset: 0x0038DCAE
		public override string GetSoundName()
		{
			return "GasVent";
		}

		// Token: 0x0600A562 RID: 42338 RVA: 0x0038FAB5 File Offset: 0x0038DCB5
		public GasConduits() : base(OverlayScreen.GasVentIDs)
		{
		}

		// Token: 0x040081E0 RID: 33248
		public static readonly HashedString ID = "GasConduit";
	}

	// Token: 0x02001C1D RID: 7197
	public class LiquidConduits : OverlayModes.ConduitMode
	{
		// Token: 0x0600A564 RID: 42340 RVA: 0x0038FAD3 File Offset: 0x0038DCD3
		public override HashedString ViewMode()
		{
			return OverlayModes.LiquidConduits.ID;
		}

		// Token: 0x0600A565 RID: 42341 RVA: 0x0038FADA File Offset: 0x0038DCDA
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x0600A566 RID: 42342 RVA: 0x0038FAE1 File Offset: 0x0038DCE1
		public LiquidConduits() : base(OverlayScreen.LiquidVentIDs)
		{
		}

		// Token: 0x040081E1 RID: 33249
		public static readonly HashedString ID = "LiquidConduit";
	}

	// Token: 0x02001C1E RID: 7198
	public abstract class ConduitMode : OverlayModes.Mode
	{
		// Token: 0x0600A568 RID: 42344 RVA: 0x0038FB00 File Offset: 0x0038DD00
		public ConduitMode(ICollection<Tag> ids)
		{
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = ids;
		}

		// Token: 0x0600A569 RID: 42345 RVA: 0x0038FB88 File Offset: 0x0038DD88
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x0600A56A RID: 42346 RVA: 0x0038FBE4 File Offset: 0x0038DDE4
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600A56B RID: 42347 RVA: 0x0038FC18 File Offset: 0x0038DE18
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600A56C RID: 42348 RVA: 0x0038FC64 File Offset: 0x0038DE64
		public override void Disable()
		{
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = saveLoadRoot.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x0600A56D RID: 42349 RVA: 0x0038FD54 File Offset: 0x0038DF54
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
				Vector3 position = root.transform.GetPosition();
				position.z = defaultDepth;
				root.transform.SetPosition(position);
				KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					this.TriggerResorting(componentsInChildren[i]);
				}
			});
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
				if (saveLoadRoot.GetComponent<Conduit>() != null)
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.conduitTargetLayer, null, null);
				}
				else
				{
					base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.layerTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
					{
						Vector3 position = root.transform.GetPosition();
						float z = position.z;
						KPrefabID component3 = root.GetComponent<KPrefabID>();
						if (component3 != null)
						{
							if (component3.HasTag(GameTags.OverlayInFrontOfConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) - 0.2f;
							}
							else if (component3.HasTag(GameTags.OverlayBehindConduits))
							{
								z = Grid.GetLayerZ((this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Grid.SceneLayer.LiquidConduits : Grid.SceneLayer.GasConduits) + 0.2f;
							}
						}
						position.z = z;
						root.transform.SetPosition(position);
						KBatchedAnimController[] componentsInChildren = root.GetComponentsInChildren<KBatchedAnimController>();
						for (int i = 0; i < componentsInChildren.Length; i++)
						{
							this.TriggerResorting(componentsInChildren[i]);
						}
					}, null);
				}
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
				if (component != null)
				{
					int networkCell = component.GetNetworkCell();
					UtilityNetworkManager<FlowUtilityNetwork, Vent> mgr = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitSystem : Game.Instance.gasConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(networkCell, mgr, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			Game.ConduitVisInfo conduitVisInfo = (this.ViewMode() == OverlayModes.LiquidConduits.ID) ? Game.Instance.liquidConduitVisInfo : Game.Instance.gasConduitVisInfo;
			foreach (SaveLoadRoot saveLoadRoot2 in this.layerTargets)
			{
				if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<IBridgedNetworkItem>() != null)
				{
					BuildingDef def = saveLoadRoot2.GetComponent<Building>().Def;
					Color32 colorByName;
					if (def.ThermalConductivity == 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayTintName);
					}
					else if (def.ThermalConductivity < 1f)
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayInsulatedTintName);
					}
					else
					{
						colorByName = GlobalAssets.Instance.colorSet.GetColorByName(conduitVisInfo.overlayRadiantTintName);
					}
					if (this.connectedNetworks.Count > 0)
					{
						IBridgedNetworkItem component2 = saveLoadRoot2.GetComponent<IBridgedNetworkItem>();
						if (component2 != null && component2.IsConnectedToNetworks(this.connectedNetworks))
						{
							colorByName.r = (byte)((float)colorByName.r * num);
							colorByName.g = (byte)((float)colorByName.g * num);
							colorByName.b = (byte)((float)colorByName.b * num);
						}
					}
					saveLoadRoot2.GetComponent<KBatchedAnimController>().TintColour = colorByName;
				}
			}
		}

		// Token: 0x0600A56E RID: 42350 RVA: 0x00390088 File Offset: 0x0038E288
		private void TriggerResorting(KBatchedAnimController kbac)
		{
			if (kbac.enabled)
			{
				kbac.enabled = false;
				kbac.enabled = true;
			}
		}

		// Token: 0x0600A56F RID: 42351 RVA: 0x003900A0 File Offset: 0x0038E2A0
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						IBridgedNetworkItem component = networkItem.GameObject.GetComponent<IBridgedNetworkItem>();
						if (component != null)
						{
							component.AddNetworks(networks);
						}
					}
				}
			}
		}

		// Token: 0x040081E2 RID: 33250
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x040081E3 RID: 33251
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x040081E4 RID: 33252
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x040081E5 RID: 33253
		private List<int> visited = new List<int>();

		// Token: 0x040081E6 RID: 33254
		private ICollection<Tag> targetIDs;

		// Token: 0x040081E7 RID: 33255
		private int objectTargetLayer;

		// Token: 0x040081E8 RID: 33256
		private int conduitTargetLayer;

		// Token: 0x040081E9 RID: 33257
		private int cameraLayerMask;

		// Token: 0x040081EA RID: 33258
		private int selectionMask;
	}

	// Token: 0x02001C1F RID: 7199
	public class Crop : OverlayModes.BasePlantMode
	{
		// Token: 0x0600A572 RID: 42354 RVA: 0x00390284 File Offset: 0x0038E484
		public override HashedString ViewMode()
		{
			return OverlayModes.Crop.ID;
		}

		// Token: 0x0600A573 RID: 42355 RVA: 0x0039028B File Offset: 0x0038E48B
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x0600A574 RID: 42356 RVA: 0x00390294 File Offset: 0x0038E494
		public Crop(Canvas ui_root, GameObject harvestable_notification_prefab)
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[3];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropHalted, delegate(KMonoBehaviour h)
			{
				WiltCondition component = h.GetComponent<WiltCondition>();
				return component != null && component.IsWilting();
			});
			array[1] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrowing, (KMonoBehaviour h) => !(h as HarvestDesignatable).CanBeHarvested());
			array[2] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour h) => GlobalAssets.Instance.colorSet.cropGrown, (KMonoBehaviour h) => (h as HarvestDesignatable).CanBeHarvested());
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
			this.uiRoot = ui_root;
			this.harvestableNotificationPrefab = harvestable_notification_prefab;
		}

		// Token: 0x0600A575 RID: 42357 RVA: 0x003903B0 File Offset: 0x0038E5B0
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.CROP.FULLY_GROWN, UI.OVERLAYS.CROP.TOOLTIPS.FULLY_GROWN, GlobalAssets.Instance.colorSet.cropGrown, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWING, UI.OVERLAYS.CROP.TOOLTIPS.GROWING, GlobalAssets.Instance.colorSet.cropGrowing, null, null, true),
				new LegendEntry(UI.OVERLAYS.CROP.GROWTH_HALTED, UI.OVERLAYS.CROP.TOOLTIPS.GROWTH_HALTED, GlobalAssets.Instance.colorSet.cropHalted, null, null, true)
			};
		}

		// Token: 0x0600A576 RID: 42358 RVA: 0x00390464 File Offset: 0x0038E664
		public override void Update()
		{
			this.updateCropInfo.Clear();
			this.freeHarvestableNotificationIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (HarvestDesignatable harvestDesignatable in this.layerTargets)
			{
				Vector2I vector2I3 = Grid.PosToXY(harvestDesignatable.transform.GetPosition());
				if (vector2I <= vector2I3 && vector2I3 <= vector2I2)
				{
					this.AddCropUI(harvestDesignatable);
				}
			}
			foreach (OverlayModes.Crop.UpdateCropInfo updateCropInfo in this.updateCropInfo)
			{
				updateCropInfo.harvestableUI.GetComponent<HarvestableOverlayWidget>().Refresh(updateCropInfo.harvestable);
			}
			for (int i = this.freeHarvestableNotificationIdx; i < this.harvestableNotificationList.Count; i++)
			{
				if (this.harvestableNotificationList[i].activeSelf)
				{
					this.harvestableNotificationList[i].SetActive(false);
				}
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x0600A577 RID: 42359 RVA: 0x00390654 File Offset: 0x0038E854
		public override void Disable()
		{
			this.DisableHarvestableUINotifications();
			base.Disable();
		}

		// Token: 0x0600A578 RID: 42360 RVA: 0x00390664 File Offset: 0x0038E864
		private void DisableHarvestableUINotifications()
		{
			this.freeHarvestableNotificationIdx = 0;
			foreach (GameObject gameObject in this.harvestableNotificationList)
			{
				gameObject.SetActive(false);
			}
			this.updateCropInfo.Clear();
		}

		// Token: 0x0600A579 RID: 42361 RVA: 0x003906C8 File Offset: 0x0038E8C8
		public GameObject GetFreeCropUI()
		{
			GameObject gameObject;
			if (this.freeHarvestableNotificationIdx < this.harvestableNotificationList.Count)
			{
				gameObject = this.harvestableNotificationList[this.freeHarvestableNotificationIdx];
				if (!gameObject.gameObject.activeSelf)
				{
					gameObject.gameObject.SetActive(true);
				}
				this.freeHarvestableNotificationIdx++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.harvestableNotificationPrefab.gameObject, this.uiRoot.transform.gameObject, false);
				this.harvestableNotificationList.Add(gameObject);
				this.freeHarvestableNotificationIdx++;
			}
			return gameObject;
		}

		// Token: 0x0600A57A RID: 42362 RVA: 0x00390764 File Offset: 0x0038E964
		private void AddCropUI(HarvestDesignatable harvestable)
		{
			GameObject freeCropUI = this.GetFreeCropUI();
			OverlayModes.Crop.UpdateCropInfo item = new OverlayModes.Crop.UpdateCropInfo(harvestable, freeCropUI);
			Vector3 b = Grid.CellToPos(Grid.PosToCell(harvestable), 0.5f, -1.25f, 0f) + harvestable.iconOffset;
			freeCropUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b);
			this.updateCropInfo.Add(item);
		}

		// Token: 0x040081EB RID: 33259
		public static readonly HashedString ID = "Crop";

		// Token: 0x040081EC RID: 33260
		private Canvas uiRoot;

		// Token: 0x040081ED RID: 33261
		private List<OverlayModes.Crop.UpdateCropInfo> updateCropInfo = new List<OverlayModes.Crop.UpdateCropInfo>();

		// Token: 0x040081EE RID: 33262
		private int freeHarvestableNotificationIdx;

		// Token: 0x040081EF RID: 33263
		private List<GameObject> harvestableNotificationList = new List<GameObject>();

		// Token: 0x040081F0 RID: 33264
		private GameObject harvestableNotificationPrefab;

		// Token: 0x040081F1 RID: 33265
		private OverlayModes.ColorHighlightCondition[] highlightConditions;

		// Token: 0x02002634 RID: 9780
		private struct UpdateCropInfo
		{
			// Token: 0x0600C1AB RID: 49579 RVA: 0x003DECAF File Offset: 0x003DCEAF
			public UpdateCropInfo(HarvestDesignatable harvestable, GameObject harvestableUI)
			{
				this.harvestable = harvestable;
				this.harvestableUI = harvestableUI;
			}

			// Token: 0x0400AA00 RID: 43520
			public HarvestDesignatable harvestable;

			// Token: 0x0400AA01 RID: 43521
			public GameObject harvestableUI;
		}
	}

	// Token: 0x02001C20 RID: 7200
	public class Harvest : OverlayModes.BasePlantMode
	{
		// Token: 0x0600A57C RID: 42364 RVA: 0x003907E0 File Offset: 0x0038E9E0
		public override HashedString ViewMode()
		{
			return OverlayModes.Harvest.ID;
		}

		// Token: 0x0600A57D RID: 42365 RVA: 0x003907E7 File Offset: 0x0038E9E7
		public override string GetSoundName()
		{
			return "Harvest";
		}

		// Token: 0x0600A57E RID: 42366 RVA: 0x003907F0 File Offset: 0x0038E9F0
		public Harvest()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition((KMonoBehaviour harvestable) => new Color(0.65f, 0.65f, 0.65f, 0.65f), (KMonoBehaviour harvestable) => true);
			this.highlightConditions = array;
			base..ctor(OverlayScreen.HarvestableIDs);
		}

		// Token: 0x0600A57F RID: 42367 RVA: 0x0039085C File Offset: 0x0038EA5C
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<HarvestDesignatable>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				HarvestDesignatable instance = (HarvestDesignatable)obj;
				base.AddTargetIfVisible<HarvestDesignatable>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<HarvestDesignatable>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Constant, this.targetLayer);
			base.Update();
		}

		// Token: 0x040081F2 RID: 33266
		public static readonly HashedString ID = "HarvestWhenReady";

		// Token: 0x040081F3 RID: 33267
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001C21 RID: 7201
	public abstract class BasePlantMode : OverlayModes.Mode
	{
		// Token: 0x0600A581 RID: 42369 RVA: 0x00390948 File Offset: 0x0038EB48
		public BasePlantMode(ICollection<Tag> ids)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			this.targetIDs = ids;
		}

		// Token: 0x0600A582 RID: 42370 RVA: 0x003909B7 File Offset: 0x0038EBB7
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<HarvestDesignatable>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
		}

		// Token: 0x0600A583 RID: 42371 RVA: 0x003909F8 File Offset: 0x0038EBF8
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (!this.targetIDs.Contains(saveLoadTag))
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			this.partition.Add(component);
		}

		// Token: 0x0600A584 RID: 42372 RVA: 0x00390A40 File Offset: 0x0038EC40
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			HarvestDesignatable component = item.GetComponent<HarvestDesignatable>();
			if (component == null)
			{
				return;
			}
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x0600A585 RID: 42373 RVA: 0x00390AA0 File Offset: 0x0038ECA0
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			base.DisableHighlightTypeOverlay<HarvestDesignatable>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.partition.Clear();
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x040081F4 RID: 33268
		protected UniformGrid<HarvestDesignatable> partition;

		// Token: 0x040081F5 RID: 33269
		protected HashSet<HarvestDesignatable> layerTargets = new HashSet<HarvestDesignatable>();

		// Token: 0x040081F6 RID: 33270
		protected ICollection<Tag> targetIDs;

		// Token: 0x040081F7 RID: 33271
		protected int targetLayer;

		// Token: 0x040081F8 RID: 33272
		private int cameraLayerMask;

		// Token: 0x040081F9 RID: 33273
		private int selectionMask;
	}

	// Token: 0x02001C22 RID: 7202
	public class Decor : OverlayModes.Mode
	{
		// Token: 0x0600A586 RID: 42374 RVA: 0x00390AF7 File Offset: 0x0038ECF7
		public override HashedString ViewMode()
		{
			return OverlayModes.Decor.ID;
		}

		// Token: 0x0600A587 RID: 42375 RVA: 0x00390AFE File Offset: 0x0038ECFE
		public override string GetSoundName()
		{
			return "Decor";
		}

		// Token: 0x0600A588 RID: 42376 RVA: 0x00390B08 File Offset: 0x0038ED08
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.DECOR.HIGHDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.HIGHDECOR, GlobalAssets.Instance.colorSet.decorPositive, null, null, true),
				new LegendEntry(UI.OVERLAYS.DECOR.LOWDECOR, UI.OVERLAYS.DECOR.TOOLTIPS.LOWDECOR, GlobalAssets.Instance.colorSet.decorNegative, null, null, true)
			};
		}

		// Token: 0x0600A589 RID: 42377 RVA: 0x00390B88 File Offset: 0x0038ED88
		public Decor()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour dp)
			{
				Color black = Color.black;
				Color b = Color.black;
				if (dp != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					float decorForCell = (dp as DecorProvider).GetDecorForCell(cell);
					if (decorForCell > 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
					}
					else if (decorForCell < 0f)
					{
						b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
					}
					else if (dp.GetComponent<MonumentPart>() != null && dp.GetComponent<MonumentPart>().IsMonumentCompleted())
					{
						foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(dp.GetComponent<AttachableBuilding>()))
						{
							decorForCell = gameObject.GetComponent<DecorProvider>().GetDecorForCell(cell);
							if (decorForCell > 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightPositive;
								break;
							}
							if (decorForCell < 0f)
							{
								b = GlobalAssets.Instance.colorSet.decorHighlightNegative;
								break;
							}
						}
					}
				}
				return Color.Lerp(black, b, 0.85f);
			}, (KMonoBehaviour dp) => SelectToolHoverTextCard.highlightedObjects.Contains(dp.gameObject));
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x0600A58A RID: 42378 RVA: 0x00390C40 File Offset: 0x0038EE40
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<DecorProvider>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			foreach (Tag item in new Tag[]
			{
				new Tag("Tile"),
				new Tag("SnowTile"),
				new Tag("WoodTile"),
				new Tag("MeshTile"),
				new Tag("InsulationTile"),
				new Tag("GasPermeableMembrane"),
				new Tag("CarpetTile")
			})
			{
				this.targetIDs.Remove(item);
			}
			foreach (Tag item2 in OverlayScreen.GasVentIDs)
			{
				this.targetIDs.Remove(item2);
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				this.targetIDs.Remove(item3);
			}
			this.partition = OverlayModes.Mode.PopulatePartition<DecorProvider>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x0600A58B RID: 42379 RVA: 0x00390DC8 File Offset: 0x0038EFC8
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<DecorProvider>(this.layerTargets, vector2I, vector2I2, null);
			this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), this.workingTargets);
			for (int i = 0; i < this.workingTargets.Count; i++)
			{
				DecorProvider instance = this.workingTargets[i];
				base.AddTargetIfVisible<DecorProvider>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<DecorProvider>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
			this.workingTargets.Clear();
		}

		// Token: 0x0600A58C RID: 42380 RVA: 0x00390E8C File Offset: 0x0038F08C
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				DecorProvider component = item.GetComponent<DecorProvider>();
				if (component != null)
				{
					this.partition.Add(component);
				}
			}
		}

		// Token: 0x0600A58D RID: 42381 RVA: 0x00390ED0 File Offset: 0x0038F0D0
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			DecorProvider component = item.GetComponent<DecorProvider>();
			if (component != null)
			{
				if (this.layerTargets.Contains(component))
				{
					this.layerTargets.Remove(component);
				}
				this.partition.Remove(component);
			}
		}

		// Token: 0x0600A58E RID: 42382 RVA: 0x00390F2C File Offset: 0x0038F12C
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<DecorProvider>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x040081FA RID: 33274
		public static readonly HashedString ID = "Decor";

		// Token: 0x040081FB RID: 33275
		private UniformGrid<DecorProvider> partition;

		// Token: 0x040081FC RID: 33276
		private HashSet<DecorProvider> layerTargets = new HashSet<DecorProvider>();

		// Token: 0x040081FD RID: 33277
		private List<DecorProvider> workingTargets = new List<DecorProvider>();

		// Token: 0x040081FE RID: 33278
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x040081FF RID: 33279
		private int targetLayer;

		// Token: 0x04008200 RID: 33280
		private int cameraLayerMask;

		// Token: 0x04008201 RID: 33281
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001C23 RID: 7203
	public class Disease : OverlayModes.Mode
	{
		// Token: 0x0600A590 RID: 42384 RVA: 0x00390F8C File Offset: 0x0038F18C
		private static float CalculateHUE(Color32 colour)
		{
			byte b = Math.Max(colour.r, Math.Max(colour.g, colour.b));
			byte b2 = Math.Min(colour.r, Math.Min(colour.g, colour.b));
			float result = 0f;
			int num = (int)(b - b2);
			if (num == 0)
			{
				result = 0f;
			}
			else if (b == colour.r)
			{
				result = (float)(colour.g - colour.b) / (float)num % 6f;
			}
			else if (b == colour.g)
			{
				result = (float)(colour.b - colour.r) / (float)num + 2f;
			}
			else if (b == colour.b)
			{
				result = (float)(colour.r - colour.g) / (float)num + 4f;
			}
			return result;
		}

		// Token: 0x0600A591 RID: 42385 RVA: 0x00391050 File Offset: 0x0038F250
		public override HashedString ViewMode()
		{
			return OverlayModes.Disease.ID;
		}

		// Token: 0x0600A592 RID: 42386 RVA: 0x00391057 File Offset: 0x0038F257
		public override string GetSoundName()
		{
			return "Disease";
		}

		// Token: 0x0600A593 RID: 42387 RVA: 0x00391060 File Offset: 0x0038F260
		public Disease(Canvas diseaseUIParent, GameObject diseaseOverlayPrefab)
		{
			this.diseaseUIParent = diseaseUIParent;
			this.diseaseOverlayPrefab = diseaseOverlayPrefab;
			this.legendFilters = this.CreateDefaultFilters();
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
		}

		// Token: 0x0600A594 RID: 42388 RVA: 0x003910E8 File Offset: 0x0038F2E8
		public override void Enable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disease);
			CameraController.Instance.ToggleColouredOverlayView(true);
			Camera.main.cullingMask |= this.cameraLayerMask;
			base.RegisterSaveLoadListeners();
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(this.ViewMode());
				}
			}
		}

		// Token: 0x0600A595 RID: 42389 RVA: 0x00391180 File Offset: 0x0038F380
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GASCONDUIT,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600A596 RID: 42390 RVA: 0x003911AB File Offset: 0x0038F3AB
		public override void OnFiltersChanged()
		{
			Game.Instance.showGasConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, this.legendFilters);
			Game.Instance.showLiquidConduitDisease = base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, this.legendFilters);
		}

		// Token: 0x0600A597 RID: 42391 RVA: 0x003911E4 File Offset: 0x0038F3E4
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			if (item == null)
			{
				return;
			}
			KBatchedAnimController component = item.GetComponent<KBatchedAnimController>();
			if (component == null)
			{
				return;
			}
			InfraredVisualizerComponents.ClearOverlayColour(component);
		}

		// Token: 0x0600A598 RID: 42392 RVA: 0x00391212 File Offset: 0x0038F412
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
		}

		// Token: 0x0600A599 RID: 42393 RVA: 0x00391214 File Offset: 0x0038F414
		public override void Disable()
		{
			foreach (DiseaseSourceVisualizer diseaseSourceVisualizer in Components.DiseaseSourceVisualizers.Items)
			{
				if (!(diseaseSourceVisualizer == null))
				{
					diseaseSourceVisualizer.Show(OverlayModes.None.ID);
				}
			}
			base.UnregisterSaveLoadListeners();
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			foreach (KMonoBehaviour kmonoBehaviour in this.layerTargets)
			{
				if (!(kmonoBehaviour == null))
				{
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
					Vector3 position = kmonoBehaviour.transform.GetPosition();
					position.z = defaultDepth;
					kmonoBehaviour.transform.SetPosition(position);
					KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
					component.enabled = false;
					component.enabled = true;
				}
			}
			CameraController.Instance.ToggleColouredOverlayView(false);
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			Game.Instance.showGasConduitDisease = false;
			Game.Instance.showLiquidConduitDisease = false;
			this.freeDiseaseUI = 0;
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.gameObject.SetActive(false);
			}
			this.updateDiseaseInfo.Clear();
			this.privateTargets.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x0600A59A RID: 42394 RVA: 0x003913B8 File Offset: 0x0038F5B8
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<OverlayModes.Disease.DiseaseSortInfo> list2 = new List<OverlayModes.Disease.DiseaseSortInfo>();
			foreach (Klei.AI.Disease d in Db.Get().Diseases.resources)
			{
				list2.Add(new OverlayModes.Disease.DiseaseSortInfo(d));
			}
			list2.Sort((OverlayModes.Disease.DiseaseSortInfo a, OverlayModes.Disease.DiseaseSortInfo b) => a.sortkey.CompareTo(b.sortkey));
			foreach (OverlayModes.Disease.DiseaseSortInfo diseaseSortInfo in list2)
			{
				list.Add(new LegendEntry(diseaseSortInfo.disease.Name, diseaseSortInfo.disease.overlayLegendHovertext.ToString(), GlobalAssets.Instance.colorSet.GetColorByName(diseaseSortInfo.disease.overlayColourName), null, null, true));
			}
			return list;
		}

		// Token: 0x0600A59B RID: 42395 RVA: 0x003914D0 File Offset: 0x0038F6D0
		public GameObject GetFreeDiseaseUI()
		{
			GameObject gameObject;
			if (this.freeDiseaseUI < this.diseaseUIList.Count)
			{
				gameObject = this.diseaseUIList[this.freeDiseaseUI];
				gameObject.gameObject.SetActive(true);
				this.freeDiseaseUI++;
			}
			else
			{
				gameObject = global::Util.KInstantiateUI(this.diseaseOverlayPrefab, this.diseaseUIParent.transform.gameObject, false);
				this.diseaseUIList.Add(gameObject);
				this.freeDiseaseUI++;
			}
			return gameObject;
		}

		// Token: 0x0600A59C RID: 42396 RVA: 0x00391558 File Offset: 0x0038F758
		private void AddDiseaseUI(MinionIdentity target)
		{
			GameObject gameObject = this.GetFreeDiseaseUI();
			DiseaseOverlayWidget component = gameObject.GetComponent<DiseaseOverlayWidget>();
			AmountInstance amount_inst = target.GetComponent<Modifiers>().amounts.Get(Db.Get().Amounts.ImmuneLevel);
			OverlayModes.Disease.UpdateDiseaseInfo item = new OverlayModes.Disease.UpdateDiseaseInfo(amount_inst, component);
			KAnimControllerBase component2 = target.GetComponent<KAnimControllerBase>();
			Vector3 position = (component2 != null) ? component2.GetWorldPivot() : (target.transform.GetPosition() + Vector3.down);
			gameObject.GetComponent<RectTransform>().SetPosition(position);
			this.updateDiseaseInfo.Add(item);
		}

		// Token: 0x0600A59D RID: 42397 RVA: 0x003915E4 File Offset: 0x0038F7E4
		public override void Update()
		{
			Vector2I u;
			Vector2I v;
			Grid.GetVisibleExtents(out u, out v);
			using (new KProfiler.Region("UpdateDiseaseCarriers", null))
			{
				this.queuedAdds.Clear();
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!(minionIdentity == null))
					{
						Vector2I vector2I = Grid.PosToXY(minionIdentity.transform.GetPosition());
						if (u <= vector2I && vector2I <= v && !this.privateTargets.Contains(minionIdentity))
						{
							this.AddDiseaseUI(minionIdentity);
							this.queuedAdds.Add(minionIdentity);
						}
					}
				}
				foreach (KMonoBehaviour item in this.queuedAdds)
				{
					this.privateTargets.Add(item);
				}
				this.queuedAdds.Clear();
			}
			foreach (OverlayModes.Disease.UpdateDiseaseInfo updateDiseaseInfo in this.updateDiseaseInfo)
			{
				updateDiseaseInfo.ui.Refresh(updateDiseaseInfo.valueSrc);
			}
			bool flag = false;
			if (Game.Instance.showLiquidConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.LiquidVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item2 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item2))
						{
							OverlayScreen.DiseaseIDs.Add(item2);
							flag = true;
						}
					}
					goto IL_1F1;
				}
			}
			foreach (Tag item3 in OverlayScreen.LiquidVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item3))
				{
					OverlayScreen.DiseaseIDs.Remove(item3);
					flag = true;
				}
			}
			IL_1F1:
			if (Game.Instance.showGasConduitDisease)
			{
				using (HashSet<Tag>.Enumerator enumerator4 = OverlayScreen.GasVentIDs.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						Tag item4 = enumerator4.Current;
						if (!OverlayScreen.DiseaseIDs.Contains(item4))
						{
							OverlayScreen.DiseaseIDs.Add(item4);
							flag = true;
						}
					}
					goto IL_297;
				}
			}
			foreach (Tag item5 in OverlayScreen.GasVentIDs)
			{
				if (OverlayScreen.DiseaseIDs.Contains(item5))
				{
					OverlayScreen.DiseaseIDs.Remove(item5);
					flag = true;
				}
			}
			IL_297:
			if (flag)
			{
				this.SetLayerZ(-50f);
			}
		}

		// Token: 0x0600A59E RID: 42398 RVA: 0x003918FC File Offset: 0x0038FAFC
		private void SetLayerZ(float offset_z)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.ClearOutsideViewObjects<KMonoBehaviour>(this.layerTargets, vector2I, vector2I2, OverlayScreen.DiseaseIDs, delegate(KMonoBehaviour go)
			{
				if (go != null)
				{
					float defaultDepth2 = OverlayModes.Mode.GetDefaultDepth(go);
					Vector3 position2 = go.transform.GetPosition();
					position2.z = defaultDepth2;
					go.transform.SetPosition(position2);
					KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
					component2.enabled = false;
					component2.enabled = true;
				}
			});
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			foreach (Tag key in OverlayScreen.DiseaseIDs)
			{
				List<SaveLoadRoot> list;
				if (lists.TryGetValue(key, out list))
				{
					foreach (KMonoBehaviour kmonoBehaviour in list)
					{
						if (!(kmonoBehaviour == null) && !this.layerTargets.Contains(kmonoBehaviour))
						{
							Vector3 position = kmonoBehaviour.transform.GetPosition();
							if (Grid.IsVisible(Grid.PosToCell(position)) && vector2I <= position && position <= vector2I2)
							{
								float defaultDepth = OverlayModes.Mode.GetDefaultDepth(kmonoBehaviour);
								position.z = defaultDepth + offset_z;
								kmonoBehaviour.transform.SetPosition(position);
								KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
								component.enabled = false;
								component.enabled = true;
								this.layerTargets.Add(kmonoBehaviour);
							}
						}
					}
				}
			}
		}

		// Token: 0x04008202 RID: 33282
		public static readonly HashedString ID = "Disease";

		// Token: 0x04008203 RID: 33283
		private int cameraLayerMask;

		// Token: 0x04008204 RID: 33284
		private int freeDiseaseUI;

		// Token: 0x04008205 RID: 33285
		private List<GameObject> diseaseUIList = new List<GameObject>();

		// Token: 0x04008206 RID: 33286
		private List<OverlayModes.Disease.UpdateDiseaseInfo> updateDiseaseInfo = new List<OverlayModes.Disease.UpdateDiseaseInfo>();

		// Token: 0x04008207 RID: 33287
		private HashSet<KMonoBehaviour> layerTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04008208 RID: 33288
		private HashSet<KMonoBehaviour> privateTargets = new HashSet<KMonoBehaviour>();

		// Token: 0x04008209 RID: 33289
		private List<KMonoBehaviour> queuedAdds = new List<KMonoBehaviour>();

		// Token: 0x0400820A RID: 33290
		private Canvas diseaseUIParent;

		// Token: 0x0400820B RID: 33291
		private GameObject diseaseOverlayPrefab;

		// Token: 0x02002638 RID: 9784
		private struct DiseaseSortInfo
		{
			// Token: 0x0600C1BC RID: 49596 RVA: 0x003DEEFA File Offset: 0x003DD0FA
			public DiseaseSortInfo(Klei.AI.Disease d)
			{
				this.disease = d;
				this.sortkey = OverlayModes.Disease.CalculateHUE(GlobalAssets.Instance.colorSet.GetColorByName(d.overlayColourName));
			}

			// Token: 0x0400AA0F RID: 43535
			public float sortkey;

			// Token: 0x0400AA10 RID: 43536
			public Klei.AI.Disease disease;
		}

		// Token: 0x02002639 RID: 9785
		private struct UpdateDiseaseInfo
		{
			// Token: 0x0600C1BD RID: 49597 RVA: 0x003DEF23 File Offset: 0x003DD123
			public UpdateDiseaseInfo(AmountInstance amount_inst, DiseaseOverlayWidget ui)
			{
				this.ui = ui;
				this.valueSrc = amount_inst;
			}

			// Token: 0x0400AA11 RID: 43537
			public DiseaseOverlayWidget ui;

			// Token: 0x0400AA12 RID: 43538
			public AmountInstance valueSrc;
		}
	}

	// Token: 0x02001C24 RID: 7204
	public class Logic : OverlayModes.Mode
	{
		// Token: 0x0600A5A0 RID: 42400 RVA: 0x00391A95 File Offset: 0x0038FC95
		public override HashedString ViewMode()
		{
			return OverlayModes.Logic.ID;
		}

		// Token: 0x0600A5A1 RID: 42401 RVA: 0x00391A9C File Offset: 0x0038FC9C
		public override string GetSoundName()
		{
			return "Logic";
		}

		// Token: 0x0600A5A2 RID: 42402 RVA: 0x00391AA4 File Offset: 0x0038FCA4
		public override List<LegendEntry> GetCustomLegendData()
		{
			return new List<LegendEntry>
			{
				new LegendEntry(UI.OVERLAYS.LOGIC.INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.INPUT, Color.white, null, Assets.GetSprite("logicInput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.OUTPUT, Color.white, null, Assets.GetSprite("logicOutput"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_INPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_in"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RIBBON_OUTPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.RIBBON_OUTPUT, Color.white, null, Assets.GetSprite("logic_ribbon_all_out"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.RESET_UPDATE, UI.OVERLAYS.LOGIC.TOOLTIPS.RESET_UPDATE, Color.white, null, Assets.GetSprite("logicResetUpdate"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CONTROL_INPUT, UI.OVERLAYS.LOGIC.TOOLTIPS.CONTROL_INPUT, Color.white, null, Assets.GetSprite("control_input_frame_legend"), true),
				new LegendEntry(UI.OVERLAYS.LOGIC.CIRCUIT_STATUS_HEADER, null, Color.white, null, null, false),
				new LegendEntry(UI.OVERLAYS.LOGIC.ONE, null, GlobalAssets.Instance.colorSet.logicOnText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.ZERO, null, GlobalAssets.Instance.colorSet.logicOffText, null, null, true),
				new LegendEntry(UI.OVERLAYS.LOGIC.DISCONNECTED, UI.OVERLAYS.LOGIC.TOOLTIPS.DISCONNECTED, GlobalAssets.Instance.colorSet.logicDisconnected, null, null, true)
			};
		}

		// Token: 0x0600A5A3 RID: 42403 RVA: 0x00391CA4 File Offset: 0x0038FEA4
		public Logic(LogicModeUI ui_asset)
		{
			this.conduitTargetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.objectTargetLayer = LayerMask.NameToLayer("MaskedOverlayBG");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.uiAsset = ui_asset;
		}

		// Token: 0x0600A5A4 RID: 42404 RVA: 0x00391D88 File Offset: 0x0038FF88
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.gameObjPartition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayModes.Logic.HighlightItemIDs);
			this.ioPartition = this.CreateLogicUIPartition();
			GridCompositor.Instance.ToggleMinor(true);
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Combine(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterLogicOn);
		}

		// Token: 0x0600A5A5 RID: 42405 RVA: 0x00391E54 File Offset: 0x00390054
		public override void Disable()
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			logicCircuitManager.onElemAdded = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager.onElemAdded, new Action<ILogicUIElement>(this.OnUIElemAdded));
			LogicCircuitManager logicCircuitManager2 = Game.Instance.logicCircuitManager;
			logicCircuitManager2.onElemRemoved = (Action<ILogicUIElement>)Delegate.Remove(logicCircuitManager2.onElemRemoved, new Action<ILogicUIElement>(this.OnUIElemRemoved));
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterLogicOn, STOP_MODE.ALLOWFADEOUT);
			foreach (SaveLoadRoot saveLoadRoot in this.gameObjTargets)
			{
				float defaultDepth = OverlayModes.Mode.GetDefaultDepth(saveLoadRoot);
				Vector3 position = saveLoadRoot.transform.GetPosition();
				position.z = defaultDepth;
				saveLoadRoot.transform.SetPosition(position);
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = false;
				saveLoadRoot.GetComponent<KBatchedAnimController>().enabled = true;
			}
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.gameObjTargets);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.wireControllers);
			OverlayModes.Mode.ResetDisplayValues<KBatchedAnimController>(this.ribbonControllers);
			this.ResetRibbonSymbolTints<KBatchedAnimController>(this.ribbonControllers);
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
			{
				if (bridgeInfo.controller != null)
				{
					OverlayModes.Mode.ResetDisplayValues(bridgeInfo.controller);
				}
			}
			foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
			{
				if (bridgeInfo2.controller != null)
				{
					this.ResetRibbonTint(bridgeInfo2.controller);
				}
			}
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				uiinfo.Release();
			}
			this.uiInfo.Clear();
			this.uiNodes.Clear();
			this.ioPartition.Clear();
			this.ioTargets.Clear();
			this.gameObjPartition.Clear();
			this.gameObjTargets.Clear();
			this.wireControllers.Clear();
			this.ribbonControllers.Clear();
			this.bridgeControllers.Clear();
			this.ribbonBridgeControllers.Clear();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x0600A5A6 RID: 42406 RVA: 0x00392110 File Offset: 0x00390310
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayModes.Logic.HighlightItemIDs.Contains(saveLoadTag))
			{
				this.gameObjPartition.Add(item);
			}
		}

		// Token: 0x0600A5A7 RID: 42407 RVA: 0x00392144 File Offset: 0x00390344
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.gameObjTargets.Contains(item))
			{
				this.gameObjTargets.Remove(item);
			}
			this.gameObjPartition.Remove(item);
		}

		// Token: 0x0600A5A8 RID: 42408 RVA: 0x00392190 File Offset: 0x00390390
		private void OnUIElemAdded(ILogicUIElement elem)
		{
			this.ioPartition.Add(elem);
		}

		// Token: 0x0600A5A9 RID: 42409 RVA: 0x0039219E File Offset: 0x0039039E
		private void OnUIElemRemoved(ILogicUIElement elem)
		{
			this.ioPartition.Remove(elem);
			if (this.ioTargets.Contains(elem))
			{
				this.ioTargets.Remove(elem);
				this.FreeUI(elem);
			}
		}

		// Token: 0x0600A5AA RID: 42410 RVA: 0x003921D0 File Offset: 0x003903D0
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			Tag wire_id = TagManager.Create("LogicWire");
			Tag ribbon_id = TagManager.Create("LogicRibbon");
			Tag bridge_id = TagManager.Create("LogicWireBridge");
			Tag ribbon_bridge_id = TagManager.Create("LogicRibbonBridge");
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.gameObjTargets, vector2I, vector2I2, delegate(SaveLoadRoot root)
			{
				if (root == null)
				{
					return;
				}
				KPrefabID component7 = root.GetComponent<KPrefabID>();
				if (component7 != null)
				{
					Tag prefabTag = component7.PrefabTag;
					if (prefabTag == wire_id)
					{
						this.wireControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == ribbon_id)
					{
						this.ResetRibbonTint(root.GetComponent<KBatchedAnimController>());
						this.ribbonControllers.Remove(root.GetComponent<KBatchedAnimController>());
						return;
					}
					if (prefabTag == bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.bridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					if (prefabTag == ribbon_bridge_id)
					{
						KBatchedAnimController controller = root.GetComponent<KBatchedAnimController>();
						this.ResetRibbonTint(controller);
						this.ribbonBridgeControllers.RemoveWhere((OverlayModes.Logic.BridgeInfo x) => x.controller == controller);
						return;
					}
					float defaultDepth = OverlayModes.Mode.GetDefaultDepth(root);
					Vector3 position = root.transform.GetPosition();
					position.z = defaultDepth;
					root.transform.SetPosition(position);
					root.GetComponent<KBatchedAnimController>().enabled = false;
					root.GetComponent<KBatchedAnimController>().enabled = true;
				}
			});
			OverlayModes.Mode.RemoveOffscreenTargets<ILogicUIElement>(this.ioTargets, this.workingIOTargets, vector2I, vector2I2, new Action<ILogicUIElement>(this.FreeUI), null);
			using (new KProfiler.Region("UpdateLogicOverlay", null))
			{
				Action<SaveLoadRoot> <>9__3;
				foreach (object obj in this.gameObjPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					SaveLoadRoot saveLoadRoot = (SaveLoadRoot)obj;
					if (saveLoadRoot != null)
					{
						KPrefabID component = saveLoadRoot.GetComponent<KPrefabID>();
						if (component.PrefabTag == wire_id || component.PrefabTag == bridge_id || component.PrefabTag == ribbon_id || component.PrefabTag == ribbon_bridge_id)
						{
							SaveLoadRoot instance = saveLoadRoot;
							Vector2I vis_min = vector2I;
							Vector2I vis_max = vector2I2;
							ICollection<SaveLoadRoot> targets = this.gameObjTargets;
							int layer = this.conduitTargetLayer;
							Action<SaveLoadRoot> on_added;
							if ((on_added = <>9__3) == null)
							{
								on_added = (<>9__3 = delegate(SaveLoadRoot root)
								{
									if (root == null)
									{
										return;
									}
									KPrefabID component7 = root.GetComponent<KPrefabID>();
									if (OverlayModes.Logic.HighlightItemIDs.Contains(component7.PrefabTag))
									{
										if (component7.PrefabTag == wire_id)
										{
											this.wireControllers.Add(root.GetComponent<KBatchedAnimController>());
											return;
										}
										if (component7.PrefabTag == ribbon_id)
										{
											this.ribbonControllers.Add(root.GetComponent<KBatchedAnimController>());
											return;
										}
										if (component7.PrefabTag == bridge_id)
										{
											KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
											int networkCell2 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
											this.bridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
											{
												cell = networkCell2,
												controller = component8
											});
											return;
										}
										if (component7.PrefabTag == ribbon_bridge_id)
										{
											KBatchedAnimController component9 = root.GetComponent<KBatchedAnimController>();
											int networkCell3 = root.GetComponent<LogicUtilityNetworkLink>().GetNetworkCell();
											this.ribbonBridgeControllers.Add(new OverlayModes.Logic.BridgeInfo
											{
												cell = networkCell3,
												controller = component9
											});
										}
									}
								});
							}
							base.AddTargetIfVisible<SaveLoadRoot>(instance, vis_min, vis_max, targets, layer, on_added, null);
						}
						else
						{
							base.AddTargetIfVisible<SaveLoadRoot>(saveLoadRoot, vector2I, vector2I2, this.gameObjTargets, this.objectTargetLayer, delegate(SaveLoadRoot root)
							{
								Vector3 position = root.transform.GetPosition();
								float z = position.z;
								KPrefabID component7 = root.GetComponent<KPrefabID>();
								if (component7 != null)
								{
									if (component7.HasTag(GameTags.OverlayInFrontOfConduits))
									{
										z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) - 0.2f;
									}
									else if (component7.HasTag(GameTags.OverlayBehindConduits))
									{
										z = Grid.GetLayerZ(Grid.SceneLayer.LogicWires) + 0.2f;
									}
								}
								position.z = z;
								root.transform.SetPosition(position);
								KBatchedAnimController component8 = root.GetComponent<KBatchedAnimController>();
								component8.enabled = false;
								component8.enabled = true;
							}, null);
						}
					}
				}
				foreach (object obj2 in this.ioPartition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					ILogicUIElement logicUIElement = (ILogicUIElement)obj2;
					if (logicUIElement != null)
					{
						base.AddTargetIfVisible<ILogicUIElement>(logicUIElement, vector2I, vector2I2, this.ioTargets, this.objectTargetLayer, new Action<ILogicUIElement>(this.AddUI), (KMonoBehaviour kcmp) => kcmp != null && OverlayModes.Logic.HighlightItemIDs.Contains(kcmp.GetComponent<KPrefabID>().PrefabTag));
					}
				}
				this.connectedNetworks.Clear();
				float num = 1f;
				GameObject gameObject = null;
				if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
				{
					gameObject = SelectTool.Instance.hover.gameObject;
				}
				if (gameObject != null)
				{
					IBridgedNetworkItem component2 = gameObject.GetComponent<IBridgedNetworkItem>();
					if (component2 != null)
					{
						int networkCell = component2.GetNetworkCell();
						this.visited.Clear();
						this.FindConnectedNetworks(networkCell, Game.Instance.logicCircuitSystem, this.connectedNetworks, this.visited);
						this.visited.Clear();
						num = OverlayModes.ModeUtil.GetHighlightScale();
					}
				}
				LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
				Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
				Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
				logicOff.a = (logicOn.a = 0);
				foreach (KBatchedAnimController kbatchedAnimController in this.wireControllers)
				{
					if (!(kbatchedAnimController == null))
					{
						Color32 color = logicOff;
						LogicCircuitNetwork networkForCell = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController.transform.GetPosition()));
						if (networkForCell != null)
						{
							color = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component3 = kbatchedAnimController.GetComponent<IBridgedNetworkItem>();
							if (component3 != null && component3.IsConnectedToNetworks(this.connectedNetworks))
							{
								color.r = (byte)((float)color.r * num);
								color.g = (byte)((float)color.g * num);
								color.b = (byte)((float)color.b * num);
							}
						}
						kbatchedAnimController.TintColour = color;
					}
				}
				foreach (KBatchedAnimController kbatchedAnimController2 in this.ribbonControllers)
				{
					if (!(kbatchedAnimController2 == null))
					{
						Color32 color2 = logicOff;
						Color32 color3 = logicOff;
						Color32 color4 = logicOff;
						Color32 color5 = logicOff;
						LogicCircuitNetwork networkForCell2 = logicCircuitManager.GetNetworkForCell(Grid.PosToCell(kbatchedAnimController2.transform.GetPosition()));
						if (networkForCell2 != null)
						{
							color2 = (networkForCell2.IsBitActive(0) ? logicOn : logicOff);
							color3 = (networkForCell2.IsBitActive(1) ? logicOn : logicOff);
							color4 = (networkForCell2.IsBitActive(2) ? logicOn : logicOff);
							color5 = (networkForCell2.IsBitActive(3) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component4 = kbatchedAnimController2.GetComponent<IBridgedNetworkItem>();
							if (component4 != null && component4.IsConnectedToNetworks(this.connectedNetworks))
							{
								color2.r = (byte)((float)color2.r * num);
								color2.g = (byte)((float)color2.g * num);
								color2.b = (byte)((float)color2.b * num);
								color3.r = (byte)((float)color3.r * num);
								color3.g = (byte)((float)color3.g * num);
								color3.b = (byte)((float)color3.b * num);
								color4.r = (byte)((float)color4.r * num);
								color4.g = (byte)((float)color4.g * num);
								color4.b = (byte)((float)color4.b * num);
								color5.r = (byte)((float)color5.r * num);
								color5.g = (byte)((float)color5.g * num);
								color5.b = (byte)((float)color5.b * num);
							}
						}
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color2);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color3);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color4);
						kbatchedAnimController2.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color5);
					}
				}
				foreach (OverlayModes.Logic.BridgeInfo bridgeInfo in this.bridgeControllers)
				{
					if (!(bridgeInfo.controller == null))
					{
						Color32 color6 = logicOff;
						LogicCircuitNetwork networkForCell3 = logicCircuitManager.GetNetworkForCell(bridgeInfo.cell);
						if (networkForCell3 != null)
						{
							color6 = (networkForCell3.IsBitActive(0) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component5 = bridgeInfo.controller.GetComponent<IBridgedNetworkItem>();
							if (component5 != null && component5.IsConnectedToNetworks(this.connectedNetworks))
							{
								color6.r = (byte)((float)color6.r * num);
								color6.g = (byte)((float)color6.g * num);
								color6.b = (byte)((float)color6.b * num);
							}
						}
						bridgeInfo.controller.TintColour = color6;
					}
				}
				foreach (OverlayModes.Logic.BridgeInfo bridgeInfo2 in this.ribbonBridgeControllers)
				{
					if (!(bridgeInfo2.controller == null))
					{
						Color32 color7 = logicOff;
						Color32 color8 = logicOff;
						Color32 color9 = logicOff;
						Color32 color10 = logicOff;
						LogicCircuitNetwork networkForCell4 = logicCircuitManager.GetNetworkForCell(bridgeInfo2.cell);
						if (networkForCell4 != null)
						{
							color7 = (networkForCell4.IsBitActive(0) ? logicOn : logicOff);
							color8 = (networkForCell4.IsBitActive(1) ? logicOn : logicOff);
							color9 = (networkForCell4.IsBitActive(2) ? logicOn : logicOff);
							color10 = (networkForCell4.IsBitActive(3) ? logicOn : logicOff);
						}
						if (this.connectedNetworks.Count > 0)
						{
							IBridgedNetworkItem component6 = bridgeInfo2.controller.GetComponent<IBridgedNetworkItem>();
							if (component6 != null && component6.IsConnectedToNetworks(this.connectedNetworks))
							{
								color7.r = (byte)((float)color7.r * num);
								color7.g = (byte)((float)color7.g * num);
								color7.b = (byte)((float)color7.b * num);
								color8.r = (byte)((float)color8.r * num);
								color8.g = (byte)((float)color8.g * num);
								color8.b = (byte)((float)color8.b * num);
								color9.r = (byte)((float)color9.r * num);
								color9.g = (byte)((float)color9.g * num);
								color9.b = (byte)((float)color9.b * num);
								color10.r = (byte)((float)color10.r * num);
								color10.g = (byte)((float)color10.g * num);
								color10.b = (byte)((float)color10.b * num);
							}
						}
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, color7);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, color8);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, color9);
						bridgeInfo2.controller.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, color10);
					}
				}
			}
			this.UpdateUI();
		}

		// Token: 0x0600A5AB RID: 42411 RVA: 0x00392C28 File Offset: 0x00390E28
		private void UpdateUI()
		{
			Color32 logicOn = GlobalAssets.Instance.colorSet.logicOn;
			Color32 logicOff = GlobalAssets.Instance.colorSet.logicOff;
			Color32 logicDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
			logicOff.a = (logicOn.a = byte.MaxValue);
			foreach (OverlayModes.Logic.UIInfo uiinfo in this.uiInfo.GetDataList())
			{
				LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(uiinfo.cell);
				Color32 c = logicDisconnected;
				LogicControlInputUI component = uiinfo.instance.GetComponent<LogicControlInputUI>();
				if (component != null)
				{
					component.SetContent(networkForCell);
				}
				else if (uiinfo.bitDepth == 4)
				{
					LogicRibbonDisplayUI component2 = uiinfo.instance.GetComponent<LogicRibbonDisplayUI>();
					if (component2 != null)
					{
						component2.SetContent(networkForCell);
					}
				}
				else if (uiinfo.bitDepth == 1)
				{
					if (networkForCell != null)
					{
						c = (networkForCell.IsBitActive(0) ? logicOn : logicOff);
					}
					if (uiinfo.image.color != c)
					{
						uiinfo.image.color = c;
					}
				}
			}
		}

		// Token: 0x0600A5AC RID: 42412 RVA: 0x00392D80 File Offset: 0x00390F80
		private void AddUI(ILogicUIElement ui_elem)
		{
			if (this.uiNodes.ContainsKey(ui_elem))
			{
				return;
			}
			HandleVector<int>.Handle uiHandle = this.uiInfo.Allocate(new OverlayModes.Logic.UIInfo(ui_elem, this.uiAsset));
			this.uiNodes.Add(ui_elem, new OverlayModes.Logic.EventInfo
			{
				uiHandle = uiHandle
			});
		}

		// Token: 0x0600A5AD RID: 42413 RVA: 0x00392DD4 File Offset: 0x00390FD4
		private void FreeUI(ILogicUIElement item)
		{
			if (item == null)
			{
				return;
			}
			OverlayModes.Logic.EventInfo eventInfo;
			if (this.uiNodes.TryGetValue(item, out eventInfo))
			{
				this.uiInfo.GetData(eventInfo.uiHandle).Release();
				this.uiInfo.Free(eventInfo.uiHandle);
				this.uiNodes.Remove(item);
			}
		}

		// Token: 0x0600A5AE RID: 42414 RVA: 0x00392E30 File Offset: 0x00391030
		protected UniformGrid<ILogicUIElement> CreateLogicUIPartition()
		{
			UniformGrid<ILogicUIElement> uniformGrid = new UniformGrid<ILogicUIElement>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (ILogicUIElement logicUIElement in Game.Instance.logicCircuitManager.GetVisElements())
			{
				if (logicUIElement != null)
				{
					uniformGrid.Add(logicUIElement);
				}
			}
			return uniformGrid;
		}

		// Token: 0x0600A5AF RID: 42415 RVA: 0x00392E9C File Offset: 0x0039109C
		private bool IsBitActive(int value, int bit)
		{
			return (value & 1 << bit) > 0;
		}

		// Token: 0x0600A5B0 RID: 42416 RVA: 0x00392EAC File Offset: 0x003910AC
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x0600A5B1 RID: 42417 RVA: 0x00392F3C File Offset: 0x0039113C
		private void ResetRibbonSymbolTints<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					this.ResetRibbonTint(component);
				}
			}
		}

		// Token: 0x0600A5B2 RID: 42418 RVA: 0x00392FA0 File Offset: 0x003911A0
		private void ResetRibbonTint(KBatchedAnimController kbac)
		{
			if (kbac != null)
			{
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_1_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_2_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_3_SYMBOL_NAME, Color.white);
				kbac.SetSymbolTint(OverlayModes.Logic.RIBBON_WIRE_4_SYMBOL_NAME, Color.white);
			}
		}

		// Token: 0x0400820C RID: 33292
		public static readonly HashedString ID = "Logic";

		// Token: 0x0400820D RID: 33293
		public static HashSet<Tag> HighlightItemIDs = new HashSet<Tag>();

		// Token: 0x0400820E RID: 33294
		public static KAnimHashedString RIBBON_WIRE_1_SYMBOL_NAME = "wire1";

		// Token: 0x0400820F RID: 33295
		public static KAnimHashedString RIBBON_WIRE_2_SYMBOL_NAME = "wire2";

		// Token: 0x04008210 RID: 33296
		public static KAnimHashedString RIBBON_WIRE_3_SYMBOL_NAME = "wire3";

		// Token: 0x04008211 RID: 33297
		public static KAnimHashedString RIBBON_WIRE_4_SYMBOL_NAME = "wire4";

		// Token: 0x04008212 RID: 33298
		private int conduitTargetLayer;

		// Token: 0x04008213 RID: 33299
		private int objectTargetLayer;

		// Token: 0x04008214 RID: 33300
		private int cameraLayerMask;

		// Token: 0x04008215 RID: 33301
		private int selectionMask;

		// Token: 0x04008216 RID: 33302
		private UniformGrid<ILogicUIElement> ioPartition;

		// Token: 0x04008217 RID: 33303
		private HashSet<ILogicUIElement> ioTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04008218 RID: 33304
		private HashSet<ILogicUIElement> workingIOTargets = new HashSet<ILogicUIElement>();

		// Token: 0x04008219 RID: 33305
		private HashSet<KBatchedAnimController> wireControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x0400821A RID: 33306
		private HashSet<KBatchedAnimController> ribbonControllers = new HashSet<KBatchedAnimController>();

		// Token: 0x0400821B RID: 33307
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x0400821C RID: 33308
		private List<int> visited = new List<int>();

		// Token: 0x0400821D RID: 33309
		private HashSet<OverlayModes.Logic.BridgeInfo> bridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x0400821E RID: 33310
		private HashSet<OverlayModes.Logic.BridgeInfo> ribbonBridgeControllers = new HashSet<OverlayModes.Logic.BridgeInfo>();

		// Token: 0x0400821F RID: 33311
		private UniformGrid<SaveLoadRoot> gameObjPartition;

		// Token: 0x04008220 RID: 33312
		private HashSet<SaveLoadRoot> gameObjTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008221 RID: 33313
		private LogicModeUI uiAsset;

		// Token: 0x04008222 RID: 33314
		private Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo> uiNodes = new Dictionary<ILogicUIElement, OverlayModes.Logic.EventInfo>();

		// Token: 0x04008223 RID: 33315
		private KCompactedVector<OverlayModes.Logic.UIInfo> uiInfo = new KCompactedVector<OverlayModes.Logic.UIInfo>(0);

		// Token: 0x0200263B RID: 9787
		private struct BridgeInfo
		{
			// Token: 0x0400AA16 RID: 43542
			public int cell;

			// Token: 0x0400AA17 RID: 43543
			public KBatchedAnimController controller;
		}

		// Token: 0x0200263C RID: 9788
		private struct EventInfo
		{
			// Token: 0x0400AA18 RID: 43544
			public HandleVector<int>.Handle uiHandle;
		}

		// Token: 0x0200263D RID: 9789
		private struct UIInfo
		{
			// Token: 0x0600C1C2 RID: 49602 RVA: 0x003DEFB0 File Offset: 0x003DD1B0
			public UIInfo(ILogicUIElement ui_elem, LogicModeUI ui_data)
			{
				this.cell = ui_elem.GetLogicUICell();
				GameObject original = null;
				Sprite sprite = null;
				this.bitDepth = 1;
				switch (ui_elem.GetLogicPortSpriteType())
				{
				case LogicPortSpriteType.Input:
					original = ui_data.prefab;
					sprite = ui_data.inputSprite;
					break;
				case LogicPortSpriteType.Output:
					original = ui_data.prefab;
					sprite = ui_data.outputSprite;
					break;
				case LogicPortSpriteType.ResetUpdate:
					original = ui_data.prefab;
					sprite = ui_data.resetSprite;
					break;
				case LogicPortSpriteType.ControlInput:
					original = ui_data.controlInputPrefab;
					break;
				case LogicPortSpriteType.RibbonInput:
					original = ui_data.ribbonInputPrefab;
					this.bitDepth = 4;
					break;
				case LogicPortSpriteType.RibbonOutput:
					original = ui_data.ribbonOutputPrefab;
					this.bitDepth = 4;
					break;
				}
				this.instance = global::Util.KInstantiate(original, Grid.CellToPosCCC(this.cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
				this.instance.SetActive(true);
				this.image = this.instance.GetComponent<Image>();
				if (this.image != null)
				{
					this.image.raycastTarget = false;
					this.image.sprite = sprite;
				}
			}

			// Token: 0x0600C1C3 RID: 49603 RVA: 0x003DF0C0 File Offset: 0x003DD2C0
			public void Release()
			{
				global::Util.KDestroyGameObject(this.instance);
			}

			// Token: 0x0400AA19 RID: 43545
			public GameObject instance;

			// Token: 0x0400AA1A RID: 43546
			public Image image;

			// Token: 0x0400AA1B RID: 43547
			public int cell;

			// Token: 0x0400AA1C RID: 43548
			public int bitDepth;
		}
	}

	// Token: 0x02001C25 RID: 7205
	public enum BringToFrontLayerSetting
	{
		// Token: 0x04008225 RID: 33317
		None,
		// Token: 0x04008226 RID: 33318
		Constant,
		// Token: 0x04008227 RID: 33319
		Conditional
	}

	// Token: 0x02001C26 RID: 7206
	public class ColorHighlightCondition
	{
		// Token: 0x0600A5B4 RID: 42420 RVA: 0x0039305A File Offset: 0x0039125A
		public ColorHighlightCondition(Func<KMonoBehaviour, Color> highlight_color, Func<KMonoBehaviour, bool> highlight_condition)
		{
			this.highlight_color = highlight_color;
			this.highlight_condition = highlight_condition;
		}

		// Token: 0x04008228 RID: 33320
		public Func<KMonoBehaviour, Color> highlight_color;

		// Token: 0x04008229 RID: 33321
		public Func<KMonoBehaviour, bool> highlight_condition;
	}

	// Token: 0x02001C27 RID: 7207
	public class None : OverlayModes.Mode
	{
		// Token: 0x0600A5B5 RID: 42421 RVA: 0x00393070 File Offset: 0x00391270
		public override HashedString ViewMode()
		{
			return OverlayModes.None.ID;
		}

		// Token: 0x0600A5B6 RID: 42422 RVA: 0x00393077 File Offset: 0x00391277
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x0400822A RID: 33322
		public static readonly HashedString ID = HashedString.Invalid;
	}

	// Token: 0x02001C28 RID: 7208
	public class PathProber : OverlayModes.Mode
	{
		// Token: 0x0600A5B9 RID: 42425 RVA: 0x00393092 File Offset: 0x00391292
		public override HashedString ViewMode()
		{
			return OverlayModes.PathProber.ID;
		}

		// Token: 0x0600A5BA RID: 42426 RVA: 0x00393099 File Offset: 0x00391299
		public override string GetSoundName()
		{
			return "Off";
		}

		// Token: 0x0400822B RID: 33323
		public static readonly HashedString ID = "PathProber";
	}

	// Token: 0x02001C29 RID: 7209
	public class Oxygen : OverlayModes.Mode
	{
		// Token: 0x0600A5BD RID: 42429 RVA: 0x003930B9 File Offset: 0x003912B9
		public override HashedString ViewMode()
		{
			return OverlayModes.Oxygen.ID;
		}

		// Token: 0x0600A5BE RID: 42430 RVA: 0x003930C0 File Offset: 0x003912C0
		public override string GetSoundName()
		{
			return "Oxygen";
		}

		// Token: 0x0600A5BF RID: 42431 RVA: 0x003930C8 File Offset: 0x003912C8
		public override void Enable()
		{
			base.Enable();
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x0600A5C0 RID: 42432 RVA: 0x00393107 File Offset: 0x00391307
		public override void Disable()
		{
			base.Disable();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x0400822C RID: 33324
		public static readonly HashedString ID = "Oxygen";
	}

	// Token: 0x02001C2A RID: 7210
	public class Light : OverlayModes.Mode
	{
		// Token: 0x0600A5C3 RID: 42435 RVA: 0x00393132 File Offset: 0x00391332
		public override HashedString ViewMode()
		{
			return OverlayModes.Light.ID;
		}

		// Token: 0x0600A5C4 RID: 42436 RVA: 0x00393139 File Offset: 0x00391339
		public override string GetSoundName()
		{
			return "Lights";
		}

		// Token: 0x0400822D RID: 33325
		public static readonly HashedString ID = "Light";
	}

	// Token: 0x02001C2B RID: 7211
	public class Priorities : OverlayModes.Mode
	{
		// Token: 0x0600A5C7 RID: 42439 RVA: 0x00393159 File Offset: 0x00391359
		public override HashedString ViewMode()
		{
			return OverlayModes.Priorities.ID;
		}

		// Token: 0x0600A5C8 RID: 42440 RVA: 0x00393160 File Offset: 0x00391360
		public override string GetSoundName()
		{
			return "Priorities";
		}

		// Token: 0x0400822E RID: 33326
		public static readonly HashedString ID = "Priorities";
	}

	// Token: 0x02001C2C RID: 7212
	public class ThermalConductivity : OverlayModes.Mode
	{
		// Token: 0x0600A5CB RID: 42443 RVA: 0x00393180 File Offset: 0x00391380
		public override HashedString ViewMode()
		{
			return OverlayModes.ThermalConductivity.ID;
		}

		// Token: 0x0600A5CC RID: 42444 RVA: 0x00393187 File Offset: 0x00391387
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x0400822F RID: 33327
		public static readonly HashedString ID = "ThermalConductivity";
	}

	// Token: 0x02001C2D RID: 7213
	public class HeatFlow : OverlayModes.Mode
	{
		// Token: 0x0600A5CF RID: 42447 RVA: 0x003931A7 File Offset: 0x003913A7
		public override HashedString ViewMode()
		{
			return OverlayModes.HeatFlow.ID;
		}

		// Token: 0x0600A5D0 RID: 42448 RVA: 0x003931AE File Offset: 0x003913AE
		public override string GetSoundName()
		{
			return "HeatFlow";
		}

		// Token: 0x04008230 RID: 33328
		public static readonly HashedString ID = "HeatFlow";
	}

	// Token: 0x02001C2E RID: 7214
	public class Rooms : OverlayModes.Mode
	{
		// Token: 0x0600A5D3 RID: 42451 RVA: 0x003931CE File Offset: 0x003913CE
		public override HashedString ViewMode()
		{
			return OverlayModes.Rooms.ID;
		}

		// Token: 0x0600A5D4 RID: 42452 RVA: 0x003931D5 File Offset: 0x003913D5
		public override string GetSoundName()
		{
			return "Rooms";
		}

		// Token: 0x0600A5D5 RID: 42453 RVA: 0x003931DC File Offset: 0x003913DC
		public override List<LegendEntry> GetCustomLegendData()
		{
			List<LegendEntry> list = new List<LegendEntry>();
			List<RoomType> list2 = new List<RoomType>(Db.Get().RoomTypes.resources);
			list2.Sort((RoomType a, RoomType b) => a.sortKey.CompareTo(b.sortKey));
			foreach (RoomType roomType in list2)
			{
				string text = roomType.GetCriteriaString();
				if (roomType.effects != null && roomType.effects.Length != 0)
				{
					text = text + "\n\n" + roomType.GetRoomEffectsString();
				}
				list.Add(new LegendEntry(roomType.Name + "\n" + roomType.effect, text, GlobalAssets.Instance.colorSet.GetColorByName(roomType.category.colorName), null, null, true));
			}
			return list;
		}

		// Token: 0x04008231 RID: 33329
		public static readonly HashedString ID = "Rooms";
	}

	// Token: 0x02001C2F RID: 7215
	public abstract class Mode
	{
		// Token: 0x0600A5D8 RID: 42456 RVA: 0x003932E9 File Offset: 0x003914E9
		public static void Clear()
		{
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x0600A5D9 RID: 42457
		public abstract HashedString ViewMode();

		// Token: 0x0600A5DA RID: 42458 RVA: 0x003932F5 File Offset: 0x003914F5
		public virtual void Enable()
		{
		}

		// Token: 0x0600A5DB RID: 42459 RVA: 0x003932F7 File Offset: 0x003914F7
		public virtual void Update()
		{
		}

		// Token: 0x0600A5DC RID: 42460 RVA: 0x003932F9 File Offset: 0x003914F9
		public virtual void Disable()
		{
		}

		// Token: 0x0600A5DD RID: 42461 RVA: 0x003932FB File Offset: 0x003914FB
		public virtual List<LegendEntry> GetCustomLegendData()
		{
			return null;
		}

		// Token: 0x0600A5DE RID: 42462 RVA: 0x003932FE File Offset: 0x003914FE
		public virtual Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return null;
		}

		// Token: 0x0600A5DF RID: 42463 RVA: 0x00393301 File Offset: 0x00391501
		public virtual void OnFiltersChanged()
		{
		}

		// Token: 0x0600A5E0 RID: 42464 RVA: 0x00393303 File Offset: 0x00391503
		public virtual void DisableOverlay()
		{
		}

		// Token: 0x0600A5E1 RID: 42465 RVA: 0x00393305 File Offset: 0x00391505
		public virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
		}

		// Token: 0x0600A5E2 RID: 42466
		public abstract string GetSoundName();

		// Token: 0x0600A5E3 RID: 42467 RVA: 0x00393307 File Offset: 0x00391507
		protected bool InFilter(string layer, Dictionary<string, ToolParameterMenu.ToggleState> filter)
		{
			return (filter.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && filter[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On) || (filter.ContainsKey(layer) && filter[layer] == ToolParameterMenu.ToggleState.On);
		}

		// Token: 0x0600A5E4 RID: 42468 RVA: 0x0039333A File Offset: 0x0039153A
		public void RegisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister += this.OnSaveLoadRootRegistered;
			saveManager.onUnregister += this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x0600A5E5 RID: 42469 RVA: 0x0039336B File Offset: 0x0039156B
		public void UnregisterSaveLoadListeners()
		{
			SaveManager saveManager = SaveLoader.Instance.saveManager;
			saveManager.onRegister -= this.OnSaveLoadRootRegistered;
			saveManager.onUnregister -= this.OnSaveLoadRootUnregistered;
		}

		// Token: 0x0600A5E6 RID: 42470 RVA: 0x0039339C File Offset: 0x0039159C
		protected virtual void OnSaveLoadRootRegistered(SaveLoadRoot root)
		{
		}

		// Token: 0x0600A5E7 RID: 42471 RVA: 0x0039339E File Offset: 0x0039159E
		protected virtual void OnSaveLoadRootUnregistered(SaveLoadRoot root)
		{
		}

		// Token: 0x0600A5E8 RID: 42472 RVA: 0x003933A0 File Offset: 0x003915A0
		protected void ProcessExistingSaveLoadRoots()
		{
			foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in SaveLoader.Instance.saveManager.GetLists())
			{
				foreach (SaveLoadRoot root in keyValuePair.Value)
				{
					this.OnSaveLoadRootRegistered(root);
				}
			}
		}

		// Token: 0x0600A5E9 RID: 42473 RVA: 0x00393438 File Offset: 0x00391638
		protected static UniformGrid<T> PopulatePartition<T>(ICollection<Tag> tags) where T : IUniformGridObject
		{
			Dictionary<Tag, List<SaveLoadRoot>> lists = SaveLoader.Instance.saveManager.GetLists();
			UniformGrid<T> uniformGrid = new UniformGrid<T>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			foreach (Tag key in tags)
			{
				List<SaveLoadRoot> list = null;
				if (lists.TryGetValue(key, out list))
				{
					foreach (SaveLoadRoot saveLoadRoot in list)
					{
						T component = saveLoadRoot.GetComponent<T>();
						if (component != null)
						{
							uniformGrid.Add(component);
						}
					}
				}
			}
			return uniformGrid;
		}

		// Token: 0x0600A5EA RID: 42474 RVA: 0x003934FC File Offset: 0x003916FC
		protected static void ResetDisplayValues<T>(ICollection<T> targets) where T : MonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
				}
			}
		}

		// Token: 0x0600A5EB RID: 42475 RVA: 0x00393568 File Offset: 0x00391768
		protected static void ResetDisplayValues(KBatchedAnimController controller)
		{
			controller.SetLayer(0);
			controller.HighlightColour = Color.clear;
			controller.TintColour = Color.white;
			controller.SetLayer(controller.GetComponent<KPrefabID>().defaultLayer);
		}

		// Token: 0x0600A5EC RID: 42476 RVA: 0x003935A4 File Offset: 0x003917A4
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, Vector2I min, Vector2I max, Action<T> on_removed = null) where T : KMonoBehaviour
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, min, max, null, delegate(T cmp)
			{
				if (cmp != null)
				{
					KBatchedAnimController component = cmp.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						OverlayModes.Mode.ResetDisplayValues(component);
					}
					if (on_removed != null)
					{
						on_removed(cmp);
					}
				}
			});
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x0600A5ED RID: 42477 RVA: 0x003935E0 File Offset: 0x003917E0
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, Vector2I vis_min, Vector2I vis_max, ICollection<Tag> item_ids, Action<T> on_remove) where T : KMonoBehaviour
		{
			OverlayModes.Mode.workingTargets.Clear();
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector2I vector2I = Grid.PosToXY(t.transform.GetPosition());
					if (!(vis_min <= vector2I) || !(vector2I <= vis_max) || t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
					{
						OverlayModes.Mode.workingTargets.Add(t);
					}
					else
					{
						KPrefabID component = t.GetComponent<KPrefabID>();
						if (item_ids != null && !item_ids.Contains(component.PrefabTag) && t.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
						{
							OverlayModes.Mode.workingTargets.Add(t);
						}
					}
				}
			}
			foreach (KMonoBehaviour kmonoBehaviour in OverlayModes.Mode.workingTargets)
			{
				T t2 = (T)((object)kmonoBehaviour);
				if (!(t2 == null))
				{
					if (on_remove != null)
					{
						on_remove(t2);
					}
					targets.Remove(t2);
				}
			}
			OverlayModes.Mode.workingTargets.Clear();
		}

		// Token: 0x0600A5EE RID: 42478 RVA: 0x00393754 File Offset: 0x00391954
		protected static void RemoveOffscreenTargets<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null, Func<T, bool> special_clear_condition = null) where T : IUniformGridObject
		{
			OverlayModes.Mode.ClearOutsideViewObjects<T>(targets, working_targets, vis_min, vis_max, delegate(T cmp)
			{
				if (cmp != null && on_removed != null)
				{
					on_removed(cmp);
				}
			});
			if (special_clear_condition != null)
			{
				working_targets.Clear();
				foreach (T t in targets)
				{
					if (special_clear_condition(t))
					{
						working_targets.Add(t);
					}
				}
				foreach (T t2 in working_targets)
				{
					if (t2 != null)
					{
						if (on_removed != null)
						{
							on_removed(t2);
						}
						targets.Remove(t2);
					}
				}
				working_targets.Clear();
			}
		}

		// Token: 0x0600A5EF RID: 42479 RVA: 0x00393830 File Offset: 0x00391A30
		protected static void ClearOutsideViewObjects<T>(ICollection<T> targets, ICollection<T> working_targets, Vector2I vis_min, Vector2I vis_max, Action<T> on_removed = null) where T : IUniformGridObject
		{
			working_targets.Clear();
			foreach (T t in targets)
			{
				if (t != null)
				{
					Vector2 vector = t.PosMin();
					Vector2 vector2 = t.PosMin();
					if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || (float)vis_max.x < vector.x || (float)vis_max.y < vector.y)
					{
						working_targets.Add(t);
					}
				}
			}
			foreach (T t2 in working_targets)
			{
				if (t2 != null)
				{
					if (on_removed != null)
					{
						on_removed(t2);
					}
					targets.Remove(t2);
				}
			}
			working_targets.Clear();
		}

		// Token: 0x0600A5F0 RID: 42480 RVA: 0x00393934 File Offset: 0x00391B34
		protected static float GetDefaultDepth(KMonoBehaviour cmp)
		{
			BuildingComplete component = cmp.GetComponent<BuildingComplete>();
			float layerZ;
			if (component != null)
			{
				layerZ = Grid.GetLayerZ(component.Def.SceneLayer);
			}
			else
			{
				layerZ = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			}
			return layerZ;
		}

		// Token: 0x0600A5F1 RID: 42481 RVA: 0x00393970 File Offset: 0x00391B70
		protected void UpdateHighlightTypeOverlay<T>(Vector2I min, Vector2I max, ICollection<T> targets, ICollection<Tag> item_ids, OverlayModes.ColorHighlightCondition[] highlights, OverlayModes.BringToFrontLayerSetting bringToFrontSetting, int layer) where T : KMonoBehaviour
		{
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					Vector3 position = t.transform.GetPosition();
					int cell = Grid.PosToCell(position);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell) && min <= position && position <= max)
					{
						KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
						if (!(component == null))
						{
							int layer2 = 0;
							Color32 highlightColour = Color.clear;
							if (highlights != null)
							{
								foreach (OverlayModes.ColorHighlightCondition colorHighlightCondition in highlights)
								{
									if (colorHighlightCondition.highlight_condition(t))
									{
										highlightColour = colorHighlightCondition.highlight_color(t);
										layer2 = layer;
										break;
									}
								}
							}
							if (bringToFrontSetting != OverlayModes.BringToFrontLayerSetting.Constant)
							{
								if (bringToFrontSetting == OverlayModes.BringToFrontLayerSetting.Conditional)
								{
									component.SetLayer(layer2);
								}
							}
							else
							{
								component.SetLayer(layer);
							}
							component.HighlightColour = highlightColour;
						}
					}
				}
			}
		}

		// Token: 0x0600A5F2 RID: 42482 RVA: 0x00393AC8 File Offset: 0x00391CC8
		protected void DisableHighlightTypeOverlay<T>(ICollection<T> targets) where T : KMonoBehaviour
		{
			Color32 highlightColour = Color.clear;
			foreach (T t in targets)
			{
				if (!(t == null))
				{
					KBatchedAnimController component = t.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.HighlightColour = highlightColour;
						component.SetLayer(0);
					}
				}
			}
			targets.Clear();
		}

		// Token: 0x0600A5F3 RID: 42483 RVA: 0x00393B4C File Offset: 0x00391D4C
		protected void AddTargetIfVisible<T>(T instance, Vector2I vis_min, Vector2I vis_max, ICollection<T> targets, int layer, Action<T> on_added = null, Func<KMonoBehaviour, bool> should_add = null) where T : IUniformGridObject
		{
			if (instance.Equals(null))
			{
				return;
			}
			Vector2 vector = instance.PosMin();
			Vector2 vector2 = instance.PosMax();
			if (vector2.x < (float)vis_min.x || vector2.y < (float)vis_min.y || vector.x > (float)vis_max.x || vector.y > (float)vis_max.y)
			{
				return;
			}
			if (targets.Contains(instance))
			{
				return;
			}
			bool flag = false;
			int num = (int)vector.y;
			while ((float)num <= vector2.y)
			{
				int num2 = (int)vector.x;
				while ((float)num2 <= vector2.x)
				{
					int num3 = Grid.XYToCell(num2, num);
					if ((Grid.IsValidCell(num3) && Grid.Visible[num3] > 20 && (int)Grid.WorldIdx[num3] == ClusterManager.Instance.activeWorldId) || !PropertyTextures.IsFogOfWarEnabled)
					{
						flag = true;
						break;
					}
					num2++;
				}
				num++;
			}
			if (flag)
			{
				bool flag2 = true;
				KMonoBehaviour kmonoBehaviour = instance as KMonoBehaviour;
				if (kmonoBehaviour != null && should_add != null)
				{
					flag2 = should_add(kmonoBehaviour);
				}
				if (flag2)
				{
					if (kmonoBehaviour != null)
					{
						KBatchedAnimController component = kmonoBehaviour.GetComponent<KBatchedAnimController>();
						if (component != null)
						{
							component.SetLayer(layer);
						}
					}
					targets.Add(instance);
					if (on_added != null)
					{
						on_added(instance);
					}
				}
			}
		}

		// Token: 0x04008232 RID: 33330
		public Dictionary<string, ToolParameterMenu.ToggleState> legendFilters;

		// Token: 0x04008233 RID: 33331
		private static List<KMonoBehaviour> workingTargets = new List<KMonoBehaviour>();
	}

	// Token: 0x02001C30 RID: 7216
	public class ModeUtil
	{
		// Token: 0x0600A5F6 RID: 42486 RVA: 0x00393CC0 File Offset: 0x00391EC0
		public static float GetHighlightScale()
		{
			return Mathf.SmoothStep(0.5f, 1f, Mathf.Abs(Mathf.Sin(Time.unscaledTime * 4f)));
		}
	}

	// Token: 0x02001C31 RID: 7217
	public class Power : OverlayModes.Mode
	{
		// Token: 0x0600A5F8 RID: 42488 RVA: 0x00393CEE File Offset: 0x00391EEE
		public override HashedString ViewMode()
		{
			return OverlayModes.Power.ID;
		}

		// Token: 0x0600A5F9 RID: 42489 RVA: 0x00393CF5 File Offset: 0x00391EF5
		public override string GetSoundName()
		{
			return "Power";
		}

		// Token: 0x0600A5FA RID: 42490 RVA: 0x00393CFC File Offset: 0x00391EFC
		public Power(Canvas powerLabelParent, LocText powerLabelPrefab, BatteryUI batteryUIPrefab, Vector3 powerLabelOffset, Vector3 batteryUIOffset, Vector3 batteryUITransformerOffset, Vector3 batteryUISmallTransformerOffset)
		{
			this.powerLabelParent = powerLabelParent;
			this.powerLabelPrefab = powerLabelPrefab;
			this.batteryUIPrefab = batteryUIPrefab;
			this.powerLabelOffset = powerLabelOffset;
			this.batteryUIOffset = batteryUIOffset;
			this.batteryUITransformerOffset = batteryUITransformerOffset;
			this.batteryUISmallTransformerOffset = batteryUISmallTransformerOffset;
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x0600A5FB RID: 42491 RVA: 0x00393DE4 File Offset: 0x00391FE4
		public override void Enable()
		{
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(OverlayScreen.WireIDs);
			GridCompositor.Instance.ToggleMinor(true);
		}

		// Token: 0x0600A5FC RID: 42492 RVA: 0x00393E3C File Offset: 0x0039203C
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			this.privateTargets.Clear();
			this.queuedAdds.Clear();
			this.DisablePowerLabels();
			this.DisableBatteryUIs();
			GridCompositor.Instance.ToggleMinor(false);
		}

		// Token: 0x0600A5FD RID: 42493 RVA: 0x00393EC0 File Offset: 0x003920C0
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (OverlayScreen.WireIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600A5FE RID: 42494 RVA: 0x00393EF4 File Offset: 0x003920F4
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600A5FF RID: 42495 RVA: 0x00393F40 File Offset: 0x00392140
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			using (new KProfiler.Region("UpdatePowerOverlay", null))
			{
				foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
				{
					SaveLoadRoot instance = (SaveLoadRoot)obj;
					base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
				}
				this.connectedNetworks.Clear();
				float num = 1f;
				GameObject gameObject = null;
				if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
				{
					gameObject = SelectTool.Instance.hover.gameObject;
				}
				if (gameObject != null)
				{
					IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
					if (component != null)
					{
						int networkCell = component.GetNetworkCell();
						this.visited.Clear();
						this.FindConnectedNetworks(networkCell, Game.Instance.electricalConduitSystem, this.connectedNetworks, this.visited);
						this.visited.Clear();
						num = OverlayModes.ModeUtil.GetHighlightScale();
					}
				}
				CircuitManager circuitManager = Game.Instance.circuitManager;
				foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
				{
					if (!(saveLoadRoot == null))
					{
						IBridgedNetworkItem component2 = saveLoadRoot.GetComponent<IBridgedNetworkItem>();
						if (component2 != null)
						{
							KAnimControllerBase component3 = (component2 as KMonoBehaviour).GetComponent<KBatchedAnimController>();
							int networkCell2 = component2.GetNetworkCell();
							UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(networkCell2);
							ushort num2 = (networkForCell != null) ? ((ushort)networkForCell.id) : ushort.MaxValue;
							float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(num2);
							float num3 = circuitManager.GetMaxSafeWattageForCircuit(num2);
							num3 += POWER.FLOAT_FUDGE_FACTOR;
							float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(num2);
							Color32 color;
							if (wattsUsedByCircuit <= 0f)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitUnpowered;
							}
							else if (wattsUsedByCircuit > num3)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitOverloading;
							}
							else if (wattsNeededWhenActive > num3 && num3 > 0f && wattsUsedByCircuit / num3 >= 0.75f)
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitStraining;
							}
							else
							{
								color = GlobalAssets.Instance.colorSet.powerCircuitSafe;
							}
							if (this.connectedNetworks.Count > 0 && component2.IsConnectedToNetworks(this.connectedNetworks))
							{
								color.r = (byte)((float)color.r * num);
								color.g = (byte)((float)color.g * num);
								color.b = (byte)((float)color.b * num);
							}
							component3.TintColour = color;
						}
					}
				}
			}
			this.queuedAdds.Clear();
			using (new KProfiler.Region("BatteryUI", null))
			{
				foreach (Battery battery in Components.Batteries.Items)
				{
					Vector2I vector2I3 = Grid.PosToXY(battery.transform.GetPosition());
					if (vector2I <= vector2I3 && vector2I3 <= vector2I2 && battery.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component4 = battery.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component4))
						{
							this.AddBatteryUI(battery);
							this.queuedAdds.Add(component4);
						}
					}
				}
				foreach (Generator generator in Components.Generators.Items)
				{
					Vector2I vector2I4 = Grid.PosToXY(generator.transform.GetPosition());
					if (vector2I <= vector2I4 && vector2I4 <= vector2I2 && generator.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component5 = generator.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component5))
						{
							this.privateTargets.Add(component5);
							if (generator.GetComponent<PowerTransformer>() == null)
							{
								this.AddPowerLabels(generator);
							}
						}
					}
				}
				foreach (EnergyConsumer energyConsumer in Components.EnergyConsumers.Items)
				{
					Vector2I vector2I5 = Grid.PosToXY(energyConsumer.transform.GetPosition());
					if (vector2I <= vector2I5 && vector2I5 <= vector2I2 && energyConsumer.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
					{
						SaveLoadRoot component6 = energyConsumer.GetComponent<SaveLoadRoot>();
						if (!this.privateTargets.Contains(component6))
						{
							this.privateTargets.Add(component6);
							this.AddPowerLabels(energyConsumer);
						}
					}
				}
			}
			foreach (SaveLoadRoot item in this.queuedAdds)
			{
				this.privateTargets.Add(item);
			}
			this.queuedAdds.Clear();
			this.UpdatePowerLabels();
		}

		// Token: 0x0600A600 RID: 42496 RVA: 0x00394558 File Offset: 0x00392758
		private LocText GetFreePowerLabel()
		{
			LocText locText;
			if (this.freePowerLabelIdx < this.powerLabels.Count)
			{
				locText = this.powerLabels[this.freePowerLabelIdx];
				this.freePowerLabelIdx++;
			}
			else
			{
				locText = global::Util.KInstantiateUI<LocText>(this.powerLabelPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.powerLabels.Add(locText);
				this.freePowerLabelIdx++;
			}
			return locText;
		}

		// Token: 0x0600A601 RID: 42497 RVA: 0x003945DC File Offset: 0x003927DC
		private void UpdatePowerLabels()
		{
			foreach (OverlayModes.Power.UpdatePowerInfo updatePowerInfo in this.updatePowerInfo)
			{
				KMonoBehaviour item = updatePowerInfo.item;
				LocText powerLabel = updatePowerInfo.powerLabel;
				LocText unitLabel = updatePowerInfo.unitLabel;
				Generator generator = updatePowerInfo.generator;
				IEnergyConsumer consumer = updatePowerInfo.consumer;
				if (updatePowerInfo.item == null || updatePowerInfo.item.gameObject.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
				{
					powerLabel.gameObject.SetActive(false);
				}
				else
				{
					powerLabel.gameObject.SetActive(true);
					if (generator != null && consumer == null)
					{
						int num;
						if (generator.GetComponent<ManualGenerator>() == null)
						{
							generator.GetComponent<Operational>();
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						else
						{
							num = Mathf.Max(0, Mathf.RoundToInt(generator.WattageRating));
						}
						powerLabel.text = ((num != 0) ? ("+" + num.ToString()) : num.ToString());
						BuildingEnabledButton component = item.GetComponent<BuildingEnabledButton>();
						Color color = (component != null && !component.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerGenerator;
						powerLabel.color = color;
						unitLabel.color = color;
						BuildingCellVisualizer component2 = generator.GetComponent<BuildingCellVisualizer>();
						if (component2 != null)
						{
							Image powerOutputIcon = component2.GetPowerOutputIcon();
							if (powerOutputIcon != null)
							{
								powerOutputIcon.color = color;
							}
						}
					}
					if (consumer != null)
					{
						BuildingEnabledButton component3 = item.GetComponent<BuildingEnabledButton>();
						Color color2 = (component3 != null && !component3.IsEnabled) ? GlobalAssets.Instance.colorSet.powerBuildingDisabled : GlobalAssets.Instance.colorSet.powerConsumer;
						int num2 = Mathf.Max(0, Mathf.RoundToInt(consumer.WattsNeededWhenActive));
						string text = num2.ToString();
						powerLabel.text = ((num2 != 0) ? ("-" + text) : text);
						powerLabel.color = color2;
						unitLabel.color = color2;
						Image powerInputIcon = item.GetComponentInChildren<BuildingCellVisualizer>().GetPowerInputIcon();
						if (powerInputIcon != null)
						{
							powerInputIcon.color = color2;
						}
					}
				}
			}
			foreach (OverlayModes.Power.UpdateBatteryInfo updateBatteryInfo in this.updateBatteryInfo)
			{
				updateBatteryInfo.ui.SetContent(updateBatteryInfo.battery);
			}
		}

		// Token: 0x0600A602 RID: 42498 RVA: 0x003948B8 File Offset: 0x00392AB8
		private void AddPowerLabels(KMonoBehaviour item)
		{
			if (item.gameObject.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
			{
				IEnergyConsumer componentInChildren = item.gameObject.GetComponentInChildren<IEnergyConsumer>();
				Generator componentInChildren2 = item.gameObject.GetComponentInChildren<Generator>();
				if (componentInChildren != null || componentInChildren2 != null)
				{
					float num = -10f;
					if (componentInChildren2 != null)
					{
						LocText freePowerLabel = this.GetFreePowerLabel();
						freePowerLabel.gameObject.SetActive(true);
						freePowerLabel.gameObject.name = item.gameObject.name + "power label";
						LocText component = freePowerLabel.transform.GetChild(0).GetComponent<LocText>();
						component.gameObject.SetActive(true);
						freePowerLabel.enabled = true;
						component.enabled = true;
						Vector3 a = Grid.CellToPos(componentInChildren2.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel.rectTransform.SetPosition(a + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						if (componentInChildren != null && componentInChildren.PowerCell == componentInChildren2.PowerCell)
						{
							num -= 15f;
						}
						this.SetToolTip(freePowerLabel, UI.OVERLAYS.POWER.WATTS_GENERATED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel, component, componentInChildren2, null));
					}
					if (componentInChildren != null && componentInChildren.GetType() != typeof(Battery))
					{
						LocText freePowerLabel2 = this.GetFreePowerLabel();
						LocText component2 = freePowerLabel2.transform.GetChild(0).GetComponent<LocText>();
						freePowerLabel2.gameObject.SetActive(true);
						component2.gameObject.SetActive(true);
						freePowerLabel2.gameObject.name = item.gameObject.name + "power label";
						freePowerLabel2.enabled = true;
						component2.enabled = true;
						Vector3 a2 = Grid.CellToPos(componentInChildren.PowerCell, 0.5f, 0f, 0f);
						freePowerLabel2.rectTransform.SetPosition(a2 + this.powerLabelOffset + Vector3.up * (num * 0.02f));
						this.SetToolTip(freePowerLabel2, UI.OVERLAYS.POWER.WATTS_CONSUMED);
						this.updatePowerInfo.Add(new OverlayModes.Power.UpdatePowerInfo(item, freePowerLabel2, component2, null, componentInChildren));
					}
				}
			}
		}

		// Token: 0x0600A603 RID: 42499 RVA: 0x00394B04 File Offset: 0x00392D04
		private void DisablePowerLabels()
		{
			this.freePowerLabelIdx = 0;
			foreach (LocText locText in this.powerLabels)
			{
				locText.gameObject.SetActive(false);
			}
			this.updatePowerInfo.Clear();
		}

		// Token: 0x0600A604 RID: 42500 RVA: 0x00394B6C File Offset: 0x00392D6C
		private void AddBatteryUI(Battery bat)
		{
			BatteryUI freeBatteryUI = this.GetFreeBatteryUI();
			freeBatteryUI.SetContent(bat);
			Vector3 b = Grid.CellToPos(bat.PowerCell, 0.5f, 0f, 0f);
			bool flag = bat.powerTransformer != null;
			float num = 1f;
			Rotatable component = bat.GetComponent<Rotatable>();
			if (component != null && component.GetVisualizerFlipX())
			{
				num = -1f;
			}
			Vector3 b2 = this.batteryUIOffset;
			if (flag)
			{
				b2 = ((bat.GetComponent<Building>().Def.WidthInCells == 2) ? this.batteryUISmallTransformerOffset : this.batteryUITransformerOffset);
			}
			b2.x *= num;
			freeBatteryUI.GetComponent<RectTransform>().SetPosition(Vector3.up + b + b2);
			this.updateBatteryInfo.Add(new OverlayModes.Power.UpdateBatteryInfo(bat, freeBatteryUI));
		}

		// Token: 0x0600A605 RID: 42501 RVA: 0x00394C3C File Offset: 0x00392E3C
		private void SetToolTip(LocText label, string text)
		{
			ToolTip component = label.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = text;
			}
		}

		// Token: 0x0600A606 RID: 42502 RVA: 0x00394C60 File Offset: 0x00392E60
		private void DisableBatteryUIs()
		{
			this.freeBatteryUIIdx = 0;
			foreach (BatteryUI batteryUI in this.batteryUIList)
			{
				batteryUI.gameObject.SetActive(false);
			}
			this.updateBatteryInfo.Clear();
		}

		// Token: 0x0600A607 RID: 42503 RVA: 0x00394CC8 File Offset: 0x00392EC8
		private BatteryUI GetFreeBatteryUI()
		{
			BatteryUI batteryUI;
			if (this.freeBatteryUIIdx < this.batteryUIList.Count)
			{
				batteryUI = this.batteryUIList[this.freeBatteryUIIdx];
				batteryUI.gameObject.SetActive(true);
				this.freeBatteryUIIdx++;
			}
			else
			{
				batteryUI = global::Util.KInstantiateUI<BatteryUI>(this.batteryUIPrefab.gameObject, this.powerLabelParent.transform.gameObject, false);
				this.batteryUIList.Add(batteryUI);
				this.freeBatteryUIIdx++;
			}
			return batteryUI;
		}

		// Token: 0x0600A608 RID: 42504 RVA: 0x00394D58 File Offset: 0x00392F58
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
			}
		}

		// Token: 0x04008234 RID: 33332
		public static readonly HashedString ID = "Power";

		// Token: 0x04008235 RID: 33333
		private int targetLayer;

		// Token: 0x04008236 RID: 33334
		private int cameraLayerMask;

		// Token: 0x04008237 RID: 33335
		private int selectionMask;

		// Token: 0x04008238 RID: 33336
		private List<OverlayModes.Power.UpdatePowerInfo> updatePowerInfo = new List<OverlayModes.Power.UpdatePowerInfo>();

		// Token: 0x04008239 RID: 33337
		private List<OverlayModes.Power.UpdateBatteryInfo> updateBatteryInfo = new List<OverlayModes.Power.UpdateBatteryInfo>();

		// Token: 0x0400823A RID: 33338
		private Canvas powerLabelParent;

		// Token: 0x0400823B RID: 33339
		private LocText powerLabelPrefab;

		// Token: 0x0400823C RID: 33340
		private Vector3 powerLabelOffset;

		// Token: 0x0400823D RID: 33341
		private BatteryUI batteryUIPrefab;

		// Token: 0x0400823E RID: 33342
		private Vector3 batteryUIOffset;

		// Token: 0x0400823F RID: 33343
		private Vector3 batteryUITransformerOffset;

		// Token: 0x04008240 RID: 33344
		private Vector3 batteryUISmallTransformerOffset;

		// Token: 0x04008241 RID: 33345
		private int freePowerLabelIdx;

		// Token: 0x04008242 RID: 33346
		private int freeBatteryUIIdx;

		// Token: 0x04008243 RID: 33347
		private List<LocText> powerLabels = new List<LocText>();

		// Token: 0x04008244 RID: 33348
		private List<BatteryUI> batteryUIList = new List<BatteryUI>();

		// Token: 0x04008245 RID: 33349
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x04008246 RID: 33350
		private List<SaveLoadRoot> queuedAdds = new List<SaveLoadRoot>();

		// Token: 0x04008247 RID: 33351
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008248 RID: 33352
		private HashSet<SaveLoadRoot> privateTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008249 RID: 33353
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x0400824A RID: 33354
		private List<int> visited = new List<int>();

		// Token: 0x02002645 RID: 9797
		private struct UpdatePowerInfo
		{
			// Token: 0x0600C1D6 RID: 49622 RVA: 0x003DF514 File Offset: 0x003DD714
			public UpdatePowerInfo(KMonoBehaviour item, LocText power_label, LocText unit_label, Generator g, IEnergyConsumer c)
			{
				this.item = item;
				this.powerLabel = power_label;
				this.unitLabel = unit_label;
				this.generator = g;
				this.consumer = c;
			}

			// Token: 0x0400AA2C RID: 43564
			public KMonoBehaviour item;

			// Token: 0x0400AA2D RID: 43565
			public LocText powerLabel;

			// Token: 0x0400AA2E RID: 43566
			public LocText unitLabel;

			// Token: 0x0400AA2F RID: 43567
			public Generator generator;

			// Token: 0x0400AA30 RID: 43568
			public IEnergyConsumer consumer;
		}

		// Token: 0x02002646 RID: 9798
		private struct UpdateBatteryInfo
		{
			// Token: 0x0600C1D7 RID: 49623 RVA: 0x003DF53B File Offset: 0x003DD73B
			public UpdateBatteryInfo(Battery battery, BatteryUI ui)
			{
				this.battery = battery;
				this.ui = ui;
			}

			// Token: 0x0400AA31 RID: 43569
			public Battery battery;

			// Token: 0x0400AA32 RID: 43570
			public BatteryUI ui;
		}
	}

	// Token: 0x02001C32 RID: 7218
	public class Radiation : OverlayModes.Mode
	{
		// Token: 0x0600A60A RID: 42506 RVA: 0x00394DF6 File Offset: 0x00392FF6
		public override HashedString ViewMode()
		{
			return OverlayModes.Radiation.ID;
		}

		// Token: 0x0600A60B RID: 42507 RVA: 0x00394DFD File Offset: 0x00392FFD
		public override string GetSoundName()
		{
			return "Radiation";
		}

		// Token: 0x0600A60C RID: 42508 RVA: 0x00394E04 File Offset: 0x00393004
		public override void Enable()
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterRadiationOn);
		}

		// Token: 0x0600A60D RID: 42509 RVA: 0x00394E1B File Offset: 0x0039301B
		public override void Disable()
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterRadiationOn, STOP_MODE.ALLOWFADEOUT);
		}

		// Token: 0x0400824B RID: 33355
		public static readonly HashedString ID = "Radiation";
	}

	// Token: 0x02001C33 RID: 7219
	public class SolidConveyor : OverlayModes.Mode
	{
		// Token: 0x0600A610 RID: 42512 RVA: 0x00394E4C File Offset: 0x0039304C
		public override HashedString ViewMode()
		{
			return OverlayModes.SolidConveyor.ID;
		}

		// Token: 0x0600A611 RID: 42513 RVA: 0x00394E53 File Offset: 0x00393053
		public override string GetSoundName()
		{
			return "LiquidVent";
		}

		// Token: 0x0600A612 RID: 42514 RVA: 0x00394E5C File Offset: 0x0039305C
		public SolidConveyor()
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
		}

		// Token: 0x0600A613 RID: 42515 RVA: 0x00394EF4 File Offset: 0x003930F4
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			this.partition = OverlayModes.Mode.PopulatePartition<SaveLoadRoot>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x0600A614 RID: 42516 RVA: 0x00394F50 File Offset: 0x00393150
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600A615 RID: 42517 RVA: 0x00394F84 File Offset: 0x00393184
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600A616 RID: 42518 RVA: 0x00394FD0 File Offset: 0x003931D0
		public override void Disable()
		{
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x0600A617 RID: 42519 RVA: 0x00395038 File Offset: 0x00393238
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			GameObject gameObject = null;
			if (SelectTool.Instance != null && SelectTool.Instance.hover != null)
			{
				gameObject = SelectTool.Instance.hover.gameObject;
			}
			this.connectedNetworks.Clear();
			float num = 1f;
			if (gameObject != null)
			{
				SolidConduit component = gameObject.GetComponent<SolidConduit>();
				if (component != null)
				{
					int cell = Grid.PosToCell(component);
					UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem = Game.Instance.solidConduitSystem;
					this.visited.Clear();
					this.FindConnectedNetworks(cell, solidConduitSystem, this.connectedNetworks, this.visited);
					this.visited.Clear();
					num = OverlayModes.ModeUtil.GetHighlightScale();
				}
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					Color32 color = this.tint_color;
					SolidConduit component2 = saveLoadRoot.GetComponent<SolidConduit>();
					if (component2 != null)
					{
						if (this.connectedNetworks.Count > 0 && this.IsConnectedToNetworks(component2, this.connectedNetworks))
						{
							color.r = (byte)((float)color.r * num);
							color.g = (byte)((float)color.g * num);
							color.b = (byte)((float)color.b * num);
						}
						saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = color;
					}
				}
			}
		}

		// Token: 0x0600A618 RID: 42520 RVA: 0x0039525C File Offset: 0x0039345C
		public bool IsConnectedToNetworks(SolidConduit conduit, ICollection<UtilityNetwork> networks)
		{
			UtilityNetwork network = conduit.GetNetwork();
			return networks.Contains(network);
		}

		// Token: 0x0600A619 RID: 42521 RVA: 0x00395278 File Offset: 0x00393478
		private void FindConnectedNetworks(int cell, IUtilityNetworkMgr mgr, ICollection<UtilityNetwork> networks, List<int> visited)
		{
			if (visited.Contains(cell))
			{
				return;
			}
			visited.Add(cell);
			UtilityNetwork networkForCell = mgr.GetNetworkForCell(cell);
			if (networkForCell != null)
			{
				networks.Add(networkForCell);
				UtilityConnections connections = mgr.GetConnections(cell, false);
				if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellRight(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellLeft(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellAbove(cell), mgr, networks, visited);
				}
				if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					this.FindConnectedNetworks(Grid.CellBelow(cell), mgr, networks, visited);
				}
				object endpoint = mgr.GetEndpoint(cell);
				if (endpoint != null)
				{
					FlowUtilityNetwork.NetworkItem networkItem = endpoint as FlowUtilityNetwork.NetworkItem;
					if (networkItem != null)
					{
						GameObject gameObject = networkItem.GameObject;
						if (gameObject != null)
						{
							IBridgedNetworkItem component = gameObject.GetComponent<IBridgedNetworkItem>();
							if (component != null)
							{
								component.AddNetworks(networks);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400824C RID: 33356
		public static readonly HashedString ID = "SolidConveyor";

		// Token: 0x0400824D RID: 33357
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x0400824E RID: 33358
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x0400824F RID: 33359
		private ICollection<Tag> targetIDs = OverlayScreen.SolidConveyorIDs;

		// Token: 0x04008250 RID: 33360
		private Color32 tint_color = new Color32(201, 201, 201, 0);

		// Token: 0x04008251 RID: 33361
		private HashSet<UtilityNetwork> connectedNetworks = new HashSet<UtilityNetwork>();

		// Token: 0x04008252 RID: 33362
		private List<int> visited = new List<int>();

		// Token: 0x04008253 RID: 33363
		private int targetLayer;

		// Token: 0x04008254 RID: 33364
		private int cameraLayerMask;

		// Token: 0x04008255 RID: 33365
		private int selectionMask;
	}

	// Token: 0x02001C34 RID: 7220
	public class Sound : OverlayModes.Mode
	{
		// Token: 0x0600A61B RID: 42523 RVA: 0x00395352 File Offset: 0x00393552
		public override HashedString ViewMode()
		{
			return OverlayModes.Sound.ID;
		}

		// Token: 0x0600A61C RID: 42524 RVA: 0x00395359 File Offset: 0x00393559
		public override string GetSoundName()
		{
			return "Sound";
		}

		// Token: 0x0600A61D RID: 42525 RVA: 0x00395360 File Offset: 0x00393560
		public Sound()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour np)
			{
				Color black = Color.black;
				Color black2 = Color.black;
				float t = 0.8f;
				if (np != null)
				{
					int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
					if ((np as NoisePolluter).GetNoiseForCell(cell) < 36f)
					{
						t = 1f;
						black2 = new Color(0.4f, 0.4f, 0.4f);
					}
				}
				return Color.Lerp(black, black2, t);
			}, delegate(KMonoBehaviour np)
			{
				List<GameObject> highlightedObjects = SelectToolHoverTextCard.highlightedObjects;
				bool result = false;
				for (int i = 0; i < highlightedObjects.Count; i++)
				{
					if (highlightedObjects[i] != null && highlightedObjects[i] == np.gameObject)
					{
						result = true;
						break;
					}
				}
				return result;
			});
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
		}

		// Token: 0x0600A61E RID: 42526 RVA: 0x00395420 File Offset: 0x00393620
		public override void Enable()
		{
			base.RegisterSaveLoadListeners();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<NoisePolluter>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			this.partition = OverlayModes.Mode.PopulatePartition<NoisePolluter>(this.targetIDs);
			Camera.main.cullingMask |= this.cameraLayerMask;
		}

		// Token: 0x0600A61F RID: 42527 RVA: 0x00395470 File Offset: 0x00393670
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<NoisePolluter>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				NoisePolluter instance = (NoisePolluter)obj;
				base.AddTargetIfVisible<NoisePolluter>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			base.UpdateHighlightTypeOverlay<NoisePolluter>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x0600A620 RID: 42528 RVA: 0x00395540 File Offset: 0x00393740
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				NoisePolluter component = item.GetComponent<NoisePolluter>();
				this.partition.Add(component);
			}
		}

		// Token: 0x0600A621 RID: 42529 RVA: 0x0039557C File Offset: 0x0039377C
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			NoisePolluter component = item.GetComponent<NoisePolluter>();
			if (this.layerTargets.Contains(component))
			{
				this.layerTargets.Remove(component);
			}
			this.partition.Remove(component);
		}

		// Token: 0x0600A622 RID: 42530 RVA: 0x003955D0 File Offset: 0x003937D0
		public override void Disable()
		{
			base.DisableHighlightTypeOverlay<NoisePolluter>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			base.UnregisterSaveLoadListeners();
			this.partition.Clear();
			this.layerTargets.Clear();
		}

		// Token: 0x04008256 RID: 33366
		public static readonly HashedString ID = "Sound";

		// Token: 0x04008257 RID: 33367
		private UniformGrid<NoisePolluter> partition;

		// Token: 0x04008258 RID: 33368
		private HashSet<NoisePolluter> layerTargets = new HashSet<NoisePolluter>();

		// Token: 0x04008259 RID: 33369
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x0400825A RID: 33370
		private int targetLayer;

		// Token: 0x0400825B RID: 33371
		private int cameraLayerMask;

		// Token: 0x0400825C RID: 33372
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}

	// Token: 0x02001C35 RID: 7221
	public class Suit : OverlayModes.Mode
	{
		// Token: 0x0600A624 RID: 42532 RVA: 0x0039562E File Offset: 0x0039382E
		public override HashedString ViewMode()
		{
			return OverlayModes.Suit.ID;
		}

		// Token: 0x0600A625 RID: 42533 RVA: 0x00395635 File Offset: 0x00393835
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x0600A626 RID: 42534 RVA: 0x0039563C File Offset: 0x0039383C
		public Suit(Canvas ui_parent, GameObject overlay_prefab)
		{
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.selectionMask = this.cameraLayerMask;
			this.targetIDs = OverlayScreen.SuitIDs;
			this.uiParent = ui_parent;
			this.overlayPrefab = overlay_prefab;
		}

		// Token: 0x0600A627 RID: 42535 RVA: 0x003956BC File Offset: 0x003938BC
		public override void Enable()
		{
			this.partition = new UniformGrid<SaveLoadRoot>(Grid.WidthInCells, Grid.HeightInCells, 8, 8);
			base.ProcessExistingSaveLoadRoots();
			base.RegisterSaveLoadListeners();
			Camera.main.cullingMask |= this.cameraLayerMask;
			SelectTool.Instance.SetLayerMask(this.selectionMask);
			GridCompositor.Instance.ToggleMinor(false);
			base.Enable();
		}

		// Token: 0x0600A628 RID: 42536 RVA: 0x00395724 File Offset: 0x00393924
		public override void Disable()
		{
			base.UnregisterSaveLoadListeners();
			OverlayModes.Mode.ResetDisplayValues<SaveLoadRoot>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			SelectTool.Instance.ClearLayerMask();
			this.partition.Clear();
			this.partition = null;
			this.layerTargets.Clear();
			for (int i = 0; i < this.uiList.Count; i++)
			{
				this.uiList[i].SetActive(false);
			}
			GridCompositor.Instance.ToggleMinor(false);
			base.Disable();
		}

		// Token: 0x0600A629 RID: 42537 RVA: 0x003957BC File Offset: 0x003939BC
		protected override void OnSaveLoadRootRegistered(SaveLoadRoot item)
		{
			Tag saveLoadTag = item.GetComponent<KPrefabID>().GetSaveLoadTag();
			if (this.targetIDs.Contains(saveLoadTag))
			{
				this.partition.Add(item);
			}
		}

		// Token: 0x0600A62A RID: 42538 RVA: 0x003957F0 File Offset: 0x003939F0
		protected override void OnSaveLoadRootUnregistered(SaveLoadRoot item)
		{
			if (item == null || item.gameObject == null)
			{
				return;
			}
			if (this.layerTargets.Contains(item))
			{
				this.layerTargets.Remove(item);
			}
			this.partition.Remove(item);
		}

		// Token: 0x0600A62B RID: 42539 RVA: 0x0039583C File Offset: 0x00393A3C
		private GameObject GetFreeUI()
		{
			GameObject gameObject;
			if (this.freeUiIdx >= this.uiList.Count)
			{
				gameObject = global::Util.KInstantiateUI(this.overlayPrefab, this.uiParent.transform.gameObject, false);
				this.uiList.Add(gameObject);
			}
			else
			{
				List<GameObject> list = this.uiList;
				int num = this.freeUiIdx;
				this.freeUiIdx = num + 1;
				gameObject = list[num];
			}
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			return gameObject;
		}

		// Token: 0x0600A62C RID: 42540 RVA: 0x003958B8 File Offset: 0x00393AB8
		public override void Update()
		{
			this.freeUiIdx = 0;
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<SaveLoadRoot>(this.layerTargets, vector2I, vector2I2, null);
			foreach (object obj in this.partition.GetAllIntersecting(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y)))
			{
				SaveLoadRoot instance = (SaveLoadRoot)obj;
				base.AddTargetIfVisible<SaveLoadRoot>(instance, vector2I, vector2I2, this.layerTargets, this.targetLayer, null, null);
			}
			foreach (SaveLoadRoot saveLoadRoot in this.layerTargets)
			{
				if (!(saveLoadRoot == null))
				{
					saveLoadRoot.GetComponent<KBatchedAnimController>().TintColour = Color.white;
					bool flag = false;
					if (saveLoadRoot.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
					{
						flag = true;
					}
					else
					{
						SuitLocker component = saveLoadRoot.GetComponent<SuitLocker>();
						if (component != null)
						{
							flag = (component.GetStoredOutfit() != null);
						}
					}
					if (flag)
					{
						this.GetFreeUI().GetComponent<RectTransform>().SetPosition(saveLoadRoot.transform.GetPosition());
					}
				}
			}
			for (int i = this.freeUiIdx; i < this.uiList.Count; i++)
			{
				if (this.uiList[i].activeSelf)
				{
					this.uiList[i].SetActive(false);
				}
			}
		}

		// Token: 0x0400825D RID: 33373
		public static readonly HashedString ID = "Suit";

		// Token: 0x0400825E RID: 33374
		private UniformGrid<SaveLoadRoot> partition;

		// Token: 0x0400825F RID: 33375
		private HashSet<SaveLoadRoot> layerTargets = new HashSet<SaveLoadRoot>();

		// Token: 0x04008260 RID: 33376
		private ICollection<Tag> targetIDs;

		// Token: 0x04008261 RID: 33377
		private List<GameObject> uiList = new List<GameObject>();

		// Token: 0x04008262 RID: 33378
		private int freeUiIdx;

		// Token: 0x04008263 RID: 33379
		private int targetLayer;

		// Token: 0x04008264 RID: 33380
		private int cameraLayerMask;

		// Token: 0x04008265 RID: 33381
		private int selectionMask;

		// Token: 0x04008266 RID: 33382
		private Canvas uiParent;

		// Token: 0x04008267 RID: 33383
		private GameObject overlayPrefab;
	}

	// Token: 0x02001C36 RID: 7222
	public class Temperature : OverlayModes.Mode
	{
		// Token: 0x0600A62E RID: 42542 RVA: 0x00395A85 File Offset: 0x00393C85
		public override HashedString ViewMode()
		{
			return OverlayModes.Temperature.ID;
		}

		// Token: 0x0600A62F RID: 42543 RVA: 0x00395A8C File Offset: 0x00393C8C
		public override string GetSoundName()
		{
			return "Temperature";
		}

		// Token: 0x0600A630 RID: 42544 RVA: 0x00395A94 File Offset: 0x00393C94
		public Temperature()
		{
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x0600A631 RID: 42545 RVA: 0x0039612B File Offset: 0x0039432B
		public override void Update()
		{
			base.Update();
			if (this.previousUserSetting != SimDebugView.Instance.user_temperatureThresholds)
			{
				this.RefreshLegendValues();
				this.previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
			}
		}

		// Token: 0x0600A632 RID: 42546 RVA: 0x00396160 File Offset: 0x00394360
		public override void Enable()
		{
			base.Enable();
			this.previousUserSetting = SimDebugView.Instance.user_temperatureThresholds;
			this.RefreshLegendValues();
		}

		// Token: 0x0600A633 RID: 42547 RVA: 0x00396180 File Offset: 0x00394380
		public void RefreshLegendValues()
		{
			int num = SimDebugView.Instance.temperatureThresholds.Length - 1;
			for (int i = 0; i < num; i++)
			{
				this.temperatureLegend[i].colour = GlobalAssets.Instance.colorSet.GetColorByName(SimDebugView.Instance.temperatureThresholds[num - i].colorName);
				this.temperatureLegend[i].desc_arg = GameUtil.GetFormattedTemperature(SimDebugView.Instance.temperatureThresholds[num - i].value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
			}
		}

		// Token: 0x0600A634 RID: 42548 RVA: 0x00396215 File Offset: 0x00394415
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.HEATFLOW,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.STATECHANGE,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600A635 RID: 42549 RVA: 0x0039624C File Offset: 0x0039444C
		public override void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			KAnimBatchManager.Instance().RenderKAnimTemperaturePostProcessingEffects();
		}

		// Token: 0x0600A636 RID: 42550 RVA: 0x00396260 File Offset: 0x00394460
		public override List<LegendEntry> GetCustomLegendData()
		{
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				return this.temperatureLegend;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				return this.expandedTemperatureLegend;
			case Game.TemperatureOverlayModes.HeatFlow:
				return this.heatFlowLegend;
			case Game.TemperatureOverlayModes.StateChange:
				return this.stateChangeLegend;
			case Game.TemperatureOverlayModes.RelativeTemperature:
				return new List<LegendEntry>();
			default:
				return this.temperatureLegend;
			}
		}

		// Token: 0x0600A637 RID: 42551 RVA: 0x003962BC File Offset: 0x003944BC
		public override void OnFiltersChanged()
		{
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.HEATFLOW, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.HeatFlow;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ABSOLUTETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AbsoluteTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.RELATIVETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.RelativeTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ADAPTIVETEMPERATURE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.AdaptiveTemperature;
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.STATECHANGE, this.legendFilters))
			{
				Game.Instance.temperatureOverlayMode = Game.TemperatureOverlayModes.StateChange;
			}
			switch (Game.Instance.temperatureOverlayMode)
			{
			case Game.TemperatureOverlayModes.AbsoluteTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.AdaptiveTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			case Game.TemperatureOverlayModes.HeatFlow:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			case Game.TemperatureOverlayModes.StateChange:
				Infrared.Instance.SetMode(Infrared.Mode.Disabled);
				CameraController.Instance.ToggleColouredOverlayView(false);
				return;
			case Game.TemperatureOverlayModes.RelativeTemperature:
				Infrared.Instance.SetMode(Infrared.Mode.Infrared);
				CameraController.Instance.ToggleColouredOverlayView(true);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600A638 RID: 42552 RVA: 0x003963F7 File Offset: 0x003945F7
		public override void Disable()
		{
			Infrared.Instance.SetMode(Infrared.Mode.Disabled);
			CameraController.Instance.ToggleColouredOverlayView(false);
			base.Disable();
		}

		// Token: 0x04008268 RID: 33384
		public static readonly HashedString ID = "Temperature";

		// Token: 0x04008269 RID: 33385
		private Vector2 previousUserSetting;

		// Token: 0x0400826A RID: 33386
		public List<LegendEntry> temperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x0400826B RID: 33387
		public List<LegendEntry> heatFlowLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0.9098039f, 0.25882354f, 0.14901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL, UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0.30980393f, 0.30980393f, 0.30980393f), null, null, true),
			new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0.2509804f, 0.6313726f, 0.90588236f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x0400826C RID: 33388
		public List<LegendEntry> expandedTemperatureLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9843137f, 0.3254902f, 0.3137255f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0.6627451f, 0.14117648f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.9372549f, 1f, 0f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.12156863f, 0.6313726f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.16862746f, 0.79607844f, 1f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};

		// Token: 0x0400826D RID: 33389
		public List<LegendEntry> stateChangeLegend = new List<LegendEntry>
		{
			new LegendEntry(UI.OVERLAYS.STATECHANGE.HIGHPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.HIGHPOINT, new Color(0.8901961f, 0.13725491f, 0.12941177f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.STABLE, UI.OVERLAYS.STATECHANGE.TOOLTIPS.STABLE, new Color(0.23137255f, 0.99607843f, 0.2901961f), null, null, true),
			new LegendEntry(UI.OVERLAYS.STATECHANGE.LOWPOINT, UI.OVERLAYS.STATECHANGE.TOOLTIPS.LOWPOINT, new Color(0.5019608f, 0.99607843f, 0.9411765f), null, null, true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSOURCES, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSOURCES, Color.white, null, Assets.GetSprite("heat_source"), true),
			new LegendEntry(UI.OVERLAYS.TEMPERATURE.HEATSINK, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.HEATSINK, Color.white, null, Assets.GetSprite("heat_sink"), true)
		};
	}

	// Token: 0x02001C37 RID: 7223
	public class TileMode : OverlayModes.Mode
	{
		// Token: 0x0600A63A RID: 42554 RVA: 0x00396426 File Offset: 0x00394626
		public override HashedString ViewMode()
		{
			return OverlayModes.TileMode.ID;
		}

		// Token: 0x0600A63B RID: 42555 RVA: 0x0039642D File Offset: 0x0039462D
		public override string GetSoundName()
		{
			return "SuitRequired";
		}

		// Token: 0x0600A63C RID: 42556 RVA: 0x00396434 File Offset: 0x00394634
		public TileMode()
		{
			OverlayModes.ColorHighlightCondition[] array = new OverlayModes.ColorHighlightCondition[1];
			array[0] = new OverlayModes.ColorHighlightCondition(delegate(KMonoBehaviour primary_element)
			{
				Color result = Color.black;
				if (primary_element != null)
				{
					result = (primary_element as PrimaryElement).Element.substance.uiColour;
				}
				return result;
			}, (KMonoBehaviour primary_element) => primary_element.gameObject.GetComponent<KBatchedAnimController>().IsVisible());
			this.highlightConditions = array;
			base..ctor();
			this.targetLayer = LayerMask.NameToLayer("MaskedOverlay");
			this.cameraLayerMask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay",
				"MaskedOverlayBG"
			});
			this.legendFilters = this.CreateDefaultFilters();
		}

		// Token: 0x0600A63D RID: 42557 RVA: 0x003964EC File Offset: 0x003946EC
		public override void Enable()
		{
			base.Enable();
			List<Tag> prefabTagsWithComponent = Assets.GetPrefabTagsWithComponent<PrimaryElement>();
			this.targetIDs.UnionWith(prefabTagsWithComponent);
			Camera.main.cullingMask |= this.cameraLayerMask;
			int defaultLayerMask = SelectTool.Instance.GetDefaultLayerMask();
			int mask = LayerMask.GetMask(new string[]
			{
				"MaskedOverlay"
			});
			SelectTool.Instance.SetLayerMask(defaultLayerMask | mask);
		}

		// Token: 0x0600A63E RID: 42558 RVA: 0x00396554 File Offset: 0x00394754
		public override void Update()
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			Grid.GetVisibleExtents(out vector2I, out vector2I2);
			OverlayModes.Mode.RemoveOffscreenTargets<PrimaryElement>(this.layerTargets, vector2I, vector2I2, null);
			int height = vector2I2.y - vector2I.y;
			int width = vector2I2.x - vector2I.x;
			Extents extents = new Extents(vector2I.x, vector2I.y, width, height);
			List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.pickupablesLayer, list);
			foreach (ScenePartitionerEntry scenePartitionerEntry in list)
			{
				PrimaryElement component = ((Pickupable)scenePartitionerEntry.obj).gameObject.GetComponent<PrimaryElement>();
				if (component != null)
				{
					this.TryAddObject(component, vector2I, vector2I2);
				}
			}
			list.Clear();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.completeBuildings, list);
			foreach (ScenePartitionerEntry scenePartitionerEntry2 in list)
			{
				BuildingComplete buildingComplete = (BuildingComplete)scenePartitionerEntry2.obj;
				PrimaryElement component2 = buildingComplete.gameObject.GetComponent<PrimaryElement>();
				if (component2 != null && buildingComplete.gameObject.layer == 0)
				{
					this.TryAddObject(component2, vector2I, vector2I2);
				}
			}
			base.UpdateHighlightTypeOverlay<PrimaryElement>(vector2I, vector2I2, this.layerTargets, this.targetIDs, this.highlightConditions, OverlayModes.BringToFrontLayerSetting.Conditional, this.targetLayer);
		}

		// Token: 0x0600A63F RID: 42559 RVA: 0x003966E0 File Offset: 0x003948E0
		private void TryAddObject(PrimaryElement pe, Vector2I min, Vector2I max)
		{
			Element element = pe.Element;
			foreach (Tag search_tag in Game.Instance.tileOverlayFilters)
			{
				if (element.HasTag(search_tag))
				{
					base.AddTargetIfVisible<PrimaryElement>(pe, min, max, this.layerTargets, this.targetLayer, null, null);
					break;
				}
			}
		}

		// Token: 0x0600A640 RID: 42560 RVA: 0x0039675C File Offset: 0x0039495C
		public override void Disable()
		{
			base.Disable();
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			Camera.main.cullingMask &= ~this.cameraLayerMask;
			this.layerTargets.Clear();
			SelectTool.Instance.ClearLayerMask();
		}

		// Token: 0x0600A641 RID: 42561 RVA: 0x003967A8 File Offset: 0x003949A8
		public override Dictionary<string, ToolParameterMenu.ToggleState> CreateDefaultFilters()
		{
			return new Dictionary<string, ToolParameterMenu.ToggleState>
			{
				{
					ToolParameterMenu.FILTERLAYERS.ALL,
					ToolParameterMenu.ToggleState.On
				},
				{
					ToolParameterMenu.FILTERLAYERS.METAL,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.BUILDABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FILTER,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.ORGANICS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.FARMABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.GAS,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.LIQUID,
					ToolParameterMenu.ToggleState.Off
				},
				{
					ToolParameterMenu.FILTERLAYERS.MISC,
					ToolParameterMenu.ToggleState.Off
				}
			};
		}

		// Token: 0x0600A642 RID: 42562 RVA: 0x00396840 File Offset: 0x00394A40
		public override void OnFiltersChanged()
		{
			Game.Instance.tileOverlayFilters.Clear();
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.METAL, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Metal);
				Game.Instance.tileOverlayFilters.Add(GameTags.RefinedMetal);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.BUILDABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableRaw);
				Game.Instance.tileOverlayFilters.Add(GameTags.BuildableProcessed);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FILTER, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Filter);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUIFIABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquifiable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.LIQUID, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Liquid);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.CONSUMABLEORE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.ConsumableOre);
				Game.Instance.tileOverlayFilters.Add(GameTags.Sublimating);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.ORGANICS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Organics);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.FARMABLE, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Farmable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Agriculture);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.GAS, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Breathable);
				Game.Instance.tileOverlayFilters.Add(GameTags.Unbreathable);
			}
			if (base.InFilter(ToolParameterMenu.FILTERLAYERS.MISC, this.legendFilters))
			{
				Game.Instance.tileOverlayFilters.Add(GameTags.Other);
			}
			base.DisableHighlightTypeOverlay<PrimaryElement>(this.layerTargets);
			this.layerTargets.Clear();
			Game.Instance.ForceOverlayUpdate(false);
		}

		// Token: 0x0400826E RID: 33390
		public static readonly HashedString ID = "TileMode";

		// Token: 0x0400826F RID: 33391
		private HashSet<PrimaryElement> layerTargets = new HashSet<PrimaryElement>();

		// Token: 0x04008270 RID: 33392
		private HashSet<Tag> targetIDs = new HashSet<Tag>();

		// Token: 0x04008271 RID: 33393
		private int targetLayer;

		// Token: 0x04008272 RID: 33394
		private int cameraLayerMask;

		// Token: 0x04008273 RID: 33395
		private OverlayModes.ColorHighlightCondition[] highlightConditions;
	}
}
