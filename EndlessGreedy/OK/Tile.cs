﻿using HarmonyLib;
using UnityEngine;

/// <summary>
/// 砖块
/// </summary>
[HarmonyPatch(typeof(TileConfig))]
internal class SuperTile
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
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 100f;
		simCellOccupier.movementSpeedMultiplier = 100f;
	}
}

/// <summary>
/// 液培砖
/// </summary>
[HarmonyPatch(typeof(HydroponicFarmConfig))]
internal class SuperHydroponicFarm
{
	[HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static void Postfix(GameObject go)
	{
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 100f;
		simCellOccupier.movementSpeedMultiplier = 100f;
	}

}

/// <summary>
/// 网格砖
/// </summary>
[HarmonyPatch(typeof(MeshTileConfig))]
internal class SuperMeshTile
{
	[HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static void Postfix(GameObject go)
	{
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.strengthMultiplier = 100f;
		simCellOccupier.movementSpeedMultiplier = 100f;
	}

}