using System;
using ImGuiNET;

// Token: 0x02000607 RID: 1543
public abstract class DevTool
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x0600260F RID: 9743 RVA: 0x000D2D60 File Offset: 0x000D0F60
	// (remove) Token: 0x06002610 RID: 9744 RVA: 0x000D2D98 File Offset: 0x000D0F98
	public event System.Action OnInit;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06002611 RID: 9745 RVA: 0x000D2DD0 File Offset: 0x000D0FD0
	// (remove) Token: 0x06002612 RID: 9746 RVA: 0x000D2E08 File Offset: 0x000D1008
	public event System.Action OnUpdate;

	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06002613 RID: 9747 RVA: 0x000D2E40 File Offset: 0x000D1040
	// (remove) Token: 0x06002614 RID: 9748 RVA: 0x000D2E78 File Offset: 0x000D1078
	public event System.Action OnUninit;

	// Token: 0x06002615 RID: 9749 RVA: 0x000D2EAD File Offset: 0x000D10AD
	public DevTool()
	{
		this.Name = DevToolUtil.GenerateDevToolName(this);
	}

	// Token: 0x06002616 RID: 9750 RVA: 0x000D2EC1 File Offset: 0x000D10C1
	public void DoImGui(DevPanel panel)
	{
		if (this.RequiresGameRunning && Game.Instance == null)
		{
			ImGui.Text("Game must be loaded to use this devtool.");
			return;
		}
		this.RenderTo(panel);
	}

	// Token: 0x06002617 RID: 9751 RVA: 0x000D2EEA File Offset: 0x000D10EA
	public void ClosePanel()
	{
		this.isRequestingToClosePanel = true;
	}

	// Token: 0x06002618 RID: 9752
	protected abstract void RenderTo(DevPanel panel);

	// Token: 0x06002619 RID: 9753 RVA: 0x000D2EF3 File Offset: 0x000D10F3
	public void Internal_TryInit()
	{
		if (this.didInit)
		{
			return;
		}
		this.didInit = true;
		if (this.OnInit != null)
		{
			this.OnInit();
		}
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x000D2F18 File Offset: 0x000D1118
	public void Internal_Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x0600261B RID: 9755 RVA: 0x000D2F2D File Offset: 0x000D112D
	public void Internal_Uninit()
	{
		if (this.OnUninit != null)
		{
			this.OnUninit();
		}
	}

	// Token: 0x040015B1 RID: 5553
	public string Name;

	// Token: 0x040015B2 RID: 5554
	public bool RequiresGameRunning;

	// Token: 0x040015B3 RID: 5555
	public bool isRequestingToClosePanel;

	// Token: 0x040015B4 RID: 5556
	public ImGuiWindowFlags drawFlags;

	// Token: 0x040015B8 RID: 5560
	private bool didInit;
}
