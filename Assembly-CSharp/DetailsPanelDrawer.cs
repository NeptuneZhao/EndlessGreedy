using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BC4 RID: 3012
public class DetailsPanelDrawer
{
	// Token: 0x06005BC0 RID: 23488 RVA: 0x002172EC File Offset: 0x002154EC
	public DetailsPanelDrawer(GameObject label_prefab, GameObject parent)
	{
		this.parent = parent;
		this.labelPrefab = label_prefab;
		this.stringformatter = new UIStringFormatter();
		this.floatFormatter = new UIFloatFormatter();
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x00217324 File Offset: 0x00215524
	public DetailsPanelDrawer NewLabel(string text)
	{
		DetailsPanelDrawer.Label label = default(DetailsPanelDrawer.Label);
		if (this.activeLabelCount >= this.labels.Count)
		{
			label.text = Util.KInstantiate(this.labelPrefab, this.parent, null).GetComponent<LocText>();
			label.tooltip = label.text.GetComponent<ToolTip>();
			label.text.transform.localScale = new Vector3(1f, 1f, 1f);
			this.labels.Add(label);
		}
		else
		{
			label = this.labels[this.activeLabelCount];
		}
		this.activeLabelCount++;
		label.text.text = text;
		label.tooltip.toolTip = "";
		label.tooltip.OnToolTip = null;
		label.text.gameObject.SetActive(true);
		return this;
	}

	// Token: 0x06005BC2 RID: 23490 RVA: 0x00217408 File Offset: 0x00215608
	public DetailsPanelDrawer BeginDrawing()
	{
		return this;
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x0021740B File Offset: 0x0021560B
	public DetailsPanelDrawer EndDrawing()
	{
		return this;
	}

	// Token: 0x04003D6C RID: 15724
	private List<DetailsPanelDrawer.Label> labels = new List<DetailsPanelDrawer.Label>();

	// Token: 0x04003D6D RID: 15725
	private int activeLabelCount;

	// Token: 0x04003D6E RID: 15726
	private UIStringFormatter stringformatter;

	// Token: 0x04003D6F RID: 15727
	private UIFloatFormatter floatFormatter;

	// Token: 0x04003D70 RID: 15728
	private GameObject parent;

	// Token: 0x04003D71 RID: 15729
	private GameObject labelPrefab;

	// Token: 0x02001CB0 RID: 7344
	private struct Label
	{
		// Token: 0x040084D2 RID: 34002
		public LocText text;

		// Token: 0x040084D3 RID: 34003
		public ToolTip tooltip;
	}
}
