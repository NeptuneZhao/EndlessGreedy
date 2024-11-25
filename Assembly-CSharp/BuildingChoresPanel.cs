using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BEA RID: 3050
public class BuildingChoresPanel : TargetPanel
{
	// Token: 0x06005CE8 RID: 23784 RVA: 0x00222370 File Offset: 0x00220570
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		return component != null && component.HasTag(GameTags.HasChores) && !component.HasTag(GameTags.BaseMinion);
	}

	// Token: 0x06005CE9 RID: 23785 RVA: 0x002223AA File Offset: 0x002205AA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreGroup = Util.KInstantiateUI<HierarchyReferences>(this.choreGroupPrefab, base.gameObject, false);
		this.choreGroup.gameObject.SetActive(true);
	}

	// Token: 0x06005CEA RID: 23786 RVA: 0x002223DB File Offset: 0x002205DB
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06005CEB RID: 23787 RVA: 0x002223E3 File Offset: 0x002205E3
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		this.Refresh();
	}

	// Token: 0x06005CEC RID: 23788 RVA: 0x002223F2 File Offset: 0x002205F2
	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
	}

	// Token: 0x06005CED RID: 23789 RVA: 0x002223FB File Offset: 0x002205FB
	private void Refresh()
	{
		this.RefreshDetails();
	}

	// Token: 0x06005CEE RID: 23790 RVA: 0x00222404 File Offset: 0x00220604
	private void RefreshDetails()
	{
		int myParentWorldId = this.selectedTarget.GetMyParentWorldId();
		List<Chore> list = null;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(myParentWorldId, out list);
		int num = 0;
		while (list != null && num < list.Count)
		{
			Chore chore = list[num];
			if (!chore.isNull && chore.gameObject == this.selectedTarget)
			{
				this.AddChoreEntry(chore);
			}
			num++;
		}
		List<FetchChore> list2 = null;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(myParentWorldId, out list2);
		int num2 = 0;
		while (list2 != null && num2 < list2.Count)
		{
			FetchChore fetchChore = list2[num2];
			if (!fetchChore.isNull && fetchChore.gameObject == this.selectedTarget)
			{
				this.AddChoreEntry(fetchChore);
			}
			num2++;
		}
		for (int i = this.activeDupeEntries; i < this.dupeEntries.Count; i++)
		{
			this.dupeEntries[i].gameObject.SetActive(false);
		}
		this.activeDupeEntries = 0;
		for (int j = this.activeChoreEntries; j < this.choreEntries.Count; j++)
		{
			this.choreEntries[j].gameObject.SetActive(false);
		}
		this.activeChoreEntries = 0;
	}

	// Token: 0x06005CEF RID: 23791 RVA: 0x0022254C File Offset: 0x0022074C
	private void AddChoreEntry(Chore chore)
	{
		HierarchyReferences choreEntry = this.GetChoreEntry(GameUtil.GetChoreName(chore, null), chore.choreType, this.choreGroup.GetReference<RectTransform>("EntriesContainer"));
		FetchChore fetchChore = chore as FetchChore;
		ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
		List<GameObject> list = new List<GameObject>();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			list.Add(minionIdentity.gameObject);
		}
		foreach (RobotAi.Instance instance in Components.LiveRobotsIdentities.Items)
		{
			list.Add(instance.gameObject);
		}
		foreach (GameObject gameObject in list)
		{
			pooledList.Clear();
			ChoreConsumer component = gameObject.GetComponent<ChoreConsumer>();
			Chore.Precondition.Context context = default(Chore.Precondition.Context);
			ChoreConsumer.PreconditionSnapshot lastPreconditionSnapshot = component.GetLastPreconditionSnapshot();
			if (lastPreconditionSnapshot.doFailedContextsNeedSorting)
			{
				lastPreconditionSnapshot.failedContexts.Sort();
				lastPreconditionSnapshot.doFailedContextsNeedSorting = false;
			}
			pooledList.AddRange(lastPreconditionSnapshot.failedContexts);
			pooledList.AddRange(lastPreconditionSnapshot.succeededContexts);
			int num = -1;
			int num2 = 0;
			for (int i = pooledList.Count - 1; i >= 0; i--)
			{
				if (!(pooledList[i].chore.driver != null) || !(pooledList[i].chore.driver != component.choreDriver))
				{
					bool flag = pooledList[i].IsPotentialSuccess();
					if (flag)
					{
						num2++;
					}
					FetchAreaChore fetchAreaChore = pooledList[i].chore as FetchAreaChore;
					if (pooledList[i].chore == chore || (fetchChore != null && fetchAreaChore != null && fetchAreaChore.smi.SameDestination(fetchChore)))
					{
						num = (flag ? num2 : int.MaxValue);
						context = pooledList[i];
						break;
					}
				}
			}
			if (num >= 0)
			{
				this.DupeEntryDatas.Add(new BuildingChoresPanel.DupeEntryData
				{
					consumer = component,
					context = context,
					personalPriority = component.GetPersonalPriority(chore.choreType),
					rank = num
				});
			}
		}
		pooledList.Recycle();
		this.DupeEntryDatas.Sort();
		foreach (BuildingChoresPanel.DupeEntryData data in this.DupeEntryDatas)
		{
			this.GetDupeEntry(data, choreEntry.GetReference<RectTransform>("DupeContainer"));
		}
		this.DupeEntryDatas.Clear();
	}

	// Token: 0x06005CF0 RID: 23792 RVA: 0x00222870 File Offset: 0x00220A70
	private HierarchyReferences GetChoreEntry(string label, ChoreType choreType, RectTransform parent)
	{
		HierarchyReferences hierarchyReferences;
		if (this.activeChoreEntries >= this.choreEntries.Count)
		{
			hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.chorePrefab, parent.gameObject, false);
			this.choreEntries.Add(hierarchyReferences);
		}
		else
		{
			hierarchyReferences = this.choreEntries[this.activeChoreEntries];
			hierarchyReferences.transform.SetParent(parent);
			hierarchyReferences.transform.SetAsLastSibling();
		}
		this.activeChoreEntries++;
		hierarchyReferences.GetReference<LocText>("ChoreLabel").text = label;
		hierarchyReferences.GetReference<LocText>("ChoreSubLabel").text = GameUtil.ChoreGroupsForChoreType(choreType);
		Image reference = hierarchyReferences.GetReference<Image>("Icon");
		if (choreType.groups.Length != 0)
		{
			Sprite sprite = Assets.GetSprite(choreType.groups[0].sprite);
			reference.sprite = sprite;
			reference.gameObject.SetActive(true);
			reference.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, choreType.groups[0].Name);
		}
		else
		{
			reference.gameObject.SetActive(false);
		}
		Image reference2 = hierarchyReferences.GetReference<Image>("Icon2");
		if (choreType.groups.Length > 1)
		{
			Sprite sprite2 = Assets.GetSprite(choreType.groups[1].sprite);
			reference2.sprite = sprite2;
			reference2.gameObject.SetActive(true);
			reference2.GetComponent<ToolTip>().toolTip = string.Format(UI.DETAILTABS.BUILDING_CHORES.CHORE_TYPE_TOOLTIP, choreType.groups[1].Name);
		}
		else
		{
			reference2.gameObject.SetActive(false);
		}
		hierarchyReferences.gameObject.SetActive(true);
		return hierarchyReferences;
	}

	// Token: 0x06005CF1 RID: 23793 RVA: 0x00222A0C File Offset: 0x00220C0C
	private BuildingChoresPanelDupeRow GetDupeEntry(BuildingChoresPanel.DupeEntryData data, RectTransform parent)
	{
		BuildingChoresPanelDupeRow buildingChoresPanelDupeRow;
		if (this.activeDupeEntries >= this.dupeEntries.Count)
		{
			buildingChoresPanelDupeRow = Util.KInstantiateUI<BuildingChoresPanelDupeRow>(this.dupePrefab.gameObject, parent.gameObject, false);
			this.dupeEntries.Add(buildingChoresPanelDupeRow);
		}
		else
		{
			buildingChoresPanelDupeRow = this.dupeEntries[this.activeDupeEntries];
			buildingChoresPanelDupeRow.transform.SetParent(parent);
			buildingChoresPanelDupeRow.transform.SetAsLastSibling();
		}
		this.activeDupeEntries++;
		buildingChoresPanelDupeRow.Init(data);
		buildingChoresPanelDupeRow.gameObject.SetActive(true);
		return buildingChoresPanelDupeRow;
	}

	// Token: 0x04003E32 RID: 15922
	public GameObject choreGroupPrefab;

	// Token: 0x04003E33 RID: 15923
	public GameObject chorePrefab;

	// Token: 0x04003E34 RID: 15924
	public BuildingChoresPanelDupeRow dupePrefab;

	// Token: 0x04003E35 RID: 15925
	private GameObject detailsPanel;

	// Token: 0x04003E36 RID: 15926
	private DetailsPanelDrawer drawer;

	// Token: 0x04003E37 RID: 15927
	private HierarchyReferences choreGroup;

	// Token: 0x04003E38 RID: 15928
	private List<HierarchyReferences> choreEntries = new List<HierarchyReferences>();

	// Token: 0x04003E39 RID: 15929
	private int activeChoreEntries;

	// Token: 0x04003E3A RID: 15930
	private List<BuildingChoresPanelDupeRow> dupeEntries = new List<BuildingChoresPanelDupeRow>();

	// Token: 0x04003E3B RID: 15931
	private int activeDupeEntries;

	// Token: 0x04003E3C RID: 15932
	private List<BuildingChoresPanel.DupeEntryData> DupeEntryDatas = new List<BuildingChoresPanel.DupeEntryData>();

	// Token: 0x02001CD1 RID: 7377
	public class DupeEntryData : IComparable<BuildingChoresPanel.DupeEntryData>
	{
		// Token: 0x0600A704 RID: 42756 RVA: 0x00399668 File Offset: 0x00397868
		public int CompareTo(BuildingChoresPanel.DupeEntryData other)
		{
			if (this.personalPriority != other.personalPriority)
			{
				return other.personalPriority.CompareTo(this.personalPriority);
			}
			if (this.rank != other.rank)
			{
				return this.rank.CompareTo(other.rank);
			}
			if (this.consumer.GetProperName() != other.consumer.GetProperName())
			{
				return this.consumer.GetProperName().CompareTo(other.consumer.GetProperName());
			}
			return this.consumer.GetInstanceID().CompareTo(other.consumer.GetInstanceID());
		}

		// Token: 0x0400852E RID: 34094
		public ChoreConsumer consumer;

		// Token: 0x0400852F RID: 34095
		public Chore.Precondition.Context context;

		// Token: 0x04008530 RID: 34096
		public int personalPriority;

		// Token: 0x04008531 RID: 34097
		public int rank;
	}
}
