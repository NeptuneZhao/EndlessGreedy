using System;
using System.Collections.Generic;
using System.Linq;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000BF0 RID: 3056
public class ClusterMapPath : MonoBehaviour
{
	// Token: 0x06005D1B RID: 23835 RVA: 0x00223918 File Offset: 0x00221B18
	public void Init()
	{
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06005D1C RID: 23836 RVA: 0x00223937 File Offset: 0x00221B37
	public void Init(List<Vector2> nodes, Color color)
	{
		this.m_nodes = nodes;
		this.m_color = color;
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		this.UpdateColor();
		this.UpdateRenderer();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06005D1D RID: 23837 RVA: 0x00223970 File Offset: 0x00221B70
	public void SetColor(Color color)
	{
		this.m_color = color;
		this.UpdateColor();
	}

	// Token: 0x06005D1E RID: 23838 RVA: 0x0022397F File Offset: 0x00221B7F
	private void UpdateColor()
	{
		this.lineRenderer.color = this.m_color;
		this.pathStart.color = this.m_color;
		this.pathEnd.color = this.m_color;
	}

	// Token: 0x06005D1F RID: 23839 RVA: 0x002239B4 File Offset: 0x00221BB4
	public void SetPoints(List<Vector2> points)
	{
		this.m_nodes = points;
		this.UpdateRenderer();
	}

	// Token: 0x06005D20 RID: 23840 RVA: 0x002239C4 File Offset: 0x00221BC4
	private void UpdateRenderer()
	{
		HashSet<Vector2> pointsOnCatmullRomSpline = ProcGen.Util.GetPointsOnCatmullRomSpline(this.m_nodes, 10);
		this.lineRenderer.Points = pointsOnCatmullRomSpline.ToArray<Vector2>();
		if (this.lineRenderer.Points.Length > 1)
		{
			this.pathStart.transform.localPosition = this.lineRenderer.Points[0];
			this.pathStart.gameObject.SetActive(true);
			Vector2 vector = this.lineRenderer.Points[this.lineRenderer.Points.Length - 1];
			Vector2 b = this.lineRenderer.Points[this.lineRenderer.Points.Length - 2];
			this.pathEnd.transform.localPosition = vector;
			Vector2 v = vector - b;
			this.pathEnd.transform.rotation = Quaternion.LookRotation(Vector3.forward, v);
			this.pathEnd.gameObject.SetActive(true);
			return;
		}
		this.pathStart.gameObject.SetActive(false);
		this.pathEnd.gameObject.SetActive(false);
	}

	// Token: 0x06005D21 RID: 23841 RVA: 0x00223AEC File Offset: 0x00221CEC
	public float GetRotationForNextSegment()
	{
		if (this.m_nodes.Count > 1)
		{
			Vector2 b = this.m_nodes[0];
			Vector2 to = this.m_nodes[1] - b;
			return Vector2.SignedAngle(Vector2.up, to);
		}
		return 0f;
	}

	// Token: 0x04003E58 RID: 15960
	private List<Vector2> m_nodes;

	// Token: 0x04003E59 RID: 15961
	private Color m_color;

	// Token: 0x04003E5A RID: 15962
	public UILineRenderer lineRenderer;

	// Token: 0x04003E5B RID: 15963
	public Image pathStart;

	// Token: 0x04003E5C RID: 15964
	public Image pathEnd;
}
