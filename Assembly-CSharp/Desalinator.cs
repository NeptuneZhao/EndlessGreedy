using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006B2 RID: 1714
[SerializationConfig(MemberSerialization.OptIn)]
public class Desalinator : StateMachineComponent<Desalinator.StatesInstance>
{
	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06002B2A RID: 11050 RVA: 0x000F276E File Offset: 0x000F096E
	// (set) Token: 0x06002B2B RID: 11051 RVA: 0x000F2776 File Offset: 0x000F0976
	public float SaltStorageLeft
	{
		get
		{
			return this._storageLeft;
		}
		set
		{
			this._storageLeft = value;
			base.smi.sm.saltStorageLeft.Set(value, base.smi, false);
		}
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000F27A0 File Offset: 0x000F09A0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.OnConduitConnectionChanged(base.GetComponent<ConduitConsumer>().IsConnected);
		base.Subscribe<Desalinator>(-2094018600, Desalinator.OnConduitConnectionChangedDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x000F27F4 File Offset: 0x000F09F4
	private void OnConduitConnectionChanged(object data)
	{
		bool pause = (bool)data;
		foreach (ManualDeliveryKG manualDeliveryKG in this.deliveryComponents)
		{
			Element element = ElementLoader.GetElement(manualDeliveryKG.RequestedItemTag);
			if (element != null && element.IsLiquid)
			{
				manualDeliveryKG.Pause(pause, "pipe connected");
			}
		}
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x000F2848 File Offset: 0x000F0A48
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.HasSalt || base.smi.emptyChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_desalinator_needs_emptying", UI.USERMENUACTIONS.EMPTYDESALINATOR.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyEmpty);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x000F28DC File Offset: 0x000F0ADC
	private bool CheckCanConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].CanConvertAtAll())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x000F2924 File Offset: 0x000F0B24
	private bool CheckEnoughMassToConvert()
	{
		if (this.converters == null)
		{
			this.converters = base.GetComponents<ElementConverter>();
		}
		for (int i = 0; i < this.converters.Length; i++)
		{
			if (this.converters[i].HasEnoughMassToStartConverting(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040018C5 RID: 6341
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040018C6 RID: 6342
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x040018C7 RID: 6343
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040018C8 RID: 6344
	[Serialize]
	public float maxSalt = 1000f;

	// Token: 0x040018C9 RID: 6345
	[Serialize]
	private float _storageLeft = 1000f;

	// Token: 0x040018CA RID: 6346
	private ElementConverter[] converters;

	// Token: 0x040018CB RID: 6347
	private static readonly EventSystem.IntraObjectHandler<Desalinator> OnConduitConnectionChangedDelegate = new EventSystem.IntraObjectHandler<Desalinator>(delegate(Desalinator component, object data)
	{
		component.OnConduitConnectionChanged(data);
	});

	// Token: 0x020014A6 RID: 5286
	public class StatesInstance : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.GameInstance
	{
		// Token: 0x06008BAA RID: 35754 RVA: 0x00337714 File Offset: 0x00335914
		public StatesInstance(Desalinator smi) : base(smi)
		{
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06008BAB RID: 35755 RVA: 0x0033771D File Offset: 0x0033591D
		public bool HasSalt
		{
			get
			{
				return base.master.storage.Has(ElementLoader.FindElementByHash(SimHashes.Salt).tag);
			}
		}

		// Token: 0x06008BAC RID: 35756 RVA: 0x0033773E File Offset: 0x0033593E
		public bool IsFull()
		{
			return base.master.SaltStorageLeft <= 0f;
		}

		// Token: 0x06008BAD RID: 35757 RVA: 0x00337755 File Offset: 0x00335955
		public bool IsSaltRemoved()
		{
			return !this.HasSalt;
		}

		// Token: 0x06008BAE RID: 35758 RVA: 0x00337760 File Offset: 0x00335960
		public void CreateEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("dupe");
			}
			DesalinatorWorkableEmpty component = base.master.GetComponent<DesalinatorWorkableEmpty>();
			this.emptyChore = new WorkChore<DesalinatorWorkableEmpty>(Db.Get().ChoreTypes.EmptyDesalinator, component, null, true, new Action<Chore>(this.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06008BAF RID: 35759 RVA: 0x003377C8 File Offset: 0x003359C8
		public void CancelEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("Cancelled");
				this.emptyChore = null;
			}
		}

		// Token: 0x06008BB0 RID: 35760 RVA: 0x003377EC File Offset: 0x003359EC
		private void OnEmptyComplete(Chore chore)
		{
			this.emptyChore = null;
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			ListPool<GameObject, Desalinator>.PooledList pooledList = ListPool<GameObject, Desalinator>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x06008BB1 RID: 35761 RVA: 0x00337878 File Offset: 0x00335A78
		public void UpdateStorageLeft()
		{
			Tag tag = GameTagExtensions.Create(SimHashes.Salt);
			base.master.SaltStorageLeft = base.master.maxSalt - base.master.storage.GetMassAvailable(tag);
		}

		// Token: 0x04006A8A RID: 27274
		public Chore emptyChore;
	}

	// Token: 0x020014A7 RID: 5287
	public class States : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator>
	{
		// Token: 0x06008BB2 RID: 35762 RVA: 0x003378B8 File Offset: 0x00335AB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (Desalinator.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (Desalinator.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (Desalinator.StatesInstance smi) => smi.master.CheckEnoughMassToConvert());
			this.on.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.on.working_pst, (Desalinator.StatesInstance smi) => !smi.master.CheckCanConvert()).ParamTransition<float>(this.saltStorageLeft, this.full, (Desalinator.StatesInstance smi, float p) => smi.IsFull()).EventHandler(GameHashes.OnStorageChange, delegate(Desalinator.StatesInstance smi)
			{
				smi.UpdateStorageLeft();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.waiting);
			this.earlyEmpty.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForEmpty);
			this.earlyWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.full.PlayAnims((Desalinator.StatesInstance smi) => Desalinator.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForEmpty);
			this.fullWaitingForEmpty.Enter(delegate(Desalinator.StatesInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(Desalinator.StatesInstance smi)
			{
				smi.CancelEmptyChore();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.DesalinatorNeedsEmptying, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Desalinator.StatesInstance smi) => smi.IsSaltRemoved());
			this.empty.PlayAnim("off").Enter("ResetStorage", delegate(Desalinator.StatesInstance smi)
			{
				smi.master.SaltStorageLeft = smi.master.maxSalt;
			}).GoTo(this.on.waiting);
		}

		// Token: 0x04006A8B RID: 27275
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State off;

		// Token: 0x04006A8C RID: 27276
		public Desalinator.States.OnStates on;

		// Token: 0x04006A8D RID: 27277
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State full;

		// Token: 0x04006A8E RID: 27278
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State fullWaitingForEmpty;

		// Token: 0x04006A8F RID: 27279
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyEmpty;

		// Token: 0x04006A90 RID: 27280
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State earlyWaitingForEmpty;

		// Token: 0x04006A91 RID: 27281
		public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State empty;

		// Token: 0x04006A92 RID: 27282
		private static readonly HashedString[] FULL_ANIMS = new HashedString[]
		{
			"working_pst",
			"off"
		};

		// Token: 0x04006A93 RID: 27283
		public StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter saltStorageLeft = new StateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.FloatParameter(0f);

		// Token: 0x020024CA RID: 9418
		public class OnStates : GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State
		{
			// Token: 0x0400A33F RID: 41791
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State waiting;

			// Token: 0x0400A340 RID: 41792
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pre;

			// Token: 0x0400A341 RID: 41793
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working;

			// Token: 0x0400A342 RID: 41794
			public GameStateMachine<Desalinator.States, Desalinator.StatesInstance, Desalinator, object>.State working_pst;
		}
	}
}
