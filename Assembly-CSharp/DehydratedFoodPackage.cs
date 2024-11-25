using System;
using System.Linq;
using FoodRehydrator;
using KSerialization;
using UnityEngine;

// Token: 0x0200083B RID: 2107
public class DehydratedFoodPackage : Workable, IApproachable
{
	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x06003AE7 RID: 15079 RVA: 0x00143964 File Offset: 0x00141B64
	// (set) Token: 0x06003AE8 RID: 15080 RVA: 0x00143993 File Offset: 0x00141B93
	public GameObject Rehydrator
	{
		get
		{
			Storage storage = base.gameObject.GetComponent<Pickupable>().storage;
			if (storage != null)
			{
				return storage.gameObject;
			}
			return null;
		}
		private set
		{
		}
	}

	// Token: 0x06003AE9 RID: 15081 RVA: 0x00143995 File Offset: 0x00141B95
	public override BuildingFacade GetBuildingFacade()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<BuildingFacade>();
	}

	// Token: 0x06003AEA RID: 15082 RVA: 0x001439B2 File Offset: 0x00141BB2
	public override KAnimControllerBase GetAnimController()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06003AEB RID: 15083 RVA: 0x001439D0 File Offset: 0x00141BD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetOffsets(new CellOffset[]
		{
			default(CellOffset),
			new CellOffset(0, -1)
		});
		if (this.storage.items.Count < 1)
		{
			this.storage.ConsumeAllIgnoringDisease(this.FoodTag);
			int cell = Grid.PosToCell(this);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.FoodTag), Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			gameObject.GetComponent<Edible>().Calories = 1000000f;
			this.storage.Store(gameObject, false, false, true, false);
		}
		base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
		this.DehydrateItem(this.storage.items.ElementAtOrDefault(0));
	}

	// Token: 0x06003AEC RID: 15084 RVA: 0x00143A9C File Offset: 0x00141C9C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (this.Rehydrator != null)
		{
			DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
			if (component != null)
			{
				component.SetFabricatedFoodSymbol(this.FoodTag);
			}
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(this);
		}
	}

	// Token: 0x06003AED RID: 15085 RVA: 0x00143AF0 File Offset: 0x00141CF0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.storage.items.Count != 1)
		{
			DebugUtil.DevAssert(false, "OnCompleteWork invalid contents of package", null);
			return;
		}
		GameObject gameObject = this.storage.items[0];
		this.storage.Transfer(worker.GetComponent<Storage>(), false, false);
		DebugUtil.DevAssert(this.Rehydrator != null, "OnCompleteWork but no rehydrator", null);
		DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
		this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		component.ConsumeResourcesForRehydration(base.gameObject, gameObject);
		DehydratedFoodPackage.RehydrateStartWorkItem rehydrateStartWorkItem = (DehydratedFoodPackage.RehydrateStartWorkItem)worker.GetStartWorkInfo();
		if (rehydrateStartWorkItem != null && rehydrateStartWorkItem.setResultCb != null && gameObject != null)
		{
			rehydrateStartWorkItem.setResultCb(gameObject);
		}
	}

	// Token: 0x06003AEE RID: 15086 RVA: 0x00143BB4 File Offset: 0x00141DB4
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Rehydrator != null)
		{
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		}
	}

	// Token: 0x06003AEF RID: 15087 RVA: 0x00143BDC File Offset: 0x00141DDC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x00143BE4 File Offset: 0x00141DE4
	private void StorageChangeHandler(object obj)
	{
		GameObject item = (GameObject)obj;
		DebugUtil.DevAssert(!this.storage.items.Contains(item), "Attempting to add item to a dehydrated food package which is not allowed", null);
		this.RehydrateItem(item);
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x00143C20 File Offset: 0x00141E20
	public void DehydrateItem(GameObject item)
	{
		DebugUtil.DevAssert(item != null, "Attempting to dehydrate contents of an empty packet", null);
		if (this.storage.items.Count != 1 || item == null)
		{
			DebugUtil.DevAssert(false, "DehydrateItem called, incorrect content", null);
			return;
		}
		item.AddTag(GameTags.Dehydrated);
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x00143C74 File Offset: 0x00141E74
	public void RehydrateItem(GameObject item)
	{
		if (this.storage.items.Count != 0)
		{
			DebugUtil.DevAssert(false, "RehydrateItem called, incorrect storage content", null);
			return;
		}
		item.RemoveTag(GameTags.Dehydrated);
		item.AddTag(GameTags.Rehydrated);
		item.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
	}

	// Token: 0x06003AF3 RID: 15091 RVA: 0x00143CD8 File Offset: 0x00141ED8
	private void Swap<Type>(ref Type a, ref Type b)
	{
		Type type = a;
		a = b;
		b = type;
	}

	// Token: 0x040023C8 RID: 9160
	[Serialize]
	public Tag FoodTag;

	// Token: 0x040023C9 RID: 9161
	[MyCmpReq]
	private Storage storage;

	// Token: 0x02001758 RID: 5976
	public class RehydrateStartWorkItem : WorkerBase.StartWorkInfo
	{
		// Token: 0x06009567 RID: 38247 RVA: 0x0035F7B5 File Offset: 0x0035D9B5
		public RehydrateStartWorkItem(DehydratedFoodPackage pkg, Action<GameObject> setResultCB) : base(pkg)
		{
			this.package = pkg;
			this.setResultCb = setResultCB;
		}

		// Token: 0x04007283 RID: 29315
		public DehydratedFoodPackage package;

		// Token: 0x04007284 RID: 29316
		public Action<GameObject> setResultCb;
	}
}
