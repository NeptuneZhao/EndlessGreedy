using System;
using TUNING;
using UnityEngine;

// Token: 0x020003CA RID: 970
public class SpecialCargoBayConfig : IBuildingConfig
{
	// Token: 0x06001440 RID: 5184 RVA: 0x0006F45C File Offset: 0x0006D65C
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x0006F464 File Offset: 0x0006D664
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpecialCargoBay";
		int width = 5;
		int height = 5;
		string anim = "rocket_storage_live_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
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
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x0006F514 File Offset: 0x0006D714
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

	// Token: 0x06001443 RID: 5187 RVA: 0x0006F578 File Offset: 0x0006D778
	public override void DoPostConfigureComplete(GameObject go)
	{
		CargoBay cargoBay = go.AddOrGet<CargoBay>();
		cargoBay.storage = go.AddOrGet<Storage>();
		cargoBay.storageType = CargoBay.CargoType.Entities;
		cargoBay.storage.capacityKg = 100f;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_storage_live_bg_kanim", false);
	}

	// Token: 0x04000B8E RID: 2958
	public const string ID = "SpecialCargoBay";
}
