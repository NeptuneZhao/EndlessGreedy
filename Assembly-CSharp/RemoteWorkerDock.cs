using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x02000A2B RID: 2603
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkDock")]
public class RemoteWorkerDock : KMonoBehaviour
{
	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x06004B68 RID: 19304 RVA: 0x001ADDAE File Offset: 0x001ABFAE
	// (set) Token: 0x06004B69 RID: 19305 RVA: 0x001ADDB6 File Offset: 0x001ABFB6
	public RemoteWorkerSM RemoteWorker
	{
		get
		{
			return this.remoteWorker;
		}
		private set
		{
			this.remoteWorker = value;
			this.worker = ((value != null) ? new Ref<KSelectable>(value.GetComponent<KSelectable>()) : null);
		}
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x001ADDDC File Offset: 0x001ABFDC
	public WorkerBase GetActiveTerminalWorker()
	{
		if (this.terminal == null)
		{
			return null;
		}
		return this.terminal.worker;
	}

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x06004B6B RID: 19307 RVA: 0x001ADDF9 File Offset: 0x001ABFF9
	public bool IsOperational
	{
		get
		{
			return this.operational.IsOperational;
		}
	}

	// Token: 0x06004B6C RID: 19308 RVA: 0x001ADE08 File Offset: 0x001AC008
	private bool canWork(IRemoteDockWorkTarget provider)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(provider.Approachable.GetCell(), out num3, out num4);
		return num2 == num4 && Math.Abs(num - num3) <= 12;
	}

	// Token: 0x06004B6D RID: 19309 RVA: 0x001ADE4D File Offset: 0x001AC04D
	private void considerProvider(IRemoteDockWorkTarget provider)
	{
		if (this.canWork(provider))
		{
			this.providers.Add(provider);
		}
	}

	// Token: 0x06004B6E RID: 19310 RVA: 0x001ADE64 File Offset: 0x001AC064
	private void forgetProvider(IRemoteDockWorkTarget provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x06004B6F RID: 19311 RVA: 0x001ADE74 File Offset: 0x001AC074
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Components.RemoteWorkerDocks.Add(this.GetMyWorldId(), this);
		this.add_provider_binding = new Action<IRemoteDockWorkTarget>(this.considerProvider);
		this.remove_provider_binding = new Action<IRemoteDockWorkTarget>(this.forgetProvider);
		Components.RemoteDockWorkTargets.Register(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		Ref<KSelectable> @ref = this.worker;
		RemoteWorkerSM remoteWorkerSM;
		if (@ref == null)
		{
			remoteWorkerSM = null;
		}
		else
		{
			KSelectable kselectable = @ref.Get();
			remoteWorkerSM = ((kselectable != null) ? kselectable.GetComponent<RemoteWorkerSM>() : null);
		}
		this.remoteWorker = remoteWorkerSM;
		if (this.remoteWorker == null)
		{
			this.RequestNewWorker(null);
			return;
		}
		this.remoteWorkerDestroyedEventId = this.remoteWorker.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
	}

	// Token: 0x06004B70 RID: 19312 RVA: 0x001ADF4C File Offset: 0x001AC14C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteWorkerDocks.Remove(this.GetMyWorldId(), this);
		Components.RemoteDockWorkTargets.Unregister(this.GetMyWorldId(), this.add_provider_binding, this.remove_provider_binding);
		if (this.remoteWorker != null)
		{
			this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
		}
		if (this.newRemoteWorkerHandle.IsValid)
		{
			this.newRemoteWorkerHandle.ClearScheduler();
		}
	}

	// Token: 0x06004B71 RID: 19313 RVA: 0x001ADFC4 File Offset: 0x001AC1C4
	public void CollectChores(ChoreConsumerState duplicant_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		if (this.remoteWorker == null)
		{
			return;
		}
		ChoreConsumerState consumerState = this.remoteWorker.GetComponent<ChoreConsumer>().consumerState;
		consumerState.resume = duplicant_state.resume;
		foreach (IRemoteDockWorkTarget remoteDockWorkTarget in this.providers)
		{
			Chore remoteDockChore = remoteDockWorkTarget.RemoteDockChore;
			if (remoteDockChore != null)
			{
				remoteDockChore.CollectChores(consumerState, succeeded_contexts, incomplete_contexts, failed_contexts, false);
			}
		}
	}

	// Token: 0x06004B72 RID: 19314 RVA: 0x001AE054 File Offset: 0x001AC254
	public bool AvailableForWorkBy(RemoteWorkTerminal terminal)
	{
		return this.terminal == null || this.terminal == terminal;
	}

	// Token: 0x06004B73 RID: 19315 RVA: 0x001AE072 File Offset: 0x001AC272
	public bool HasWorker()
	{
		return this.remoteWorker != null;
	}

	// Token: 0x06004B74 RID: 19316 RVA: 0x001AE080 File Offset: 0x001AC280
	public void SetNextChore(RemoteWorkTerminal terminal, Chore.Precondition.Context chore_context)
	{
		global::Debug.Assert(this.worker != null);
		global::Debug.Assert(this.terminal == null || this.terminal == terminal);
		this.terminal = terminal;
		if (this.remoteWorker != null)
		{
			this.remoteWorker.SetNextChore(chore_context);
		}
	}

	// Token: 0x06004B75 RID: 19317 RVA: 0x001AE0E0 File Offset: 0x001AC2E0
	public bool StartWorking(RemoteWorkTerminal terminal)
	{
		if (this.terminal == null)
		{
			this.terminal = terminal;
		}
		if (this.terminal == terminal && this.remoteWorker != null)
		{
			this.remoteWorker.ActivelyControlled = true;
			return true;
		}
		return false;
	}

