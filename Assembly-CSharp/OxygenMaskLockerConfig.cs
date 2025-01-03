﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000321 RID: 801
public class OxygenMaskLockerConfig : IBuildingConfig
{
	// Token: 0x060010C4 RID: 4292 RVA: 0x0005E7B0 File Offset: 0x0005C9B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxygenMaskLocker";
		int width = 1;
		int height = 2;
		string anim = "oxygen_mask_locker_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = raw_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "OxygenMaskLocker");
		return buildingDef;
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x0005E824 File Offset: 0x0005CA24
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<SuitLocker>().OutfitTags = new Tag[]
		{
			GameTags.OxygenMask
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.capacityKG = 30f;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("OxygenMaskLocker"),
			new Tag("OxygenMaskMarker")
		};
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x0005E8D3 File Offset: 0x0005CAD3
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x04000A40 RID: 2624
	public const string ID = "OxygenMaskLocker";
}
