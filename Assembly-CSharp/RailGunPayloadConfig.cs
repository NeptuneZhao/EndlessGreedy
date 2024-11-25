using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000376 RID: 886
public class RailGunPayloadConfig : IEntityConfig
{
	// Token: 0x0600125D RID: 4701 RVA: 0x00064ADC File Offset: 0x00062CDC
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x00064AE4 File Offset: 0x00062CE4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("RailGunPayload", ITEMS.RAILGUNPAYLOAD.NAME, ITEMS.RAILGUNPAYLOAD.DESC, 200f, true, Assets.GetAnim("railgun_capsule_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 1f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental
		});
		gameObject.AddOrGetDef<RailGunPayload.Def>().attractToBeacons = true;
		gameObject.AddComponent<LoopingSounds>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(gameObject, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		storage.capacityKg = 200f;
		DropAllWorkable dropAllWorkable = gameObject.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 30f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "payload01_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.RAILGUNPAYLOAD.NAME");
		gameObject.AddOrGet<ClusterTraveler>();
		return gameObject;
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x00064C28 File Offset: 0x00062E28
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x00064C2A File Offset: 0x00062E2A
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AA8 RID: 2728
	public const string ID = "RailGunPayload";

	// Token: 0x04000AA9 RID: 2729
	public const float MASS = 200f;

	// Token: 0x04000AAA RID: 2730
	public const int LANDING_EDGE_PADDING = 3;
}
