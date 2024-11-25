using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000605 RID: 1541
public class DevPanel
{
	// Token: 0x170001DD RID: 477
	// (get) Token: 0x060025EF RID: 9711 RVA: 0x000D25C7 File Offset: 0x000D07C7
	// (set) Token: 0x060025F0 RID: 9712 RVA: 0x000D25CF File Offset: 0x000D07CF
	public bool isRequestingToClose { get; private set; }

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060025F1 RID: 9713 RVA: 0x000D25D8 File Offset: 0x000D07D8
	// (set) Token: 0x060025F2 RID: 9714 RVA: 0x000D25E0 File Offset: 0x000D07E0
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowPosition { get; private set; }

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060025F3 RID: 9715 RVA: 0x000D25E9 File Offset: 0x000D07E9
	// (set) Token: 0x060025F4 RID: 9716 RVA: 0x000D25F1 File Offset: 0x000D07F1
	public Option<ValueTuple<Vector2, ImGuiCond>> nextImGuiWindowSize { get; private set; }

	// Token: 0x060025F5 RID: 9717 RVA: 0x000D25FC File Offset: 0x000D07FC
	public DevPanel(DevTool devTool, DevPanelList manager)
	{
		this.manager = manager;
		this.devTools = new List<DevTool>();
		this.devTools.Add(devTool);
		this.currentDevToolIndex = 0;
		this.initialDevToolType = devTool.GetType();
		manager.Internal_InitPanelId(this.initialDevToolType, out this.uniquePanelId, out this.idPostfixNumber);
	}

	// Token: 0x060025F6 RID: 9718 RVA: 0x000D2658 File Offset: 0x000D0858
	public void PushValue<T>(T value) where T : class
	{
		this.PushDevTool(new DevToolObjectViewer<T>(() => value));
	}

	// Token: 0x060025F7 RID: 9719 RVA: 0x000D2689 File Offset: 0x000D0889
	public void PushValue<T>(Func<T> value)
	{
		this.PushDevTool(new DevToolObjectViewer<T>(value));
	}

