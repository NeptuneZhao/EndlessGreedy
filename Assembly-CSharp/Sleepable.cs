using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000AA0 RID: 2720
[AddComponentMenu("KMonoBehaviour/Workable/Sleepable")]
public class Sleepable : Workable
{
	// Token: 0x06005003 RID: 20483 RVA: 0x001CC754 File Offset: 0x001CA954
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		this.workerStatusItem = null;
		this.synchronizeAnims = false;
		this.triggerWorkReactions = false;
		this.lightEfficiencyBonus = false;
		this.approachable = base.GetComponent<IApproachable>();
	}

	// Token: 0x06005004 RID: 20484 RVA: 0x001CC793 File Offset: 0x001CA993
	protected override void OnSpawn()
	{
		if (this.isNormalBed)
		{
			Components.NormalBeds.Add(base.gameObject.GetMyWorldId(), this);
		}
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06005005 RID: 20485 RVA: 0x001CC7C0 File Offset: 0x001CA9C0
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkAnims;
		}
		return Sleepable.normalWorkAnims;
	}

	// Token: 0x06005006 RID: 20486 RVA: 0x001CC800 File Offset: 0x001CAA00
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkPstAnim;
		}
		return Sleepable.normalWorkPstAnim;
	}

	// Token: 0x06005007 RID: 20487 RVA: 0x001CC840 File Offset: 0x001CAA40
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		KAnimControllerBase animController = this.GetAnimController();
		if (animController != null)
		{
			animController.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			animController.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		}
		base.Subscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (this.operational != null)
		{
			this.operational.SetActive(true, false);
		}
		worker.Trigger(-1283701846, this);
		worker.GetComponent<Effects>().Add(this.effectName, false);
		this.isDoneSleeping = false;
	}

	// Token: 0x06005008 RID: 20488 RVA: 0x001CC8FC File Offset: 0x001CAAFC
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.isDoneSleeping)
		{
			return Time.time > this.wakeTime;
		}
		if (this.Dreamable != null && !this.Dreamable.DreamIsDisturbed)
		{
			this.Dreamable.WorkTick(worker, dt);
		}
		if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
		{
			this.isDoneSleeping = true;
			this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
		}
		return false;
	}

	// Token: 0x06005009 RID: 20489 RVA: 0x001CC974 File Offset: 0x001CAB74
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.operational != null)
		{
			this.operational.SetActive(false, false);
		}
		base.Unsubscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (worker != null)
		{
			Effects component = worker.GetComponent<Effects>();
			component.Remove(this.effectName);
			if (this.wakeEffects != null)
			{
				foreach (string effect_id in this.wakeEffects)
				{
					component.Add(effect_id, true);
				}
			}
			if (this.stretchOnWake && UnityEngine.Random.value < 0.33f)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.MorningStretch, 1, null);
			}
			if (worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value < worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
			{
				worker.Trigger(1338475637, this);
			}
		}
	}

	// Token: 0x0600500A RID: 20490 RVA: 0x001CCAC0 File Offset: 0x001CACC0
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x0600500B RID: 20491 RVA: 0x001CCAC3 File Offset: 0x001CACC3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.isNormalBed)
		{
			Components.NormalBeds.Remove(base.gameObject.GetMyWorldId(), this);
		}
	}

	// Token: 0x0600500C RID: 20492 RVA: 0x001CCAEC File Offset: 0x001CACEC
	private void PlayPstAnim(object data)
	{
		WorkerBase workerBase = (WorkerBase)data;
		if (workerBase != null && workerBase.GetWorkable() != null)
		{
			KAnimControllerBase component = workerBase.GetWorkable().gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x04003524 RID: 13604
	private const float STRECH_CHANCE = 0.33f;

	// Token: 0x04003525 RID: 13605
	[MyCmpGet]
	public Assignable assignable;

	// Token: 0x04003526 RID: 13606
	public IApproachable approachable;

	// Token: 0x04003527 RID: 13607
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003528 RID: 13608
	public string effectName = "Sleep";

	// Token: 0x04003529 RID: 13609
	public List<string> wakeEffects;

	// Token: 0x0400352A RID: 13610
	public bool stretchOnWake = true;

	// Token: 0x0400352B RID: 13611
	private float wakeTime;

	// Token: 0x0400352C RID: 13612
	private bool isDoneSleeping;

	// Token: 0x0400352D RID: 13613
	public bool isNormalBed = true;

	// Token: 0x0400352E RID: 13614
	public ClinicDreamable Dreamable;

	// Token: 0x0400352F RID: 13615
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04003530 RID: 13616
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x04003531 RID: 13617
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04003532 RID: 13618
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};
}
