﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public class SolidLimitValveConfig : IBuildingConfig
{
	// Token: 0x060013FF RID: 5119 RVA: 0x0006DEE0 File Offset: 0x0006C0E0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidLimitValve";
		int width = 1;
		int height = 2;
		string anim = "limit_valve_solid_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier, 0.2f);
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.PowerInputOffset = new CellOffset(0, 1);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			new LogicPorts.Port(LimitValve.RESET_PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.LOGIC_PORT_RESET, STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.RESET_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.RESET_PORT_INACTIVE, false, LogicPortSpriteType.ResetUpdate, true)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LimitValve.OUTPUT_PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.LOGIC_PORT_OUTPUT, STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.OUTPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SOLIDLIMITVALVE.OUTPUT_PORT_INACTIVE, false, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidLimitValve");
		return buildingDef;
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x0006E06F File Offset: 0x0006C26F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		UnityEngine.Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x0006E07C File Offset: 0x0006C27C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveTransitionController.Def>();
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		go.AddOrGet<SolidConduitBridge>();
		LimitValve limitValve = go.AddOrGet<LimitValve>();
		limitValve.conduitType = ConduitType.Solid;
		limitValve.displayUnitsInsteadOfMass = true;
		limitValve.Limit = 0f;
		limitValve.maxLimitKg = 500f;
		limitValve.sliderRanges = new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(50f, 50f),
			new NonLinearSlider.Range(30f, 200f),
			new NonLinearSlider.Range(20f, limitValve.maxLimitKg)
		};
	}

	// Token: 0x04000B65 RID: 2917
	public const string ID = "SolidLimitValve";

	// Token: 0x04000B66 RID: 2918
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}
