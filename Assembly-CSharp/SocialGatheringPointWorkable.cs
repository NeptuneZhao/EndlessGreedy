using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000AA6 RID: 2726
[AddComponentMenu("KMonoBehaviour/Workable/SocialGatheringPointWorkable")]
public class SocialGatheringPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06005038 RID: 20536 RVA: 0x001CD329 File Offset: 0x001CB529
	private SocialGatheringPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06005039 RID: 20537 RVA: 0x001CD33C File Offset: 0x001CB53C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"idle"
		};
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x0600503A RID: 20538 RVA: 0x001CD3C2 File Offset: 0x001CB5C2
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x001CD3EC File Offset: 0x001CB5EC
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!worker.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation))
		{
			Effects component = worker.GetComponent<Effects>();
			if (string.IsNullOrEmpty(this.specificEffect) || component.HasEffect(this.specificEffect))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600503C RID: 20540 RVA: 0x001CD43C File Offset: 0x001CB63C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
		this.timesConversed = 0;
	}

	// Token: 0x0600503D RID: 20541 RVA: 0x001CD498 File Offset: 0x001CB698
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x0600503E RID: 20542 RVA: 0x001CD4EC File Offset: 0x001CB6EC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.timesConversed > 0)
		{
			Effects component = worker.GetComponent<Effects>();
			if (!string.IsNullOrEmpty(this.specificEffect))
			{
				component.Add(this.specificEffect, true);
			}
		}
	}

	// Token: 0x0600503F RID: 20543 RVA: 0x001CD524 File Offset: 0x001CB724
	private void OnStartedTalking(object data)
	{
		ConversationManager.StartedTalkingEvent startedTalkingEvent = data as ConversationManager.StartedTalkingEvent;
		if (startedTalkingEvent == null)
		{
			return;
		}
		GameObject talker = startedTalkingEvent.talker;
		if (talker == base.worker.gameObject)
		{
			KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
			string text = startedTalkingEvent.anim;
			text += UnityEngine.Random.Range(1, 9).ToString();
			component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			this.lastTalker = talker;
		}
		this.timesConversed++;
	}

	// Token: 0x06005040 RID: 20544 RVA: 0x001CD5E2 File Offset: 0x001CB7E2
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x06005041 RID: 20545 RVA: 0x001CD5E4 File Offset: 0x001CB7E4
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400354F RID: 13647
	private GameObject lastTalker;

	// Token: 0x04003550 RID: 13648
	public int basePriority;

	// Token: 0x04003551 RID: 13649
	public string specificEffect;

	// Token: 0x04003552 RID: 13650
	public int timesConversed;
}
