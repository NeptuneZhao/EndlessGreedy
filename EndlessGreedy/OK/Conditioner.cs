using HarmonyLib;
using System.Collections.Generic;

/// <summary>
/// 液温调节器
/// </summary>
[HarmonyPatch(typeof(LiquidConditionerConfig))]
internal class SuperLiquidConditioner
{
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>
	{
		Storage.StoredItemModifier.Hide,
		Storage.StoredItemModifier.Insulate,
		Storage.StoredItemModifier.Seal
	};

	[HarmonyPostfix, HarmonyPatch(nameof(LiquidConditionerConfig.CreateBuildingDef))]
	public static void Postfix(ref BuildingDef __result)
	{
		__result.SelfHeatKilowattsWhenActive = -100f;
		__result.Overheatable = false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(LiquidConditionerConfig.ConfigureBuildingTemplate))]
	public static bool Prefix(UnityEngine.GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		AirConditioner airConditioner = go.AddOrGet<AirConditioner>();
		airConditioner.temperatureDelta = -50f;
		airConditioner.maxEnvironmentDelta = -50f;
		airConditioner.isLiquidConditioner = true;
		go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 10f;
		Storage storage = BuildingTemplates.CreateDefaultStorage(go);
		storage.showInUI = true;
		storage.capacityKg = 2f * conduitConsumer.consumptionRate;
		storage.SetDefaultStoredItemModifiers(StoredItemModifiers);

		return false;
	}
}

/// <summary>
/// 温度调节器
/// </summary>
[HarmonyPatch(typeof(AirConditionerConfig))]
internal class SuperAirConditioner
{
	[HarmonyPostfix, HarmonyPatch(nameof(AirConditionerConfig.CreateBuildingDef))]
	public static void Postfix(ref BuildingDef __result)
	{
		__result.SelfHeatKilowattsWhenActive = -100f;
		__result.Overheatable = false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(AirConditionerConfig.ConfigureBuildingTemplate))]
	public static bool Prefix(UnityEngine.GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		AirConditioner airConditioner = go.AddOrGet<AirConditioner>();
		airConditioner.temperatureDelta = -50f;
		airConditioner.maxEnvironmentDelta = -50f;
		go.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;

		return false;
	}
}