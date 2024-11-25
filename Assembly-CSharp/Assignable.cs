using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000653 RID: 1619
public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x060027B9 RID: 10169 RVA: 0x000E204B File Offset: 0x000E024B
	public AssignableSlot slot
	{
		get
		{
			if (this._slot == null)
			{
				this._slot = Db.Get().AssignableSlots.Get(this.slotID);
			}
			return this._slot;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060027BA RID: 10170 RVA: 0x000E2076 File Offset: 0x000E0276
	public bool CanBeAssigned
	{
		get
		{
			return this.canBeAssigned;
		}
	}

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x060027BB RID: 10171 RVA: 0x000E2080 File Offset: 0x000E0280
	// (remove) Token: 0x060027BC RID: 10172 RVA: 0x000E20B8 File Offset: 0x000E02B8
	public event Action<IAssignableIdentity> OnAssign;

	// Token: 0x060027BD RID: 10173 RVA: 0x000E20ED File Offset: 0x000E02ED
	[OnDeserialized]
	internal void OnDeserialized()
	{
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x000E20F0 File Offset: 0x000E02F0
	private void RestoreAssignee()
	{
		IAssignableIdentity savedAssignee = this.GetSavedAssignee();
		if (savedAssignee != null)
		{
			AssignableSlotInstance savedSlotInstance = this.GetSavedSlotInstance(savedAssignee);
			this.Assign(savedAssignee, savedSlotInstance);
		}
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x000E2118 File Offset: 0x000E0318
	private AssignableSlotInstance GetSavedSlotInstance(IAssignableIdentity savedAsignee)
	{
		if ((savedAsignee != null && savedAsignee is MinionIdentity) || savedAsignee is StoredMinionIdentity || savedAsignee is MinionAssignablesProxy)
		{
			Ownables soleOwner = savedAsignee.GetSoleOwner();
			if (soleOwner != null)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				if (slots != null)
				{
					AssignableSlotInstance assignableSlotInstance = slots.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance != null)
					{
						return assignableSlotInstance;
					}
				}
			}
			Equipment component = soleOwner.GetComponent<Equipment>();
			if (component != null)
			{
				AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
				if (slots2 != null)
				{
					AssignableSlotInstance assignableSlotInstance2 = slots2.FindFirst((AssignableSlotInstance i) => i.ID == this.assignee_slotInstanceID);
					if (assignableSlotInstance2 != null)
					{
						return assignableSlotInstance2;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x000E21B8 File Offset: 0x000E03B8
	private IAssignableIdentity GetSavedAssignee()
	{
		if (this.assignee_identityRef.Get() != null)
		{
			return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
		}
		if (!string.IsNullOrEmpty(this.assignee_groupID))
		{
			return Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID];
		}
		return null;
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x000E2214 File Offset: 0x000E0414
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreAssignee();
		Components.AssignableItems.Add(this);
		Game.Instance.assignmentManager.Add(this);
		if (this.assignee == null && this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			GameObject targetGameObject = proxy.GetTargetGameObject();
			return targetGameObject.GetComponent<KMonoBehaviour>().GetMyWorldId() == this.GetMyWorldId() || targetGameObject.IsMyParentWorld(base.gameObject);
		});
		this.autoassignmentPreconditions.Add(delegate(MinionAssignablesProxy proxy)
		{
			Operational component = base.GetComponent<Operational>();
			return !(component != null) || component.IsOperational;
		});
	}

	// Token: 0x060027C2 RID: 10178 RVA: 0x000E22A5 File Offset: 0x000E04A5
	protected override void OnCleanUp()
	{
		this.Unassign();
		Components.AssignableItems.Remove(this);
		Game.Instance.assignmentManager.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060027C3 RID: 10179 RVA: 0x000E22D0 File Offset: 0x000E04D0
	public bool CanAutoAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		if (!this.CanAssignTo(minionAssignablesProxy))
		{
			return false;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.autoassignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060027C4 RID: 10180 RVA: 0x000E2348 File Offset: 0x000E0548
	public bool CanAssignTo(IAssignableIdentity identity)
	{
		MinionAssignablesProxy minionAssignablesProxy = identity as MinionAssignablesProxy;
		if (minionAssignablesProxy == null)
		{
			return true;
		}
		using (List<Func<MinionAssignablesProxy, bool>>.Enumerator enumerator = this.assignmentPreconditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current(minionAssignablesProxy))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x000E23B4 File Offset: 0x000E05B4
	public bool IsAssigned()
	{
		return this.assignee != null;
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x000E23C0 File Offset: 0x000E05C0
	public bool IsAssignedTo(IAssignableIdentity identity)
	{
		global::Debug.Assert(identity != null, "IsAssignedTo identity is null");
		Ownables soleOwner = identity.GetSoleOwner();
		global::Debug.Assert(soleOwner != null, "IsAssignedTo identity sole owner is null");
		if (this.assignee != null)
		{
			foreach (Ownables ownables in this.assignee.GetOwners())
			{
				global::Debug.Assert(ownables, "Assignable owners list contained null");
				if (ownables.gameObject == soleOwner.gameObject)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x000E2468 File Offset: 0x000E0668
	public virtual void Assign(IAssignableIdentity new_assignee)
	{
		this.Assign(new_assignee, null);
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000E2474 File Offset: 0x000E0674
	public virtual void Assign(IAssignableIdentity new_assignee, AssignableSlotInstance specificSlotInstance)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee is KMonoBehaviour)
		{
			if (!this.CanAssignTo(new_assignee))
			{
				return;
			}
			this.assignee_identityRef.Set((KMonoBehaviour)new_assignee);
			this.assignee_groupID = "";
		}
		else if (new_assignee is AssignmentGroup)
		{
			this.assignee_identityRef.Set(null);
			this.assignee_groupID = ((AssignmentGroup)new_assignee).id;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Assigned, false);
		this.assignee = new_assignee;
		this.assignee_slotInstanceID = null;
		if (this.slot != null && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			if (specificSlotInstance == null)
			{
				Ownables soleOwner = new_assignee.GetSoleOwner();
				if (soleOwner != null)
				{
					AssignableSlotInstance slot = soleOwner.GetSlot(this.slot);
					if (slot != null)
					{
						this.assignee_slotInstanceID = slot.ID;
						slot.Assign(this);
					}
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance slot2 = component.GetSlot(this.slot);
					if (slot2 != null)
					{
						this.assignee_slotInstanceID = slot2.ID;
						slot2.Assign(this);
					}
				}
			}
			else
			{
				this.assignee_slotInstanceID = specificSlotInstance.ID;
				specificSlotInstance.Assign(this);
			}
		}
		if (this.OnAssign != null)
		{
			this.OnAssign(new_assignee);
		}
		base.Trigger(684616645, new_assignee);
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x000E25C0 File Offset: 0x000E07C0
	public virtual void Unassign()
	{
		if (this.assignee == null)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
		if (this.slot != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				AssignableSlotInstance[] slots = soleOwner.GetSlots(this.slot);
				AssignableSlotInstance assignableSlotInstance = (slots == null) ? null : slots.FindFirst((AssignableSlotInstance s) => s.assignable == this);
				if (assignableSlotInstance != null)
				{
					assignableSlotInstance.Unassign(true);
				}
				Equipment component = soleOwner.GetComponent<Equipment>();
				if (component != null)
				{
					AssignableSlotInstance[] slots2 = component.GetSlots(this.slot);
					assignableSlotInstance = ((slots2 == null) ? null : slots2.FindFirst((AssignableSlotInstance s) => s.assignable == this));
					if (assignableSlotInstance != null)
					{
						assignableSlotInstance.Unassign(true);
					}
				}
			}
		}
		this.assignee = null;
		if (this.canBePublic)
		{
			this.Assign(Game.Instance.assignmentManager.assignment_groups["public"]);
		}
		this.assignee_slotInstanceID = null;
		this.assignee_identityRef.Set(null);
		this.assignee_groupID = "";
		if (this.OnAssign != null)
		{
			this.OnAssign(null);
		}
		base.Trigger(684616645, null);
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x000E26E4 File Offset: 0x000E08E4
	public void SetCanBeAssigned(bool state)
	{
		this.canBeAssigned = state;
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000E26ED File Offset: 0x000E08ED
	public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.assignmentPreconditions.Add(precondition);
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000E26FB File Offset: 0x000E08FB
	public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
	{
		this.autoassignmentPreconditions.Add(precondition);
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x000E270C File Offset: 0x000E090C
	public int GetNavigationCost(Navigator navigator)
	{
		int num = -1;
		int cell = Grid.PosToCell(this);
		IApproachable component = base.GetComponent<IApproachable>();
		CellOffset[] array = (component != null) ? component.GetOffsets() : new CellOffset[1];
		DebugUtil.DevAssert(navigator != null, "Navigator is mysteriously null", null);
		if (navigator == null)
		{
			return -1;
		}
		foreach (CellOffset offset in array)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			int navigationCost = navigator.GetNavigationCost(cell2);
			if (navigationCost != -1 && (num == -1 || navigationCost < num))
			{
				num = navigationCost;
			}
		}
		return num;
	}

	// Token: 0x040016FF RID: 5887
	public string slotID;

	// Token: 0x04001700 RID: 5888
	private AssignableSlot _slot;

	// Token: 0x04001701 RID: 5889
	public IAssignableIdentity assignee;

	// Token: 0x04001702 RID: 5890
	[Serialize]
	protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();

	// Token: 0x04001703 RID: 5891
	[Serialize]
	protected string assignee_slotInstanceID;

	// Token: 0x04001704 RID: 5892
	[Serialize]
	private string assignee_groupID = "";

	// Token: 0x04001705 RID: 5893
	public AssignableSlot[] subSlots;

	// Token: 0x04001706 RID: 5894
	public bool canBePublic;

	// Token: 0x04001707 RID: 5895
	[Serialize]
	private bool canBeAssigned = true;

	// Token: 0x04001708 RID: 5896
	private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();

	// Token: 0x04001709 RID: 5897
	private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
}
