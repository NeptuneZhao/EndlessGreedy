using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B7 RID: 951
public class SolidCargoBayClusterConfig : IBuildingConfig
{
	// Token: 0x060013C7 RID: 5063 RVA: 0x0006D119 File Offset: 0x0006B319
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x0006D120 File Offset: 0x0006B320
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CargoBayCluster";
		int width = 5;
		int height = 5;
		string anim = "rocket_cluster_storage_solid_kanim";
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
		buildingDef.Invincible = true;
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

	// Token: 0x060013C9 RID: 5065 RVA: 0x0006D1E4 File Offset: 0x0006B3E4
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

	// Token: 0x060013CA RID: 5066 RVA: 0x0006D248 File Offset: 0x0006B448
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.STORAGE_LOCKERS_STANDARD, CargoBay.CargoType.Solids);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MAJOR, 0f, 0f);
	}

	// Token: 0x04000B55 RID: 2901
	public const string ID = "CargoBayCluster";

	// Token: 0x04000B56 RID: 2902
	public float CAPACITY = ROCKETRY.SOLID_CARGO_BAY_CLUSTER_CAPACITY * ROCKETRY.CARGO_CAPACITY_SCALE;
}
