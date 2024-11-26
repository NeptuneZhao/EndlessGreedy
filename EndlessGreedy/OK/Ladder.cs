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
		__result.BaseDecor = 25f;
		__result.BaseDecorRadius = 25f;
	}

    [HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
    public static void Postfix(GameObject go)
    {
        Ladder ladder = go.AddOrGet<Ladder>();
        ladder.upwardsMovementSpeedMultiplier = 100f;
        ladder.downwardsMovementSpeedMultiplier = 100f;
        go.AddOrGet<AnimTileable>();
    }
}