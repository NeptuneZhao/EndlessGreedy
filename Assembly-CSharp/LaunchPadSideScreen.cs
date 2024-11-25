using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D77 RID: 3447
public class LaunchPadSideScreen : SideScreenContent
{
	// Token: 0x06006C68 RID: 27752 RVA: 0x0028C911 File Offset: 0x0028AB11
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.startNewRocketbutton.onClick += this.ClickStartNewRocket;
		this.devAutoRocketButton.onClick += this.ClickAutoRocket;
	}

	// Token: 0x06006C69 RID: 27753 RVA: 0x0028C947 File Offset: 0x0028AB47
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
		}
	}

	// Token: 0x06006C6A RID: 27754 RVA: 0x0028C95D File Offset: 0x0028AB5D
	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x06006C6B RID: 27755 RVA: 0x0028C961 File Offset: 0x0028AB61
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x06006C6C RID: 27756 RVA: 0x0028C970 File Offset: 0x0028AB70
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		if (this.refreshEventHandle != -1)
		{
			this.selectedPad.Unsubscribe(this.refreshEventHandle);
		}
		this.selectedPad = new_target.GetComponent<LaunchPad>();
		if (this.selectedPad == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchPad component");
			return;
		}
		this.refreshEventHandle = this.selectedPad.Subscribe(-887025858, new Action<object>(this.RefreshWaitingToLandList));
		this.RefreshRocketButton();
		this.RefreshWaitingToLandList(null);
	}

	// Token: 0x06006C6D RID: 27757 RVA: 0x0028CA00 File Offset: 0x0028AC00
	private void RefreshWaitingToLandList(object data = null)
	{
		for (int i = this.waitingToLandRows.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.waitingToLandRows[i]);
		}
		this.waitingToLandRows.Clear();
		this.nothingWaitingRow.SetActive(true);
		AxialI myWorldLocation = this.selectedPad.GetMyWorldLocation();
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesInRange(myWorldLocation, 1))
		{
			Clustercraft craft = clusterGridEntity as Clustercraft;
			if (!(craft == null) && craft.Status == Clustercraft.CraftStatus.InFlight && (!craft.IsFlightInProgress() || !(craft.Destination != myWorldLocation)))
			{
				GameObject gameObject = Util.KInstantiateUI(this.landableRocketRowPrefab, this.landableRowContainer, true);
				gameObject.GetComponentInChildren<LocText>().text = craft.Name;
				this.waitingToLandRows.Add(gameObject);
				KButton componentInChildren = gameObject.GetComponentInChildren<KButton>();
				componentInChildren.GetComponentInChildren<LocText>().SetText((craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad) ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.CANCEL_LAND_BUTTON : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAND_BUTTON);
				string simpleTooltip;
				componentInChildren.isInteractable = (craft.CanLandAtPad(this.selectedPad, out simpleTooltip) != Clustercraft.PadLandingStatus.CanNeverLand);
				if (!componentInChildren.isInteractable)
				{
					componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
				}
				else
				{
					componentInChildren.GetComponent<ToolTip>().ClearMultiStringTooltip();
				}
				componentInChildren.onClick += delegate()
				{
					if (craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad)
					{
						craft.GetComponent<ClusterDestinationSelector>().SetDestination(craft.Location);
					}
					else
					{
						craft.LandAtPad(this.selectedPad);
					}
					this.RefreshWaitingToLandList(null);
				};
				this.nothingWaitingRow.SetActive(false);
			}
		}
	}

	// Token: 0x06006C6E RID: 27758 RVA: 0x0028CC00 File Offset: 0x0028AE00
	private void ClickStartNewRocket()
	{
		((SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL)).SetLaunchPad(this.selectedPad);
	}

	// Token: 0x06006C6F RID: 27759 RVA: 0x0028CC2C File Offset: 0x0028AE2C
	private void RefreshRocketButton()
	{
		bool isOperational = this.selectedPad.GetComponent<Operational>().IsOperational;
		this.startNewRocketbutton.isInteractable = (this.selectedPad.LandedRocket == null && isOperational);
		if (!isOperational)
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED);
		}
		else
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().ClearMultiStringTooltip();
		}
		this.devAutoRocketButton.isInteractable = (this.selectedPad.LandedRocket == null);
		this.devAutoRocketButton.gameObject.SetActive(DebugHandler.InstantBuildMode);
	}

	// Token: 0x06006C70 RID: 27760 RVA: 0x0028CCC8 File Offset: 0x0028AEC8
	private void ClickAutoRocket()
	{
		AutoRocketUtility.StartAutoRocket(this.selectedPad);
	}

	// Token: 0x040049EC RID: 18924
	public GameObject content;

	// Token: 0x040049ED RID: 18925
	private LaunchPad selectedPad;

	// Token: 0x040049EE RID: 18926
	public LocText DescriptionText;

	// Token: 0x040049EF RID: 18927
	public GameObject landableRocketRowPrefab;

	// Token: 0x040049F0 RID: 18928
	public GameObject newRocketPanel;

	// Token: 0x040049F1 RID: 18929
	public KButton startNewRocketbutton;

	// Token: 0x040049F2 RID: 18930
	public KButton devAutoRocketButton;

	// Token: 0x040049F3 RID: 18931
	public GameObject landableRowContainer;

	// Token: 0x040049F4 RID: 18932
	public GameObject nothingWaitingRow;

	// Token: 0x040049F5 RID: 18933
	public KScreen changeModuleSideScreen;

	// Token: 0x040049F6 RID: 18934
	private int refreshEventHandle = -1;

	// Token: 0x040049F7 RID: 18935
	public List<GameObject> waitingToLandRows = new List<GameObject>();
}
