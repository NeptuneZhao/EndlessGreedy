using System;
using UnityEngine;

// Token: 0x02000572 RID: 1394
public class InfraredVisualizerComponents : KGameObjectComponentManager<InfraredVisualizerData>
{
	// Token: 0x06002045 RID: 8261 RVA: 0x000B50F3 File Offset: 0x000B32F3
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new InfraredVisualizerData(go));
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000B5104 File Offset: 0x000B3304
	public void UpdateTemperature()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				Vector3 position = controller.transform.GetPosition();
				if (visibleArea.Min <= position && position <= visibleArea.Max)
				{
					this.data[i].Update();
				}
			}
		}
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000B5194 File Offset: 0x000B3394
	public void ClearOverlayColour()
	{
		Color32 c = Color.black;
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				controller.OverlayColour = c;
			}
		}
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x000B51E9 File Offset: 0x000B33E9
	public static void ClearOverlayColour(KBatchedAnimController controller)
	{
		if (controller != null)
		{
			controller.OverlayColour = Color.black;
		}
	}
}
