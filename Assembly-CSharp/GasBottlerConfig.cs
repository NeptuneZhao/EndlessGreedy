using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class GasBottlerConfig : IBuildingConfig
{
	// Token: 0x060009E6 RID: 2534 RVA: 0x0003A770 File Offset: 0x00038970
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("GasBottler", 3, 2, "gas_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasBottler");
		return buildingDef;
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x0003A7F4 File Offset: 0x000389F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(GasBottlerConfig.GasBottlerStoredItemModifiers);
		storage.allowItemRemoval = false;
		go.AddTag(GameTags.GasSource);
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.removeTags = new List<Tag>
		{
			GameTags.GasSource
		};
		dropAllWorkable.resetTargetWorkableOnCompleteWork = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.storage = storage;
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = 200f;
		conduitConsumer.keepZeroMassObject = false;
		Bottler bottler = go.AddOrGet<Bottler>();
		bottler.storage = storage;
		bottler.workTime = 9f;
		bottler.userMaxCapacity = 25f;
		bottler.consumer = conduitConsumer;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x0003A8CA File Offset: 0x00038ACA
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x0400067F RID: 1663
	public const string ID = "GasBottler";

	// Token: 0x04000680 RID: 1664
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000681 RID: 1665
	private const int WIDTH = 3;

	// Token: 0x04000682 RID: 1666
	private const int HEIGHT = 2;

	// Token: 0x04000683 RID: 1667
	private const float DEFAULT_FILL_LEVEL = 25f;

	// Token: 0x04000684 RID: 1668
	private const float CAPACITY = 200f;

	// Token: 0x04000685 RID: 1669
	private static readonly List<Storage.StoredItemModifier> GasBottlerStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide
	};
}
