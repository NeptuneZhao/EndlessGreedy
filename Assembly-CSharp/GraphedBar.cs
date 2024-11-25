using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C54 RID: 3156
[AddComponentMenu("KMonoBehaviour/scripts/GraphedBar")]
[Serializable]
public class GraphedBar : KMonoBehaviour
{
	// Token: 0x06006106 RID: 24838 RVA: 0x00241E23 File Offset: 0x00240023
	public void SetFormat(GraphedBarFormatting format)
	{
		this.format = format;
	}

	// Token: 0x06006107 RID: 24839 RVA: 0x00241E2C File Offset: 0x0024002C
	public void SetValues(int[] values, float x_position)
	{
		this.ClearValues();
		base.gameObject.rectTransform().anchorMin = new Vector2(x_position, 0f);
		base.gameObject.rectTransform().anchorMax = new Vector2(x_position, 1f);
		base.gameObject.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)this.format.width);
		for (int i = 0; i < values.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_segment, this.segments_container, true);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredHeight = (float)values[i];
			component.minWidth = (float)this.format.width;
			gameObject.GetComponent<Image>().color = this.format.colors[i % this.format.colors.Length];
			this.segments.Add(gameObject);
		}
	}

	// Token: 0x06006108 RID: 24840 RVA: 0x00241F0C File Offset: 0x0024010C
	public void ClearValues()
	{
		foreach (GameObject obj in this.segments)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		this.segments.Clear();
	}

	// Token: 0x040041B3 RID: 16819
	public GameObject segments_container;

	// Token: 0x040041B4 RID: 16820
	public GameObject prefab_segment;

	// Token: 0x040041B5 RID: 16821
	private List<GameObject> segments = new List<GameObject>();

	// Token: 0x040041B6 RID: 16822
	private GraphedBarFormatting format;
}
