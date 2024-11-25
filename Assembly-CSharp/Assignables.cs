using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000656 RID: 1622
[AddComponentMenu("KMonoBehaviour/scripts/Assignables")]
public class Assignables : KMonoBehaviour
{
	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060027E0 RID: 10208 RVA: 0x000E29A0 File Offset: 0x000E0BA0
	public List<AssignableSlotInstance> Slots
	{
		get
		{
			return this.slots;
		}
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x000E29A8 File Offset: 0x000E0BA8
	protected IAssignableIdentity GetAssignableIdentity()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null)
		{
			return component.assignableProxy.Get();
		}
		return base.GetComponent<MinionAssignablesProxy>();
	}

	// Token: 0x060027E2 RID: 10210 RVA: 0x000E29D7 File Offset: 0x000E0BD7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameUtil.SubscribeToTags<Assignables>(this, Assignables.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x060027E3 RID: 10211 RVA: 0x000E29EC File Offset: 0x000E0BEC
	private void OnDeath(object data)
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x060027E4 RID: 10212 RVA: 0x000E2A40 File Offset: 0x000E0C40
	public void Add(AssignableSlotInstance slot_instance)
	{
		this.slots.Add(slot_instance);
	}

	// Token: 0x060027E5 RID: 10213 RVA: 0x000E2A50 File Offset: 0x000E0C50
	public Assignable GetAssignable(AssignableSlot slot)
	{
		AssignableSlotInstance slot2 = this.GetSlot(slot);
		if (slot2 == null)
		{
			return null;
		}
		return slot2.assignable;
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x000E2A70 File Offset: 0x000E0C70
	public AssignableSlotInstance GetSlot(AssignableSlot slot)
	{
		global::Debug.Assert(this.slots.Count > 0, "GetSlot called with no slots configured");
		if (slot == null)
		{
			return null;
		}
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.slot == slot)
			{
				return assignableSlotInstance;
			}
		}
		return null;
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x000E2AEC File Offset: 0x000E0CEC
	public AssignableSlotInstance[] GetSlots(AssignableSlot slot)
	{
		global::Debug.Assert(this.slots.Count > 0, "GetSlot called with no slots configured");
		if (slot == null)
		{
			return null;
		}
		List<AssignableSlotInstance> list = this.slots.FindAll((AssignableSlotInstance s) => s.slot == slot);
		if (list != null && list.Count > 0)
		{
			return list.ToArray();
		}
		return null;
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x000E2B54 File Offset: 0x000E0D54
	public Assignable AutoAssignSlot(AssignableSlot slot)
	{
		Assignable assignable = this.GetAssignable(slot);
		if (assignable != null)
		{
			return assignable;
		}
		GameObject targetGameObject = base.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject == null)
		{
			global::Debug.LogWarning("AutoAssignSlot failed, proxy game object was null.");
			return null;
		}
		Navigator component = targetGameObject.GetComponent<Navigator>();
		IAssignableIdentity assignableIdentity = this.GetAssignableIdentity();
		int num = int.MaxValue;
		foreach (Assignable assignable2 in Game.Instance.assignmentManager)
		{
			if (!(assignable2 == null) && !assignable2.IsAssigned() && assignable2.slot == slot && assignable2.CanAutoAssignTo(assignableIdentity))
			{
				int navigationCost = assignable2.GetNavigationCost(component);
				if (navigationCost != -1 && navigationCost < num)
				{
					num = navigationCost;
					assignable = assignable2;
				}
			}
		}
		if (assignable != null)
		{
			assignable.Assign(assignableIdentity);
		}
		return assignable;
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000E2C44 File Offset: 0x000E0E44
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			assignableSlotInstance.Unassign(true);
		}
	}

	// Token: 0x04001711 RID: 5905
	protected List<AssignableSlotInstance> slots = new List<AssignableSlotInstance>();

	// Token: 0x04001712 RID: 5906
	private static readonly EventSystem.IntraObjectHandler<Assignables> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<Assignables>(GameTags.Dead, delegate(Assignables component, object data)
	{
		component.OnDeath(data);
	});
}
