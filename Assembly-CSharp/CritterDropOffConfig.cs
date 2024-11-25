using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000061 RID: 97
public class CritterDropOffConfig : IBuildingConfig
{
	// Token: 0x060001CA RID: 458 RVA: 0x0000CEEC File Offset: 0x0000B0EC
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CritterDropOff", 1, 3, "relocator_dropoff_02_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CritterDropOffInput", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.CRITTERDROPOFF.LOGIC_INPUT.DESC, STRINGS.BUILDINGS.PREFABS.CRITTERDROPOFF.LOGIC_INPUT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.CRITTERDROPOFF.LOGIC_INPUT.LOGIC_PORT_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000CF90 File Offset: 0x0000B190
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
		go.AddOrGet<BaggableCritterCapacityTracker>().filteredCount = true;
		go.AddOrGet<TreeFilterable>();
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000D050 File Offset: 0x0000B250
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400011C RID: 284
	public const string ID = "CritterDropOff";

	// Token: 0x0400011D RID: 285
	public const string INPUT_PORT = "CritterDropOffInput";
}
