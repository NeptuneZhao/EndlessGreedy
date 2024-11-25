using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000A06 RID: 2566
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestInstance : ISaveLoadable
{
	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06004A74 RID: 19060 RVA: 0x001AA37A File Offset: 0x001A857A
	public HashedString Id
	{
		get
		{
			return this.quest.IdHash;
		}
	}

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06004A75 RID: 19061 RVA: 0x001AA387 File Offset: 0x001A8587
	public int CriteriaCount
	{
		get
		{
			return this.quest.Criteria.Length;
		}
	}

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06004A76 RID: 19062 RVA: 0x001AA396 File Offset: 0x001A8596
	public string Name
	{
		get
		{
			return this.quest.Name;
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06004A77 RID: 19063 RVA: 0x001AA3A3 File Offset: 0x001A85A3
	public string CompletionText
	{
		get
		{
			return this.quest.CompletionText;
		}
	}

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06004A78 RID: 19064 RVA: 0x001AA3B0 File Offset: 0x001A85B0
	public bool IsStarted
	{
		get
		{
			return this.currentState > Quest.State.NotStarted;
		}
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06004A79 RID: 19065 RVA: 0x001AA3BB File Offset: 0x001A85BB
	public bool IsComplete
	{
		get
		{
			return this.currentState == Quest.State.Completed;
		}
	}

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06004A7A RID: 19066 RVA: 0x001AA3C6 File Offset: 0x001A85C6
	// (set) Token: 0x06004A7B RID: 19067 RVA: 0x001AA3CE File Offset: 0x001A85CE
	public float CurrentProgress { get; private set; }

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06004A7C RID: 19068 RVA: 0x001AA3D7 File Offset: 0x001A85D7
	public Quest.State CurrentState
	{
		get
		{
			return this.currentState;
		}
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x001AA3E0 File Offset: 0x001A85E0
	public QuestInstance(Quest quest)
	{
		this.quest = quest;
		this.criteriaStates = new Dictionary<int, QuestInstance.CriteriaState>(quest.Criteria.Length);
		for (int i = 0; i < quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = quest.Criteria[i];
			QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
			{
				Handle = i
			};
			if (questCriteria.TargetValues != null)
			{
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
				{
					value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
				if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
				{
					value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
				}
			}
			this.criteriaStates[questCriteria.CriteriaId.GetHash()] = value;
		}
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x001AA4B0 File Offset: 0x001A86B0
	public void Initialize(Quest quest)
	{
		this.quest = quest;
		this.ValidateCriteriasOnLoad();
		this.UpdateQuestProgress(false);
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x001AA4C6 File Offset: 0x001A86C6
	public bool HasCriteria(HashedString criteriaId)
	{
		return this.criteriaStates.ContainsKey(criteriaId.HashValue);
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x001AA4DC File Offset: 0x001A86DC
	public bool HasBehavior(QuestCriteria.BehaviorFlags behavior)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < this.quest.Criteria.Length)
		{
			flag = ((this.quest.Criteria[num].EvaluationBehaviors & behavior) > QuestCriteria.BehaviorFlags.None);
			num++;
		}
		return flag;
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x001AA520 File Offset: 0x001A8720
	public int GetTargetCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return this.quest.Criteria[criteriaState.Handle].RequiredCount;
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x001AA55C File Offset: 0x001A875C
	public int GetCurrentCount(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return 0;
		}
		return criteriaState.CurrentCount;
	}

	// Token: 0x06004A83 RID: 19075 RVA: 0x001AA588 File Offset: 0x001A8788
	public float GetCurrentValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.CurrentValues == null)
		{
			return float.NaN;
		}
		return criteriaState.CurrentValues[valueHandle];
	}

	// Token: 0x06004A84 RID: 19076 RVA: 0x001AA5C4 File Offset: 0x001A87C4
	public float GetTargetValue(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (this.quest.Criteria[criteriaState.Handle].TargetValues == null)
		{
			return float.NaN;
		}
		return this.quest.Criteria[criteriaState.Handle].TargetValues[valueHandle];
	}

	// Token: 0x06004A85 RID: 19077 RVA: 0x001AA628 File Offset: 0x001A8828
	public Tag GetSatisfyingItem(HashedString criteriaId, int valueHandle = 0)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return default(Tag);
		}
		return criteriaState.SatisfyingItems[valueHandle];
	}

	// Token: 0x06004A86 RID: 19078 RVA: 0x001AA66C File Offset: 0x001A886C
	public float GetAreaAverage(HashedString criteriaId)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return float.NaN;
		}
		if (!QuestCriteria.HasBehavior(this.quest.Criteria[criteriaState.Handle].EvaluationBehaviors, (QuestCriteria.BehaviorFlags)5))
		{
			return float.NaN;
		}
		float num = 0f;
		for (int i = 0; i < criteriaState.CurrentValues.Length; i++)
		{
			num += criteriaState.CurrentValues[i];
		}
		return num / (float)criteriaState.CurrentValues.Length;
	}

	// Token: 0x06004A87 RID: 19079 RVA: 0x001AA6EC File Offset: 0x001A88EC
	public bool IsItemRedundant(HashedString criteriaId, Tag item)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState) || criteriaState.SatisfyingItems == null)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		while (!flag && num < criteriaState.SatisfyingItems.Length)
		{
			flag = (criteriaState.SatisfyingItems[num] == item);
			num++;
		}
		return flag;
	}

	// Token: 0x06004A88 RID: 19080 RVA: 0x001AA748 File Offset: 0x001A8948
	public bool IsCriteriaSatisfied(HashedString id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.HashValue, out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x06004A89 RID: 19081 RVA: 0x001AA794 File Offset: 0x001A8994
	public bool IsCriteriaSatisfied(Tag id)
	{
		QuestInstance.CriteriaState criteriaState;
		return this.criteriaStates.TryGetValue(id.GetHash(), out criteriaState) && this.quest.Criteria[criteriaState.Handle].IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
	}

	// Token: 0x06004A8A RID: 19082 RVA: 0x001AA7E0 File Offset: 0x001A89E0
	public void TrackAreaForCriteria(HashedString criteriaId, Extents area)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(criteriaId.HashValue, out criteriaState))
		{
			return;
		}
		int num = area.width * area.height;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		global::Debug.Assert(num <= 32);
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues = new float[num];
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems))
		{
			criteriaState.SatisfyingItems = new Tag[num];
		}
		this.criteriaStates[criteriaId.HashValue] = criteriaState;
	}

	// Token: 0x06004A8B RID: 19083 RVA: 0x001AA87C File Offset: 0x001A8A7C
	private uint GetSatisfactionMask(QuestInstance.CriteriaState state)
	{
		QuestCriteria questCriteria = this.quest.Criteria[state.Handle];
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			int num = 0;
			if (state.SatisfyingItems != null)
			{
				num = state.SatisfyingItems.Length;
			}
			else if (state.CurrentValues != null)
			{
				num = state.CurrentValues.Length;
			}
			return (uint)(Mathf.Pow(2f, (float)num) - 1f);
		}
		return questCriteria.GetSatisfactionMask();
	}

	// Token: 0x06004A8C RID: 19084 RVA: 0x001AA8EC File Offset: 0x001A8AEC
	public int TrackProgress(Quest.ItemData data, out bool dataSatisfies, out bool itemIsRedundant)
	{
		dataSatisfies = false;
		itemIsRedundant = false;
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return -1;
		}
		int valueHandle = data.ValueHandle;
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		dataSatisfies = this.DataSatisfiesCriteria(data, ref valueHandle);
		if (valueHandle == -1)
		{
			return valueHandle;
		}
		bool flag = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.AllowsRegression);
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackItems);
		Tag tag = flag2 ? criteriaState.SatisfyingItems[valueHandle] : default(Tag);
		if (dataSatisfies)
		{
			itemIsRedundant = (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.UniqueItems) && this.IsItemRedundant(data.CriteriaId, data.SatisfyingItem));
			if (itemIsRedundant)
			{
				return valueHandle;
			}
			tag = data.SatisfyingItem;
			criteriaState.SatisfactionState |= questCriteria.GetValueMask(valueHandle);
		}
		else if (flag)
		{
			criteriaState.SatisfactionState &= ~questCriteria.GetValueMask(valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
		{
			criteriaState.CurrentValues[valueHandle] = data.CurrentValue;
		}
		if (flag2)
		{
			criteriaState.SatisfyingItems[valueHandle] = tag;
		}
		bool flag3 = this.IsCriteriaSatisfied(data.CriteriaId);
		bool flag4 = questCriteria.IsSatisfied(criteriaState.SatisfactionState, this.GetSatisfactionMask(criteriaState));
		if (flag3 != flag4)
		{
			criteriaState.CurrentCount += (flag3 ? -1 : 1);
			if (flag4 && criteriaState.CurrentCount < questCriteria.RequiredCount)
			{
				criteriaState.SatisfactionState = 0U;
			}
		}
		this.criteriaStates[data.CriteriaId.HashValue] = criteriaState;
		this.UpdateQuestProgress(true);
		return valueHandle;
	}

	// Token: 0x06004A8D RID: 19085 RVA: 0x001AAA88 File Offset: 0x001A8C88
	public bool DataSatisfiesCriteria(Quest.ItemData data, ref int valueHandle)
	{
		QuestInstance.CriteriaState criteriaState;
		if (!this.criteriaStates.TryGetValue(data.CriteriaId.HashValue, out criteriaState))
		{
			return false;
		}
		QuestCriteria questCriteria = this.quest.Criteria[criteriaState.Handle];
		bool flag = questCriteria.AcceptedTags == null || (data.QualifyingTag.IsValid && questCriteria.AcceptedTags.Contains(data.QualifyingTag));
		if (flag && questCriteria.TargetValues == null)
		{
			valueHandle = 0;
		}
		if (!flag || valueHandle != -1)
		{
			return flag && questCriteria.ValueSatisfies(data.CurrentValue, valueHandle);
		}
		if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle = data.LocalCellId;
		}
		int num = -1;
		bool flag2 = QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues);
		bool flag3 = false;
		int num2 = 0;
		while (!flag3 && num2 < questCriteria.TargetValues.Length)
		{
			if (questCriteria.ValueSatisfies(data.CurrentValue, num2))
			{
				flag3 = true;
				num = num2;
				break;
			}
			if (flag2 && (num == -1 || criteriaState.CurrentValues[num] > criteriaState.CurrentValues[num2]))
			{
				num = num2;
			}
			num2++;
		}
		if (valueHandle == -1 && num != -1)
		{
			valueHandle = questCriteria.RequiredCount * num + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
		}
		return flag3;
	}

	// Token: 0x06004A8E RID: 19086 RVA: 0x001AABC0 File Offset: 0x001A8DC0
	private void UpdateQuestProgress(bool startQuest = false)
	{
		if (!this.IsStarted && !startQuest)
		{
			return;
		}
		float currentProgress = this.CurrentProgress;
		Quest.State state = this.currentState;
		this.currentState = Quest.State.InProgress;
		this.CurrentProgress = 0f;
		float num = 0f;
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria questCriteria = this.quest.Criteria[i];
			QuestInstance.CriteriaState criteriaState = this.criteriaStates[questCriteria.CriteriaId.GetHash()];
			float num2 = (float)((questCriteria.TargetValues != null) ? questCriteria.TargetValues.Length : 1);
			num += (float)questCriteria.RequiredCount;
			this.CurrentProgress += (float)criteriaState.CurrentCount;
			if (!this.IsCriteriaSatisfied(questCriteria.CriteriaId))
			{
				float num3 = 0f;
				int num4 = 0;
				while (questCriteria.TargetValues != null && (float)num4 < num2)
				{
					if ((criteriaState.SatisfactionState & questCriteria.GetValueMask(num4)) == 0U)
					{
						if (QuestCriteria.HasBehavior(questCriteria.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackValues))
						{
							int num5 = questCriteria.RequiredCount * num4 + Mathf.Min(criteriaState.CurrentCount, questCriteria.RequiredCount - 1);
							num3 += Mathf.Max(0f, criteriaState.CurrentValues[num5] / questCriteria.TargetValues[num4]);
						}
					}
					else
					{
						num3 += 1f;
					}
					num4++;
				}
				this.CurrentProgress += num3 / num2;
			}
		}
		this.CurrentProgress = Mathf.Clamp01(this.CurrentProgress / num);
		if (this.CurrentProgress == 1f)
		{
			this.currentState = Quest.State.Completed;
		}
		float num6 = this.CurrentProgress - currentProgress;
		if (state != this.currentState || Mathf.Abs(num6) > Mathf.Epsilon)
		{
			Action<QuestInstance, Quest.State, float> questProgressChanged = this.QuestProgressChanged;
			if (questProgressChanged == null)
			{
				return;
			}
			questProgressChanged(this, state, num6);
		}
	}

	// Token: 0x06004A8F RID: 19087 RVA: 0x001AAD9C File Offset: 0x001A8F9C
	public ICheckboxListGroupControl.CheckboxItem[] GetCheckBoxData(Func<int, string, QuestInstance, string> resolveToolTip = null)
	{
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[this.quest.Criteria.Length];
		for (int i = 0; i < this.quest.Criteria.Length; i++)
		{
			QuestCriteria c = this.quest.Criteria[i];
			array[i] = new ICheckboxListGroupControl.CheckboxItem
			{
				text = c.Text,
				isOn = this.IsCriteriaSatisfied(c.CriteriaId),
				tooltip = c.Tooltip
			};
			if (resolveToolTip != null)
			{
				array[i].resolveTooltipCallback = ((string tooltip, object owner) => resolveToolTip(c.CriteriaId.GetHash(), c.Tooltip, this));
			}
		}
		return array;
	}

	// Token: 0x06004A90 RID: 19088 RVA: 0x001AAE84 File Offset: 0x001A9084
	public void ValidateCriteriasOnLoad()
	{
		if (this.criteriaStates.Count != this.quest.Criteria.Length)
		{
			Dictionary<int, QuestInstance.CriteriaState> dictionary = new Dictionary<int, QuestInstance.CriteriaState>(this.quest.Criteria.Length);
			for (int i = 0; i < this.quest.Criteria.Length; i++)
			{
				QuestCriteria questCriteria = this.quest.Criteria[i];
				int hash = questCriteria.CriteriaId.GetHash();
				if (this.criteriaStates.ContainsKey(hash))
				{
					dictionary[hash] = this.criteriaStates[hash];
				}
				else
				{
					QuestInstance.CriteriaState value = new QuestInstance.CriteriaState
					{
						Handle = i
					};
					if (questCriteria.TargetValues != null)
					{
						if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackItems) == QuestCriteria.BehaviorFlags.TrackItems)
						{
							value.SatisfyingItems = new Tag[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
						}
						if ((questCriteria.EvaluationBehaviors & QuestCriteria.BehaviorFlags.TrackValues) == QuestCriteria.BehaviorFlags.TrackValues)
						{
							value.CurrentValues = new float[questCriteria.TargetValues.Length * questCriteria.RequiredCount];
						}
					}
					dictionary[hash] = value;
				}
			}
			this.criteriaStates = dictionary;
		}
	}

	// Token: 0x040030DD RID: 12509
	public Action<QuestInstance, Quest.State, float> QuestProgressChanged;

	// Token: 0x040030DF RID: 12511
	private Quest quest;

	// Token: 0x040030E0 RID: 12512
	[Serialize]
	private Dictionary<int, QuestInstance.CriteriaState> criteriaStates;

	// Token: 0x040030E1 RID: 12513
	[Serialize]
	private Quest.State currentState;

	// Token: 0x02001A28 RID: 6696
	private struct CriteriaState
	{
		// Token: 0x06009F40 RID: 40768 RVA: 0x0037BBBC File Offset: 0x00379DBC
		public static bool ItemAlreadySatisfying(QuestInstance.CriteriaState state, Tag item)
		{
			bool result = false;
			int num = 0;
			while (state.SatisfyingItems != null && num < state.SatisfyingItems.Length)
			{
				if (state.SatisfyingItems[num] == item)
				{
					result = true;
					break;
				}
				num++;
			}
			return result;
		}

		// Token: 0x04007B98 RID: 31640
		public int Handle;

		// Token: 0x04007B99 RID: 31641
		public int CurrentCount;

		// Token: 0x04007B9A RID: 31642
		public uint SatisfactionState;

		// Token: 0x04007B9B RID: 31643
		public Tag[] SatisfyingItems;

		// Token: 0x04007B9C RID: 31644
		public float[] CurrentValues;
	}
}
