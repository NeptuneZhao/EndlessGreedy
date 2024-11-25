using System;
using System.Linq;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D32 RID: 3378
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleMinionWidget")]
public class ScheduleMinionWidget : KMonoBehaviour
{
	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x06006A38 RID: 27192 RVA: 0x00280268 File Offset: 0x0027E468
	// (set) Token: 0x06006A39 RID: 27193 RVA: 0x00280270 File Offset: 0x0027E470
	public Schedulable schedulable { get; private set; }

	// Token: 0x06006A3A RID: 27194 RVA: 0x0028027C File Offset: 0x0027E47C
	public void ChangeAssignment(Schedule targetSchedule, Schedulable schedulable)
	{
		DebugUtil.LogArgs(new object[]
		{
			"Assigning",
			schedulable,
			"from",
			ScheduleManager.Instance.GetSchedule(schedulable).name,
			"to",
			targetSchedule.name
		});
		ScheduleManager.Instance.GetSchedule(schedulable).Unassign(schedulable);
		targetSchedule.Assign(schedulable);
	}

	// Token: 0x06006A3B RID: 27195 RVA: 0x002802E4 File Offset: 0x0027E4E4
	public void Setup(Schedulable schedulable)
	{
		this.schedulable = schedulable;
		IAssignableIdentity component = schedulable.GetComponent<IAssignableIdentity>();
		this.portrait.SetIdentityObject(component, true);
		this.label.text = component.GetProperName();
		MinionIdentity minionIdentity = component as MinionIdentity;
		StoredMinionIdentity storedMinionIdentity = component as StoredMinionIdentity;
		this.RefreshWidgetWorldData();
		if (minionIdentity != null)
		{
			Traits component2 = minionIdentity.GetComponent<Traits>();
			if (component2.HasTrait("NightOwl"))
			{
				this.nightOwlIcon.SetActive(true);
			}
			else if (component2.HasTrait("EarlyBird"))
			{
				this.earlyBirdIcon.SetActive(true);
			}
		}
		else if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.traitIDs.Contains("NightOwl"))
			{
				this.nightOwlIcon.SetActive(true);
			}
			else if (storedMinionIdentity.traitIDs.Contains("EarlyBird"))
			{
				this.earlyBirdIcon.SetActive(true);
			}
		}
		this.dropDown.Initialize(ScheduleManager.Instance.GetSchedules().Cast<IListableOption>(), new Action<IListableOption, object>(this.OnDropEntryClick), null, new Action<DropDownEntry, object>(this.DropEntryRefreshAction), false, schedulable);
	}

	// Token: 0x06006A3C RID: 27196 RVA: 0x002803F4 File Offset: 0x0027E5F4
	public void RefreshWidgetWorldData()
	{
		this.worldContainer.SetActive(DlcManager.IsExpansion1Active());
		MinionIdentity minionIdentity = this.schedulable.GetComponent<IAssignableIdentity>() as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer myWorld = minionIdentity.GetMyWorld();
			string text = myWorld.GetComponent<ClusterGridEntity>().Name;
			Image componentInChildren = this.worldContainer.GetComponentInChildren<Image>();
			componentInChildren.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
			componentInChildren.SetAlpha((ClusterManager.Instance.activeWorld == myWorld) ? 1f : 0.7f);
			if (ClusterManager.Instance.activeWorld != myWorld)
			{
				text = string.Concat(new string[]
				{
					"<color=",
					Constants.NEUTRAL_COLOR_STR,
					">",
					text,
					"</color>"
				});
			}
			this.worldContainer.GetComponentInChildren<LocText>().SetText(text);
		}
	}

	// Token: 0x06006A3D RID: 27197 RVA: 0x002804DC File Offset: 0x0027E6DC
	private void OnDropEntryClick(IListableOption option, object obj)
	{
		Schedule targetSchedule = (Schedule)option;
		this.ChangeAssignment(targetSchedule, this.schedulable);
	}

	// Token: 0x06006A3E RID: 27198 RVA: 0x00280500 File Offset: 0x0027E700
	private void DropEntryRefreshAction(DropDownEntry entry, object obj)
	{
		Schedule schedule = (Schedule)entry.entryData;
		if (((Schedulable)obj).GetSchedule() == schedule)
		{
			entry.label.text = string.Format(UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, schedule.name);
			entry.button.isInteractable = false;
		}
		else
		{
			entry.label.text = schedule.name;
			entry.button.isInteractable = true;
		}
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(false);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(true);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(false);
	}

	// Token: 0x06006A3F RID: 27199 RVA: 0x002805D4 File Offset: 0x0027E7D4
	public void SetupBlank(Schedule schedule)
	{
		this.label.text = UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_BLANK;
		this.dropDown.Initialize(Components.LiveMinionIdentities.Items.Cast<IListableOption>(), new Action<IListableOption, object>(this.OnBlankDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.BlankDropEntrySort), new Action<DropDownEntry, object>(this.BlankDropEntryRefreshAction), false, schedule);
		Components.LiveMinionIdentities.OnAdd += this.OnLivingMinionsChanged;
		Components.LiveMinionIdentities.OnRemove += this.OnLivingMinionsChanged;
	}

	// Token: 0x06006A40 RID: 27200 RVA: 0x00280662 File Offset: 0x0027E862
	private void OnLivingMinionsChanged(MinionIdentity minion)
	{
		this.dropDown.ChangeContent(Components.LiveMinionIdentities.Items.Cast<IListableOption>());
	}

	// Token: 0x06006A41 RID: 27201 RVA: 0x00280680 File Offset: 0x0027E880
	private void OnBlankDropEntryClick(IListableOption option, object obj)
	{
		Schedule targetSchedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)option;
		if (minionIdentity == null || minionIdentity.HasTag(GameTags.Dead))
		{
			return;
		}
		this.ChangeAssignment(targetSchedule, minionIdentity.GetComponent<Schedulable>());
	}

	// Token: 0x06006A42 RID: 27202 RVA: 0x002806C0 File Offset: 0x0027E8C0
	private void BlankDropEntryRefreshAction(DropDownEntry entry, object obj)
	{
		Schedule schedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)entry.entryData;
		WorldContainer myWorld = minionIdentity.GetMyWorld();
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(DlcManager.IsExpansion1Active());
		Image reference = entry.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("worldIcon");
		reference.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
		reference.SetAlpha((ClusterManager.Instance.activeWorld == myWorld) ? 1f : 0.7f);
		string text = myWorld.GetComponent<ClusterGridEntity>().Name;
		if (ClusterManager.Instance.activeWorld != myWorld)
		{
			text = string.Concat(new string[]
			{
				"<color=",
				Constants.NEUTRAL_COLOR_STR,
				">",
				text,
				"</color>"
			});
		}
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("worldLabel").SetText(text);
		if (schedule.IsAssigned(minionIdentity.GetComponent<Schedulable>()))
		{
			entry.label.text = string.Format(UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, minionIdentity.GetProperName());
			entry.button.isInteractable = false;
		}
		else
		{
			entry.label.text = minionIdentity.GetProperName();
			entry.button.isInteractable = true;
		}
		Traits component = minionIdentity.GetComponent<Traits>();
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightOwlIcon").gameObject.SetActive(component.HasTrait("NightOwl"));
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EarlyBirdIcon").gameObject.SetActive(component.HasTrait("EarlyBird"));
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(false);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(true);
	}

	// Token: 0x06006A43 RID: 27203 RVA: 0x002808B4 File Offset: 0x0027EAB4
	private int BlankDropEntrySort(IListableOption a, IListableOption b, object obj)
	{
		Schedule schedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)a;
		MinionIdentity minionIdentity2 = (MinionIdentity)b;
		bool flag = schedule.IsAssigned(minionIdentity.GetComponent<Schedulable>());
		bool flag2 = schedule.IsAssigned(minionIdentity2.GetComponent<Schedulable>());
		if (flag && !flag2)
		{
			return -1;
		}
		if (!flag && flag2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x06006A44 RID: 27204 RVA: 0x00280901 File Offset: 0x0027EB01
	protected override void OnCleanUp()
	{
		Components.LiveMinionIdentities.OnAdd -= this.OnLivingMinionsChanged;
		Components.LiveMinionIdentities.OnRemove -= this.OnLivingMinionsChanged;
	}

	// Token: 0x04004861 RID: 18529
	[SerializeField]
	private CrewPortrait portrait;

	// Token: 0x04004862 RID: 18530
	[SerializeField]
	private DropDown dropDown;

	// Token: 0x04004863 RID: 18531
	[SerializeField]
	private LocText label;

	// Token: 0x04004864 RID: 18532
	[SerializeField]
	private GameObject nightOwlIcon;

	// Token: 0x04004865 RID: 18533
	[SerializeField]
	private GameObject earlyBirdIcon;

	// Token: 0x04004866 RID: 18534
	[SerializeField]
	private GameObject worldContainer;
}
