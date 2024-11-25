using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D86 RID: 3462
public class OwnablesSecondSideScreen : KScreen
{
	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x06006CE2 RID: 27874 RVA: 0x0028F435 File Offset: 0x0028D635
	// (set) Token: 0x06006CE1 RID: 27873 RVA: 0x0028F42C File Offset: 0x0028D62C
	public AssignableSlotInstance Slot { get; private set; }

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06006CE4 RID: 27876 RVA: 0x0028F446 File Offset: 0x0028D646
	// (set) Token: 0x06006CE3 RID: 27875 RVA: 0x0028F43D File Offset: 0x0028D63D
	public IAssignableIdentity OwnerIdentity { get; private set; }

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06006CE5 RID: 27877 RVA: 0x0028F44E File Offset: 0x0028D64E
	public AssignableSlot SlotType
	{
		get
		{
			if (this.Slot != null)
			{
				return this.Slot.slot;
			}
			return null;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06006CE6 RID: 27878 RVA: 0x0028F465 File Offset: 0x0028D665
	public Assignable CurrentSlotItem
	{
		get
		{
			if (!this.HasItem)
			{
				return null;
			}
			return this.Slot.assignable;
		}
	}

	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06006CE7 RID: 27879 RVA: 0x0028F47C File Offset: 0x0028D67C
	public bool HasItem
	{
		get
		{
			return this.Slot != null && this.Slot.IsAssigned();
		}
	}

	// Token: 0x06006CE8 RID: 27880 RVA: 0x0028F493 File Offset: 0x0028D693
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.originalRow.gameObject.SetActive(false);
		MultiToggle multiToggle = this.noneRow;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnNoneRowClicked));
	}

	// Token: 0x06006CE9 RID: 27881 RVA: 0x0028F4D3 File Offset: 0x0028D6D3
	private void OnNoneRowClicked()
	{
		this.UnassignCurrentItem();
		this.RefreshNoneRow();
	}

	// Token: 0x06006CEA RID: 27882 RVA: 0x0028F4E1 File Offset: 0x0028D6E1
	protected override void OnCmpDisable()
	{
		this.SetSlot(null);
		base.OnCmpDisable();
	}

	// Token: 0x06006CEB RID: 27883 RVA: 0x0028F4F0 File Offset: 0x0028D6F0
	public void SetSlot(AssignableSlotInstance slot)
	{
		Components.AssignableItems.Unregister(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
		this.Slot = slot;
		this.OwnerIdentity = ((slot == null) ? null : slot.assignables.GetComponent<IAssignableIdentity>());
		if (this.Slot != null)
		{
			Components.AssignableItems.Register(new Action<Assignable>(this.OnNewItemAvailable), new Action<Assignable>(this.OnItemUnregistered));
		}
		this.RefreshItemListOptions();
	}

	// Token: 0x06006CEC RID: 27884 RVA: 0x0028F570 File Offset: 0x0028D770
	public void SortRows()
	{
		if (this.itemRows != null)
		{
			OwnablesSecondSideScreenRow ownablesSecondSideScreenRow = null;
			for (int i = 0; i < this.itemRows.Count; i++)
			{
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow2 = this.itemRows[i];
				if (ownablesSecondSideScreenRow2.item == null || ownablesSecondSideScreenRow2.item.IsAssigned())
				{
					if (ownablesSecondSideScreenRow == null && ownablesSecondSideScreenRow2 != null && ownablesSecondSideScreenRow2.item != null && ownablesSecondSideScreenRow2.item.IsAssigned() && ownablesSecondSideScreenRow2.item == this.CurrentSlotItem)
					{
						ownablesSecondSideScreenRow = ownablesSecondSideScreenRow2;
					}
					else
					{
						ownablesSecondSideScreenRow2.transform.SetAsLastSibling();
					}
				}
			}
			if (ownablesSecondSideScreenRow != null)
			{
				ownablesSecondSideScreenRow.transform.SetAsFirstSibling();
			}
		}
		this.noneRow.transform.SetAsFirstSibling();
	}

	// Token: 0x06006CED RID: 27885 RVA: 0x0028F640 File Offset: 0x0028D840
	public void RefreshItemListOptions()
	{
		GameObject gameObject = (this.OwnerIdentity == null) ? null : this.OwnerIdentity.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		int worldID = (this.OwnerIdentity == null) ? 255 : gameObject.GetMyWorldId();
		List<Assignable> list = null;
		int b = 0;
		if (worldID != 255)
		{
			list = Components.AssignableItems.Items.FindAll(delegate(Assignable i)
			{
				bool flag = i.slotID == this.SlotType.Id && i.CanAssignTo(this.OwnerIdentity);
				if (flag && i is Equippable)
				{
					Equippable equippable = i as Equippable;
					GameObject gameObject2 = equippable.gameObject;
					if (equippable.isEquipped)
					{
						gameObject2 = equippable.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
					}
					flag = (gameObject2.GetMyWorldId() == worldID);
				}
				return flag;
			});
			b = list.Count;
		}
		for (int j = 0; j < Mathf.Max(this.itemRows.Count, b); j++)
		{
			if (list != null && j < list.Count)
			{
				Assignable assignable = list[j];
				if (j >= this.itemRows.Count)
				{
					OwnablesSecondSideScreenRow item = this.CreateItemRow(assignable);
					this.itemRows.Add(item);
				}
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow = this.itemRows[j];
				ownablesSecondSideScreenRow.gameObject.SetActive(true);
				ownablesSecondSideScreenRow.SetData(this.Slot, assignable);
			}
			else
			{
				OwnablesSecondSideScreenRow ownablesSecondSideScreenRow2 = this.itemRows[j];
				ownablesSecondSideScreenRow2.ClearData();
				ownablesSecondSideScreenRow2.gameObject.SetActive(false);
			}
		}
		this.SortRows();
		this.RefreshNoneRow();
	}

	// Token: 0x06006CEE RID: 27886 RVA: 0x0028F787 File Offset: 0x0028D987
	private void RefreshNoneRow()
	{
		this.noneRow.ChangeState(this.HasItem ? 0 : 1);
	}

	// Token: 0x06006CEF RID: 27887 RVA: 0x0028F7A0 File Offset: 0x0028D9A0
	private OwnablesSecondSideScreenRow CreateItemRow(Assignable item)
	{
		OwnablesSecondSideScreenRow component = Util.KInstantiateUI(this.originalRow.gameObject, this.originalRow.transform.parent.gameObject, false).GetComponent<OwnablesSecondSideScreenRow>();
		component.OnRowClicked = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowClicked, new Action<OwnablesSecondSideScreenRow>(this.OnItemRowClicked));
		component.OnRowItemAssigneeChanged = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowItemAssigneeChanged, new Action<OwnablesSecondSideScreenRow>(this.OnItemRowAsigneeChanged));
		component.OnRowItemDestroyed = (Action<OwnablesSecondSideScreenRow>)Delegate.Combine(component.OnRowItemDestroyed, new Action<OwnablesSecondSideScreenRow>(this.OnItemDestroyed));
		return component;
	}

	// Token: 0x06006CF0 RID: 27888 RVA: 0x0028F83E File Offset: 0x0028DA3E
	private void OnItemDestroyed(OwnablesSecondSideScreenRow correspondingItemRow)
	{
		correspondingItemRow.ClearData();
		correspondingItemRow.gameObject.SetActive(false);
	}

	// Token: 0x06006CF1 RID: 27889 RVA: 0x0028F852 File Offset: 0x0028DA52
	private void OnItemRowAsigneeChanged(OwnablesSecondSideScreenRow correspondingItemRow)
	{
		correspondingItemRow.Refresh();
		this.SortRows();
		this.RefreshNoneRow();
	}

	// Token: 0x06006CF2 RID: 27890 RVA: 0x0028F868 File Offset: 0x0028DA68
	private void OnItemRowClicked(OwnablesSecondSideScreenRow rowClicked)
	{
		Assignable item = rowClicked.item;
		bool flag = item.IsAssigned() && item.assignee is AssignmentGroup;
		bool flag2 = item.IsAssigned() && item.IsAssignedTo(this.OwnerIdentity) && !flag && this.Slot.IsAssigned() && this.Slot.assignable == item;
		if (item.IsAssigned())
		{
			item.Unassign();
		}
		if (!flag2)
		{
			item.Assign(this.OwnerIdentity, this.Slot);
		}
		rowClicked.Refresh();
		this.RefreshNoneRow();
	}

	// Token: 0x06006CF3 RID: 27891 RVA: 0x0028F8FE File Offset: 0x0028DAFE
	private void UnassignCurrentItem()
	{
		if (this.Slot != null)
		{
			this.Slot.Unassign(true);
			this.RefreshItemListOptions();
		}
	}

	// Token: 0x06006CF4 RID: 27892 RVA: 0x0028F91A File Offset: 0x0028DB1A
	private void OnNewItemAvailable(Assignable item)
	{
		if (this.Slot != null && item.slotID == this.SlotType.Id)
		{
			this.RefreshItemListOptions();
		}
	}

	// Token: 0x06006CF5 RID: 27893 RVA: 0x0028F942 File Offset: 0x0028DB42
	private void OnItemUnregistered(Assignable item)
	{
		if (this.Slot != null && item.slotID == this.SlotType.Id)
		{
			this.RefreshItemListOptions();
		}
	}

	// Token: 0x04004A3E RID: 19006
	public MultiToggle noneRow;

	// Token: 0x04004A3F RID: 19007
	public OwnablesSecondSideScreenRow originalRow;

	// Token: 0x04004A42 RID: 19010
	public System.Action OnScreenDeactivated;

	// Token: 0x04004A43 RID: 19011
	private List<OwnablesSecondSideScreenRow> itemRows = new List<OwnablesSecondSideScreenRow>();
}
