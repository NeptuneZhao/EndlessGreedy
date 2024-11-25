using System;
using KSerialization;

// Token: 0x020009DE RID: 2526
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
	// Token: 0x06004946 RID: 18758 RVA: 0x001A3D53 File Offset: 0x001A1F53
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x001A3D5C File Offset: 0x001A1F5C
	public void UnassignAll()
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.assignable != null)
			{
				assignableSlotInstance.assignable.Unassign();
			}
		}
	}
}
