using System;
using STRINGS;

// Token: 0x02000B09 RID: 2825
public class ConditionRocketHeight : ProcessCondition
{
	// Token: 0x06005435 RID: 21557 RVA: 0x001E24F1 File Offset: 0x001E06F1
	public ConditionRocketHeight(RocketEngineCluster engine)
	{
		this.engine = engine;
	}

	// Token: 0x06005436 RID: 21558 RVA: 0x001E2500 File Offset: 0x001E0700
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.engine.maxHeight < this.engine.GetComponent<RocketModuleCluster>().CraftInterface.RocketHeight)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005437 RID: 21559 RVA: 0x001E2528 File Offset: 0x001E0728
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005438 RID: 21560 RVA: 0x001E2568 File Offset: 0x001E0768
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005439 RID: 21561 RVA: 0x001E25A8 File Offset: 0x001E07A8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003746 RID: 14150
	private RocketEngineCluster engine;
}
