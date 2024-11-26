using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(PolymerizerConfig))]
internal class SuperPolymerizer
{
	[HarmonyPostfix]
	[HarmonyPatch("CreateBuildingDef")]
	public static void Postfix(ref BuildingDef __result)
    {
        __result.SelfHeatKilowattsWhenActive = -250f;
    }

    [HarmonyPrefix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static bool Prefix(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		Polymerizer polymerizer = go.AddOrGet<Polymerizer>();
		polymerizer.emitMass = 30f;
		polymerizer.emitTag = GameTagExtensions.Create(SimHashes.Polypropylene);
		polymerizer.emitOffset = new Vector3(-1.45f, 1f, 0f);
		polymerizer.exhaustElement = SimHashes.Steam;

		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 2f;
		conduitConsumer.capacityTag = GameTags.PlastifiableLiquid;
		conduitConsumer.capacityKG = 2f;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.invertElementFilter = false;
		conduitDispenser.elementFilter = new SimHashes[1] { SimHashes.Oxygen };

		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[1]
		{
			new ElementConverter.ConsumedElement(GameTags.PlastifiableLiquid, 1f)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[3]
		{
			new ElementConverter.OutputElement(10f, SimHashes.Polypropylene, 273.15f, useEntityTemperature: false, storeOutput: true),
			new ElementConverter.OutputElement(0, SimHashes.Steam, 273.15f, useEntityTemperature: false, storeOutput: true),
			new ElementConverter.OutputElement(0.5f, SimHashes.Oxygen, 273.15f, useEntityTemperature: false, storeOutput: true)
		};
		go.AddOrGet<DropAllWorkable>();
		Prioritizable.AddRef(go);
		return false;
	}
}