using System;

// Token: 0x020004B5 RID: 1205
public class ClosestOxyliteSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x060019FF RID: 6655 RVA: 0x0008A570 File Offset: 0x00088770
	public ClosestOxyliteSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.OxyRock, shouldStartActive)
	{
	}
}
