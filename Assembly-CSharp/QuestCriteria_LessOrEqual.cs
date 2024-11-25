using System;
using System.Collections.Generic;

// Token: 0x02000A0C RID: 2572
public class QuestCriteria_LessOrEqual : QuestCriteria
{
	// Token: 0x06004AA5 RID: 19109 RVA: 0x001AB184 File Offset: 0x001A9384
	public QuestCriteria_LessOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004AA6 RID: 19110 RVA: 0x001AB193 File Offset: 0x001A9393
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current <= target;
	}
}
