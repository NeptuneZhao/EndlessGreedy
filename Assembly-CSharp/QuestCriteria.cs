using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A07 RID: 2567
public class QuestCriteria
{
	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06004A91 RID: 19089 RVA: 0x001AAF98 File Offset: 0x001A9198
	// (set) Token: 0x06004A92 RID: 19090 RVA: 0x001AAFA0 File Offset: 0x001A91A0
	public string Text { get; private set; }

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06004A93 RID: 19091 RVA: 0x001AAFA9 File Offset: 0x001A91A9
	// (set) Token: 0x06004A94 RID: 19092 RVA: 0x001AAFB1 File Offset: 0x001A91B1
	public string Tooltip { get; private set; }

	// Token: 0x06004A95 RID: 19093 RVA: 0x001AAFBC File Offset: 0x001A91BC
	public QuestCriteria(Tag id, float[] targetValues = null, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.None)
	{
		global::Debug.Assert(targetValues == null || (targetValues.Length != 0 && targetValues.Length <= 32));
		this.CriteriaId = id;
		this.EvaluationBehaviors = flags;
		this.TargetValues = targetValues;
		this.AcceptedTags = acceptedTags;
		this.RequiredCount = requiredCount;
	}

	// Token: 0x06004A96 RID: 19094 RVA: 0x001AB018 File Offset: 0x001A9218
	public bool ValueSatisfies(float value, int valueHandle)
	{
		if (float.IsNaN(value))
		{
			return false;
		}
		float target = (this.TargetValues == null) ? 0f : this.TargetValues[valueHandle];
		return this.ValueSatisfies_Internal(value, target);
	}

	// Token: 0x06004A97 RID: 19095 RVA: 0x001AB04F File Offset: 0x001A924F
	protected virtual bool ValueSatisfies_Internal(float current, float target)
	{
		return true;
	}

	// Token: 0x06004A98 RID: 19096 RVA: 0x001AB052 File Offset: 0x001A9252
	public bool IsSatisfied(uint satisfactionState, uint satisfactionMask)
	{
		return (satisfactionState & satisfactionMask) == satisfactionMask;
	}

	// Token: 0x06004A99 RID: 19097 RVA: 0x001AB05C File Offset: 0x001A925C
	public void PopulateStrings(string prefix)
	{
		string str = this.CriteriaId.Name.ToUpperInvariant();
		StringEntry stringEntry;
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".NAME", out stringEntry))
		{
			this.Text = stringEntry.String;
		}
		if (Strings.TryGet(prefix + "CRITERIA." + str + ".TOOLTIP", out stringEntry))
		{
			this.Tooltip = stringEntry.String;
		}
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x001AB0C9 File Offset: 0x001A92C9
	public uint GetSatisfactionMask()
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		return (uint)Mathf.Pow(2f, (float)(this.TargetValues.Length - 1));
	}

	// Token: 0x06004A9B RID: 19099 RVA: 0x001AB0EB File Offset: 0x001A92EB
	public uint GetValueMask(int valueHandle)
	{
		if (this.TargetValues == null)
		{
			return 1U;
		}
		if (!QuestCriteria.HasBehavior(this.EvaluationBehaviors, QuestCriteria.BehaviorFlags.TrackArea))
		{
			valueHandle %= this.TargetValues.Length;
		}
		return 1U << valueHandle;
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x001AB117 File Offset: 0x001A9317
	public static bool HasBehavior(QuestCriteria.BehaviorFlags flags, QuestCriteria.BehaviorFlags behavior)
	{
		return (flags & behavior) == behavior;
	}

	// Token: 0x040030E2 RID: 12514
	public const int MAX_VALUES = 32;

	// Token: 0x040030E3 RID: 12515
	public const int INVALID_VALUE = -1;

	// Token: 0x040030E4 RID: 12516
	public readonly Tag CriteriaId;

	// Token: 0x040030E5 RID: 12517
	public readonly QuestCriteria.BehaviorFlags EvaluationBehaviors;

	// Token: 0x040030E6 RID: 12518
	public readonly float[] TargetValues;

	// Token: 0x040030E7 RID: 12519
	public readonly int RequiredCount = 1;

	// Token: 0x040030E8 RID: 12520
	public readonly HashSet<Tag> AcceptedTags;

	// Token: 0x02001A2B RID: 6699
	public enum BehaviorFlags
	{
		// Token: 0x04007BA2 RID: 31650
		None,
		// Token: 0x04007BA3 RID: 31651
		TrackArea,
		// Token: 0x04007BA4 RID: 31652
		AllowsRegression,
		// Token: 0x04007BA5 RID: 31653
		TrackValues = 4,
		// Token: 0x04007BA6 RID: 31654
		TrackItems = 8,
		// Token: 0x04007BA7 RID: 31655
		UniqueItems = 24
	}
}
