using System;
using UnityEngine;

// Token: 0x0200012E RID: 302
[AddComponentMenu("KMonoBehaviour/scripts/UpdateElementConsumerPosition")]
public class UpdateElementConsumerPosition : KMonoBehaviour, ISim200ms
{
	// Token: 0x060005D3 RID: 1491 RVA: 0x000294D9 File Offset: 0x000276D9
	public void Sim200ms(float dt)
	{
		base.GetComponent<ElementConsumer>().RefreshConsumptionRate();
	}
}
