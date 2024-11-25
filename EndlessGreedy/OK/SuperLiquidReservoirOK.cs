using HarmonyLib;
using UnityEngine;
using TUNING;

namespace SuperLiquidReservoir
{
	[HarmonyPatch(typeof(LiquidReservoirConfig), "ConfigureBuildingTemplate")]
	internal class Patches
	{
		[HarmonyPrefix]
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
}