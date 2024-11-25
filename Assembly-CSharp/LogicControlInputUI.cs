using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C9B RID: 3227
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicControlInputUI : KMonoBehaviour
{
	// Token: 0x06006350 RID: 25424 RVA: 0x0024FDAC File Offset: 0x0024DFAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.colourDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
		this.icon.raycastTarget = false;
		this.border.raycastTarget = false;
	}

	// Token: 0x06006351 RID: 25425 RVA: 0x0024FE34 File Offset: 0x0024E034
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 c = (network == null) ? GlobalAssets.Instance.colorSet.logicDisconnected : (network.IsBitActive(0) ? this.colourOn : this.colourOff);
		this.icon.color = c;
	}

	// Token: 0x04004362 RID: 17250
	[SerializeField]
	private Image icon;

	// Token: 0x04004363 RID: 17251
	[SerializeField]
	private Image border;

	// Token: 0x04004364 RID: 17252
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04004365 RID: 17253
	private Color32 colourOn;

	// Token: 0x04004366 RID: 17254
	private Color32 colourOff;

	// Token: 0x04004367 RID: 17255
	private Color32 colourDisconnected;
}
