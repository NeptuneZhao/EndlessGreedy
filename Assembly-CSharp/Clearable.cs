using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000539 RID: 1337
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Clearable")]
public class Clearable : Workable, ISaveLoadable, IRender1000ms
{
	// Token: 0x06001E78 RID: 7800 RVA: 0x000A9A48 File Offset: 0x000A7C48
	protected override void OnPrefabInit()
	{
		base.Subscribe<Clearable>(2127324410, Clearable.OnCancelDelegate);
		base.Subscribe<Clearable>(856640610, Clearable.OnStoreDelegate);
		base.Subscribe<Clearable>(-2064133523, Clearable.OnAbsorbDelegate);
		base.Subscribe<Clearable>(493375141, Clearable.OnRefreshUserMenuDelegate);
		base.Subscribe<Clearable>(-1617557748, Clearable.OnEquippedDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Clearing;
		this.simRenderLoadBalance = true;
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x000A9AD0 File Offset: 0x000A7CD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForClear)
		{
			if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
			{
				if (!base.transform.parent.GetComponent<Storage>().allowClearable)
				{
					this.isMarkedForClear = false;
				}
				else
				{
					this.MarkForClear(true, true);
				}
			}
			else
			{
				this.MarkForClear(true, false);
			}
		}
		this.RefreshClearableStatus(true);
	}

	// Token: 0x06001E7A RID: 7802 RVA: 0x000A9B3B File Offset: 0x000A7D3B
	private void OnStore(object data)
	{
		this.CancelClearing();
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x000A9B44 File Offset: 0x000A7D44
	private void OnCancel(object data)
	{
		for (ObjectLayerListItem objectLayerListItem = this.pickupable.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
		{
			if (objectLayerListItem.gameObject != null)
			{
				objectLayerListItem.gameObject.GetComponent<Clearable>().CancelClearing();
			}
		}
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x000A9B88 File Offset: 0x000A7D88
	public void CancelClearing()
	{
		if (this.isMarkedForClear)
		{
			this.isMarkedForClear = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Garbage);
			Prioritizable.RemoveRef(base.gameObject);
			if (this.clearHandle.IsValid())
			{
				GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
				this.clearHandle.Clear();
			}
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Remove(this);
		}
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x000A9BFC File Offset: 0x000A7DFC
	public void MarkForClear(bool restoringFromSave = false, bool allowWhenStored = false)
	{
		if (!this.isClearable)
		{
			return;
		}
		if ((!this.isMarkedForClear || restoringFromSave) && !this.pickupable.IsEntombed && !this.clearHandle.IsValid() && (!this.HasTag(GameTags.Stored) || allowWhenStored))
		{
			Prioritizable.AddRef(base.gameObject);
			this.pickupable.KPrefabID.AddTag(GameTags.Garbage, false);
			this.isMarkedForClear = true;
			this.clearHandle = GlobalChoreProvider.Instance.RegisterClearable(this);
			this.RefreshClearableStatus(true);
			SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
		}
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x000A9C9C File Offset: 0x000A7E9C
	private void OnClickClear()
	{
		this.MarkForClear(false, false);
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x000A9CA6 File Offset: 0x000A7EA6
	private void OnClickCancel()
	{
		this.CancelClearing();
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x000A9CAE File Offset: 0x000A7EAE
	private void OnEquipped(object data)
	{
		this.CancelClearing();
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x000A9CB6 File Offset: 0x000A7EB6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.clearHandle.IsValid())
		{
			GlobalChoreProvider.Instance.UnregisterClearable(this.clearHandle);
			this.clearHandle.Clear();
		}
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x000A9CE8 File Offset: 0x000A7EE8
	private void OnRefreshUserMenu(object data)
	{
		if (!this.isClearable || base.GetComponent<Health>() != null || this.pickupable.KPrefabID.HasTag(GameTags.Stored) || this.pickupable.KPrefabID.HasTag(GameTags.MarkedForMove))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForClear ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME_OFF, new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.CLEAR.NAME, new System.Action(this.OnClickClear), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEAR.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x000A9DC8 File Offset: 0x000A7FC8
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Clearable component = pickupable.GetComponent<Clearable>();
			if (component != null && component.isMarkedForClear)
			{
				this.MarkForClear(false, false);
			}
		}
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x000A9E05 File Offset: 0x000A8005
	public void Render1000ms(float dt)
	{
		this.RefreshClearableStatus(false);
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x000A9E10 File Offset: 0x000A8010
	public void RefreshClearableStatus(bool force_update)
	{
		if (force_update || this.isMarkedForClear)
		{
			bool show = false;
			bool show2 = false;
			if (this.isMarkedForClear)
			{
				show2 = !(show = GlobalChoreProvider.Instance.ClearableHasDestination(this.pickupable));
			}
			this.pendingClearGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClear, this.pendingClearGuid, show, this);
			this.pendingClearNoStorageGuid = this.selectable.ToggleStatusItem(Db.Get().MiscStatusItems.PendingClearNoStorage, this.pendingClearNoStorageGuid, show2, this);
		}
	}

	// Token: 0x04001125 RID: 4389
	[MyCmpReq]
	private Pickupable pickupable;

	// Token: 0x04001126 RID: 4390
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04001127 RID: 4391
	[Serialize]
	private bool isMarkedForClear;

	// Token: 0x04001128 RID: 4392
	private HandleVector<int>.Handle clearHandle;

	// Token: 0x04001129 RID: 4393
	public bool isClearable = true;

	// Token: 0x0400112A RID: 4394
	private Guid pendingClearGuid;

	// Token: 0x0400112B RID: 4395
	private Guid pendingClearNoStorageGuid;

	// Token: 0x0400112C RID: 4396
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x0400112D RID: 4397
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x0400112E RID: 4398
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x0400112F RID: 4399
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001130 RID: 4400
	private static readonly EventSystem.IntraObjectHandler<Clearable> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Clearable>(delegate(Clearable component, object data)
	{
		component.OnEquipped(data);
	});
}
