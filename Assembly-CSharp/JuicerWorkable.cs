using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000935 RID: 2357
[AddComponentMenu("KMonoBehaviour/Workable/JuicerWorkable")]
public class JuicerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004474 RID: 17524 RVA: 0x00185986 File Offset: 0x00183B86
	private JuicerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004475 RID: 17525 RVA: 0x001859A4 File Offset: 0x00183BA4
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06004476 RID: 17526 RVA: 0x001859D8 File Offset: 0x00183BD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_juicer_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
		this.juicer = base.GetComponent<Juicer>();
	}

	// Token: 0x06004477 RID: 17527 RVA: 0x00185A35 File Offset: 0x00183C35
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004478 RID: 17528 RVA: 0x00185A44 File Offset: 0x00183C44
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, this.juicer.waterMassPerUse, out num, out diseaseInfo, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		for (int i = 0; i < this.juicer.ingredientTags.Length; i++)
		{
			SimUtil.DiseaseInfo diseaseInfo2;
			component.ConsumeAndGetDisease(this.juicer.ingredientTags[i], this.juicer.ingredientMassesPerUse[i], out num, out diseaseInfo2, out num2);
			if (smi != null)
			{
				smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, this.juicer.ingredientTags[i], Sickness.InfectionVector.Digestion);
			}
		}
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.juicer.specificEffect))
		{
			component2.Add(this.juicer.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.juicer.trackingEffect))
		{
			component2.Add(this.juicer.trackingEffect, true);
		}
	}

	// Token: 0x06004479 RID: 17529 RVA: 0x00185B61 File Offset: 0x00183D61
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x0600447A RID: 17530 RVA: 0x00185B70 File Offset: 0x00183D70
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.juicer.trackingEffect) && component.HasEffect(this.juicer.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.juicer.specificEffect) && component.HasEffect(this.juicer.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002CCA RID: 11466
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x04002CCB RID: 11467
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002CCC RID: 11468
	public int basePriority;

	// Token: 0x04002CCD RID: 11469
	private Juicer juicer;
}
