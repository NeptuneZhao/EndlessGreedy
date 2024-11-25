using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public abstract class ClosestPickupableSensor<T> : Sensor where T : Component
{
	// Token: 0x06001A00 RID: 6656 RVA: 0x0008A57F File Offset: 0x0008877F
	public ClosestPickupableSensor(Sensors sensors, Tag itemSearchTag, bool shouldStartActive) : base(sensors, shouldStartActive)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.itemSearchTag = itemSearchTag;
	}

	// Token: 0x06001A01 RID: 6657 RVA: 0x0008A5A7 File Offset: 0x000887A7
	public T GetItem()
	{
		return this.item;
	}

	// Token: 0x06001A02 RID: 6658 RVA: 0x0008A5AF File Offset: 0x000887AF
	public int GetItemNavCost()
	{
		if (!(this.item == null))
		{
			return this.itemNavCost;
		}
		return int.MaxValue;
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x0008A5D0 File Offset: 0x000887D0
	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		int maxValue = int.MaxValue;
		Pickupable pickupable = this.FindClosestPickupable(base.GetComponent<Storage>(), forbiddenTagSet, out maxValue, this.itemSearchTag, this.requiredTags);
		bool flag = this.itemInReachButNotPermitted;
		T t = default(T);
		bool flag2 = false;
		if (pickupable != null)
		{
			t = pickupable.GetComponent<T>();
			flag2 = true;
			flag = false;
		}
		else
		{
			int num;
			flag = (this.FindClosestPickupable(base.GetComponent<Storage>(), new HashSet<Tag>(), out num, this.itemSearchTag, this.requiredTags) != null);
		}
		if (t != this.item || this.isThereAnyItemAvailable != flag2)
		{
			this.item = t;
			this.itemNavCost = maxValue;
			this.isThereAnyItemAvailable = flag2;
			this.itemInReachButNotPermitted = flag;
			this.ItemChanged();
		}
	}

	// Token: 0x06001A04 RID: 6660 RVA: 0x0008A6A4 File Offset: 0x000888A4
	public Pickupable FindClosestPickupable(Storage destination, HashSet<Tag> exclude_tags, out int cost, Tag categoryTag, Tag[] otherRequiredTags = null)
	{
		ICollection<Pickupable> pickupables = base.gameObject.GetMyWorld().worldInventory.GetPickupables(categoryTag, false);
		if (pickupables == null)
		{
			cost = int.MaxValue;
			return null;
		}
		if (otherRequiredTags == null)
		{
			otherRequiredTags = new Tag[]
			{
				categoryTag
			};
		}
		Pickupable result = null;
		int num = int.MaxValue;
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, otherRequiredTags, destination))
			{
				int navigationCost = pickupable.GetNavigationCost(this.navigator, pickupable.cachedCell);
				if (navigationCost != -1 && navigationCost < num)
				{
					result = pickupable;
					num = navigationCost;
				}
			}
		}
		cost = num;
		return result;
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x0008A774 File Offset: 0x00088974
	public virtual void ItemChanged()
	{
		Action<T> onItemChanged = this.OnItemChanged;
		if (onItemChanged == null)
		{
			return;
		}
		onItemChanged(this.item);
	}

	// Token: 0x04000EC8 RID: 3784
	public Action<T> OnItemChanged;

	// Token: 0x04000EC9 RID: 3785
	protected T item;

	// Token: 0x04000ECA RID: 3786
	protected int itemNavCost = int.MaxValue;

	// Token: 0x04000ECB RID: 3787
	protected Tag itemSearchTag;

	// Token: 0x04000ECC RID: 3788
	protected Tag[] requiredTags;

	// Token: 0x04000ECD RID: 3789
	protected bool isThereAnyItemAvailable;

	// Token: 0x04000ECE RID: 3790
	protected bool itemInReachButNotPermitted;

	// Token: 0x04000ECF RID: 3791
	private Navigator navigator;
}
