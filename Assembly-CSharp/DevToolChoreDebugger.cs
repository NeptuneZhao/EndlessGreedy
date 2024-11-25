using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200060D RID: 1549
public class DevToolChoreDebugger : DevTool
{
	// Token: 0x0600262F RID: 9775 RVA: 0x000D3FFD File Offset: 0x000D21FD
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
	}

	// Token: 0x06002630 RID: 9776 RVA: 0x000D4008 File Offset: 0x000D2208
	public void Update()
	{
		if (!Application.isPlaying || SelectTool.Instance == null || SelectTool.Instance.selected == null || SelectTool.Instance.selected.gameObject == null)
		{
			return;
		}
		GameObject gameObject = SelectTool.Instance.selected.gameObject;
		if (this.Consumer == null || (!this.lockSelection && this.selectedGameObject != gameObject))
		{
			this.Consumer = gameObject.GetComponent<ChoreConsumer>();
			this.selectedGameObject = gameObject;
		}
		if (this.Consumer != null)
		{
			ImGui.InputText("Filter:", ref this.filter, 256U);
			this.DisplayAvailableChores();
			ImGui.Text("");
		}
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x000D40D0 File Offset: 0x000D22D0
	private void DisplayAvailableChores()
	{
		ImGui.Checkbox("Lock selection", ref this.lockSelection);
		ImGui.Checkbox("Show Last Successful Chore Selection", ref this.showLastSuccessfulPreconditionSnapshot);
		ImGui.Text("Available Chores:");
		ChoreConsumer.PreconditionSnapshot target_snapshot = this.Consumer.GetLastPreconditionSnapshot();
		if (this.showLastSuccessfulPreconditionSnapshot)
		{
			target_snapshot = this.Consumer.GetLastSuccessfulPreconditionSnapshot();
		}
		this.ShowChores(target_snapshot);
	}

	// Token: 0x06002632 RID: 9778 RVA: 0x000D4130 File Offset: 0x000D2330
	private void ShowChores(ChoreConsumer.PreconditionSnapshot target_snapshot)
	{
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollX | ImGuiTableFlags.ScrollY;
		this.rowIndex = 0;
		if (ImGui.BeginTable("Available Chores", this.columns.Count, flags))
		{
			foreach (object obj in this.columns.Keys)
			{
				ImGui.TableSetupColumn(obj.ToString(), ImGuiTableColumnFlags.WidthFixed);
			}
			ImGui.TableHeadersRow();
			for (int i = target_snapshot.succeededContexts.Count - 1; i >= 0; i--)
			{
				this.ShowContext(target_snapshot.succeededContexts[i]);
			}
			if (target_snapshot.doFailedContextsNeedSorting)
			{
				target_snapshot.failedContexts.Sort();
				target_snapshot.doFailedContextsNeedSorting = false;
			}
			for (int j = target_snapshot.failedContexts.Count - 1; j >= 0; j--)
			{
				this.ShowContext(target_snapshot.failedContexts[j]);
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x06002633 RID: 9779 RVA: 0x000D4234 File Offset: 0x000D2434
	private void ShowContext(Chore.Precondition.Context context)
	{
		string text = "";
		Chore chore = context.chore;
		if (!context.IsSuccess())
		{
			text = context.chore.GetPreconditions()[context.failedPreconditionId].condition.id;
		}
		string text2 = "";
		if (chore.driver != null)
		{
			text2 = chore.driver.name;
		}
		string text3 = "";
		if (chore.overrideTarget != null)
		{
			text3 = chore.overrideTarget.name;
		}
		string text4 = "";
		if (!chore.isNull)
		{
			text4 = chore.gameObject.name;
		}
		if (Chore.Precondition.Context.ShouldFilter(this.filter, chore.GetType().ToString()) && Chore.Precondition.Context.ShouldFilter(this.filter, chore.choreType.Id) && Chore.Precondition.Context.ShouldFilter(this.filter, text) && Chore.Precondition.Context.ShouldFilter(this.filter, text2) && Chore.Precondition.Context.ShouldFilter(this.filter, text3) && Chore.Precondition.Context.ShouldFilter(this.filter, text4))
		{
			return;
		}
		this.columns["Id"] = chore.id.ToString();
		this.columns["Class"] = chore.GetType().ToString().Replace("`1", "");
		this.columns["Type"] = chore.choreType.Id;
		this.columns["PriorityClass"] = context.masterPriority.priority_class.ToString();
		this.columns["PersonalPriority"] = context.personalPriority.ToString();
		this.columns["PriorityValue"] = context.masterPriority.priority_value.ToString();
		this.columns["Priority"] = context.priority.ToString();
		this.columns["PriorityMod"] = context.priorityMod.ToString();
		this.columns["ConsumerPriority"] = context.consumerPriority.ToString();
		this.columns["Cost"] = context.cost.ToString();
		this.columns["Interrupt"] = context.interruptPriority.ToString();
		this.columns["Precondition"] = text;
		this.columns["Override"] = text3;
		this.columns["Assigned To"] = text2;
		this.columns["Owner"] = text4;
		this.columns["Details"] = "";
		ImGui.TableNextRow();
		string format = "ID_row_{0}";
		int num = this.rowIndex;
		this.rowIndex = num + 1;
		ImGui.PushID(string.Format(format, num));
		for (int i = 0; i < this.columns.Count; i++)
		{
			ImGui.TableSetColumnIndex(i);
			ImGui.Text(this.columns[i].ToString());
		}
		ImGui.PopID();
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x000D4557 File Offset: 0x000D2757
	public void ConsumerDebugDisplayLog()
	{
	}

	// Token: 0x040015C0 RID: 5568
	private string filter = "";

	// Token: 0x040015C1 RID: 5569
	private bool showLastSuccessfulPreconditionSnapshot;

	// Token: 0x040015C2 RID: 5570
	private bool lockSelection;

	// Token: 0x040015C3 RID: 5571
	private ChoreConsumer Consumer;

	// Token: 0x040015C4 RID: 5572
	private GameObject selectedGameObject;

	// Token: 0x040015C5 RID: 5573
	private OrderedDictionary columns = new OrderedDictionary
	{
		{
			"BP",
			""
		},
		{
			"Id",
			""
		},
		{
			"Class",
			""
		},
		{
			"Type",
			""
		},
		{
			"PriorityClass",
			""
		},
		{
			"PersonalPriority",
			""
		},
		{
			"PriorityValue",
			""
		},
		{
			"Priority",
			""
		},
		{
			"PriorityMod",
			""
		},
		{
			"ConsumerPriority",
			""
		},
		{
			"Cost",
			""
		},
		{
			"Interrupt",
			""
		},
		{
			"Precondition",
			""
		},
		{
			"Override",
			""
		},
		{
			"Assigned To",
			""
		},
		{
			"Owner",
			""
		},
		{
			"Details",
			""
		}
	};

	// Token: 0x040015C6 RID: 5574
	private int rowIndex;

	// Token: 0x020013F3 RID: 5107
	public class EditorPreconditionSnapshot
	{
		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060088D1 RID: 35025 RVA: 0x0032F24B File Offset: 0x0032D44B
		// (set) Token: 0x060088D2 RID: 35026 RVA: 0x0032F253 File Offset: 0x0032D453
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> SucceededContexts { get; set; }

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x060088D3 RID: 35027 RVA: 0x0032F25C File Offset: 0x0032D45C
		// (set) Token: 0x060088D4 RID: 35028 RVA: 0x0032F264 File Offset: 0x0032D464
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> FailedContexts { get; set; }

		// Token: 0x020024A7 RID: 9383
		public struct EditorContext
		{
			// Token: 0x17000C13 RID: 3091
			// (get) Token: 0x0600BA90 RID: 47760 RVA: 0x003D4082 File Offset: 0x003D2282
			// (set) Token: 0x0600BA91 RID: 47761 RVA: 0x003D408A File Offset: 0x003D228A
			public string Chore { readonly get; set; }

			// Token: 0x17000C14 RID: 3092
			// (get) Token: 0x0600BA92 RID: 47762 RVA: 0x003D4093 File Offset: 0x003D2293
			// (set) Token: 0x0600BA93 RID: 47763 RVA: 0x003D409B File Offset: 0x003D229B
			public string ChoreType { readonly get; set; }

			// Token: 0x17000C15 RID: 3093
			// (get) Token: 0x0600BA94 RID: 47764 RVA: 0x003D40A4 File Offset: 0x003D22A4
			// (set) Token: 0x0600BA95 RID: 47765 RVA: 0x003D40AC File Offset: 0x003D22AC
			public string FailedPrecondition { readonly get; set; }

			// Token: 0x17000C16 RID: 3094
			// (get) Token: 0x0600BA96 RID: 47766 RVA: 0x003D40B5 File Offset: 0x003D22B5
			// (set) Token: 0x0600BA97 RID: 47767 RVA: 0x003D40BD File Offset: 0x003D22BD
			public int WorldId { readonly get; set; }
		}
	}
}
