using System;
using System.Collections.Generic;
using Database;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BE4 RID: 3044
[AddComponentMenu("KMonoBehaviour/scripts/AsteroidDescriptorPanel")]
public class AsteroidDescriptorPanel : KMonoBehaviour
{
	// Token: 0x06005C90 RID: 23696 RVA: 0x0021DB26 File Offset: 0x0021BD26
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x06005C91 RID: 23697 RVA: 0x0021DB36 File Offset: 0x0021BD36
	public void EnableClusterDetails(bool setActive)
	{
		this.clusterNameLabel.gameObject.SetActive(setActive);
		this.clusterDifficultyLabel.gameObject.SetActive(setActive);
	}

	// Token: 0x06005C92 RID: 23698 RVA: 0x0021DB5A File Offset: 0x0021BD5A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005C93 RID: 23699 RVA: 0x0021DB64 File Offset: 0x0021BD64
	public void SetClusterDetailLabels(ColonyDestinationAsteroidBeltData cluster)
	{
		StringEntry stringEntry;
		Strings.TryGet(cluster.properName, out stringEntry);
		this.clusterNameLabel.SetText((stringEntry == null) ? "" : string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, stringEntry.String));
		int index = Mathf.Clamp(cluster.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		string text = string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third);
		text = text.Trim('\n');
		this.clusterDifficultyLabel.SetText(text);
	}

	// Token: 0x06005C94 RID: 23700 RVA: 0x0021DC00 File Offset: 0x0021BE00
	public void SetParameterDescriptors(IList<AsteroidDescriptor> descriptors)
	{
		for (int i = 0; i < this.parameterWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.parameterWidgets[i]);
		}
		this.parameterWidgets.Clear();
		for (int j = 0; j < descriptors.Count; j++)
		{
			GameObject gameObject = global::Util.KInstantiateUI(this.prefabParameterWidget, base.gameObject, true);
			gameObject.GetComponent<LocText>().SetText(descriptors[j].text);
			ToolTip component = gameObject.GetComponent<ToolTip>();
			if (!string.IsNullOrEmpty(descriptors[j].tooltip))
			{
				component.SetSimpleTooltip(descriptors[j].tooltip);
			}
			this.parameterWidgets.Add(gameObject);
		}
	}

	// Token: 0x06005C95 RID: 23701 RVA: 0x0021DCB4 File Offset: 0x0021BEB4
	private void ClearTraitDescriptors()
	{
		for (int i = 0; i < this.traitWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.traitWidgets[i]);
		}
		this.traitWidgets.Clear();
		for (int j = 0; j < this.traitCategoryWidgets.Count; j++)
		{
			UnityEngine.Object.Destroy(this.traitCategoryWidgets[j]);
		}
		this.traitCategoryWidgets.Clear();
	}

	// Token: 0x06005C96 RID: 23702 RVA: 0x0021DD28 File Offset: 0x0021BF28
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, List<string> stories, bool includeDescriptions = true)
	{
		foreach (string id in stories)
		{
			WorldTrait storyTrait = Db.Get().Stories.Get(id).StoryTrait;
			string tooltip = DlcManager.IsPureVanilla() ? Strings.Get(storyTrait.description + "_SHORT") : Strings.Get(storyTrait.description);
			descriptors.Add(new AsteroidDescriptor(Strings.Get(storyTrait.name).String, tooltip, Color.white, null, storyTrait.icon));
		}
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
		if (stories.Count != 0)
		{
			this.storyTraitHeader.rectTransform().SetSiblingIndex(this.storyTraitHeader.rectTransform().parent.childCount - stories.Count - 1);
			this.storyTraitHeader.SetActive(true);
			return;
		}
		this.storyTraitHeader.SetActive(false);
	}

	// Token: 0x06005C97 RID: 23703 RVA: 0x0021DE40 File Offset: 0x0021C040
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, bool includeDescriptions = true)
	{
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
	}

	// Token: 0x06005C98 RID: 23704 RVA: 0x0021DE58 File Offset: 0x0021C058
	public void SetTraitDescriptors(List<IList<AsteroidDescriptor>> descriptorSets, bool includeDescriptions = true, List<global::Tuple<string, Sprite>> headerData = null)
	{
		this.ClearTraitDescriptors();
		for (int i = 0; i < descriptorSets.Count; i++)
		{
			IList<AsteroidDescriptor> list = descriptorSets[i];
			GameObject gameObject = base.gameObject;
			if (descriptorSets.Count > 1)
			{
				global::Debug.Assert(headerData != null, "Asteroid Header data is null - traits wont have their world as contex in the selection UI");
				GameObject gameObject2 = global::Util.KInstantiate(this.prefabTraitCategoryWidget, base.gameObject, null);
				HierarchyReferences component = gameObject2.GetComponent<HierarchyReferences>();
				gameObject2.transform.localScale = Vector3.one;
				StringEntry stringEntry;
				string text = Strings.TryGet(headerData[i].first, out stringEntry) ? stringEntry.String : headerData[i].first;
				component.GetReference<LocText>("NameLabel").SetText(text);
				component.GetReference<Image>("Icon").sprite = headerData[i].second;
				gameObject2.SetActive(true);
				gameObject = component.GetReference<RectTransform>("Contents").gameObject;
				this.traitCategoryWidgets.Add(gameObject2);
			}
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject3 = global::Util.KInstantiate(this.prefabTraitWidget, gameObject, null);
				HierarchyReferences component2 = gameObject3.GetComponent<HierarchyReferences>();
				gameObject3.SetActive(true);
				component2.GetReference<LocText>("NameLabel").SetText("<b>" + list[j].text + "</b>");
				Image reference = component2.GetReference<Image>("Icon");
				reference.color = list[j].associatedColor;
				if (list[j].associatedIcon != null)
				{
					Sprite sprite = Assets.GetSprite(list[j].associatedIcon);
					if (sprite != null)
					{
						reference.sprite = sprite;
					}
				}
				if (gameObject3.GetComponent<ToolTip>() != null)
				{
					gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(list[j].tooltip);
				}
				LocText reference2 = component2.GetReference<LocText>("DescLabel");
				if (includeDescriptions && !string.IsNullOrEmpty(list[j].tooltip))
				{
					reference2.SetText(list[j].tooltip);
				}
				else
				{
					reference2.gameObject.SetActive(false);
				}
				gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject3.SetActive(true);
				this.traitWidgets.Add(gameObject3);
			}
		}
	}

	// Token: 0x06005C99 RID: 23705 RVA: 0x0021E0B8 File Offset: 0x0021C2B8
	public void EnableClusterLocationLabels(bool enable)
	{
		this.startingAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.nearbyAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.distantAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
	}

	// Token: 0x06005C9A RID: 23706 RVA: 0x0021E118 File Offset: 0x0021C318
	public void RefreshAsteroidLines(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> storyTraits)
	{
		cluster.RemixClusterLayout();
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			if (!keyValuePair.Value.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(keyValuePair.Value);
			}
		}
		this.asteroidLines.Clear();
		this.SpawnAsteroidLine(cluster.GetStartWorld, this.startingAsteroidRowContainer, cluster);
		for (int i = 0; i < cluster.worlds.Count; i++)
		{
			ProcGen.World world = cluster.worlds[i];
			WorldPlacement worldPlacement = null;
			for (int j = 0; j < cluster.Layout.worldPlacements.Count; j++)
			{
				if (cluster.Layout.worldPlacements[j].world == world.filePath)
				{
					worldPlacement = cluster.Layout.worldPlacements[j];
					break;
				}
			}
			this.SpawnAsteroidLine(world, (worldPlacement.locationType == WorldPlacement.LocationType.InnerCluster) ? this.nearbyAsteroidRowContainer : this.distantAsteroidRowContainer, cluster);
		}
		using (Dictionary<ProcGen.World, GameObject>.Enumerator enumerator = this.asteroidLines.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ProcGen.World, GameObject> line = enumerator.Current;
				MultiToggle component = line.Value.GetComponent<MultiToggle>();
				component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
				{
					this.SelectAsteroidInCluster(line.Key, cluster, selectedAsteroidDetailsPanel);
				}));
			}
		}
		this.SelectWholeClusterDetails(cluster, selectedAsteroidDetailsPanel, storyTraits);
	}

	// Token: 0x06005C9B RID: 23707 RVA: 0x0021E328 File Offset: 0x0021C528
	private void SelectAsteroidInCluster(ProcGen.World asteroid, ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(true);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(0);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == asteroid) ? 1 : 0);
			if (keyValuePair.Key == asteroid)
			{
				this.SetSelectedAsteroid(keyValuePair.Key, selectedAsteroidDetailsPanel, cluster.GenerateTraitDescriptors(keyValuePair.Key, true));
			}
		}
	}

	// Token: 0x06005C9C RID: 23708 RVA: 0x0021E3D8 File Offset: 0x0021C5D8
	public void SelectWholeClusterDetails(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> stories)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(false);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState(0);
		}
		this.SetSelectedCluster(cluster, selectedAsteroidDetailsPanel, stories);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(1);
	}

	// Token: 0x06005C9D RID: 23709 RVA: 0x0021E45C File Offset: 0x0021C65C
	private void SpawnAsteroidLine(ProcGen.World asteroid, GameObject parentContainer, ColonyDestinationAsteroidBeltData cluster)
	{
		if (this.asteroidLines.ContainsKey(asteroid))
		{
			return;
		}
		GameObject gameObject = global::Util.KInstantiateUI(this.prefabAsteroidLine, parentContainer.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("Label");
		RectTransform reference3 = component.GetReference<RectTransform>("TraitsRow");
		LocText reference4 = component.GetReference<LocText>("TraitLabel");
		ToolTip component2 = gameObject.GetComponent<ToolTip>();
		Image component3 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		Sprite uisprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		reference.sprite = uisprite;
		reference2.SetText(asteroid.GetProperName());
		List<WorldTrait> worldTraits = cluster.GetWorldTraits(asteroid);
		reference4.gameObject.SetActive(worldTraits.Count == 0);
		reference4.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS);
		RectTransform reference5 = component.GetReference<RectTransform>("TraitIconPrefab");
		foreach (WorldTrait worldTrait in worldTraits)
		{
			Image component4 = global::Util.KInstantiateUI(reference5.gameObject, reference3.gameObject, true).GetComponent<Image>();
			Sprite sprite = Assets.GetSprite(worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1));
			if (sprite != null)
			{
				component4.sprite = sprite;
			}
			component4.color = global::Util.ColorFromHex(worldTrait.colorHex);
		}
		string text = "";
		if (worldTraits.Count > 0)
		{
			for (int i = 0; i < worldTraits.Count; i++)
			{
				StringEntry stringEntry;
				Strings.TryGet(worldTraits[i].name, out stringEntry);
				StringEntry stringEntry2;
				Strings.TryGet(worldTraits[i].description, out stringEntry2);
				text = string.Concat(new string[]
				{
					text,
					"<color=#",
					worldTraits[i].colorHex,
					">",
					stringEntry.String,
					"</color>\n",
					stringEntry2.String
				});
				if (i != worldTraits.Count - 1)
				{
					text += "\n\n";
				}
			}
		}
		else
		{
			text = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
		{
			text = text + "\n\n" + string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_CONTENT, DlcManager.GetDlcTitle(asteroid.dlcIdFrom));
		}
		component2.SetSimpleTooltip(text);
		if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
		{
			component3.color = DlcManager.GetDlcBannerColor(asteroid.dlcIdFrom);
			component3.gameObject.SetActive(true);
		}
		else
		{
			component3.gameObject.SetActive(false);
		}
		this.asteroidLines.Add(asteroid, gameObject);
	}

	// Token: 0x06005C9E RID: 23710 RVA: 0x0021E73C File Offset: 0x0021C93C
	private void SetSelectedAsteroid(ProcGen.World asteroid, AsteroidDescriptorPanel detailPanel, List<AsteroidDescriptor> traitDescriptors)
	{
		detailPanel.SetTraitDescriptors(traitDescriptors, true);
		detailPanel.selectedAsteroidIcon.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(true);
		detailPanel.selectedAsteroidLabel.SetText(asteroid.GetProperName());
		detailPanel.selectedAsteroidDescription.SetText(asteroid.GetProperDescription());
	}

	// Token: 0x06005C9F RID: 23711 RVA: 0x0021E79C File Offset: 0x0021C99C
	private void SetSelectedCluster(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel detailPanel, List<string> stories)
	{
		List<IList<AsteroidDescriptor>> list = new List<IList<AsteroidDescriptor>>();
		List<global::Tuple<string, Sprite>> list2 = new List<global::Tuple<string, Sprite>>();
		List<AsteroidDescriptor> list3 = cluster.GenerateTraitDescriptors(cluster.GetStartWorld, false);
		if (list3.Count != 0)
		{
			list2.Add(new global::Tuple<string, Sprite>(cluster.GetStartWorld.name, ColonyDestinationAsteroidBeltData.GetUISprite(cluster.GetStartWorld.asteroidIcon)));
			list.Add(list3);
		}
		foreach (ProcGen.World world in cluster.worlds)
		{
			List<AsteroidDescriptor> list4 = cluster.GenerateTraitDescriptors(world, false);
			if (list4.Count != 0)
			{
				list2.Add(new global::Tuple<string, Sprite>(world.name, ColonyDestinationAsteroidBeltData.GetUISprite(world.asteroidIcon)));
				list.Add(list4);
			}
		}
		list2.Add(new global::Tuple<string, Sprite>("STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER", Assets.GetSprite("codexIconStoryTraits")));
		List<AsteroidDescriptor> list5 = new List<AsteroidDescriptor>();
		foreach (string id in stories)
		{
			Story story = Db.Get().Stories.Get(id);
			string icon = story.StoryTrait.icon;
			AsteroidDescriptor item = new AsteroidDescriptor(Strings.Get(story.StoryTrait.name).String, Strings.Get(story.StoryTrait.description).String, Color.white, null, icon);
			list5.Add(item);
		}
		list.Add(list5);
		detailPanel.SetTraitDescriptors(list, false, list2);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(false);
		string text = cluster.properName;
		StringEntry stringEntry;
		if (Strings.TryGet(cluster.properName, out stringEntry))
		{
			text = stringEntry.String;
		}
		detailPanel.selectedAsteroidLabel.SetText(text);
		detailPanel.selectedAsteroidDescription.SetText("");
	}

	// Token: 0x04003DCA RID: 15818
	[Header("Destination Details")]
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x04003DCB RID: 15819
	[SerializeField]
	private GameObject prefabTraitWidget;

	// Token: 0x04003DCC RID: 15820
	[SerializeField]
	private GameObject prefabTraitCategoryWidget;

	// Token: 0x04003DCD RID: 15821
	[SerializeField]
	private GameObject prefabParameterWidget;

	// Token: 0x04003DCE RID: 15822
	[SerializeField]
	private GameObject startingAsteroidRowContainer;

	// Token: 0x04003DCF RID: 15823
	[SerializeField]
	private GameObject nearbyAsteroidRowContainer;

	// Token: 0x04003DD0 RID: 15824
	[SerializeField]
	private GameObject distantAsteroidRowContainer;

	// Token: 0x04003DD1 RID: 15825
	[SerializeField]
	private LocText clusterNameLabel;

	// Token: 0x04003DD2 RID: 15826
	[SerializeField]
	private LocText clusterDifficultyLabel;

	// Token: 0x04003DD3 RID: 15827
	[SerializeField]
	public LocText headerLabel;

	// Token: 0x04003DD4 RID: 15828
	[SerializeField]
	public MultiToggle clusterDetailsButton;

	// Token: 0x04003DD5 RID: 15829
	[SerializeField]
	public GameObject storyTraitHeader;

	// Token: 0x04003DD6 RID: 15830
	private List<GameObject> labels = new List<GameObject>();

	// Token: 0x04003DD7 RID: 15831
	[Header("Selected Asteroid Details")]
	[SerializeField]
	private GameObject SpacedOutContentContainer;

	// Token: 0x04003DD8 RID: 15832
	public Image selectedAsteroidIcon;

	// Token: 0x04003DD9 RID: 15833
	public LocText selectedAsteroidLabel;

	// Token: 0x04003DDA RID: 15834
	public LocText selectedAsteroidDescription;

	// Token: 0x04003DDB RID: 15835
	[SerializeField]
	private GameObject prefabAsteroidLine;

	// Token: 0x04003DDC RID: 15836
	private Dictionary<ProcGen.World, GameObject> asteroidLines = new Dictionary<ProcGen.World, GameObject>();

	// Token: 0x04003DDD RID: 15837
	private List<GameObject> traitWidgets = new List<GameObject>();

	// Token: 0x04003DDE RID: 15838
	private List<GameObject> traitCategoryWidgets = new List<GameObject>();

	// Token: 0x04003DDF RID: 15839
	private List<GameObject> parameterWidgets = new List<GameObject>();
}
