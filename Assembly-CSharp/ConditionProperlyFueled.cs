using System;
using STRINGS;

// Token: 0x02000B07 RID: 2823
public class ConditionProperlyFueled : ProcessCondition
{
	// Token: 0x0600542A RID: 21546 RVA: 0x001E2108 File Offset: 0x001E0308
	public ConditionProperlyFueled(IFuelTank fuelTank)
	{
		this.fuelTank = fuelTank;
	}

	// Token: 0x0600542B RID: 21547 RVA: 0x001E2118 File Offset: 0x001E0318
	public override ProcessCondition.Status EvaluateCondition()
	{
		RocketModuleCluster component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>();
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
				if (!component2.HasResourcesToMove(1, Clustercraft.CombustionResource.Fuel))
				{
					return ProcessCondition.Status.Failure;
				}
				return ProcessCondition.Status.Ready;
			}
			else
			{
				bool flag = component2.HasResourcesToMove(num * 2, Clustercraft.CombustionResource.Fuel);
				bool flag2 = component2.HasResourcesToMove(num, Clustercraft.CombustionResource.Fuel);
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

	// Token: 0x0600542C RID: 21548 RVA: 0x001E21BC File Offset: 0x001E03BC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x0600542D RID: 21549 RVA: 0x001E21FC File Offset: 0x001E03FC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		Clustercraft component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				if (component.Destination == component.Location)
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY_NO_DESTINATION;
				}
				else
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY;
				}
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x0600542E RID: 21550 RVA: 0x001E2277 File Offset: 0x001E0477
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003742 RID: 14146
	private IFuelTank fuelTank;
}
