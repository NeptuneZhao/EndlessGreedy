using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000862 RID: 2146
[AddComponentMenu("KMonoBehaviour/Workable/DropAllWorkable")]
public class DropAllWorkable : Workable
{
	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x00149341 File Offset: 0x00147541
	// (set) Token: 0x06003BC9 RID: 15305 RVA: 0x00149349 File Offset: 0x00147549
	private Chore Chore
	{
		get
		{
			return this._chore;
		}
		set
		{
			this._chore = value;
			this.markedForDrop = (this._chore != null);
		}
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x00149361 File Offset: 0x00147561
	protected DropAllWorkable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x00149380 File Offset: 0x00147580
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<DropAllWorkable>(493375141, DropAllWorkable.OnRefreshUserMenuDelegate);
		base.Subscribe<DropAllWorkable>(-1697596308, DropAllWorkable.OnStorageChangeDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.synchronizeAnims = false;
		base.SetWorkTime(this.dropWorkTime);
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x001493E8 File Offset: 0x001475E8
	private Storage[] GetStorages()
	{
		if (this.storages == null)
		{
			this.storages = base.GetComponents<Storage>();
		}
		return this.storages;
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x00149404 File Offset: 0x00147604
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showCmd = this.GetNewShowCmd();
		if (this.markedForDrop)
		{
			this.DropAll();
		}
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x00149428 File Offset: 0x00147628
	public void DropAll()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
		}
		else if (this.Chore == null)
		{
			ChoreType chore_type = (!string.IsNullOrEmpty(this.choreTypeID)) ? Db.Get().ChoreTypes.Get(this.choreTypeID) : Db.Get().ChoreTypes.EmptyStorage;
			this.Chore = new WorkChore<DropAllWorkable>(chore_type, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
		else
		{
			this.Chore.Cancel("Cancelled emptying");
			this.Chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
			base.ShowProgressBar(false);
		}
		this.RefreshStatusItem();
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x001494DC File Offset: 0x001476DC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			List<GameObject> list = new List<GameObject>(array[i].items);
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject = array[i].Drop(list[j], true);
				if (gameObject != null)
				{
					foreach (Tag tag in this.removeTags)
					{
						gameObject.RemoveTag(tag);
					}
					gameObject.Trigger(580035959, worker);
					if (this.resetTargetWorkableOnCompleteWork)
					{
						Pickupable component = gameObject.GetComponent<Pickupable>();
						component.targetWorkable = component;
						component.SetOffsetTable(OffsetGroups.InvertedStandardTable);
					}
				}
			}
		}
		this.Chore = null;
		this.RefreshStatusItem();
		base.Trigger(-1957399615, null);
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x001495D8 File Offset: 0x001477D8
	private void OnRefreshUserMenu(object data)
	{
		if (this.showCmd)
		{
			KIconButtonMenu.ButtonInfo button = (this.Chore == null) ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME_OFF, new System.Action(this.DropAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP_OFF, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x0014967C File Offset: 0x0014787C
	private bool GetNewShowCmd()
	{
		bool flag = false;
		Storage[] array = this.GetStorages();
		for (int i = 0; i < array.Length; i++)
		{
			flag = (flag || !array[i].IsEmpty());
		}
		return flag;
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x001496B4 File Offset: 0x001478B4
	private void OnStorageChange(object data)
	{
		bool newShowCmd = this.GetNewShowCmd();
		if (newShowCmd != this.showCmd)
		{
			this.showCmd = newShowCmd;
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x001496F0 File Offset: 0x001478F0
	private void RefreshStatusItem()
	{
		if (this.Chore != null && this.statusItem == Guid.Empty)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.statusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
			return;
		}
		if (this.Chore == null && this.statusItem != Guid.Empty)
		{
			KSelectable component2 = base.GetComponent<KSelectable>();
			this.statusItem = component2.RemoveStatusItem(this.statusItem, false);
		}
	}

	// Token: 0x04002426 RID: 9254
	[Serialize]
	private bool markedForDrop;

	// Token: 0x04002427 RID: 9255
	private Chore _chore;

	// Token: 0x04002428 RID: 9256
	private bool showCmd;

	// Token: 0x04002429 RID: 9257
	private Storage[] storages;

	// Token: 0x0400242A RID: 9258
	public float dropWorkTime = 0.1f;

	// Token: 0x0400242B RID: 9259
	public string choreTypeID;

	// Token: 0x0400242C RID: 9260
	[MyCmpAdd]
	private Prioritizable _prioritizable;

	// Token: 0x0400242D RID: 9261
	public List<Tag> removeTags;

	// Token: 0x0400242E RID: 9262
	public bool resetTargetWorkableOnCompleteWork;

	// Token: 0x0400242F RID: 9263
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002430 RID: 9264
	private static readonly EventSystem.IntraObjectHandler<DropAllWorkable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DropAllWorkable>(delegate(DropAllWorkable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002431 RID: 9265
	private Guid statusItem;
}
