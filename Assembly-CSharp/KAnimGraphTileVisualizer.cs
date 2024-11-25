using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000573 RID: 1395
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGraphTileVisualizer")]
public class KAnimGraphTileVisualizer : KMonoBehaviour, ISaveLoadable, IUtilityItem
{
	// Token: 0x17000153 RID: 339
	// (get) Token: 0x0600204A RID: 8266 RVA: 0x000B5207 File Offset: 0x000B3407
	// (set) Token: 0x0600204B RID: 8267 RVA: 0x000B520F File Offset: 0x000B340F
	public UtilityConnections Connections
	{
		get
		{
			return this._connections;
		}
		set
		{
			this._connections = value;
			base.Trigger(-1041684577, this._connections);
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x0600204C RID: 8268 RVA: 0x000B5230 File Offset: 0x000B3430
	public IUtilityNetworkMgr ConnectionManager
	{
		get
		{
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				return Game.Instance.gasConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				return Game.Instance.liquidConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				return Game.Instance.electricalConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				return Game.Instance.logicCircuitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				return Game.Instance.travelTubeSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				return Game.Instance.solidConduitSystem;
			default:
				return null;
			}
		}
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x000B52A8 File Offset: 0x000B34A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.connectionManager = this.ConnectionManager;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.connectionManager.SetConnections(this.Connections, cell, this.isPhysicalBuilding);
		Building component = base.GetComponent<Building>();
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x000B5314 File Offset: 0x000B3514
	protected override void OnCleanUp()
	{
		if (this.connectionManager != null && !this.skipCleanup)
		{
			this.skipRefresh = true;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.ClearCell(cell, this.isPhysicalBuilding);
			Building component = base.GetComponent<Building>();
			TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		}
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000B5380 File Offset: 0x000B3580
	[ContextMenu("Refresh")]
	public void Refresh()
	{
		if (this.connectionManager == null || this.skipRefresh)
		{
			return;
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.Connections = this.connectionManager.GetConnections(cell, this.isPhysicalBuilding);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			string text = this.connectionManager.GetVisualizerString(cell);
			if (base.GetComponent<BuildingUnderConstruction>() != null && component.HasAnimation(text + "_place"))
			{
				text += "_place";
			}
			if (text != null && text != "")
			{
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000B5440 File Offset: 0x000B3640
	public int GetNetworkID()
	{
		UtilityNetwork network = this.GetNetwork();
		if (network == null)
		{
			return -1;
		}
		return network.id;
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000B5460 File Offset: 0x000B3660
	private UtilityNetwork GetNetwork()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, Direction.None);
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000B548C File Offset: 0x000B368C
	public UtilityNetwork GetNetworkForDirection(Direction d)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, d);
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000B54B8 File Offset: 0x000B36B8
	public void UpdateConnections(UtilityConnections new_connections)
	{
		this._connections = new_connections;
		if (this.connectionManager != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.SetConnections(new_connections, cell, this.isPhysicalBuilding);
		}
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000B54F8 File Offset: 0x000B36F8
	public KAnimGraphTileVisualizer GetNeighbour(Direction d)
	{
		KAnimGraphTileVisualizer result = null;
		Vector2I vector2I;
		Grid.PosToXY(base.transform.GetPosition(), out vector2I);
		int num = -1;
		switch (d)
		{
		case Direction.Up:
			if (vector2I.y < Grid.HeightInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y + 1);
			}
			break;
		case Direction.Right:
			if (vector2I.x < Grid.WidthInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x + 1, vector2I.y);
			}
			break;
		case Direction.Down:
			if (vector2I.y > 0)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y - 1);
			}
			break;
		case Direction.Left:
			if (vector2I.x > 0)
			{
				num = Grid.XYToCell(vector2I.x - 1, vector2I.y);
			}
			break;
		}
		if (num != -1)
		{
			ObjectLayer layer;
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				layer = ObjectLayer.GasConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				layer = ObjectLayer.LiquidConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				layer = ObjectLayer.WireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				layer = ObjectLayer.LogicWireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				layer = ObjectLayer.TravelTubeTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				layer = ObjectLayer.SolidConduitTile;
				break;
			default:
				throw new ArgumentNullException("wtf");
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (gameObject != null)
			{
				result = gameObject.GetComponent<KAnimGraphTileVisualizer>();
			}
		}
		return result;
	}

	// Token: 0x0400123B RID: 4667
	[Serialize]
	private UtilityConnections _connections;

	// Token: 0x0400123C RID: 4668
	public bool isPhysicalBuilding;

	// Token: 0x0400123D RID: 4669
	public bool skipCleanup;

	// Token: 0x0400123E RID: 4670
	public bool skipRefresh;

	// Token: 0x0400123F RID: 4671
	public KAnimGraphTileVisualizer.ConnectionSource connectionSource;

	// Token: 0x04001240 RID: 4672
	[NonSerialized]
	public IUtilityNetworkMgr connectionManager;

	// Token: 0x02001370 RID: 4976
	public enum ConnectionSource
	{
		// Token: 0x0400669E RID: 26270
		Gas,
		// Token: 0x0400669F RID: 26271
		Liquid,
		// Token: 0x040066A0 RID: 26272
		Electrical,
		// Token: 0x040066A1 RID: 26273
		Logic,
		// Token: 0x040066A2 RID: 26274
		Tube,
		// Token: 0x040066A3 RID: 26275
		Solid
	}
}
