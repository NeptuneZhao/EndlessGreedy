using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000799 RID: 1945
public class WatchRoboDancerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600353E RID: 13630 RVA: 0x00122110 File Offset: 0x00120310
	private WatchRoboDancerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x00122178 File Offset: 0x00120378
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(30f);
		this.showProgressBar = false;
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x001221A8 File Offset: 0x001203A8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.TRACKING_EFFECT, true);
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			component.Add(WatchRoboDancerWorkable.SPECIFIC_EFFECT, true);
		}
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x001221F0 File Offset: 0x001203F0
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.TRACKING_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.TRACKING_EFFECT))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(WatchRoboDancerWorkable.SPECIFIC_EFFECT) && component.HasEffect(WatchRoboDancerWorkable.SPECIFIC_EFFECT))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x0012224B File Offset: 0x0012044B
	protected override void OnStartWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x0012225F File Offset: 0x0012045F
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker.GetComponent<Facing>().Face(this.owner.transform.position.x);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x00122289 File Offset: 0x00120489
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("Dancing");
		ChoreHelpers.DestroyLocator(base.gameObject);
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x001222A8 File Offset: 0x001204A8
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x04001FA3 RID: 8099
	public GameObject owner;

	// Token: 0x04001FA4 RID: 8100
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04001FA5 RID: 8101
	public static string SPECIFIC_EFFECT = "SawRoboDancer";

	// Token: 0x04001FA6 RID: 8102
	public static string TRACKING_EFFECT = "RecentlySawRoboDancer";

	// Token: 0x04001FA7 RID: 8103
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_robotdance1_kanim")
		}
	};
}
