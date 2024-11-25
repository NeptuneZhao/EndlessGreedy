using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class LogicPowerRelayConfig : IBuildingConfig
{
	// Token: 0x06000CE5 RID: 3301 RVA: 0x0004A2CC File Offset: 0x000484CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicPowerRelayConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "switchpowershutoff_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICPOWERRELAY.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICPOWERRELAY.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICPOWERRELAY.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicPowerRelayConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0004A3C8 File Offset: 0x000485C8
	public override void DoPostConfigureComplete(GameObject go)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<OperationalControlledSwitch>().objectLayer = ObjectLayer.Wire;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000810 RID: 2064
	public static string ID = "LogicPowerRelay";
}
