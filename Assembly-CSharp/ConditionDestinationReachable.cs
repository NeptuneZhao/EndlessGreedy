using System;
using STRINGS;

// Token: 0x02000AF9 RID: 2809
public class ConditionDestinationReachable : ProcessCondition
{
	// Token: 0x060053D8 RID: 21464 RVA: 0x001E0A93 File Offset: 0x001DEC93
	public ConditionDestinationReachable(RocketModule module)
	{
		this.module = module;
		this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
	}

	// Token: 0x060053D9 RID: 21465 RVA: 0x001E0AB4 File Offset: 0x001DECB4
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (!this.module.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<RocketClusterDestinationSelector>().IsAtDestination())
				{
					result = ProcessCondition.Status.Ready;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (spacecraftDestination != null && this.CanReachSpacecraftDestination(spacecraftDestination) && spacecraftDestination.GetDestinationType().visitable)
			{
				result = ProcessCondition.Status.Ready;
			}
		}
		return result;
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x001E0B38 File Offset: 0x001DED38
	public bool CanReachSpacecraftDestination(SpaceDestination destination)
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		float rocketMaxDistance = this.module.GetComponent<CommandModule>().rocketStats.GetRocketMaxDistance();
		return (float)destination.OneBasedDistance * 10000f <= rocketMaxDistance;
	}

	// Token: 0x060053DB RID: 21467 RVA: 0x001E0B7C File Offset: 0x001DED7C
	public SpaceDestination GetSpacecraftDestination()
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
		return SpacecraftManager.instance.GetSpacecraftDestination(id);
	}

	// Token: 0x060053DC RID: 21468 RVA: 0x001E0BBC File Offset: 0x001DEDBC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x001E0C28 File Offset: 0x001DEE28
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (status == ProcessCondition.Status.Ready)
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
				}
				else
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
				}
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x060053DE RID: 21470 RVA: 0x001E0CA3 File Offset: 0x001DEEA3
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400372A RID: 14122
	private LaunchableRocketRegisterType craftRegisterType;

	// Token: 0x0400372B RID: 14123
	private RocketModule module;
}
