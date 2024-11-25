using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000209 RID: 521
public class GunkEmptierWorkable : Workable
{
	// Token: 0x06000ABA RID: 2746 RVA: 0x00040113 File Offset: 0x0003E313
	private GunkEmptierWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00040124 File Offset: 0x0003E324
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gunkdump_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		this.storage = base.GetComponent<Storage>();
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00040190 File Offset: 0x0003E390
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float mass = Mathf.Min(new float[]
		{
			dt / this.workTime * GunkMonitor.GUNK_CAPACITY,
			this.gunkMonitor.CurrentGunkMass,
			this.storage.RemainingCapacity()
		});
		this.gunkMonitor.ExpellGunk(mass, this.storage);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x000401F0 File Offset: 0x0003E3F0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.gunkMonitor = worker.GetSMI<GunkMonitor.Instance>();
		if (Sim.IsRadiationEnabled() && worker.GetAmounts().Get(Db.Get().Amounts.RadiationBalance).value > 0f)
		{
			worker.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, null);
		}
		this.TriggerRoomEffects();
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x00040264 File Offset: 0x0003E464
	private void TriggerRoomEffects()
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), base.worker.GetComponent<Effects>());
		}
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x000402A6 File Offset: 0x0003E4A6
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.gunkMonitor != null)
		{
			this.gunkMonitor.ExpellAllGunk(this.storage);
		}
		this.gunkMonitor = null;
		base.OnCompleteWork(worker);
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x000402CF File Offset: 0x0003E4CF
	protected override void OnStopWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnStopWork(worker);
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x000402DE File Offset: 0x0003E4DE
	protected override void OnAbortWork(WorkerBase worker)
	{
		this.RemoveExpellingRadStatusItem();
		base.OnAbortWork(worker);
		this.gunkMonitor = null;
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x000402F4 File Offset: 0x0003E4F4
	private void RemoveExpellingRadStatusItem()
	{
		if (Sim.IsRadiationEnabled())
		{
			base.worker.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.ExpellingRads, false);
		}
	}

	// Token: 0x04000713 RID: 1811
	private Storage storage;

	// Token: 0x04000714 RID: 1812
	private GunkMonitor.Instance gunkMonitor;
}
