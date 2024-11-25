using HarmonyLib;
using UnityEngine;

namespace SuperFlushToilet
{
    [HarmonyPatch(typeof(FlushToiletConfig), "ConfigureBuildingTemplate")]
    internal class Patches
    {
        [HarmonyPostfix]
        public static void Postfix(GameObject go)
        {
            FlushToilet flushToilet = go.AddOrGet<FlushToilet>();
            flushToilet.massEmittedPerUse = 10f;
            flushToilet.diseasePerFlush = 0;
            flushToilet.diseaseOnDupePerFlush = 0;
        }
    }
}