using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000758 RID: 1880
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Refinery")]
public class Refinery : KMonoBehaviour
{
	// Token: 0x06003260 RID: 12896 RVA: 0x00114AC4 File Offset: 0x00112CC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x020015D6 RID: 5590
	[Serializable]
	public struct OrderSaveData
	{
		// Token: 0x0600900E RID: 36878 RVA: 0x0034A5F8 File Offset: 0x003487F8
		public OrderSaveData(string id, bool infinite)
		{
			this.id = id;
			this.infinite = infinite;
		}

		// Token: 0x04006DF3 RID: 28147
		public string id;

		// Token: 0x04006DF4 RID: 28148
		public bool infinite;
	}
}
