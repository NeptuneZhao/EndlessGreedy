using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using STRINGS;
using UnityEngine;

// Token: 0x02000594 RID: 1428
public class Navigator : StateMachineComponent<Navigator.StatesInstance>, ISaveLoadableDetails
{
	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06002169 RID: 8553 RVA: 0x000BB05F File Offset: 0x000B925F
	// (set) Token: 0x0600216A RID: 8554 RVA: 0x000BB067 File Offset: 0x000B9267
	public KMonoBehaviour target { get; set; }

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x0600216B RID: 8555 RVA: 0x000BB070 File Offset: 0x000B9270
	// (set) Token: 0x0600216C RID: 8556 RVA: 0x000BB078 File Offset: 0x000B9278
	public CellOffset[] targetOffsets { get; private set; }

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x0600216D RID: 8557 RVA: 0x000BB081 File Offset: 0x000B9281
	// (set) Token: 0x0600216E RID: 8558 RVA: 0x000BB089 File Offset: 0x000B9289
	public NavGrid NavGrid { get; private set; }

	// Token: 0x0600216F RID: 8559 RVA: 0x000BB094 File Offset: 0x000B9294
	public void Serialize(BinaryWriter writer)
	{
		byte currentNavType = (byte)this.CurrentNavType;
		writer.Write(currentNavType);
		writer.Write(this.distanceTravelledByNavType.Count);
		foreach (KeyValuePair<NavType, int> keyValuePair in this.distanceTravelledByNavType)
		{
			byte key = (byte)keyValuePair.Key;
			writer.Write(key);
			writer.Write(keyValuePair.Value);
		}
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x000BB11C File Offset: 0x000B931C
	public void Deserialize(IReader reader)
	{
		NavType navType = (NavType)reader.ReadByte();
		if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 11))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				NavType key = (NavType)reader.ReadByte();
				int value = reader.ReadInt32();
				if (this.distanceTravelledByNavType.ContainsKey(key))
				{
					this.distanceTravelledByNavType[key] = value;
				}
			}
		}
		bool flag = false;
		NavType[] validNavTypes = this.NavGrid.ValidNavTypes;
		for (int j = 0; j < validNavTypes.Length; j++)
		{
			if (validNavTypes[j] == navType)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.CurrentNavType = navType;
		}
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x000BB1C4 File Offset: 0x000B93C4
	protected override void OnPrefabInit()
	{
		this.transitionDriver = new TransitionDriver(this);
		this.targetLocator = Util.KInstantiate(Assets.GetPrefab(TargetLocator.ID), null, null).GetComponent<KPrefabID>();
		this.targetLocator.gameObject.SetActive(true);
		this.log = new LoggerFSS("Navigator", 35);
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
		this.NavGrid = Pathfinding.Instance.GetNavGrid(this.NavGridName);
		base.GetComponent<PathProber>().SetValidNavTypes(this.NavGrid.ValidNavTypes, this.maxProbingRadius);
		this.distanceTravelledByNavType = new Dictionary<NavType, int>();
		for (int i = 0; i < 11; i++)
		{
			this.distanceTravelledByNavType.Add((NavType)i, 0);
		}
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x000BB288 File Offset: 0x000B9488
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Navigator>(1623392196, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(-1506500077, Navigator.OnDefeatedDelegate);
		base.Subscribe<Navigator>(493375141, Navigator.OnRefreshUserMenuDelegate);
		base.Subscribe<Navigator>(-1503271301, Navigator.OnSelectObjectDelegate);
		base.Subscribe<Navigator>(856640610, Navigator.OnStoreDelegate);
		if (this.updateProber)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
		this.pathProbeTask = new Navigator.PathProbeTask(this);
		this.SetCurrentNavType(this.CurrentNavType);
		this.SubscribeUnstuckFunctions();
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x000BB322 File Offset: 0x000B9522
	private void SubscribeUnstuckFunctions()
	{
		if (this.CurrentNavType == NavType.Tube)
		{
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
		}
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x000BB34F File Offset: 0x000B954F
	private void UnsubscribeUnstuckFunctions()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingTileChanged));
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x000BB374 File Offset: 0x000B9574
	private void OnBuildingTileChanged(int cell, object building)
	{
		if (this.CurrentNavType == NavType.Tube && building == null)
		{
			bool flag = cell == Grid.PosToCell(this);
			if (base.smi != null && flag)
			{
				this.SetCurrentNavType(NavType.Floor);
				this.UnsubscribeUnstuckFunctions();
			}
		}
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x000BB3B1 File Offset: 0x000B95B1
	protected override void OnCleanUp()
	{
		this.UnsubscribeUnstuckFunctions();
		base.OnCleanUp();
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x000BB3BF File Offset: 0x000B95BF
	public bool IsMoving()
	{
		return base.smi.IsInsideState(base.smi.sm.normal.moving);
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x000BB3E1 File Offset: 0x000B95E1
	public bool GoTo(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, NavigationTactics.ReduceTravelDistance);
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x000BB419 File Offset: 0x000B9619
	public bool GoTo(int cell, CellOffset[] offsets, NavTactic tactic)
	{
		if (offsets == null)
		{
			offsets = new CellOffset[1];
		}
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		return this.GoTo(this.targetLocator, offsets, tactic);
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x000BB44D File Offset: 0x000B964D
	public void UpdateTarget(int cell)
	{
		this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x000BB468 File Offset: 0x000B9668
	public bool GoTo(KMonoBehaviour target, CellOffset[] offsets, NavTactic tactic)
	{
		if (tactic == null)
		{
			tactic = NavigationTactics.ReduceTravelDistance;
		}
		base.smi.GoTo(base.smi.sm.normal.moving);
		base.smi.sm.moveTarget.Set(target.gameObject, base.smi, false);
		this.tactic = tactic;
		this.target = target;
		this.targetOffsets = offsets;
		this.ClearReservedCell();
		this.AdvancePath(true);
		return this.IsMoving();
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x000BB4EA File Offset: 0x000B96EA
	public void BeginTransition(NavGrid.Transition transition)
	{
		this.transitionDriver.EndTransition();
		base.smi.GoTo(base.smi.sm.normal.moving);
		this.transitionDriver.BeginTransition(this, transition, this.defaultSpeed);
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x000BB52C File Offset: 0x000B972C
	private bool ValidatePath(ref PathFinder.Path path, out bool atNextNode)
	{
		atNextNode = false;
		bool flag = false;
		if (path.IsValid())
		{
			int target_cell = Grid.PosToCell(this.target);
			flag = (this.reservedCell != NavigationReservations.InvalidReservation && this.CanReach(this.reservedCell));
			flag &= Grid.IsCellOffsetOf(this.reservedCell, target_cell, this.targetOffsets);
		}
		if (flag)
		{
			int num = Grid.PosToCell(this);
			flag = (num == path.nodes[0].cell && this.CurrentNavType == path.nodes[0].navType);
			flag |= (atNextNode = (num == path.nodes[1].cell && this.CurrentNavType == path.nodes[1].navType));
		}
		if (!flag)
		{
			return false;
		}
		PathFinderAbilities currentAbilities = this.GetCurrentAbilities();
		return PathFinder.ValidatePath(this.NavGrid, currentAbilities, ref path);
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x000BB614 File Offset: 0x000B9814
	public void AdvancePath(bool trigger_advance = true)
	{
		int num = Grid.PosToCell(this);
		if (this.target == null)
		{
			base.Trigger(-766531887, null);
			this.Stop(false, true);
		}
		else if (num == this.reservedCell && this.CurrentNavType != NavType.Tube)
		{
			this.Stop(true, true);
		}
		else
		{
			bool flag2;
			bool flag = !this.ValidatePath(ref this.path, out flag2);
			if (flag2)
			{
				this.path.nodes.RemoveAt(0);
			}
			if (flag)
			{
				int root = Grid.PosToCell(this.target);
				int cellPreferences = this.tactic.GetCellPreferences(root, this.targetOffsets, this);
				this.SetReservedCell(cellPreferences);
				if (this.reservedCell == NavigationReservations.InvalidReservation)
				{
					this.Stop(false, true);
				}
				else
				{
					PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(num, this.CurrentNavType, this.flags);
					PathFinder.UpdatePath(this.NavGrid, this.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(this.reservedCell), ref this.path);
				}
			}
			if (this.path.IsValid())
			{
				this.BeginTransition(this.NavGrid.transitions[(int)this.path.nodes[1].transitionId]);
				this.distanceTravelledByNavType[this.CurrentNavType] = Mathf.Max(this.distanceTravelledByNavType[this.CurrentNavType] + 1, this.distanceTravelledByNavType[this.CurrentNavType]);
			}
			else if (this.path.HasArrived())
			{
				this.Stop(true, true);
			}
			else
			{
				this.ClearReservedCell();
				this.Stop(false, true);
			}
		}
		if (trigger_advance)
		{
			base.Trigger(1347184327, null);
		}
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x000BB7B9 File Offset: 0x000B99B9
	public NavGrid.Transition GetNextTransition()
	{
		return this.NavGrid.transitions[(int)this.path.nodes[1].transitionId];
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x000BB7E4 File Offset: 0x000B99E4
	public void Stop(bool arrived_at_destination = false, bool play_idle = true)
	{
		this.target = null;
		this.targetOffsets = null;
		this.path.Clear();
		base.smi.sm.moveTarget.Set(null, base.smi);
		this.transitionDriver.EndTransition();
		if (play_idle)
		{
			HashedString idleAnim = this.NavGrid.GetIdleAnim(this.CurrentNavType);
			this.animController.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
		}
		if (arrived_at_destination)
		{
			base.smi.GoTo(base.smi.sm.normal.arrived);
			return;
		}
		if (base.smi.GetCurrentState() == base.smi.sm.normal.moving)
		{
			this.ClearReservedCell();
			base.smi.GoTo(base.smi.sm.normal.failed);
		}
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x000BB8C9 File Offset: 0x000B9AC9
	private void SimEveryTick(float dt)
	{
		if (this.IsMoving())
		{
			this.transitionDriver.UpdateTransition(dt);
		}
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x000BB8DF File Offset: 0x000B9ADF
	public void Sim4000ms(float dt)
	{
		this.UpdateProbe(true);
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x000BB8E8 File Offset: 0x000B9AE8
	public void UpdateProbe(bool forceUpdate = false)
	{
		if (forceUpdate || !this.executePathProbeTaskAsync)
		{
			this.pathProbeTask.Update();
			this.pathProbeTask.Run(null);
		}
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x000BB90C File Offset: 0x000B9B0C
	public void DrawPath()
	{
		if (base.gameObject.activeInHierarchy && this.IsMoving())
		{
			NavPathDrawer.Instance.DrawPath(this.animController.GetPivotSymbolPosition(), this.path);
		}
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x000BB93E File Offset: 0x000B9B3E
	public void Pause(string reason)
	{
		base.smi.sm.isPaused.Set(true, base.smi, false);
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x000BB95E File Offset: 0x000B9B5E
	public void Unpause(string reason)
	{
		base.smi.sm.isPaused.Set(false, base.smi, false);
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x000BB97E File Offset: 0x000B9B7E
	private void OnDefeated(object data)
	{
		this.ClearReservedCell();
		this.Stop(false, false);
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x000BB98E File Offset: 0x000B9B8E
	private void ClearReservedCell()
	{
		if (this.reservedCell != NavigationReservations.InvalidReservation)
		{
			NavigationReservations.Instance.RemoveOccupancy(this.reservedCell);
			this.reservedCell = NavigationReservations.InvalidReservation;
		}
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x000BB9B8 File Offset: 0x000B9BB8
	private void SetReservedCell(int cell)
	{
		this.ClearReservedCell();
		this.reservedCell = cell;
		NavigationReservations.Instance.AddOccupancy(cell);
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x000BB9D2 File Offset: 0x000B9BD2
	public int GetReservedCell()
	{
		return this.reservedCell;
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x000BB9DA File Offset: 0x000B9BDA
	public int GetAnchorCell()
	{
		return this.AnchorCell;
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000BB9E2 File Offset: 0x000B9BE2
	public bool IsValidNavType(NavType nav_type)
	{
		return this.NavGrid.HasNavTypeData(nav_type);
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x000BB9F0 File Offset: 0x000B9BF0
	public void SetCurrentNavType(NavType nav_type)
	{
		this.CurrentNavType = nav_type;
		this.AnchorCell = NavTypeHelper.GetAnchorCell(nav_type, Grid.PosToCell(this));
		NavGrid.NavTypeData navTypeData = this.NavGrid.GetNavTypeData(this.CurrentNavType);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Vector2 one = Vector2.one;
		if (navTypeData.flipX)
		{
			one.x = -1f;
		}
		if (navTypeData.flipY)
		{
			one.y = -1f;
		}
		component.navMatrix = Matrix2x3.Translate(navTypeData.animControllerOffset * 200f) * Matrix2x3.Rotate(navTypeData.rotation) * Matrix2x3.Scale(one);
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000BBA94 File Offset: 0x000B9C94
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (NavPathDrawer.Instance.GetNavigator() != this) ? new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_navigable_regions", UI.USERMENUACTIONS.DRAWPATHS.NAME_OFF, new System.Action(this.OnDrawPaths), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0.1f);
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", UI.USERMENUACTIONS.FOLLOWCAM.NAME, new System.Action(this.OnFollowCam), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP, true), 0.3f);
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x000BBB97 File Offset: 0x000B9D97
	private void OnFollowCam()
	{
		if (CameraController.Instance.followTarget == base.transform)
		{
			CameraController.Instance.ClearFollowTarget();
			return;
		}
		CameraController.Instance.SetFollowTarget(base.transform);
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x000BBBCB File Offset: 0x000B9DCB
	private void OnDrawPaths()
	{
		if (NavPathDrawer.Instance.GetNavigator() != this)
		{
			NavPathDrawer.Instance.SetNavigator(this);
			return;
		}
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000BBBF5 File Offset: 0x000B9DF5
	private void OnSelectObject(object data)
	{
		NavPathDrawer.Instance.ClearNavigator();
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x000BBC01 File Offset: 0x000B9E01
	public void OnStore(object data)
	{
		if (data is Storage || (data != null && (bool)data))
		{
			this.Stop(false, true);
		}
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000BBC24 File Offset: 0x000B9E24
	public PathFinderAbilities GetCurrentAbilities()
	{
		this.abilities.Refresh();
		return this.abilities;
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x000BBC37 File Offset: 0x000B9E37
	public void SetAbilities(PathFinderAbilities abilities)
	{
		this.abilities = abilities;
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000BBC40 File Offset: 0x000B9E40
	public bool CanReach(IApproachable approachable)
	{
		return this.CanReach(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x000BBC54 File Offset: 0x000B9E54
	public bool CanReach(int cell, CellOffset[] offsets)
	{
		foreach (CellOffset offset in offsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			if (this.CanReach(cell2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000BBC8D File Offset: 0x000B9E8D
	public bool CanReach(int cell)
	{
		return this.GetNavigationCost(cell) != -1;
	}

	// Token: 0x06002198 RID: 8600 RVA: 0x000BBC9C File Offset: 0x000B9E9C
	public int GetNavigationCost(int cell)
	{
		if (Grid.IsValidCell(cell))
		{
			return this.PathProber.GetCost(cell);
		}
		return -1;
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x000BBCB4 File Offset: 0x000B9EB4
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathProber.GetNavigationCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x0600219A RID: 8602 RVA: 0x000BBCC4 File Offset: 0x000B9EC4
	public int GetNavigationCost(int cell, CellOffset[] offsets)
	{
		int num = -1;
		int num2 = offsets.Length;
		for (int i = 0; i < num2; i++)
		{
			int cell2 = Grid.OffsetCell(cell, offsets[i]);
			int navigationCost = this.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x0600219B RID: 8603 RVA: 0x000BBD0C File Offset: 0x000B9F0C
	public int GetNavigationCost(IApproachable approachable)
	{
		return this.GetNavigationCost(approachable.GetCell(), approachable.GetOffsets());
	}

	// Token: 0x0600219C RID: 8604 RVA: 0x000BBD20 File Offset: 0x000B9F20
	public void RunQuery(PathFinderQuery query)
	{
		int cell = Grid.PosToCell(this);
		PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(cell, this.CurrentNavType, this.flags);
		PathFinder.Run(this.NavGrid, this.GetCurrentAbilities(), potential_path, query);
	}

	// Token: 0x0600219D RID: 8605 RVA: 0x000BBD5B File Offset: 0x000B9F5B
	public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags |= new_flags;
	}

	// Token: 0x0600219E RID: 8606 RVA: 0x000BBD6B File Offset: 0x000B9F6B
	public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
	{
		this.flags &= ~new_flags;
	}

	// Token: 0x0600219F RID: 8607 RVA: 0x000BBD7D File Offset: 0x000B9F7D
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void BeginDetailedSample(string region_name)
	{
	}

	// Token: 0x060021A0 RID: 8608 RVA: 0x000BBD7F File Offset: 0x000B9F7F
	[Conditional("ENABLE_DETAILED_NAVIGATOR_PROFILE_INFO")]
	public static void EndDetailedSample(string region_name)
	{
	}

	// Token: 0x040012BD RID: 4797
	public bool DebugDrawPath;

	// Token: 0x040012C1 RID: 4801
	[MyCmpAdd]
	public PathProber PathProber;

	// Token: 0x040012C2 RID: 4802
	[MyCmpAdd]
	public Facing facing;

	// Token: 0x040012C3 RID: 4803
	public float defaultSpeed = 1f;

	// Token: 0x040012C4 RID: 4804
	public TransitionDriver transitionDriver;

	// Token: 0x040012C5 RID: 4805
	public string NavGridName;

	// Token: 0x040012C6 RID: 4806
	public bool updateProber;

	// Token: 0x040012C7 RID: 4807
	public int maxProbingRadius;

	// Token: 0x040012C8 RID: 4808
	public PathFinder.PotentialPath.Flags flags;

	// Token: 0x040012C9 RID: 4809
	private LoggerFSS log;

	// Token: 0x040012CA RID: 4810
	public Dictionary<NavType, int> distanceTravelledByNavType;

	// Token: 0x040012CB RID: 4811
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Move;

	// Token: 0x040012CC RID: 4812
	private PathFinderAbilities abilities;

	// Token: 0x040012CD RID: 4813
	[MyCmpReq]
	public KBatchedAnimController animController;

	// Token: 0x040012CE RID: 4814
	[NonSerialized]
	public PathFinder.Path path;

	// Token: 0x040012CF RID: 4815
	public NavType CurrentNavType;

	// Token: 0x040012D0 RID: 4816
	private int AnchorCell;

	// Token: 0x040012D1 RID: 4817
	private KPrefabID targetLocator;

	// Token: 0x040012D2 RID: 4818
	private int reservedCell = NavigationReservations.InvalidReservation;

	// Token: 0x040012D3 RID: 4819
	private NavTactic tactic;

	// Token: 0x040012D4 RID: 4820
	public Navigator.PathProbeTask pathProbeTask;

	// Token: 0x040012D5 RID: 4821
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x040012D6 RID: 4822
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040012D7 RID: 4823
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnSelectObjectDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnSelectObject(data);
	});

	// Token: 0x040012D8 RID: 4824
	private static readonly EventSystem.IntraObjectHandler<Navigator> OnStoreDelegate = new EventSystem.IntraObjectHandler<Navigator>(delegate(Navigator component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x040012D9 RID: 4825
	public bool executePathProbeTaskAsync;

	// Token: 0x02001382 RID: 4994
	public class ActiveTransition
	{
		// Token: 0x06008765 RID: 34661 RVA: 0x0032B8AC File Offset: 0x00329AAC
		public void Init(NavGrid.Transition transition, float default_speed)
		{
			this.x = transition.x;
			this.y = transition.y;
			this.isLooping = transition.isLooping;
			this.start = transition.start;
			this.end = transition.end;
			this.preAnim = transition.preAnim;
			this.anim = transition.anim;
			this.speed = default_speed;
			this.animSpeed = transition.animSpeed;
			this.navGridTransition = transition;
		}

		// Token: 0x06008766 RID: 34662 RVA: 0x0032B934 File Offset: 0x00329B34
		public void Copy(Navigator.ActiveTransition other)
		{
			this.x = other.x;
			this.y = other.y;
			this.isLooping = other.isLooping;
			this.start = other.start;
			this.end = other.end;
			this.preAnim = other.preAnim;
			this.anim = other.anim;
			this.speed = other.speed;
			this.animSpeed = other.animSpeed;
			this.navGridTransition = other.navGridTransition;
		}

		// Token: 0x040066DD RID: 26333
		public int x;

		// Token: 0x040066DE RID: 26334
		public int y;

		// Token: 0x040066DF RID: 26335
		public bool isLooping;

		// Token: 0x040066E0 RID: 26336
		public NavType start;

		// Token: 0x040066E1 RID: 26337
		public NavType end;

		// Token: 0x040066E2 RID: 26338
		public HashedString preAnim;

		// Token: 0x040066E3 RID: 26339
		public HashedString anim;

		// Token: 0x040066E4 RID: 26340
		public float speed;

		// Token: 0x040066E5 RID: 26341
		public float animSpeed = 1f;

		// Token: 0x040066E6 RID: 26342
		public Func<bool> isCompleteCB;

		// Token: 0x040066E7 RID: 26343
		public NavGrid.Transition navGridTransition;
	}

	// Token: 0x02001383 RID: 4995
	public class StatesInstance : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.GameInstance
	{
		// Token: 0x06008768 RID: 34664 RVA: 0x0032B9CC File Offset: 0x00329BCC
		public StatesInstance(Navigator master) : base(master)
		{
		}
	}

	// Token: 0x02001384 RID: 4996
	public class States : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator>
	{
		// Token: 0x06008769 RID: 34665 RVA: 0x0032B9D8 File Offset: 0x00329BD8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal.stopped;
			this.saveHistory = true;
			this.normal.ParamTransition<bool>(this.isPaused, this.paused, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsTrue).Update("NavigatorProber", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.Sim4000ms(dt);
			}, UpdateRate.SIM_4000ms, false);
			this.normal.moving.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementWakeUp);
			}).Update("UpdateNavigator", delegate(Navigator.StatesInstance smi, float dt)
			{
				smi.master.SimEveryTick(dt);
			}, UpdateRate.SIM_EVERY_TICK, true).Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.Trigger(1027377649, GameHashes.ObjectMovementSleep);
			});
			this.normal.arrived.TriggerOnEnter(GameHashes.DestinationReached, null).GoTo(this.normal.stopped);
			this.normal.failed.TriggerOnEnter(GameHashes.NavigationFailed, null).GoTo(this.normal.stopped);
			this.normal.stopped.Enter(delegate(Navigator.StatesInstance smi)
			{
				smi.master.SubscribeUnstuckFunctions();
			}).DoNothing().Exit(delegate(Navigator.StatesInstance smi)
			{
				smi.master.UnsubscribeUnstuckFunctions();
			});
			this.paused.ParamTransition<bool>(this.isPaused, this.normal, GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.IsFalse);
		}

		// Token: 0x040066E8 RID: 26344
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.TargetParameter moveTarget;

		// Token: 0x040066E9 RID: 26345
		public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter isPaused = new StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.BoolParameter(false);

		// Token: 0x040066EA RID: 26346
		public Navigator.States.NormalStates normal;

		// Token: 0x040066EB RID: 26347
		public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State paused;

		// Token: 0x0200249A RID: 9370
		public class NormalStates : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State
		{
			// Token: 0x0400A247 RID: 41543
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State moving;

			// Token: 0x0400A248 RID: 41544
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State arrived;

			// Token: 0x0400A249 RID: 41545
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State failed;

			// Token: 0x0400A24A RID: 41546
			public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State stopped;
		}
	}

	// Token: 0x02001385 RID: 4997
	public struct PathProbeTask : IWorkItem<object>
	{
		// Token: 0x0600876B RID: 34667 RVA: 0x0032BB98 File Offset: 0x00329D98
		public PathProbeTask(Navigator navigator)
		{
			this.navigator = navigator;
			this.cell = -1;
		}

		// Token: 0x0600876C RID: 34668 RVA: 0x0032BBA8 File Offset: 0x00329DA8
		public void Update()
		{
			this.cell = Grid.PosToCell(this.navigator);
			this.navigator.abilities.Refresh();
		}

		// Token: 0x0600876D RID: 34669 RVA: 0x0032BBCC File Offset: 0x00329DCC
		public void Run(object sharedData)
		{
			this.navigator.PathProber.UpdateProbe(this.navigator.NavGrid, this.cell, this.navigator.CurrentNavType, this.navigator.abilities, this.navigator.flags);
		}

		// Token: 0x040066EC RID: 26348
		private int cell;

		// Token: 0x040066ED RID: 26349
		private Navigator navigator;
	}
}
