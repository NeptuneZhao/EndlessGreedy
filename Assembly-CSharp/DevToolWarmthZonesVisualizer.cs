using System;
using UnityEngine;

// Token: 0x02000631 RID: 1585
public class DevToolWarmthZonesVisualizer : DevTool
{
	// Token: 0x06002704 RID: 9988 RVA: 0x000DE044 File Offset: 0x000DC244
	private void SetupColors()
	{
		if (this.colors == null)
		{
			this.colors = new Color[3];
			for (int i = 1; i <= 3; i++)
			{
				this.colors[i - 1] = this.CreateColorForWarmthValue(i);
			}
		}
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x000DE088 File Offset: 0x000DC288
	private Color CreateColorForWarmthValue(int warmValue)
	{
		float b = (float)Mathf.Clamp(warmValue, 1, 3) / 3f;
		Color result = this.WARM_CELL_COLOR * b;
		result.a = this.WARM_CELL_COLOR.a;
		return result;
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x000DE0C8 File Offset: 0x000DC2C8
	private Color GetBorderColor(int warmValue)
	{
		int num = Mathf.Clamp(warmValue, 0, 3);
		return this.colors[num];
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x000DE0EC File Offset: 0x000DC2EC
	private Color GetFillColor(int warmValue)
	{
		Color borderColor = this.GetBorderColor(warmValue);
		borderColor.a = 0.3f;
		return borderColor;
	}

	// Token: 0x06002708 RID: 9992 RVA: 0x000DE110 File Offset: 0x000DC310
	protected override void RenderTo(DevPanel panel)
	{
		this.SetupColors();
		foreach (int num in WarmthProvider.WarmCells.Keys)
		{
			if (Grid.IsValidCell(num) && WarmthProvider.IsWarmCell(num))
			{
				int warmthValue = WarmthProvider.GetWarmthValue(num);
				Option<ValueTuple<Vector2, Vector2>> screenRect = new DevToolEntityTarget.ForSimCell(num).GetScreenRect();
				string value = warmthValue.ToString();
				DevToolEntity.DrawScreenRect(screenRect.Unwrap(), value, this.GetBorderColor(warmthValue - 1), this.GetFillColor(warmthValue - 1), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
			}
		}
	}

	// Token: 0x0400165A RID: 5722
	private const int MAX_COLOR_VARIANTS = 3;

	// Token: 0x0400165B RID: 5723
	private Color WARM_CELL_COLOR = Color.red;

	// Token: 0x0400165C RID: 5724
	private Color[] colors;
}
