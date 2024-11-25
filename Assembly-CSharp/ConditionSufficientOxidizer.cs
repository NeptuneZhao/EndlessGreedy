using System;
using STRINGS;

// Token: 0x02000B0B RID: 2827
public class ConditionSufficientOxidizer : ProcessCondition
{
	// Token: 0x0600543F RID: 21567 RVA: 0x001E2616 File Offset: 0x001E0816
	public ConditionSufficientOxidizer(OxidizerTank oxidizerTank)
	{
		this.oxidizerTank = oxidizerTank;
	}

	// Token: 0x06005440 RID: 21568 RVA: 0x001E2628 File Offset: 0x001E0828
	public override ProcessCondition.Status EvaluateCondition()
	{
		RocketModuleCluster component = this.oxidizerTank.GetComponent<RocketModuleCluster>();
		if (component != null && component.CraftInterface != null)
		{
			Clustercraft component2 = component.CraftInterface.GetComponent<Clustercraft>();
			ClusterTraveler component3 = component.CraftInterface.GetComponent<ClusterTraveler>();
			if (component2 == null || component3 == null || component3.CurrentPath == null)
			{
				return ProcessCondition.Status.Failure;
			}
			int num = component3.RemainingTravelNodes();
			if (num == 0)
			{
				if (!component2.HasResourcesToMove(1, Clustercraft.CombustionResource.Oxidizer))
				{
					return ProcessCondition.Status.Failure;
				}
				return ProcessCondition.Status.Ready;
			}
			else
			{
				bool flag = component2.HasResourcesToMove(num * 2, Clustercraft.CombustionResource.Oxidizer);
				bool flag2 = component2.HasResourcesToMove(num, Clustercraft.CombustionResource.Oxidizer);
				if (flag)
				{
					return ProcessCondition.Status.Ready;
				}
				if (flag2)
				{
					return ProcessCondition.Status.Warning;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06005441 RID: 21569 RVA: 0x001E26C8 File Offset: 0x001E08C8
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06005442 RID: 21570 RVA: 0x001E2708 File Offset: 0x001E0908
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.SUFFICIENT_OXIDIZER.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06005443 RID: 21571 RVA: 0x001E2748 File Offset: 0x001E0948
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003748 RID: 14152
	private OxidizerTank oxidizerTank;
}
