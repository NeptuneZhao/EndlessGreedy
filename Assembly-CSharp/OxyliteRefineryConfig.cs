using System;
using TUNING;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class OxyliteRefineryConfig : IBuildingConfig
{
	// Token: 0x060010D0 RID: 4304 RVA: 0x0005EBE8 File Offset: 0x0005CDE8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxyliteRefinery";
		int width = 3;
		int height = 4;
		string anim = "oxylite_refinery_kanim";
		int hitpoints = 100;
		float construction_time = 480f;
		string[] array = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
		};
		string[] construction_materials = array;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.ExhaustKilowattsWhenActive = 8f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(1, 0);
		return buildingDef;
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x0005ECC4 File Offset: 0x0005CEC4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Tag tag = SimHashes.Oxygen.CreateTag();
		Tag tag2 = SimHashes.Gold.CreateTag();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		OxyliteRefinery oxyliteRefinery = go.AddOrGet<OxyliteRefinery>();
		oxyliteRefinery.emitTag = SimHashes.OxyRock.CreateTag();
		oxyliteRefinery.emitMass = 10f;
		oxyliteRefinery.dropOffset = new Vector3(0f, 1f);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1.2f;
		conduitConsumer.capacityTag = tag;
		conduitConsumer.capacityKG = 6f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 23.2f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.showInUI = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = tag2;
		manualDeliveryKG.refillMass = 1.8000001f;
		manualDeliveryKG.capacity = 7.2000003f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(tag, 0.6f, true),
			new ElementConverter.ConsumedElement(tag2, 0.003f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.6f, SimHashes.OxyRock, 303.15f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};
		Prioritizable.AddRef(go);
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x0005EE49 File Offset: 0x0005D049
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000A48 RID: 2632
	public const string ID = "OxyliteRefinery";

	// Token: 0x04000A49 RID: 2633
	public const float EMIT_MASS = 10f;

	// Token: 0x04000A4A RID: 2634
	public const float INPUT_O2_PER_SECOND = 0.6f;

	// Token: 0x04000A4B RID: 2635
	public const float OXYLITE_PER_SECOND = 0.6f;

	// Token: 0x04000A4C RID: 2636
	public const float GOLD_PER_SECOND = 0.003f;

	// Token: 0x04000A4D RID: 2637
	public const float OUTPUT_TEMP = 303.15f;

	// Token: 0x04000A4E RID: 2638
	public const float REFILL_RATE = 2400f;

	// Token: 0x04000A4F RID: 2639
	public const float GOLD_STORAGE_AMOUNT = 7.2000003f;

	// Token: 0x04000A50 RID: 2640
	public const float O2_STORAGE_AMOUNT = 6f;

	// Token: 0x04000A51 RID: 2641
	public const float STORAGE_CAPACITY = 23.2f;
}
