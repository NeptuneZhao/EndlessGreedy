﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000336 RID: 822
public class PolymerizerConfig : IBuildingConfig
{
	// Token: 0x06001120 RID: 4384 RVA: 0x00060508 File Offset: 0x0005E708
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Polymerizer";
		int width = 3;
		int height = 3;
		string anim = "plasticrefinery_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		BuildingTemplates.CreateElectricalBuildingDef(buildingDef);
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 32f;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.OutputConduitType = ConduitType.Gas;
		buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x000605DC File Offset: 0x0005E7DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Polymerizer polymerizer = go.AddOrGet<Polymerizer>();
		polymerizer.emitMass = 30f;
		polymerizer.emitTag = GameTagExtensions.Create(SimHashes.Polypropylene);
		polymerizer.emitOffset = new Vector3(-1.45f, 1f, 0f);
		polymerizer.exhaustElement = SimHashes.Steam;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 1.6666666f;
		conduitConsumer.capacityTag = PolymerizerConfig.INPUT_ELEMENT_TAG;
		conduitConsumer.capacityKG = 1.6666666f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.invertElementFilter = false;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.CarbonDioxide
		};
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(PolymerizerConfig.INPUT_ELEMENT_TAG, 0.8333333f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.5f, SimHashes.Polypropylene, 348.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.008333334f, SimHashes.Steam, 473.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.008333334f, SimHashes.CarbonDioxide, 423.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		go.AddOrGet<DropAllWorkable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x0006077D File Offset: 0x0005E97D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000A6F RID: 2671
	public const string ID = "Polymerizer";

	// Token: 0x04000A70 RID: 2672
	private const ConduitType INPUT_CONDUIT_TYPE = ConduitType.Liquid;

	// Token: 0x04000A71 RID: 2673
	private const ConduitType OUTPUT_CONDUIT_TYPE = ConduitType.Gas;

	// Token: 0x04000A72 RID: 2674
	private const float CONSUMED_OIL_KG_PER_DAY = 500f;

	// Token: 0x04000A73 RID: 2675
	private const float GENERATED_PLASTIC_KG_PER_DAY = 300f;

	// Token: 0x04000A74 RID: 2676
	private const float SECONDS_PER_PLASTIC_BLOCK = 60f;

	// Token: 0x04000A75 RID: 2677
	private const float GENERATED_EXHAUST_STEAM_KG_PER_DAY = 5f;

	// Token: 0x04000A76 RID: 2678
	private const float GENERATED_EXHAUST_CO2_KG_PER_DAY = 5f;

	// Token: 0x04000A77 RID: 2679
	public static Tag INPUT_ELEMENT_TAG = GameTags.PlastifiableLiquid;

	// Token: 0x04000A78 RID: 2680
	private const SimHashes PRODUCED_ELEMENT = SimHashes.Polypropylene;

	// Token: 0x04000A79 RID: 2681
	private const SimHashes EXHAUST_ENVIRONMENT_ELEMENT = SimHashes.Steam;

	// Token: 0x04000A7A RID: 2682
	private const SimHashes EXHAUST_CONDUIT_ELEMENT = SimHashes.CarbonDioxide;
}
