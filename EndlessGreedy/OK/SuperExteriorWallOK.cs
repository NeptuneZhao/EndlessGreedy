using HarmonyLib;

namespace SuperExteriorWall
{
    [HarmonyPatch(typeof(ExteriorWallConfig), "CreateBuildingDef")]
    internal class Patches
    {
        [HarmonyPostfix]
        public static void Postfix(ref BuildingDef __result)
        {
            __result.BaseDecor = 5f;
            __result.BaseDecorRadius = 100f;
        }
    }
}