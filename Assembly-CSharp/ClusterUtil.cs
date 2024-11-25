using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public static class ClusterUtil
{
	// Token: 0x06003695 RID: 13973 RVA: 0x001296C1 File Offset: 0x001278C1
	public static WorldContainer GetMyWorld(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorld();
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x001296CE File Offset: 0x001278CE
	public static WorldContainer GetMyWorld(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorld();
	}

	// Token: 0x06003697 RID: 13975 RVA: 0x001296DC File Offset: 0x001278DC
	public static WorldContainer GetMyWorld(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
		}
		return null;
	}

	// Token: 0x06003698 RID: 13976 RVA: 0x00129719 File Offset: 0x00127919
	public static int GetMyWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldId();
	}

	// Token: 0x06003699 RID: 13977 RVA: 0x00129726 File Offset: 0x00127926
	public static int GetMyWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldId();
	}

	// Token: 0x0600369A RID: 13978 RVA: 0x00129734 File Offset: 0x00127934
	public static int GetMyWorldId(this GameObject gameObject)
	{
		int num = Grid.PosToCell(gameObject);
		if (Grid.IsValidCell(num) && Grid.WorldIdx[num] != 255)
		{
			return (int)Grid.WorldIdx[num];
		}
		return -1;
	}

	// Token: 0x0600369B RID: 13979 RVA: 0x00129767 File Offset: 0x00127967
	public static int GetMyParentWorldId(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyParentWorldId();
	}

	// Token: 0x0600369C RID: 13980 RVA: 0x00129774 File Offset: 0x00127974
	public static int GetMyParentWorldId(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyParentWorldId();
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x00129784 File Offset: 0x00127984
	public static int GetMyParentWorldId(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld == null)
		{
			return gameObject.GetMyWorldId();
		}
		return myWorld.ParentWorldId;
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x001297AE File Offset: 0x001279AE
	public static AxialI GetMyWorldLocation(this StateMachine.Instance smi)
	{
		return smi.GetComponent<StateMachineController>().GetMyWorldLocation();
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x001297BB File Offset: 0x001279BB
	public static AxialI GetMyWorldLocation(this KMonoBehaviour component)
	{
		return component.gameObject.GetMyWorldLocation();
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x001297C8 File Offset: 0x001279C8
	public static AxialI GetMyWorldLocation(this GameObject gameObject)
	{
		ClusterGridEntity component = gameObject.GetComponent<ClusterGridEntity>();
		if (component != null)
		{
			return component.Location;
		}
		WorldContainer myWorld = gameObject.GetMyWorld();
		DebugUtil.DevAssertArgs(myWorld != null, new object[]
		{
			"GetMyWorldLocation called on object with no world",
			gameObject
		});
		return myWorld.GetComponent<ClusterGridEntity>().Location;
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x0012981C File Offset: 0x00127A1C
	public static bool IsMyWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyWorld(otherCell);
	}

	// Token: 0x060036A2 RID: 13986 RVA: 0x00129838 File Offset: 0x00127A38
	public static bool IsMyWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		return Grid.IsValidCell(num) && Grid.IsValidCell(otherCell) && Grid.WorldIdx[num] == Grid.WorldIdx[otherCell];
	}

	// Token: 0x060036A3 RID: 13987 RVA: 0x00129870 File Offset: 0x00127A70
	public static bool IsMyParentWorld(this GameObject go, GameObject otherGo)
	{
		int otherCell = Grid.PosToCell(otherGo);
		return go.IsMyParentWorld(otherCell);
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x0012988C File Offset: 0x00127A8C
	public static bool IsMyParentWorld(this GameObject go, int otherCell)
	{
		int num = Grid.PosToCell(go);
		if (Grid.IsValidCell(num) && Grid.IsValidCell(otherCell))
		{
			if (Grid.WorldIdx[num] == Grid.WorldIdx[otherCell])
			{
				return true;
			}
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			WorldContainer world2 = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[otherCell]);
			if (world == null)
			{
				DebugUtil.DevLogError(string.Format("{0} at {1} has a valid cell but no world", go, num));
			}
			if (world2 == null)
			{
				DebugUtil.DevLogError(string.Format("{0} is a valid cell but no world", otherCell));
			}
			if (world != null && world2 != null && world.ParentWorldId == world2.ParentWorldId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x0012994C File Offset: 0x00127B4C
	public static int GetAsteroidWorldIdAtLocation(AxialI location)
	{
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.cellContents[location])
		{
			if (clusterGridEntity.Layer == EntityLayer.Asteroid)
			{
				WorldContainer component = clusterGridEntity.GetComponent<WorldContainer>();
				if (component != null)
				{
					return component.id;
				}
			}
		}
		return -1;
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x001299C8 File Offset: 0x00127BC8
	public static bool ActiveWorldIsRocketInterior()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior;
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x001299D9 File Offset: 0x00127BD9
	public static bool ActiveWorldHasPrinter()
	{
		return ClusterManager.Instance.activeWorld.IsModuleInterior || Components.Telepads.GetWorldItems(ClusterManager.Instance.activeWorldId, false).Count > 0;
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x00129A0C File Offset: 0x00127C0C
	public static float GetAmountFromRelatedWorlds(WorldInventory worldInventory, Tag element)
	{
		WorldContainer worldContainer = worldInventory.WorldContainer;
		float num = 0f;
		int parentWorldId = worldContainer.ParentWorldId;
		foreach (WorldContainer worldContainer2 in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer2.ParentWorldId == parentWorldId)
			{
				num += worldContainer2.worldInventory.GetAmount(element, false);
			}
		}
		return num;
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x00129A84 File Offset: 0x00127C84
	public static List<Pickupable> GetPickupablesFromRelatedWorlds(WorldInventory worldInventory, Tag tag)
	{
		List<Pickupable> list = new List<Pickupable>();
		int parentWorldId = worldInventory.GetComponent<WorldContainer>().ParentWorldId;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.ParentWorldId == parentWorldId)
			{
				ICollection<Pickupable> pickupables = worldContainer.worldInventory.GetPickupables(tag, false);
				if (pickupables != null)
				{
					list.AddRange(pickupables);
				}
			}
		}
		return list;
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x00129B08 File Offset: 0x00127D08
	public static string DebugGetMyWorldName(this GameObject gameObject)
	{
		WorldContainer myWorld = gameObject.GetMyWorld();
		if (myWorld != null)
		{
			return myWorld.worldName;
		}
		return string.Format("InvalidWorld(pos={0})", gameObject.transform.GetPosition());
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x00129B48 File Offset: 0x00127D48
	public static ClusterGridEntity ClosestVisibleAsteroidToLocation(AxialI location)
	{
		foreach (AxialI cell in AxialUtil.SpiralOut(location, ClusterGrid.Instance.numRings))
		{
			if (ClusterGrid.Instance.IsValidCell(cell) && ClusterGrid.Instance.IsCellVisible(cell))
			{
				ClusterGridEntity asteroidAtCell = ClusterGrid.Instance.GetAsteroidAtCell(cell);
				if (asteroidAtCell != null)
				{
					return asteroidAtCell;
				}
			}
		}
		return null;
	}
}
