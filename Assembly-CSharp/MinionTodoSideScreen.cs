using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D7D RID: 3453
public class MinionTodoSideScreen : SideScreenContent
{
	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x06006CA0 RID: 27808 RVA: 0x0028DCF8 File Offset: 0x0028BEF8
	public static List<JobsTableScreen.PriorityInfo> priorityInfo
	{
		get
		{
			if (MinionTodoSideScreen._priorityInfo == null)
			{
				MinionTodoSideScreen._priorityInfo = new List<JobsTableScreen.PriorityInfo>
				{
					new JobsTableScreen.PriorityInfo(4, Assets.GetSprite("ic_dupe"), UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY),
					new JobsTableScreen.PriorityInfo(3, Assets.GetSprite("notification_exclamation"), UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY),
					new JobsTableScreen.PriorityInfo(2, Assets.GetSprite("status_item_room_required"), UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS),
					new JobsTableScreen.PriorityInfo(1, Assets.GetSprite("status_item_prioritized"), UI.JOBSSCREEN.PRIORITY_CLASS.HIGH),
					new JobsTableScreen.PriorityInfo(0, null, UI.JOBSSCREEN.PRIORITY_CLASS.BASIC),
					new JobsTableScreen.PriorityInfo(-1, Assets.GetSprite("icon_gear"), UI.JOBSSCREEN.PRIORITY_CLASS.IDLE)
				};
			}
			return MinionTodoSideScreen._priorityInfo;
		}
	}

