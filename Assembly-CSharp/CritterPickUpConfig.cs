using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class CritterPickUpConfig : IBuildingConfig
{
	// Token: 0x060001CE RID: 462 RVA: 0x0000D05C File Offset: 0x0000B25C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("CritterPickUp", 1, 3, "relocator_pickup_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CritterPickUpInput", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.DESC, STRINGS.BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.CRITTERPICKUP.LOGIC_INPUT.LOGIC_PORT_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000D100 File Offset: 0x0000B300
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
		go.AddOrGet<TreeFilterable>();
		go.AddOrGet<BaggableCritterCapacityTracker>().filteredCount = true;
		Prioritizable.AddRef(go);
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000D1BF File Offset: 0x0000B3BF
	public override void DoPostConfigureComplete(GameObject go)
	{
		FixedCapturePoint.Def def = go.AddOrGetDef<FixedCapturePoint.Def>();
		def.isAmountStoredOverCapacity = delegate(FixedCapturePoint.Instance smi, FixedCapturableMonitor.Instance capturable)
		{
			TreeFilterable component = smi.GetComponent<TreeFilterable>();
			IUserControlledCapacity component2 = smi.GetComponent<IUserControlledCapacity>();
			float amountStored = component2.AmountStored;
			float userMaxCapacity = component2.UserMaxCapacity;
			return amountStored > userMaxCapacity && component.ContainsTag(capturable.PrefabTag);
		};
		def.allowBabies = true;
	}

	// Token: 0x0400011E RID: 286
	public const string ID = "CritterPickUp";

	// Token: 0x0400011F RID: 287
	public const string INPUT_PORT = "CritterPickUpInput";
}
