using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000B01 RID: 2817
public class ConditionHasNosecone : ProcessCondition
{
	// Token: 0x0600540C RID: 21516 RVA: 0x001E1B37 File Offset: 0x001DFD37
	public ConditionHasNosecone(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x0600540D RID: 21517 RVA: 0x001E1B48 File Offset: 0x001DFD48
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().HasTag(GameTags.NoseRocketModule))
				{
					return ProcessCondition.Status.Ready;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x0600540E RID: 21518 RVA: 0x001E1BAC File Offset: 0x001DFDAC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x0600540F RID: 21519 RVA: 0x001E1BEC File Offset: 0x001DFDEC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005410 RID: 21520 RVA: 0x001E1C2C File Offset: 0x001DFE2C
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003739 RID: 14137
	private LaunchableRocketCluster launchable;
}
