using HarmonyLib;
using UnityEngine;

namespace SuperBetteryMedium
{
	[HarmonyPatch(typeof(BatteryMediumConfig), "DoPostConfigureComplete")]
	public class Patches
	{
		[HarmonyPostfix]
		public static void Postfix(GameObject go)
		{
			Battery battery = go.AddOrGet<Battery>();
			battery.capacity = 1e10f;
			battery.joulesLostPerSecond = -1e5f;
		}
	}
}