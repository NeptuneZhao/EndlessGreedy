using System;
using STRINGS;

// Token: 0x02000B04 RID: 2820
public class ConditionOnLaunchPad : ProcessCondition
{
	// Token: 0x0600541B RID: 21531 RVA: 0x001E1E55 File Offset: 0x001E0055
	public ConditionOnLaunchPad(CraftModuleInterface craftInterface)
	{
		this.craftInterface = craftInterface;
	}

	// Token: 0x0600541C RID: 21532 RVA: 0x001E1E64 File Offset: 0x001E0064
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!(this.craftInterface.CurrentPad != null))
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x0600541D RID: 21533 RVA: 0x001E1E7C File Offset: 0x001E007C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x0600541E RID: 21534 RVA: 0x001E1EBC File Offset: 0x001E00BC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x001E1EFC File Offset: 0x001E00FC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400373E RID: 14142
	private CraftModuleInterface craftInterface;
}
