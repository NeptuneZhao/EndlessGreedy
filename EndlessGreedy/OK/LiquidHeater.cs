using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(LiquidHeaterConfig))]
internal class SuperLiquidHeater
{
	[HarmonyPostfix, HarmonyPatch(nameof(LiquidHeaterConfig.CreateBuildingDef))]
	public static void Postfix0(ref BuildingDef __result)
	{
		__result.ExhaustKilowattsWhenActive = 16000f;
        __result.OverheatTemperature = 1273.15f;

    }

    [HarmonyPostfix, HarmonyPatch(nameof(LiquidHeaterConfig.ConfigureBuildingTemplate))]
	public static void Postfix1(GameObject go)
	{
		go.AddOrGet<SpaceHeater>().targetTemperature = 378.15f;
	}
}
