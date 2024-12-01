using HarmonyLib;

public static class Efficiency
{
	public const float eff = 240f;
}

[HarmonyPatch(typeof(KeroseneEngineClusterSmallConfig))]
internal class SuperKeroseneEngineClusterSmall
{
	[HarmonyPostfix, HarmonyPatch(nameof(KeroseneEngineClusterSmallConfig.DoPostConfigureComplete))]
	public static void Postfix(ref UnityEngine.GameObject go)
	{
		go.AddOrGet<RocketEngineCluster>().efficiency = Efficiency.eff;
	}
}

[HarmonyPatch(typeof(KeroseneEngineClusterConfig))]
internal class SuperKeroseneEngineCluster
{
	[HarmonyPostfix, HarmonyPatch(nameof(KeroseneEngineClusterConfig.DoPostConfigureComplete))]
	public static void Postfix(ref UnityEngine.GameObject go)
	{
		go.AddOrGet<RocketEngineCluster>().efficiency = Efficiency.eff;
    }
}

[HarmonyPatch(typeof(KeroseneEngineConfig))]
internal class SuperKeroseneEngine
{
	[HarmonyPostfix, HarmonyPatch(nameof(KeroseneEngineConfig.DoPostConfigureComplete))]
	public static void Postfix(ref UnityEngine.GameObject go)
	{
		go.AddOrGet<RocketEngine>().efficiency = Efficiency.eff;
    }
}