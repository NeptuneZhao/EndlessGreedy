using System;

// Token: 0x020004AF RID: 1199
public class AssignableReachabilitySensor : Sensor
{
	// Token: 0x060019EE RID: 6638 RVA: 0x00089E2C File Offset: 0x0008802C
	public AssignableReachabilitySensor(Sensors sensors) : base(sensors)
	{
		MinionAssignablesProxy minionAssignablesProxy = base.gameObject.GetComponent<MinionIdentity>().assignableProxy.Get();
		minionAssignablesProxy.ConfigureAssignableSlots();
		Assignables[] components = minionAssignablesProxy.GetComponents<Assignables>();
		if (components.Length == 0)
		{
			Debug.LogError(base.gameObject.GetProperName() + ": No 'Assignables' components found for AssignableReachabilitySensor");
		}
		int num = 0;
		foreach (Assignables assignables in components)
		{
			num += assignables.Slots.Count;
		}
		this.slots = new AssignableReachabilitySensor.SlotEntry[num];
		int num2 = 0;
		foreach (Assignables assignables2 in components)
		{
			for (int k = 0; k < assignables2.Slots.Count; k++)
			{
				this.slots[num2++].slot = assignables2.Slots[k];
			}
		}
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x00089F14 File Offset: 0x00088114
	public bool IsReachable(AssignableSlot slot)
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			if (this.slots[i].slot.slot == slot)
			{
				return this.slots[i].isReachable;
			}
		}
		Debug.LogError("Could not find slot: " + ((slot != null) ? slot.ToString() : null));
		return false;
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00089F7C File Offset: 0x0008817C
	public override void Update()
	{
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableReachabilitySensor.SlotEntry slotEntry = this.slots[i];
			AssignableSlotInstance slot = slotEntry.slot;
			if (slot.IsAssigned())
			{
				bool flag = slot.assignable.GetNavigationCost(this.navigator) != -1;
				Operational component = slot.assignable.GetComponent<Operational>();
				if (component != null)
				{
					flag = (flag && component.IsOperational);
				}
				if (flag != slotEntry.isReachable)
				{
					slotEntry.isReachable = flag;
					this.slots[i] = slotEntry;
					base.Trigger(334784980, slotEntry);
				}
			}
			else if (slotEntry.isReachable)
			{
				slotEntry.isReachable = false;
				this.slots[i] = slotEntry;
				base.Trigger(334784980, slotEntry);
			}
		}
	}

	// Token: 0x04000EBC RID: 3772
	private AssignableReachabilitySensor.SlotEntry[] slots;

	// Token: 0x04000EBD RID: 3773
	private Navigator navigator;

	// Token: 0x02001277 RID: 4727
	private struct SlotEntry
	{
		// Token: 0x04006387 RID: 25479
		public AssignableSlotInstance slot;

		// Token: 0x04006388 RID: 25480
		public bool isReachable;
	}
}
