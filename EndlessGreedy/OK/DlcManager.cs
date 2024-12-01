using HarmonyLib;

[HarmonyPatch(typeof(DlcManager))]
internal class SuperDlcManager
{
    [HarmonyPostfix, HarmonyPatch(nameof(DlcManager.IsExpansion1Active))]
    public static void Postfix(ref bool __result) => __result = true;
}