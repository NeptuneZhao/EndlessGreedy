using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000899 RID: 2201
[AddComponentMenu("KMonoBehaviour/Workable/EspressoMachineWorkable")]
public class EspressoMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003DBA RID: 15802 RVA: 0x001551BC File Offset: 0x001533BC
	private EspressoMachineWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x001551E4 File Offset: 0x001533E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_espresso_machine_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x00155238 File Offset: 0x00153438
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x0015526A File Offset: 0x0015346A
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06003DBE RID: 15806 RVA: 0x0015527C File Offset: 0x0015347C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, EspressoMachine.WATER_MASS_PER_USE, out num, out diseaseInfo, out num2);
		SimUtil.DiseaseInfo diseaseInfo2;
		component.ConsumeAndGetDisease(EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE, out num, out diseaseInfo2, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
			smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, EspressoMachine.INGREDIENT_TAG, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect))
		{
			component2.Add(EspressoMachineWorkable.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect))
		{
			component2.Add(EspressoMachineWorkable.trackingEffect, true);
		}
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x00155334 File Offset: 0x00153534
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x00155344 File Offset: 0x00153544
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect) && component.HasEffect(EspressoMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect) && component.HasEffect(EspressoMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040025AB RID: 9643
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

	// Token: 0x040025AC RID: 9644
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040025AD RID: 9645
	public int basePriority = RELAXATION.PRIORITY.TIER5;

	// Token: 0x040025AE RID: 9646
	private static string specificEffect = "Espresso";

	// Token: 0x040025AF RID: 9647
	private static string trackingEffect = "RecentlyRecDrink";
}