	// Token: 0x06004B76 RID: 19318 RVA: 0x001AE12D File Offset: 0x001AC32D
	public void StopWorking(RemoteWorkTerminal terminal)
	{
		if (terminal == this.terminal)
		{
			this.terminal = null;
			if (this.remoteWorker != null)
			{
				this.remoteWorker.ActivelyControlled = false;
			}
		}
	}

	// Token: 0x06004B77 RID: 19319 RVA: 0x001AE15E File Offset: 0x001AC35E
	public bool OnRemoteWorkTick(float dt)
	{
		return this.remoteWorker == null || (!this.remoteWorker.ActivelyWorking && !this.remoteWorker.HasChoreQueued());
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x001AE18D File Offset: 0x001AC38D
	private void OnStorageChanged(object _)
	{
		if (this.remoteWorker == null || this.worker.Get() == null)
		{
			this.RequestNewWorker(null);
		}
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x001AE1B8 File Offset: 0x001AC3B8
	private void RequestNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		Tag build_MATERIAL_TAG = RemoteWorkerConfig.BUILD_MATERIAL_TAG;
		if (this.storage.FindFirstWithMass(build_MATERIAL_TAG, 200f) == null)
		{
			if (!this.activeFetch)
			{
				this.activeFetch = true;
				FetchList2 fetchList = new FetchList2(this.storage, Db.Get().ChoreTypes.Fetch);
				fetchList.Add(build_MATERIAL_TAG, null, 200f, Operational.State.None);
				fetchList.Submit(delegate
				{
					this.activeFetch = false;
					this.RequestNewWorker(null);
				}, true);
				return;
			}
		}
		else
		{
			this.MakeNewWorker(null);
		}
	}

	// Token: 0x06004B7A RID: 19322 RVA: 0x001AE244 File Offset: 0x001AC444
	private void MakeNewWorker(object _ = null)
	{
		if (this.newRemoteWorkerHandle.IsValid)
		{
			return;
		}
		if (this.storage.GetAmountAvailable(RemoteWorkerConfig.BUILD_MATERIAL_TAG) < 200f)
		{
			return;
		}
		PrimaryElement elem = this.storage.FindFirstWithMass(RemoteWorkerConfig.BUILD_MATERIAL_TAG, 200f);
		if (elem == null)
		{
			return;
		}
		float temperature;
		SimUtil.DiseaseInfo disease;
		float num;
		this.storage.ConsumeAndGetDisease(elem.ElementID.CreateTag(), 200f, out num, out disease, out temperature);
		this.status_item_handle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RemoteWorkDockMakingWorker, null);
		this.newRemoteWorkerHandle = GameScheduler.Instance.Schedule("MakeRemoteWorker", 2f, delegate(object _)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(RemoteWorkerConfig.ID), this.transform.position, Grid.SceneLayer.Creatures, null, 0);
			if (this.remoteWorkerDestroyedEventId != -1 && this.remoteWorker != null)
			{
				this.remoteWorker.Unsubscribe(this.remoteWorkerDestroyedEventId);
			}
			this.RemoteWorker = gameObject.GetComponent<RemoteWorkerSM>();
			this.remoteWorker.HomeDepot = this;
			this.remoteWorker.playNewWorker = true;
			gameObject.SetActive(true);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = elem.ElementID;
			component.Temperature = temperature;
			if (disease.idx != 255)
			{
				component.AddDisease(disease.idx, disease.count, "Inherited from construction material");
			}
			this.remoteWorkerDestroyedEventId = gameObject.Subscribe(1969584890, new Action<object>(this.RequestNewWorker));
			this.newRemoteWorkerHandle.ClearScheduler();
			this.GetComponent<KSelectable>().RemoveStatusItem(this.status_item_handle, false);
		}, null, null);
	}

	// Token: 0x04003163 RID: 12643
	[Serialize]
	protected Ref<KSelectable> worker;

	// Token: 0x04003164 RID: 12644
	protected RemoteWorkerSM remoteWorker;

	// Token: 0x04003165 RID: 12645
	private int remoteWorkerDestroyedEventId = -1;

	// Token: 0x04003166 RID: 12646
	protected RemoteWorkTerminal terminal;

	// Token: 0x04003167 RID: 12647
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003168 RID: 12648
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003169 RID: 12649
	[MyCmpAdd]
	private EnterableDock enter_;

	// Token: 0x0400316A RID: 12650
	[MyCmpAdd]
	private ExitableDock exit_;

	// Token: 0x0400316B RID: 12651
	[MyCmpAdd]
	private WorkerRecharger recharger_;

	// Token: 0x0400316C RID: 12652
	[MyCmpAdd]
	private WorkerGunkRemover gunk_remover_;

	// Token: 0x0400316D RID: 12653
	[MyCmpAdd]
	private WorkerOilRefiller oil_refiller_;

	// Token: 0x0400316E RID: 12654
	private Guid status_item_handle;

	// Token: 0x0400316F RID: 12655
	private SchedulerHandle newRemoteWorkerHandle;

	// Token: 0x04003170 RID: 12656
	private List<IRemoteDockWorkTarget> providers = new List<IRemoteDockWorkTarget>();

	// Token: 0x04003171 RID: 12657
	private Action<IRemoteDockWorkTarget> add_provider_binding;

	// Token: 0x04003172 RID: 12658
	private Action<IRemoteDockWorkTarget> remove_provider_binding;

	// Token: 0x04003173 RID: 12659
	private bool activeFetch;
}
