using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200064C RID: 1612
[AddComponentMenu("KMonoBehaviour/Workable/ArcadeMachineWorkable")]
public class ArcadeMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600276C RID: 10092 RVA: 0x000E08FA File Offset: 0x000DEAFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x0600276D RID: 10093 RVA: 0x000E092A File Offset: 0x000DEB2A
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("ArcadePlaying", false);
	}

	// Token: 0x0600276E RID: 10094 RVA: 0x000E0945 File Offset: 0x000DEB45
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("ArcadePlaying");
	}

	// Token: 0x0600276F RID: 10095 RVA: 0x000E0960 File Offset: 0x000DEB60
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect))
		{
			component.Add(ArcadeMachineWorkable.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect))
		{
			component.Add(ArcadeMachineWorkable.specificEffect, true);
		}
	}

	// Token: 0x06002770 RID: 10096 RVA: 0x000E09A8 File Offset: 0x000DEBA8
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect) && component.HasEffect(ArcadeMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect) && component.HasEffect(ArcadeMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040016BC RID: 5820
	public ArcadeMachine owner;

	// Token: 0x040016BD RID: 5821
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x040016BE RID: 5822
	private static string specificEffect = "PlayedArcade";

	// Token: 0x040016BF RID: 5823
	private static string trackingEffect = "RecentlyPlayedArcade";
}
