using System;
using UnityEngine;

// Token: 0x02000D19 RID: 3353
public class QuickLayout : KMonoBehaviour
{
	// Token: 0x060068B8 RID: 26808 RVA: 0x0027367C File Offset: 0x0027187C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ForceUpdate();
	}

	// Token: 0x060068B9 RID: 26809 RVA: 0x0027368A File Offset: 0x0027188A
	private void OnEnable()
	{
		this.ForceUpdate();
	}

	// Token: 0x060068BA RID: 26810 RVA: 0x00273692 File Offset: 0x00271892
	private void LateUpdate()
	{
		this.Run(false);
	}

	// Token: 0x060068BB RID: 26811 RVA: 0x0027369B File Offset: 0x0027189B
	public void ForceUpdate()
	{
		this.Run(true);
	}

	// Token: 0x060068BC RID: 26812 RVA: 0x002736A4 File Offset: 0x002718A4
	private void Run(bool forceUpdate = false)
	{
		forceUpdate = (forceUpdate || this._elementSize != this.elementSize);
		forceUpdate = (forceUpdate || this._spacing != this.spacing);
		forceUpdate = (forceUpdate || this._layoutDirection != this.layoutDirection);
		forceUpdate = (forceUpdate || this._offset != this.offset);
		if (forceUpdate)
		{
			this._elementSize = this.elementSize;
			this._spacing = this.spacing;
			this._layoutDirection = this.layoutDirection;
			this._offset = this.offset;
		}
		int num = 0;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				num++;
			}
		}
		if (num != this.oldActiveChildCount || forceUpdate)
		{
			this.Layout();
			this.oldActiveChildCount = num;
		}
	}

	// Token: 0x060068BD RID: 26813 RVA: 0x0027379C File Offset: 0x0027199C
	public void Layout()
	{
		Vector3 vector = this._offset;
		bool flag = false;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeInHierarchy)
			{
				flag = true;
				base.transform.GetChild(i).rectTransform().anchoredPosition = vector;
				vector += (float)(this._elementSize + this._spacing) * this.GetDirectionVector();
			}
		}
		if (this.driveParentRectSize != null)
		{
			if (!flag)
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(this.driveParentRectSize.sizeDelta.x), 0f);
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(0f, Mathf.Abs(this.driveParentRectSize.sizeDelta.y));
					return;
				}
			}
			else
			{
				if (this._layoutDirection == QuickLayout.LayoutDirection.BottomToTop || this._layoutDirection == QuickLayout.LayoutDirection.TopToBottom)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(this.driveParentRectSize.sizeDelta.x, Mathf.Abs(vector.y));
					return;
				}
				if (this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight || this._layoutDirection == QuickLayout.LayoutDirection.LeftToRight)
				{
					this.driveParentRectSize.sizeDelta = new Vector2(Mathf.Abs(vector.x), this.driveParentRectSize.sizeDelta.y);
				}
			}
		}
	}

	// Token: 0x060068BE RID: 26814 RVA: 0x00273934 File Offset: 0x00271B34
	private Vector2 GetDirectionVector()
	{
		Vector2 result = Vector3.zero;
		switch (this._layoutDirection)
		{
		case QuickLayout.LayoutDirection.TopToBottom:
			result = Vector2.down;
			break;
		case QuickLayout.LayoutDirection.BottomToTop:
			result = Vector2.up;
			break;
		case QuickLayout.LayoutDirection.LeftToRight:
			result = Vector2.right;
			break;
		case QuickLayout.LayoutDirection.RightToLeft:
			result = Vector2.left;
			break;
		}
		return result;
	}

	// Token: 0x040046D9 RID: 18137
	[Header("Configuration")]
	[SerializeField]
	private int elementSize;

	// Token: 0x040046DA RID: 18138
	[SerializeField]
	private int spacing;

	// Token: 0x040046DB RID: 18139
	[SerializeField]
	private QuickLayout.LayoutDirection layoutDirection;

	// Token: 0x040046DC RID: 18140
	[SerializeField]
	private Vector2 offset;

	// Token: 0x040046DD RID: 18141
	[SerializeField]
	private RectTransform driveParentRectSize;

	// Token: 0x040046DE RID: 18142
	private int _elementSize;

	// Token: 0x040046DF RID: 18143
	private int _spacing;

	// Token: 0x040046E0 RID: 18144
	private QuickLayout.LayoutDirection _layoutDirection;

	// Token: 0x040046E1 RID: 18145
	private Vector2 _offset;

	// Token: 0x040046E2 RID: 18146
	private int oldActiveChildCount;

	// Token: 0x02001E3B RID: 7739
	private enum LayoutDirection
	{
		// Token: 0x040089DB RID: 35291
		TopToBottom,
		// Token: 0x040089DC RID: 35292
		BottomToTop,
		// Token: 0x040089DD RID: 35293
		LeftToRight,
		// Token: 0x040089DE RID: 35294
		RightToLeft
	}
}
