using HarmonyLib;
using UnityEngine;

/// <summary>
/// 电池
/// </summary>
[HarmonyPatch(typeof(BatteryConfig))]
public class SuperBattery
{
    [HarmonyPostfix]
    [HarmonyPatch("DoPostConfigureComplete")]
    public static void Postfix(GameObject go)
    {
        Battery battery = go.AddOrGet<Battery>();
        battery.capacity = 1e5f;
        battery.joulesLostPerSecond = -100f;
    }
}

/// <summary>
/// 巨型电池
/// </summary>
[HarmonyPatch(typeof(BatteryMediumConfig))]
public class SuperBatteryMedium
{
	[HarmonyPostfix]
	[HarmonyPatch("DoPostConfigureComplete")]
	public static void Postfix(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 1e10f;
		battery.joulesLostPerSecond = -1e5f;
	}
}