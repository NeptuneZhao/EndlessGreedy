using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200023F RID: 575
public class LiquidBottlerConfig : IBuildingConfig
{
	// Token: 0x06000BEA RID: 3050 RVA: 0x00046120 File Offset: 0x00044320
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LiquidBottler", 3, 2, "liquid_bottler_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.Floodable = false;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidBottler");
		return buildingDef;
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x000461A4 File Offset: 0x000443A4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.capacityKg = 200f;
		storage.SetDefaultStoredItemModifiers(LiquidBottlerConfig.LiquidBottlerStoredItemModifiers);
		storage.allowItemRemoval = false;
		go.AddTag(GameTags.LiquidSource);
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.removeTags = new List<Tag>
		{
			GameTags.LiquidSource
		};
		dropAllWorkable.resetTargetWorkableOnCompleteWork = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.storage = storage;
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = 200f;
		conduitConsumer.keepZeroMassObject = false;
		Bottler bottler = go.AddOrGet<Bottler>();
		bottler.storage = storage;
		bottler.workTime = 9f;
		bottler.consumer = conduitConsumer;
		bottler.userMaxCapacity = 200f;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0004627A File Offset: 0x0004447A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040007C2 RID: 1986
	public const string ID = "LiquidBottler";

	// Token: 0x040007C3 RID: 1987
	private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x040007C4 RID: 1988
	private const int WIDTH = 3;

	// Token: 0x040007C5 RID: 1989
	private const int HEIGHT = 2;

	// Token: 0x040007C6 RID: 1990
	private const float CAPACITY = 200f;

	// Token: 0x040007C7 RID: 1991
	private static readonly List<Storage.StoredItemModifier> LiquidBottlerStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};
}
