using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
[AddComponentMenu("KMonoBehaviour/Workable/Studyable")]
public class Studyable : Workable, ISidescreenButtonControl
{
	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x0600240F RID: 9231 RVA: 0x000C9925 File Offset: 0x000C7B25
	public bool Studied
	{
		get
		{
			return this.studied;
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06002410 RID: 9232 RVA: 0x000C992D File Offset: 0x000C7B2D
	public bool Studying
	{
		get
		{
			return this.chore != null && this.chore.InProgress();
		}
	}

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06002411 RID: 9233 RVA: 0x000C9944 File Offset: 0x000C7B44
	public string SidescreenTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06002412 RID: 9234 RVA: 0x000C994B File Offset: 0x000C7B4B
	public string SidescreenStatusMessage
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06002413 RID: 9235 RVA: 0x000C997D File Offset: 0x000C7B7D
	public string SidescreenButtonText
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06002414 RID: 9236 RVA: 0x000C99AF File Offset: 0x000C7BAF
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000C99E1 File Offset: 0x000C7BE1
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x000C99E4 File Offset: 0x000C7BE4
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x000C99EB File Offset: 0x000C7BEB
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000C99EE File Offset: 0x000C7BEE
	public bool SidescreenButtonInteractable()
	{
		return !this.studied;
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000C99FC File Offset: 0x000C7BFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
		this.resetProgressOnStop = false;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		base.SetWorkTime(3600f);
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000C9AC4 File Offset: 0x000C7CC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.studiedIndicator = new MeterController(base.GetComponent<KBatchedAnimController>(), this.meterTrackerSymbol, this.meterAnim, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			this.meterTrackerSymbol
		});
		this.studiedIndicator.meterController.gameObject.AddComponent<LoopingSounds>();
		this.Refresh();
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000C9B22 File Offset: 0x000C7D22
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Studyable.CancelChore");
			this.chore = null;
			base.Trigger(1488501379, null);
		}
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x000C9B50 File Offset: 0x000C7D50
	public void Refresh()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.studied)
		{
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.Studied, null);
			this.studiedIndicator.gameObject.SetActive(true);
			this.studiedIndicator.meterController.Play(this.meterAnim, KAnim.PlayMode.Loop, 1f, 0f);
			this.requiredSkillPerk = null;
			this.UpdateStatusItem(null);
			return;
		}
		if (this.markedForStudy)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<Studyable>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy, null);
		}
		else
		{
			this.CancelChore();
			this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid, false);
		}
		this.studiedIndicator.gameObject.SetActive(false);
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000C9C68 File Offset: 0x000C7E68
	private void ToggleStudyChore()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.studied = true;
			if (this.chore != null)
			{
				this.chore.Cancel("debug");
				this.chore = null;
			}
			base.Trigger(-1436775550, null);
		}
		else
		{
			this.markedForStudy = !this.markedForStudy;
		}
		this.Refresh();
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x000C9CC5 File Offset: 0x000C7EC5
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.studied = true;
		this.chore = null;
		this.Refresh();
		base.Trigger(-1436775550, null);
		if (DlcManager.IsExpansion1Active())
		{
			this.DropDatabanks();
		}
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x000C9CFC File Offset: 0x000C7EFC
	private void DropDatabanks()
	{
		int num = UnityEngine.Random.Range(7, 13);
		for (int i = 0; i <= num; i++)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), base.transform.position + new Vector3(0f, 1f, 0f), Grid.SceneLayer.Ore, null, 0);
			gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x000C9D70 File Offset: 0x000C7F70
	public void OnSidescreenButtonPressed()
	{
		this.ToggleStudyChore();
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x000C9D78 File Offset: 0x000C7F78
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04001490 RID: 5264
	public string meterTrackerSymbol;

	// Token: 0x04001491 RID: 5265
	public string meterAnim;

	// Token: 0x04001492 RID: 5266
	private Chore chore;

	// Token: 0x04001493 RID: 5267
	private const float STUDY_WORK_TIME = 3600f;

	// Token: 0x04001494 RID: 5268
	[Serialize]
	private bool studied;

	// Token: 0x04001495 RID: 5269
	[Serialize]
	private bool markedForStudy;

	// Token: 0x04001496 RID: 5270
	private Guid statusItemGuid;

	// Token: 0x04001497 RID: 5271
	private Guid additionalStatusItemGuid;

	// Token: 0x04001498 RID: 5272
	public MeterController studiedIndicator;
}
