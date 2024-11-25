﻿using System;
using ImGuiNET;

// Token: 0x02000610 RID: 1552
public class DevToolDLCManager : DevTool
{
	// Token: 0x0600263E RID: 9790 RVA: 0x000D4BB0 File Offset: 0x000D2DB0
	protected override void RenderTo(DevPanel panel)
	{
		string name = DistributionPlatform.Inst.Name;
		if (!DistributionPlatform.Initialized)
		{
			ImGui.Text("Failed to initialize " + name);
			return;
		}
		ImGui.Text("Active content letters: " + DlcManager.GetActiveContentLetters());
		ImGui.Separator();
		foreach (string text in DlcManager.RELEASED_VERSIONS)
		{
			if (!text.IsNullOrWhiteSpace())
			{
				ImGui.Text(text);
				ImGui.SameLine();
				bool flag = DlcManager.IsContentSubscribed(text);
				if (ImGui.Checkbox("Enabled ", ref flag))
				{
					DlcManager.ToggleDLC(text);
				}
				ImGui.SameLine();
				bool flag2 = DistributionPlatform.Inst.IsDLCSubscribed(text);
				if (ImGui.Checkbox("Subscribed ", ref flag2))
				{
					DistributionPlatform.Inst.ToggleDLCSubscription(text);
				}
			}
		}
	}
}
