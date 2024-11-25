using System;
using ImGuiNET;

// Token: 0x02000624 RID: 1572
public class DevToolSaveGameInfo : DevTool
{
	// Token: 0x060026A8 RID: 9896 RVA: 0x000D97A4 File Offset: 0x000D79A4
	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			ImGui.Text("No game loaded");
			return;
		}
		ImGui.Text("Seed: " + CustomGameSettings.Instance.GetSettingsCoordinate());
		ImGui.Text("Generated: " + Game.Instance.dateGenerated);
		ImGui.Text("DebugWasUsed: " + Game.Instance.debugWasUsed.ToString());
		ImGui.Text("Content Enabled: ");
		foreach (string text in SaveLoader.Instance.GameInfo.dlcIds)
		{
			string str = (text == "") ? "VANILLA_ID" : text;
			ImGui.Text(" - " + str);
		}
		ImGui.PushItemWidth(100f);
		ImGui.NewLine();
		ImGui.Text("Changelists played on");
		ImGui.InputText("Search", ref this.clSearch, 10U);
		ImGui.PopItemWidth();
		foreach (uint num in Game.Instance.changelistsPlayedOn)
		{
			if (this.clSearch.IsNullOrWhiteSpace() || num.ToString().Contains(this.clSearch))
			{
				ImGui.Text(num.ToString());
			}
		}
		ImGui.NewLine();
	}

	// Token: 0x0400161D RID: 5661
	private string clSearch = "";
}
