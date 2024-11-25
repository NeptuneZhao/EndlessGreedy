using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B10 RID: 2832
public class TransferCargoCompleteCondition : ProcessCondition
{
	// Token: 0x06005458 RID: 21592 RVA: 0x001E2A23 File Offset: 0x001E0C23
	public TransferCargoCompleteCondition(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x06005459 RID: 21593 RVA: 0x001E2A34 File Offset: 0x001E0C34
	public override ProcessCondition.Status EvaluateCondition()
	{
		LaunchPad component = this.target.GetComponent<LaunchPad>();
		CraftModuleInterface craftModuleInterface;
		if (component == null)
		{
			craftModuleInterface = this.target.GetComponent<Clustercraft>().ModuleInterface;
		}
		else
		{
			RocketModuleCluster landedRocket = component.LandedRocket;
			if (landedRocket == null)
			{
				return ProcessCondition.Status.Ready;
			}
			craftModuleInterface = landedRocket.CraftInterface;
		}
		if (!craftModuleInterface.HasCargoModule)
		{
			return ProcessCondition.Status.Ready;
		}
		if (!this.target.HasTag(GameTags.TransferringCargoComplete))
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x0600545A RID: 21594 RVA: 0x001E2AA3 File Offset: 0x001E0CA3
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x001E2ABE File Offset: 0x001E0CBE
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x0600545C RID: 21596 RVA: 0x001E2AD9 File Offset: 0x001E0CD9
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003751 RID: 14161
	private GameObject target;
}
