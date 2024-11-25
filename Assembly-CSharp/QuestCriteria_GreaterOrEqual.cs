using System;
using System.Collections.Generic;

// Token: 0x02000A0B RID: 2571
public class QuestCriteria_GreaterOrEqual : QuestCriteria
{
	// Token: 0x06004AA3 RID: 19107 RVA: 0x001AB16C File Offset: 0x001A936C
	public QuestCriteria_GreaterOrEqual(Tag id, float[] targetValues, int requiredCount = 1, HashSet<Tag> acceptedTags = null, QuestCriteria.BehaviorFlags flags = QuestCriteria.BehaviorFlags.TrackValues) : base(id, targetValues, requiredCount, acceptedTags, flags)
	{
	}

	// Token: 0x06004AA4 RID: 19108 RVA: 0x001AB17B File Offset: 0x001A937B
	protected override bool ValueSatisfies_Internal(float current, float target)
	{
		return current >= target;
	}
}
