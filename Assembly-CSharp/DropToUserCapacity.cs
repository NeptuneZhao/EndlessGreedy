using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000863 RID: 2147
[AddComponentMenu("KMonoBehaviour/Workable/DropToUserCapacity")]
public class DropToUserCapacity : Workable
{
	// Token: 0x06003BD5 RID: 15317 RVA: 0x001497A5 File Offset: 0x001479A5
	protected DropToUserCapacity()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x001497B8 File Offset: 0x001479B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		base.Subscribe<DropToUserCapacity>(-945020481, DropToUserCapacity.OnStorageCapacityChangedHandler);
		base.Subscribe<DropToUserCapacity>(-1697596308, DropToUserCapacity.OnStorageChangedHandler);
		this.synchronizeAnims = false;
		base.SetWorkTime(0.1f);
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x00149814 File Offset: 0x00147A14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateChore();
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x00149822 File Offset: 0x00147A22
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x0014983E File Offset: 0x00147A3E
	private void OnStorageChanged(object data)
	{
		this.UpdateChore();
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x00149848 File Offset: 0x00147A48
	public void UpdateChore()
	{
		IUserControlledCapacity component = base.GetComponent<IUserControlledCapacity>();
		if (component != null && component.AmountStored > component.UserMaxCapacity)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<DropToUserCapacity>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Cancelled emptying");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x001498DC File Offset: 0x00147ADC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		IUserControlledCapacity component2 = base.GetComponent<IUserControlledCapacity>();
		float num = Mathf.Max(0f, component2.AmountStored - component2.UserMaxCapacity);
		List<GameObject> list = new List<GameObject>(component.items);
		for (int i = 0; i < list.Count; i++)
		{
			Pickupable component3 = list[i].GetComponent<Pickupable>();
			if (component3.PrimaryElement.Mass > num)
			{
				component3.Take(num).transform.SetPosition(base.transform.GetPosition());
				return;
			}
			num -= component3.PrimaryElement.Mass;
			component.Drop(component3.gameObject, true);
		}
		this.chore = null;
	}

	// Token: 0x04002432 RID: 9266
	private Chore chore;

	// Token: 0x04002433 RID: 9267
	private bool showCmd;

	// Token: 0x04002434 RID: 9268
	private Storage[] storages;

	// Token: 0x04002435 RID: 9269
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageCapacityChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x04002436 RID: 9270
	private static readonly EventSystem.IntraObjectHandler<DropToUserCapacity> OnStorageChangedHandler = new EventSystem.IntraObjectHandler<DropToUserCapacity>(delegate(DropToUserCapacity component, object data)
	{
		component.OnStorageChanged(data);
	});
}
