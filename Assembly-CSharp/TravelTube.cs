using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200078B RID: 1931
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTube")]
public class TravelTube : KMonoBehaviour, IFirstFrameCallback, ITravelTubePiece, IHaveUtilityNetworkMgr
{
	// Token: 0x06003493 RID: 13459 RVA: 0x0011EB07 File Offset: 0x0011CD07
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06003494 RID: 13460 RVA: 0x0011EB13 File Offset: 0x0011CD13
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x0011EB20 File Offset: 0x0011CD20
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Grid.HasTube[Grid.PosToCell(this)] = true;
		Components.ITravelTubePieces.Add(this);
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x0011EB44 File Offset: 0x0011CD44
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.travelTubeSystem.AddToNetworks(cell, this, false);
		base.Subscribe<TravelTube>(-1041684577, TravelTube.OnConnectionsChangedDelegate);
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x0011EB8C File Offset: 0x0011CD8C
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.travelTubeSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe(-1041684577);
		Grid.HasTube[Grid.PosToCell(this)] = false;
		Components.ITravelTubePieces.Remove(this);
		GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x0011EC30 File Offset: 0x0011CE30
	private void OnConnectionsChanged(object data)
	{
		this.connections = (UtilityConnections)data;
		bool flag = this.connections == UtilityConnections.Up || this.connections == UtilityConnections.Down || this.connections == UtilityConnections.Left || this.connections == UtilityConnections.Right;
		if (flag != this.isExitTube)
		{
			this.isExitTube = flag;
			this.UpdateExitListener(this.isExitTube);
			this.UpdateExitStatus();
		}
	}

	// Token: 0x06003499 RID: 13465 RVA: 0x0011EC94 File Offset: 0x0011CE94
	private void UpdateExitListener(bool enable)
	{
		if (enable && !this.dirtyNavCellUpdatedEntry.IsValid())
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.dirtyNavCellUpdatedEntry = GameScenePartitioner.Instance.Add("TravelTube.OnDirtyNavCellUpdated", this, cell, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, new Action<object>(this.OnDirtyNavCellUpdated));
			this.OnDirtyNavCellUpdated(null);
			return;
		}
		if (!enable && this.dirtyNavCellUpdatedEntry.IsValid())
		{
			GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
		}
	}

	// Token: 0x0600349A RID: 13466 RVA: 0x0011ED18 File Offset: 0x0011CF18
	private void OnDirtyNavCellUpdated(object data)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		int num2 = num * navGrid.maxLinksPerCell;
		bool flag = false;
		if (this.isExitTube)
		{
			NavGrid.Link link = navGrid.Links[num2];
			while (link.link != PathFinder.InvalidHandle)
			{
				if (link.startNavType == NavType.Tube)
				{
					if (link.endNavType != NavType.Tube)
					{
						flag = true;
						break;
					}
					UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(link.link, num);
					if (this.connections == utilityConnections)
					{
						flag = true;
						break;
					}
				}
				num2++;
				link = navGrid.Links[num2];
			}
		}
		if (flag != this.hasValidExitTransitions)
		{
			this.hasValidExitTransitions = flag;
			this.UpdateExitStatus();
		}
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x0011EDD4 File Offset: 0x0011CFD4
	private void UpdateExitStatus()
	{
		if (!this.isExitTube || this.hasValidExitTransitions)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NoTubeExits, null);
		}
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x0011EE3D File Offset: 0x0011D03D
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x0011EE53 File Offset: 0x0011D053
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04001F10 RID: 7952
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001F11 RID: 7953
	private HandleVector<int>.Handle dirtyNavCellUpdatedEntry;

	// Token: 0x04001F12 RID: 7954
	private bool isExitTube;

	// Token: 0x04001F13 RID: 7955
	private bool hasValidExitTransitions;

	// Token: 0x04001F14 RID: 7956
	private UtilityConnections connections;

	// Token: 0x04001F15 RID: 7957
	private static readonly EventSystem.IntraObjectHandler<TravelTube> OnConnectionsChangedDelegate = new EventSystem.IntraObjectHandler<TravelTube>(delegate(TravelTube component, object data)
	{
		component.OnConnectionsChanged(data);
	});

	// Token: 0x04001F16 RID: 7958
	private Guid connectedStatus;

	// Token: 0x04001F17 RID: 7959
	private System.Action firstFrameCallback;
}
