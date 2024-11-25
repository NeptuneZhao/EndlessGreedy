using System;
using System.Collections.Generic;

// Token: 0x02000ACD RID: 2765
public class LaunchPadConditions : KMonoBehaviour, IProcessConditionSet
{
	// Token: 0x06005230 RID: 21040 RVA: 0x001D7D80 File Offset: 0x001D5F80
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (conditionType != ProcessCondition.ProcessConditionType.RocketStorage)
		{
			return null;
		}
		return this.conditions;
	}

	// Token: 0x06005231 RID: 21041 RVA: 0x001D7D8E File Offset: 0x001D5F8E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.conditions = new List<ProcessCondition>();
		this.conditions.Add(new TransferCargoCompleteCondition(base.gameObject));
	}

	// Token: 0x0400363B RID: 13883
	private List<ProcessCondition> conditions;
}
