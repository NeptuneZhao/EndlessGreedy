using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A08 RID: 2568
public class QuestCriteria_Equals : QuestCriteria
{
	// Token: 0x06004A9D RID: 19101 RVA: 0x001AB11F File Offset: 0x001A931F
	public QuestCriteria_Equals(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004A9E RID: 19102 RVA: 0x001AB12E File Offset: 0x001A932E
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return Mathf.Abs(target - current) <= Mathf.Epsilon;
	}
}
