using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200055A RID: 1370
[AddComponentMenu("KMonoBehaviour/Workable/Disinfectable")]
public class Disinfectable : Workable
{
	// Token: 0x06001F99 RID: 8089 RVA: 0x000B1AEC File Offset: 0x000AFCEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		base.Subscribe<Disinfectable>(2127324410, Disinfectable.OnCancelDelegate);
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x000B1BA3 File Offset: 0x000AFDA3
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDisinfect)
		{
			this.MarkForDisinfect(true);
		}
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x000B1BCC File Offset: 0x000AFDCC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x000B1BED File Offset: 0x000AFDED
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x000B1C20 File Offset: 0x000AFE20
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.isMarkedForDisinfect = false;
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x000B1CA2 File Offset: 0x000AFEA2
	private void ToggleMarkForDisinfect()
	{
		if (this.isMarkedForDisinfect)
		{
			this.CancelDisinfection();
			return;
		}
		base.SetWorkTime(10f);
		this.MarkForDisinfect(false);
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x000B1CC8 File Offset: 0x000AFEC8
	private void CancelDisinfection()
	{
		if (this.isMarkedForDisinfect)
		{
			Prioritizable.RemoveRef(base.gameObject);
			base.ShowProgressBar(false);
			this.isMarkedForDisinfect = false;
			this.chore.Cancel("disinfection cancelled");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x000B1D30 File Offset: 0x000AFF30
	public void MarkForDisinfect(bool force = false)
	{
		if (!this.isMarkedForDisinfect || force)
		{
			this.isMarkedForDisinfect = true;
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Disinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x000B1DA4 File Offset: 0x000AFFA4
	private void OnCancel(object data)
	{
		this.CancelDisinfection();
	}

	// Token: 0x040011CC RID: 4556
	private Chore chore;

	// Token: 0x040011CD RID: 4557
	[Serialize]
	private bool isMarkedForDisinfect;

	// Token: 0x040011CE RID: 4558
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x040011CF RID: 4559
	private float diseasePerSecond;

	// Token: 0x040011D0 RID: 4560
	private static readonly EventSystem.IntraObjectHandler<Disinfectable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Disinfectable>(delegate(Disinfectable component, object data)
	{
		component.OnCancel(data);
	});
}
