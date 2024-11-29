using HarmonyLib;

/// <summary>
/// 干板墙
/// </summary>
[HarmonyPatch(typeof(ExteriorWallConfig))]
internal class SuperExteriorWall
{
	[HarmonyPostfix]
	[HarmonyPatch("CreateBuildingDef")]
	public static void Postfix(ref BuildingDef __result)
	{
		__result.BaseDecor = 5f;
		__result.BaseDecorRadius = 5f;
	}
}

/// <summary>
/// 变温板
/// </summary>
[HarmonyPatch(typeof(ThermalBlockConfig))]
internal class SuperThermalBlock
{
	[HarmonyPostfix]
	[HarmonyPatch("CreateBuildingDef")]
	public static void Postfix(ref BuildingDef __result)
	{
		__result.Mass = new float[] { 10f };
		__result.MassForTemperatureModification = 2f;
	}
}