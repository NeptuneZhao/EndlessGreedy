using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020008F0 RID: 2288
[AddComponentMenu("KMonoBehaviour/Workable/HotTubWorkable")]
public class HotTubWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x060041D4 RID: 16852 RVA: 0x001755C9 File Offset: 0x001737C9
	private HotTubWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060041D5 RID: 16853 RVA: 0x001755D9 File Offset: 0x001737D9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x00175608 File Offset: 0x00173808
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new HotTubWorkerStateMachine.StatesInstance(worker);
		return anim;
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x0017562B File Offset: 0x0017382B
	protected override void OnStartWork(WorkerBase worker)
	{
		this.faceLeft = (UnityEngine.Random.value > 0.5f);
		worker.GetComponent<Effects>().Add("HotTubRelaxing", false);
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x00175655 File Offset: 0x00173855
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove("HotTubRelaxing");
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x00175667 File Offset: 0x00173867
	public override Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition() + (this.faceLeft ? Vector3.left : Vector3.right);
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x00175690 File Offset: 0x00173890
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect))
		{
			component.Add(this.hotTub.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect))
		{
			component.Add(this.hotTub.specificEffect, true);
		}
		component.Add("WarmTouch", true).timeRemaining = 1800f;
	}

	// Token: 0x060041DB RID: 16859 RVA: 0x00175704 File Offset: 0x00173904
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.hotTub.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect) && component.HasEffect(this.hotTub.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect) && component.HasEffect(this.hotTub.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002BA6 RID: 11174
	public HotTub hotTub;

	// Token: 0x04002BA7 RID: 11175
	private bool faceLeft;
}
