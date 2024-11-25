using System;
using System.Collections.Generic;

// Token: 0x02000A09 RID: 2569
public class QuestCriteria_GreaterThan : QuestCriteria
{
	// Token: 0x06004A9F RID: 19103 RVA: 0x001AB142 File Offset: 0x001A9342
	public QuestCriteria_GreaterThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004AA0 RID: 19104 RVA: 0x001AB151 File Offset: 0x001A9351
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current > target;
	}
}
