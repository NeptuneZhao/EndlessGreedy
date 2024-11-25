using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000B30 RID: 2864
[AddComponentMenu("KMonoBehaviour/Workable/TelephoneWorkable")]
public class TelephoneCallerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600556F RID: 21871 RVA: 0x001E7F70 File Offset: 0x001E6170
	private TelephoneCallerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.workingPstComplete = new HashedString[]
		{
			"on_pst"
		};
		this.workAnims = new HashedString[]
		{
			"on_pre",
			"on",
			"on_receiving",
			"on_pre_loop_receiving",
			"on_loop",
			"on_loop_pre"
		};
	}

	// Token: 0x06005570 RID: 21872 RVA: 0x001E801C File Offset: 0x001E621C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telephone_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(40f);
		this.telephone = base.GetComponent<Telephone>();
	}

	// Token: 0x06005571 RID: 21873 RVA: 0x001E8079 File Offset: 0x001E6279
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
		this.telephone.isInUse = true;
	}

	// Token: 0x06005572 RID: 21874 RVA: 0x001E8094 File Offset: 0x001E6294
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.telephone.HasTag(GameTags.LongDistanceCall))
		{
			if (!string.IsNullOrEmpty(this.telephone.longDistanceEffect))
			{
				component.Add(this.telephone.longDistanceEffect, true);
			}
		}
		else if (this.telephone.wasAnswered)
		{
			if (!string.IsNullOrEmpty(this.telephone.chatEffect))
			{
				component.Add(this.telephone.chatEffect, true);
			}
		}
		else if (!string.IsNullOrEmpty(this.telephone.babbleEffect))
		{
			component.Add(this.telephone.babbleEffect, true);
		}
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect))
		{
			component.Add(this.telephone.trackingEffect, true);
		}
	}

	// Token: 0x06005573 RID: 21875 RVA: 0x001E815F File Offset: 0x001E635F
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		this.telephone.HangUp();
	}

	// Token: 0x06005574 RID: 21876 RVA: 0x001E817C File Offset: 0x001E637C
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect) && component.HasEffect(this.telephone.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.telephone.chatEffect) && component.HasEffect(this.telephone.chatEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		if (!string.IsNullOrEmpty(this.telephone.babbleEffect) && component.HasEffect(this.telephone.babbleEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040037E8 RID: 14312
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040037E9 RID: 14313
	public int basePriority;

	// Token: 0x040037EA RID: 14314
	private Telephone telephone;
}
