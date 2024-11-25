using System;
using UnityEngine;

// Token: 0x020002AF RID: 687
public class ClustercraftConfig : IEntityConfig
{
	// Token: 0x06000E53 RID: 3667 RVA: 0x00054B0D File Offset: 0x00052D0D
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000E54 RID: 3668 RVA: 0x00054B14 File Offset: 0x00052D14
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Clustercraft", "Clustercraft", true);
		SaveLoadRoot saveLoadRoot = gameObject.AddOrGet<SaveLoadRoot>();
		saveLoadRoot.DeclareOptionalComponent<WorldInventory>();
		saveLoadRoot.DeclareOptionalComponent<WorldContainer>();
		saveLoadRoot.DeclareOptionalComponent<OrbitalMechanics>();
		gameObject.AddOrGet<Clustercraft>();
		gameObject.AddOrGet<CraftModuleInterface>();
		gameObject.AddOrGet<UserNameable>();
		RocketClusterDestinationSelector rocketClusterDestinationSelector = gameObject.AddOrGet<RocketClusterDestinationSelector>();
		rocketClusterDestinationSelector.requireLaunchPadOnAsteroidDestination = true;
		rocketClusterDestinationSelector.assignable = true;
		rocketClusterDestinationSelector.shouldPointTowardsPath = true;
		gameObject.AddOrGet<ClusterTraveler>().stopAndNotifyWhenPathChanges = true;
		gameObject.AddOrGetDef<AlertStateManager.Def>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGetDef<RocketSelfDestructMonitor.Def>();
		return gameObject;
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x00054B98 File Offset: 0x00052D98
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x00054B9A File Offset: 0x00052D9A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008FE RID: 2302
	public const string ID = "Clustercraft";
}
