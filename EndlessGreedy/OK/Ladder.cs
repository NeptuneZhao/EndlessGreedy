using HarmonyLib;
using UnityEngine;

/// <summary>
/// 梯子
/// </summary>
[HarmonyPatch(typeof(LadderConfig))]
internal class SuperLadder
{
	[HarmonyPostfix]
	[HarmonyPatch("CreateBuildingDef")]
	public static void Postfix(ref BuildingDef __result)
	{
		__result.BaseDecor = 10f;
		__result.BaseDecorRadius = 10f;
	}

    [HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
    public static void Postfix(GameObject go)
    {
        Ladder ladder = go.AddOrGet<Ladder>();
        ladder.upwardsMovementSpeedMultiplier = 10f;
        ladder.downwardsMovementSpeedMultiplier = 10f;
        go.AddOrGet<AnimTileable>();
    }
}