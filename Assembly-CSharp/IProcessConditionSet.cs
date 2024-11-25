using System;
using System.Collections.Generic;

// Token: 0x02000D5C RID: 3420
public interface IProcessConditionSet
{
	// Token: 0x06006BC6 RID: 27590
	List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType);
}
