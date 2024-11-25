using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class GasReservoirConfig : IBuildingConfig
{
	// Token: 0x06000A30 RID: 2608 RVA: 0x0003BE4C File Offset: 0x0003A04C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GasReservoir", 5, 3, "gasstorage_kanim", 100, 120f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(1, 2);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(SmartReservoir.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SMARTRESERVOIR.LOGIC_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasReservoir");
		return buildingDef;
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0003BF28 File Offset: 0x0003A128
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Reservoir>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.capacityKg = 1000f;
		storage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<SmartReservoir>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.elementFilter = null;
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x0003BFC1 File Offset: 0x0003A1C1
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x0400069F RID: 1695
	public const string ID = "GasReservoir";

	// Token: 0x040006A0 RID: 1696
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x040006A1 RID: 1697
	private const int WIDTH = 5;

	// Token: 0x040006A2 RID: 1698
	private const int HEIGHT = 3;

	// Token: 0x040006A3 RID: 1699
	public static readonly List<Storage.StoredItemModifier> ReservoirStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};
}
