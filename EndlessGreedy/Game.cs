using HarmonyLib;

[HarmonyPatch(typeof(Game))]
internal class SuperGame
{
	[HarmonyPostfix, HarmonyPatch("OnPrefabInit")]
	public static void Postfix(Game __instance)
	{
		//__instance.gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, __instance.gasConduitSystem, 100f, 0.1f);
		//__instance.liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, __instance.liquidConduitSystem, 100f, 0.1f);
	}
}