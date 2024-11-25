using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003C3 RID: 963
public class SolidLogicValveConfig : IBuildingConfig
{
	// Token: 0x06001403 RID: 5123 RVA: 0x0006E128 File Offset: 0x0006C328
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidLogicValve";
		int width = 1;
		int height = 2;
		string anim = "conveyor_shutoff_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, tier2, 0.2f);
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
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.SOLIDLOGICVALVE.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.SOLIDLOGICVALVE.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.SOLIDLOGICVALVE.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidLogicValve");
		return buildingDef;
	}

	// Token: 0x06001404 RID: 5124 RVA: 0x0006E24E File Offset: 0x0006C44E
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x06001405 RID: 5125 RVA: 0x0006E250 File Offset: 0x0006C450
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
		go.GetComponent<RequireInputs>().SetRequirements(true, false);
		go.AddOrGet<SolidConduitBridge>();
		go.AddOrGet<SolidLogicValve>();
	}

	// Token: 0x04000B67 RID: 2919
	public const string ID = "SolidLogicValve";

	// Token: 0x04000B68 RID: 2920
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}
