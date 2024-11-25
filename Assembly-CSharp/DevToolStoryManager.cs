using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x0200062C RID: 1580
public class DevToolStoryManager : DevTool
{
	// Token: 0x060026E5 RID: 9957 RVA: 0x000DD4C8 File Offset: 0x000DB6C8
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.CollapsingHeader("Story Instance Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.DrawStoryInstanceData();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Story Telemetry Data", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.DrawTelemetryData();
		}
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x000DD4F8 File Offset: 0x000DB6F8
	private void DrawStoryInstanceData()
	{
		if (StoryManager.Instance == null)
		{
			ImGui.Text("Couldn't find StoryManager instance");
			return;
		}
		ImGui.Text(string.Format("Stories (count: {0})", StoryManager.Instance.GetStoryInstances().Count));
		string str = (StoryManager.Instance.GetHighestCoordinate() == -2) ? "Before stories" : StoryManager.Instance.GetHighestCoordinate().ToString();
		ImGui.Text("Highest generated: " + str);
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in StoryManager.Instance.GetStoryInstances())
		{
			ImGui.Text(" - " + keyValuePair.Value.storyId + ": " + keyValuePair.Value.CurrentState.ToString());
		}
		if (StoryManager.Instance.GetStoryInstances().Count == 0)
		{
			ImGui.Text(" - No stories");
		}
	}

	// Token: 0x060026E7 RID: 9959 RVA: 0x000DD614 File Offset: 0x000DB814
	private void DrawTelemetryData()
	{
		ImGuiEx.DrawObjectTable<StoryManager.StoryTelemetry>("ID_telemetry", StoryManager.GetTelemetry(), null);
	}
}
