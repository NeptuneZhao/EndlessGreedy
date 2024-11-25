using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007A7 RID: 1959
[AddComponentMenu("KMonoBehaviour/Workable/Carvable")]
public class Carvable : Workable, IDigActionEntity
{
	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06003593 RID: 13715 RVA: 0x0012359F File Offset: 0x0012179F
	public bool IsMarkedForCarve
	{
		get
		{
			return this.isMarkedForCarve;
		}
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x001235A8 File Offset: 0x001217A8
	protected Carvable()
	{
		this.buttonLabel = UI.USERMENUACTIONS.CARVE.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.CARVE.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELCARVE.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELCARVE.TOOLTIP;
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x00123604 File Offset: 0x00121804
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = new StatusItem("PendingCarve", "MISC", "status_item_pending_carve", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem = new StatusItem("Carving", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		this.workerStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Workable workable = (Workable)data;
			if (workable != null && workable.GetComponent<KSelectable>() != null)
			{
				str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
			}
			return str;
		};
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x001236B8 File Offset: 0x001218B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		base.Subscribe<Carvable>(2127324410, Carvable.OnCancelDelegate);
		base.Subscribe<Carvable>(493375141, Carvable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Prioritizable.AddRef(base.gameObject);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForCarve)
		{
			this.MarkForCarve(true);
		}
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x00123770 File Offset: 0x00121970
	public void Carve()
	{
		this.isMarkedForCarve = false;
		this.chore = null;
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.workerStatusItem, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
		this.ProducePickupable(this.dropItemPrefabId);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x001237E0 File Offset: 0x001219E0
	public void MarkForCarve(bool instantOnDebug = true)
	{
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Carve();
			return;
		}
		if (this.chore == null)
		{
			this.isMarkedForCarve = true;
			this.chore = new WorkChore<Carvable>(Db.Get().ChoreTypes.Dig, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x00123861 File Offset: 0x00121A61
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Carve();
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x0012386C File Offset: 0x00121A6C
	private void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
		}
		this.isMarkedForCarve = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x001238C7 File Offset: 0x00121AC7
	private void OnClickCarve()
	{
		this.MarkForCarve(true);
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x001238D0 File Offset: 0x00121AD0
	protected void OnClickCancelCarve()
	{
		this.OnCancel(null);
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x001238DC File Offset: 0x00121ADC
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_carve", this.cancelButtonLabel, new System.Action(this.OnClickCancelCarve), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_carve", this.buttonLabel, new System.Action(this.OnClickCarve), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x0012396E File Offset: 0x00121B6E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x00123986 File Offset: 0x00121B86
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(this.pendingStatusItem, false);
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x001239A4 File Offset: 0x00121BA4
	private GameObject ProducePickupable(string pickupablePrefabId)
	{
		if (pickupablePrefabId != null)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(pickupablePrefabId)), position, Grid.SceneLayer.Ore, null, 0);
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			gameObject.GetComponent<PrimaryElement>().Temperature = component.Temperature;
			gameObject.SetActive(true);
			string properName = gameObject.GetProperName();
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, properName, gameObject.transform, 1.5f, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x00123A47 File Offset: 0x00121C47
	public void Dig()
	{
		this.Carve();
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x00123A4F File Offset: 0x00121C4F
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForCarve(instantOnDebug);
	}

	// Token: 0x04001FD8 RID: 8152
	[Serialize]
	protected bool isMarkedForCarve;

	// Token: 0x04001FD9 RID: 8153
	protected Chore chore;

	// Token: 0x04001FDA RID: 8154
	private string buttonLabel;

	// Token: 0x04001FDB RID: 8155
	private string buttonTooltip;

	// Token: 0x04001FDC RID: 8156
	private string cancelButtonLabel;

	// Token: 0x04001FDD RID: 8157
	private string cancelButtonTooltip;

	// Token: 0x04001FDE RID: 8158
	private StatusItem pendingStatusItem;

	// Token: 0x04001FDF RID: 8159
	public bool showUserMenuButtons = true;

	// Token: 0x04001FE0 RID: 8160
	public string dropItemPrefabId;

	// Token: 0x04001FE1 RID: 8161
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001FE2 RID: 8162
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04001FE3 RID: 8163
	private static readonly EventSystem.IntraObjectHandler<Carvable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Carvable>(delegate(Carvable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
