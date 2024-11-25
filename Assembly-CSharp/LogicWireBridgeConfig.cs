using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class LogicWireBridgeConfig : IBuildingConfig
{
	// Token: 0x06000D1E RID: 3358 RVA: 0x0004B3DC File Offset: 0x000495DC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicWireBridge";
		int width = 3;
		int height = 1;
		string anim = "logic_bridge_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier_TINY = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.LogicBridge;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier_TINY, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicWireBridgeConfig.BRIDGE_LOGIC_IO_ID, new CellOffset(-1, 0), STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_INACTIVE, false, false),
			LogicPorts.Port.InputPort(LogicWireBridgeConfig.BRIDGE_LOGIC_IO_ID, new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICWIREBRIDGE.LOGIC_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicWireBridge");
		return buildingDef;
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0004B524 File Offset: 0x00049724
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x0004B53B File Offset: 0x0004973B
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0004B559 File Offset: 0x00049759
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0004B576 File Offset: 0x00049776
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0004B58C File Offset: 0x0004978C
	private LogicUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		LogicUtilityNetworkLink logicUtilityNetworkLink = go.AddOrGet<LogicUtilityNetworkLink>();
		logicUtilityNetworkLink.bitDepth = LogicWire.BitDepth.OneBit;
		logicUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		logicUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return logicUtilityNetworkLink;
	}

	// Token: 0x0400081F RID: 2079
	public const string ID = "LogicWireBridge";

	// Token: 0x04000820 RID: 2080
	public static readonly HashedString BRIDGE_LOGIC_IO_ID = new HashedString("BRIDGE_LOGIC_IO");
}
