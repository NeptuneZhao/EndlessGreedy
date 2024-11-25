using System;
using KSerialization;

// Token: 0x02000878 RID: 2168
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConverterOperationalRequirement : KMonoBehaviour
{
	// Token: 0x06003C96 RID: 15510 RVA: 0x0015045F File Offset: 0x0014E65F
	private void onStorageChanged(object _)
	{
		this.operational.SetFlag(this.sufficientResources, this.converter.HasEnoughMassToStartConverting(false));
	}

	// Token: 0x06003C97 RID: 15511 RVA: 0x0015047E File Offset: 0x0014E67E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.sufficientResources = new Operational.Flag("sufficientResources", this.operationalReq);
		base.Subscribe(-1697596308, new Action<object>(this.onStorageChanged));
		this.onStorageChanged(null);
	}

	// Token: 0x04002506 RID: 9478
	[MyCmpReq]
	private ElementConverter converter;

	// Token: 0x04002507 RID: 9479
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002508 RID: 9480
	private Operational.Flag.Type operationalReq;

	// Token: 0x04002509 RID: 9481
	private Operational.Flag sufficientResources;
}
