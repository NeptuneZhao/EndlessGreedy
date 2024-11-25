using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020006DA RID: 1754
[AddComponentMenu("KMonoBehaviour/Workable/GeneShuffler")]
public class GeneShuffler : Workable
{
	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06002C67 RID: 11367 RVA: 0x000F950A File Offset: 0x000F770A
	public bool WorkComplete
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working.complete);
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06002C68 RID: 11368 RVA: 0x000F952C File Offset: 0x000F772C
	public bool IsWorking
	{
		get
		{
			return this.geneShufflerSMI.IsInsideState(this.geneShufflerSMI.sm.working);
		}
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x000F9549 File Offset: 0x000F7749
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.assignable.OnAssign += this.Assign;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x000F9570 File Offset: 0x000F7770
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.showProgressBar = false;
		this.geneShufflerSMI = new GeneShuffler.GeneShufflerSM.Instance(this);
		this.RefreshRechargeChore();
		this.RefreshConsumedState();
		base.Subscribe<GeneShuffler>(-1697596308, GeneShuffler.OnStorageChangeDelegate);
		this.geneShufflerSMI.StartSM();
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x000F95BE File Offset: 0x000F77BE
	private void Assign(IAssignableIdentity new_assignee)
	{
		this.CancelChore();
		if (new_assignee != null)
		{
			this.ActivateChore();
		}
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x000F95CF File Offset: 0x000F77CF
	private void Recharge()
	{
		this.SetConsumed(false);
		this.RequestRecharge(false);
		this.RefreshRechargeChore();
		this.RefreshSideScreen();
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x000F95EB File Offset: 0x000F77EB
	private void SetConsumed(bool consumed)
	{
		this.IsConsumed = consumed;
		this.RefreshConsumedState();
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x000F95FA File Offset: 0x000F77FA
	private void RefreshConsumedState()
	{
		this.geneShufflerSMI.sm.isCharged.Set(!this.IsConsumed, this.geneShufflerSMI, false);
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x000F9624 File Offset: 0x000F7824
	private void OnStorageChange(object data)
	{
		if (this.storage_recursion_guard)
		{
			return;
		}
		this.storage_recursion_guard = true;
		if (this.IsConsumed)
		{
			for (int i = this.storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.storage.items[i];
				if (!(gameObject == null) && gameObject.IsPrefabID(GeneShuffler.RechargeTag))
				{
					this.storage.ConsumeIgnoringDisease(gameObject);
					this.Recharge();
					break;
				}
			}
		}
		this.storage_recursion_guard = false;
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000F96AC File Offset: 0x000F78AC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.notification = new Notification(MISC.NOTIFICATIONS.GENESHUFFLER.NAME, NotificationType.Good, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.GENESHUFFLER.TOOLTIP + notificationList.ReduceMessages(false), null, false, 0f, null, null, null, true, false, false);
		this.notifier.Add(this.notification, "");
		this.DeSelectBuilding();
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000F971E File Offset: 0x000F791E
	private void DeSelectBuilding()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x000F9739 File Offset: 0x000F7939
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x000F9743 File Offset: 0x000F7943
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("aborted");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x000F9775 File Offset: 0x000F7975
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.chore != null)
		{
			this.chore.Cancel("stopped");
		}
		this.notifier.Remove(this.notification);
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x000F97A8 File Offset: 0x000F79A8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		CameraController.Instance.CameraGoTo(base.transform.GetPosition(), 1f, false);
		this.ApplyRandomTrait(worker);
		this.assignable.Unassign();
		this.DeSelectBuilding();
		this.notifier.Remove(this.notification);
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x000F9800 File Offset: 0x000F7A00
	private void ApplyRandomTrait(WorkerBase worker)
	{
		Traits component = worker.GetComponent<Traits>();
		List<string> list = new List<string>();
		foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.GENESHUFFLERTRAITS)
		{
			if (!component.HasTrait(traitVal.id))
			{
				list.Add(traitVal.id);
			}
		}
		if (list.Count > 0)
		{
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			Trait trait = Db.Get().traits.TryGet(id);
			worker.GetComponent<Traits>().Add(trait);
			InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			string text = string.Format(UI.GENESHUFFLERMESSAGE.BODY_SUCCESS, worker.GetProperName(), trait.Name, trait.GetTooltip());
			infoDialogScreen.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text).AddDefaultOK(false);
			this.SetConsumed(true);
			return;
		}
		InfoDialogScreen infoDialogScreen2 = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		string text2 = string.Format(UI.GENESHUFFLERMESSAGE.BODY_FAILURE, worker.GetProperName());
		infoDialogScreen2.SetHeader(UI.GENESHUFFLERMESSAGE.HEADER).AddPlainText(text2).AddDefaultOK(false);
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x000F9990 File Offset: 0x000F7B90
	private void ActivateChore()
	{
		global::Debug.Assert(this.chore == null);
		base.GetComponent<Workable>().SetWorkTime(float.PositiveInfinity);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.GeneShuffle, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim("anim_interacts_neuralvacillator_kanim"), false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x000F9A00 File Offset: 0x000F7C00
	private void CancelChore()
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x000F9A22 File Offset: 0x000F7C22
	private void CompleteChore()
	{
		this.chore.Cleanup();
		this.chore = null;
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x000F9A36 File Offset: 0x000F7C36
	public void RequestRecharge(bool request)
	{
		this.RechargeRequested = request;
		this.RefreshRechargeChore();
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000F9A45 File Offset: 0x000F7C45
	private void RefreshRechargeChore()
	{
		this.delivery.Pause(!this.RechargeRequested, "No recharge requested");
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x000F9A60 File Offset: 0x000F7C60
	public void RefreshSideScreen()
	{
		if (base.GetComponent<KSelectable>().IsSelected)
		{
			DetailsScreen.Instance.Refresh(base.gameObject);
		}
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x000F9A7F File Offset: 0x000F7C7F
	public void SetAssignable(bool set_it)
	{
		this.assignable.SetCanBeAssigned(set_it);
		this.RefreshSideScreen();
	}

	// Token: 0x040019A1 RID: 6561
	[MyCmpReq]
	public Assignable assignable;

	// Token: 0x040019A2 RID: 6562
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x040019A3 RID: 6563
	[MyCmpReq]
	public ManualDeliveryKG delivery;

	// Token: 0x040019A4 RID: 6564
	[MyCmpReq]
	public Storage storage;

	// Token: 0x040019A5 RID: 6565
	[Serialize]
	public bool IsConsumed;

	// Token: 0x040019A6 RID: 6566
	[Serialize]
	public bool RechargeRequested;

	// Token: 0x040019A7 RID: 6567
	private Chore chore;

	// Token: 0x040019A8 RID: 6568
	private GeneShuffler.GeneShufflerSM.Instance geneShufflerSMI;

	// Token: 0x040019A9 RID: 6569
	private Notification notification;

	// Token: 0x040019AA RID: 6570
	private static Tag RechargeTag = new Tag("GeneShufflerRecharge");

	// Token: 0x040019AB RID: 6571
	private static readonly EventSystem.IntraObjectHandler<GeneShuffler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<GeneShuffler>(delegate(GeneShuffler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x040019AC RID: 6572
	private bool storage_recursion_guard;

	// Token: 0x020014E5 RID: 5349
	public class GeneShufflerSM : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler>
	{
		// Token: 0x06008C84 RID: 35972 RVA: 0x0033ABCC File Offset: 0x00338DCC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("on").Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(true);
			}).Exit(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.SetAssignable(false);
			}).WorkableStartTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pre).ParamTransition<bool>(this.isCharged, this.consumed, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsFalse);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.working.complete);
			this.working.complete.ToggleStatusItem(Db.Get().BuildingStatusItems.GeneShuffleCompleted, null).Enter(delegate(GeneShuffler.GeneShufflerSM.Instance smi)
			{
				smi.master.RefreshSideScreen();
			}).WorkableStopTransition((GeneShuffler.GeneShufflerSM.Instance smi) => smi.master, this.working.pst);
			this.working.pst.OnAnimQueueComplete(this.consumed);
			this.consumed.PlayAnim("off", KAnim.PlayMode.Once).ParamTransition<bool>(this.isCharged, this.recharging, GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.IsTrue);
			this.recharging.PlayAnim("recharging", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		}

		// Token: 0x04006B4F RID: 27471
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State idle;

		// Token: 0x04006B50 RID: 27472
		public GeneShuffler.GeneShufflerSM.WorkingStates working;

		// Token: 0x04006B51 RID: 27473
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State consumed;

		// Token: 0x04006B52 RID: 27474
		public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State recharging;

		// Token: 0x04006B53 RID: 27475
		public StateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.BoolParameter isCharged;

		// Token: 0x020024DA RID: 9434
		public class WorkingStates : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State
		{
			// Token: 0x0400A3B1 RID: 41905
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pre;

			// Token: 0x0400A3B2 RID: 41906
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State loop;

			// Token: 0x0400A3B3 RID: 41907
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State complete;

			// Token: 0x0400A3B4 RID: 41908
			public GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.State pst;
		}

		// Token: 0x020024DB RID: 9435
		public new class Instance : GameStateMachine<GeneShuffler.GeneShufflerSM, GeneShuffler.GeneShufflerSM.Instance, GeneShuffler, object>.GameInstance
		{
			// Token: 0x0600BBC7 RID: 48071 RVA: 0x003D5726 File Offset: 0x003D3926
			public Instance(GeneShuffler master) : base(master)
			{
			}
		}
	}
}