	// Token: 0x060025F8 RID: 9720 RVA: 0x000D2697 File Offset: 0x000D0897
	public void PushDevTool<T>() where T : DevTool, new()
	{
		this.PushDevTool(Activator.CreateInstance<T>());
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x000D26AC File Offset: 0x000D08AC
	public void PushDevTool(DevTool devTool)
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			this.manager.AddPanelFor(devTool);
			return;
		}
		for (int i = this.devTools.Count - 1; i > this.currentDevToolIndex; i--)
		{
			this.devTools[i].Internal_Uninit();
			this.devTools.RemoveAt(i);
		}
		this.devTools.Add(devTool);
		this.currentDevToolIndex = this.devTools.Count - 1;
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x000D272C File Offset: 0x000D092C
	public bool NavGoBack()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x000D275C File Offset: 0x000D095C
	public bool NavGoForward()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(1);
		if (option.IsNone())
		{
			return false;
		}
		this.currentDevToolIndex = option.Unwrap();
		return true;
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x000D278A File Offset: 0x000D098A
	public DevTool GetCurrentDevTool()
	{
		return this.devTools[this.currentDevToolIndex];
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x000D27A0 File Offset: 0x000D09A0
	public Option<int> TryGetDevToolIndexByOffset(int offsetFromCurrentIndex)
	{
		int num = this.currentDevToolIndex + offsetFromCurrentIndex;
		if (num < 0)
		{
			return Option.None;
		}
		if (num >= this.devTools.Count)
		{
			return Option.None;
		}
		return num;
	}

	// Token: 0x060025FE RID: 9726 RVA: 0x000D27E4 File Offset: 0x000D09E4
	public void RenderPanel()
	{
		DevTool currentDevTool = this.GetCurrentDevTool();
		currentDevTool.Internal_TryInit();
		if (currentDevTool.isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
			return;
		}
		ImGuiWindowFlags flags;
		this.ConfigureImGuiWindowFor(currentDevTool, out flags);
		currentDevTool.Internal_Update();
		bool flag = true;
		if (ImGui.Begin(currentDevTool.Name + "###ID_" + this.uniquePanelId, ref flag, flags))
		{
			if (!flag)
			{
				this.isRequestingToClose = true;
				ImGui.End();
				return;
			}
			if (ImGui.BeginMenuBar())
			{
				this.DrawNavigation();
				ImGui.SameLine(0f, 20f);
				this.DrawMenuBarContents();
				ImGui.EndMenuBar();
			}
			currentDevTool.DoImGui(this);
			if (this.GetCurrentDevTool() != currentDevTool)
			{
				ImGui.SetScrollY(0f);
			}
		}
		ImGui.End();
		if (this.GetCurrentDevTool().isRequestingToClosePanel)
		{
			this.isRequestingToClose = true;
		}
	}

	// Token: 0x060025FF RID: 9727 RVA: 0x000D28AC File Offset: 0x000D0AAC
	private void DrawNavigation()
	{
		Option<int> option = this.TryGetDevToolIndexByOffset(-1);
		if (ImGuiEx.Button(" < ", option.IsSome()))
		{
			this.currentDevToolIndex = option.Unwrap();
		}
		if (option.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go back to " + this.devTools[option.Unwrap()].Name);
		}
		else
		{
			ImGuiEx.TooltipForPrevious("Go back");
		}
		ImGui.SameLine(0f, 5f);
		Option<int> option2 = this.TryGetDevToolIndexByOffset(1);
		if (ImGuiEx.Button(" > ", option2.IsSome()))
		{
			this.currentDevToolIndex = option2.Unwrap();
		}
		if (option2.IsSome())
		{
			ImGuiEx.TooltipForPrevious("Go forward to " + this.devTools[option2.Unwrap()].Name);
			return;
		}
		ImGuiEx.TooltipForPrevious("Go forward");
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x000D298D File Offset: 0x000D0B8D
	private void DrawMenuBarContents()
	{
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x000D2990 File Offset: 0x000D0B90
	private void ConfigureImGuiWindowFor(DevTool currentDevTool, out ImGuiWindowFlags drawFlags)
	{
		drawFlags = (ImGuiWindowFlags.MenuBar | currentDevTool.drawFlags);
		if (this.nextImGuiWindowPosition.HasValue)
		{
			ValueTuple<Vector2, ImGuiCond> value = this.nextImGuiWindowPosition.Value;
			Vector2 item = value.Item1;
			ImGuiCond item2 = value.Item2;
			ImGui.SetNextWindowPos(item, item2);
			this.nextImGuiWindowPosition = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
		if (this.nextImGuiWindowSize.HasValue)
		{
			Vector2 item3 = this.nextImGuiWindowSize.Value.Item1;
			ImGui.SetNextWindowSize(item3);
			this.nextImGuiWindowSize = default(Option<ValueTuple<Vector2, ImGuiCond>>);
		}
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x000D2A27 File Offset: 0x000D0C27
	public void SetPosition(Vector2 position, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowPosition = new ValueTuple<Vector2, ImGuiCond>(position, condition);
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x000D2A3B File Offset: 0x000D0C3B
	public void SetSize(Vector2 size, ImGuiCond condition = ImGuiCond.None)
	{
		this.nextImGuiWindowSize = new ValueTuple<Vector2, ImGuiCond>(size, condition);
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x000D2A4F File Offset: 0x000D0C4F
	public void Close()
	{
		this.isRequestingToClose = true;
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x000D2A58 File Offset: 0x000D0C58
	public void Internal_Uninit()
	{
		foreach (DevTool devTool in this.devTools)
		{
			devTool.Internal_Uninit();
		}
	}

	// Token: 0x040015A9 RID: 5545
	public readonly string uniquePanelId;

	// Token: 0x040015AA RID: 5546
	public readonly DevPanelList manager;

	// Token: 0x040015AB RID: 5547
	public readonly Type initialDevToolType;

	// Token: 0x040015AC RID: 5548
	public readonly uint idPostfixNumber;

	// Token: 0x040015AD RID: 5549
	private List<DevTool> devTools;

	// Token: 0x040015AE RID: 5550
	private int currentDevToolIndex;
}
