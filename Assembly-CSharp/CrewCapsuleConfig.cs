using System;
using TUNING;
using UnityEngine;

// Token: 0x0200005F RID: 95
public class CrewCapsuleConfig : IBuildingConfig
{
	// Token: 0x060001C0 RID: 448 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CrewCapsule";
		int width = 5;
		int height = 19;
		string anim = "rocket_small_steam_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.UtilityInputOffset = new CellOffset(2, 6);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000CC8F File Offset: 0x0000AE8F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LaunchConditionManager>();
		go.AddOrGet<RocketLaunchConditionVisualizer>();
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000CCB8 File Offset: 0x0000AEB8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<Storage>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
	}

	// Token: 0x04000119 RID: 281
	public const string ID = "CrewCapsule";
}
