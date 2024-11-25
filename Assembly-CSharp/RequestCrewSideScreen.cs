using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D98 RID: 3480
public class RequestCrewSideScreen : SideScreenContent
{
	// Token: 0x06006DB9 RID: 28089 RVA: 0x0029426C File Offset: 0x0029246C
	protected override void OnSpawn()
	{
		this.changeCrewButton.onClick += this.OnChangeCrewButtonPressed;
		this.crewReleaseButton.onClick += this.CrewRelease;
		this.crewRequestButton.onClick += this.CrewRequest;
		this.toggleMap.Add(this.crewReleaseButton, PassengerRocketModule.RequestCrewState.Release);
		this.toggleMap.Add(this.crewRequestButton, PassengerRocketModule.RequestCrewState.Request);
		this.Refresh();
	}

	// Token: 0x06006DBA RID: 28090 RVA: 0x002942E8 File Offset: 0x002924E8
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x06006DBB RID: 28091 RVA: 0x002942EC File Offset: 0x002924EC
	public override bool IsValidForTarget(GameObject target)
	{
		PassengerRocketModule component = target.GetComponent<PassengerRocketModule>();
		RocketControlStation component2 = target.GetComponent<RocketControlStation>();
		if (component != null)
		{
			return component.GetMyWorld() != null;
		}
		if (component2 != null)
		{
			RocketControlStation.StatesInstance smi = component2.GetSMI<RocketControlStation.StatesInstance>();
			return !smi.sm.IsInFlight(smi) && !smi.sm.IsLaunching(smi);
		}
		return false;
	}

	// Token: 0x06006DBC RID: 28092 RVA: 0x0029434E File Offset: 0x0029254E
	public override void SetTarget(GameObject target)
	{
		if (target.GetComponent<RocketControlStation>() != null)
		{
			this.rocketModule = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule();
		}
		else
		{
			this.rocketModule = target.GetComponent<PassengerRocketModule>();
		}
		this.Refresh();
	}

	// Token: 0x06006DBD RID: 28093 RVA: 0x0029438D File Offset: 0x0029258D
	private void Refresh()
	{
		this.RefreshRequestButtons();
	}

	// Token: 0x06006DBE RID: 28094 RVA: 0x00294395 File Offset: 0x00292595
	private void CrewRelease()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
		this.RefreshRequestButtons();
	}

	// Token: 0x06006DBF RID: 28095 RVA: 0x002943A9 File Offset: 0x002925A9
	private void CrewRequest()
	{
		this.rocketModule.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Request);
		this.RefreshRequestButtons();
	}

	// Token: 0x06006DC0 RID: 28096 RVA: 0x002943C0 File Offset: 0x002925C0
	private void RefreshRequestButtons()
	{
		foreach (KeyValuePair<KToggle, PassengerRocketModule.RequestCrewState> keyValuePair in this.toggleMap)
		{
			this.RefreshRequestButton(keyValuePair.Key);
		}
	}

	// Token: 0x06006DC1 RID: 28097 RVA: 0x0029441C File Offset: 0x0029261C
	private void RefreshRequestButton(KToggle button)
	{
		ImageToggleState[] componentsInChildren;
		if (this.toggleMap[button] == this.rocketModule.PassengersRequested)
		{
			button.isOn = true;
			componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SetActive();
			}
			button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			return;
		}
		button.isOn = false;
		componentsInChildren = button.GetComponentsInChildren<ImageToggleState>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetInactive();
		}
		button.GetComponent<ImageToggleStateThrobber>().enabled = false;
	}

	// Token: 0x06006DC2 RID: 28098 RVA: 0x002944A4 File Offset: 0x002926A4
	private void OnChangeCrewButtonPressed()
	{
		if (this.activeChangeCrewSideScreen == null)
		{
			this.activeChangeCrewSideScreen = (AssignmentGroupControllerSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeCrewSideScreenPrefab, UI.UISIDESCREENS.ASSIGNMENTGROUPCONTROLLER.TITLE);
			this.activeChangeCrewSideScreen.SetTarget(this.rocketModule.gameObject);
			return;
		}
		DetailsScreen.Instance.ClearSecondarySideScreen();
		this.activeChangeCrewSideScreen = null;
	}

	// Token: 0x06006DC3 RID: 28099 RVA: 0x0029450C File Offset: 0x0029270C
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			this.activeChangeCrewSideScreen = null;
		}
	}

	// Token: 0x04004AE0 RID: 19168
	private PassengerRocketModule rocketModule;

	// Token: 0x04004AE1 RID: 19169
	public KToggle crewReleaseButton;

	// Token: 0x04004AE2 RID: 19170
	public KToggle crewRequestButton;

	// Token: 0x04004AE3 RID: 19171
	private Dictionary<KToggle, PassengerRocketModule.RequestCrewState> toggleMap = new Dictionary<KToggle, PassengerRocketModule.RequestCrewState>();

	// Token: 0x04004AE4 RID: 19172
	public KButton changeCrewButton;

	// Token: 0x04004AE5 RID: 19173
	public KScreen changeCrewSideScreenPrefab;

	// Token: 0x04004AE6 RID: 19174
	private AssignmentGroupControllerSideScreen activeChangeCrewSideScreen;
}
