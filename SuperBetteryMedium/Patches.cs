using HarmonyLib;
using UnityEngine;

namespace SuperBetteryMedium
{
	[HarmonyPatch(typeof(BatteryMediumConfig), "DoPostConfigureComplete")]
	public class Patches
	{
		[HarmonyPostfix]
		public static void Postfix(ref GameObject go)
		{
			Battery battery = go.AddOrGet<Battery>();
			battery.capacity = float.MaxValue;
			battery.joulesLostPerSecond = -100f;
		}
	}
}
