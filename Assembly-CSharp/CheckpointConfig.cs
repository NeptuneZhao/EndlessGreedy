using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class CheckpointConfig : IBuildingConfig
{
	// Token: 0x0600011E RID: 286 RVA: 0x00008778 File Offset: 0x00006978
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Checkpoint";
		int width = 1;
		int height = 3;
		string anim = "checkpoint_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.Floodable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 2);
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(Checkpoint.PORT_ID, new CellOffset(0, 2), STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.CHECKPOINT.LOGIC_PORT_INACTIVE, true, false)
		};
		return buildingDef;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00008849 File Offset: 0x00006A49
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Checkpoint>();
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00008852 File Offset: 0x00006A52
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040000AF RID: 175
	public const string ID = "Checkpoint";
}
