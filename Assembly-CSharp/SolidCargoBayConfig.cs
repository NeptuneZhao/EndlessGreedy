﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B8 RID: 952
public class SolidCargoBayConfig : IBuildingConfig
{
	// Token: 0x060013CC RID: 5068 RVA: 0x0006D28E File Offset: 0x0006B48E
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x0006D298 File Offset: 0x0006B498
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CargoBay";
		int width = 5;
		int height = 5;
		string anim = "rocket_storage_solid_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			1000f,
			1000f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.Invincible = true;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 3);
		return buildingDef;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x0006D37C File Offset: 0x0006B57C
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

	// Token: 0x060013CF RID: 5071 RVA: 0x0006D3E0 File Offset: 0x0006B5E0
	public override void DoPostConfigureComplete(GameObject go)
	{
		CargoBay cargoBay = go.AddOrGet<CargoBay>();
		cargoBay.storage = go.AddOrGet<Storage>();
		cargoBay.storageType = CargoBay.CargoType.Solids;
		cargoBay.storage.capacityKg = 1000f;
		cargoBay.storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_storage_solid_bg_kanim", false);
		go.AddOrGet<SolidConduitDispenser>();
	}

	// Token: 0x04000B57 RID: 2903
	public const string ID = "CargoBay";
}
