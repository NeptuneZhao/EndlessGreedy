using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C59 RID: 3161
public class SparkLayer : LineLayer
{
	// Token: 0x0600611F RID: 24863 RVA: 0x00242FD8 File Offset: 0x002411D8
	public void SetColor(ColonyDiagnostic.DiagnosticResult result)
	{
		switch (result.opinion)
		{
		case ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
			this.SetColor(Constants.NEGATIVE_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
			this.SetColor(Constants.WARNING_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
			this.SetColor(Constants.NEUTRAL_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
			this.SetColor(Constants.POSITIVE_COLOR);
			return;
		default:
			this.SetColor(Constants.NEUTRAL_COLOR);
			return;
		}
	}

	// Token: 0x06006120 RID: 24864 RVA: 0x00243051 File Offset: 0x00241251
	public void SetColor(Color color)
	{
		this.line_formatting[0].color = color;
	}

	// Token: 0x06006121 RID: 24865 RVA: 0x00243068 File Offset: 0x00241268
	public override GraphedLine NewLine(global::Tuple<float, float>[] points, string ID = "")
	{
		Color positive_COLOR = Constants.POSITIVE_COLOR;
		Color neutral_COLOR = Constants.NEUTRAL_COLOR;
		Color negative_COLOR = Constants.NEGATIVE_COLOR;
		if (this.colorRules.setOwnColor)
		{
			if (points.Length > 2)
			{
				if (this.colorRules.zeroIsBad && points[points.Length - 1].second == 0f)
				{
					this.line_formatting[0].color = negative_COLOR;
				}
				else if (points[points.Length - 1].second > points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? positive_COLOR : negative_COLOR);
				}
				else if (points[points.Length - 1].second < points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? negative_COLOR : positive_COLOR);
				}
				else
				{
					this.line_formatting[0].color = neutral_COLOR;
				}
			}
			else
			{
				this.line_formatting[0].color = neutral_COLOR;
			}
		}
		this.ScaleToData(points);
		if (this.subZeroAreaFill != null)
		{
			this.subZeroAreaFill.color = new Color(this.line_formatting[0].color.r, this.line_formatting[0].color.g, this.line_formatting[0].color.b, this.fillAlphaMin);
		}
		return base.NewLine(points, ID);
	}

	// Token: 0x06006122 RID: 24866 RVA: 0x002431EE File Offset: 0x002413EE
	public override void RefreshLine(global::Tuple<float, float>[] points, string ID)
	{
		this.SetColor(points);
		this.ScaleToData(points);
		base.RefreshLine(points, ID);
	}

	// Token: 0x06006123 RID: 24867 RVA: 0x00243208 File Offset: 0x00241408
	private void SetColor(global::Tuple<float, float>[] points)
	{
		Color positive_COLOR = Constants.POSITIVE_COLOR;
		Color neutral_COLOR = Constants.NEUTRAL_COLOR;
		Color negative_COLOR = Constants.NEGATIVE_COLOR;
		if (this.colorRules.setOwnColor)
		{
			if (points.Length > 2)
			{
				if (this.colorRules.zeroIsBad && points[points.Length - 1].second == 0f)
				{
					this.line_formatting[0].color = negative_COLOR;
				}
				else if (points[points.Length - 1].second > points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? positive_COLOR : negative_COLOR);
				}
				else if (points[points.Length - 1].second < points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? negative_COLOR : positive_COLOR);
				}
				else
				{
					this.line_formatting[0].color = neutral_COLOR;
				}
			}
			else
			{
				this.line_formatting[0].color = neutral_COLOR;
			}
		}
		if (this.subZeroAreaFill != null)
		{
			this.subZeroAreaFill.color = new Color(this.line_formatting[0].color.r, this.line_formatting[0].color.g, this.line_formatting[0].color.b, this.fillAlphaMin);
		}
	}

	// Token: 0x06006124 RID: 24868 RVA: 0x00243380 File Offset: 0x00241580
	private void ScaleToData(global::Tuple<float, float>[] points)
	{
		if (this.scaleWidthToData || this.scaleHeightToData)
		{
			Vector2 vector = base.CalculateMin(points);
			Vector2 vector2 = base.CalculateMax(points);
			if (this.scaleHeightToData)
			{
				base.graph.ClearHorizontalGuides();
				base.graph.axis_y.max_value = vector2.y;
				base.graph.axis_y.min_value = vector.y;
				base.graph.RefreshHorizontalGuides();
			}
			if (this.scaleWidthToData)
			{
				base.graph.ClearVerticalGuides();
				base.graph.axis_x.max_value = vector2.x;
				base.graph.axis_x.min_value = vector.x;
				base.graph.RefreshVerticalGuides();
			}
		}
	}

	// Token: 0x040041C9 RID: 16841
	public Image subZeroAreaFill;

	// Token: 0x040041CA RID: 16842
	public SparkLayer.ColorRules colorRules;

	// Token: 0x040041CB RID: 16843
	public bool debugMark;

	// Token: 0x040041CC RID: 16844
	public bool scaleHeightToData = true;

	// Token: 0x040041CD RID: 16845
	public bool scaleWidthToData = true;

	// Token: 0x02001D40 RID: 7488
	[Serializable]
	public struct ColorRules
	{
		// Token: 0x0400867C RID: 34428
		public bool setOwnColor;

		// Token: 0x0400867D RID: 34429
		public bool positiveIsGood;

		// Token: 0x0400867E RID: 34430
		public bool zeroIsBad;
	}
}
