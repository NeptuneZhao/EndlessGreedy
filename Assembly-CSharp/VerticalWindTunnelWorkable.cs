using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000B53 RID: 2899
[AddComponentMenu("KMonoBehaviour/Workable/VerticalWindTunnelWorkable")]
public class VerticalWindTunnelWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060056AC RID: 22188 RVA: 0x001EFB23 File Offset: 0x001EDD23
	private VerticalWindTunnelWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060056AD RID: 22189 RVA: 0x001EFB34 File Offset: 0x001EDD34
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new WindTunnelWorkerStateMachine.StatesInstance(worker, this);
		return anim;
	}

	// Token: 0x060056AE RID: 22190 RVA: 0x001EFB58 File Offset: 0x001EDD58
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x060056AF RID: 22191 RVA: 0x001EFB80 File Offset: 0x001EDD80
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("VerticalWindTunnelFlying", false);
	}

	// Token: 0x060056B0 RID: 22192 RVA: 0x001EFB9B File Offset: 0x001EDD9B
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("VerticalWindTunnelFlying");
	}

	// Token: 0x060056B1 RID: 22193 RVA: 0x001EFBB4 File Offset: 0x001EDDB4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		component.Add(this.windTunnel.trackingEffect, true);
		component.Add(this.windTunnel.specificEffect, true);
	}

	// Token: 0x060056B2 RID: 22194 RVA: 0x001EFBE4 File Offset: 0x001EDDE4
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.windTunnel.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.windTunnel.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.windTunnel.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040038B8 RID: 14520
	public VerticalWindTunnel windTunnel;

	// Token: 0x040038B9 RID: 14521
	public HashedString overrideAnim;

	// Token: 0x040038BA RID: 14522
	public string[] preAnims;

	// Token: 0x040038BB RID: 14523
	public string loopAnim;

	// Token: 0x040038BC RID: 14524
	public string[] pstAnims;
}
