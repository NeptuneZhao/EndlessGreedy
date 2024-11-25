using System;
using TUNING;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class CreatureDeliveryPointConfig : IBuildingConfig
{
	// Token: 0x060001B1 RID: 433 RVA: 0x0000C7BC File Offset: 0x0000A9BC
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CreatureDeliveryPoint", 1, 3, "relocator_dropoff_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000C820 File Offset: 0x0000AA20
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CreatureRelocator, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = false;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.BAGABLE_CREATURES;
		storage.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		storage.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_restrain_creature_kanim")
		};
		storage.workAnimPlayMode = KAnim.PlayMode.Once;
		storage.synchronizeAnims = false;
		storage.useGunForDelivery = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		go.AddOrGet<CreatureDeliveryPoint>();
		go.AddOrGet<BaggableCritterCapacityTracker>().maximumCreatures = 20;
		go.AddOrGet<FixedCapturePoint.AutoWrangleCapture>();
		go.AddOrGet<TreeFilterable>();
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0000C8E8 File Offset: 0x0000AAE8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<FixedCapturePoint.Def>().isAmountStoredOverCapacity = delegate(FixedCapturePoint.Instance smi, FixedCapturableMonitor.Instance capturable)
		{
			TreeFilterable component = smi.GetComponent<TreeFilterable>();
			IUserControlledCapacity component2 = smi.GetComponent<IUserControlledCapacity>();
			float amountStored = component2.AmountStored;
			float userMaxCapacity = component2.UserMaxCapacity;
			return !component.ContainsTag(capturable.PrefabTag) || amountStored > userMaxCapacity;
		};
	}

	// Token: 0x04000115 RID: 277
	public const string ID = "CreatureDeliveryPoint";
}
