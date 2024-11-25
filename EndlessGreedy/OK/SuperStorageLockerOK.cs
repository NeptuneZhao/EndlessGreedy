using HarmonyLib;
using UnityEngine;
using TUNING;

namespace SuperStorageLocker
{
	[HarmonyPatch(typeof(StorageLockerConfig), "ConfigureBuildingTemplate")]
	internal class Patches
	{
		[HarmonyPostfix]
		public static void Postfix(GameObject go) => go.AddOrGet<Storage>().capacityKg = 1e10f;
	}
}