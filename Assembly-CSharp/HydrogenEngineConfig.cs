using System;
using TUNING;
using UnityEngine;

// Token: 0x02000216 RID: 534
public class HydrogenEngineConfig : IBuildingConfig
{
	// Token: 0x06000B08 RID: 2824 RVA: 0x00041F72 File Offset: 0x00040172
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00041F7C File Offset: 0x0004017C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HydrogenEngine";
		int width = 7;
		int height = 5;
		string anim = "rocket_hydrogen_engine_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] engine_MASS_LARGE = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_LARGE;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, engine_MASS_LARGE, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0004202C File Offset: 0x0004022C
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

	// Token: 0x06000B0B RID: 2827 RVA: 0x00042090 File Offset: 0x00040290
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00042092 File Offset: 0x00040292
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00042094 File Offset: 0x00040294
	public override void DoPostConfigureComplete(GameObject go)
	{
		RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
		rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
		rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG;
		rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
		rocketEngine.exhaustElement = SimHashes.Steam;
		rocketEngine.exhaustTemperature = 2000f;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_hydrogen_engine_bg_kanim", false);
	}

	// Token: 0x04000749 RID: 1865
	public const string ID = "HydrogenEngine";
}
