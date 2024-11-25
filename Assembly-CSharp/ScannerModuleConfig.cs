using System;
using TUNING;
using UnityEngine;

// Token: 0x0200039B RID: 923
public class ScannerModuleConfig : IBuildingConfig
{
	// Token: 0x06001335 RID: 4917 RVA: 0x0006A854 File Offset: 0x00068A54
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x0006A85C File Offset: 0x00068A5C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ScannerModule";
		int width = 5;
		int height = 5;
		string anim = "rocket_scanner_module_kanim";
		int hitpoints = 1000;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			350f,
			1000f
		};
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString(),
			SimHashes.Polypropylene.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
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
		return buildingDef;
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x0006A938 File Offset: 0x00068B38
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGetDef<ScannerModule.Def>().scanRadius = 0;
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x0006A9A8 File Offset: 0x00068BA8
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR_PLUS, 0f, 0f);
	}

	// Token: 0x04000B31 RID: 2865
	public const string ID = "ScannerModule";
}
