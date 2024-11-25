using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C37 RID: 3127
public class DetailTabHeader : KMonoBehaviour
{
	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06005FF1 RID: 24561 RVA: 0x0023A752 File Offset: 0x00238952
	public TargetPanel ActivePanel
	{
		get
		{
			if (this.tabPanels.ContainsKey(this.selectedTabID))
			{
				return this.tabPanels[this.selectedTabID];
			}
			return null;
		}
	}

	// Token: 0x06005FF2 RID: 24562 RVA: 0x0023A77C File Offset: 0x0023897C
	public void Init()
	{
		this.detailsScreen = DetailsScreen.Instance;
		this.MakeTab("SIMPLEINFO", UI.DETAILTABS.SIMPLEINFO.NAME, Assets.GetSprite("icon_display_screen_status"), UI.DETAILTABS.SIMPLEINFO.TOOLTIP, this.simpleInfoScreen);
		this.MakeTab("PERSONALITY", UI.DETAILTABS.PERSONALITY.NAME, Assets.GetSprite("icon_display_screen_bio"), UI.DETAILTABS.PERSONALITY.TOOLTIP, this.minionPersonalityPanel);
		this.MakeTab("BUILDINGCHORES", UI.DETAILTABS.BUILDING_CHORES.NAME, Assets.GetSprite("icon_display_screen_errands"), UI.DETAILTABS.BUILDING_CHORES.TOOLTIP, this.buildingInfoPanel);
		this.MakeTab("DETAILS", UI.DETAILTABS.DETAILS.NAME, Assets.GetSprite("icon_display_screen_properties"), UI.DETAILTABS.DETAILS.TOOLTIP, this.additionalDetailsPanel);
		this.ChangeToDefaultTab();
	}

	// Token: 0x06005FF3 RID: 24563 RVA: 0x0023A86A File Offset: 0x00238A6A
	private void MakeTabContents(GameObject panelToActivate)
	{
	}

	// Token: 0x06005FF4 RID: 24564 RVA: 0x0023A86C File Offset: 0x00238A6C
	private void MakeTab(string id, string label, Sprite sprite, string tooltip, GameObject panelToActivate)
	{
		GameObject gameObject = Util.KInstantiateUI(this.tabPrefab, this.tabContainer, true);
		gameObject.name = "tab: " + id;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(tooltip);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = sprite;
		component.GetReference<LocText>("label").text = label;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		GameObject gameObject2 = Util.KInstantiateUI(panelToActivate, this.panelContainer.gameObject, true);
		TargetPanel component3 = gameObject2.GetComponent<TargetPanel>();
		component3.SetTarget(this.detailsScreen.target);
		this.tabPanels.Add(id, component3);
		string targetTab = id;
		MultiToggle multiToggle = component2;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.ChangeTab(targetTab);
		}));
		this.tabs.Add(id, component2);
		gameObject2.SetActive(false);
	}

	// Token: 0x06005FF5 RID: 24565 RVA: 0x0023A958 File Offset: 0x00238B58
	private void ChangeTab(string id)
	{
		this.selectedTabID = id;
		foreach (KeyValuePair<string, MultiToggle> keyValuePair in this.tabs)
		{
			keyValuePair.Value.ChangeState((keyValuePair.Key == this.selectedTabID) ? 1 : 0);
		}
		foreach (KeyValuePair<string, TargetPanel> keyValuePair2 in this.tabPanels)
		{
			if (keyValuePair2.Key == id)
			{
				keyValuePair2.Value.gameObject.SetActive(true);
				keyValuePair2.Value.SetTarget(this.detailsScreen.target);
			}
			else
			{
				keyValuePair2.Value.SetTarget(null);
				keyValuePair2.Value.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06005FF6 RID: 24566 RVA: 0x0023AA64 File Offset: 0x00238C64
	private void ChangeToDefaultTab()
	{
		this.ChangeTab("SIMPLEINFO");
	}

	// Token: 0x06005FF7 RID: 24567 RVA: 0x0023AA74 File Offset: 0x00238C74
	public void RefreshTabDisplayForTarget(GameObject target)
	{
		foreach (KeyValuePair<string, TargetPanel> keyValuePair in this.tabPanels)
		{
			this.tabs[keyValuePair.Key].gameObject.SetActive(keyValuePair.Value.IsValidForTarget(target));
		}
		if (this.tabPanels[this.selectedTabID].IsValidForTarget(target))
		{
			this.ChangeTab(this.selectedTabID);
			return;
		}
		this.ChangeToDefaultTab();
	}

	// Token: 0x040040BA RID: 16570
	private Dictionary<string, MultiToggle> tabs = new Dictionary<string, MultiToggle>();

	// Token: 0x040040BB RID: 16571
	private string selectedTabID;

	// Token: 0x040040BC RID: 16572
	[SerializeField]
	private GameObject tabPrefab;

	// Token: 0x040040BD RID: 16573
	[SerializeField]
	private GameObject tabContainer;

	// Token: 0x040040BE RID: 16574
	[SerializeField]
	private GameObject panelContainer;

	// Token: 0x040040BF RID: 16575
	[Header("Screen Prefabs")]
	[SerializeField]
	private GameObject simpleInfoScreen;

	// Token: 0x040040C0 RID: 16576
	[SerializeField]
	private GameObject minionPersonalityPanel;

	// Token: 0x040040C1 RID: 16577
	[SerializeField]
	private GameObject buildingInfoPanel;

	// Token: 0x040040C2 RID: 16578
	[SerializeField]
	private GameObject additionalDetailsPanel;

	// Token: 0x040040C3 RID: 16579
	[SerializeField]
	private GameObject cosmeticsPanel;

	// Token: 0x040040C4 RID: 16580
	[SerializeField]
	private GameObject materialPanel;

	// Token: 0x040040C5 RID: 16581
	private DetailsScreen detailsScreen;

	// Token: 0x040040C6 RID: 16582
	private Dictionary<string, TargetPanel> tabPanels = new Dictionary<string, TargetPanel>();
}
