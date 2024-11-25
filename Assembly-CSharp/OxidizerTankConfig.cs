﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031E RID: 798
public class OxidizerTankConfig : IBuildingConfig
{
	// Token: 0x060010B5 RID: 4277 RVA: 0x0005E0FD File Offset: 0x0005C2FD
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0005E104 File Offset: 0x0005C304
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxidizerTank";
		int width = 5;
		int height = 5;
		string anim = "rocket_oxidizer_tank_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] fuel_TANK_DRY_MASS = TUNING.BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_DRY_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, fuel_TANK_DRY_MASS, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "grounded";
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0005E1C0 File Offset: 0x0005C3C0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Prioritizable.AddRef(go);
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0005E22C File Offset: 0x0005C42C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 2700f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		FlatTagFilterable flatTagFilterable = go.AddOrGet<FlatTagFilterable>();
		flatTagFilterable.tagOptions = new List<Tag>
		{
			SimHashes.OxyRock.CreateTag()
		};
		flatTagFilterable.headerText = STRINGS.BUILDINGS.PREFABS.OXIDIZERTANK.UI_FILTER_CATEGORY;
		flatTagFilterable.selectedTags.Add(SimHashes.OxyRock.CreateTag());
		OxidizerTank oxidizerTank = go.AddOrGet<OxidizerTank>();
		oxidizerTank.consumeOnLand = !DlcManager.FeatureClusterSpaceEnabled();
		oxidizerTank.storage = storage;
		oxidizerTank.supportsMultipleOxidizers = true;
		oxidizerTank.maxFillMass = 2700f;
		oxidizerTank.targetFillMass = 2700f;
		oxidizerTank.discoverResourcesOnSpawn = new List<SimHashes>
		{
			SimHashes.OxyRock
		};
		go.AddOrGet<CopyBuildingSettings>();
		go.AddOrGet<DropToUserCapacity>();
		go.AddOrGet<Prioritizable>();
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_oxidizer_tank_bg_kanim", false);
	}

	// Token: 0x04000A3A RID: 2618
	public const string ID = "OxidizerTank";

	// Token: 0x04000A3B RID: 2619
	public const float FuelCapacity = 2700f;
}
