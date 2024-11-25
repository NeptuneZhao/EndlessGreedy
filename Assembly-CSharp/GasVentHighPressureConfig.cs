using System;
using TUNING;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class GasVentHighPressureConfig : IBuildingConfig
{
	// Token: 0x06000A3D RID: 2621 RVA: 0x0003C2E4 File Offset: 0x0003A4E4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GasVentHighPressure";
		int width = 1;
		int height = 1;
		string anim = "ventgas_powered_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
		};
		string[] construction_materials = array;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.GasConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, "GasVentHighPressure");
		SoundEventVolumeCache.instance.AddVolume("ventgas_kanim", "GasVent_clunk", NOISE_POLLUTION.NOISY.TIER0);
		return buildingDef;
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x0003C3D4 File Offset: 0x0003A5D4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Exhaust>();
		go.AddOrGet<LogicOperationalController>();
		Vent vent = go.AddOrGet<Vent>();
		vent.conduitType = ConduitType.Gas;
		vent.endpointType = Endpoint.Sink;
		vent.overpressureMass = 20f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
		go.AddOrGet<SimpleVent>();
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0003C44C File Offset: 0x0003A64C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<VentController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040006A9 RID: 1705
	public const string ID = "GasVentHighPressure";

	// Token: 0x040006AA RID: 1706
	private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x040006AB RID: 1707
	public const float OVERPRESSURE_MASS = 20f;
}
