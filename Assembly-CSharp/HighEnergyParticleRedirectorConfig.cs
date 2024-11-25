using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class HighEnergyParticleRedirectorConfig : IBuildingConfig
{
	// Token: 0x06000AF3 RID: 2803 RVA: 0x000417E3 File Offset: 0x0003F9E3
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x000417EC File Offset: 0x0003F9EC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighEnergyParticleRedirector";
		int width = 1;
		int height = 2;
		string anim = "orb_transporter_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(0, 0);
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(HighEnergyParticleRedirector.PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.HIGHENERGYPARTICLEREDIRECTOR.LOGIC_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleRedirector");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x000418E4 File Offset: 0x0003FAE4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.showInUI = false;
		highEnergyParticleStorage.capacity = 501f;
		go.AddOrGet<HighEnergyParticleRedirector>().directorDelay = 0.5f;
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0004193A File Offset: 0x0003FB3A
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000736 RID: 1846
	public const string ID = "HighEnergyParticleRedirector";

	// Token: 0x04000737 RID: 1847
	public const float TRAVEL_DELAY = 0.5f;

	// Token: 0x04000738 RID: 1848
	public const float REDIRECT_PARTICLE_COST = 0.1f;
}
