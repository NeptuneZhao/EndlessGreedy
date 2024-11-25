using System;

// Token: 0x020009E2 RID: 2530
public class POITechItemUnlockWorkable : Workable
{
	// Token: 0x06004969 RID: 18793 RVA: 0x001A4810 File Offset: 0x001A2A10
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.ResearchingFromPOI;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_research_unlock_kanim")
		};
		this.synchronizeAnims = true;
	}

	// Token: 0x0600496A RID: 18794 RVA: 0x001A486C File Offset: 0x001A2A6C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		POITechItemUnlocks.Instance smi = this.GetSMI<POITechItemUnlocks.Instance>();
		smi.UnlockTechItems();
		smi.sm.pendingChore.Set(false, smi, false);
		base.gameObject.Trigger(1980521255, null);
		Prioritizable.RemoveRef(base.gameObject);
	}
}
