using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020009E3 RID: 2531
public class PartyPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600496C RID: 18796 RVA: 0x001A48C5 File Offset: 0x001A2AC5
	private PartyPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600496D RID: 18797 RVA: 0x001A48D8 File Offset: 0x001A2AD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnimPlayMode = KAnim.PlayMode.Loop;
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
		if (UnityEngine.Random.Range(0f, 100f) > 80f)
		{
			this.activity = PartyPointWorkable.ActivityType.Dance;
		}
		else
		{
			this.activity = PartyPointWorkable.ActivityType.Talk;
		}
		PartyPointWorkable.ActivityType activityType = this.activity;
		if (activityType == PartyPointWorkable.ActivityType.Talk)
		{
			this.workAnims = new HashedString[]
			{
				"idle"
			};
			this.workerOverrideAnims = new KAnimFile[][]
			{
				new KAnimFile[]
				{
					Assets.GetAnim("anim_generic_convo_kanim")
				}
			};
			return;
		}
		if (activityType != PartyPointWorkable.ActivityType.Dance)
		{
			return;
		}
		this.workAnims = new HashedString[]
		{
			"working_loop"
		};
		this.workerOverrideAnims = new KAnimFile[][]
		{
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_danceone_kanim")
			},
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_dancetwo_kanim")
			},
			new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_phonobox_dancethree_kanim")
			}
		};
	}

	// Token: 0x0600496E RID: 18798 RVA: 0x001A4A3C File Offset: 0x001A2C3C
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x0600496F RID: 18799 RVA: 0x001A4A6D File Offset: 0x001A2C6D
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x06004970 RID: 18800 RVA: 0x001A4A94 File Offset: 0x001A2C94
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x06004971 RID: 18801 RVA: 0x001A4A98 File Offset: 0x001A2C98
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06004972 RID: 18802 RVA: 0x001A4AF0 File Offset: 0x001A2CF0
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06004973 RID: 18803 RVA: 0x001A4B44 File Offset: 0x001A2D44
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06004974 RID: 18804 RVA: 0x001A4B74 File Offset: 0x001A2D74
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
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
				string text = startedTalkingEvent.anim;
				text += UnityEngine.Random.Range(1, 9).ToString();
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else
		{
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			}
			this.lastTalker = talker;
		}
	}

	// Token: 0x06004975 RID: 18805 RVA: 0x001A4C36 File Offset: 0x001A2E36
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x06004976 RID: 18806 RVA: 0x001A4C38 File Offset: 0x001A2E38
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04003009 RID: 12297
	private GameObject lastTalker;

	// Token: 0x0400300A RID: 12298
	public int basePriority;

	// Token: 0x0400300B RID: 12299
	public string specificEffect;

	// Token: 0x0400300C RID: 12300
	public KAnimFile[][] workerOverrideAnims;

	// Token: 0x0400300D RID: 12301
	private PartyPointWorkable.ActivityType activity;

	// Token: 0x020019E7 RID: 6631
	private enum ActivityType
	{
		// Token: 0x04007AD1 RID: 31441
		Talk,
		// Token: 0x04007AD2 RID: 31442
		Dance,
		// Token: 0x04007AD3 RID: 31443
		LENGTH
	}
}
