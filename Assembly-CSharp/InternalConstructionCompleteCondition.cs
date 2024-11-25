using System;
using STRINGS;

// Token: 0x02000B0C RID: 2828
public class InternalConstructionCompleteCondition : ProcessCondition
{
	// Token: 0x06005444 RID: 21572 RVA: 0x001E274B File Offset: 0x001E094B
	public InternalConstructionCompleteCondition(BuildingInternalConstructor.Instance target)
	{
		this.target = target;
	}

	// Token: 0x06005445 RID: 21573 RVA: 0x001E275A File Offset: 0x001E095A
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.target.IsRequestingConstruction() && !this.target.HasOutputInStorage())
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005446 RID: 21574 RVA: 0x001E2779 File Offset: 0x001E0979
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.FAILURE;
	}

	// Token: 0x06005447 RID: 21575 RVA: 0x001E2790 File Offset: 0x001E0990
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
	}

	// Token: 0x06005448 RID: 21576 RVA: 0x001E27A7 File Offset: 0x001E09A7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003749 RID: 14153
	private BuildingInternalConstructor.Instance target;
}
