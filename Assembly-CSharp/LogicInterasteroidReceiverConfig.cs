using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class LogicInterasteroidReceiverConfig : IBuildingConfig
{
	// Token: 0x06000CCE RID: 3278 RVA: 0x00049CFD File Offset: 0x00047EFD
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x00049D04 File Offset: 0x00047F04
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidReceiver", 1, 1, "inter_asteroid_automation_signal_receiver_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("OutputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidReceiver");
		return buildingDef;
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x00049DDF File Offset: 0x00047FDF
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x00049DE7 File Offset: 0x00047FE7
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicBroadcastReceiver>().PORT_ID = "OutputPort";
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x00049DFF File Offset: 0x00047FFF
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00049E07 File Offset: 0x00048007
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x0400080A RID: 2058
	public const string ID = "LogicInterasteroidReceiver";

	// Token: 0x0400080B RID: 2059
	public const string OUTPUT_PORT_ID = "OutputPort";
}
