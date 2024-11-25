using HarmonyLib;
using UnityEngine;

namespace SuperLadder
{
	[HarmonyPatch(typeof(LadderConfig), "CreateBuildingDef")]
	internal class Patches_1
	{
		[HarmonyPostfix]
		public static void Postfix(ref BuildingDef __result)
		{
			__result.BaseDecor = 25f;
			__result.BaseDecorRadius = 25f;
		}
	}

	[HarmonyPatch(typeof(LadderConfig), "ConfigureBuildingTemplate")]
	internal class Patches_2
	{
		[HarmonyPostfix]
		public static void Postfix(GameObject go)
		{
			Ladder ladder = go.AddOrGet<Ladder>();
			ladder.upwardsMovementSpeedMultiplier = 100f;
			ladder.downwardsMovementSpeedMultiplier = 100f;
			go.AddOrGet<AnimTileable>();
		}
	}
}