using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class DevToolUnlockedIds : DevTool
{
	// Token: 0x060026F3 RID: 9971 RVA: 0x000DDB07 File Offset: 0x000DBD07
	public DevToolUnlockedIds()
	{
		this.RequiresGameRunning = true;
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x000DDB2C File Offset: 0x000DBD2C
	protected override void RenderTo(DevPanel panel)
	{
		bool flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper;
		this.GetUnlocks().Deconstruct(out flag, out unlocksWrapper);
		bool flag2 = flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper2 = unlocksWrapper;
		if (!flag2)
		{
			ImGui.Text("Couldn't access global unlocks");
			return;
		}
		if (ImGui.TreeNode("Help"))
		{
			ImGui.TextWrapped("This is a list of global unlocks that are persistant across saves. Changes made here will be saved to disk immediately.");
			ImGui.Spacing();
			ImGui.TextWrapped("NOTE: It may be necessary to relaunch the game after modifying unlocks in order for systems to respond.");
			ImGui.TreePop();
		}
		ImGui.Spacing();
		ImGuiEx.InputFilter("Filter", ref this.filterForUnlockIds, 50U);
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.ScrollY;
		if (ImGui.BeginTable("ID_unlockIds", 2, flags))
		{
			ImGui.TableSetupScrollFreeze(2, 2);
			ImGui.TableSetupColumn("Unlock ID");
			ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableHeadersRow();
			ImGui.PushID("ID_row_add_new");
			ImGui.TableNextRow();
			ImGui.TableSetColumnIndex(0);
			ImGui.InputText("", ref this.unlockIdToAdd, 50U);
			ImGui.TableSetColumnIndex(1);
			if (ImGui.Button("Add"))
			{
				unlocksWrapper2.AddId(this.unlockIdToAdd);
				global::Debug.Log("[Added unlock id] " + this.unlockIdToAdd);
				this.unlockIdToAdd = "";
			}
			ImGui.PopID();
			int num = 0;
			foreach (string text in unlocksWrapper2.GetAllIds())
			{
				string text2 = (text == null) ? "<<null>>" : ("\"" + text + "\"");
				if (text2.ToLower().Contains(this.filterForUnlockIds.ToLower()))
				{
					ImGui.TableNextRow();
					ImGui.PushID(string.Format("ID_row_{0}", num++));
					ImGui.TableSetColumnIndex(0);
					ImGui.Text(text2);
					ImGui.TableSetColumnIndex(1);
					if (ImGui.Button("Copy"))
					{
						GUIUtility.systemCopyBuffer = text;
						global::Debug.Log("[Copied to clipboard] " + text);
					}
					ImGui.SameLine();
					if (ImGui.Button("Remove"))
					{
						unlocksWrapper2.RemoveId(text);
						global::Debug.Log("[Removed unlock id] " + text);
					}
					ImGui.PopID();
				}
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x000DDD58 File Offset: 0x000DBF58
	private Option<DevToolUnlockedIds.UnlocksWrapper> GetUnlocks()
	{
		if (App.IsExiting)
		{
			return Option.None;
		}
		if (Game.Instance == null || !Game.Instance)
		{
			return Option.None;
		}
		if (Game.Instance.unlocks == null)
		{
			return Option.None;
		}
		return Option.Some<DevToolUnlockedIds.UnlocksWrapper>(new DevToolUnlockedIds.UnlocksWrapper(Game.Instance.unlocks));
	}

	// Token: 0x04001658 RID: 5720
	private string filterForUnlockIds = "";

	// Token: 0x04001659 RID: 5721
	private string unlockIdToAdd = "";

	// Token: 0x0200140D RID: 5133
	public readonly struct UnlocksWrapper
	{
		// Token: 0x06008931 RID: 35121 RVA: 0x0032FC5F File Offset: 0x0032DE5F
		public UnlocksWrapper(Unlocks unlocks)
		{
			this.unlocks = unlocks;
		}

		// Token: 0x06008932 RID: 35122 RVA: 0x0032FC68 File Offset: 0x0032DE68
		public void AddId(string unlockId)
		{
			this.unlocks.Unlock(unlockId, true);
		}

		// Token: 0x06008933 RID: 35123 RVA: 0x0032FC77 File Offset: 0x0032DE77
		public void RemoveId(string unlockId)
		{
			this.unlocks.Lock(unlockId);
		}

		// Token: 0x06008934 RID: 35124 RVA: 0x0032FC85 File Offset: 0x0032DE85
		public IEnumerable<string> GetAllIds()
		{
			return from s in this.unlocks.GetAllUnlockedIds()
			orderby s
			select s;
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06008935 RID: 35125 RVA: 0x0032FCB6 File Offset: 0x0032DEB6
		public int Count
		{
			get
			{
				return this.unlocks.GetAllUnlockedIds().Count;
			}
		}

		// Token: 0x040068BA RID: 26810
		public readonly Unlocks unlocks;
	}
}
