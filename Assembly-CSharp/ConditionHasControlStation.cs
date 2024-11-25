using System;
using STRINGS;

// Token: 0x02000AFE RID: 2814
public class ConditionHasControlStation : ProcessCondition
{
	// Token: 0x060053FC RID: 21500 RVA: 0x001E15B0 File Offset: 0x001DF7B0
	public ConditionHasControlStation(RocketModuleCluster module)
	{
		this.module = module;
	}

	// Token: 0x060053FD RID: 21501 RVA: 0x001E15BF File Offset: 0x001DF7BF
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (Components.RocketControlStations.GetWorldItems(this.module.CraftInterface.GetComponent<WorldContainer>().id, false).Count <= 0)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x001E15EC File Offset: 0x001DF7EC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.FAILURE;
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x001E1607 File Offset: 0x001DF807
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.FAILURE;
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x001E1622 File Offset: 0x001DF822
	public override bool ShowInUI()
	{
		return this.EvaluateCondition() == ProcessCondition.Status.Failure;
	}

	// Token: 0x04003736 RID: 14134
	private RocketModuleCluster module;
}
