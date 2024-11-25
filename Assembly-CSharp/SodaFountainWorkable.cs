using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000AA8 RID: 2728
[AddComponentMenu("KMonoBehaviour/Workable/SodaFountainWorkable")]
public class SodaFountainWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06005047 RID: 20551 RVA: 0x001CD75B File Offset: 0x001CB95B
	private SodaFountainWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06005048 RID: 20552 RVA: 0x001CD778 File Offset: 0x001CB978
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sodamaker_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
		this.sodaFountain = base.GetComponent<SodaFountain>();
	}

	// Token: 0x06005049 RID: 20553 RVA: 0x001CD7D8 File Offset: 0x001CB9D8
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x0600504A RID: 20554 RVA: 0x001CD80A File Offset: 0x001CBA0A
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x0600504B RID: 20555 RVA: 0x001CD81C File Offset: 0x001CBA1C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, this.sodaFountain.waterMassPerUse, out num, out diseaseInfo, out num2);
		SimUtil.DiseaseInfo diseaseInfo2;
		component.ConsumeAndGetDisease(this.sodaFountain.ingredientTag, this.sodaFountain.ingredientMassPerUse, out num, out diseaseInfo2, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
			smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, this.sodaFountain.ingredientTag, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sodaFountain.specificEffect))
		{
			component2.Add(this.sodaFountain.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.sodaFountain.trackingEffect))
		{
			component2.Add(this.sodaFountain.trackingEffect, true);
		}
	}

	// Token: 0x0600504C RID: 20556 RVA: 0x001CD904 File Offset: 0x001CBB04
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x0600504D RID: 20557 RVA: 0x001CD914 File Offset: 0x001CBB14
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sodaFountain.trackingEffect) && component.HasEffect(this.sodaFountain.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.sodaFountain.specificEffect) && component.HasEffect(this.sodaFountain.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04003558 RID: 13656
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x04003559 RID: 13657
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400355A RID: 13658
	public int basePriority;

	// Token: 0x0400355B RID: 13659
	private SodaFountain sodaFountain;
}
