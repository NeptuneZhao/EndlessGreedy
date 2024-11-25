using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class NuclearReactorConfig : IBuildingConfig
{
	// Token: 0x06001058 RID: 4184 RVA: 0x0005C5AC File Offset: 0x0005A7AC
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0005C5B4 File Offset: 0x0005A7B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "NuclearReactor";
		int width = 5;
		int height = 6;
		string anim = "generatornuclear_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 0f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.RequiresPowerInput = false;
		buildingDef.RequiresPowerOutput = false;
		buildingDef.ThermalConductivity = 0.1f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.Overheatable = false;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(-2, 2);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("CONTROL_FUEL_DELIVERY", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.NUCLEARREACTOR.INPUT_PORT_INACTIVE, false, true)
		};
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Breakable = false;
		buildingDef.Invincible = true;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0005C714 File Offset: 0x0005A914
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		UnityEngine.Object.Destroy(go.GetComponent<BuildingEnabledButton>());
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 25;
		radiationEmitter.emitRadiusY = 25;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emissionOffset = new Vector3(0f, 2f, 0f);
		Storage storage = go.AddComponent<Storage>();
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		go.AddComponent<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Hide
		});
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.RequestedItemTag = ElementLoader.FindElementByHash(SimHashes.EnrichedUranium).tag;
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
		manualDeliveryKG.capacity = 180f;
		manualDeliveryKG.MinimumMass = 0.5f;
		go.AddOrGet<Reactor>();
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 90f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.storage = storage;
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0005C892 File Offset: 0x0005AA92
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddTag(GameTags.CorrosionProof);
	}

	// Token: 0x040009F9 RID: 2553
	public const string ID = "NuclearReactor";

	// Token: 0x040009FA RID: 2554
	private const float FUEL_CAPACITY = 180f;

	// Token: 0x040009FB RID: 2555
	public const float VENT_STEAM_TEMPERATURE = 673.15f;

	// Token: 0x040009FC RID: 2556
	public const float MELT_DOWN_TEMPERATURE = 3000f;

	// Token: 0x040009FD RID: 2557
	public const float MAX_VENT_PRESSURE = 150f;

	// Token: 0x040009FE RID: 2558
	public const float INCREASED_CONDUCTION_SCALE = 5f;

	// Token: 0x040009FF RID: 2559
	public const float REACTION_STRENGTH = 100f;

	// Token: 0x04000A00 RID: 2560
	public const int RADIATION_EMITTER_RANGE = 25;

	// Token: 0x04000A01 RID: 2561
	public const float OPERATIONAL_RADIATOR_INTENSITY = 2400f;

	// Token: 0x04000A02 RID: 2562
	public const float MELT_DOWN_RADIATOR_INTENSITY = 4800f;

	// Token: 0x04000A03 RID: 2563
	public const float FUEL_CONSUMPTION_SPEED = 0.016666668f;

	// Token: 0x04000A04 RID: 2564
	public const float BEGIN_REACTION_MASS = 0.5f;

	// Token: 0x04000A05 RID: 2565
	public const float STOP_REACTION_MASS = 0.25f;

	// Token: 0x04000A06 RID: 2566
	public const float DUMP_WASTE_AMOUNT = 100f;

	// Token: 0x04000A07 RID: 2567
	public const float WASTE_MASS_MULTIPLIER = 100f;

	// Token: 0x04000A08 RID: 2568
	public const float REACTION_MASS_TARGET = 60f;

	// Token: 0x04000A09 RID: 2569
	public const float COOLANT_AMOUNT = 30f;

	// Token: 0x04000A0A RID: 2570
	public const float COOLANT_CAPACITY = 90f;

	// Token: 0x04000A0B RID: 2571
	public const float MINIMUM_COOLANT_MASS = 30f;

	// Token: 0x04000A0C RID: 2572
	public const float WASTE_GERMS_PER_KG = 50f;

	// Token: 0x04000A0D RID: 2573
	public const float PST_MELTDOWN_COOLING_TIME = 3000f;

	// Token: 0x04000A0E RID: 2574
	public const string INPUT_PORT_ID = "CONTROL_FUEL_DELIVERY";
}
