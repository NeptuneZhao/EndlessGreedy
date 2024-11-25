using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AC0 RID: 2752
public class ClustercraftExteriorDoor : KMonoBehaviour
{
	// Token: 0x0600517E RID: 20862 RVA: 0x001D3DAC File Offset: 0x001D1FAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetWorldId < 0)
		{
			GameObject gameObject = base.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
			WorldContainer worldContainer = ClusterManager.Instance.CreateRocketInteriorWorld(gameObject, this.interiorTemplateName, delegate
			{
				this.PairWithInteriorDoor();
			});
			if (worldContainer != null)
			{
				this.targetWorldId = worldContainer.id;
			}
		}
		else
		{
			this.PairWithInteriorDoor();
		}
		base.Subscribe<ClustercraftExteriorDoor>(-1277991738, ClustercraftExteriorDoor.OnLaunchDelegate);
		base.Subscribe<ClustercraftExteriorDoor>(-887025858, ClustercraftExteriorDoor.OnLandDelegate);
	}

	// Token: 0x0600517F RID: 20863 RVA: 0x001D3E36 File Offset: 0x001D2036
	protected override void OnCleanUp()
	{
		ClusterManager.Instance.DestoryRocketInteriorWorld(this.targetWorldId, this);
		base.OnCleanUp();
	}

	// Token: 0x06005180 RID: 20864 RVA: 0x001D3E50 File Offset: 0x001D2050
	private void PairWithInteriorDoor()
	{
		foreach (object obj in Components.ClusterCraftInteriorDoors)
		{
			ClustercraftInteriorDoor clustercraftInteriorDoor = (ClustercraftInteriorDoor)obj;
			if (clustercraftInteriorDoor.GetMyWorldId() == this.targetWorldId)
			{
				this.SetTarget(clustercraftInteriorDoor);
				break;
			}
		}
		if (this.targetDoor == null)
		{
			global::Debug.LogWarning("No ClusterCraftInteriorDoor found on world");
		}
		WorldContainer targetWorld = this.GetTargetWorld();
		int myWorldId = this.GetMyWorldId();
		if (targetWorld != null && myWorldId != -1)
		{
			targetWorld.SetParentIdx(myWorldId);
		}
		if (base.gameObject.GetComponent<KSelectable>().IsSelected)
		{
			RocketModuleSideScreen.instance.UpdateButtonStates();
		}
		base.Trigger(-1118736034, null);
		targetWorld.gameObject.Trigger(-1118736034, null);
	}

	// Token: 0x06005181 RID: 20865 RVA: 0x001D3F30 File Offset: 0x001D2130
	public void SetTarget(ClustercraftInteriorDoor target)
	{
		this.targetDoor = target;
		target.GetComponent<AssignmentGroupController>().SetGroupID(base.GetComponent<AssignmentGroupController>().AssignmentGroupID);
		base.GetComponent<NavTeleporter>().TwoWayTarget(target.GetComponent<NavTeleporter>());
	}

	// Token: 0x06005182 RID: 20866 RVA: 0x001D3F60 File Offset: 0x001D2160
	public bool HasTargetWorld()
	{
		return this.targetDoor != null;
	}

	// Token: 0x06005183 RID: 20867 RVA: 0x001D3F6E File Offset: 0x001D216E
	public WorldContainer GetTargetWorld()
	{
		global::Debug.Assert(this.targetDoor != null, "Clustercraft Exterior Door has no targetDoor");
		return this.targetDoor.GetMyWorld();
	}

	// Token: 0x06005184 RID: 20868 RVA: 0x001D3F94 File Offset: 0x001D2194
	public void FerryMinion(GameObject minion)
	{
		Vector3 b = Vector3.left * 3f;
		minion.transform.SetPosition(Grid.CellToPos(Grid.PosToCell(this.targetDoor.transform.position + b), CellAlignment.Bottom, Grid.SceneLayer.Move));
		ClusterManager.Instance.MigrateMinion(minion.GetComponent<MinionIdentity>(), this.targetDoor.GetMyWorldId());
	}

	// Token: 0x06005185 RID: 20869 RVA: 0x001D3FFC File Offset: 0x001D21FC
	private void OnLaunch(object data)
	{
		NavTeleporter component = base.GetComponent<NavTeleporter>();
		component.EnableTwoWayTarget(false);
		component.Deregister();
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			targetWorld.SetParentIdx(targetWorld.id);
		}
	}

	// Token: 0x06005186 RID: 20870 RVA: 0x001D4038 File Offset: 0x001D2238
	private void OnLand(object data)
	{
		base.GetComponent<NavTeleporter>().EnableTwoWayTarget(true);
		WorldContainer targetWorld = this.GetTargetWorld();
		if (targetWorld != null)
		{
			int myWorldId = this.GetMyWorldId();
			targetWorld.SetParentIdx(myWorldId);
		}
	}

	// Token: 0x06005187 RID: 20871 RVA: 0x001D406F File Offset: 0x001D226F
	public int TargetCell()
	{
		return this.targetDoor.GetComponent<NavTeleporter>().GetCell();
	}

	// Token: 0x06005188 RID: 20872 RVA: 0x001D4081 File Offset: 0x001D2281
	public ClustercraftInteriorDoor GetInteriorDoor()
	{
		return this.targetDoor;
	}

	// Token: 0x040035FD RID: 13821
	public string interiorTemplateName;

	// Token: 0x040035FE RID: 13822
	private ClustercraftInteriorDoor targetDoor;

	// Token: 0x040035FF RID: 13823
	[Serialize]
	private int targetWorldId = -1;

	// Token: 0x04003600 RID: 13824
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04003601 RID: 13825
	private static readonly EventSystem.IntraObjectHandler<ClustercraftExteriorDoor> OnLandDelegate = new EventSystem.IntraObjectHandler<ClustercraftExteriorDoor>(delegate(ClustercraftExteriorDoor component, object data)
	{
		component.OnLand(data);
	});
}
