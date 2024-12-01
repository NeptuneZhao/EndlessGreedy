using HarmonyLib;

[HarmonyPatch(typeof(ManualGeneratorConfig))]
internal class SuperManualGenerator
{
    [HarmonyPostfix, HarmonyPatch(nameof(ManualGeneratorConfig.CreateBuildingDef))]
    public static void Postfix(ref BuildingDef __result)
    {
        __result.GeneratorWattageRating = 4000f;
        __result.GeneratorBaseCapacity = 4000f;
    }
}

[HarmonyPatch(typeof(HydrogenGeneratorConfig))]
internal class SuperHydrogenGenerator
{
    [HarmonyPostfix, HarmonyPatch(nameof(HydrogenGeneratorConfig.CreateBuildingDef))]
    public static void Postfix(ref BuildingDef __result)
    {
        __result.GeneratorWattageRating = 8000f;
        __result.GeneratorBaseCapacity = 8000f;
    }
}