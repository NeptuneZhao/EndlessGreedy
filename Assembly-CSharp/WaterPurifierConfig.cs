﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class WaterPurifierConfig : IBuildingConfig
{
	// Token: 0x0600157D RID: 5501 RVA: 0x00075FBC File Offset: 0x000741BC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WaterPurifier";
		int width = 4;
		int height = 3;
		string anim = "waterpurifier_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(-1, 0));
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.PowerInputOffset = new CellOffset(2, 0);
		buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
		buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "WaterPurifier");
		return buildingDef;
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x000760A0 File Offset: 0x000742A0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<WaterPurifier>();
		Prioritizable.AddRef(go);
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Filter"), 1f, true),
			new ElementConverter.ConsumedElement(new Tag("DirtyWater"), 5f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(5f, SimHashes.Water, 0f, false, true, 0f, 0.5f, 0.75f, byte.MaxValue, 0, true),
			new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 0f, false, true, 0f, 0.5f, 0.25f, byte.MaxValue, 0, true)
		};
		ElementDropper elementDropper = go.AddComponent<ElementDropper>();
		elementDropper.emitMass = 10f;
		elementDropper.emitTag = new Tag("ToxicSand");
		elementDropper.emitOffset = new Vector3(0f, 1f, 0f);
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("Filter");
		manualDeliveryKG.capacity = 1200f;
		manualDeliveryKG.refillMass = 300f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		conduitConsumer.capacityKG = 20f;
		conduitConsumer.capacityTag = GameTags.AnyWater;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.DirtyWater
		};
	}

	// Token: 0x0600157F RID: 5503 RVA: 0x00076283 File Offset: 0x00074483
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000C2A RID: 3114
	public const string ID = "WaterPurifier";

	// Token: 0x04000C2B RID: 3115
	private const float FILTER_INPUT_RATE = 1f;

	// Token: 0x04000C2C RID: 3116
	private const float DIRTY_WATER_INPUT_RATE = 5f;

	// Token: 0x04000C2D RID: 3117
	private const float FILTER_CAPACITY = 1200f;

	// Token: 0x04000C2E RID: 3118
	private const float USED_FILTER_OUTPUT_RATE = 0.2f;

	// Token: 0x04000C2F RID: 3119
	private const float CLEAN_WATER_OUTPUT_RATE = 5f;

	// Token: 0x04000C30 RID: 3120
	private const float TARGET_OUTPUT_TEMPERATURE = 313.15f;
}
