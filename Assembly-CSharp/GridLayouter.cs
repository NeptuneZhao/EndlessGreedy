using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C5A RID: 3162
public class GridLayouter
{
	// Token: 0x06006126 RID: 24870 RVA: 0x0024345C File Offset: 0x0024165C
	[Conditional("UNITY_EDITOR")]
	private void ValidateImportantFieldsAreSet()
	{
		global::Debug.Assert(this.minCellSize >= 0f, string.Format("[{0} Error] Minimum cell size is invalid. Given: {1}", "GridLayouter", this.minCellSize));
		global::Debug.Assert(this.maxCellSize >= 0f, string.Format("[{0} Error] Maximum cell size is invalid. Given: {1}", "GridLayouter", this.maxCellSize));
		global::Debug.Assert(this.targetGridLayouts != null, string.Format("[{0} Error] Target grid layout is invalid. Given: {1}", "GridLayouter", this.targetGridLayouts));
	}

	// Token: 0x06006127 RID: 24871 RVA: 0x002434EC File Offset: 0x002416EC
	public void CheckIfShouldResizeGrid()
	{
		Vector2 lhs = new Vector2((float)Screen.width, (float)Screen.height);
		if (lhs != this.oldScreenSize)
		{
			this.RequestGridResize();
		}
		this.oldScreenSize = lhs;
		float @float = KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey);
		if (@float != this.oldScreenScale)
		{
			this.RequestGridResize();
		}
		this.oldScreenScale = @float;
		this.ResizeGridIfRequested();
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x0024354E File Offset: 0x0024174E
	public void RequestGridResize()
	{
		this.framesLeftToResizeGrid = 3;
	}

	// Token: 0x06006129 RID: 24873 RVA: 0x00243557 File Offset: 0x00241757
	private void ResizeGridIfRequested()
	{
		if (this.framesLeftToResizeGrid > 0)
		{
			this.ImmediateSizeGridToScreenResolution();
			this.framesLeftToResizeGrid--;
			if (this.framesLeftToResizeGrid == 0 && this.OnSizeGridComplete != null)
			{
				this.OnSizeGridComplete();
			}
		}
	}

	// Token: 0x0600612A RID: 24874 RVA: 0x00243594 File Offset: 0x00241794
	public void ImmediateSizeGridToScreenResolution()
	{
		foreach (GridLayoutGroup gridLayoutGroup in this.targetGridLayouts)
		{
			float workingWidth = ((this.overrideParentForSizeReference != null) ? this.overrideParentForSizeReference.rect.size.x : gridLayoutGroup.transform.parent.rectTransform().rect.size.x) - (float)gridLayoutGroup.padding.left - (float)gridLayoutGroup.padding.right;
			float x = gridLayoutGroup.spacing.x;
			int num = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(this.maxCellSize, x, workingWidth) + 1;
			float num2;
			for (num2 = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num); num2 < this.minCellSize; num2 = Mathf.Min(this.maxCellSize, GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num)))
			{
				num--;
				if (num <= 0)
				{
					num = 1;
					num2 = this.minCellSize;
					break;
				}
			}
			gridLayoutGroup.childAlignment = ((num == 1) ? TextAnchor.UpperCenter : TextAnchor.UpperLeft);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = num;
			gridLayoutGroup.cellSize = Vector2.one * num2;
		}
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x0024370A File Offset: 0x0024190A
	[CompilerGenerated]
	internal static float <ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(float workingWidth, float spacingSize, int count)
	{
		return (workingWidth - (spacingSize * (float)count - 1f)) / (float)count;
	}

	// Token: 0x0600612D RID: 24877 RVA: 0x0024371C File Offset: 0x0024191C
	[CompilerGenerated]
	internal static int <ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(float cellSize, float spacingSize, float workingWidth)
	{
		int num = 0;
		for (float num2 = cellSize; num2 < workingWidth; num2 += cellSize + spacingSize)
		{
			num++;
		}
		return num;
	}

	// Token: 0x040041CE RID: 16846
	public float minCellSize = -1f;

	// Token: 0x040041CF RID: 16847
	public float maxCellSize = -1f;

	// Token: 0x040041D0 RID: 16848
	public List<GridLayoutGroup> targetGridLayouts;

	// Token: 0x040041D1 RID: 16849
	public RectTransform overrideParentForSizeReference;

	// Token: 0x040041D2 RID: 16850
	public System.Action OnSizeGridComplete;

	// Token: 0x040041D3 RID: 16851
	private Vector2 oldScreenSize;

	// Token: 0x040041D4 RID: 16852
	private float oldScreenScale;

	// Token: 0x040041D5 RID: 16853
	private int framesLeftToResizeGrid;
}
