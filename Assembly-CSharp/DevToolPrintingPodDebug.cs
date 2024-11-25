using System;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000622 RID: 1570
public class DevToolPrintingPodDebug : DevTool
{
	// Token: 0x060026A2 RID: 9890 RVA: 0x000D94C3 File Offset: 0x000D76C3
	protected override void RenderTo(DevPanel panel)
	{
		if (Immigration.Instance != null)
		{
			this.ShowButtons();
			return;
		}
		ImGui.Text("Game not available");
	}

	// Token: 0x060026A3 RID: 9891 RVA: 0x000D94E4 File Offset: 0x000D76E4
	private void ShowButtons()
	{
		if (Components.Telepads.Count == 0)
		{
			ImGui.Text("No printing pods available");
			return;
		}
		ImGui.Text("Time until next print available: " + Mathf.CeilToInt(Immigration.Instance.timeBeforeSpawn).ToString() + "s");
		if (ImGui.Button("Activate now"))
		{
			Immigration.Instance.timeBeforeSpawn = 0f;
		}
		if (ImGui.Button("Shuffle Options"))
		{
			if (ImmigrantScreen.instance.Telepad == null)
			{
				ImmigrantScreen.InitializeImmigrantScreen(Components.Telepads[0]);
				return;
			}
			ImmigrantScreen.instance.DebugShuffleOptions();
		}
	}
}
