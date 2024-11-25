﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class AutoMinerConfig : IBuildingConfig
{
	// Token: 0x0600008B RID: 139 RVA: 0x00005820 File Offset: 0x00003A20
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AutoMiner", 2, 2, "auto_miner_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "AutoMiner");
		return buildingDef;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x000058C7 File Offset: 0x00003AC7
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<Operational>();
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<MiningSounds>();
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000058EF File Offset: 0x00003AEF
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, true);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000058F8 File Offset: 0x00003AF8
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00005904 File Offset: 0x00003B04
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
		autoMiner.x = -7;
		autoMiner.y = 0;
		autoMiner.width = 16;
		autoMiner.height = 9;
		autoMiner.vision_offset = new CellOffset(0, 1);
		AutoMinerConfig.AddVisualizer(go, false);
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00005950 File Offset: 0x00003B50
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -7;
		rangeVisualizer.RangeMin.y = -1;
		rangeVisualizer.RangeMax.x = 8;
		rangeVisualizer.RangeMax.y = 7;
		rangeVisualizer.OriginOffset = new Vector2I(0, 1);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(AutoMiner.DigBlockingCB);
		};
	}

	// Token: 0x04000070 RID: 112
	public const string ID = "AutoMiner";

	// Token: 0x04000071 RID: 113
	private const int RANGE = 7;

	// Token: 0x04000072 RID: 114
	private const int X = -7;

	// Token: 0x04000073 RID: 115
	private const int Y = 0;

	// Token: 0x04000074 RID: 116
	private const int WIDTH = 16;

	// Token: 0x04000075 RID: 117
	private const int HEIGHT = 9;

	// Token: 0x04000076 RID: 118
	private const int VISION_OFFSET = 1;
}
