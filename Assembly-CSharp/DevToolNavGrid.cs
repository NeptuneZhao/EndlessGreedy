using System;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200061E RID: 1566
public class DevToolNavGrid : DevTool
{
	// Token: 0x06002692 RID: 9874 RVA: 0x000D8736 File Offset: 0x000D6936
	public DevToolNavGrid()
	{
		DevToolNavGrid.Instance = this;
	}

	// Token: 0x06002693 RID: 9875 RVA: 0x000D8744 File Offset: 0x000D6944
	private bool Init()
	{
		if (Pathfinding.Instance == null)
		{
			return false;
		}
		if (this.navGridNames != null)
		{
			return true;
		}
		this.navGridNames = (from x in Pathfinding.Instance.GetNavGrids()
		select x.id).ToArray<string>();
		return true;
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x000D87A4 File Offset: 0x000D69A4
	protected override void RenderTo(DevPanel panel)
	{
		if (this.Init())
		{
			this.Contents();
			return;
		}
		ImGui.Text("Game not initialized");
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x000D87BF File Offset: 0x000D69BF
	public void SetCell(int cell)
	{
		this.selectedCell = cell;
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x000D87C8 File Offset: 0x000D69C8
	private void Contents()
	{
		ImGui.Combo("Nav Grid ID", ref this.selectedNavGrid, this.navGridNames, this.navGridNames.Length);
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid(this.navGridNames[this.selectedNavGrid]);
		ImGui.Text("Max Links per cell: " + navGrid.maxLinksPerCell.ToString());
		ImGui.Spacing();
		if (ImGui.Button("Calculate Stats"))
		{
			this.linkStats = new int[navGrid.maxLinksPerCell];
			this.highestLinkCell = 0;
			this.highestLinkCount = 0;
			for (int i = 0; i < Grid.CellCount; i++)
			{
				int num = 0;
				for (int j = 0; j < navGrid.maxLinksPerCell; j++)
				{
					int num2 = i * navGrid.maxLinksPerCell + j;
					if (navGrid.Links[num2].link == Grid.InvalidCell)
					{
						break;
					}
					num++;
				}
				if (num > this.highestLinkCount)
				{
					this.highestLinkCell = i;
					this.highestLinkCount = num;
				}
				this.linkStats[num]++;
			}
		}
		ImGui.SameLine();
		if (ImGui.Button("Clear"))
		{
			this.linkStats = null;
		}
		ImGui.SameLine();
		if (ImGui.Button("Rescan"))
		{
			navGrid.InitializeGraph();
		}
		if (this.linkStats != null)
		{
			ImGui.Text("Highest link count: " + this.highestLinkCount.ToString());
			ImGui.Text(string.Format("Utilized percentage: {0} %", (float)this.highestLinkCount / (float)navGrid.maxLinksPerCell * 100f));
			ImGui.SameLine();
			if (ImGui.Button(string.Format("Select {0}", this.highestLinkCell)))
			{
				this.selectedCell = this.highestLinkCell;
			}
			for (int k = 0; k < this.linkStats.Length; k++)
			{
				if (this.linkStats[k] > 0)
				{
					ImGui.Text(string.Format("\t{0}: {1}", k, this.linkStats[k]));
				}
			}
		}
		ImGui.Checkbox("DrawDebugPath", ref DebugHandler.DebugPathFinding);
		if (Camera.main != null && SelectTool.Instance != null)
		{
			GameObject gameObject = null;
			ImGui.Checkbox("Lock", ref this.follow);
			if (this.follow)
			{
				if (this.lockObject == null && SelectTool.Instance.selected != null)
				{
					this.lockObject = SelectTool.Instance.selected.gameObject;
				}
				gameObject = this.lockObject;
			}
			else if (SelectTool.Instance.selected != null)
			{
				gameObject = SelectTool.Instance.selected.gameObject;
				this.lockObject = null;
			}
			if (gameObject != null)
			{
				Navigator component = gameObject.GetComponent<Navigator>();
				if (component != null)
				{
					Vector2 positionFor = DevToolEntity.GetPositionFor(component.gameObject);
					ImGui.GetBackgroundDrawList().AddCircleFilled(positionFor, 10f, ImGui.GetColorU32(Color.green));
					Vector2 screenPosition = DevToolEntity.GetScreenPosition(component.GetComponent<KBatchedAnimController>().GetPivotSymbolPosition());
					ImGui.GetBackgroundDrawList().AddCircleFilled(screenPosition, 10f, ImGui.GetColorU32(Color.blue));
				}
			}
		}
		ImGui.Spacing();
		ImGui.Checkbox("Draw Links", ref this.drawLinks);
		if (this.drawLinks)
		{
			this.DebugDrawLinks(navGrid);
		}
		ImGui.Spacing();
		int num3;
		int num4;
		Grid.CellToXY(this.selectedCell, out num3, out num4);
		ImGui.Text(string.Format("Selected Cell: {0} ({1},{2})", this.selectedCell, num3, num4));
		if (Grid.IsValidCell(this.selectedCell) && navGrid.Links != null && navGrid.Links.Length > navGrid.maxLinksPerCell * this.selectedCell)
		{
			for (int l = 0; l < navGrid.maxLinksPerCell; l++)
			{
				int num5 = this.selectedCell * navGrid.maxLinksPerCell + l;
				NavGrid.Link link = navGrid.Links[num5];
				if (link.link == Grid.InvalidCell)
				{
					break;
				}
				this.DrawLink(l, link, navGrid);
			}
		}
	}

	// Token: 0x06002697 RID: 9879 RVA: 0x000D8BE8 File Offset: 0x000D6DE8
	private void DrawLink(int idx, NavGrid.Link l, NavGrid navGrid)
	{
		NavGrid.Transition transition = navGrid.transitions[(int)l.transitionId];
		ImGui.Text(string.Format("   {0} -> {1} x:{2} y:{3} anim:{4} cost:{5}", new object[]
		{
			transition.start,
			transition.end,
			transition.x,
			transition.y,
			transition.anim,
			transition.cost
		}));
	}

	// Token: 0x06002698 RID: 9880 RVA: 0x000D8C6C File Offset: 0x000D6E6C
	private void DebugDrawLinks(NavGrid navGrid)
	{
		if (Camera.main == null)
		{
			return;
		}
		Camera main = Camera.main;
		int pixelHeight = main.pixelHeight;
		Color white = Color.white;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			int num = i * navGrid.maxLinksPerCell;
			for (int link = navGrid.Links[num].link; link != NavGrid.InvalidCell; link = navGrid.Links[num].link)
			{
				if (this.DrawNavTypeLink(navGrid, num, ref white))
				{
					Vector3 navPos = NavTypeHelper.GetNavPos(i, navGrid.Links[num].startNavType);
					Vector3 navPos2 = NavTypeHelper.GetNavPos(link, navGrid.Links[num].endNavType);
					if (this.IsInCameraView(main, navPos) && this.IsInCameraView(main, navPos2))
					{
						Vector2 vector = main.WorldToScreenPoint(navPos);
						Vector2 vector2 = main.WorldToScreenPoint(navPos2);
						vector.y = (float)pixelHeight - vector.y;
						vector2.y = (float)pixelHeight - vector2.y;
						uint colorU = ImGui.GetColorU32(white);
						this.DrawArrowLink(vector, vector2, colorU);
					}
				}
				num++;
			}
		}
	}

	// Token: 0x06002699 RID: 9881 RVA: 0x000D8DB0 File Offset: 0x000D6FB0
	private bool IsInCameraView(Camera camera, Vector3 pos)
	{
		Vector3 vector = camera.WorldToViewportPoint(pos);
		return vector.x >= 0f && vector.y >= 0f && vector.x <= 1f && vector.y <= 1f;
	}

	// Token: 0x0600269A RID: 9882 RVA: 0x000D8E00 File Offset: 0x000D7000
	private bool DrawNavTypeLink(NavGrid navGrid, int end_cell_idx, ref Color color)
	{
		for (int i = 0; i < navGrid.ValidNavTypes.Length; i++)
		{
			if (navGrid.ValidNavTypes[i] == navGrid.Links[end_cell_idx].startNavType)
			{
				color = navGrid.NavTypeColor(navGrid.Links[end_cell_idx].startNavType);
				return true;
			}
			if (navGrid.ValidNavTypes[i] == navGrid.Links[end_cell_idx].endNavType)
			{
				color = navGrid.NavTypeColor(navGrid.Links[end_cell_idx].endNavType);
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600269B RID: 9883 RVA: 0x000D8E98 File Offset: 0x000D7098
	private void DrawArrowLink(Vector2 start, Vector2 end, uint color)
	{
		ImDrawListPtr backgroundDrawList = ImGui.GetBackgroundDrawList();
		Vector2 vector = end - start;
		float magnitude = vector.magnitude;
		if (magnitude > 0f)
		{
			vector *= 1f / Mathf.Sqrt(magnitude);
		}
		Vector2 p = end - vector * 1f + new Vector2(-vector.y, vector.x) * 1f;
		Vector2 p2 = end - vector * 1f - new Vector2(-vector.y, vector.x) * 1f;
		backgroundDrawList.AddLine(start, end, color);
		backgroundDrawList.AddTriangleFilled(end, p, p2, color);
	}

	// Token: 0x04001610 RID: 5648
	private const string INVALID_OVERLAY_MODE_STR = "None";

	// Token: 0x04001611 RID: 5649
	private string[] navGridNames;

	// Token: 0x04001612 RID: 5650
	private int selectedNavGrid;

	// Token: 0x04001613 RID: 5651
	private bool drawLinks;

	// Token: 0x04001614 RID: 5652
	public static DevToolNavGrid Instance;

	// Token: 0x04001615 RID: 5653
	private int[] linkStats;

	// Token: 0x04001616 RID: 5654
	private int highestLinkCell;

	// Token: 0x04001617 RID: 5655
	private int highestLinkCount;

	// Token: 0x04001618 RID: 5656
	private int selectedCell;

	// Token: 0x04001619 RID: 5657
	private bool follow;

	// Token: 0x0400161A RID: 5658
	private GameObject lockObject;
}
