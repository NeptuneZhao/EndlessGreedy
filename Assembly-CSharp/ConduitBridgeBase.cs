using System;

// Token: 0x020006A5 RID: 1701
public class ConduitBridgeBase : KMonoBehaviour
{
	// Token: 0x06002ABA RID: 10938 RVA: 0x000F0D72 File Offset: 0x000EEF72
	protected void SendEmptyOnMassTransfer()
	{
		if (this.OnMassTransfer != null)
		{
			this.OnMassTransfer(SimHashes.Void, 0f, 0f, 0, 0, null);
		}
	}

	// Token: 0x04001896 RID: 6294
	public ConduitBridgeBase.DesiredMassTransfer desiredMassTransfer;

	// Token: 0x04001897 RID: 6295
	public ConduitBridgeBase.ConduitBridgeEvent OnMassTransfer;

	// Token: 0x0200149C RID: 5276
	// (Invoke) Token: 0x06008B88 RID: 35720
	public delegate float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);

	// Token: 0x0200149D RID: 5277
	// (Invoke) Token: 0x06008B8C RID: 35724
	public delegate void ConduitBridgeEvent(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);
}
