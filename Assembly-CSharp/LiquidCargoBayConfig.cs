﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000241 RID: 577
public class LiquidCargoBayConfig : IBuildingConfig
{
	// Token: 0x06000BF4 RID: 3060 RVA: 0x0004640E File Offset: 0x0004460E
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00046418 File Offset: 0x00044618
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LiquidCargoBay";
		int width = 5;
		int height = 5;
		string anim = "rocket_storage_liquid_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] cargo_MASS = BUILDINGS.ROCKETRY_MASS_KG.CARGO_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, cargo_MASS, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 3);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		return buildingDef;
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x000464DC File Offset: 0x000446DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00046540 File Offset: 0x00044740
	public override void DoPostConfigureComplete(GameObject go)
	{
		CargoBay cargoBay = go.AddOrGet<CargoBay>();
		cargoBay.storage = go.AddOrGet<Storage>();
		cargoBay.storageType = CargoBay.CargoType.Liquids;
		cargoBay.storage.capacityKg = 1000f;
		cargoBay.storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.storage = cargoBay.storage;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_storage_liquid_bg_kanim", false);
	}

	// Token: 0x040007CA RID: 1994
	public const string ID = "LiquidCargoBay";
}
