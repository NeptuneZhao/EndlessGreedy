﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class MethaneGeneratorConfig : IBuildingConfig
{
	// Token: 0x06000DC4 RID: 3524 RVA: 0x0004EC6C File Offset: 0x0004CE6C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MethaneGenerator";
		int width = 4;
		int height = 3;
		string anim = "generatormethane_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 800f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 2f;
		buildingDef.SelfHeatKilowattsWhenActive = 8f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x0004ED44 File Offset: 0x0004CF44
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.HeavyDutyGeneratorType, false);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<Storage>().capacityKg = 50f;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 0.90000004f;
		conduitConsumer.capacityTag = GameTags.CombustibleGas;
		conduitConsumer.capacityKG = 0.90000004f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
		energyGenerator.powerDistributionOrder = 8;
		energyGenerator.ignoreBatteryRefillPercent = true;
		energyGenerator.formula = new EnergyGenerator.Formula
		{
			inputs = new EnergyGenerator.InputItem[]
			{
				new EnergyGenerator.InputItem(GameTags.Methane, 0.09f, 0.90000004f)
			},
			outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(SimHashes.DirtyWater, 0.0675f, false, new CellOffset(1, 1), 313.15f),
				new EnergyGenerator.OutputItem(SimHashes.CarbonDioxide, 0.0225f, true, new CellOffset(0, 2), 383.15f)
			}
		};
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Methane,
			SimHashes.Syngas
		};
		Tinkerable.MakePowerTinkerable(go);
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x0400089C RID: 2204
	public const string ID = "MethaneGenerator";

	// Token: 0x0400089D RID: 2205
	public const float FUEL_CONSUMPTION_RATE = 0.09f;

	// Token: 0x0400089E RID: 2206
	private const float CO2_RATIO = 0.25f;

	// Token: 0x0400089F RID: 2207
	public const float WATER_OUTPUT_TEMPERATURE = 313.15f;

	// Token: 0x040008A0 RID: 2208
	private const int WIDTH = 4;

	// Token: 0x040008A1 RID: 2209
	private const int HEIGHT = 3;
}
