using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200060E RID: 1550
public class DevToolCommandPalette : DevTool
{
	// Token: 0x06002636 RID: 9782 RVA: 0x000D4695 File Offset: 0x000D2895
	public DevToolCommandPalette() : this(null)
	{
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x000D46A0 File Offset: 0x000D28A0
	public DevToolCommandPalette(List<DevToolCommandPalette.Command> commands = null)
	{
		this.drawFlags |= ImGuiWindowFlags.NoResize;
		this.drawFlags |= ImGuiWindowFlags.NoScrollbar;
		this.drawFlags |= ImGuiWindowFlags.NoScrollWithMouse;
		if (commands == null)
		{
			this.commands.allValues = DevToolCommandPaletteUtil.GenerateDefaultCommandPalette();
			return;
		}
		this.commands.allValues = commands;
	}

	// Token: 0x06002638 RID: 9784 RVA: 0x000D472F File Offset: 0x000D292F
	public static void Init()
	{
		DevToolCommandPalette.InitWithCommands(DevToolCommandPaletteUtil.GenerateDefaultCommandPalette());
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x000D473B File Offset: 0x000D293B
	public static void InitWithCommands(List<DevToolCommandPalette.Command> commands)
	{
		DevToolManager.Instance.panels.AddPanelFor(new DevToolCommandPalette(commands));
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x000D4754 File Offset: 0x000D2954
	protected override void RenderTo(DevPanel panel)
	{
		DevToolCommandPalette.Resize(panel);
		if (this.commands.allValues == null)
		{
			ImGui.Text("No commands list given");
			return;
		}
		if (this.commands.allValues.Count == 0)
		{
			ImGui.Text("Given command list is empty, no results to show.");
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			panel.Close();
			return;
		}
		if (!ImGui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows))
		{
			panel.Close();
			return;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.m_selected_index--;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.m_selected_index++;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (this.commands.filteredValues.Count > 0)
		{
			while (this.m_selected_index < 0)
			{
				this.m_selected_index += this.commands.filteredValues.Count;
			}
			this.m_selected_index %= this.commands.filteredValues.Count;
		}
		else
		{
			this.m_selected_index = 0;
		}
		DevToolCommandPalette.Command command = null;
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.commands.filteredValues.Count > 0 && command == null)
		{
			command = this.commands.filteredValues[this.m_selected_index];
		}
		if (this.m_should_focus_search)
		{
			ImGui.SetKeyboardFocusHere();
		}
		if (ImGui.InputText("Filter", ref this.commands.filter, 30U) || this.m_should_focus_search)
		{
			this.commands.Refilter();
		}
		this.m_should_focus_search = false;
		ImGui.Separator();
		string text = "Up arrow & down arrow to navigate. Enter to select. ";
		if (this.commands.filteredValues.Count > 0 && this.commands.didUseFilter)
		{
			text += string.Format("Found {0} Results", this.commands.filteredValues.Count);
		}
		ImGui.Text(text);
		ImGui.Separator();
		if (ImGui.BeginChild("ID_scroll_region"))
		{
			if (this.commands.filteredValues.Count <= 0)
			{
				ImGui.Text("Couldn't find anything that matches \"" + this.commands.filter + "\", maybe it hasn't been added yet?");
			}
			else
			{
				for (int i = 0; i < this.commands.filteredValues.Count; i++)
				{
					DevToolCommandPalette.Command command2 = this.commands.filteredValues[i];
					bool flag = i == this.m_selected_index;
					ImGui.PushID(i);
					bool flag2;
					if (flag)
					{
						flag2 = ImGui.Selectable("> " + command2.display_name, flag);
					}
					else
					{
						flag2 = ImGui.Selectable("  " + command2.display_name, flag);
					}
					ImGui.PopID();
					if (this.shouldScrollToSelectedCommandFlag && flag)
					{
						this.shouldScrollToSelectedCommandFlag = false;
						ImGui.SetScrollHereY(0.5f);
					}
					if (flag2 && command == null)
					{
						command = command2;
					}
				}
			}
		}
		ImGui.EndChild();
		if (command != null)
		{
			command.Internal_Select();
			panel.Close();
		}
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x000D4A44 File Offset: 0x000D2C44
	private static void Resize(DevPanel devToolPanel)
	{
		float num = 800f;
		float num2 = 400f;
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Rect rect2 = new Rect
		{
			x = rect.x + rect.width / 2f - num / 2f,
			y = rect.y + rect.height / 2f - num2 / 2f,
			width = num,
			height = num2
		};
		devToolPanel.SetPosition(rect2.position, ImGuiCond.None);
		devToolPanel.SetSize(rect2.size, ImGuiCond.None);
	}

	// Token: 0x040015C7 RID: 5575
	private int m_selected_index;

	// Token: 0x040015C8 RID: 5576
	private StringSearchableList<DevToolCommandPalette.Command> commands = new StringSearchableList<DevToolCommandPalette.Command>(delegate(DevToolCommandPalette.Command command, in string filter)
	{
		return !StringSearchableListUtil.DoAnyTagsMatchFilter(command.tags, filter);
	});

	// Token: 0x040015C9 RID: 5577
	private bool m_should_focus_search = true;

	// Token: 0x040015CA RID: 5578
	private bool shouldScrollToSelectedCommandFlag;

	// Token: 0x020013F4 RID: 5108
	public class Command
	{
		// Token: 0x060088D6 RID: 35030 RVA: 0x0032F275 File Offset: 0x0032D475
		public Command(string primary_tag, System.Action on_select) : this(new string[]
		{
			primary_tag
		}, on_select)
		{
		}

		// Token: 0x060088D7 RID: 35031 RVA: 0x0032F288 File Offset: 0x0032D488
		public Command(string primary_tag, string tag_a, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a
		}, on_select)
		{
		}

		// Token: 0x060088D8 RID: 35032 RVA: 0x0032F29F File Offset: 0x0032D49F
		public Command(string primary_tag, string tag_a, string tag_b, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b
		}, on_select)
		{
		}

		// Token: 0x060088D9 RID: 35033 RVA: 0x0032F2BB File Offset: 0x0032D4BB
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c
		}, on_select)
		{
		}

