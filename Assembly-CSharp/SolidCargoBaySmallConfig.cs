using System;
using TUNING;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public class SolidCargoBaySmallConfig : IBuildingConfig
{
	// Token: 0x060013D1 RID: 5073 RVA: 0x0006D441 File Offset: 0x0006B641
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x0006D448 File Offset: 0x0006B648
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidCargoBaySmall";
		int width = 3;
		int height = 3;
		string anim = "rocket_storage_solid_small_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
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
		return buildingDef;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x0006D4EC File Offset: 0x0006B6EC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
		};
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x0006D550 File Offset: 0x0006B750
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.STORAGE_LOCKERS_STANDARD, CargoBay.CargoType.Solids);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE, 0f, 0f);
	}

	// Token: 0x04000B58 RID: 2904
	public const string ID = "SolidCargoBaySmall";

	// Token: 0x04000B59 RID: 2905
	public float CAPACITY = 1200f * ROCKETRY.CARGO_CAPACITY_SCALE;
}