	// Token: 0x06006CA1 RID: 27809 RVA: 0x0028DDD0 File Offset: 0x0028BFD0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.priorityGroups.Count != 0)
		{
			return;
		}
		foreach (JobsTableScreen.PriorityInfo priorityInfo in MinionTodoSideScreen.priorityInfo)
		{
			PriorityScreen.PriorityClass priority = (PriorityScreen.PriorityClass)priorityInfo.priority;
			if (priority == PriorityScreen.PriorityClass.basic)
			{
				for (int i = 5; i >= 0; i--)
				{
					global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple = new global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, i, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
					tuple.third.name = "PriorityGroup_" + priorityInfo.name + "_" + i.ToString();
					tuple.third.gameObject.SetActive(true);
					JobsTableScreen.PriorityInfo priorityInfo2 = JobsTableScreen.priorityInfo[i];
					tuple.third.GetReference<LocText>("Title").text = priorityInfo2.name.text.ToUpper();
					tuple.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo2.sprite;
					this.priorityGroups.Add(tuple);
				}
			}
			else
			{
				global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple2 = new global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, 3, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
				tuple2.third.name = "PriorityGroup_" + priorityInfo.name;
				tuple2.third.gameObject.SetActive(true);
				tuple2.third.GetReference<LocText>("Title").text = priorityInfo.name.text.ToUpper();
				tuple2.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo.sprite;
				this.priorityGroups.Add(tuple2);
			}
		}
	}

	// Token: 0x06006CA2 RID: 27810 RVA: 0x0028DFBC File Offset: 0x0028C1BC
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MinionIdentity>() != null && !target.HasTag(GameTags.Dead);
	}

	// Token: 0x06006CA3 RID: 27811 RVA: 0x0028DFDC File Offset: 0x0028C1DC
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.refreshHandle.ClearScheduler();
	}

	// Token: 0x06006CA4 RID: 27812 RVA: 0x0028DFEF File Offset: 0x0028C1EF
	public override void SetTarget(GameObject target)
	{
		this.refreshHandle.ClearScheduler();
		if (this.priorityGroups.Count == 0)
		{
			this.OnPrefabInit();
		}
		base.SetTarget(target);
	}

	// Token: 0x06006CA5 RID: 27813 RVA: 0x0028E016 File Offset: 0x0028C216
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.PopulateElements(null);
	}

	// Token: 0x06006CA6 RID: 27814 RVA: 0x0028E028 File Offset: 0x0028C228
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.refreshHandle.ClearScheduler();
		if (!show)
		{
			if (this.useOffscreenIndicators)
			{
				foreach (GameObject target in this.choreTargets)
				{
					OffscreenIndicator.Instance.DeactivateIndicator(target);
				}
			}
			return;
		}
		if (DetailsScreen.Instance.target == null)
		{
			return;
		}
		this.choreConsumer = DetailsScreen.Instance.target.GetComponent<ChoreConsumer>();
		this.PopulateElements(null);
	}

	// Token: 0x06006CA7 RID: 27815 RVA: 0x0028E0CC File Offset: 0x0028C2CC
	private void PopulateElements(object data = null)
	{
		this.refreshHandle.ClearScheduler();
		this.refreshHandle = UIScheduler.Instance.Schedule("RefreshToDoList", 0.1f, new Action<object>(this.PopulateElements), null, null);
		ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
		ChoreConsumer.PreconditionSnapshot lastPreconditionSnapshot = this.choreConsumer.GetLastPreconditionSnapshot();
		if (lastPreconditionSnapshot.doFailedContextsNeedSorting)
		{
			lastPreconditionSnapshot.failedContexts.Sort();
			lastPreconditionSnapshot.doFailedContextsNeedSorting = false;
		}
		pooledList.AddRange(lastPreconditionSnapshot.failedContexts);
		pooledList.AddRange(lastPreconditionSnapshot.succeededContexts);
		Chore.Precondition.Context choreB = default(Chore.Precondition.Context);
		MinionTodoChoreEntry minionTodoChoreEntry = null;
		int num = 0;
		Schedulable component = DetailsScreen.Instance.target.GetComponent<Schedulable>();
		string arg = "";
		Schedule schedule = component.GetSchedule();
		if (schedule != null)
		{
			arg = schedule.GetCurrentScheduleBlock().name;
		}
		this.currentScheduleBlockLabel.SetText(string.Format(UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CURRENT_SCHEDULE_BLOCK, arg));
		this.choreTargets.Clear();
		bool flag = false;
		this.activeChoreEntries = 0;
		for (int i = pooledList.Count - 1; i >= 0; i--)
		{
			if (pooledList[i].chore != null && !pooledList[i].chore.target.isNull && !(pooledList[i].chore.target.gameObject == null) && pooledList[i].IsPotentialSuccess())
			{
				if (pooledList[i].chore.driver == this.choreConsumer.choreDriver)
				{
					this.currentTask.Apply(pooledList[i]);
					minionTodoChoreEntry = this.currentTask;
					choreB = pooledList[i];
					num = 0;
					flag = true;
				}
				else if (!flag && this.activeChoreEntries != 0 && GameUtil.AreChoresUIMergeable(pooledList[i], choreB))
				{
					num++;
					minionTodoChoreEntry.SetMoreAmount(num);
				}
				else
				{
					HierarchyReferences hierarchyReferences = this.PriorityGroupForPriority(this.choreConsumer, pooledList[i].chore);
					if (hierarchyReferences == null)
					{
						DebugUtil.DevLogError(string.Format("Priority group was null for {0} with priority class {1} and personaly priority {2}", pooledList[i].chore.GetReportName(null), pooledList[i].chore.masterPriority.priority_class, this.choreConsumer.GetPersonalPriority(pooledList[i].chore.choreType)));
					}
					else
					{
						MinionTodoChoreEntry choreEntry = this.GetChoreEntry(hierarchyReferences.GetReference<RectTransform>("EntriesContainer"));
						choreEntry.Apply(pooledList[i]);
						minionTodoChoreEntry = choreEntry;
						choreB = pooledList[i];
						num = 0;
						flag = false;
					}
				}
			}
		}
		pooledList.Recycle();
		for (int j = this.choreEntries.Count - 1; j >= this.activeChoreEntries; j--)
		{
			this.choreEntries[j].gameObject.SetActive(false);
		}
		foreach (global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple in this.priorityGroups)
		{
			RectTransform reference = tuple.third.GetReference<RectTransform>("EntriesContainer");
			tuple.third.gameObject.SetActive(reference.childCount > 0);
		}
	}

	// Token: 0x06006CA8 RID: 27816 RVA: 0x0028E424 File Offset: 0x0028C624
	private MinionTodoChoreEntry GetChoreEntry(RectTransform parent)
	{
		MinionTodoChoreEntry minionTodoChoreEntry;
		if (this.activeChoreEntries >= this.choreEntries.Count - 1)
		{
			minionTodoChoreEntry = Util.KInstantiateUI<MinionTodoChoreEntry>(this.taskEntryPrefab.gameObject, parent.gameObject, false);
			this.choreEntries.Add(minionTodoChoreEntry);
		}
		else
		{
			minionTodoChoreEntry = this.choreEntries[this.activeChoreEntries];
			minionTodoChoreEntry.transform.SetParent(parent);
			minionTodoChoreEntry.transform.SetAsLastSibling();
		}
		this.activeChoreEntries++;
		minionTodoChoreEntry.gameObject.SetActive(true);
		return minionTodoChoreEntry;
	}

	// Token: 0x06006CA9 RID: 27817 RVA: 0x0028E4B0 File Offset: 0x0028C6B0
	private HierarchyReferences PriorityGroupForPriority(ChoreConsumer choreConsumer, Chore chore)
	{
		foreach (global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple in this.priorityGroups)
		{
			if (tuple.first == chore.masterPriority.priority_class)
			{
				if (chore.masterPriority.priority_class != PriorityScreen.PriorityClass.basic)
				{
					return tuple.third;
				}
				if (tuple.second == choreConsumer.GetPersonalPriority(chore.choreType))
				{
					return tuple.third;
				}
			}
		}
		return null;
	}

	// Token: 0x06006CAA RID: 27818 RVA: 0x0028E548 File Offset: 0x0028C748
	private void Button_onPointerEnter()
	{
		throw new NotImplementedException();
	}

	// Token: 0x04004A19 RID: 18969
	private bool useOffscreenIndicators;

	// Token: 0x04004A1A RID: 18970
	public MinionTodoChoreEntry taskEntryPrefab;

	// Token: 0x04004A1B RID: 18971
	public GameObject priorityGroupPrefab;

	// Token: 0x04004A1C RID: 18972
	public GameObject taskEntryContainer;

	// Token: 0x04004A1D RID: 18973
	public MinionTodoChoreEntry currentTask;

	// Token: 0x04004A1E RID: 18974
	public LocText currentScheduleBlockLabel;

	// Token: 0x04004A1F RID: 18975
	private List<global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>> priorityGroups = new List<global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>>();

	// Token: 0x04004A20 RID: 18976
	private List<MinionTodoChoreEntry> choreEntries = new List<MinionTodoChoreEntry>();

	// Token: 0x04004A21 RID: 18977
	private List<GameObject> choreTargets = new List<GameObject>();

	// Token: 0x04004A22 RID: 18978
	private SchedulerHandle refreshHandle;

	// Token: 0x04004A23 RID: 18979
	private ChoreConsumer choreConsumer;

	// Token: 0x04004A24 RID: 18980
	[SerializeField]
	private ColorStyleSetting buttonColorSettingCurrent;

	// Token: 0x04004A25 RID: 18981
	[SerializeField]
	private ColorStyleSetting buttonColorSettingStandard;

	// Token: 0x04004A26 RID: 18982
	private static List<JobsTableScreen.PriorityInfo> _priorityInfo;

	// Token: 0x04004A27 RID: 18983
	private int activeChoreEntries;
}
