using System;
using System.Collections.Generic;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x02000AD6 RID: 2774
public class PassengerRocketModule : KMonoBehaviour
{
	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x0600526D RID: 21101 RVA: 0x001D8B5B File Offset: 0x001D6D5B
	public PassengerRocketModule.RequestCrewState PassengersRequested
	{
		get
		{
			return this.passengersRequested;
		}
	}

	// Token: 0x0600526E RID: 21102 RVA: 0x001D8B64 File Offset: 0x001D6D64
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		GameUtil.SubscribeToTags<PassengerRocketModule>(this, PassengerRocketModule.OnRocketOnGroundTagDelegate, false);
		base.Subscribe<PassengerRocketModule>(-1547247383, PassengerRocketModule.OnClustercraftStateChanged);
		base.Subscribe<PassengerRocketModule>(1655598572, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(191901966, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-71801987, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-1277991738, PassengerRocketModule.OnLaunchDelegate);
		base.Subscribe<PassengerRocketModule>(-1432940121, PassengerRocketModule.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(base.GetComponent<Workable>()).StartSM();
	}

	// Token: 0x0600526F RID: 21103 RVA: 0x001D8C15 File Offset: 0x001D6E15
	protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		base.OnCleanUp();
	}

	// Token: 0x06005270 RID: 21104 RVA: 0x001D8C38 File Offset: 0x001D6E38
	private void OnAssignmentGroupChanged(object data)
	{
		this.RefreshOrders();
	}

