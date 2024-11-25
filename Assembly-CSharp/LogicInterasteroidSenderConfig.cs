using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class LogicInterasteroidSenderConfig : IBuildingConfig
{
	// Token: 0x06000CD5 RID: 3285 RVA: 0x00049E2B File Offset: 0x0004802B
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x00049E34 File Offset: 0x00048034
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidSender", 1, 1, "inter_asteroid_automation_signal_sender_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("InputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidSender");
		return buildingDef;
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x00049F0F File Offset: 0x0004810F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.AddOrGet<UserNameable>().savedName = STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.DEFAULTNAME;
		go.AddOrGet<LogicBroadcaster>().PORT_ID = "InputPort";
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00049F3E File Offset: 0x0004813E
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x00049F46 File Offset: 0x00048146
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x00049F4E File Offset: 0x0004814E
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00049F56 File Offset: 0x00048156
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x0400080C RID: 2060
	public const string ID = "LogicInterasteroidSender";

	// Token: 0x0400080D RID: 2061
	public const string INPUT_PORT_ID = "InputPort";
}
