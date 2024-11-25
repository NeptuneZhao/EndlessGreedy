using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200065F RID: 1631
[AddComponentMenu("KMonoBehaviour/Workable/BeachChairWorkable")]
public class BeachChairWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06002835 RID: 10293 RVA: 0x000E432C File Offset: 0x000E252C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_beach_chair_kanim")
		};
		this.workAnims = null;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.lightEfficiencyBonus = false;
		base.SetWorkTime(150f);
		this.beachChair = base.GetComponent<BeachChair>();
	}

	// Token: 0x06002836 RID: 10294 RVA: 0x000E43AD File Offset: 0x000E25AD
	protected override void OnStartWork(WorkerBase worker)
	{
		this.timeLit = 0f;
		this.beachChair.SetWorker(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("BeachChairRelaxing", false);
	}

	// Token: 0x06002837 RID: 10295 RVA: 0x000E43E8 File Offset: 0x000E25E8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		int i = Grid.PosToCell(base.gameObject);
		bool flag = (float)Grid.LightIntensity[i] >= (float)BeachChairConfig.TAN_LUX - 1f;
		this.beachChair.SetLit(flag);
		if (flag)
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 1f);
			this.timeLit += dt;
		}
		else
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 0f);
		}
		return false;
	}

	// Token: 0x06002838 RID: 10296 RVA: 0x000E4478 File Offset: 0x000E2678
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.timeLit / this.workTime >= 0.75f)
		{
			component.Add(this.beachChair.specificEffectLit, true);
			component.Remove(this.beachChair.specificEffectUnlit);
		}
		else
		{
			component.Add(this.beachChair.specificEffectUnlit, true);
			component.Remove(this.beachChair.specificEffectLit);
		}
		component.Add(this.beachChair.trackingEffect, true);
	}

	// Token: 0x06002839 RID: 10297 RVA: 0x000E44FD File Offset: 0x000E26FD
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("BeachChairRelaxing");
	}

	// Token: 0x0600283A RID: 10298 RVA: 0x000E451C File Offset: 0x000E271C
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.beachChair.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.beachChair.specificEffectLit) || component.HasEffect(this.beachChair.specificEffectUnlit))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400172C RID: 5932
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400172D RID: 5933
	private float timeLit;

	// Token: 0x0400172E RID: 5934
	public string soundPath = GlobalAssets.GetSound("BeachChair_music_lp", false);

	// Token: 0x0400172F RID: 5935
	public HashedString BEACH_CHAIR_LIT_PARAMETER = "beachChair_lit";

	// Token: 0x04001730 RID: 5936
	public int basePriority;

	// Token: 0x04001731 RID: 5937
	private BeachChair beachChair;
}
