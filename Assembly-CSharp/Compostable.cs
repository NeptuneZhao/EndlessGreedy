using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200053E RID: 1342
[AddComponentMenu("KMonoBehaviour/scripts/Compostable")]
public class Compostable : KMonoBehaviour
{
	// Token: 0x06001EC6 RID: 7878 RVA: 0x000AB7D8 File Offset: 0x000A99D8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.isMarkedForCompost = base.GetComponent<KPrefabID>().HasTag(GameTags.Compostable);
		if (this.isMarkedForCompost)
		{
			this.MarkForCompost(false);
		}
		base.Subscribe<Compostable>(493375141, Compostable.OnRefreshUserMenuDelegate);
		base.Subscribe<Compostable>(856640610, Compostable.OnStoreDelegate);
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x000AB834 File Offset: 0x000A9A34
	private void MarkForCompost(bool force = false)
	{
		this.RefreshStatusItem();
		Storage storage = base.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Drop(base.gameObject, true);
		}
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x000AB86C File Offset: 0x000A9A6C
	private void OnToggleCompost()
	{
		if (!this.isMarkedForCompost)
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component.storage != null)
			{
				component.storage.Drop(base.gameObject, true);
			}
			Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.compostPrefab);
			if (pickupable != null)
			{
				SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
				return;
			}
		}
		else
		{
			Pickupable component2 = base.GetComponent<Pickupable>();
			Pickupable pickupable2 = EntitySplitter.Split(component2, component2.TotalAmount, this.originalPrefab);
			SelectTool.Instance.SelectNextFrame(pickupable2.GetComponent<KSelectable>(), true);
		}
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x000AB900 File Offset: 0x000A9B00
	private void RefreshStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, false);
		if (this.isMarkedForCompost)
		{
			if (base.GetComponent<Pickupable>() != null && base.GetComponent<Pickupable>().storage == null)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, null);
				return;
			}
			component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, null);
		}
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x000AB99A File Offset: 0x000A9B9A
	private void OnStore(object data)
	{
		this.RefreshStatusItem();
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x000AB9A4 File Offset: 0x000A9BA4
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.isMarkedForCompost)
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME_OFF, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0400114A RID: 4426
	[SerializeField]
	public bool isMarkedForCompost;

	// Token: 0x0400114B RID: 4427
	public GameObject originalPrefab;

	// Token: 0x0400114C RID: 4428
	public GameObject compostPrefab;

	// Token: 0x0400114D RID: 4429
	private static readonly EventSystem.IntraObjectHandler<Compostable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400114E RID: 4430
	private static readonly EventSystem.IntraObjectHandler<Compostable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnStore(data);
	});
}
