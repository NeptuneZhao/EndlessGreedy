using System;

// Token: 0x020004B4 RID: 1204
public class ClosestOxygenCanisterSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x060019FE RID: 6654 RVA: 0x0008A549 File Offset: 0x00088749
	public ClosestOxygenCanisterSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.Gas, shouldStartActive)
	{
		this.requiredTags = new Tag[]
		{
			GameTags.Breathable
		};
	}
}