	// Token: 0x06005271 RID: 21105 RVA: 0x001D8C40 File Offset: 0x001D6E40
	private void RefreshClusterStateForAudio()
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			if (activeWorld != null && activeWorld.IsModuleInterior)
			{
				UnityEngine.Object craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
				Clustercraft component = activeWorld.GetComponent<Clustercraft>();
				if (craftInterface == component.ModuleInterface)
				{
					ClusterManager.Instance.UpdateRocketInteriorAudio();
				}
			}
		}
	}

	// Token: 0x06005272 RID: 21106 RVA: 0x001D8CA0 File Offset: 0x001D6EA0
	private void OnReachableChanged(object data)
	{
		bool flag = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, this);
	}

	// Token: 0x06005273 RID: 21107 RVA: 0x001D8CEB File Offset: 0x001D6EEB
	public void RequestCrewBoard(PassengerRocketModule.RequestCrewState requestBoard)
	{
		this.passengersRequested = requestBoard;
		this.RefreshOrders();
	}

	// Token: 0x06005274 RID: 21108 RVA: 0x001D8CFC File Offset: 0x001D6EFC
	public bool ShouldCrewGetIn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		return this.passengersRequested == PassengerRocketModule.RequestCrewState.Request || (craftInterface.IsLaunchRequested() && craftInterface.CheckPreppedForLaunch());
	}

	// Token: 0x06005275 RID: 21109 RVA: 0x001D8D30 File Offset: 0x001D6F30
	private void RefreshOrders()
	{
		if (!this.HasTag(GameTags.RocketOnGround) || !base.GetComponent<ClustercraftExteriorDoor>().HasTargetWorld())
		{
			return;
		}
		int cell = base.GetComponent<NavTeleporter>().GetCell();
		int num = base.GetComponent<ClustercraftExteriorDoor>().TargetCell();
		bool flag = this.ShouldCrewGetIn();
		if (flag)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity minionIdentity = enumerator.Current;
					bool flag2 = Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minionIdentity.assignableProxy.Get());
					bool flag3 = minionIdentity.GetMyWorldId() == (int)Grid.WorldIdx[num];
					if (!flag3 && flag2)
					{
						minionIdentity.GetSMI<RocketPassengerMonitor.Instance>().SetMoveTarget(num);
					}
					else if (flag3 && !flag2)
					{
						minionIdentity.GetSMI<RocketPassengerMonitor.Instance>().SetMoveTarget(cell);
					}
					else
					{
						minionIdentity.GetSMI<RocketPassengerMonitor.Instance>().ClearMoveTarget(num);
					}
				}
				goto IL_148;
			}
		}
		foreach (MinionIdentity cmp in Components.LiveMinionIdentities.Items)
		{
			RocketPassengerMonitor.Instance smi = cmp.GetSMI<RocketPassengerMonitor.Instance>();
			if (smi != null)
			{
				smi.ClearMoveTarget(cell);
				smi.ClearMoveTarget(num);
			}
		}
		IL_148:
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			this.RefreshAccessStatus(Components.LiveMinionIdentities[i], flag);
		}
	}

	// Token: 0x06005276 RID: 21110 RVA: 0x001D8ED0 File Offset: 0x001D70D0
	private void RefreshAccessStatus(MinionIdentity minion, bool restrict)
	{
		Component interiorDoor = base.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		AccessControl component = base.GetComponent<AccessControl>();
		AccessControl component2 = interiorDoor.GetComponent<AccessControl>();
		if (!restrict)
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			return;
		}
		if (Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minion.assignableProxy.Get()))
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
			return;
		}
		component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
		component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
	}

	// Token: 0x06005277 RID: 21111 RVA: 0x001D8F98 File Offset: 0x001D7198
	public bool CheckPilotBoarded()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return false;
		}
		List<IAssignableIdentity> list = new List<IAssignableIdentity>();
		foreach (IAssignableIdentity assignableIdentity in members)
		{
			MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
			if (minionAssignablesProxy != null)
			{
				MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
				if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
				{
					list.Add(assignableIdentity);
				}
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		using (List<IAssignableIdentity>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator2.Current).GetTargetGameObject().GetMyWorldId() == (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005278 RID: 21112 RVA: 0x001D90AC File Offset: 0x001D72AC
	public global::Tuple<int, int> GetCrewBoardedFraction()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return new global::Tuple<int, int>(0, 0);
		}
		int num = 0;
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					num++;
				}
			}
		}
		return new global::Tuple<int, int>(members.Count - num, members.Count);
	}

	// Token: 0x06005279 RID: 21113 RVA: 0x001D9144 File Offset: 0x001D7344
	public bool HasCrewAssigned()
	{
		return ((ICollection<IAssignableIdentity>)base.GetComponent<AssignmentGroupController>().GetMembers()).Count > 0;
	}

	// Token: 0x0600527A RID: 21114 RVA: 0x001D915C File Offset: 0x001D735C
	public bool CheckPassengersBoarded(bool require_pilot = true)
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return false;
		}
		if (require_pilot)
		{
			bool flag = false;
			foreach (IAssignableIdentity assignableIdentity in members)
			{
				MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
				if (minionAssignablesProxy != null)
				{
					MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
					if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600527B RID: 21115 RVA: 0x001D9258 File Offset: 0x001D7458
	public bool CheckExtraPassengers()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			byte worldId = Grid.WorldIdx[component.TargetCell()];
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems((int)worldId, false);
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			for (int i = 0; i < worldItems.Count; i++)
			{
				if (!Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(worldItems[i].assignableProxy.Get()))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600527C RID: 21116 RVA: 0x001D92E0 File Offset: 0x001D74E0
	public void RemoveRocketPassenger(MinionIdentity minion)
	{
		if (minion != null)
		{
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			MinionAssignablesProxy member = minion.assignableProxy.Get();
			if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member))
			{
				Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
			}
			this.RefreshOrders();
		}
	}

	// Token: 0x0600527D RID: 21117 RVA: 0x001D934C File Offset: 0x001D754C
	public void RemovePassengersOnOtherWorlds()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			int myWorldId = component.GetMyWorldId();
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				MinionAssignablesProxy member = minionIdentity.assignableProxy.Get();
				if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member) && minionIdentity.GetMyParentWorldId() != myWorldId)
				{
					Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
				}
			}
		}
	}

	// Token: 0x0600527E RID: 21118 RVA: 0x001D9414 File Offset: 0x001D7614
	public void ClearMinionAssignments(object data)
	{
		string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
		foreach (IAssignableIdentity minionIdentity in Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].GetMembers())
		{
			Game.Instance.assignmentManager.RemoveFromWorld(minionIdentity, this.GetMyWorldId());
		}
	}

	// Token: 0x0400365D RID: 13917
	public EventReference interiorReverbSnapshot;

	// Token: 0x0400365E RID: 13918
	[Serialize]
	private PassengerRocketModule.RequestCrewState passengersRequested;

	// Token: 0x0400365F RID: 13919
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<PassengerRocketModule>(GameTags.RocketOnGround, delegate(PassengerRocketModule component, object data)
	{
		component.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
	});

	// Token: 0x04003660 RID: 13920
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnClustercraftStateChanged = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshClusterStateForAudio();
	});

	// Token: 0x04003661 RID: 13921
	private static EventSystem.IntraObjectHandler<PassengerRocketModule> RefreshDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshOrders();
		cmp.RefreshClusterStateForAudio();
	});

	// Token: 0x04003662 RID: 13922
	private static EventSystem.IntraObjectHandler<PassengerRocketModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.ClearMinionAssignments(data);
	});

	// Token: 0x04003663 RID: 13923
	private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x02001B25 RID: 6949
	public enum RequestCrewState
	{
		// Token: 0x04007EED RID: 32493
		Release,
		// Token: 0x04007EEE RID: 32494
		Request
	}
}
