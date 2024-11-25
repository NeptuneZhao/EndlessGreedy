using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DC6 RID: 3526
public class SpacePOISimpleInfoPanel : SimpleInfoPanel
{
	// Token: 0x06006FE4 RID: 28644 RVA: 0x002A2FCC File Offset: 0x002A11CC
	public SpacePOISimpleInfoPanel(SimpleInfoScreen simpleInfoScreen) : base(simpleInfoScreen)
	{
	}

	// Token: 0x06006FE5 RID: 28645 RVA: 0x002A2FEC File Offset: 0x002A11EC
	public override void Refresh(CollapsibleDetailContentPanel spacePOIPanel, GameObject selectedTarget)
	{
		spacePOIPanel.SetTitle(UI.CLUSTERMAP.POI.TITLE);
		if (selectedTarget == null)
		{
			spacePOIPanel.gameObject.SetActive(false);
			return;
		}
		HarvestablePOIClusterGridEntity harvestablePOIClusterGridEntity = (selectedTarget == null) ? null : selectedTarget.GetComponent<HarvestablePOIClusterGridEntity>();
		Clustercraft component = selectedTarget.GetComponent<Clustercraft>();
		ArtifactPOIConfigurator component2 = selectedTarget.GetComponent<ArtifactPOIConfigurator>();
		if (harvestablePOIClusterGridEntity == null && component == null && component2 == null)
		{
			spacePOIPanel.gameObject.SetActive(false);
			return;
		}
		if (harvestablePOIClusterGridEntity == null && component2 == null && component != null)
		{
			RocketModuleCluster rocketModuleCluster = null;
			CraftModuleInterface craftModuleInterface = null;
			RocketSimpleInfoPanel.GetRocketStuffFromTarget(selectedTarget, ref rocketModuleCluster, ref component, ref craftModuleInterface);
			if (component != null)
			{
				foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesOnCell(component.GetMyWorldLocation()))
				{
					HarvestablePOIClusterGridEntity harvestablePOIClusterGridEntity2 = clusterGridEntity as HarvestablePOIClusterGridEntity;
					if (harvestablePOIClusterGridEntity2 != null)
					{
						harvestablePOIClusterGridEntity = harvestablePOIClusterGridEntity2;
						component2 = harvestablePOIClusterGridEntity2.GetComponent<ArtifactPOIConfigurator>();
						break;
					}
				}
			}
		}
		bool flag = harvestablePOIClusterGridEntity != null || component2 != null;
		spacePOIPanel.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		HarvestablePOIStates.Instance harvestable = (harvestablePOIClusterGridEntity == null) ? null : harvestablePOIClusterGridEntity.GetSMI<HarvestablePOIStates.Instance>();
		this.RefreshMassHeader(harvestable, selectedTarget, spacePOIPanel);
		this.RefreshElements(harvestable, selectedTarget, spacePOIPanel);
		this.RefreshArtifacts(component2, selectedTarget, spacePOIPanel);
	}

	// Token: 0x06006FE6 RID: 28646 RVA: 0x002A3160 File Offset: 0x002A1360
	private void RefreshMassHeader(HarvestablePOIStates.Instance harvestable, GameObject selectedTarget, CollapsibleDetailContentPanel spacePOIPanel)
	{
		if (this.massHeader == null)
		{
			this.massHeader = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
		}
		this.massHeader.SetActive(harvestable != null);
		if (harvestable == null)
		{
			return;
		}
		HierarchyReferences component = this.massHeader.GetComponent<HierarchyReferences>();
		Sprite sprite = Assets.GetSprite("icon_asteroid_type");
		if (sprite != null)
		{
			component.GetReference<Image>("Icon").sprite = sprite;
		}
		component.GetReference<LocText>("NameLabel").text = UI.CLUSTERMAP.POI.MASS_REMAINING;
		component.GetReference<LocText>("ValueLabel").text = GameUtil.GetFormattedMass(harvestable.poiCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
	}

	// Token: 0x06006FE7 RID: 28647 RVA: 0x002A3238 File Offset: 0x002A1438
	private void RefreshElements(HarvestablePOIStates.Instance harvestable, GameObject selectedTarget, CollapsibleDetailContentPanel spacePOIPanel)
	{
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.elementRows)
		{
			if (keyValuePair.Value != null)
			{
				keyValuePair.Value.SetActive(false);
			}
		}
		if (harvestable == null)
		{
			return;
		}
		Dictionary<SimHashes, float> elementsWithWeights = harvestable.configuration.GetElementsWithWeights();
		float num = 0f;
		List<KeyValuePair<SimHashes, float>> list = new List<KeyValuePair<SimHashes, float>>();
		foreach (KeyValuePair<SimHashes, float> item in elementsWithWeights)
		{
			num += item.Value;
			list.Add(item);
		}
		list.Sort((KeyValuePair<SimHashes, float> a, KeyValuePair<SimHashes, float> b) => b.Value.CompareTo(a.Value));
		foreach (KeyValuePair<SimHashes, float> keyValuePair2 in list)
		{
			SimHashes key = keyValuePair2.Key;
			Tag tag = key.CreateTag();
			if (!this.elementRows.ContainsKey(key.CreateTag()))
			{
				this.elementRows.Add(tag, Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true));
			}
			this.elementRows[tag].SetActive(true);
			HierarchyReferences component = this.elementRows[tag].GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("NameLabel").text = ElementLoader.GetElement(tag).name;
			component.GetReference<LocText>("ValueLabel").text = GameUtil.GetFormattedPercent(keyValuePair2.Value / num * 100f, GameUtil.TimeSlice.None);
			component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		}
	}

	// Token: 0x06006FE8 RID: 28648 RVA: 0x002A3498 File Offset: 0x002A1698
	private void RefreshRocketsAtThisLocation(HarvestablePOIStates.Instance harvestable, GameObject selectedTarget, CollapsibleDetailContentPanel spacePOIPanel)
	{
		if (this.rocketsHeader == null)
		{
			this.rocketsSpacer = Util.KInstantiateUI(this.simpleInfoRoot.spacerRow, spacePOIPanel.Content.gameObject, true);
			this.rocketsHeader = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
			HierarchyReferences component = this.rocketsHeader.GetComponent<HierarchyReferences>();
			Sprite sprite = Assets.GetSprite("ic_rocket");
			if (sprite != null)
			{
				component.GetReference<Image>("Icon").sprite = sprite;
				component.GetReference<Image>("Icon").color = Color.black;
			}
			component.GetReference<LocText>("NameLabel").text = UI.CLUSTERMAP.POI.ROCKETS_AT_THIS_LOCATION;
			component.GetReference<LocText>("ValueLabel").text = "";
		}
		this.rocketsSpacer.rectTransform().SetAsLastSibling();
		this.rocketsHeader.rectTransform().SetAsLastSibling();
		foreach (KeyValuePair<Clustercraft, GameObject> keyValuePair in this.rocketRows)
		{
			keyValuePair.Value.SetActive(false);
		}
		bool flag = true;
		for (int i = 0; i < Components.Clustercrafts.Count; i++)
		{
			Clustercraft clustercraft = Components.Clustercrafts[i];
			if (!this.rocketRows.ContainsKey(clustercraft))
			{
				GameObject value = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
				this.rocketRows.Add(clustercraft, value);
			}
			bool flag2 = clustercraft.Location == selectedTarget.GetComponent<KMonoBehaviour>().GetMyWorldLocation();
			flag = (flag && !flag2);
			this.rocketRows[clustercraft].SetActive(flag2);
			if (flag2)
			{
				HierarchyReferences component2 = this.rocketRows[clustercraft].GetComponent<HierarchyReferences>();
				component2.GetReference<Image>("Icon").sprite = clustercraft.GetUISprite();
				component2.GetReference<Image>("Icon").color = Color.grey;
				component2.GetReference<LocText>("NameLabel").text = clustercraft.Name;
				component2.GetReference<LocText>("ValueLabel").text = "";
				component2.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
				this.rocketRows[clustercraft].rectTransform().SetAsLastSibling();
			}
		}
		this.rocketsHeader.SetActive(!flag);
		this.rocketsSpacer.SetActive(this.rocketsHeader.activeSelf);
	}

	// Token: 0x06006FE9 RID: 28649 RVA: 0x002A3748 File Offset: 0x002A1948
	private void RefreshArtifacts(ArtifactPOIConfigurator artifactConfigurator, GameObject selectedTarget, CollapsibleDetailContentPanel spacePOIPanel)
	{
		if (this.artifactsSpacer == null)
		{
			this.artifactsSpacer = Util.KInstantiateUI(this.simpleInfoRoot.spacerRow, spacePOIPanel.Content.gameObject, true);
			this.artifactRow = Util.KInstantiateUI(this.simpleInfoRoot.iconLabelRow, spacePOIPanel.Content.gameObject, true);
		}
		this.artifactsSpacer.rectTransform().SetAsLastSibling();
		this.artifactRow.rectTransform().SetAsLastSibling();
		ArtifactPOIStates.Instance smi = artifactConfigurator.GetSMI<ArtifactPOIStates.Instance>();
		smi.configuration.GetArtifactID();
		HierarchyReferences component = this.artifactRow.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("NameLabel").text = UI.CLUSTERMAP.POI.ARTIFACTS;
		component.GetReference<LocText>("ValueLabel").alignment = TextAlignmentOptions.MidlineRight;
		component.GetReference<Image>("Icon").sprite = Assets.GetSprite("ic_artifacts");
		component.GetReference<Image>("Icon").color = Color.black;
		if (smi.CanHarvestArtifact())
		{
			component.GetReference<LocText>("ValueLabel").text = UI.CLUSTERMAP.POI.ARTIFACTS_AVAILABLE;
			return;
		}
		component.GetReference<LocText>("ValueLabel").text = string.Format(UI.CLUSTERMAP.POI.ARTIFACTS_DEPLETED, GameUtil.GetFormattedCycles(smi.RechargeTimeRemaining(), "F1", true));
	}

	// Token: 0x04004CAE RID: 19630
	private Dictionary<Tag, GameObject> elementRows = new Dictionary<Tag, GameObject>();

	// Token: 0x04004CAF RID: 19631
	private Dictionary<Clustercraft, GameObject> rocketRows = new Dictionary<Clustercraft, GameObject>();

	// Token: 0x04004CB0 RID: 19632
	private GameObject massHeader;

	// Token: 0x04004CB1 RID: 19633
	private GameObject rocketsSpacer;

	// Token: 0x04004CB2 RID: 19634
	private GameObject rocketsHeader;

	// Token: 0x04004CB3 RID: 19635
	private GameObject artifactsSpacer;

	// Token: 0x04004CB4 RID: 19636
	private GameObject artifactRow;
}