		// Token: 0x060088DA RID: 35034 RVA: 0x0032F2DC File Offset: 0x0032D4DC
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d
		}, on_select)
		{
		}

		// Token: 0x060088DB RID: 35035 RVA: 0x0032F302 File Offset: 0x0032D502
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e
		}, on_select)
		{
		}

		// Token: 0x060088DC RID: 35036 RVA: 0x0032F32D File Offset: 0x0032D52D
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, string tag_f, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e,
			tag_f
		}, on_select)
		{
		}

		// Token: 0x060088DD RID: 35037 RVA: 0x0032F35D File Offset: 0x0032D55D
		public Command(string primary_tag, string[] additional_tags, System.Action on_select) : this(new string[]
		{
			primary_tag
		}.Concat(additional_tags).ToArray<string>(), on_select)
		{
		}

		// Token: 0x060088DE RID: 35038 RVA: 0x0032F37C File Offset: 0x0032D57C
		public Command(string[] tags, System.Action on_select)
		{
			this.display_name = tags[0];
			this.tags = (from t in tags
			select t.ToLowerInvariant()).ToArray<string>();
			this.m_on_select = on_select;
		}

		// Token: 0x060088DF RID: 35039 RVA: 0x0032F3CF File Offset: 0x0032D5CF
		public void Internal_Select()
		{
			this.m_on_select();
		}

		// Token: 0x04006879 RID: 26745
		public string display_name;

		// Token: 0x0400687A RID: 26746
		public string[] tags;

		// Token: 0x0400687B RID: 26747
		private System.Action m_on_select;
	}
}
