using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090A RID: 2314
public class DisconnectTool : FilteredDragTool
{
	// Token: 0x060042BE RID: 17086 RVA: 0x0017B939 File Offset: 0x00179B39
	public static void DestroyInstance()
	{
		DisconnectTool.Instance = null;
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x0017B944 File Offset: 0x00179B44
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisconnectTool.Instance = this;
		this.disconnectVisPool = new GameObjectPool(new Func<GameObject>(this.InstantiateDisconnectVis), this.singleDisconnectMode ? 1 : 10);
		if (this.singleDisconnectMode)
		{
			this.lineModeMaxLength = 2;
		}
	}

	// Token: 0x060042C0 RID: 17088 RVA: 0x0017B990 File Offset: 0x00179B90
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060042C1 RID: 17089 RVA: 0x0017B99D File Offset: 0x00179B9D
	protected override DragTool.Mode GetMode()
	{
		if (!this.singleDisconnectMode)
		{
			return DragTool.Mode.Box;
		}
		return DragTool.Mode.Line;
	}

	// Token: 0x060042C2 RID: 17090 RVA: 0x0017B9AA File Offset: 0x00179BAA
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		if (this.singleDisconnectMode)
		{
			upPos = base.SnapToLine(upPos);
		}
		this.RunOnRegion(downPos, upPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.DisconnectCellsAction));
		this.ClearVisualizers();
	}

	// Token: 0x060042C3 RID: 17091 RVA: 0x0017B9D7 File Offset: 0x00179BD7
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.lastRefreshedCell = -1;
	}

	// Token: 0x060042C4 RID: 17092 RVA: 0x0017B9E8 File Offset: 0x00179BE8
	private void DisconnectCellsAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		Building component = objectOnCell.GetComponent<Building>();
		KAnimGraphTileVisualizer component2 = objectOnCell.GetComponent<KAnimGraphTileVisualizer>();
		if (component2 != null)
		{
			UtilityConnections new_connections = utilityComponent.GetNetworkManager().GetConnections(cell, false) & ~removeConnections;
			component2.UpdateConnections(new_connections);
			component2.Refresh();
		}
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		utilityComponent.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x060042C5 RID: 17093 RVA: 0x0017BA54 File Offset: 0x00179C54
	private void RunOnRegion(Vector3 pos1, Vector3 pos2, Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections> action)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(pos1, pos2), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(pos1, pos2), false);
		Vector2I vector2I = new Vector2I((int)regularizedPos.x, (int)regularizedPos.y);
		Vector2I vector2I2 = new Vector2I((int)regularizedPos2.x, (int)regularizedPos2.y);
		for (int i = vector2I.x; i < vector2I2.x; i++)
		{
			for (int j = vector2I.y; j < vector2I2.y; j++)
			{
				int num = Grid.XYToCell(i, j);
				if (Grid.IsVisible(num))
				{
					for (int k = 0; k < 45; k++)
					{
						GameObject gameObject = Grid.Objects[num, k];
						if (!(gameObject == null))
						{
							string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
							if (base.IsActiveLayer(filterLayerFromGameObject))
							{
								Building component = gameObject.GetComponent<Building>();
								if (!(component == null))
								{
									IHaveUtilityNetworkMgr component2 = component.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
									if (!component2.IsNullOrDestroyed())
									{
										UtilityConnections connections = component2.GetNetworkManager().GetConnections(num, false);
										UtilityConnections utilityConnections = (UtilityConnections)0;
										if ((connections & UtilityConnections.Left) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, -1, 0))
										{
											utilityConnections |= UtilityConnections.Left;
										}
										if ((connections & UtilityConnections.Right) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 1, 0))
										{
											utilityConnections |= UtilityConnections.Right;
										}
										if ((connections & UtilityConnections.Up) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, 1))
										{
											utilityConnections |= UtilityConnections.Up;
										}
										if ((connections & UtilityConnections.Down) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, -1))
										{
											utilityConnections |= UtilityConnections.Down;
										}
										if (utilityConnections > (UtilityConnections)0)
										{
											action(num, gameObject, component2, utilityConnections);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060042C6 RID: 17094 RVA: 0x0017BC20 File Offset: 0x00179E20
	private bool IsInsideRegion(Vector2I min, Vector2I max, int cell, int xoff, int yoff)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, xoff, yoff), out num, out num2);
		return num >= min.x && num < max.x && num2 >= min.y && num2 < max.y;
	}

	// Token: 0x060042C7 RID: 17095 RVA: 0x0017BC68 File Offset: 0x00179E68
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		if (!base.Dragging)
		{
			return;
		}
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.singleDisconnectMode)
		{
			cursorPos = base.SnapToLine(cursorPos);
		}
		int num = Grid.PosToCell(cursorPos);
		if (this.lastRefreshedCell == num)
		{
			return;
		}
		this.lastRefreshedCell = num;
		this.ClearVisualizers();
		this.RunOnRegion(this.downPos, cursorPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.VisualizeAction));
	}

	// Token: 0x060042C8 RID: 17096 RVA: 0x0017BCE0 File Offset: 0x00179EE0
	private GameObject InstantiateDisconnectVis()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.singleDisconnectMode ? this.disconnectVisSingleModePrefab : this.disconnectVisMultiModePrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x060042C9 RID: 17097 RVA: 0x0017BD08 File Offset: 0x00179F08
	private void VisualizeAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		if ((removeConnections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellBelow(cell), true);
		}
		if ((removeConnections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellRight(cell), false);
		}
	}

	// Token: 0x060042CA RID: 17098 RVA: 0x0017BD34 File Offset: 0x00179F34
	private void CreateVisualizer(int cell1, int cell2, bool rotate)
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			if (visData.Equals(cell1, cell2))
			{
				return;
			}
		}
		Vector3 a = Grid.CellToPosCCC(cell1, Grid.SceneLayer.FXFront);
		Vector3 b = Grid.CellToPosCCC(cell2, Grid.SceneLayer.FXFront);
		GameObject instance = this.disconnectVisPool.GetInstance();
		instance.transform.rotation = Quaternion.Euler(0f, 0f, (float)(rotate ? 90 : 0));
		instance.transform.SetPosition(Vector3.Lerp(a, b, 0.5f));
		instance.SetActive(true);
		this.visualizersInUse.Add(new DisconnectTool.VisData(cell1, cell2, instance));
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x0017BE04 File Offset: 0x0017A004
	private void ClearVisualizers()
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			visData.go.SetActive(false);
			this.disconnectVisPool.ReleaseInstance(visData.go);
		}
		this.visualizersInUse.Clear();
	}

	// Token: 0x060042CC RID: 17100 RVA: 0x0017BE78 File Offset: 0x0017A078
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.ClearVisualizers();
	}

	// Token: 0x060042CD RID: 17101 RVA: 0x0017BE87 File Offset: 0x0017A087
	protected override string GetConfirmSound()
	{
		return "OutletDisconnected";
	}

	// Token: 0x060042CE RID: 17102 RVA: 0x0017BE8E File Offset: 0x0017A08E
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x060042CF RID: 17103 RVA: 0x0017BE98 File Offset: 0x0017A098
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x04002C09 RID: 11273
	[SerializeField]
	private GameObject disconnectVisSingleModePrefab;

	// Token: 0x04002C0A RID: 11274
	[SerializeField]
	private GameObject disconnectVisMultiModePrefab;

	// Token: 0x04002C0B RID: 11275
	private GameObjectPool disconnectVisPool;

	// Token: 0x04002C0C RID: 11276
	private List<DisconnectTool.VisData> visualizersInUse = new List<DisconnectTool.VisData>();

	// Token: 0x04002C0D RID: 11277
	private int lastRefreshedCell;

	// Token: 0x04002C0E RID: 11278
	private bool singleDisconnectMode = true;

	// Token: 0x04002C0F RID: 11279
	public static DisconnectTool Instance;

	// Token: 0x0200186E RID: 6254
	public struct VisData
	{
		// Token: 0x0600988F RID: 39055 RVA: 0x00368606 File Offset: 0x00366806
		public VisData(int cell1, int cell2, GameObject go)
		{
			this.cell1 = cell1;
			this.cell2 = cell2;
			this.go = go;
		}

		// Token: 0x06009890 RID: 39056 RVA: 0x0036861D File Offset: 0x0036681D
		public bool Equals(int cell1, int cell2)
		{
			return (this.cell1 == cell1 && this.cell2 == cell2) || (this.cell1 == cell2 && this.cell2 == cell1);
		}

		// Token: 0x0400763D RID: 30269
		public readonly int cell1;

		// Token: 0x0400763E RID: 30270
		public readonly int cell2;

		// Token: 0x0400763F RID: 30271
		public GameObject go;
	}
}
