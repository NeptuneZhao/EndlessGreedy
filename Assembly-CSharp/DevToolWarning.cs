using System;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x02000632 RID: 1586
public class DevToolWarning
{
	// Token: 0x0600270A RID: 9994 RVA: 0x000DE1DB File Offset: 0x000DC3DB
	public DevToolWarning()
	{
		this.Name = UI.FRONTEND.DEVTOOLS.TITLE;
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x000DE1F3 File Offset: 0x000DC3F3
	public void DrawMenuBar()
	{
		if (ImGui.BeginMainMenuBar())
		{
			ImGui.Checkbox(this.Name, ref this.ShouldDrawWindow);
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x000DE214 File Offset: 0x000DC414
	public void DrawWindow(out bool isOpen)
	{
		ImGuiWindowFlags flags = ImGuiWindowFlags.None;
		isOpen = true;
		if (ImGui.Begin(this.Name + "###ID_DevToolWarning", ref isOpen, flags))
		{
			if (!isOpen)
			{
				ImGui.End();
				return;
			}
			ImGui.SetWindowSize(new Vector2(500f, 250f));
			ImGui.TextWrapped(UI.FRONTEND.DEVTOOLS.WARNING);
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Spacing();
			ImGui.Checkbox(UI.FRONTEND.DEVTOOLS.DONTSHOW, ref this.showAgain);
			if (ImGui.Button(UI.FRONTEND.DEVTOOLS.BUTTON))
			{
				if (this.showAgain)
				{
					KPlayerPrefs.SetInt("ShowDevtools", 1);
				}
				DevToolManager.Instance.UserAcceptedWarning = true;
				isOpen = false;
			}
			ImGui.End();
		}
	}

	// Token: 0x0400165D RID: 5725
	private bool showAgain;

	// Token: 0x0400165E RID: 5726
	public string Name;

	// Token: 0x0400165F RID: 5727
	public bool ShouldDrawWindow;
}
