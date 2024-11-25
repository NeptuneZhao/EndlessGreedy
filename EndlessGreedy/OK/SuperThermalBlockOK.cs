using HarmonyLib;

namespace SuperThermalBlock
{
    [HarmonyPatch(typeof(ThermalBlockConfig), "CreateBuildingDef")]
    internal class Patches
    {
        [HarmonyPostfix]
        public static void Postfix(ref BuildingDef __result)
        {
            __result.Mass = new float[] { 10f };
            __result.MassForTemperatureModification = 2f;
        }
    }
}