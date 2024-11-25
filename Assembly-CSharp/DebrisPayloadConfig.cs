using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class DebrisPayloadConfig : IEntityConfig
{
	// Token: 0x060001E2 RID: 482 RVA: 0x0000D6C7 File Offset: 0x0000B8C7
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000D6D0 File Offset: 0x0000B8D0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("DebrisPayload", ITEMS.DEBRISPAYLOAD.NAME, ITEMS.DEBRISPAYLOAD.DESC, 100f, true, Assets.GetAnim("rocket_debris_combined_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental
		});
		RailGunPayload.Def def = gameObject.AddOrGetDef<RailGunPayload.Def>();
		def.attractToBeacons = false;
		def.clusterAnimSymbolSwapTarget = "debris1";
		def.randomClusterSymbolSwaps = new List<string>
		{
			"debris1",
			"debris2",
			"debris3"
		};
		def.worldAnimSymbolSwapTarget = "debris";
		def.randomWorldSymbolSwaps = new List<string>
		{
			"debris",
			"2_debris",
			"3_debris"
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(gameObject, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		storage.capacityKg = 5000f;
		DropAllWorkable dropAllWorkable = gameObject.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 30f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "rocket_debris_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.DEBRISPAYLOAD.NAME");
		gameObject.AddOrGet<ClusterTraveler>();
		return gameObject;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x0000D890 File Offset: 0x0000BA90
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000D892 File Offset: 0x0000BA92
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000134 RID: 308
	public const string ID = "DebrisPayload";

	// Token: 0x04000135 RID: 309
	public const float MASS = 100f;

	// Token: 0x04000136 RID: 310
	public const float MAX_STORAGE_KG_MASS = 5000f;

	// Token: 0x04000137 RID: 311
	public const float STARMAP_SPEED = 10f;
}
