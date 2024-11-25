﻿using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009DB RID: 2523
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x0600493D RID: 18749 RVA: 0x001A3A80 File Offset: 0x001A1C80
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Ownables>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x0600493E RID: 18750 RVA: 0x001A3B1C File Offset: 0x001A1D1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateTint();
		this.UpdateStatusString();
		base.OnAssign += this.OnNewAssignment;
		if (this.assignee == null)
		{
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component)
			{
				List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
				if (storedMinionInfo.Count > 0)
				{
					Ref<KPrefabID> serializedMinion = storedMinionInfo[0].serializedMinion;
					if (serializedMinion != null && serializedMinion.GetId() != -1)
					{
						StoredMinionIdentity component2 = serializedMinion.Get().GetComponent<StoredMinionIdentity>();
						component2.ValidateProxy();
						this.Assign(component2);
					}
				}
			}
		}
	}

	// Token: 0x0600493F RID: 18751 RVA: 0x001A3BA6 File Offset: 0x001A1DA6
	private void OnNewAssignment(IAssignableIdentity assignables)
	{
		this.UpdateTint();
		this.UpdateStatusString();
	}

	// Token: 0x06004940 RID: 18752 RVA: 0x001A3BB4 File Offset: 0x001A1DB4
	private void UpdateTint()
	{
		if (this.tintWhenUnassigned)
		{
			KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
			if (component != null && component.HasBatchInstanceData)
			{
				component.TintColour = ((this.assignee == null) ? this.unownedTint : this.ownedTint);
				return;
			}
			KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
			if (component2 != null && component2.HasBatchInstanceData)
			{
				component2.TintColour = ((this.assignee == null) ? this.unownedTint : this.ownedTint);
			}
		}
	}

	// Token: 0x06004941 RID: 18753 RVA: 0x001A3C3C File Offset: 0x001A1E3C
	private void UpdateStatusString()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		StatusItem status_item;
		if (this.assignee != null)
		{
			if (this.assignee is MinionIdentity)
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
			else if (this.assignee is Room)
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
			else
			{
				status_item = Db.Get().BuildingStatusItems.AssignedTo;
			}
		}
		else
		{
			status_item = Db.Get().BuildingStatusItems.Unassigned;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.Ownable, status_item, this);
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x001A3CDC File Offset: 0x001A1EDC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ASSIGNEDDUPLICANT, UI.BUILDINGEFFECTS.TOOLTIPS.ASSIGNEDDUPLICANT, Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

	// Token: 0x04002FEA RID: 12266
	public bool tintWhenUnassigned = true;

	// Token: 0x04002FEB RID: 12267
	private Color unownedTint = Color.gray;

	// Token: 0x04002FEC RID: 12268
	private Color ownedTint = Color.white;
}
