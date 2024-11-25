using System;
using ImGuiNET;

// Token: 0x02000620 RID: 1568
public class DevToolPOITechUnlocks : DevTool
{
	// Token: 0x0600269F RID: 9887 RVA: 0x000D8FC8 File Offset: 0x000D71C8
	protected override void RenderTo(DevPanel panel)
	{
		if (Research.Instance == null)
		{
			return;
		}
		foreach (TechItem techItem in Db.Get().TechItems.resources)
		{
			if (techItem.isPOIUnlock)
			{
				ImGui.Text(techItem.Id);
				ImGui.SameLine();
				bool flag = techItem.IsComplete();
				if (ImGui.Checkbox("Unlocked ", ref flag))
				{
					techItem.POIUnlocked();
				}
			}
		}
	}
}
