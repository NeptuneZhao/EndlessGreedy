using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000634 RID: 1588
public class DevTool_StoryTraits_Reveal : DevTool
{
	// Token: 0x06002711 RID: 10001 RVA: 0x000DE3A4 File Offset: 0x000DC5A4
	protected override void RenderTo(DevPanel panel)
	{
		int cellIndex;
		bool flag = DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out cellIndex);
		if (ImGuiEx.Button("Focus on headquaters", flag))
		{
			DevToolUtil.FocusCameraOnCell(cellIndex);
		}
		if (!flag)
		{
			ImGuiEx.TooltipForPrevious("Couldn't find headquaters");
		}
		if (ImGui.CollapsingHeader("Search world for entity", ImGuiTreeNodeFlags.DefaultOpen))
		{
			IReadOnlyList<WorldGenSpawner.Spawnable> allSpawnables = this.GetAllSpawnables();
			if (allSpawnables == null)
			{
				ImGui.Text("Couldn't find a list of spawnables");
				return;
			}
			foreach (string text in this.GetPrefabIDsToSearchFor())
			{
				int cellIndex2;
				bool cellIndexForSpawnable = this.GetCellIndexForSpawnable(text, allSpawnables, out cellIndex2);
				string str = "\"" + text + "\"";
				bool flag2 = cellIndexForSpawnable;
				if (ImGuiEx.Button("Reveal and focus on " + str, flag2))
				{
					DevToolUtil.RevealAndFocusAt(cellIndex2);
				}
				if (!flag2)
				{
					ImGuiEx.TooltipForPrevious("Couldn't find a cell that contained a spawnable with component " + str);
				}
			}
		}
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x000DE490 File Offset: 0x000DC690
	public IEnumerable<string> GetPrefabIDsToSearchFor()
	{
		yield return "MegaBrainTank";
		yield return "GravitasCreatureManipulator";
		yield return "LonelyMinionHouse";
		yield return "FossilDig";
		yield break;
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x000DE49C File Offset: 0x000DC69C
	private bool GetCellIndexForSpawnable(string prefabId, IReadOnlyList<WorldGenSpawner.Spawnable> spawnablesToSearch, out int cellIndex)
	{
		foreach (WorldGenSpawner.Spawnable spawnable in spawnablesToSearch)
		{
			if (prefabId == spawnable.spawnInfo.id)
			{
				cellIndex = spawnable.cell;
				return true;
			}
		}
		cellIndex = -1;
		return false;
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x000DE504 File Offset: 0x000DC704
	private IReadOnlyList<WorldGenSpawner.Spawnable> GetAllSpawnables()
	{
		WorldGenSpawner worldGenSpawner = UnityEngine.Object.FindObjectOfType<WorldGenSpawner>(true);
		if (worldGenSpawner == null)
		{
			return null;
		}
		IReadOnlyList<WorldGenSpawner.Spawnable> spawnables = worldGenSpawner.GetSpawnables();
		if (spawnables == null)
		{
			return null;
		}
		return spawnables;
	}
}
