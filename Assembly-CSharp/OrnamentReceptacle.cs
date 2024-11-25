using System;
using UnityEngine;

// Token: 0x02000744 RID: 1860
public class OrnamentReceptacle : SingleEntityReceptacle
{
	// Token: 0x06003192 RID: 12690 RVA: 0x00110E2C File Offset: 0x0010F02C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x00110E34 File Offset: 0x0010F034
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapTo_ornament", false);
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x00110E54 File Offset: 0x0010F054
	protected override void PositionOccupyingObject()
	{
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.transform.SetLocalPosition(new Vector3(0f, 0f, -0.1f));
		this.occupyingTracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
		this.occupyingTracker.symbol = new HashedString("snapTo_ornament");
		this.occupyingTracker.forceAlwaysVisible = true;
		this.animLink = new KAnimLink(base.GetComponent<KBatchedAnimController>(), component);
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x00110ED4 File Offset: 0x0010F0D4
	protected override void ClearOccupant()
	{
		if (this.occupyingTracker != null)
		{
			UnityEngine.Object.Destroy(this.occupyingTracker);
			this.occupyingTracker = null;
		}
		if (this.animLink != null)
		{
			this.animLink.Unregister();
			this.animLink = null;
		}
		base.ClearOccupant();
	}

	// Token: 0x04001D24 RID: 7460
	[MyCmpReq]
	private SnapOn snapOn;

	// Token: 0x04001D25 RID: 7461
	private KBatchedAnimTracker occupyingTracker;

	// Token: 0x04001D26 RID: 7462
	private KAnimLink animLink;
}
