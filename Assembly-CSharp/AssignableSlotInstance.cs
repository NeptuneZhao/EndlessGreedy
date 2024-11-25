using System;
using UnityEngine;

// Token: 0x02000655 RID: 1621
public abstract class AssignableSlotInstance
{
	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000E28BA File Offset: 0x000E0ABA
	// (set) Token: 0x060027D8 RID: 10200 RVA: 0x000E28C2 File Offset: 0x000E0AC2
	public Assignables assignables { get; private set; }

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060027D9 RID: 10201 RVA: 0x000E28CB File Offset: 0x000E0ACB
	public GameObject gameObject
	{
		get
		{
			return this.assignables.gameObject;
		}
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x000E28D8 File Offset: 0x000E0AD8
	public AssignableSlotInstance(Assignables assignables, AssignableSlot slot) : this(slot.Id, assignables, slot)
	{
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x000E28E8 File Offset: 0x000E0AE8
	public AssignableSlotInstance(string id, Assignables assignables, AssignableSlot slot)
	{
		this.ID = id;
		this.slot = slot;
		this.assignables = assignables;
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x000E2905 File Offset: 0x000E0B05
	public void Assign(Assignable assignable)
	{
		if (this.assignable == assignable)
		{
			return;
		}
		this.Unassign(false);
		this.assignable = assignable;
		this.assignables.Trigger(-1585839766, this);
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x000E2938 File Offset: 0x000E0B38
	public virtual void Unassign(bool trigger_event = true)
	{
		if (this.unassigning)
		{
			return;
		}
		if (this.IsAssigned())
		{
			this.unassigning = true;
			this.assignable.Unassign();
			if (trigger_event)
			{
				this.assignables.Trigger(-1585839766, this);
			}
			this.assignable = null;
			this.unassigning = false;
		}
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000E298A File Offset: 0x000E0B8A
	public bool IsAssigned()
	{
		return this.assignable != null;
	}

	// Token: 0x060027DF RID: 10207 RVA: 0x000E2998 File Offset: 0x000E0B98
	public bool IsUnassigning()
	{
		return this.unassigning;
	}

	// Token: 0x0400170C RID: 5900
	public string ID;

	// Token: 0x0400170D RID: 5901
	public AssignableSlot slot;

	// Token: 0x0400170E RID: 5902
	public Assignable assignable;

	// Token: 0x04001710 RID: 5904
	private bool unassigning;
}
