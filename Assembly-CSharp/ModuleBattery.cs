using System;

// Token: 0x02000738 RID: 1848
public class ModuleBattery : Battery
{
	// Token: 0x06003119 RID: 12569 RVA: 0x0010F194 File Offset: 0x0010D394
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.connectedTags = new Tag[0];
		base.IsVirtual = true;
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x0010F1B0 File Offset: 0x0010D3B0
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}
}
