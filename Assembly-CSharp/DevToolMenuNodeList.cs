using System;
using System.IO;
using ImGuiNET;

// Token: 0x0200061A RID: 1562
public class DevToolMenuNodeList
{
	// Token: 0x06002684 RID: 9860 RVA: 0x000D84FC File Offset: 0x000D66FC
	public DevToolMenuNodeParent AddOrGetParentFor(string childPath)
	{
		string[] array = Path.GetDirectoryName(childPath).Split('/', StringSplitOptions.None);
		string text = "";
		DevToolMenuNodeParent devToolMenuNodeParent = this.root;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string split = array2[i];
			text += devToolMenuNodeParent.GetName();
			IMenuNode menuNode = devToolMenuNodeParent.children.Find((IMenuNode x) => x.GetName() == split);
			DevToolMenuNodeParent devToolMenuNodeParent3;
			if (menuNode != null)
			{
				DevToolMenuNodeParent devToolMenuNodeParent2 = menuNode as DevToolMenuNodeParent;
				if (devToolMenuNodeParent2 == null)
				{
					throw new Exception("Conflict! Both a leaf and parent node exist at path: " + text);
				}
				devToolMenuNodeParent3 = devToolMenuNodeParent2;
			}
			else
			{
				devToolMenuNodeParent3 = new DevToolMenuNodeParent(split);
				devToolMenuNodeParent.AddChild(devToolMenuNodeParent3);
			}
			devToolMenuNodeParent = devToolMenuNodeParent3;
		}
		return devToolMenuNodeParent;
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x000D85A8 File Offset: 0x000D67A8
	public DevToolMenuNodeAction AddAction(string path, System.Action onClickFn)
	{
		DevToolMenuNodeAction devToolMenuNodeAction = new DevToolMenuNodeAction(Path.GetFileName(path), onClickFn);
		this.AddOrGetParentFor(path).AddChild(devToolMenuNodeAction);
		return devToolMenuNodeAction;
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x000D85D0 File Offset: 0x000D67D0
	public void Draw()
	{
		foreach (IMenuNode menuNode in this.root.children)
		{
			menuNode.Draw();
		}
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x000D8628 File Offset: 0x000D6828
	public void DrawFull()
	{
		if (ImGui.BeginMainMenuBar())
		{
			this.Draw();
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x0400160A RID: 5642
	private DevToolMenuNodeParent root = new DevToolMenuNodeParent("<root>");
}
