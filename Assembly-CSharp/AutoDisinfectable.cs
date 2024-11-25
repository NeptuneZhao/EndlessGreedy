using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200052B RID: 1323
[AddComponentMenu("KMonoBehaviour/Workable/AutoDisinfectable")]
public class AutoDisinfectable : Workable
{
	// Token: 0x06001DC2 RID: 7618 RVA: 0x000A5234 File Offset: 0x000A3434
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.resetProgressOnStop = true;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000A529C File Offset: 0x000A349C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<AutoDisinfectable>(493375141, AutoDisinfectable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x000A5317 File Offset: 0x000A3517
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("AutoDisinfectable.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000A5338 File Offset: 0x000A3538
	public void RefreshChore()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.enableAutoDisinfect || !SaveGame.Instance.enableAutoDisinfect)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Autodisinfect Disabled");
				this.chore = null;
				return;
			}
		}
		else if (this.chore == null || !(this.chore.driver != null))
		{
			int diseaseCount = this.primaryElement.DiseaseCount;
			if (this.chore == null && diseaseCount > SaveGame.Instance.minGermCountForDisinfect)
			{
				this.chore = new WorkChore<AutoDisinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
				return;
			}
			if (diseaseCount < SaveGame.Instance.minGermCountForDisinfect && this.chore != null)
			{
				this.chore.Cancel("AutoDisinfectable.Update");
				this.chore = null;
			}
		}
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x000A5419 File Offset: 0x000A3619
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x000A543A File Offset: 0x000A363A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000A546C File Offset: 0x000A366C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000A54DC File Offset: 0x000A36DC
	private void EnableAutoDisinfect()
	{
		this.enableAutoDisinfect = true;
		this.RefreshChore();
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x000A54EB File Offset: 0x000A36EB
	private void DisableAutoDisinfect()
	{
		this.enableAutoDisinfect = false;
		this.RefreshChore();
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000A54FC File Offset: 0x000A36FC
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.enableAutoDisinfect)
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.NAME, new System.Action(this.EnableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.NAME, new System.Action(this.DisableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 10f);
	}

	// Token: 0x040010B6 RID: 4278
	private Chore chore;

	// Token: 0x040010B7 RID: 4279
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x040010B8 RID: 4280
	private float diseasePerSecond;

	// Token: 0x040010B9 RID: 4281
	[MyCmpGet]
	private PrimaryElement primaryElement;

	// Token: 0x040010BA RID: 4282
	[Serialize]
	private bool enableAutoDisinfect = true;

	// Token: 0x040010BB RID: 4283
	private static readonly EventSystem.IntraObjectHandler<AutoDisinfectable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<AutoDisinfectable>(delegate(AutoDisinfectable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
