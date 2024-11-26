using HarmonyLib;
using UnityEngine;
using TUNING;
using System.Collections.Generic;

/// <summary>
/// 存储箱
/// </summary>
[HarmonyPatch(typeof(StorageLockerConfig))]
internal class SuperStorageLocker
{
	[HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static void Postfix(GameObject go) => go.AddOrGet<Storage>().capacityKg = 1e10f;
}

/// <summary>
/// 储液罐
/// </summary>
[HarmonyPatch(typeof(LiquidReservoirConfig))]
internal class SuperLiquidReservoir
{
	[HarmonyPrefix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static bool Prefix(GameObject go)
	{
		go.AddOrGet<Reservoir>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go);
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.capacityKg = 1e10f;
		storage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;

		go.AddOrGet<SmartReservoir>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;

		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = null;

		return false;
	}
}

/// <summary>
/// 储气库
/// </summary>
[HarmonyPatch(typeof(GasReservoirConfig))]
internal class SuperGasReservoir
{
	public static readonly List<Storage.StoredItemModifier> ReservoirStoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Seal
	};

	[HarmonyPrefix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static bool Prefix(GameObject go)
	{
		go.AddOrGet<Reservoir>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go);
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.capacityKg = 1e10f;
		storage.SetDefaultStoredItemModifiers(ReservoirStoredItemModifiers);
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;

		go.AddOrGet<SmartReservoir>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.ignoreMinMassCheck = true;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.alwaysConsume = true;
		conduitConsumer.capacityKG = storage.capacityKg;

		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Gas;
		conduitDispenser.elementFilter = null;

		return false;
	}
}

/// <summary>
/// 冰箱
/// </summary>
[HarmonyPatch(typeof(RefrigeratorConfig))]
internal class SuperRefrigerator
{
	[HarmonyPostfix]
	[HarmonyPatch("DoPostConfigureComplete")]
	public static void Postfix(GameObject go) => go.AddOrGet<Storage>().capacityKg = 1e10f;
}