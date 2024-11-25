using System;
using ImGuiNET;

// Token: 0x02000616 RID: 1558
public class DevToolEntity_SearchGameObjects : DevTool
{
	// Token: 0x06002662 RID: 9826 RVA: 0x000D648D File Offset: 0x000D468D
	public DevToolEntity_SearchGameObjects(Action<DevToolEntityTarget> onSelectionMadeFn)
	{
		this.onSelectionMadeFn = onSelectionMadeFn;
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x000D649C File Offset: 0x000D469C
	protected override void RenderTo(DevPanel panel)
	{
		ImGui.Text("Not implemented yet");
	}

	// Token: 0x040015D9 RID: 5593
	private Action<DevToolEntityTarget> onSelectionMadeFn;
}
