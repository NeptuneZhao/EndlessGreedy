using HarmonyLib;

[HarmonyPatch(typeof(KeroseneEngineClusterSmallConfig))]
internal class SuperKeroseneEngineClusterSmall
{
	[HarmonyPostfix, HarmonyPatch(nameof(KeroseneEngineClusterSmallConfig.DoPostConfigureComplete))]
	public static void Postfix(ref UnityEngine.GameObject go)
	{
		go.AddOrGet<RocketEngineCluster>().efficiency = 1e5f;
	}
}

[HarmonyPatch(typeof(KeroseneEngineClusterConfig))]
internal class SuperKeroseneEngineCluster
{
	[HarmonyPostfix, HarmonyPatch(nameof(KeroseneEngineClusterConfig.DoPostConfigureComplete))]
	public static void Postfix(ref UnityEngine.GameObject go)
	{
		go.AddOrGet<RocketEngineCluster>().efficiency = 1e5f;
	}
}

[HarmonyPatch(typeof(KeroseneEngineConfig))]
internal class SuperKeroseneEngine
{
	[HarmonyPostfix, HarmonyPatch(nameof(KeroseneEngineConfig.DoPostConfigureComplete))]
	public static void Postfix(ref UnityEngine.GameObject go)
	{
		go.AddOrGet<RocketEngine>().efficiency = 1e5f;
	}
}