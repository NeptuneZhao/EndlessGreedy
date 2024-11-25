using System;
using STRINGS;

// Token: 0x02000B0D RID: 2829
public class LoadingCompleteCondition : ProcessCondition
{
	// Token: 0x06005449 RID: 21577 RVA: 0x001E27AA File Offset: 0x001E09AA
	public LoadingCompleteCondition(Storage target)
	{
		this.target = target;
		this.userControlledTarget = target.GetComponent<IUserControlledCapacity>();
	}

	// Token: 0x0600544A RID: 21578 RVA: 0x001E27C5 File Offset: 0x001E09C5
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.userControlledTarget != null)
		{
			if (this.userControlledTarget.AmountStored < this.userControlledTarget.UserMaxCapacity)
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
		else
		{
			if (!this.target.IsFull())
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
	}

	// Token: 0x0600544B RID: 21579 RVA: 0x001E27FB File Offset: 0x001E09FB
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x0600544C RID: 21580 RVA: 0x001E2812 File Offset: 0x001E0A12
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x0600544D RID: 21581 RVA: 0x001E2829 File Offset: 0x001E0A29
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400374A RID: 14154
	private Storage target;

	// Token: 0x0400374B RID: 14155
	private IUserControlledCapacity userControlledTarget;
}
