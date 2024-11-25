using System;

// Token: 0x020004B3 RID: 1203
public class ClosestElectrobankSensor : ClosestPickupableSensor<Electrobank>
{
	// Token: 0x060019FD RID: 6653 RVA: 0x0008A53A File Offset: 0x0008873A
	public ClosestElectrobankSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.ChargedPortableBattery, shouldStartActive)
	{
	}
}
