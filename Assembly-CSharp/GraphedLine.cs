using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02000C57 RID: 3159
[AddComponentMenu("KMonoBehaviour/scripts/GraphedLine")]
[Serializable]
public class GraphedLine : KMonoBehaviour
{
	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x0600610C RID: 24844 RVA: 0x00241FA5 File Offset: 0x002401A5
	public int PointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x0600610D RID: 24845 RVA: 0x00241FAF File Offset: 0x002401AF
	public void SetPoints(Vector2[] points)
	{
		this.points = points;
		this.UpdatePoints();
	}

	// Token: 0x0600610E RID: 24846 RVA: 0x00241FC0 File Offset: 0x002401C0
	private void UpdatePoints()
	{
		Vector2[] array = new Vector2[this.points.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.layer.graph.GetRelativePosition(this.points[i]);
		}
		this.line_renderer.Points = array;
	}

	// Token: 0x0600610F RID: 24847 RVA: 0x00242018 File Offset: 0x00240218
	public Vector2 GetClosestDataToPointOnXAxis(Vector2 toPoint)
	{
		float num = toPoint.x / this.layer.graph.rectTransform().sizeDelta.x;
		float num2 = this.layer.graph.axis_x.min_value + this.layer.graph.axis_x.range * num;
		Vector2 vector = Vector2.zero;
		foreach (Vector2 vector2 in this.points)
		{
			if (Mathf.Abs(vector2.x - num2) < Mathf.Abs(vector.x - num2))
			{
				vector = vector2;
			}
		}
		return vector;
	}

	// Token: 0x06006110 RID: 24848 RVA: 0x002420BF File Offset: 0x002402BF
	public void HidePointHighlight()
	{
		if (this.highlightPoint != null)
		{
			this.highlightPoint.SetActive(false);
		}
	}

	// Token: 0x06006111 RID: 24849 RVA: 0x002420DC File Offset: 0x002402DC
	public void SetPointHighlight(Vector2 point)
	{
		if (this.highlightPoint == null)
		{
			return;
		}
		this.highlightPoint.SetActive(true);
		Vector2 relativePosition = this.layer.graph.GetRelativePosition(point);
		this.highlightPoint.rectTransform().SetLocalPosition(new Vector2(relativePosition.x * this.layer.graph.rectTransform().sizeDelta.x - this.layer.graph.rectTransform().sizeDelta.x / 2f, relativePosition.y * this.layer.graph.rectTransform().sizeDelta.y - this.layer.graph.rectTransform().sizeDelta.y / 2f));
		ToolTip component = this.layer.graph.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		component.tooltipPositionOffset = new Vector2(this.highlightPoint.rectTransform().localPosition.x, this.layer.graph.rectTransform().rect.height / 2f - 12f);
		component.SetSimpleTooltip(string.Concat(new string[]
		{
			this.layer.graph.axis_x.name,
			" ",
			point.x.ToString(),
			", ",
			Mathf.RoundToInt(point.y).ToString(),
			" ",
			this.layer.graph.axis_y.name
		}));
		ToolTipScreen.Instance.SetToolTip(component);
	}

	// Token: 0x040041BA RID: 16826
	public UILineRenderer line_renderer;

	// Token: 0x040041BB RID: 16827
	public LineLayer layer;

	// Token: 0x040041BC RID: 16828
	private Vector2[] points = new Vector2[0];

	// Token: 0x040041BD RID: 16829
	[SerializeField]
	private GameObject highlightPoint;
}
