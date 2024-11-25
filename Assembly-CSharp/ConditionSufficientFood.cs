using System;
using STRINGS;

// Token: 0x02000B0A RID: 2826
public class ConditionSufficientFood : ProcessCondition
{
	// Token: 0x0600543A RID: 21562 RVA: 0x001E25AB File Offset: 0x001E07AB
	public ConditionSufficientFood(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x0600543B RID: 21563 RVA: 0x001E25BA File Offset: 0x001E07BA
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.Edible) <= 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x0600543C RID: 21564 RVA: 0x001E25DD File Offset: 0x001E07DD
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.NAME;
		}
		return UI.STARMAP.NOFOOD.NAME;
	}

	// Token: 0x0600543D RID: 21565 RVA: 0x001E25F8 File Offset: 0x001E07F8
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.TOOLTIP;
		}
		return UI.STARMAP.NOFOOD.TOOLTIP;
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x001E2613 File Offset: 0x001E0813
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003747 RID: 14151
	private CommandModule module;
}
