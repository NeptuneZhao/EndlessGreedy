using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000AFD RID: 2813
public class ConditionHasCargoBayForNoseconeHarvest : ProcessCondition
{
	// Token: 0x060053F6 RID: 21494 RVA: 0x001E1425 File Offset: 0x001DF625
	public ConditionHasCargoBayForNoseconeHarvest(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x001E1434 File Offset: 0x001DF634
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.HasHarvestNosecone())
		{
			return ProcessCondition.Status.Ready;
		}
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().GetComponent<CargoBayCluster>())
				{
					return ProcessCondition.Status.Ready;
				}
			}
		}
		return ProcessCondition.Status.Warning;
	}

	// Token: 0x060053F8 RID: 21496 RVA: 0x001E14A0 File Offset: 0x001DF6A0
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result = "";
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.FAILURE;
			break;
		case ProcessCondition.Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.WARNING;
			break;
		case ProcessCondition.Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.READY;
			break;
		}
		return result;
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x001E14F0 File Offset: 0x001DF6F0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result = "";
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.FAILURE;
			break;
		case ProcessCondition.Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.WARNING;
			break;
		case ProcessCondition.Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.READY;
			break;
		}
		return result;
	}

	// Token: 0x060053FA RID: 21498 RVA: 0x001E153D File Offset: 0x001DF73D
	public override bool ShowInUI()
	{
		return this.HasHarvestNosecone();
	}

	// Token: 0x060053FB RID: 21499 RVA: 0x001E1548 File Offset: 0x001DF748
	private bool HasHarvestNosecone()
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().HasTag("NoseconeHarvest"))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04003735 RID: 14133
	private LaunchableRocketCluster launchable;
}
