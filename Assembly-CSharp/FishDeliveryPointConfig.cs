using System;
using TUNING;
using UnityEngine;

// Token: 0x0200014E RID: 334
public class FishDeliveryPointConfig : IBuildingConfig
{
	// Token: 0x0600068A RID: 1674 RVA: 0x0002C34C File Offset: 0x0002A54C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishDeliveryPoint", 1, 3, "fishrelocator_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Entombable = true;
		buildingDef.Floodable = true;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		return buildingDef;
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x0002C3BC File Offset: 0x0002A5BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CreatureRelocator, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.SWIMMING_CREATURES;
		storage.workAnims = new HashedString[]
		{
			new HashedString("working_pre")
		};
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		storage.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fishrelocator_kanim")
		};
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.faceTargetWhenWorking = false;
		CreatureDeliveryPoint creatureDeliveryPoint = go.AddOrGet<CreatureDeliveryPoint>();
		creatureDeliveryPoint.deliveryOffsets = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		creatureDeliveryPoint.spawnOffset = new CellOffset(0, -1);
		creatureDeliveryPoint.playAnimsOnFetch = true;
		BaggableCritterCapacityTracker baggableCritterCapacityTracker = go.AddOrGet<BaggableCritterCapacityTracker>();
		baggableCritterCapacityTracker.maximumCreatures = 20;
		baggableCritterCapacityTracker.cavityOffset = CellOffset.down;
		go.AddOrGet<TreeFilterable>();
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x0002C4AC File Offset: 0x0002A6AC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
	}

	// Token: 0x040004B0 RID: 1200
	public const string ID = "FishDeliveryPoint";
}
