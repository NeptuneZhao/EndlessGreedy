using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B08 RID: 2824
public class ConditionRobotPilotReady : ProcessCondition
{
	// Token: 0x0600542F RID: 21551 RVA: 0x001E227A File Offset: 0x001E047A
	public ConditionRobotPilotReady(RoboPilotModule module)
	{
		this.module = module;
		this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
		if (this.craftRegisterType == LaunchableRocketRegisterType.Clustercraft)
		{
			this.craftInterface = module.GetComponent<RocketModuleCluster>().CraftInterface;
		}
	}

	// Token: 0x06005430 RID: 21552 RVA: 0x001E22B4 File Offset: 0x001E04B4
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				UnityEngine.Object component = this.craftInterface.GetComponent<Clustercraft>();
				ClusterTraveler component2 = this.craftInterface.GetComponent<ClusterTraveler>();
				if (component == null || component2 == null || component2.CurrentPath == null)
				{
					return ProcessCondition.Status.Failure;
				}
				int num = component2.RemainingTravelNodes();
				bool flag = this.module.HasResourcesToMove(num * 2);
				bool flag2 = this.module.HasResourcesToMove(num);
				if (flag)
				{
					result = ProcessCondition.Status.Ready;
				}
				else if (this.RocketHasDupeControlStation())
				{
					result = ProcessCondition.Status.Warning;
				}
				else if (flag2)
				{
					result = ProcessCondition.Status.Warning;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (spacecraftDestination == null)
			{
				result = ((this.module.GetDataBanksStored() >= 1f) ? ProcessCondition.Status.Warning : ProcessCondition.Status.Failure);
			}
			else if (this.module.HasResourcesToMove(spacecraftDestination.OneBasedDistance * 2))
			{
				result = ProcessCondition.Status.Ready;
			}
			else if (this.module.GetDataBanksStored() >= 1f)
			{
				result = ProcessCondition.Status.Warning;
			}
		}
		return result;
	}

	// Token: 0x06005431 RID: 21553 RVA: 0x001E23BC File Offset: 0x001E05BC
	private bool RocketHasDupeControlStation()
	{
		if (this.craftInterface != null)
		{
			WorldContainer component = this.craftInterface.GetComponent<WorldContainer>();
			if (component != null && Components.RocketControlStations.GetWorldItems(component.id, false).Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005432 RID: 21554 RVA: 0x001E2408 File Offset: 0x001E0608
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.READY;
		}
		if (status != ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.FAILURE;
		}
		if (this.RocketHasDupeControlStation())
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING_NO_DATA_BANKS_HUMAN_PILOT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING;
	}

	// Token: 0x06005433 RID: 21555 RVA: 0x001E2448 File Offset: 0x001E0648
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
			if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
			{
				if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft && this.craftInterface.GetClusterDestinationSelector().IsAtDestination())
				{
					return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION;
				}
			}
			else
			{
				int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
				if (SpacecraftManager.instance.GetSpacecraftDestination(id) == null)
				{
					return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION;
				}
			}
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY;
		}
		if (status != ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE;
		}
		if (this.RocketHasDupeControlStation())
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING_NO_DATA_BANKS_HUMAN_PILOT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING;
	}

	// Token: 0x06005434 RID: 21556 RVA: 0x001E24EE File Offset: 0x001E06EE
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003743 RID: 14147
	private LaunchableRocketRegisterType craftRegisterType;

	// Token: 0x04003744 RID: 14148
	private RoboPilotModule module;

	// Token: 0x04003745 RID: 14149
	private CraftModuleInterface craftInterface;
}
