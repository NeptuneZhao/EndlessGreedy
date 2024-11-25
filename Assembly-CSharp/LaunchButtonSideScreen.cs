using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D76 RID: 3446
public class LaunchButtonSideScreen : SideScreenContent
{
	// Token: 0x06006C5E RID: 27742 RVA: 0x0028C233 File Offset: 0x0028A433
	protected override void OnSpawn()
	{
		this.Refresh();
		this.launchButton.onClick += this.TriggerLaunch;
	}

	// Token: 0x06006C5F RID: 27743 RVA: 0x0028C252 File Offset: 0x0028A452
	public override int GetSideScreenSortOrder()
	{
		return -100;
	}

	// Token: 0x06006C60 RID: 27744 RVA: 0x0028C256 File Offset: 0x0028A456
	public override bool IsValidForTarget(GameObject target)
	{
		return (target.GetComponent<RocketModule>() != null && target.HasTag(GameTags.LaunchButtonRocketModule)) || (target.GetComponent<LaunchPad>() && target.GetComponent<LaunchPad>().HasRocketWithCommandModule());
	}

	// Token: 0x06006C61 RID: 27745 RVA: 0x0028C290 File Offset: 0x0028A490
	public override void SetTarget(GameObject target)
	{
		bool flag = this.rocketModule == null || this.rocketModule.gameObject != target;
		this.selectedPad = null;
		this.rocketModule = target.GetComponent<RocketModuleCluster>();
		if (this.rocketModule == null)
		{
			this.selectedPad = target.GetComponent<LaunchPad>();
			if (this.selectedPad != null)
			{
				foreach (Ref<RocketModuleCluster> @ref in this.selectedPad.LandedRocket.CraftInterface.ClusterModules)
				{
					if (@ref.Get().GetComponent<LaunchableRocketCluster>())
					{
						this.rocketModule = @ref.Get().GetComponent<RocketModuleCluster>();
						break;
					}
				}
			}
		}
		if (this.selectedPad == null)
		{
			CraftModuleInterface craftInterface = this.rocketModule.CraftInterface;
			this.selectedPad = craftInterface.CurrentPad;
		}
		if (flag)
		{
			this.acknowledgeWarnings = false;
		}
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate);
		this.rocketModule.CraftInterface.Subscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate);
		this.Refresh();
	}

	// Token: 0x06006C62 RID: 27746 RVA: 0x0028C3D4 File Offset: 0x0028A5D4
	public override void ClearTarget()
	{
		if (this.rocketModule != null)
		{
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(543433792, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule.CraftInterface.Unsubscribe<LaunchButtonSideScreen>(1655598572, LaunchButtonSideScreen.RefreshDelegate, false);
			this.rocketModule = null;
		}
	}

	// Token: 0x06006C63 RID: 27747 RVA: 0x0028C42C File Offset: 0x0028A62C
	private void TriggerLaunch()
	{
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		if (flag)
		{
			this.acknowledgeWarnings = true;
		}
		else if (flag2)
		{
			this.rocketModule.CraftInterface.CancelLaunch();
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.rocketModule.CraftInterface.TriggerLaunch(false);
		}
		this.Refresh();
	}

	// Token: 0x06006C64 RID: 27748 RVA: 0x0028C4A3 File Offset: 0x0028A6A3
	public void Update()
	{
		if (Time.unscaledTime > this.lastRefreshTime + 1f)
		{
			this.lastRefreshTime = Time.unscaledTime;
			this.Refresh();
		}
	}

	// Token: 0x06006C65 RID: 27749 RVA: 0x0028C4CC File Offset: 0x0028A6CC
	private void Refresh()
	{
		if (this.rocketModule == null || this.selectedPad == null)
		{
			return;
		}
		bool flag = !this.acknowledgeWarnings && this.rocketModule.CraftInterface.HasLaunchWarnings();
		bool flag2 = this.rocketModule.CraftInterface.IsLaunchRequested();
		bool flag3 = this.selectedPad.IsLogicInputConnected();
		bool flag4 = flag3 ? this.rocketModule.CraftInterface.CheckReadyForAutomatedLaunchCommand() : this.rocketModule.CraftInterface.CheckPreppedForLaunch();
		bool flag5 = this.rocketModule.CraftInterface.HasTag(GameTags.RocketNotOnGround);
		if (flag3)
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP;
		}
		else if (DebugHandler.InstantBuildMode || flag4)
		{
			this.launchButton.isInteractable = true;
			if (flag2)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_REQUESTED_BUTTON_TOOLTIP;
			}
			else if (flag)
			{
				this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_WARNINGS_BUTTON_TOOLTIP;
			}
			else
			{
				LocString loc_string = DebugHandler.InstantBuildMode ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_DEBUG : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
				this.launchButton.GetComponentInChildren<LocText>().text = loc_string;
				this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_TOOLTIP;
			}
		}
		else
		{
			this.launchButton.isInteractable = false;
			this.launchButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON;
			this.launchButton.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAUNCH_BUTTON_NOT_READY_TOOLTIP;
		}
		UnityEngine.Object interiorWorld = this.rocketModule.CraftInterface.GetInteriorWorld();
		RoboPilotModule robotPilotModule = this.rocketModule.CraftInterface.GetRobotPilotModule();
		PassengerRocketModule passengerModule = this.rocketModule.CraftInterface.GetPassengerModule();
		if (!(interiorWorld != null))
		{
			if (robotPilotModule != null)
			{
				if (!flag4)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
					return;
				}
				if (!flag2)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
					return;
				}
				if (!flag5)
				{
					this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
					return;
				}
			}
			else
			{
				this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			}
			return;
		}
		List<RocketControlStation> worldItems = Components.RocketControlStations.GetWorldItems(this.rocketModule.CraftInterface.GetInteriorWorld().id, false);
		RocketControlStationLaunchWorkable rocketControlStationLaunchWorkable = null;
		if (worldItems != null && worldItems.Count > 0)
		{
			rocketControlStationLaunchWorkable = worldItems[0].GetComponent<RocketControlStationLaunchWorkable>();
		}
		if (passengerModule == null || rocketControlStationLaunchWorkable == null || robotPilotModule == null)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		bool flag6 = passengerModule.CheckPassengersBoarded(robotPilotModule == null);
		if (!flag6 && robotPilotModule != null)
		{
			flag6 |= !passengerModule.HasCrewAssigned();
		}
		bool flag7 = !passengerModule.CheckExtraPassengers();
		bool flag8 = robotPilotModule != null || rocketControlStationLaunchWorkable.worker != null;
		if (!flag4)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.STILL_PREPPING;
			return;
		}
		if (!flag2)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.READY_FOR_LAUNCH;
			return;
		}
		if (!flag6)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.LOADING_CREW;
			return;
		}
		if (!flag7)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.UNLOADING_PASSENGERS;
			return;
		}
		if (!flag8)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.WAITING_FOR_PILOT;
			return;
		}
		if (!flag5)
		{
			this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.COUNTING_DOWN;
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.STATUS.TAKING_OFF;
	}

	// Token: 0x040049E4 RID: 18916
	public KButton launchButton;

	// Token: 0x040049E5 RID: 18917
	public LocText statusText;

	// Token: 0x040049E6 RID: 18918
	private RocketModuleCluster rocketModule;

	// Token: 0x040049E7 RID: 18919
	private LaunchPad selectedPad;

	// Token: 0x040049E8 RID: 18920
	private bool acknowledgeWarnings;

	// Token: 0x040049E9 RID: 18921
	private float lastRefreshTime;

	// Token: 0x040049EA RID: 18922
	private const float UPDATE_FREQUENCY = 1f;

	// Token: 0x040049EB RID: 18923
	private static readonly EventSystem.IntraObjectHandler<LaunchButtonSideScreen> RefreshDelegate = new EventSystem.IntraObjectHandler<LaunchButtonSideScreen>(delegate(LaunchButtonSideScreen cmp, object data)
	{
		cmp.Refresh();
	});
}
