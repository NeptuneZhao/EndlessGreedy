using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D23 RID: 3363
public class ResearchScreenSideBar : KScreen
{
	// Token: 0x06006936 RID: 26934 RVA: 0x00277804 File Offset: 0x00275A04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.PopualteProjects();
		this.PopulateFilterButtons();
		this.RefreshCategoriesContentExpanded();
		this.RefreshWidgets();
		this.searchBox.OnValueChangesPaused = delegate()
		{
			this.UpdateCurrentSearch(this.searchBox.text);
		};
		KInputTextField kinputTextField = this.searchBox;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchBox.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.clearSearchButton.onClick += delegate()
		{
			this.ResetFilter();
		};
		this.ConfigCompletionFilters();
		base.ConsumeMouseScroll = true;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.UpdateProjectFilter));
	}

	// Token: 0x06006937 RID: 26935 RVA: 0x002778CC File Offset: 0x00275ACC
	private void Update()
	{
		for (int i = 0; i < Math.Min(this.QueuedActivations.Count, this.activationPerFrame); i++)
		{
			this.QueuedActivations[i].SetActive(true);
		}
		this.QueuedActivations.RemoveRange(0, Math.Min(this.QueuedActivations.Count, this.activationPerFrame));
		for (int j = 0; j < Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame); j++)
		{
			this.QueuedDeactivations[j].SetActive(false);
		}
		this.QueuedDeactivations.RemoveRange(0, Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame));
	}

	// Token: 0x06006938 RID: 26936 RVA: 0x00277984 File Offset: 0x00275B84
	private void ConfigCompletionFilters()
	{
		MultiToggle multiToggle = this.allFilter;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All);
		}));
		MultiToggle multiToggle2 = this.completedFilter;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Completed);
		}));
		MultiToggle multiToggle3 = this.availableFilter;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Available);
		}));
		this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All);
	}

	// Token: 0x06006939 RID: 26937 RVA: 0x00277A10 File Offset: 0x00275C10
	private void SetCompletionFilter(ResearchScreenSideBar.CompletionState state)
	{
		this.completionFilter = state;
		this.allFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.All) ? 1 : 0);
		this.completedFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.Completed) ? 1 : 0);
		this.availableFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.Available) ? 1 : 0);
		this.UpdateProjectFilter(null);
	}

	// Token: 0x0600693A RID: 26938 RVA: 0x00277A81 File Offset: 0x00275C81
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 21f;
	}

	// Token: 0x0600693B RID: 26939 RVA: 0x00277A98 File Offset: 0x00275C98
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.researchScreen != null && this.researchScreen.canvas && !this.researchScreen.canvas.enabled)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		if (!e.Consumed)
		{
			Vector2 vector = base.transform.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (vector.x >= 0f && vector.x <= base.transform.rectTransform().rect.width)
			{
				if (e.TryConsume(global::Action.MouseRight))
				{
					return;
				}
				if (e.TryConsume(global::Action.MouseLeft))
				{
					return;
				}
				if (!KInputManager.currentControllerIsGamepad)
				{
					if (e.TryConsume(global::Action.ZoomIn))
					{
						return;
					}
					e.TryConsume(global::Action.ZoomOut);
					return;
				}
			}
		}
	}

	// Token: 0x0600693C RID: 26940 RVA: 0x00277B66 File Offset: 0x00275D66
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.RefreshWidgets();
	}

	// Token: 0x0600693D RID: 26941 RVA: 0x00277B78 File Offset: 0x00275D78
	private void UpdateCurrentSearch(string newValue)
	{
		if (base.isEditing)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
			{
				this.filterStates[keyValuePair.Key] = false;
				keyValuePair.Value.GetComponent<MultiToggle>().ChangeState(0);
			}
		}
		this.currentSearchString = newValue;
		this.UpdateProjectFilter(null);
	}

	// Token: 0x0600693E RID: 26942 RVA: 0x00277C00 File Offset: 0x00275E00
	private void UpdateProjectFilter(object data = null)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectCategories)
		{
			dictionary.Add(keyValuePair.Key, false);
		}
		this.RefreshProjectsActive();
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.projectTechs)
		{
			if ((keyValuePair2.Value.activeSelf || this.QueuedActivations.Contains(keyValuePair2.Value)) && !this.QueuedDeactivations.Contains(keyValuePair2.Value))
			{
				dictionary[Db.Get().Techs.Get(keyValuePair2.Key).category] = true;
				this.categoryExpanded[Db.Get().Techs.Get(keyValuePair2.Key).category] = true;
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair3 in dictionary)
		{
			this.ChangeGameObjectActive(this.projectCategories[keyValuePair3.Key], keyValuePair3.Value);
		}
		this.RefreshCategoriesContentExpanded();
	}

	// Token: 0x0600693F RID: 26943 RVA: 0x00277D84 File Offset: 0x00275F84
	private void RefreshProjectsActive()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectTechs)
		{
			bool flag = this.CheckTechPassesFilters(keyValuePair.Key);
			this.ChangeGameObjectActive(keyValuePair.Value, flag);
			this.researchScreen.GetEntry(Db.Get().Techs.Get(keyValuePair.Key)).UpdateFilterState(flag);
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.projectTechItems[keyValuePair.Key])
			{
				bool flag2 = this.CheckTechItemPassesFilters(keyValuePair2.Key);
				HierarchyReferences component = keyValuePair2.Value.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").color = (flag2 ? Color.white : Color.grey);
				component.GetReference<Image>("Icon").color = (flag2 ? Color.white : new Color(1f, 1f, 1f, 0.5f));
			}
		}
	}

	// Token: 0x06006940 RID: 26944 RVA: 0x00277EE8 File Offset: 0x002760E8
	private void RefreshCategoriesContentExpanded()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectCategories)
		{
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject.SetActive(this.categoryExpanded[keyValuePair.Key]);
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.categoryExpanded[keyValuePair.Key] ? 1 : 0);
		}
	}

	// Token: 0x06006941 RID: 26945 RVA: 0x00277F9C File Offset: 0x0027619C
	private void PopualteProjects()
	{
		List<global::Tuple<global::Tuple<string, GameObject>, int>> list = new List<global::Tuple<global::Tuple<string, GameObject>, int>>();
		for (int i = 0; i < Db.Get().Techs.Count; i++)
		{
			Tech tech = (Tech)Db.Get().Techs.GetResource(i);
			if (!this.projectCategories.ContainsKey(tech.category))
			{
				string categoryID = tech.category;
				GameObject gameObject = Util.KInstantiateUI(this.techCategoryPrefabAlt, this.projectsContainer, true);
				gameObject.name = categoryID;
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + categoryID.ToUpper()));
				this.categoryExpanded.Add(categoryID, false);
				this.projectCategories.Add(categoryID, gameObject);
				gameObject.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.categoryExpanded[categoryID] = !this.categoryExpanded[categoryID];
					this.RefreshCategoriesContentExpanded();
				};
			}
			GameObject gameObject2 = this.SpawnTechWidget(tech.Id, this.projectCategories[tech.category]);
			list.Add(new global::Tuple<global::Tuple<string, GameObject>, int>(new global::Tuple<string, GameObject>(tech.Id, gameObject2), tech.tier));
			this.projectTechs.Add(tech.Id, gameObject2);
			gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(tech.desc);
			MultiToggle component = gameObject2.GetComponent<MultiToggle>();
			component.onEnter = (System.Action)Delegate.Combine(component.onEnter, new System.Action(delegate()
			{
				this.researchScreen.TurnEverythingOff();
				this.researchScreen.GetEntry(tech).OnHover(true, tech);
				this.soundPlayer.Play(1);
			}));
			MultiToggle component2 = gameObject2.GetComponent<MultiToggle>();
			component2.onExit = (System.Action)Delegate.Combine(component2.onExit, new System.Action(delegate()
			{
				this.researchScreen.TurnEverythingOff();
			}));
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectTechs)
		{
			Transform reference = this.projectCategories[Db.Get().Techs.Get(keyValuePair.Key).category].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
			this.projectTechs[keyValuePair.Key].transform.SetParent(reference);
		}
		list.Sort((global::Tuple<global::Tuple<string, GameObject>, int> a, global::Tuple<global::Tuple<string, GameObject>, int> b) => a.second.CompareTo(b.second));
		foreach (global::Tuple<global::Tuple<string, GameObject>, int> tuple in list)
		{
			tuple.first.second.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06006942 RID: 26946 RVA: 0x002782AC File Offset: 0x002764AC
	private void PopulateFilterButtons()
	{
		using (Dictionary<string, List<Tag>>.Enumerator enumerator = this.filterPresets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, List<Tag>> kvp = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.filterButtonPrefab, this.searchFiltersContainer, true);
				this.filterButtons.Add(kvp.Key, gameObject);
				this.filterStates.Add(kvp.Key, false);
				MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
				gameObject.GetComponentInChildren<LocText>().SetText(Strings.Get("STRINGS.UI.RESEARCHSCREEN.FILTER_BUTTONS." + kvp.Key.ToUpper()));
				MultiToggle toggle2 = toggle;
				toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
				{
					foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
					{
						if (keyValuePair.Key != kvp.Key)
						{
							this.filterStates[keyValuePair.Key] = false;
							this.filterButtons[keyValuePair.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[keyValuePair.Key] ? 1 : 0);
						}
					}
					this.filterStates[kvp.Key] = !this.filterStates[kvp.Key];
					toggle.ChangeState(this.filterStates[kvp.Key] ? 1 : 0);
					if (this.filterStates[kvp.Key])
					{
						StringEntry stringEntry = Strings.Get("STRINGS.UI.RESEARCHSCREEN.FILTER_BUTTONS." + kvp.Key.ToUpper());
						this.searchBox.text = stringEntry.String;
						return;
					}
					this.searchBox.text = "";
				}));
			}
		}
	}

	// Token: 0x06006943 RID: 26947 RVA: 0x002783B4 File Offset: 0x002765B4
	public void RefreshQueue()
	{
	}

	// Token: 0x06006944 RID: 26948 RVA: 0x002783B8 File Offset: 0x002765B8
	private void RefreshWidgets()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		using (Dictionary<string, GameObject>.Enumerator enumerator = this.projectTechs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, GameObject> kvp = enumerator.Current;
				if (Db.Get().Techs.Get(kvp.Key).IsComplete())
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(2);
				}
				else if (researchQueue.Find((TechInstance match) => match.tech.Id == kvp.Key) != null)
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(1);
				}
				else
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(0);
				}
			}
		}
	}

	// Token: 0x06006945 RID: 26949 RVA: 0x0027849C File Offset: 0x0027669C
	private void RefreshWidgetProgressBars(string techID, GameObject widget)
	{
		HierarchyReferences component = widget.GetComponent<HierarchyReferences>();
		ResearchPointInventory progressInventory = Research.Instance.GetTechInstance(techID).progressInventory;
		int num = 0;
		for (int i = 0; i < Research.Instance.researchTypes.Types.Count; i++)
		{
			if (Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID.ContainsKey(Research.Instance.researchTypes.Types[i].id) && Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id] > 0f)
			{
				HierarchyReferences component2 = component.GetReference<RectTransform>("BarRows").GetChild(1 + num).GetComponent<HierarchyReferences>();
				float num2 = progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[i].id] / Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id];
				RectTransform rectTransform = component2.GetReference<Image>("Bar").rectTransform;
				rectTransform.sizeDelta = new Vector2(rectTransform.parent.rectTransform().rect.width * num2, rectTransform.sizeDelta.y);
				component2.GetReference<LocText>("Label").SetText(progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[i].id].ToString() + "/" + Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id].ToString());
				num++;
			}
		}
	}

	// Token: 0x06006946 RID: 26950 RVA: 0x002786A4 File Offset: 0x002768A4
	private GameObject SpawnTechWidget(string techID, GameObject parentContainer)
	{
		GameObject gameObject = Util.KInstantiateUI(this.techWidgetRootAltPrefab, parentContainer, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		gameObject.name = Db.Get().Techs.Get(techID).Name;
		component.GetReference<LocText>("Label").SetText(Db.Get().Techs.Get(techID).Name);
		if (!this.projectTechItems.ContainsKey(techID))
		{
			this.projectTechItems.Add(techID, new Dictionary<string, GameObject>());
		}
		RectTransform reference = component.GetReference<RectTransform>("UnlockContainer");
		System.Action <>9__0;
		foreach (TechItem techItem in Db.Get().Techs.Get(techID).unlockedItems)
		{
			if (SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(techItem.requiredDlcIds, techItem.forbiddenDlcIds))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.techItemPrefab, reference.gameObject, true);
				gameObject2.GetComponentsInChildren<Image>()[1].sprite = techItem.UISprite();
				gameObject2.GetComponentsInChildren<LocText>()[0].SetText(techItem.Name);
				MultiToggle component2 = gameObject2.GetComponent<MultiToggle>();
				Delegate onClick = component2.onClick;
				System.Action b;
				if ((b = <>9__0) == null)
				{
					b = (<>9__0 = delegate()
					{
						this.researchScreen.ZoomToTech(techID);
					});
				}
				component2.onClick = (System.Action)Delegate.Combine(onClick, b);
				gameObject2.GetComponentsInChildren<Image>()[0].color = (this.evenRow ? this.evenRowColor : this.oddRowColor);
				this.evenRow = !this.evenRow;
				if (!this.projectTechItems[techID].ContainsKey(techItem.Id))
				{
					this.projectTechItems[techID].Add(techItem.Id, gameObject2);
				}
			}
		}
		MultiToggle component3 = gameObject.GetComponent<MultiToggle>();
		component3.onClick = (System.Action)Delegate.Combine(component3.onClick, new System.Action(delegate()
		{
			this.researchScreen.ZoomToTech(techID);
		}));
		return gameObject;
	}

	// Token: 0x06006947 RID: 26951 RVA: 0x002788F4 File Offset: 0x00276AF4
	private void ChangeGameObjectActive(GameObject target, bool targetActiveState)
	{
		if (target.activeSelf != targetActiveState)
		{
			if (targetActiveState)
			{
				this.QueuedActivations.Add(target);
				if (this.QueuedDeactivations.Contains(target))
				{
					this.QueuedDeactivations.Remove(target);
					return;
				}
			}
			else
			{
				this.QueuedDeactivations.Add(target);
				if (this.QueuedActivations.Contains(target))
				{
					this.QueuedActivations.Remove(target);
				}
			}
		}
	}

	// Token: 0x06006948 RID: 26952 RVA: 0x0027895C File Offset: 0x00276B5C
	private bool CheckTechItemPassesFilters(string techItemID)
	{
		TechItem techItem = Db.Get().TechItems.Get(techItemID);
		bool flag = true;
		switch (this.completionFilter)
		{
		case ResearchScreenSideBar.CompletionState.Available:
			flag = (flag && !techItem.IsComplete() && techItem.ParentTech.ArePrerequisitesComplete());
			break;
		case ResearchScreenSideBar.CompletionState.Completed:
			flag = (flag && techItem.IsComplete());
			break;
		}
		if (!flag)
		{
			return flag;
		}
		flag = (flag && ResearchScreen.TechItemPassesSearchFilter(techItemID, this.currentSearchString));
		foreach (KeyValuePair<string, bool> keyValuePair in this.filterStates)
		{
		}
		return flag;
	}

	// Token: 0x06006949 RID: 26953 RVA: 0x00278A1C File Offset: 0x00276C1C
	private bool CheckTechPassesFilters(string techID)
	{
		Tech tech = Db.Get().Techs.Get(techID);
		bool flag = true;
		switch (this.completionFilter)
		{
		case ResearchScreenSideBar.CompletionState.Available:
			flag = (flag && !tech.IsComplete() && tech.ArePrerequisitesComplete());
			break;
		case ResearchScreenSideBar.CompletionState.Completed:
			flag = (flag && tech.IsComplete());
			break;
		}
		if (!flag)
		{
			return flag;
		}
		flag = (flag && ResearchScreen.TechPassesSearchFilter(techID, this.currentSearchString));
		foreach (KeyValuePair<string, bool> keyValuePair in this.filterStates)
		{
		}
		return flag;
	}

	// Token: 0x0600694A RID: 26954 RVA: 0x00278AD8 File Offset: 0x00276CD8
	public void ResetFilter()
	{
		this.UpdateCurrentSearch("");
		this.searchBox.text = "";
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
		{
			this.filterStates[keyValuePair.Key] = false;
			this.filterButtons[keyValuePair.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[keyValuePair.Key] ? 1 : 0);
		}
		this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All);
	}

	// Token: 0x04004772 RID: 18290
	[Header("Containers")]
	[SerializeField]
	private GameObject queueContainer;

	// Token: 0x04004773 RID: 18291
	[SerializeField]
	private GameObject projectsContainer;

	// Token: 0x04004774 RID: 18292
	[SerializeField]
	private GameObject searchFiltersContainer;

	// Token: 0x04004775 RID: 18293
	[Header("Prefabs")]
	[SerializeField]
	private GameObject headerTechTypePrefab;

	// Token: 0x04004776 RID: 18294
	[SerializeField]
	private GameObject filterButtonPrefab;

	// Token: 0x04004777 RID: 18295
	[SerializeField]
	private GameObject techWidgetRootPrefab;

	// Token: 0x04004778 RID: 18296
	[SerializeField]
	private GameObject techWidgetRootAltPrefab;

	// Token: 0x04004779 RID: 18297
	[SerializeField]
	private GameObject techItemPrefab;

	// Token: 0x0400477A RID: 18298
	[SerializeField]
	private GameObject techWidgetUnlockedItemPrefab;

	// Token: 0x0400477B RID: 18299
	[SerializeField]
	private GameObject techWidgetRowPrefab;

	// Token: 0x0400477C RID: 18300
	[SerializeField]
	private GameObject techCategoryPrefab;

	// Token: 0x0400477D RID: 18301
	[SerializeField]
	private GameObject techCategoryPrefabAlt;

	// Token: 0x0400477E RID: 18302
	[Header("Other references")]
	[SerializeField]
	private KInputTextField searchBox;

	// Token: 0x0400477F RID: 18303
	[SerializeField]
	private MultiToggle allFilter;

	// Token: 0x04004780 RID: 18304
	[SerializeField]
	private MultiToggle availableFilter;

	// Token: 0x04004781 RID: 18305
	[SerializeField]
	private MultiToggle completedFilter;

	// Token: 0x04004782 RID: 18306
	[SerializeField]
	private ResearchScreen researchScreen;

	// Token: 0x04004783 RID: 18307
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04004784 RID: 18308
	[SerializeField]
	private Color evenRowColor;

	// Token: 0x04004785 RID: 18309
	[SerializeField]
	private Color oddRowColor;

	// Token: 0x04004786 RID: 18310
	private ResearchScreenSideBar.CompletionState completionFilter;

	// Token: 0x04004787 RID: 18311
	private Dictionary<string, bool> filterStates = new Dictionary<string, bool>();

	// Token: 0x04004788 RID: 18312
	private Dictionary<string, bool> categoryExpanded = new Dictionary<string, bool>();

	// Token: 0x04004789 RID: 18313
	private string currentSearchString = "";

	// Token: 0x0400478A RID: 18314
	private Dictionary<string, GameObject> queueTechs = new Dictionary<string, GameObject>();

	// Token: 0x0400478B RID: 18315
	private Dictionary<string, GameObject> projectTechs = new Dictionary<string, GameObject>();

	// Token: 0x0400478C RID: 18316
	private Dictionary<string, GameObject> projectCategories = new Dictionary<string, GameObject>();

	// Token: 0x0400478D RID: 18317
	private Dictionary<string, GameObject> filterButtons = new Dictionary<string, GameObject>();

	// Token: 0x0400478E RID: 18318
	private Dictionary<string, Dictionary<string, GameObject>> projectTechItems = new Dictionary<string, Dictionary<string, GameObject>>();

	// Token: 0x0400478F RID: 18319
	private Dictionary<string, List<Tag>> filterPresets = new Dictionary<string, List<Tag>>
	{
		{
			"Oxygen",
			new List<Tag>()
		},
		{
			"Food",
			new List<Tag>()
		},
		{
			"Water",
			new List<Tag>()
		},
		{
			"Power",
			new List<Tag>()
		},
		{
			"Morale",
			new List<Tag>()
		},
		{
			"Ranching",
			new List<Tag>()
		},
		{
			"Filter",
			new List<Tag>()
		},
		{
			"Tile",
			new List<Tag>()
		},
		{
			"Transport",
			new List<Tag>()
		},
		{
			"Automation",
			new List<Tag>()
		},
		{
			"Medicine",
			new List<Tag>()
		},
		{
			"Rocket",
			new List<Tag>()
		}
	};

	// Token: 0x04004790 RID: 18320
	private List<GameObject> QueuedActivations = new List<GameObject>();

	// Token: 0x04004791 RID: 18321
	private List<GameObject> QueuedDeactivations = new List<GameObject>();

	// Token: 0x04004792 RID: 18322
	public ButtonSoundPlayer soundPlayer;

	// Token: 0x04004793 RID: 18323
	[SerializeField]
	private int activationPerFrame = 5;

	// Token: 0x04004794 RID: 18324
	private bool evenRow;

	// Token: 0x02001E47 RID: 7751
	private enum CompletionState
	{
		// Token: 0x04008A01 RID: 35329
		All,
		// Token: 0x04008A02 RID: 35330
		Available,
		// Token: 0x04008A03 RID: 35331
		Completed
	}
}
