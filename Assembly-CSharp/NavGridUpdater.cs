using System;
using System.Collections.Generic;

// Token: 0x02000486 RID: 1158
public class NavGridUpdater
{
	// Token: 0x06001909 RID: 6409 RVA: 0x00086662 File Offset: 0x00084862
	public static void InitializeNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type)
	{
		NavGridUpdater.MarkValidCells(nav_table, validators, bounding_offsets);
		NavGridUpdater.CreateLinks(nav_table, max_links_per_cell, links, transitions_by_nav_type, new Dictionary<int, int>());
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x0008667C File Offset: 0x0008487C
	public static void UpdateNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions, IEnumerable<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateValidCells(dirty_nav_cells, nav_table, validators, bounding_offsets);
		NavGridUpdater.UpdateLinks(dirty_nav_cells, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x00086698 File Offset: 0x00084898
	private static void UpdateValidCells(IEnumerable<int> dirty_solid_cells, NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		foreach (int cell in dirty_solid_cells)
		{
			for (int i = 0; i < validators.Length; i++)
			{
				validators[i].UpdateCell(cell, nav_table, bounding_offsets);
			}
		}
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x000866F4 File Offset: 0x000848F4
	private static void CreateLinksForCell(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		NavGridUpdater.CreateLinks(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x00086704 File Offset: 0x00084904
	private static void UpdateLinks(IEnumerable<int> dirty_nav_cells, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		foreach (int cell in dirty_nav_cells)
		{
			NavGridUpdater.CreateLinksForCell(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
		}
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x00086750 File Offset: 0x00084950
	private static void CreateLinks(NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.CreateLinkWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x000867A0 File Offset: 0x000849A0
	private static void CreateLinks(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		int num = cell * max_links_per_cell;
		int num2 = 0;
		for (int i = 0; i < 11; i++)
		{
			NavType nav_type = (NavType)i;
			NavGrid.Transition[] array = transitions_by_nav_type[i];
			if (array != null && nav_table.IsValid(cell, nav_type))
			{
				NavGrid.Transition[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					NavGrid.Transition transition;
					if ((transition = array2[j]).start == NavType.Teleport && teleport_transitions.ContainsKey(cell))
					{
						int num3;
						int num4;
						Grid.CellToXY(cell, out num3, out num4);
						int num5 = teleport_transitions[cell];
						int num6;
						int num7;
						Grid.CellToXY(teleport_transitions[cell], out num6, out num7);
						transition.x = num6 - num3;
						transition.y = num7 - num4;
					}
					int num8 = transition.IsValid(cell, nav_table);
					if (num8 != Grid.InvalidCell)
					{
						links[num] = new NavGrid.Link(num8, transition.start, transition.end, transition.id, transition.cost);
						num++;
						num2++;
					}
				}
			}
		}
		if (num2 >= max_links_per_cell)
		{
			Debug.LogError("Out of nav links. Need to increase maxLinksPerCell:" + max_links_per_cell.ToString());
		}
		links[num].link = Grid.InvalidCell;
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x000868CC File Offset: 0x00084ACC
	private static void MarkValidCells(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.MarkValidCellWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, bounding_offsets, validators));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x00086917 File Offset: 0x00084B17
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06001912 RID: 6418 RVA: 0x0008692C File Offset: 0x00084B2C
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGridUpdater.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x04000E0D RID: 3597
	public static int InvalidHandle = -1;

	// Token: 0x04000E0E RID: 3598
	public static int InvalidIdx = -1;

	// Token: 0x04000E0F RID: 3599
	public static int InvalidCell = -1;

	// Token: 0x02001262 RID: 4706
	private struct CreateLinkWorkItem : IWorkItem<object>
	{
		// Token: 0x060082FC RID: 33532 RVA: 0x0031E1B2 File Offset: 0x0031C3B2
		public CreateLinkWorkItem(int start_cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.maxLinksPerCell = max_links_per_cell;
			this.links = links;
			this.transitionsByNavType = transitions_by_nav_type;
			this.teleportTransitions = teleport_transitions;
		}

		// Token: 0x060082FD RID: 33533 RVA: 0x0031E1E4 File Offset: 0x0031C3E4
		public void Run(object shared_data)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				NavGridUpdater.CreateLinksForCell(this.startCell + i, this.navTable, this.maxLinksPerCell, this.links, this.transitionsByNavType, this.teleportTransitions);
			}
		}

		// Token: 0x0400633C RID: 25404
		private int startCell;

		// Token: 0x0400633D RID: 25405
		private NavTable navTable;

		// Token: 0x0400633E RID: 25406
		private int maxLinksPerCell;

		// Token: 0x0400633F RID: 25407
		private NavGrid.Link[] links;

		// Token: 0x04006340 RID: 25408
		private NavGrid.Transition[][] transitionsByNavType;

		// Token: 0x04006341 RID: 25409
		private Dictionary<int, int> teleportTransitions;
	}

	// Token: 0x02001263 RID: 4707
	private struct MarkValidCellWorkItem : IWorkItem<object>
	{
		// Token: 0x060082FE RID: 33534 RVA: 0x0031E22C File Offset: 0x0031C42C
		public MarkValidCellWorkItem(int start_cell, NavTable nav_table, CellOffset[] bounding_offsets, NavTableValidator[] validators)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.boundingOffsets = bounding_offsets;
			this.validators = validators;
		}

		// Token: 0x060082FF RID: 33535 RVA: 0x0031E24C File Offset: 0x0031C44C
		public void Run(object shared_data)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				int cell = this.startCell + i;
				NavTableValidator[] array = this.validators;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].UpdateCell(cell, this.navTable, this.boundingOffsets);
				}
			}
		}

		// Token: 0x04006342 RID: 25410
		private NavTable navTable;

		// Token: 0x04006343 RID: 25411
		private CellOffset[] boundingOffsets;

		// Token: 0x04006344 RID: 25412
		private NavTableValidator[] validators;

		// Token: 0x04006345 RID: 25413
		private int startCell;
	}
}
