using System;
using System.Collections.Generic;

// Token: 0x02000A0A RID: 2570
public class QuestCriteria_LessThan : QuestCriteria
{
	// Token: 0x06004AA1 RID: 19105 RVA: 0x001AB157 File Offset: 0x001A9357
	public QuestCriteria_LessThan(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004AA2 RID: 19106 RVA: 0x001AB166 File Offset: 0x001A9366
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current < target;
	}
}
