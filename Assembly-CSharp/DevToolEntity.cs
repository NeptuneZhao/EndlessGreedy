﻿using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public class DevToolEntity : DevTool
{
	// Token: 0x0600263F RID: 9791 RVA: 0x000D4C90 File Offset: 0x000D2E90
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("New Window"))
			{
				DevToolUtil.Open(new DevToolEntity());
			}
			ImGui.EndMenuBar();
		}
		ImGui.Text(this.currentTargetOpt.IsNone() ? "Pick target:" : "Change target:");
		ImGui.SameLine();
		if (ImGui.Button("Eyedrop"))
		{
			panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget result)
			{
				this.currentTargetOpt = result;
			}, null));
		}
		ImGui.SameLine();
		if (ImGui.Button("Search GameObjects (NOT implemented)"))
		{
			panel.PushDevTool(new DevToolEntity_SearchGameObjects(delegate(DevToolEntityTarget result)
			{
				this.currentTargetOpt = result;
			}));
		}
		if (this.GetInGameSelectedEntity().IsSome())
		{
			ImGui.SameLine();
			if (ImGui.Button("\"" + this.GetInGameSelectedEntity().Unwrap().name + "\""))
			{
				this.currentTargetOpt = new DevToolEntityTarget.ForWorldGameObject(this.GetInGameSelectedEntity().Unwrap());
			}
		}
		ImGui.Separator();
		ImGui.Spacing();
		if (this.currentTargetOpt.IsNone())
		{
			this.Name = "Entity";
			ImGui.Text("<nothing selected>");
		}
		else
		{
			this.Name = "Entity: " + this.currentTargetOpt.Unwrap().ToString();
			this.Name = "EntityType: " + this.currentTargetOpt.Unwrap().GetType().FullName.Substring("For".Length);
			ImGuiEx.SimpleField("Entity Name", this.currentTargetOpt.Unwrap().ToString());
		}
		ImGui.Spacing();
		ImGui.Separator();
		ImGui.Spacing();
		if (this.currentTargetOpt.IsNone())
		{
			return;
		}
		DevToolEntityTarget devToolEntityTarget = this.currentTargetOpt.Unwrap();
		DevToolEntityTarget.ForUIGameObject forUIGameObject = devToolEntityTarget as DevToolEntityTarget.ForUIGameObject;
		Option<GameObject> option;
		if (forUIGameObject != null)
		{
			option = forUIGameObject.gameObject;
		}
		else
		{
			DevToolEntityTarget.ForWorldGameObject forWorldGameObject = devToolEntityTarget as DevToolEntityTarget.ForWorldGameObject;
			if (forWorldGameObject != null)
			{
				option = forWorldGameObject.gameObject;
			}
			else
			{
				option = Option.None;
			}
		}
		if (ImGui.CollapsingHeader("Actions", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
			if (option.IsSome())
			{
				GameObject gameObject = option.Unwrap();
				if (ImGui.Button(string.Format("Inspect GameObject in DevTools###ID_InspectInGame_{0}", gameObject.GetInstanceID())))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				WildnessMonitor.Instance smi = gameObject.GetSMI<WildnessMonitor.Instance>();
				if (smi.IsNullOrDestroyed())
				{
					ImGuiEx.Button("Taming: Covert to Tamed", "No WildnessMonitor.Instance found on the selected GameObject");
				}
				else
				{
					WildnessMonitor wildnessMonitor = (WildnessMonitor)smi.GetStateMachine();
					if (smi.GetCurrentState() != wildnessMonitor.tame)
					{
						if (ImGui.Button("Taming: Convert to Tamed"))
						{
							smi.wildness.SetValue(0f);
							smi.GoTo(wildnessMonitor.tame);
						}
					}
					else if (ImGui.Button("Taming: Convert to Untamed"))
					{
						smi.wildness.value = smi.wildness.GetMax();
						smi.GoTo(wildnessMonitor.wild);
					}
				}
			}
			ImGui.Unindent();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Related DevTools", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			if (ImGuiEx.Button("Debug Status Items", DevToolStatusItems.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolStatusItems((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug Cavity", DevToolCavity.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolCavity((DevToolEntityTarget.ForSimCell)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug GoTo", DevToolEntity_DebugGoTo.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_DebugGoTo((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug RanchStation", DevToolEntity_RanchStation.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_RanchStation((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			ImGui.Unindent();
		}
		if (this.shouldDrawBoundingBox)
		{
			Option<ValueTuple<Vector2, Vector2>> screenRect = devToolEntityTarget.GetScreenRect();
			if (screenRect.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), devToolEntityTarget.GetDebugName(), ImGui.IsWindowFocused());
			}
		}
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x000D50AC File Offset: 0x000D32AC
	public Option<GameObject> GetInGameSelectedEntity()
	{
		if (SelectTool.Instance == null)
		{
			return Option.None;
		}
		KSelectable selected = SelectTool.Instance.selected;
		if (selected.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return selected.gameObject;
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000D50FC File Offset: 0x000D32FC
	public static string GetNameFor(GameObject gameObject)
	{
		if (gameObject.IsNullOrDestroyed())
		{
			return "<null or destroyed GameObject>";
		}
		return string.Concat(new string[]
		{
			"\"",
			UI.StripLinkFormatting(gameObject.name),
			"\" [0x",
			gameObject.GetInstanceID().ToString("X"),
			"]"
		});
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000D5160 File Offset: 0x000D3360
	public static Vector2 GetPositionFor(GameObject gameObject)
	{
		if (Camera.main != null)
		{
			Camera main = Camera.main;
			Vector2 vector = main.WorldToScreenPoint(gameObject.transform.position);
			vector.y = (float)main.pixelHeight - vector.y;
			return vector;
		}
		return Vector2.zero;
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000D51B4 File Offset: 0x000D33B4
	public static Vector2 GetScreenPosition(Vector3 pos)
	{
		if (Camera.main != null)
		{
			Camera main = Camera.main;
			Vector2 vector = main.WorldToScreenPoint(pos);
			vector.y = (float)main.pixelHeight - vector.y;
			return vector;
		}
		return Vector2.zero;
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x000D5200 File Offset: 0x000D3400
	public static void DrawBoundingBox([TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})] ValueTuple<Vector2, Vector2> screenRect, string name, bool isFocused)
	{
		if (isFocused)
		{
			DevToolEntity.DrawScreenRect(screenRect, name, new Color(1f, 0f, 0f, 1f), new Color(1f, 0f, 0f, 0.3f), default(Option<DevToolUtil.TextAlignment>));
			return;
		}
		DevToolEntity.DrawScreenRect(screenRect, Option.None, new Color(0.9f, 0f, 0f, 0.6f), default(Option<Color>), default(Option<DevToolUtil.TextAlignment>));
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x000D52A4 File Offset: 0x000D34A4
	public unsafe static void DrawScreenRect([TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})] ValueTuple<Vector2, Vector2> screenRect, Option<string> text = default(Option<string>), Option<Color> outlineColor = default(Option<Color>), Option<Color> fillColor = default(Option<Color>), Option<DevToolUtil.TextAlignment> alignment = default(Option<DevToolUtil.TextAlignment>))
	{
		Vector2 vector = Vector2.Min(screenRect.Item1, screenRect.Item2);
		Vector2 vector2 = Vector2.Max(screenRect.Item1, screenRect.Item2);
		ImGui.GetBackgroundDrawList().AddRect(vector, vector2, ImGui.GetColorU32(outlineColor.UnwrapOr(Color.red, null)), 0f, ImDrawFlags.None, 4f);
		ImGui.GetBackgroundDrawList().AddRectFilled(vector, vector2, ImGui.GetColorU32(fillColor.UnwrapOr(Color.clear, null)));
		float font_size = 30f;
		if (text.IsSome())
		{
			Vector2 pos = new Vector2(vector2.x, vector.y) + new Vector2(15f, 0f);
			if (alignment.HasValue)
			{
				font_size = *ImGui.GetFont().FontSize;
				Vector2 vector3 = ImGui.CalcTextSize(text.Unwrap());
				if (alignment == DevToolUtil.TextAlignment.Center)
				{
					Vector2 vector4 = vector2 - vector;
					pos.x = vector.x + (vector4.x - vector3.x) * 0.5f;
					pos.y = vector.y + (vector4.y - vector3.y) * 0.5f;
				}
			}
			ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), font_size, pos, ImGui.GetColorU32(Color.white), text.Unwrap());
		}
	}

	// Token: 0x040015CB RID: 5579
	private Option<DevToolEntityTarget> currentTargetOpt;

	// Token: 0x040015CC RID: 5580
	private bool shouldDrawBoundingBox = true;
}
