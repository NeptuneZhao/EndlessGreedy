using HarmonyLib;
using UnityEngine;

namespace SuperBettery
{
	[HarmonyPatch(typeof(BatteryConfig), "DoPostConfigureComplete")]
	public class Patches
	{
		[HarmonyPostfix]
		public static void Postfix(GameObject go)
		{
			Battery battery = go.AddOrGet<Battery>();
			battery.capacity = 1e5f;
			battery.joulesLostPerSecond = -100f;
		}
	}
}