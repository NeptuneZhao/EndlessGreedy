﻿using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using ImGuiObjectDrawer;
using UnityEngine;

// Token: 0x0200060C RID: 1548
public class DevToolCavity : DevTool
{
	// Token: 0x06002629 RID: 9769 RVA: 0x000D3B4C File Offset: 0x000D1D4C
	public DevToolCavity() : this(Option.None)
	{
	}

	// Token: 0x0600262A RID: 9770 RVA: 0x000D3B5E File Offset: 0x000D1D5E
	public DevToolCavity(Option<DevToolEntityTarget.ForSimCell> target)
	{
		this.targetOpt = target;
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x000D3B74 File Offset: 0x000D1D74
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("Eyedrop New Target"))
			{
				panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
				{
					this.targetOpt = (DevToolEntityTarget.ForSimCell)target;
				}, new Func<DevToolEntityTarget, Option<string>>(DevToolCavity.GetErrorForCandidateTarget)));
			}
			ImGui.EndMenuBar();
		}
		this.Name = "Cavity Info";
		if (this.targetOpt.IsNone())
		{
			ImGui.TextWrapped("No Target selected");
			return;
		}
		if (Game.Instance.IsNullOrDestroyed())
		{
			ImGui.TextWrapped("No Game instance");
			return;
		}
		if (Game.Instance.roomProber.IsNullOrDestroyed())
		{
			ImGui.TextWrapped("No RoomProber instance");
			return;
		}
		DevToolEntityTarget.ForSimCell forSimCell = this.targetOpt.Unwrap();
		Option<string> errorForCandidateTarget = DevToolCavity.GetErrorForCandidateTarget(forSimCell);
		if (errorForCandidateTarget.IsSome())
		{
			ImGui.TextWrapped(errorForCandidateTarget.Unwrap());
			return;
		}
		this.Name = string.Format("Cavity Info for: Cell {0}", forSimCell.cellIndex);
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(forSimCell.cellIndex);
		if (cavityForCell.IsNullOrDestroyed())
		{
			ImGui.TextWrapped("No Cavity found");
			return;
		}
		ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
		ImGuiEx.SimpleField("Room Type", cavityForCell.room.IsNullOrDestroyed() ? "<None>" : cavityForCell.room.GetProperName());
		ImGuiEx.SimpleField("Cell Count", cavityForCell.numCells);
		DevToolCavity.DrawKPrefabIdCollection("Creatures", cavityForCell.creatures);
		DevToolCavity.DrawKPrefabIdCollection("Buildings", cavityForCell.buildings);
		DevToolCavity.DrawKPrefabIdCollection("Plants", cavityForCell.plants);
		DevToolCavity.DrawKPrefabIdCollection("Eggs", cavityForCell.eggs);
		if (ImGui.CollapsingHeader("Full CavityInfo Object"))
		{
			ImGuiEx.DrawObject("CavityInfo", cavityForCell, new MemberDrawContext?(new MemberDrawContext(false, true)));
		}
		if (this.shouldDrawBoundingBox)
		{
			Option<ValueTuple<Vector2, Vector2>> screenRect = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(cavityForCell.minX, cavityForCell.minY)).GetScreenRect();
			Option<ValueTuple<Vector2, Vector2>> screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(cavityForCell.maxX, cavityForCell.maxY)).GetScreenRect();
			if (screenRect.IsSome() && screenRect2.IsSome())
			{
				DevToolEntity.DrawBoundingBox(new ValueTuple<Vector2, Vector2>(Vector2.Min(screenRect.Unwrap().Item1, Vector2.Min(screenRect.Unwrap().Item2, Vector2.Min(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2))), Vector2.Max(screenRect.Unwrap().Item1, Vector2.Max(screenRect.Unwrap().Item2, Vector2.Max(screenRect2.Unwrap().Item1, screenRect2.Unwrap().Item2)))), cavityForCell.room.IsNullOrDestroyed() ? "<Room is null>" : cavityForCell.room.GetProperName(), ImGui.IsWindowFocused());
				Option<ValueTuple<Vector2, Vector2>> screenRect3 = forSimCell.GetScreenRect();
				if (screenRect3.IsSome())
				{
					DevToolEntity.DrawBoundingBox(screenRect3.Unwrap(), forSimCell.GetDebugName(), ImGui.IsWindowFocused());
				}
			}
		}
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x000D3E64 File Offset: 0x000D2064
	public static void DrawKPrefabIdCollection(string name, IEnumerable<KPrefabID> kprefabIds)
	{
		name += (kprefabIds.IsNullOrDestroyed() ? " (0)" : string.Format(" ({0})", kprefabIds.Count<KPrefabID>()));
		if (ImGui.CollapsingHeader(name))
		{
			if (kprefabIds.IsNullOrDestroyed())
			{
				ImGui.Text("List is null");
				return;
			}
			if (kprefabIds.Count<KPrefabID>() == 0)
			{
				ImGui.Text("List is empty");
				return;
			}
			foreach (KPrefabID kprefabID in kprefabIds)
			{
				ImGui.Text(kprefabID.ToString());
				ImGui.SameLine();
				if (ImGui.Button(string.Format("DevTool Inspect###ID_Inspect_{0}", kprefabID.GetInstanceID())))
				{
					DevToolSceneInspector.Inspect(kprefabID);
				}
			}
		}
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x000D3F34 File Offset: 0x000D2134
	public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
	{
		if (!(uncastTarget is DevToolEntityTarget.ForSimCell))
		{
			return "Target must be a sim cell";
		}
		DevToolEntityTarget.ForSimCell forSimCell = (DevToolEntityTarget.ForSimCell)uncastTarget;
		if (Game.Instance.IsNullOrDestroyed())
		{
			return "No Game instance found.";
		}
		if (forSimCell.cellIndex < 0 || Grid.CellCount <= forSimCell.cellIndex)
		{
			return string.Format("Found cell index {0} is out of range {1}..{2}", forSimCell.cellIndex, forSimCell.cellIndex, Grid.CellCount);
		}
		if (!Grid.IsValidCell(forSimCell.cellIndex))
		{
			return string.Format("Cell index {0} is invalid", forSimCell.cellIndex);
		}
		return Option.None;
	}

	// Token: 0x040015BE RID: 5566
	private Option<DevToolEntityTarget.ForSimCell> targetOpt;

	// Token: 0x040015BF RID: 5567
	private bool shouldDrawBoundingBox = true;
}
