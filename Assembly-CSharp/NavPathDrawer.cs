using System;
using UnityEngine;

// Token: 0x02000590 RID: 1424
[AddComponentMenu("KMonoBehaviour/scripts/NavPathDrawer")]
public class NavPathDrawer : KMonoBehaviour
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06002152 RID: 8530 RVA: 0x000BAAF3 File Offset: 0x000B8CF3
	// (set) Token: 0x06002153 RID: 8531 RVA: 0x000BAAFA File Offset: 0x000B8CFA
	public static NavPathDrawer Instance { get; private set; }

	// Token: 0x06002154 RID: 8532 RVA: 0x000BAB02 File Offset: 0x000B8D02
	public static void DestroyInstance()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x000BAB0C File Offset: 0x000B8D0C
	protected override void OnPrefabInit()
	{
		Shader shader = Shader.Find("Lines/Colored Blended");
		this.material = new Material(shader);
		NavPathDrawer.Instance = this;
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x000BAB36 File Offset: 0x000B8D36
	protected override void OnCleanUp()
	{
		NavPathDrawer.Instance = null;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000BAB3E File Offset: 0x000B8D3E
	public void DrawPath(Vector3 navigator_pos, PathFinder.Path path)
	{
		this.navigatorPos = navigator_pos;
		this.navigatorPos.y = this.navigatorPos.y + 0.5f;
		this.path = path;
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000BAB6A File Offset: 0x000B8D6A
	public Navigator GetNavigator()
	{
		return this.navigator;
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x000BAB72 File Offset: 0x000B8D72
	public void SetNavigator(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x000BAB7B File Offset: 0x000B8D7B
	public void ClearNavigator()
	{
		this.navigator = null;
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000BAB84 File Offset: 0x000B8D84
	private void DrawPath(PathFinder.Path path, Vector3 navigator_pos, Color color)
	{
		if (path.nodes != null && path.nodes.Count > 1)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			GL.Color(color);
			GL.Vertex(navigator_pos);
			GL.Vertex(NavTypeHelper.GetNavPos(path.nodes[1].cell, path.nodes[1].navType));
			for (int i = 1; i < path.nodes.Count - 1; i++)
			{
				if ((int)Grid.WorldIdx[path.nodes[i].cell] == ClusterManager.Instance.activeWorldId && (int)Grid.WorldIdx[path.nodes[i + 1].cell] == ClusterManager.Instance.activeWorldId)
				{
					Vector3 navPos = NavTypeHelper.GetNavPos(path.nodes[i].cell, path.nodes[i].navType);
					Vector3 navPos2 = NavTypeHelper.GetNavPos(path.nodes[i + 1].cell, path.nodes[i + 1].navType);
					GL.Vertex(navPos);
					GL.Vertex(navPos2);
				}
			}
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x000BACD0 File Offset: 0x000B8ED0
	private void OnPostRender()
	{
		this.DrawPath(this.path, this.navigatorPos, Color.white);
		this.path = default(PathFinder.Path);
		this.DebugDrawSelectedNavigator();
		if (this.navigator != null)
		{
			GL.PushMatrix();
			this.material.SetPass(0);
			GL.Begin(1);
			PathFinderQuery query = PathFinderQueries.drawNavGridQuery.Reset(null);
			this.navigator.RunQuery(query);
			GL.End();
			GL.PopMatrix();
		}
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x000BAD50 File Offset: 0x000B8F50
	private void DebugDrawSelectedNavigator()
	{
		if (!DebugHandler.DebugPathFinding)
		{
			return;
		}
		if (SelectTool.Instance == null)
		{
			return;
		}
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
		if (component == null)
		{
			return;
		}
		int mouseCell = DebugHandler.GetMouseCell();
		if (Grid.IsValidCell(mouseCell))
		{
			PathFinder.PotentialPath potential_path = new PathFinder.PotentialPath(Grid.PosToCell(component), component.CurrentNavType, component.flags);
			PathFinder.Path path = default(PathFinder.Path);
			PathFinder.UpdatePath(component.NavGrid, component.GetCurrentAbilities(), potential_path, PathFinderQueries.cellQuery.Reset(mouseCell), ref path);
			string text = "";
			text = text + "Source: " + Grid.PosToCell(component).ToString() + "\n";
			text = text + "Dest: " + mouseCell.ToString() + "\n";
			text = text + "Cost: " + path.cost.ToString();
			this.DrawPath(path, component.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), Color.green);
			DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
		}
	}

	// Token: 0x040012AE RID: 4782
	private PathFinder.Path path;

	// Token: 0x040012AF RID: 4783
	public Material material;

	// Token: 0x040012B0 RID: 4784
	private Vector3 navigatorPos;

	// Token: 0x040012B1 RID: 4785
	private Navigator navigator;
}
