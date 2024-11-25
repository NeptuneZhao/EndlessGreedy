using System;

// Token: 0x020005C5 RID: 1477
public class Storable : KMonoBehaviour
{
	// Token: 0x06002383 RID: 9091 RVA: 0x000C60A6 File Offset: 0x000C42A6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Storable>(856640610, Storable.OnStoreDelegate);
		base.Subscribe<Storable>(-778359855, Storable.RefreshStorageTagsDelegate);
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000C60D0 File Offset: 0x000C42D0
	public void OnStore(object data)
	{
		this.RefreshStorageTags(data);
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000C60DC File Offset: 0x000C42DC
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && (bool)data);
		Storage storage = (Storage)data;
		if (storage != null && storage.gameObject == base.gameObject)
		{
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		SaveLoadRoot component2 = base.GetComponent<SaveLoadRoot>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (component3)
		{
			component3.IsSelectable = !flag;
		}
		if (flag)
		{
			component.AddTag(GameTags.Stored, false);
			if (storage == null || !storage.allowItemRemoval)
			{
				component.AddTag(GameTags.StoredPrivate, false);
			}
			else
			{
				component.RemoveTag(GameTags.StoredPrivate);
			}
			if (component2 != null)
			{
				component2.SetRegistered(false);
				return;
			}
		}
		else
		{
			component.RemoveTag(GameTags.Stored);
			component.RemoveTag(GameTags.StoredPrivate);
			if (component2 != null)
			{
				component2.SetRegistered(true);
			}
		}
	}

	// Token: 0x04001432 RID: 5170
	private static readonly EventSystem.IntraObjectHandler<Storable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04001433 RID: 5171
	private static readonly EventSystem.IntraObjectHandler<Storable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.RefreshStorageTags(data);
	});
}
