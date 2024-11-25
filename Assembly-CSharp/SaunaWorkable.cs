using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000A76 RID: 2678
[AddComponentMenu("KMonoBehaviour/Workable/SaunaWorkable")]
public class SaunaWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004DEF RID: 19951 RVA: 0x001BFA71 File Offset: 0x001BDC71
	private SaunaWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x001BFA84 File Offset: 0x001BDC84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sauna_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		this.workLayer = Grid.SceneLayer.BuildingUse;
		base.SetWorkTime(30f);
		this.sauna = base.GetComponent<Sauna>();
	}

	// Token: 0x06004DF1 RID: 19953 RVA: 0x001BFAE9 File Offset: 0x001BDCE9
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("SaunaRelaxing", false);
	}

	// Token: 0x06004DF2 RID: 19954 RVA: 0x001BFB14 File Offset: 0x001BDD14
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sauna.specificEffect))
		{
			component.Add(this.sauna.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.sauna.trackingEffect))
		{
			component.Add(this.sauna.trackingEffect, true);
		}
		component.Add("WarmTouch", true).timeRemaining = 1800f;
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004DF3 RID: 19955 RVA: 0x001BFB98 File Offset: 0x001BDD98
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("SaunaRelaxing");
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(SimHashes.Steam.CreateTag(), this.sauna.steamPerUseKG, out num, out diseaseInfo, out num2);
		component.AddLiquid(SimHashes.Water, this.sauna.steamPerUseKG, this.sauna.waterOutputTemp, diseaseInfo.idx, diseaseInfo.count, true, false);
	}

	// Token: 0x06004DF4 RID: 19956 RVA: 0x001BFC18 File Offset: 0x001BDE18
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sauna.trackingEffect) && component.HasEffect(this.sauna.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.sauna.specificEffect) && component.HasEffect(this.sauna.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040033E5 RID: 13285
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040033E6 RID: 13286
	public int basePriority;

	// Token: 0x040033E7 RID: 13287
	private Sauna sauna;
}
