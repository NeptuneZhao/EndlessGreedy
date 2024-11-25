using System;
using TUNING;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class DevHEPSpawnerConfig : IBuildingConfig
{
	// Token: 0x060001F6 RID: 502 RVA: 0x0000DFC2 File Offset: 0x0000C1C2
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000DFCC File Offset: 0x0000C1CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevHEPSpawner";
		int width = 1;
		int height = 1;
		string anim = "dev_radbolt_generator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "DevHEPSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevHEPSpawner>().boltAmount = 50f;
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000E0F5 File Offset: 0x0000C2F5
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000142 RID: 322
	public const string ID = "DevHEPSpawner";
}
