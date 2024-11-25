using System;
using TUNING;
using UnityEngine;

// Token: 0x02000213 RID: 531
public class HighEnergyParticleSpawnerConfig : IBuildingConfig
{
	// Token: 0x06000AF8 RID: 2808 RVA: 0x00041944 File Offset: 0x0003FB44
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x0004194C File Offset: 0x0003FB4C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighEnergyParticleSpawner";
		int width = 1;
		int height = 2;
		string anim = "radiation_collector_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = CellOffset.none;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00041A3C File Offset: 0x0003FC3C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<HighEnergyParticleStorage>().capacity = 500f;
		go.AddOrGet<LoopingSounds>();
		HighEnergyParticleSpawner highEnergyParticleSpawner = go.AddOrGet<HighEnergyParticleSpawner>();
		highEnergyParticleSpawner.minLaunchInterval = 2f;
		highEnergyParticleSpawner.radiationSampleRate = 0.2f;
		highEnergyParticleSpawner.minSlider = 50;
		highEnergyParticleSpawner.maxSlider = 500;
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x00041AA9 File Offset: 0x0003FCA9
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000739 RID: 1849
	public const string ID = "HighEnergyParticleSpawner";

	// Token: 0x0400073A RID: 1850
	public const float MIN_LAUNCH_INTERVAL = 2f;

	// Token: 0x0400073B RID: 1851
	public const float RADIATION_SAMPLE_RATE = 0.2f;

	// Token: 0x0400073C RID: 1852
	public const float HEP_PER_RAD = 0.1f;

	// Token: 0x0400073D RID: 1853
	public const int MIN_SLIDER = 50;

	// Token: 0x0400073E RID: 1854
	public const int MAX_SLIDER = 500;

	// Token: 0x0400073F RID: 1855
	public const float DISABLED_CONSUMPTION_RATE = 1f;
}
