﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200032B RID: 811
public class PetroleumGeneratorConfig : IBuildingConfig
{
	// Token: 0x060010EE RID: 4334 RVA: 0x0005F680 File Offset: 0x0005D880
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PetroleumGenerator";
		int width = 3;
		int height = 4;
		string anim = "generatorpetrol_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"Metal"
		};
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier, 0.2f);
		buildingDef.GeneratorWattageRating = 2000f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 4f;
		buildingDef.SelfHeatKilowattsWhenActive = 16f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(1, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.InputConduitType = ConduitType.Liquid;
		return buildingDef;
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0005F75C File Offset: 0x0005D95C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Storage>();
		BuildingDef def = go.GetComponent<Building>().Def;
		float num = 20f;
		go.AddOrGet<LoopingSounds>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = def.InputConduitType;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityTag = GameTags.CombustibleLiquid;
		conduitConsumer.capacityKG = num;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
		energyGenerator.powerDistributionOrder = 8;
		energyGenerator.ignoreBatteryRefillPercent = true;
		energyGenerator.hasMeter = true;
		energyGenerator.formula = new EnergyGenerator.Formula
		{
			inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(GameTags.CombustibleLiquid, 2f, num)
			},
			outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, 0.5f, false, new CellOffset(0, 3), 383.15f),
				new EnergyGenerator.OutputItem(SimHashes.DirtyWater, 0.75f, false, new CellOffset(1, 1), 313.15f)
			}
		};
		Tinkerable.MakePowerTinkerable(go);
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000A59 RID: 2649
	public const string ID = "PetroleumGenerator";

	// Token: 0x04000A5A RID: 2650
	public const float CONSUMPTION_RATE = 2f;

	// Token: 0x04000A5B RID: 2651
	private const SimHashes INPUT_ELEMENT = SimHashes.Petroleum;

	// Token: 0x04000A5C RID: 2652
	private const SimHashes EXHAUST_ELEMENT_GAS = SimHashes.CarbonDioxide;

	// Token: 0x04000A5D RID: 2653
	private const SimHashes EXHAUST_ELEMENT_LIQUID = SimHashes.DirtyWater;

	// Token: 0x04000A5E RID: 2654
	public const float EFFICIENCY_RATE = 0.5f;

	// Token: 0x04000A5F RID: 2655
	public const float EXHAUST_GAS_RATE = 0.5f;

	// Token: 0x04000A60 RID: 2656
	public const float EXHAUST_LIQUID_RATE = 0.75f;

	// Token: 0x04000A61 RID: 2657
	private const int WIDTH = 3;

	// Token: 0x04000A62 RID: 2658
	private const int HEIGHT = 4;
}
