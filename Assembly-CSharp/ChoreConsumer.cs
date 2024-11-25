using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000467 RID: 1127
[AddComponentMenu("KMonoBehaviour/scripts/ChoreConsumer")]
public class ChoreConsumer : KMonoBehaviour, IPersonalPriorityManager
{
	// Token: 0x06001810 RID: 6160 RVA: 0x00080A2E File Offset: 0x0007EC2E
	public ChoreConsumer.PreconditionSnapshot GetLastPreconditionSnapshot()
	{
		return this.preconditionSnapshot;
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x00080A36 File Offset: 0x0007EC36
	public List<Chore.Precondition.Context> GetSuceededPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.succeededContexts;
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x00080A43 File Offset: 0x0007EC43
	public List<Chore.Precondition.Context> GetFailedPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.failedContexts;
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x00080A50 File Offset: 0x0007EC50
	public ChoreConsumer.PreconditionSnapshot GetLastSuccessfulPreconditionSnapshot()
	{
		return this.lastSuccessfulPreconditionSnapshot;
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x00080A58 File Offset: 0x0007EC58
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ChoreGroupManager.instance != null)
		{
			foreach (KeyValuePair<Tag, int> keyValuePair in ChoreGroupManager.instance.DefaultChorePermission)
			{
				bool flag = false;
				foreach (HashedString hashedString in this.userDisabledChoreGroups)
				{
					if (hashedString.HashValue == keyValuePair.Key.GetHashCode())
					{
						flag = true;
						break;
					}
				}
				if (!flag && keyValuePair.Value == 0)
				{
					this.userDisabledChoreGroups.Add(new HashedString(keyValuePair.Key.GetHashCode()));
				}
			}
		}
		this.providers.Add(this.choreProvider);
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x00080B68 File Offset: 0x0007ED68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (this.choreTable != null)
		{
			this.choreTableInstance = new ChoreTable.Instance(this.choreTable, component);
		}
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			int personalPriority = this.GetPersonalPriority(choreGroup);
			this.UpdateChoreTypePriorities(choreGroup, personalPriority);
			this.SetPermittedByUser(choreGroup, personalPriority != 0);
		}
		this.consumerState = new ChoreConsumerState(this);
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x00080C0C File Offset: 0x0007EE0C
	protected override void OnForcedCleanUp()
	{
		if (this.consumerState != null)
		{
			this.consumerState.navigator = null;
		}
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x00080C2F File Offset: 0x0007EE2F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.choreTableInstance != null)
		{
			this.choreTableInstance.OnCleanUp(base.GetComponent<KPrefabID>());
			this.choreTableInstance = null;
		}
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x00080C57 File Offset: 0x0007EE57
	public bool IsPermittedByUser(ChoreGroup chore_group)
	{
		return chore_group == null || !this.userDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x00080C74 File Offset: 0x0007EE74
	public void SetPermittedByUser(ChoreGroup chore_group, bool is_allowed)
	{
		if (is_allowed)
		{
			if (this.userDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.userDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.userDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x00080CD2 File Offset: 0x0007EED2
	public bool IsPermittedByTraits(ChoreGroup chore_group)
	{
		return chore_group == null || !this.traitDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x00080CF0 File Offset: 0x0007EEF0
	public void SetPermittedByTraits(ChoreGroup chore_group, bool is_enabled)
	{
		if (is_enabled)
		{
			if (this.traitDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.traitDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.traitDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x00080D50 File Offset: 0x0007EF50
	private bool ChooseChore(ref Chore.Precondition.Context out_context, List<Chore.Precondition.Context> succeeded_contexts)
	{
		if (succeeded_contexts.Count == 0)
		{
			return false;
		}
		Chore currentChore = this.choreDriver.GetCurrentChore();
		if (currentChore == null)
		{
			for (int i = succeeded_contexts.Count - 1; i >= 0; i--)
			{
				Chore.Precondition.Context context = succeeded_contexts[i];
				if (context.IsSuccess())
				{
					out_context = context;
					return true;
				}
			}
		}
		else
		{
			int interruptPriority = Db.Get().ChoreTypes.TopPriority.interruptPriority;
			int num = (currentChore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : currentChore.choreType.interruptPriority;
			for (int j = succeeded_contexts.Count - 1; j >= 0; j--)
			{
				Chore.Precondition.Context context2 = succeeded_contexts[j];
				if (context2.IsSuccess() && ((context2.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : context2.interruptPriority) > num && !currentChore.choreType.interruptExclusion.Overlaps(context2.chore.choreType.tags))
				{
					out_context = context2;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x00080E50 File Offset: 0x0007F050
	public bool FindNextChore(ref Chore.Precondition.Context out_context)
	{
		this.preconditionSnapshot.Clear();
		this.consumerState.Refresh();
		if (this.consumerState.hasSolidTransferArm)
		{
			global::Debug.Assert(this.stationaryReach > 0);
			CellOffset offset = Grid.GetOffset(Grid.PosToCell(this));
			Extents extents = new Extents(offset.x, offset.y, this.stationaryReach);
			ListPool<ScenePartitionerEntry, ChoreConsumer>.PooledList pooledList = ListPool<ScenePartitionerEntry, ChoreConsumer>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.fetchChoreLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				if (scenePartitionerEntry.obj == null)
				{
					DebugUtil.Assert(false, "FindNextChore found an entry that was null");
				}
				else
				{
					FetchChore fetchChore = scenePartitionerEntry.obj as FetchChore;
					if (fetchChore == null)
					{
						DebugUtil.Assert(false, "FindNextChore found an entry that wasn't a FetchChore");
					}
					else if (fetchChore.target == null)
					{
						DebugUtil.Assert(false, "FindNextChore found an entry with a null target");
					}
					else if (fetchChore.isNull)
					{
						global::Debug.LogWarning("FindNextChore found an entry that isNull");
					}
					else
					{
						int cell = Grid.PosToCell(fetchChore.gameObject);
						if (this.consumerState.solidTransferArm.IsCellReachable(cell))
						{
							fetchChore.CollectChoresFromGlobalChoreProvider(this.consumerState, this.preconditionSnapshot.succeededContexts, this.preconditionSnapshot.failedContexts, false);
						}
					}
				}
			}
			pooledList.Recycle();
		}
		else
		{
			for (int i = 0; i < this.providers.Count; i++)
			{
				this.providers[i].CollectChores(this.consumerState, this.preconditionSnapshot.succeededContexts, this.preconditionSnapshot.failedContexts);
			}
		}
		this.preconditionSnapshot.succeededContexts.Sort();
		List<Chore.Precondition.Context> succeededContexts = this.preconditionSnapshot.succeededContexts;
		bool flag = this.ChooseChore(ref out_context, succeededContexts);
		if (flag)
		{
			this.preconditionSnapshot.CopyTo(this.lastSuccessfulPreconditionSnapshot);
		}
		return flag;
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x00081048 File Offset: 0x0007F248
	public void AddProvider(ChoreProvider provider)
	{
		DebugUtil.Assert(provider != null);
		this.providers.Add(provider);
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x00081062 File Offset: 0x0007F262
	public void RemoveProvider(ChoreProvider provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x06001820 RID: 6176 RVA: 0x00081071 File Offset: 0x0007F271
	public void AddUrge(Urge urge)
	{
		DebugUtil.Assert(urge != null);
		this.urges.Add(urge);
		base.Trigger(-736698276, urge);
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x00081094 File Offset: 0x0007F294
	public void RemoveUrge(Urge urge)
	{
		this.urges.Remove(urge);
		base.Trigger(231622047, urge);
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x000810AF File Offset: 0x0007F2AF
	public bool HasUrge(Urge urge)
	{
		return this.urges.Contains(urge);
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x000810BD File Offset: 0x0007F2BD
	public List<Urge> GetUrges()
	{
		return this.urges;
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x000810C5 File Offset: 0x0007F2C5
	[Conditional("ENABLE_LOGGER")]
	public void Log(string evt, string param)
	{
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x000810C8 File Offset: 0x0007F2C8
	public bool IsPermittedOrEnabled(ChoreType chore_type, Chore chore)
	{
		if (chore_type.groups.Length == 0)
		{
			return true;
		}
		bool flag = false;
		bool flag2 = true;
		for (int i = 0; i < chore_type.groups.Length; i++)
		{
			ChoreGroup chore_group = chore_type.groups[i];
			if (!this.IsPermittedByTraits(chore_group))
			{
				flag2 = false;
			}
			if (this.IsPermittedByUser(chore_group))
			{
				flag = true;
			}
		}
		return flag && flag2;
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x00081119 File Offset: 0x0007F319
	public void SetReach(int reach)
	{
		this.stationaryReach = reach;
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x00081124 File Offset: 0x0007F324
	public bool GetNavigationCost(IApproachable approachable, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(approachable);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			if (this.consumerState.solidTransferArm.IsCellReachable(cell))
			{
				cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
				return true;
			}
		}
		cost = 0;
		return false;
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x00081190 File Offset: 0x0007F390
	public bool GetNavigationCost(int cell, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(cell);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(cell))
		{
			cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
			return true;
		}
		cost = 0;
		return false;
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x000811F4 File Offset: 0x0007F3F4
	public bool CanReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return this.navigator.CanReach(approachable);
		}
		if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			return this.consumerState.solidTransferArm.IsCellReachable(cell);
		}
		return false;
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x00081244 File Offset: 0x0007F444
	public bool IsWithinReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return !(this == null) && !(base.gameObject == null) && Grid.IsCellOffsetOf(Grid.PosToCell(this), approachable.GetCell(), approachable.GetOffsets());
		}
		return this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(approachable.GetCell());
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x000812B4 File Offset: 0x0007F4B4
	public void ShowHoverTextOnHoveredItem(Chore.Precondition.Context context, KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		if (context.chore.target.isNull || context.chore.target.gameObject != hover_obj.gameObject)
		{
			return;
		}
		drawer.NewLine(26);
		drawer.AddIndent(36);
		drawer.DrawText(context.chore.choreType.Name, hover_text_card.Styles_BodyText.Standard);
		if (!context.IsSuccess())
		{
			Chore.PreconditionInstance preconditionInstance = context.chore.GetPreconditions()[context.failedPreconditionId];
			string text = preconditionInstance.condition.description;
			if (string.IsNullOrEmpty(text))
			{
				text = preconditionInstance.condition.id;
			}
			if (context.chore.driver != null)
			{
				text = text.Replace("{Assignee}", context.chore.driver.GetProperName());
			}
			text = text.Replace("{Selected}", this.GetProperName());
			drawer.DrawText(" (" + text + ")", hover_text_card.Styles_BodyText.Standard);
		}
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x000813CC File Offset: 0x0007F5CC
	public void ShowHoverTextOnHoveredItem(KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		bool flag = false;
		foreach (Chore.Precondition.Context context in this.preconditionSnapshot.succeededContexts)
		{
			if (context.chore.showAvailabilityInHoverText && !context.chore.target.isNull && !(context.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context, hover_obj, drawer, hover_text_card);
			}
		}
		foreach (Chore.Precondition.Context context2 in this.preconditionSnapshot.failedContexts)
		{
			if (context2.chore.showAvailabilityInHoverText && !context2.chore.target.isNull && !(context2.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context2, hover_obj, drawer, hover_text_card);
			}
		}
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x00081568 File Offset: 0x0007F768
	public int GetPersonalPriority(ChoreType chore_type)
	{
		int num;
		if (!this.choreTypePriorities.TryGetValue(chore_type.IdHash, out num))
		{
			num = 3;
		}
		num = Mathf.Clamp(num, 0, 5);
		return num;
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x00081598 File Offset: 0x0007F798
	public int GetPersonalPriority(ChoreGroup group)
	{
		int value = 3;
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			value = priorityInfo.priority;
		}
		return Mathf.Clamp(value, 0, 5);
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x000815D0 File Offset: 0x0007F7D0
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
		if (group.choreTypes == null)
		{
			return;
		}
		value = Mathf.Clamp(value, 0, 5);
		ChoreConsumer.PriorityInfo priorityInfo;
		if (!this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			priorityInfo.priority = 3;
		}
		this.choreGroupPriorities[group.IdHash] = new ChoreConsumer.PriorityInfo
		{
			priority = value
		};
		this.UpdateChoreTypePriorities(group, value);
		this.SetPermittedByUser(group, value != 0);
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x00081642 File Offset: 0x0007F842
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return (int)this.GetAttributes().GetValue(group.attribute.Id);
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x0008165C File Offset: 0x0007F85C
	private void UpdateChoreTypePriorities(ChoreGroup group, int value)
	{
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		foreach (ChoreType choreType in group.choreTypes)
		{
			int num = 0;
			foreach (ChoreGroup choreGroup in choreGroups.resources)
			{
				if (choreGroup.choreTypes != null)
				{
					using (List<ChoreType>.Enumerator enumerator3 = choreGroup.choreTypes.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.IdHash == choreType.IdHash)
							{
								int personalPriority = this.GetPersonalPriority(choreGroup);
								num = Mathf.Max(num, personalPriority);
							}
						}
					}
				}
			}
			this.choreTypePriorities[choreType.IdHash] = num;
		}
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x00081774 File Offset: 0x0007F974
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x00081778 File Offset: 0x0007F978
	public bool RunBehaviourPrecondition(Tag tag)
	{
		ChoreConsumer.BehaviourPrecondition behaviourPrecondition = default(ChoreConsumer.BehaviourPrecondition);
		return this.behaviourPreconditions.TryGetValue(tag, out behaviourPrecondition) && behaviourPrecondition.cb(behaviourPrecondition.arg);
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x000817B0 File Offset: 0x0007F9B0
	public void AddBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		DebugUtil.Assert(!this.behaviourPreconditions.ContainsKey(tag));
		this.behaviourPreconditions[tag] = new ChoreConsumer.BehaviourPrecondition
		{
			cb = precondition,
			arg = arg
		};
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x000817F6 File Offset: 0x0007F9F6
	public void RemoveBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		this.behaviourPreconditions.Remove(tag);
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x00081808 File Offset: 0x0007FA08
	public bool IsChoreEqualOrAboveCurrentChorePriority<StateMachineType>()
	{
		Chore currentChore = this.choreDriver.GetCurrentChore();
		return currentChore == null || currentChore.choreType.priority <= this.choreTable.GetChorePriority<StateMachineType>(this);
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x00081844 File Offset: 0x0007FA44
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		bool result = false;
		Traits component = base.gameObject.GetComponent<Traits>();
		if (component != null && component.IsChoreGroupDisabled(chore_group))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x00081874 File Offset: 0x0007FA74
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> GetChoreGroupPriorities()
	{
		return this.choreGroupPriorities;
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x0008187C File Offset: 0x0007FA7C
	public void SetChoreGroupPriorities(Dictionary<HashedString, ChoreConsumer.PriorityInfo> priorities)
	{
		this.choreGroupPriorities = priorities;
	}

	// Token: 0x04000D5F RID: 3423
	public const int DEFAULT_PERSONAL_CHORE_PRIORITY = 3;

	// Token: 0x04000D60 RID: 3424
	public const int MIN_PERSONAL_PRIORITY = 0;

	// Token: 0x04000D61 RID: 3425
	public const int MAX_PERSONAL_PRIORITY = 5;

	// Token: 0x04000D62 RID: 3426
	public const int PRIORITY_DISABLED = 0;

	// Token: 0x04000D63 RID: 3427
	public const int PRIORITY_VERYLOW = 1;

	// Token: 0x04000D64 RID: 3428
	public const int PRIORITY_LOW = 2;

	// Token: 0x04000D65 RID: 3429
	public const int PRIORITY_FLAT = 3;

	// Token: 0x04000D66 RID: 3430
	public const int PRIORITY_HIGH = 4;

	// Token: 0x04000D67 RID: 3431
	public const int PRIORITY_VERYHIGH = 5;

	// Token: 0x04000D68 RID: 3432
	[MyCmpAdd]
	private ChoreProvider choreProvider;

	// Token: 0x04000D69 RID: 3433
	[MyCmpAdd]
	public ChoreDriver choreDriver;

	// Token: 0x04000D6A RID: 3434
	[MyCmpGet]
	public Navigator navigator;

	// Token: 0x04000D6B RID: 3435
	[MyCmpAdd]
	private User user;

	// Token: 0x04000D6C RID: 3436
	public bool prioritizeBrainIfNoChore;

	// Token: 0x04000D6D RID: 3437
	public System.Action choreRulesChanged;

	// Token: 0x04000D6E RID: 3438
	private List<ChoreProvider> providers = new List<ChoreProvider>();

	// Token: 0x04000D6F RID: 3439
	private List<Urge> urges = new List<Urge>();

	// Token: 0x04000D70 RID: 3440
	public ChoreTable choreTable;

	// Token: 0x04000D71 RID: 3441
	private ChoreTable.Instance choreTableInstance;

	// Token: 0x04000D72 RID: 3442
	public ChoreConsumerState consumerState;

	// Token: 0x04000D73 RID: 3443
	private Dictionary<Tag, ChoreConsumer.BehaviourPrecondition> behaviourPreconditions = new Dictionary<Tag, ChoreConsumer.BehaviourPrecondition>();

	// Token: 0x04000D74 RID: 3444
	private ChoreConsumer.PreconditionSnapshot preconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x04000D75 RID: 3445
	private ChoreConsumer.PreconditionSnapshot lastSuccessfulPreconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x04000D76 RID: 3446
	[Serialize]
	private Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x04000D77 RID: 3447
	private Dictionary<HashedString, int> choreTypePriorities = new Dictionary<HashedString, int>();

	// Token: 0x04000D78 RID: 3448
	private List<HashedString> traitDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04000D79 RID: 3449
	private List<HashedString> userDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04000D7A RID: 3450
	private int stationaryReach = -1;

	// Token: 0x0200121B RID: 4635
	private struct BehaviourPrecondition
	{
		// Token: 0x04006265 RID: 25189
		public Func<object, bool> cb;

		// Token: 0x04006266 RID: 25190
		public object arg;
	}

	// Token: 0x0200121C RID: 4636
	public class PreconditionSnapshot
	{
		// Token: 0x0600822C RID: 33324 RVA: 0x0031BF07 File Offset: 0x0031A107
		public void CopyTo(ChoreConsumer.PreconditionSnapshot snapshot)
		{
			snapshot.Clear();
			snapshot.succeededContexts.AddRange(this.succeededContexts);
			snapshot.failedContexts.AddRange(this.failedContexts);
			snapshot.doFailedContextsNeedSorting = true;
		}

		// Token: 0x0600822D RID: 33325 RVA: 0x0031BF38 File Offset: 0x0031A138
		public void Clear()
		{
			this.succeededContexts.Clear();
			this.failedContexts.Clear();
			this.doFailedContextsNeedSorting = true;
		}

		// Token: 0x04006267 RID: 25191
		public List<Chore.Precondition.Context> succeededContexts = new List<Chore.Precondition.Context>();

		// Token: 0x04006268 RID: 25192
		public List<Chore.Precondition.Context> failedContexts = new List<Chore.Precondition.Context>();

		// Token: 0x04006269 RID: 25193
		public bool doFailedContextsNeedSorting = true;
	}

	// Token: 0x0200121D RID: 4637
	public struct PriorityInfo
	{
		// Token: 0x0400626A RID: 25194
		public int priority;
	}
}
