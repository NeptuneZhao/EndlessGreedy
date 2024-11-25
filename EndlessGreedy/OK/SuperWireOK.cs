using HarmonyLib;

namespace SuperWire
{
    [HarmonyPatch(typeof(Wire), "GetMaxWattageAsFloat")]
    internal class Patches
    {
        [HarmonyPostfix]
        public static void Postfix(ref float __result)
        {
            __result = 1e10f;
        }
    }
}