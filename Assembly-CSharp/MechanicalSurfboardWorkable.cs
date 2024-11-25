using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000958 RID: 2392
[AddComponentMenu("KMonoBehaviour/Workable/MechanicalSurfboardWorkable")]
public class MechanicalSurfboardWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060045E2 RID: 17890 RVA: 0x0018D8DD File Offset: 0x0018BADD
	private MechanicalSurfboardWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060045E3 RID: 17891 RVA: 0x0018D8ED File Offset: 0x0018BAED
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(30f);
		this.surfboard = base.GetComponent<MechanicalSurfboard>();
	}

	// Token: 0x060045E4 RID: 17892 RVA: 0x0018D921 File Offset: 0x0018BB21
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("MechanicalSurfing", false);
	}

	// Token: 0x060045E5 RID: 17893 RVA: 0x0018D944 File Offset: 0x0018BB44
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		AttributeInstance attributeInstance = worker.GetAttributes().Get(Db.Get().Attributes.Athletics);
		if (attributeInstance.GetTotalValue() <= 7f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[0])
			};
		}
		else if (attributeInstance.GetTotalValue() <= 15f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[1])
			};
		}
		else
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[2])
			};
		}
		return result;
	}

	// Token: 0x060045E6 RID: 17894 RVA: 0x0018DA08 File Offset: 0x0018BC08
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		Building component = base.GetComponent<Building>();
		MechanicalSurfboard component2 = base.GetComponent<MechanicalSurfboard>();
		int widthInCells = component.Def.WidthInCells;
		int minInclusive = -(widthInCells - 1) / 2;
		int maxExclusive = widthInCells / 2;
		int x = UnityEngine.Random.Range(minInclusive, maxExclusive);
		float amount = component2.waterSpillRateKG * dt;
		float base_mass;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		base.GetComponent<Storage>().ConsumeAndGetDisease(SimHashes.Water.CreateTag(), amount, out base_mass, out diseaseInfo, out temperature);
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), new CellOffset(x, 0));
		ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Water);
		FallingWater.instance.AddParticle(cell, elementIndex, base_mass, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
		return false;
	}

	// Token: 0x060045E7 RID: 17895 RVA: 0x0018DAB0 File Offset: 0x0018BCB0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect))
		{
			component.Add(this.surfboard.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect))
		{
			component.Add(this.surfboard.trackingEffect, true);
		}
	}

	// Token: 0x060045E8 RID: 17896 RVA: 0x0018DB0E File Offset: 0x0018BD0E
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("MechanicalSurfing");
	}

	// Token: 0x060045E9 RID: 17897 RVA: 0x0018DB30 File Offset: 0x0018BD30
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect) && component.HasEffect(this.surfboard.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect) && component.HasEffect(this.surfboard.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002D77 RID: 11639
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002D78 RID: 11640
	public int basePriority;

	// Token: 0x04002D79 RID: 11641
	private MechanicalSurfboard surfboard;
}
