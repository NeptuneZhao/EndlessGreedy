﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class GasCargoBayClusterConfig : IBuildingConfig
{
	// Token: 0x060009EB RID: 2539 RVA: 0x0003A8F8 File Offset: 0x00038AF8
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0003A900 File Offset: 0x00038B00
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasCargoBayCluster";
		int width = 5;
		int height = 5;
		string anim = "rocket_cluster_storage_gas_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] dense_TIER = BUILDINGS.ROCKETRY_MASS_KG.DENSE_TIER2;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, dense_TIER, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		buildingDef.ShowInBuildMenu = false;
		return buildingDef;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x0003A9BC File Offset: 0x00038BBC
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

	// Token: 0x060009EE RID: 2542 RVA: 0x0003AA20 File Offset: 0x00038C20
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.GASES, CargoBay.CargoType.Gasses);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE, 0f, 0f);
	}

	// Token: 0x04000686 RID: 1670
	public const string ID = "GasCargoBayCluster";

	// Token: 0x04000687 RID: 1671
	public float CAPACITY = ROCKETRY.GAS_CARGO_BAY_CLUSTER_CAPACITY * ROCKETRY.CARGO_CAPACITY_SCALE;
}
