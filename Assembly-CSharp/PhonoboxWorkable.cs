using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020009E7 RID: 2535
[AddComponentMenu("KMonoBehaviour/Workable/PhonoboxWorkable")]
public class PhonoboxWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004985 RID: 18821 RVA: 0x001A50E4 File Offset: 0x001A32E4
	private PhonoboxWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004986 RID: 18822 RVA: 0x001A517D File Offset: 0x001A337D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06004987 RID: 18823 RVA: 0x001A51A8 File Offset: 0x001A33A8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect))
		{
			component.Add(this.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06004988 RID: 18824 RVA: 0x001A51F4 File Offset: 0x001A33F4
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x06004989 RID: 18825 RVA: 0x001A5253 File Offset: 0x001A3453
	protected override void OnStartWork(WorkerBase worker)
	{
		this.owner.AddWorker(worker);
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x0600498A RID: 18826 RVA: 0x001A5273 File Offset: 0x001A3473
	protected override void OnStopWork(WorkerBase worker)
	{
		this.owner.RemoveWorker(worker);
		worker.GetComponent<Effects>().Remove("Dancing");
	}

	// Token: 0x0600498B RID: 18827 RVA: 0x001A5294 File Offset: 0x001A3494
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x04003018 RID: 12312
	public Phonobox owner;

	// Token: 0x04003019 RID: 12313
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x0400301A RID: 12314
	public string specificEffect = "Danced";

	// Token: 0x0400301B RID: 12315
	public string trackingEffect = "RecentlyDanced";

	// Token: 0x0400301C RID: 12316
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
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
