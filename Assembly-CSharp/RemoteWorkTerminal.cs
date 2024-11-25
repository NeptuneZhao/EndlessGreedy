using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A1A RID: 2586
[AddComponentMenu("KMonoBehaviour/Workable/RemoteWorkTerminal")]
public class RemoteWorkTerminal : Workable
{
	// Token: 0x06004B0B RID: 19211 RVA: 0x001AD11C File Offset: 0x001AB31C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_remote_terminal_kanim")
		};
		this.InitializeWorkingInteracts();
		this.synchronizeAnims = true;
		this.kbac.onAnimComplete += this.PlayNextWorkingAnim;
	}

	// Token: 0x06004B0C RID: 19212 RVA: 0x001AD174 File Offset: 0x001AB374
	private void InitializeWorkingInteracts()
	{
		if (RemoteWorkTerminal.NUM_WORKING_INTERACTS != -1)
		{
			return;
		}
		KAnimFileData data = this.overrideAnims[0].GetData();
		RemoteWorkTerminal.NUM_WORKING_INTERACTS = 1;
		for (;;)
		{
			string anim_name = string.Format("working_loop_{0}", RemoteWorkTerminal.NUM_WORKING_INTERACTS);
			if (data.GetAnim(anim_name) == null)
			{
				break;
			}
			RemoteWorkTerminal.NUM_WORKING_INTERACTS++;
		}
	}

	// Token: 0x06004B0D RID: 19213 RVA: 0x001AD1CC File Offset: 0x001AB3CC
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkAnims;
		}
		return RemoteWorkTerminal.normalWorkAnims;
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x001AD20C File Offset: 0x001AB40C
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return RemoteWorkTerminal.hatWorkPstAnim;
		}
		return RemoteWorkTerminal.normalWorkPstAnim;
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06004B0F RID: 19215 RVA: 0x001AD24A File Offset: 0x001AB44A
	// (set) Token: 0x06004B10 RID: 19216 RVA: 0x001AD25D File Offset: 0x001AB45D
	public RemoteWorkerDock CurrentDock
	{
		get
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (@ref == null)
			{
				return null;
			}
			return @ref.Get();
		}
		set
		{
			Ref<RemoteWorkerDock> @ref = this.dock;
			if (((@ref != null) ? @ref.Get() : null) != null)
			{
				this.dock.Get().StopWorking(this);
			}
			this.dock = new Ref<RemoteWorkerDock>(value);
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06004B11 RID: 19217 RVA: 0x001AD296 File Offset: 0x001AB496
	// (set) Token: 0x06004B12 RID: 19218 RVA: 0x001AD2A8 File Offset: 0x001AB4A8
	public RemoteWorkerDock FutureDock
	{
		get
		{
			return this.future_dock ?? this.CurrentDock;
		}
		set
		{
			this.CurrentDock = value;
		}
	}

	// Token: 0x06004B13 RID: 19219 RVA: 0x001AD2B1 File Offset: 0x001AB4B1
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.kbac.Queue(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StartWorking(this);
	}

	// Token: 0x06004B14 RID: 19220 RVA: 0x001AD2E8 File Offset: 0x001AB4E8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		RemoteWorkerDock currentDock = this.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.StopWorking(this);
	}

	// Token: 0x06004B15 RID: 19221 RVA: 0x001AD302 File Offset: 0x001AB502
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.CurrentDock == null || this.CurrentDock.OnRemoteWorkTick(dt);
	}

	// Token: 0x06004B16 RID: 19222 RVA: 0x001AD320 File Offset: 0x001AB520
	private HashedString GetWorkingLoop()
	{
		return string.Format("working_loop_{0}", UnityEngine.Random.Range(1, RemoteWorkTerminal.NUM_WORKING_INTERACTS + 1));
	}

	// Token: 0x06004B17 RID: 19223 RVA: 0x001AD343 File Offset: 0x001AB543
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (base.worker.GetState() == WorkerBase.State.Working)
		{
			this.kbac.Play(this.GetWorkingLoop(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0400312E RID: 12590
	[Serialize]
	private Ref<RemoteWorkerDock> dock;

	// Token: 0x0400312F RID: 12591
	private static int NUM_WORKING_INTERACTS = -1;

	// Token: 0x04003130 RID: 12592
	[MyCmpReq]
	private KBatchedAnimController kbac;

	// Token: 0x04003131 RID: 12593
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre"
	};

	// Token: 0x04003132 RID: 12594
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre"
	};

	// Token: 0x04003133 RID: 12595
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04003134 RID: 12596
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"working_hat_pst"
	};

	// Token: 0x04003135 RID: 12597
	public RemoteWorkerDock future_dock;
}
