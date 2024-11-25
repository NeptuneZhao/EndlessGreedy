using HarmonyLib;
using UnityEngine;

namespace SuperTile
{
	[HarmonyPatch(typeof(TileConfig), "CreateBuildingDef")]
	internal class Patches_1
	{
		[HarmonyPostfix]
		public static void Postfix(ref BuildingDef __result)
		{
			__result.BaseDecor = 25f;
			__result.BaseDecorRadius = 25f;
		}
	}

	[HarmonyPatch(typeof(TileConfig), "ConfigureBuildingTemplate")]
	internal class Patches_2
	{
		[HarmonyPostfix]
		public static void Postfix(GameObject go)
		{
			SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.strengthMultiplier = 100f;
			simCellOccupier.movementSpeedMultiplier = 100f;
		}
	}

}