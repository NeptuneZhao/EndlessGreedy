using System;
using STRINGS;

// Token: 0x02000B03 RID: 2819
public class ConditionNoExtraPassengers : ProcessCondition
{
	// Token: 0x06005416 RID: 21526 RVA: 0x001E1DFB File Offset: 0x001DFFFB
	public ConditionNoExtraPassengers(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06005417 RID: 21527 RVA: 0x001E1E0A File Offset: 0x001E000A
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.module.CheckExtraPassengers())
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005418 RID: 21528 RVA: 0x001E1E1C File Offset: 0x001E001C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.FAILURE;
	}

	// Token: 0x06005419 RID: 21529 RVA: 0x001E1E37 File Offset: 0x001E0037
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.FAILURE;
	}

	// Token: 0x0600541A RID: 21530 RVA: 0x001E1E52 File Offset: 0x001E0052
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400373D RID: 14141
	private PassengerRocketModule module;
}
