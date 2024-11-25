using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x0200061C RID: 1564
public class DevToolMenuNodeParent : IMenuNode
{
	// Token: 0x0600268B RID: 9867 RVA: 0x000D8654 File Offset: 0x000D6854
	public DevToolMenuNodeParent(string name)
	{
		this.name = name;
		this.children = new List<IMenuNode>();
	}

	// Token: 0x0600268C RID: 9868 RVA: 0x000D866E File Offset: 0x000D686E
	public void AddChild(IMenuNode menuNode)
	{
		this.children.Add(menuNode);
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x000D867C File Offset: 0x000D687C
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x000D8684 File Offset: 0x000D6884
	public void Draw()
	{
		if (ImGui.BeginMenu(this.name))
		{
			foreach (IMenuNode menuNode in this.children)
			{
				menuNode.Draw();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x0400160B RID: 5643
	public string name;

	// Token: 0x0400160C RID: 5644
	public List<IMenuNode> children;
}
