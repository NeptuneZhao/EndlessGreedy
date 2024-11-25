using System;
using System.Diagnostics;

// Token: 0x020009DD RID: 2525
[DebuggerDisplay("{slot.Id}")]
public class OwnableSlotInstance : AssignableSlotInstance
{
	// Token: 0x06004945 RID: 18757 RVA: 0x001A3D49 File Offset: 0x001A1F49
	public OwnableSlotInstance(Assignables assignables, OwnableSlot slot) : base(assignables, slot)
	{
	}
}
