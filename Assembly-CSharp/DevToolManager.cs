using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000619 RID: 1561
public class DevToolManager
{
	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06002674 RID: 9844 RVA: 0x000D7F15 File Offset: 0x000D6115
	public bool Show
	{
		get
		{
			return this.showImGui;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06002675 RID: 9845 RVA: 0x000D7F1D File Offset: 0x000D611D
	private bool quickDevEnabled
	{
		get
		{
			return DebugHandler.enabled && GenericGameSettings.instance.quickDevTools;
		}
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x000D7F34 File Offset: 0x000D6134
	public DevToolManager()
	{
		DevToolManager.Instance = this;
		this.RegisterDevTool<DevToolSimDebug>("Debuggers/Sim Debug");
		this.RegisterDevTool<DevToolStateMachineDebug>("Debuggers/State Machine");
		this.RegisterDevTool<DevToolSaveGameInfo>("Debuggers/Save Game Info");
		this.RegisterDevTool<DevToolPerformanceInfo>("Debuggers/Performance Info");
		this.RegisterDevTool<DevToolPrintingPodDebug>("Debuggers/Printing Pod Debug");
		this.RegisterDevTool<DevToolBigBaseMutations>("Debuggers/Big Base Mutation Utilities");
		this.RegisterDevTool<DevToolNavGrid>("Debuggers/Nav Grid");
		this.RegisterDevTool<DevToolResearchDebugger>("Debuggers/Research");
		this.RegisterDevTool<DevToolStatusItems>("Debuggers/StatusItems");
		this.RegisterDevTool<DevToolUI>("Debuggers/UI");
		this.RegisterDevTool<DevToolUnlockedIds>("Debuggers/UnlockedIds List");
		this.RegisterDevTool<DevToolStringsTable>("Debuggers/StringsTable");
		this.RegisterDevTool<DevToolChoreDebugger>("Debuggers/Chore");
		this.RegisterDevTool<DevToolBatchedAnimDebug>("Debuggers/Batched Anim");
		this.RegisterDevTool<DevTool_StoryTraits_Reveal>("Debuggers/Story Traits Reveal");
		this.RegisterDevTool<DevTool_StoryTrait_CritterManipulator>("Debuggers/Story Trait - Critter Manipulator");
		this.RegisterDevTool<DevToolAnimEventManager>("Debuggers/Anim Event Manager");
		this.RegisterDevTool<DevToolSceneBrowser>("Scene/Browser");
		this.RegisterDevTool<DevToolSceneInspector>("Scene/Inspector");
		this.menuNodes.AddAction("Help/" + UI.FRONTEND.DEVTOOLS.TITLE.text, delegate
		{
			this.warning.ShouldDrawWindow = true;
		});
		this.RegisterDevTool<DevToolCommandPalette>("Help/Command Palette");
		this.RegisterAdditionalDevToolsByReflection();
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x000D809D File Offset: 0x000D629D
	public void Init()
	{
		this.UserAcceptedWarning = (KPlayerPrefs.GetInt("ShowDevtools", 0) == 1);
	}

	// Token: 0x06002678 RID: 9848 RVA: 0x000D80B4 File Offset: 0x000D62B4
	private void RegisterDevTool<T>(string location) where T : DevTool, new()
	{
		this.menuNodes.AddAction(location, delegate
		{
			this.panels.AddPanelFor<T>();
		});
		this.dontAutomaticallyRegisterTypes.Add(typeof(T));
		this.devToolNameDict[typeof(T)] = Path.GetFileName(location);
	}

	// Token: 0x06002679 RID: 9849 RVA: 0x000D810C File Offset: 0x000D630C
	private void RegisterAdditionalDevToolsByReflection()
	{
		using (List<Type>.Enumerator enumerator = ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Type type = enumerator.Current;
				if (!type.IsAbstract && !this.dontAutomaticallyRegisterTypes.Contains(type) && ReflectionUtil.HasDefaultConstructor(type))
				{
					this.menuNodes.AddAction("Debuggers/" + DevToolUtil.GenerateDevToolName(type), delegate
					{
						this.panels.AddPanelFor((DevTool)Activator.CreateInstance(type));
					});
				}
			}
		}
	}

	// Token: 0x0600267A RID: 9850 RVA: 0x000D81C8 File Offset: 0x000D63C8
	public void UpdateShouldShowTools()
	{
		if (!DebugHandler.enabled)
		{
			this.showImGui = false;
			return;
		}
		bool flag = Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));
		if (!this.toggleKeyWasDown && flag)
		{
			this.showImGui = !this.showImGui;
		}
		this.toggleKeyWasDown = flag;
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x000D8230 File Offset: 0x000D6430
	public void UpdateTools()
	{
		if (!DebugHandler.enabled)
		{
			return;
		}
		if (this.showImGui)
		{
			if (this.warning.ShouldDrawWindow)
			{
				this.warning.DrawWindow(out this.warning.ShouldDrawWindow);
			}
			if (!this.UserAcceptedWarning)
			{
				this.warning.DrawMenuBar();
			}
			else
			{
				this.DrawMenu();
				this.panels.Render();
				if (this.showImguiState)
				{
					if (ImGui.Begin("ImGui state", ref this.showImguiState))
					{
						ImGui.Checkbox("ImGui.GetIO().WantCaptureMouse", ImGui.GetIO().WantCaptureMouse);
						ImGui.Checkbox("ImGui.GetIO().WantCaptureKeyboard", ImGui.GetIO().WantCaptureKeyboard);
					}
					ImGui.End();
				}
				if (this.showImguiDemo)
				{
					ImGui.ShowDemoWindow(ref this.showImguiDemo);
				}
			}
		}
		this.UpdateConsumingGameInputs();
		this.UpdateShortcuts();
	}

	// Token: 0x0600267C RID: 9852 RVA: 0x000D8307 File Offset: 0x000D6507
	private void UpdateShortcuts()
	{
		if ((this.showImGui || this.quickDevEnabled) && this.UserAcceptedWarning)
		{
			this.<UpdateShortcuts>g__DoUpdate|26_0();
		}
	}

	// Token: 0x0600267D RID: 9853 RVA: 0x000D8328 File Offset: 0x000D6528
	private void DrawMenu()
	{
		this.menuFontSize.InitializeIfNeeded();
		if (ImGui.BeginMainMenuBar())
		{
			this.menuNodes.Draw();
			this.menuFontSize.DrawMenu();
			if (ImGui.BeginMenu("IMGUI"))
			{
				ImGui.Checkbox("ImGui state", ref this.showImguiState);
				ImGui.Checkbox("ImGui Demo", ref this.showImguiDemo);
				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x000D8398 File Offset: 0x000D6598
	private unsafe void UpdateConsumingGameInputs()
	{
		this.doesImGuiWantInput = false;
		if (this.showImGui)
		{
			this.doesImGuiWantInput = (*ImGui.GetIO().WantCaptureMouse || *ImGui.GetIO().WantCaptureKeyboard);
			if (!this.prevDoesImGuiWantInput && this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0();
			}
			if (this.prevDoesImGuiWantInput && !this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
			}
		}
		if (this.prevShowImGui && this.prevDoesImGuiWantInput && !this.showImGui)
		{
			DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
		}
		this.prevShowImGui = this.showImGui;
		this.prevDoesImGuiWantInput = this.doesImGuiWantInput;
		KInputManager.devToolFocus = (this.showImGui && this.doesImGuiWantInput);
	}

	// Token: 0x06002681 RID: 9857 RVA: 0x000D846C File Offset: 0x000D666C
	[CompilerGenerated]
	private void <UpdateShortcuts>g__DoUpdate|26_0()
	{
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Space))
		{
			DevToolCommandPalette.Init();
			this.showImGui = true;
		}
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			DevToolUI.PingHoveredObject();
			this.showImGui = true;
		}
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x000D84BC File Offset: 0x000D66BC
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0()
	{
		UnityMouseCatcherUI.SetEnabled(true);
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.GetControllerCount(); i++)
		{
			inputManager.GetController(i).HandleCancelInput();
		}
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x000D84F2 File Offset: 0x000D66F2
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputExitImGui|28_1()
	{
		UnityMouseCatcherUI.SetEnabled(false);
	}

	// Token: 0x040015FA RID: 5626
	public const string SHOW_DEVTOOLS = "ShowDevtools";

	// Token: 0x040015FB RID: 5627
	public static DevToolManager Instance;

	// Token: 0x040015FC RID: 5628
	private bool toggleKeyWasDown;

	// Token: 0x040015FD RID: 5629
	private bool showImGui;

	// Token: 0x040015FE RID: 5630
	private bool prevShowImGui;

	// Token: 0x040015FF RID: 5631
	private bool doesImGuiWantInput;

	// Token: 0x04001600 RID: 5632
	private bool prevDoesImGuiWantInput;

	// Token: 0x04001601 RID: 5633
	private bool showImguiState;

	// Token: 0x04001602 RID: 5634
	private bool showImguiDemo;

	// Token: 0x04001603 RID: 5635
	public bool UserAcceptedWarning;

	// Token: 0x04001604 RID: 5636
	private DevToolWarning warning = new DevToolWarning();

	// Token: 0x04001605 RID: 5637
	private DevToolMenuFontSize menuFontSize = new DevToolMenuFontSize();

	// Token: 0x04001606 RID: 5638
	public DevPanelList panels = new DevPanelList();

	// Token: 0x04001607 RID: 5639
	public DevToolMenuNodeList menuNodes = new DevToolMenuNodeList();

	// Token: 0x04001608 RID: 5640
	public Dictionary<Type, string> devToolNameDict = new Dictionary<Type, string>();

	// Token: 0x04001609 RID: 5641
	private HashSet<Type> dontAutomaticallyRegisterTypes = new HashSet<Type>();
}
