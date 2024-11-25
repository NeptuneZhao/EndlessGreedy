using System;
using TUNING;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class GasCargoBaySmallConfig : IBuildingConfig
{
	// Token: 0x060009F5 RID: 2549 RVA: 0x0003AC0C File Offset: 0x00038E0C
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0003AC14 File Offset: 0x00038E14
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasCargoBaySmall";
		int width = 3;
		int height = 3;
		string anim = "rocket_storage_gas_small_kanim";
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
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0003ACAC File Offset: 0x00038EAC
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

	// Token: 0x060009F8 RID: 2552 RVA: 0x0003AD10 File Offset: 0x00038F10
	public override void DoPostConfigureComplete(GameObject go)
	{
		go = BuildingTemplates.ExtendBuildingToClusterCargoBay(go, this.CAPACITY, STORAGEFILTERS.GASES, CargoBay.CargoType.Gasses);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR, 0f, 0f);
	}

	// Token: 0x04000689 RID: 1673
	public const string ID = "GasCargoBaySmall";

	// Token: 0x0400068A RID: 1674
	public float CAPACITY = 360f * ROCKETRY.CARGO_CAPACITY_SCALE;
}
