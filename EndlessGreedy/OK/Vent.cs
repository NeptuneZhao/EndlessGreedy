using HarmonyLib;
using UnityEngine;

/// <summary>
/// 排气口
/// </summary>
[HarmonyPatch(typeof(GasVentConfig))]
internal class SuperGasVent
{
	[HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static void Postfix(GameObject go)
	{
		go.AddOrGet<Vent>().overpressureMass = 1e5f;
	}
}

// 还有GasVentHigh、LiquidVent等能修改