using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C9C RID: 3228
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicRibbonDisplayUI : KMonoBehaviour
{
	// Token: 0x06006353 RID: 25427 RVA: 0x0024FE88 File Offset: 0x0024E088
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.wire1.raycastTarget = false;
		this.wire2.raycastTarget = false;
		this.wire3.raycastTarget = false;
		this.wire4.raycastTarget = false;
	}

	// Token: 0x06006354 RID: 25428 RVA: 0x0024FF14 File Offset: 0x0024E114
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 color = this.colourDisconnected;
		List<Color32> list = new List<Color32>();
		for (int i = 0; i < this.bitDepth; i++)
		{
			list.Add((network == null) ? color : (network.IsBitActive(i) ? this.colourOn : this.colourOff));
		}
		if (this.wire1.color != list[0])
		{
			this.wire1.color = list[0];
		}
		if (this.wire2.color != list[1])
		{
			this.wire2.color = list[1];
		}
		if (this.wire3.color != list[2])
		{
			this.wire3.color = list[2];
		}
		if (this.wire4.color != list[3])
		{
			this.wire4.color = list[3];
		}
	}

	// Token: 0x04004368 RID: 17256
	[SerializeField]
	private Image wire1;

	// Token: 0x04004369 RID: 17257
	[SerializeField]
	private Image wire2;

	// Token: 0x0400436A RID: 17258
	[SerializeField]
	private Image wire3;

	// Token: 0x0400436B RID: 17259
	[SerializeField]
	private Image wire4;

	// Token: 0x0400436C RID: 17260
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x0400436D RID: 17261
	private Color32 colourOn;

	// Token: 0x0400436E RID: 17262
	private Color32 colourOff;

	// Token: 0x0400436F RID: 17263
	private Color32 colourDisconnected = new Color(255f, 255f, 255f, 255f);

	// Token: 0x04004370 RID: 17264
	private int bitDepth = 4;
}
