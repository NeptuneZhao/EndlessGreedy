using System;
using STRINGS;

// Token: 0x02000B02 RID: 2818
public class ConditionHasResource : ProcessCondition
{
	// Token: 0x06005411 RID: 21521 RVA: 0x001E1C2F File Offset: 0x001DFE2F
	public ConditionHasResource(Storage storage, SimHashes resource, float thresholdMass)
	{
		this.storage = storage;
		this.resource = resource;
		this.thresholdMass = thresholdMass;
	}

	// Token: 0x06005412 RID: 21522 RVA: 0x001E1C4C File Offset: 0x001DFE4C
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.storage.GetAmountAvailable(this.resource.CreateTag()) < this.thresholdMass)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06005413 RID: 21523 RVA: 0x001E1C70 File Offset: 0x001DFE70
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.WARNING, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.FAILURE, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x06005414 RID: 21524 RVA: 0x001E1D20 File Offset: 0x001DFF20
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.WARNING, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.FAILURE, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x06005415 RID: 21525 RVA: 0x001E1DF8 File Offset: 0x001DFFF8
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400373A RID: 14138
	private Storage storage;

	// Token: 0x0400373B RID: 14139
	private SimHashes resource;

	// Token: 0x0400373C RID: 14140
	private float thresholdMass;
}
