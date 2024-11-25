using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class LogicPressureSensorLiquidConfig : IBuildingConfig
{
	// Token: 0x06000CED RID: 3309 RVA: 0x0004A584 File Offset: 0x00048784
	public override BuildingDef CreateBuildingDef()
	{
		string id = LogicPressureSensorLiquidConfig.ID;
		int width = 1;
		int height = 1;
		string anim = "switchliquidpressure_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICPRESSURESENSORLIQUID.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICPRESSURESENSORLIQUID.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICPRESSURESENSORLIQUID.LOGIC_PORT_INACTIVE, true, false)
		};
		SoundEventVolumeCache.instance.AddVolume("switchliquidpressure_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("switchliquidpressure_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicPressureSensorLiquidConfig.ID);
		return buildingDef;
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0004A688 File Offset: 0x00048888
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicPressureSensor logicPressureSensor = go.AddOrGet<LogicPressureSensor>();
		logicPressureSensor.rangeMin = 0f;
		logicPressureSensor.rangeMax = 2000f;
		logicPressureSensor.Threshold = 500f;
		logicPressureSensor.ActivateAboveThreshold = false;
		logicPressureSensor.manuallyControlled = false;
		logicPressureSensor.desiredState = Element.State.Liquid;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x04000812 RID: 2066
	public static string ID = "LogicPressureSensorLiquid";
}
