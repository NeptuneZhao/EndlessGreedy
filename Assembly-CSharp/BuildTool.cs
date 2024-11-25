using System;
using System.Collections.Generic;
using FMOD.Studio;
using Rendering;
using STRINGS;
using UnityEngine;

// Token: 0x02000901 RID: 2305
public class BuildTool : DragTool
{
	// Token: 0x0600425B RID: 16987 RVA: 0x001799ED File Offset: 0x00177BED
	public static void DestroyInstance()
	{
		BuildTool.Instance = null;
	}

	// Token: 0x0600425C RID: 16988 RVA: 0x001799F5 File Offset: 0x00177BF5
	protected override void OnPrefabInit()
	{
		BuildTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
		this.buildingCount = UnityEngine.Random.Range(1, 14);
		this.canChangeDragAxis = false;
	}

	// Token: 0x0600425D RID: 16989 RVA: 0x00179A20 File Offset: 0x00177C20
	protected override void OnActivateTool()
	{
		this.lastDragCell = -1;
		if (this.visualizer != null)
		{
			this.ClearTilePreview();
			UnityEngine.Object.Destroy(this.visualizer);
		}
		this.active = true;
		base.OnActivateTool();
		Vector3 vector = base.ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
		this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, vector, Grid.SceneLayer.Ore, null, LayerMask.NameToLayer("Place"));
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.visibilityType = KAnimControllerBase.VisibilityType.Always;
			component.isMovable = true;
			component.Offset = this.def.GetVisualizerOffset();
			component.name = component.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
		}
		if (!this.facadeID.IsNullOrWhiteSpace() && this.facadeID != "DEFAULT_FACADE")
		{
			this.visualizer.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeID), false);
		}
		Rotatable component2 = this.visualizer.GetComponent<Rotatable>();
		if (component2 != null)
		{
			this.buildingOrientation = this.def.InitialOrientation;
			component2.SetOrientation(this.buildingOrientation);
		}
		this.visualizer.SetActive(true);
		this.UpdateVis(vector);
		base.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		if (component == null)
		{
			this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		}
		else
		{
			component.SetLayer(LayerMask.NameToLayer("Place"));
		}
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x0600425E RID: 16990 RVA: 0x00179BD0 File Offset: 0x00177DD0
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.lastDragCell = -1;
		if (!this.active)
		{
			return;
		}
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.buildingOrientation = Orientation.Neutral;
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		this.ClearTilePreview();
		UnityEngine.Object.Destroy(this.visualizer);
		if (new_tool == SelectTool.Instance)
		{
			Game.Instance.Trigger(-1190690038, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x0600425F RID: 16991 RVA: 0x00179C4B File Offset: 0x00177E4B
	public void Activate(BuildingDef def, IList<Tag> selected_elements)
	{
		this.selectedElements = selected_elements;
		this.def = def;
		this.viewMode = def.ViewMode;
		ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
		PlayerController.Instance.ActivateTool(this);
		this.OnActivateTool();
	}

	// Token: 0x06004260 RID: 16992 RVA: 0x00179C89 File Offset: 0x00177E89
	public void Activate(BuildingDef def, IList<Tag> selected_elements, string facadeID)
	{
		this.facadeID = facadeID;
		this.Activate(def, selected_elements);
	}

	// Token: 0x06004261 RID: 16993 RVA: 0x00179C9A File Offset: 0x00177E9A
	public void Deactivate()
	{
		this.selectedElements = null;
		SelectTool.Instance.Activate();
		this.def = null;
		this.facadeID = null;
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06004262 RID: 16994 RVA: 0x00179CC5 File Offset: 0x00177EC5
	public int GetLastCell
	{
		get
		{
			return this.lastCell;
		}
	}

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06004263 RID: 16995 RVA: 0x00179CCD File Offset: 0x00177ECD
	public Orientation GetBuildingOrientation
	{
		get
		{
			return this.buildingOrientation;
		}
	}

	// Token: 0x06004264 RID: 16996 RVA: 0x00179CD8 File Offset: 0x00177ED8
	private void ClearTilePreview()
	{
		if (Grid.IsValidBuildingCell(this.lastCell) && this.def.IsTilePiece)
		{
			GameObject gameObject = Grid.Objects[this.lastCell, (int)this.def.TileLayer];
			if (this.visualizer == gameObject)
			{
				Grid.Objects[this.lastCell, (int)this.def.TileLayer] = null;
			}
			if (this.def.isKAnimTile)
			{
				GameObject x = null;
				if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
				{
					x = Grid.Objects[this.lastCell, (int)this.def.ReplacementLayer];
				}
				if ((gameObject == null || gameObject.GetComponent<Constructable>() == null) && (x == null || x == this.visualizer))
				{
					World.Instance.blockTileRenderer.RemoveBlock(this.def, false, SimHashes.Void, this.lastCell);
					World.Instance.blockTileRenderer.RemoveBlock(this.def, true, SimHashes.Void, this.lastCell);
					TileVisualizer.RefreshCell(this.lastCell, this.def.TileLayer, this.def.ReplacementLayer);
				}
			}
		}
	}

	// Token: 0x06004265 RID: 16997 RVA: 0x00179E19 File Offset: 0x00178019
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		this.UpdateVis(cursorPos);
	}

	// Token: 0x06004266 RID: 16998 RVA: 0x00179E3C File Offset: 0x0017803C
	private void UpdateVis(Vector3 pos)
	{
		string text;
		bool flag = this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text);
		bool flag2 = this.def.IsValidReplaceLocation(pos, this.buildingOrientation, this.def.ReplacementLayer, this.def.ObjectLayer);
		flag = (flag || flag2);
		if (this.visualizer != null)
		{
			Color c = Color.white;
			float strength = 0f;
			if (!flag)
			{
				c = Color.red;
				strength = 1f;
			}
			this.SetColor(this.visualizer, c, strength);
		}
		int num = Grid.PosToCell(pos);
		if (this.def != null)
		{
			Vector3 vector = Grid.CellToPosCBC(num, this.def.SceneLayer);
			this.visualizer.transform.SetPosition(vector);
			base.transform.SetPosition(vector - Vector3.up * 0.5f);
			if (this.def.IsTilePiece)
			{
				this.ClearTilePreview();
				if (Grid.IsValidBuildingCell(num))
				{
					GameObject gameObject = Grid.Objects[num, (int)this.def.TileLayer];
					if (gameObject == null)
					{
						Grid.Objects[num, (int)this.def.TileLayer] = this.visualizer;
					}
					if (this.def.isKAnimTile)
					{
						GameObject x = null;
						if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
						{
							x = Grid.Objects[num, (int)this.def.ReplacementLayer];
						}
						if (gameObject == null || (gameObject.GetComponent<Constructable>() == null && x == null))
						{
							TileVisualizer.RefreshCell(num, this.def.TileLayer, this.def.ReplacementLayer);
							if (this.def.BlockTileAtlas != null)
							{
								int renderLayer = LayerMask.NameToLayer("Overlay");
								BlockTileRenderer blockTileRenderer = World.Instance.blockTileRenderer;
								blockTileRenderer.SetInvalidPlaceCell(num, !flag);
								if (this.lastCell != num)
								{
									blockTileRenderer.SetInvalidPlaceCell(this.lastCell, false);
								}
								blockTileRenderer.AddBlock(renderLayer, this.def, flag2, SimHashes.Void, num);
							}
						}
					}
				}
			}
			if (this.lastCell != num)
			{
				this.lastCell = num;
			}
		}
	}

	// Token: 0x06004267 RID: 16999 RVA: 0x0017A080 File Offset: 0x00178280
	public PermittedRotations? GetPermittedRotations()
	{
		if (this.visualizer == null)
		{
			return null;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return null;
		}
		return new PermittedRotations?(component.permittedRotations);
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x0017A0CF File Offset: 0x001782CF
	public bool CanRotate()
	{
		return !(this.visualizer == null) && !(this.visualizer.GetComponent<Rotatable>() == null);
	}

	// Token: 0x06004269 RID: 17001 RVA: 0x0017A0F8 File Offset: 0x001782F8
	public void TryRotate()
	{
		if (this.visualizer == null)
		{
			return;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Rotate", false));
		this.buildingOrientation = component.Rotate();
		if (Grid.IsValidBuildingCell(this.lastCell))
		{
			Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
			this.UpdateVis(pos);
		}
		if (base.Dragging && this.lastDragCell != -1)
		{
			this.TryBuild(this.lastDragCell);
		}
	}

	// Token: 0x0600426A RID: 17002 RVA: 0x0017A185 File Offset: 0x00178385
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.RotateBuilding))
		{
			this.TryRotate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600426B RID: 17003 RVA: 0x0017A1A2 File Offset: 0x001783A2
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.TryBuild(cell);
	}

	// Token: 0x0600426C RID: 17004 RVA: 0x0017A1AC File Offset: 0x001783AC
	private void TryBuild(int cell)
	{
		if (this.visualizer == null)
		{
			return;
		}
		if (cell == this.lastDragCell && this.buildingOrientation == this.lastDragOrientation)
		{
			return;
		}
		if (Grid.PosToCell(this.visualizer) != cell)
		{
			if (this.def.BuildingComplete.GetComponent<LogicPorts>())
			{
				return;
			}
			if (this.def.BuildingComplete.GetComponent<LogicGateBase>())
			{
				return;
			}
		}
		this.lastDragCell = cell;
		this.lastDragOrientation = this.buildingOrientation;
		this.ClearTilePreview();
		Vector3 pos = Grid.CellToPosCBC(cell, Grid.SceneLayer.Building);
		GameObject gameObject = null;
		PlanScreen.Instance.LastSelectedBuildingFacade = this.facadeID;
		bool flag = DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild);
		string text;
		if (!flag)
		{
			gameObject = this.def.TryPlace(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
		}
		else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, false) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text))
		{
			float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			gameObject = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
		}
		if (gameObject == null && this.def.ReplacementLayer != ObjectLayer.NumLayers)
		{
			GameObject replacementCandidate = this.def.GetReplacementCandidate(cell);
			if (replacementCandidate != null && !this.def.IsReplacementLayerOccupied(cell))
			{
				BuildingComplete component = replacementCandidate.GetComponent<BuildingComplete>();
				if (component != null && component.Def.Replaceable && this.def.CanReplace(replacementCandidate))
				{
					Tag b2 = replacementCandidate.GetComponent<PrimaryElement>().Element.tag;
					if (b2.GetHash() == 1542131326)
					{
						b2 = SimHashes.Snow.CreateTag();
					}
					if (component.Def != this.def || this.selectedElements[0] != b2)
					{
						string text2;
						if (!flag)
						{
							gameObject = this.def.TryReplaceTile(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
							Grid.Objects[cell, (int)this.def.ReplacementLayer] = gameObject;
						}
						else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, true) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, true, out text2))
						{
							gameObject = this.InstantBuildReplace(cell, pos, replacementCandidate);
						}
					}
				}
			}
		}
		this.PostProcessBuild(flag, pos, gameObject);
	}

	// Token: 0x0600426D RID: 17005 RVA: 0x0017A49C File Offset: 0x0017869C
	private GameObject InstantBuildReplace(int cell, Vector3 pos, GameObject tile)
	{
		if (tile.GetComponent<SimCellOccupier>() == null)
		{
			UnityEngine.Object.Destroy(tile);
			float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			return this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
		}
		tile.GetComponent<SimCellOccupier>().DestroySelf(delegate
		{
			UnityEngine.Object.Destroy(tile);
			float b2 = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			GameObject builtItem = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b2), this.facadeID, false, GameClock.Instance.GetTime());
			this.PostProcessBuild(true, pos, builtItem);
		});
		return null;
	}

	// Token: 0x0600426E RID: 17006 RVA: 0x0017A55C File Offset: 0x0017875C
	private void PostProcessBuild(bool instantBuild, Vector3 pos, GameObject builtItem)
	{
		if (builtItem == null)
		{
			return;
		}
		if (!instantBuild)
		{
			Prioritizable component = builtItem.GetComponent<Prioritizable>();
			if (component != null)
			{
				if (BuildMenu.Instance != null)
				{
					component.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
				}
				if (PlanScreen.Instance != null)
				{
					component.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
				}
			}
		}
		if (this.def.MaterialsAvailable(this.selectedElements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
		{
			this.placeSound = GlobalAssets.GetSound("Place_Building_" + this.def.AudioSize, false);
			if (this.placeSound != null)
			{
				this.buildingCount = this.buildingCount % 14 + 1;
				Vector3 pos2 = pos;
				pos2.z = 0f;
				EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, pos2, 1f, false);
				if (this.def.AudioSize == "small")
				{
					instance.setParameterByName("tileCount", (float)this.buildingCount, false);
				}
				SoundEvent.EndOneShot(instance);
			}
		}
		else
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, pos, 1.5f, false, false);
		}
		if (this.def.OnePerWorld)
		{
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x0600426F RID: 17007 RVA: 0x0017A6BE File Offset: 0x001788BE
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06004270 RID: 17008 RVA: 0x0017A6C4 File Offset: 0x001788C4
	private void SetColor(GameObject root, Color c, float strength)
	{
		KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.TintColour = c;
		}
	}

	// Token: 0x06004271 RID: 17009 RVA: 0x0017A6ED File Offset: 0x001788ED
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x06004272 RID: 17010 RVA: 0x0017A6FF File Offset: 0x001788FF
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x06004273 RID: 17011 RVA: 0x0017A714 File Offset: 0x00178914
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x06004274 RID: 17012 RVA: 0x0017A74E File Offset: 0x0017894E
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x06004275 RID: 17013 RVA: 0x0017A755 File Offset: 0x00178955
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x06004276 RID: 17014 RVA: 0x0017A75E File Offset: 0x0017895E
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x06004277 RID: 17015 RVA: 0x0017A768 File Offset: 0x00178968
	public void SetToolOrientation(Orientation orientation)
	{
		if (this.visualizer != null)
		{
			Rotatable component = this.visualizer.GetComponent<Rotatable>();
			if (component != null)
			{
				this.buildingOrientation = orientation;
				component.SetOrientation(orientation);
				if (Grid.IsValidBuildingCell(this.lastCell))
				{
					Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
					this.UpdateVis(pos);
				}
				if (base.Dragging && this.lastDragCell != -1)
				{
					this.TryBuild(this.lastDragCell);
				}
			}
		}
	}

	// Token: 0x04002BEF RID: 11247
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x04002BF0 RID: 11248
	private int lastCell = -1;

	// Token: 0x04002BF1 RID: 11249
	private int lastDragCell = -1;

	// Token: 0x04002BF2 RID: 11250
	private Orientation lastDragOrientation;

	// Token: 0x04002BF3 RID: 11251
	private IList<Tag> selectedElements;

	// Token: 0x04002BF4 RID: 11252
	private BuildingDef def;

	// Token: 0x04002BF5 RID: 11253
	private Orientation buildingOrientation;

	// Token: 0x04002BF6 RID: 11254
	private string facadeID;

	// Token: 0x04002BF7 RID: 11255
	private ToolTip tooltip;

	// Token: 0x04002BF8 RID: 11256
	public static BuildTool Instance;

	// Token: 0x04002BF9 RID: 11257
	private bool active;

	// Token: 0x04002BFA RID: 11258
	private int buildingCount;
}
