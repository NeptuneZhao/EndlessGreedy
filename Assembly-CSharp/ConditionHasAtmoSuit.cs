using System;
using STRINGS;

// Token: 0x02000AFC RID: 2812
public class ConditionHasAtmoSuit : ProcessCondition
{
	// Token: 0x060053F1 RID: 21489 RVA: 0x001E1350 File Offset: 0x001DF550
	public ConditionHasAtmoSuit(CommandModule module)
	{
		this.module = module;
		ManualDeliveryKG manualDeliveryKG = this.module.FindOrAdd<ManualDeliveryKG>();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.SetStorage(module.storage);
		manualDeliveryKG.RequestedItemTag = GameTags.AtmoSuit;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.refillMass = 0.1f;
		manualDeliveryKG.capacity = 1f;
	}

	// Token: 0x060053F2 RID: 21490 RVA: 0x001E13C6 File Offset: 0x001DF5C6
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.AtmoSuit) < 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060053F3 RID: 21491 RVA: 0x001E13EC File Offset: 0x001DF5EC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.NAME;
		}
		return UI.STARMAP.NOSUIT.NAME;
	}

	// Token: 0x060053F4 RID: 21492 RVA: 0x001E1407 File Offset: 0x001DF607
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.TOOLTIP;
		}
		return UI.STARMAP.NOSUIT.TOOLTIP;
	}

	// Token: 0x060053F5 RID: 21493 RVA: 0x001E1422 File Offset: 0x001DF622
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003734 RID: 14132
	private CommandModule module;
}
