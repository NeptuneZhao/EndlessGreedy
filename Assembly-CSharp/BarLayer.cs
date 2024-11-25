using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C53 RID: 3155
public class BarLayer : GraphLayer
{
	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x06006102 RID: 24834 RVA: 0x00241C87 File Offset: 0x0023FE87
	public int bar_count
	{
		get
		{
			return this.bars.Count;
		}
	}

	// Token: 0x06006103 RID: 24835 RVA: 0x00241C94 File Offset: 0x0023FE94
	public void NewBar(int[] values, float x_position, string ID = "")
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_bar, this.bar_container, true);
		if (ID == "")
		{
			ID = this.bars.Count.ToString();
		}
		gameObject.name = string.Format("bar_{0}", ID);
		GraphedBar component = gameObject.GetComponent<GraphedBar>();
		component.SetFormat(this.bar_formats[this.bars.Count % this.bar_formats.Length]);
		int[] array = new int[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = (int)(base.graph.rectTransform().rect.height * base.graph.GetRelativeSize(new Vector2(0f, (float)values[i])).y);
		}
		component.SetValues(array, base.graph.GetRelativePosition(new Vector2(x_position, 0f)).x);
		this.bars.Add(component);
	}

	// Token: 0x06006104 RID: 24836 RVA: 0x00241D94 File Offset: 0x0023FF94
	public void ClearBars()
	{
		foreach (GraphedBar graphedBar in this.bars)
		{
			if (graphedBar != null && graphedBar.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(graphedBar.gameObject);
			}
		}
		this.bars.Clear();
	}

	// Token: 0x040041AF RID: 16815
	public GameObject bar_container;

	// Token: 0x040041B0 RID: 16816
	public GameObject prefab_bar;

	// Token: 0x040041B1 RID: 16817
	public GraphedBarFormatting[] bar_formats;

	// Token: 0x040041B2 RID: 16818
	private List<GraphedBar> bars = new List<GraphedBar>();
}
