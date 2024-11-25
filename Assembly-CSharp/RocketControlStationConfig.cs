﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class RocketControlStationConfig : IBuildingConfig
{
	// Token: 0x060012D3 RID: 4819 RVA: 0x00068C2B File Offset: 0x00066E2B
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x00068C34 File Offset: 0x00066E34
	public override BuildingDef CreateBuildingDef()
	{
		string id = RocketControlStationConfig.ID;
		int width = 2;
		int height = 2;
		string anim = "rocket_control_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Repairable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		buildingDef.OnePerWorld = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(RocketControlStation.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.ROCKETCONTROLSTATION.LOGIC_PORT_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x00068CF9 File Offset: 0x00066EF9
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		component.AddTag(GameTags.RocketInteriorBuilding, false);
		component.AddTag(GameTags.UniquePerWorld, false);
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x00068D18 File Offset: 0x00066F18
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<RocketControlStationIdleWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStationLaunchWorkable>().workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<RocketControlStation>();
		go.AddOrGetDef<PoweredController.Def>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RocketInterior, false);
	}

	// Token: 0x04000AFB RID: 2811
	public static string ID = "RocketControlStation";

	// Token: 0x04000AFC RID: 2812
	public const float CONSOLE_WORK_TIME = 30f;

	// Token: 0x04000AFD RID: 2813
	public const float CONSOLE_IDLE_TIME = 120f;

	// Token: 0x04000AFE RID: 2814
	public const float WARNING_COOLDOWN = 30f;

	// Token: 0x04000AFF RID: 2815
	public const float DEFAULT_SPEED = 1f;

	// Token: 0x04000B00 RID: 2816
	public const float SLOW_SPEED = 0.5f;

	// Token: 0x04000B01 RID: 2817
	public const float DEFAULT_PILOT_MODIFIER = 1f;
}
