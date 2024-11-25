using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D55 RID: 3413
public class ClusterDestinationSideScreen : SideScreenContent
{
	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x06006B76 RID: 27510 RVA: 0x00286C2B File Offset: 0x00284E2B
	// (set) Token: 0x06006B77 RID: 27511 RVA: 0x00286C33 File Offset: 0x00284E33
	private ClusterDestinationSelector targetSelector { get; set; }

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x06006B78 RID: 27512 RVA: 0x00286C3C File Offset: 0x00284E3C
	// (set) Token: 0x06006B79 RID: 27513 RVA: 0x00286C44 File Offset: 0x00284E44
	private RocketClusterDestinationSelector targetRocketSelector { get; set; }

	// Token: 0x06006B7A RID: 27514 RVA: 0x00286C50 File Offset: 0x00284E50
	protected override void OnSpawn()
	{
		this.changeDestinationButton.onClick += this.OnClickChangeDestination;
		this.clearDestinationButton.onClick += this.OnClickClearDestination;
		this.launchPadDropDown.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		this.launchPadDropDown.CustomizeEmptyRow(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE, null);
		this.repeatButton.onClick += this.OnRepeatClicked;
	}

	// Token: 0x06006B7B RID: 27515 RVA: 0x00286CCD File Offset: 0x00284ECD
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x06006B7C RID: 27516 RVA: 0x00286CD4 File Offset: 0x00284ED4
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh(null);
			this.m_refreshHandle = this.targetSelector.Subscribe(543433792, delegate(object data)
			{
				this.Refresh(null);
			});
			return;
		}
		if (this.m_refreshHandle != -1)
		{
			this.targetSelector.Unsubscribe(this.m_refreshHandle);
			this.m_refreshHandle = -1;
			this.launchPadDropDown.Close();
		}
	}

	// Token: 0x06006B7D RID: 27517 RVA: 0x00286D44 File Offset: 0x00284F44
	public override bool IsValidForTarget(GameObject target)
	{
		ClusterDestinationSelector component = target.GetComponent<ClusterDestinationSelector>();
		return (component != null && component.assignable) || (target.GetComponent<RocketModule>() != null && target.HasTag(GameTags.LaunchButtonRocketModule)) || (target.GetComponent<RocketControlStation>() != null && target.GetComponent<RocketControlStation>().GetMyWorld().GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x06006B7E RID: 27518 RVA: 0x00286DB4 File Offset: 0x00284FB4
	public override void SetTarget(GameObject target)
	{
		this.targetSelector = target.GetComponent<ClusterDestinationSelector>();
		if (this.targetSelector == null)
		{
			if (target.GetComponent<RocketModuleCluster>() != null)
			{
				this.targetSelector = target.GetComponent<RocketModuleCluster>().CraftInterface.GetClusterDestinationSelector();
			}
			else if (target.GetComponent<RocketControlStation>() != null)
			{
				this.targetSelector = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetClusterDestinationSelector();
			}
		}
		this.targetRocketSelector = (this.targetSelector as RocketClusterDestinationSelector);
	}

	// Token: 0x06006B7F RID: 27519 RVA: 0x00286E3C File Offset: 0x0028503C
	private void Refresh(object data = null)
	{
		if (!this.targetSelector.IsAtDestination())
		{
			Sprite sprite;
			string str;
			string text;
			ClusterGrid.Instance.GetLocationDescription(this.targetSelector.GetDestination(), out sprite, out str, out text);
			this.destinationImage.sprite = sprite;
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + str;
			this.clearDestinationButton.isInteractable = true;
		}
		else
		{
			this.destinationImage.sprite = Assets.GetSprite("hex_unknown");
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + UI.SPACEDESTINATIONS.NONE.NAME;
			this.clearDestinationButton.isInteractable = false;
		}
		if (this.targetRocketSelector != null)
		{
			List<LaunchPad> launchPadsForDestination = LaunchPad.GetLaunchPadsForDestination(this.targetRocketSelector.GetDestination());
			this.launchPadDropDown.gameObject.SetActive(true);
			this.repeatButton.gameObject.SetActive(true);
			this.launchPadDropDown.Initialize(launchPadsForDestination, new Action<IListableOption, object>(this.OnLaunchPadEntryClick), new Func<IListableOption, IListableOption, object, int>(this.PadDropDownSort), new Action<DropDownEntry, object>(this.PadDropDownEntryRefreshAction), true, this.targetRocketSelector);
			if (!this.targetRocketSelector.IsAtDestination() && launchPadsForDestination.Count > 0)
			{
				this.launchPadDropDown.openButton.isInteractable = true;
				LaunchPad destinationPad = this.targetRocketSelector.GetDestinationPad();
				if (destinationPad != null)
				{
					this.launchPadDropDown.selectedLabel.text = destinationPad.GetProperName();
				}
				else
				{
					this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				}
			}
			else
			{
				this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				this.launchPadDropDown.openButton.isInteractable = false;
			}
			this.StyleRepeatButton();
		}
		else
		{
			this.launchPadDropDown.gameObject.SetActive(false);
			this.repeatButton.gameObject.SetActive(false);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x06006B80 RID: 27520 RVA: 0x00287043 File Offset: 0x00285243
	private void OnClickChangeDestination()
	{
		if (this.targetSelector.assignable)
		{
			ClusterMapScreen.Instance.ShowInSelectDestinationMode(this.targetSelector);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x06006B81 RID: 27521 RVA: 0x00287068 File Offset: 0x00285268
	private void StyleChangeDestinationButton()
	{
	}

	// Token: 0x06006B82 RID: 27522 RVA: 0x0028706A File Offset: 0x0028526A
	private void OnClickClearDestination()
	{
		this.targetSelector.SetDestination(this.targetSelector.GetMyWorldLocation());
	}

	// Token: 0x06006B83 RID: 27523 RVA: 0x00287084 File Offset: 0x00285284
	private void OnLaunchPadEntryClick(IListableOption option, object data)
	{
		LaunchPad destinationPad = (LaunchPad)option;
		this.targetRocketSelector.SetDestinationPad(destinationPad);
	}

	// Token: 0x06006B84 RID: 27524 RVA: 0x002870A4 File Offset: 0x002852A4
	private void PadDropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		LaunchPad launchPad = (LaunchPad)entry.entryData;
		Clustercraft component = this.targetRocketSelector.GetComponent<Clustercraft>();
		if (!(launchPad != null))
		{
			entry.button.isInteractable = true;
			entry.image.sprite = Assets.GetBuildingDef("LaunchPad").GetUISprite("ui", false);
			entry.tooltip.SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_FIRST_AVAILABLE);
			return;
		}
		string simpleTooltip;
		if (component.CanLandAtPad(launchPad, out simpleTooltip) == Clustercraft.PadLandingStatus.CanNeverLand)
		{
			entry.button.isInteractable = false;
			entry.image.sprite = Assets.GetSprite("iconWarning");
			entry.tooltip.SetSimpleTooltip(simpleTooltip);
			return;
		}
		entry.button.isInteractable = true;
		entry.image.sprite = launchPad.GetComponent<Building>().Def.GetUISprite("ui", false);
		entry.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_VALID_SITE, launchPad.GetProperName()));
	}

	// Token: 0x06006B85 RID: 27525 RVA: 0x002871A3 File Offset: 0x002853A3
	private int PadDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x06006B86 RID: 27526 RVA: 0x002871A6 File Offset: 0x002853A6
	private void OnRepeatClicked()
	{
		this.targetRocketSelector.Repeat = !this.targetRocketSelector.Repeat;
		this.StyleRepeatButton();
	}

	// Token: 0x06006B87 RID: 27527 RVA: 0x002871C7 File Offset: 0x002853C7
	private void StyleRepeatButton()
	{
		this.repeatButton.bgImage.colorStyleSetting = (this.targetRocketSelector.Repeat ? this.repeatOn : this.repeatOff);
		this.repeatButton.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x0400493D RID: 18749
	public Image destinationImage;

	// Token: 0x0400493E RID: 18750
	public LocText destinationLabel;

	// Token: 0x0400493F RID: 18751
	public KButton changeDestinationButton;

	// Token: 0x04004940 RID: 18752
	public KButton clearDestinationButton;

	// Token: 0x04004941 RID: 18753
	public DropDown launchPadDropDown;

	// Token: 0x04004942 RID: 18754
	public KButton repeatButton;

	// Token: 0x04004943 RID: 18755
	public ColorStyleSetting repeatOff;

	// Token: 0x04004944 RID: 18756
	public ColorStyleSetting repeatOn;

	// Token: 0x04004945 RID: 18757
	public ColorStyleSetting defaultButton;

	// Token: 0x04004946 RID: 18758
	public ColorStyleSetting highlightButton;

	// Token: 0x04004949 RID: 18761
	private int m_refreshHandle = -1;
}
