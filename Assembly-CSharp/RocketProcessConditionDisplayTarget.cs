using System;
using System.Collections.Generic;

// Token: 0x02000B0F RID: 2831
public class RocketProcessConditionDisplayTarget : KMonoBehaviour, IProcessConditionSet, ISim1000ms
{
	// Token: 0x06005455 RID: 21589 RVA: 0x001E292E File Offset: 0x001E0B2E
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (this.craftModuleInterface == null)
		{
			this.craftModuleInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		}
		return this.craftModuleInterface.GetConditionSet(conditionType);
	}

	// Token: 0x06005456 RID: 21590 RVA: 0x001E295C File Offset: 0x001E0B5C
	public void Sim1000ms(float dt)
	{
		bool flag = false;
		using (List<ProcessCondition>.Enumerator enumerator = this.GetConditionSet(ProcessCondition.ProcessConditionType.All).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
				{
					flag = true;
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RocketChecklistIncomplete, null);
						break;
					}
					break;
				}
			}
		}
		if (!flag && this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x0400374F RID: 14159
	private CraftModuleInterface craftModuleInterface;

	// Token: 0x04003750 RID: 14160
	private Guid statusHandle = Guid.Empty;
}
