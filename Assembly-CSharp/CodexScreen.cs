using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000BFE RID: 3070
public class CodexScreen : KScreen
{
	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06005E18 RID: 24088 RVA: 0x0022F20E File Offset: 0x0022D40E
	// (set) Token: 0x06005E19 RID: 24089 RVA: 0x0022F216 File Offset: 0x0022D416
	public string activeEntryID
	{
		get
		{
			return this._activeEntryID;
		}
		private set
		{
			this._activeEntryID = value;
		}
	}

	// Token: 0x06005E1A RID: 24090 RVA: 0x0022F220 File Offset: 0x0022D420
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = true;
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		if (string.IsNullOrEmpty(this.activeEntryID))
		{
			this.ChangeArticle("HOME", false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		}
		this.searchInputField.onValueChanged.AddListener(delegate(string value)
		{
			this.FilterSearch(value);
		});
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			this.editingSearch = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			this.editingSearch = false;
		});
	}

	// Token: 0x06005E1B RID: 24091 RVA: 0x0022F2FD File Offset: 0x0022D4FD
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.editingSearch)
		{
			e.Consumed = true;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005E1C RID: 24092 RVA: 0x0022F315 File Offset: 0x0022D515
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06005E1D RID: 24093 RVA: 0x0022F31C File Offset: 0x0022D51C
	public void RefreshTutorialMessages()
	{
		if (!this.HasFocus)
		{
			return;
		}
		string key = CodexCache.FormatLinkID("MISCELLANEOUSTIPS");
		CodexEntry codexEntry;
		if (CodexCache.entries.TryGetValue(key, out codexEntry))
		{
			for (int i = 0; i < codexEntry.subEntries.Count; i++)
			{
				for (int j = 0; j < codexEntry.subEntries[i].contentContainers.Count; j++)
				{
					for (int k = 0; k < codexEntry.subEntries[i].contentContainers[j].content.Count; k++)
					{
						CodexText codexText = codexEntry.subEntries[i].contentContainers[j].content[k] as CodexText;
						if (codexText != null && codexText.messageID == MISC.NOTIFICATIONS.BASICCONTROLS.NAME)
						{
							if (KInputManager.currentControllerIsGamepad)
							{
								codexText.text = MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODYALT;
							}
							else
							{
								codexText.text = MISC.NOTIFICATIONS.BASICCONTROLS.MESSAGEBODY;
							}
							if (!string.IsNullOrEmpty(this.activeEntryID))
							{
								this.ChangeArticle("MISCELLANEOUSTIPS0", false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06005E1E RID: 24094 RVA: 0x0022F460 File Offset: 0x0022D660
	private void CodexScreenInit()
	{
		this.textStyles[CodexTextStyle.Title] = this.textStyleTitle;
		this.textStyles[CodexTextStyle.Subtitle] = this.textStyleSubtitle;
		this.textStyles[CodexTextStyle.Body] = this.textStyleBody;
		this.textStyles[CodexTextStyle.BodyWhite] = this.textStyleBodyWhite;
		this.SetupPrefabs();
		this.PopulatePools();
		this.CategorizeEntries();
		this.FilterSearch("");
		this.backButtonButton.onClick += this.HistoryStepBack;
		this.backButtonButton.soundPlayer.AcceptClickCondition = (() => this.currentHistoryIdx > 0);
		this.fwdButtonButton.onClick += this.HistoryStepForward;
		this.fwdButtonButton.soundPlayer.AcceptClickCondition = (() => this.currentHistoryIdx < this.history.Count - 1);
		Game.Instance.Subscribe(1594320620, delegate(object val)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this.FilterSearch(this.searchInputField.text);
			if (!string.IsNullOrEmpty(this.activeEntryID))
			{
				this.ChangeArticle(this.activeEntryID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		});
		KInputManager.InputChange.AddListener(new UnityAction(this.RefreshTutorialMessages));
	}

	// Token: 0x06005E1F RID: 24095 RVA: 0x0022F56C File Offset: 0x0022D76C
	private void SetupPrefabs()
	{
		this.contentContainerPool = new UIGameObjectPool(this.prefabContentContainer);
		this.contentContainerPool.disabledElementParent = this.widgetPool;
		this.ContentPrefabs[typeof(CodexText)] = this.prefabTextWidget;
		this.ContentPrefabs[typeof(CodexTextWithTooltip)] = this.prefabTextWithTooltipWidget;
		this.ContentPrefabs[typeof(CodexImage)] = this.prefabImageWidget;
		this.ContentPrefabs[typeof(CodexDividerLine)] = this.prefabDividerLineWidget;
		this.ContentPrefabs[typeof(CodexSpacer)] = this.prefabSpacer;
		this.ContentPrefabs[typeof(CodexLabelWithIcon)] = this.prefabLabelWithIcon;
		this.ContentPrefabs[typeof(CodexLabelWithLargeIcon)] = this.prefabLabelWithLargeIcon;
		this.ContentPrefabs[typeof(CodexContentLockedIndicator)] = this.prefabContentLocked;
		this.ContentPrefabs[typeof(CodexLargeSpacer)] = this.prefabLargeSpacer;
		this.ContentPrefabs[typeof(CodexVideo)] = this.prefabVideoWidget;
		this.ContentPrefabs[typeof(CodexIndentedLabelWithIcon)] = this.prefabIndentedLabelWithIcon;
		this.ContentPrefabs[typeof(CodexRecipePanel)] = this.prefabRecipePanel;
		this.ContentPrefabs[typeof(CodexConfigurableConsumerRecipePanel)] = this.PrefabConfigurableConsumerRecipePanel;
		this.ContentPrefabs[typeof(CodexTemperatureTransitionPanel)] = this.PrefabTemperatureTransitionPanel;
		this.ContentPrefabs[typeof(CodexConversionPanel)] = this.prefabConversionPanel;
		this.ContentPrefabs[typeof(CodexCollapsibleHeader)] = this.prefabCollapsibleHeader;
		this.ContentPrefabs[typeof(CodexCritterLifecycleWidget)] = this.prefabCritterLifecycleWidget;
		this.ContentPrefabs[typeof(CodexElementCategoryList)] = this.prefabElementCategoryList;
	}

	// Token: 0x06005E20 RID: 24096 RVA: 0x0022F784 File Offset: 0x0022D984
	private List<CodexEntry> FilterSearch(string input)
	{
		this.searchResults.Clear();
		this.subEntrySearchResults.Clear();
		input = input.ToLower();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
		{
			if (SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(keyValuePair.Value.GetDlcIds(), keyValuePair.Value.GetForbiddenDLCs()))
			{
				if (input == "")
				{
					if (!keyValuePair.Value.searchOnly)
					{
						this.searchResults.Add(keyValuePair.Value);
					}
				}
				else if (input == keyValuePair.Value.name.ToLower() || input.Contains(keyValuePair.Value.name.ToLower()) || keyValuePair.Value.name.ToLower().Contains(input))
				{
					this.searchResults.Add(keyValuePair.Value);
				}
			}
		}
		foreach (KeyValuePair<string, SubEntry> keyValuePair2 in CodexCache.subEntries)
		{
			if (input == keyValuePair2.Value.name.ToLower() || input.Contains(keyValuePair2.Value.name.ToLower()) || keyValuePair2.Value.name.ToLower().Contains(input))
			{
				this.subEntrySearchResults.Add(keyValuePair2.Value);
			}
		}
		this.FilterEntries(input != "");
		return this.searchResults;
	}

	// Token: 0x06005E21 RID: 24097 RVA: 0x0022F954 File Offset: 0x0022DB54
	private bool HasUnlockedCategoryEntries(string entryID)
	{
		foreach (ContentContainer contentContainer in CodexCache.entries[entryID].contentContainers)
		{
			if (string.IsNullOrEmpty(contentContainer.lockID) || Game.Instance.unlocks.IsUnlocked(contentContainer.lockID))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x0022F9D8 File Offset: 0x0022DBD8
	private void FilterEntries(bool allowOpenCategories = true)
	{
		foreach (KeyValuePair<CodexEntry, GameObject> keyValuePair in this.entryButtons)
		{
			keyValuePair.Value.SetActive(this.searchResults.Contains(keyValuePair.Key) && this.HasUnlockedCategoryEntries(keyValuePair.Key.id));
		}
		foreach (KeyValuePair<SubEntry, GameObject> keyValuePair2 in this.subEntryButtons)
		{
			keyValuePair2.Value.SetActive(this.subEntrySearchResults.Contains(keyValuePair2.Key));
		}
		foreach (GameObject gameObject in this.categoryHeaders)
		{
			bool flag = false;
			Transform transform = gameObject.transform.Find("Content");
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).gameObject.activeSelf)
				{
					flag = true;
				}
			}
			gameObject.SetActive(flag);
			if (allowOpenCategories)
			{
				if (flag)
				{
					this.ToggleCategoryOpen(gameObject, true);
				}
			}
			else
			{
				this.ToggleCategoryOpen(gameObject, false);
			}
		}
	}

	// Token: 0x06005E23 RID: 24099 RVA: 0x0022FB58 File Offset: 0x0022DD58
	private void ToggleCategoryOpen(GameObject header, bool open)
	{
		header.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").ChangeState(open ? 1 : 0);
		header.GetComponent<HierarchyReferences>().GetReference("Content").gameObject.SetActive(open);
	}

	// Token: 0x06005E24 RID: 24100 RVA: 0x0022FB94 File Offset: 0x0022DD94
	private void PopulatePools()
	{
		foreach (KeyValuePair<Type, GameObject> keyValuePair in this.ContentPrefabs)
		{
			UIGameObjectPool uigameObjectPool = new UIGameObjectPool(keyValuePair.Value);
			uigameObjectPool.disabledElementParent = this.widgetPool;
			this.ContentUIPools[keyValuePair.Key] = uigameObjectPool;
		}
	}

	// Token: 0x06005E25 RID: 24101 RVA: 0x0022FC0C File Offset: 0x0022DE0C
	private GameObject NewCategoryHeader(KeyValuePair<string, CodexEntry> entryKVP, Dictionary<string, GameObject> categories)
	{
		if (entryKVP.Value.category == "")
		{
			entryKVP.Value.category = "Root";
		}
		GameObject categoryHeader = Util.KInstantiateUI(this.prefabCategoryHeader, this.navigatorContent.gameObject, true);
		GameObject categoryContent = categoryHeader.GetComponent<HierarchyReferences>().GetReference("Content").gameObject;
		categories.Add(entryKVP.Value.category, categoryContent);
		LocText reference = categoryHeader.GetComponent<HierarchyReferences>().GetReference<LocText>("Label");
		if (CodexCache.entries.ContainsKey(entryKVP.Value.category))
		{
			reference.text = CodexCache.entries[entryKVP.Value.category].name;
		}
		else
		{
			reference.text = Strings.Get("STRINGS.UI.CODEX.CATEGORYNAMES." + entryKVP.Value.category.ToUpper());
		}
		this.categoryHeaders.Add(categoryHeader);
		categoryContent.SetActive(false);
		categoryHeader.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("ExpandToggle").onClick = delegate()
		{
			this.ToggleCategoryOpen(categoryHeader, !categoryContent.activeSelf);
		};
		return categoryHeader;
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x0022FD6C File Offset: 0x0022DF6C
	private void CategorizeEntries()
	{
		GameObject gameObject = this.navigatorContent.gameObject;
		Dictionary<string, GameObject> dictionary = new Dictionary<string, GameObject>();
		List<global::Tuple<string, CodexEntry>> list = new List<global::Tuple<string, CodexEntry>>();
		foreach (KeyValuePair<string, CodexEntry> keyValuePair in CodexCache.entries)
		{
			if (string.IsNullOrEmpty(keyValuePair.Value.sortString))
			{
				keyValuePair.Value.sortString = UI.StripLinkFormatting(Strings.Get(keyValuePair.Value.title));
			}
			list.Add(new global::Tuple<string, CodexEntry>(keyValuePair.Key, keyValuePair.Value));
		}
		list.Sort((global::Tuple<string, CodexEntry> a, global::Tuple<string, CodexEntry> b) => string.Compare(a.second.sortString, b.second.sortString));
		for (int i = 0; i < list.Count; i++)
		{
			global::Tuple<string, CodexEntry> tuple = list[i];
			string text = tuple.second.category;
			if (text == "" || text == "Root")
			{
				text = "Root";
			}
			if (!dictionary.ContainsKey(text))
			{
				this.NewCategoryHeader(new KeyValuePair<string, CodexEntry>(tuple.first, tuple.second), dictionary);
			}
			GameObject gameObject2 = Util.KInstantiateUI(this.prefabNavigatorEntry, dictionary[text], true);
			string id = tuple.second.id;
			gameObject2.GetComponent<KButton>().onClick += delegate()
			{
				this.ChangeArticle(id, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
			if (string.IsNullOrEmpty(tuple.second.name))
			{
				tuple.second.name = Strings.Get(tuple.second.title);
			}
			gameObject2.GetComponentInChildren<LocText>().text = tuple.second.name;
			this.entryButtons.Add(tuple.second, gameObject2);
			foreach (SubEntry subEntry in tuple.second.subEntries)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.prefabNavigatorEntry, dictionary[text], true);
				string subEntryId = subEntry.id;
				gameObject3.GetComponent<KButton>().onClick += delegate()
				{
					this.ChangeArticle(subEntryId, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
				};
				if (string.IsNullOrEmpty(subEntry.name))
				{
					subEntry.name = Strings.Get(subEntry.title);
				}
				gameObject3.GetComponentInChildren<LocText>().text = subEntry.name;
				this.subEntryButtons.Add(subEntry, gameObject3);
				CodexCache.subEntries.Add(subEntry.id, subEntry);
			}
		}
		foreach (KeyValuePair<string, CodexEntry> keyValuePair2 in CodexCache.entries)
		{
			if (CodexCache.entries.ContainsKey(keyValuePair2.Value.category) && CodexCache.entries.ContainsKey(CodexCache.entries[keyValuePair2.Value.category].category))
			{
				keyValuePair2.Value.searchOnly = true;
			}
		}
		List<KeyValuePair<string, GameObject>> list2 = new List<KeyValuePair<string, GameObject>>();
		foreach (KeyValuePair<string, GameObject> item in dictionary)
		{
			list2.Add(item);
		}
		list2.Sort((KeyValuePair<string, GameObject> a, KeyValuePair<string, GameObject> b) => string.Compare(a.Value.name, b.Value.name));
		for (int j = 0; j < list2.Count; j++)
		{
			list2[j].Value.transform.parent.SetSiblingIndex(j);
		}
		CodexScreen.SetupCategory(dictionary, "PLANTS");
		CodexScreen.SetupCategory(dictionary, "CREATURES");
		CodexScreen.SetupCategory(dictionary, "NOTICES");
		CodexScreen.SetupCategory(dictionary, "RESEARCHNOTES");
		CodexScreen.SetupCategory(dictionary, "JOURNALS");
		CodexScreen.SetupCategory(dictionary, "EMAILS");
		CodexScreen.SetupCategory(dictionary, "INVESTIGATIONS");
		CodexScreen.SetupCategory(dictionary, "MYLOG");
		CodexScreen.SetupCategory(dictionary, "CREATURES::GeneralInfo");
		CodexScreen.SetupCategory(dictionary, "LESSONS");
		CodexScreen.SetupCategory(dictionary, "Root");
	}

	// Token: 0x06005E27 RID: 24103 RVA: 0x0023020C File Offset: 0x0022E40C
	private static void SetupCategory(Dictionary<string, GameObject> categories, string category_name)
	{
		if (!categories.ContainsKey(category_name))
		{
			return;
		}
		categories[category_name].transform.parent.SetAsFirstSibling();
	}

	// Token: 0x06005E28 RID: 24104 RVA: 0x00230230 File Offset: 0x0022E430
	public void ChangeArticle(string id, bool playClickSound = false, Vector3 targetPosition = default(Vector3), CodexScreen.HistoryDirection historyMovement = CodexScreen.HistoryDirection.NewArticle)
	{
		global::Debug.Assert(id != null);
		if (playClickSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
		}
		if (this.contentContainerPool == null)
		{
			this.CodexScreenInit();
		}
		string text = "";
		SubEntry subEntry = null;
		if (!CodexCache.entries.ContainsKey(id))
		{
			subEntry = CodexCache.FindSubEntry(id);
			if (subEntry != null && !subEntry.disabled)
			{
				id = subEntry.parentEntryID.ToUpper();
				text = UI.StripLinkFormatting(subEntry.name);
			}
			else
			{
				id = "PAGENOTFOUND";
			}
		}
		if (CodexCache.entries[id].disabled)
		{
			id = "PAGENOTFOUND";
		}
		if (string.IsNullOrEmpty(text))
		{
			text = UI.StripLinkFormatting(CodexCache.entries[id].name);
		}
		ICodexWidget codexWidget = null;
		CodexCache.entries[id].GetFirstWidget();
		RectTransform rectTransform = null;
		if (subEntry != null)
		{
			foreach (ContentContainer contentContainer in CodexCache.entries[id].contentContainers)
			{
				if (contentContainer == subEntry.contentContainers[0])
				{
					codexWidget = contentContainer.content[0];
					break;
				}
			}
		}
		int num = 0;
		string text2 = "";
		while (this.contentContainers.transform.childCount > 0)
		{
			while (!string.IsNullOrEmpty(text2) && CodexCache.entries[this.activeEntryID].contentContainers[num].lockID == text2)
			{
				num++;
			}
			GameObject gameObject = this.contentContainers.transform.GetChild(0).gameObject;
			int num2 = 0;
			while (gameObject.transform.childCount > 0)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(0).gameObject;
				Type key;
				if (gameObject2.name == "PrefabContentLocked")
				{
					text2 = CodexCache.entries[this.activeEntryID].contentContainers[num].lockID;
					key = typeof(CodexContentLockedIndicator);
				}
				else
				{
					key = CodexCache.entries[this.activeEntryID].contentContainers[num].content[num2].GetType();
				}
				this.ContentUIPools[key].ClearElement(gameObject2);
				num2++;
			}
			this.contentContainerPool.ClearElement(this.contentContainers.transform.GetChild(0).gameObject);
			num++;
		}
		bool flag = CodexCache.entries[id] is CategoryEntry;
		this.activeEntryID = id;
		if (CodexCache.entries[id].contentContainers == null)
		{
			CodexCache.entries[id].CreateContentContainerCollection();
		}
		bool flag2 = false;
		string a = "";
		for (int i = 0; i < CodexCache.entries[id].contentContainers.Count; i++)
		{
			ContentContainer contentContainer2 = CodexCache.entries[id].contentContainers[i];
			if (!string.IsNullOrEmpty(contentContainer2.lockID) && !Game.Instance.unlocks.IsUnlocked(contentContainer2.lockID))
			{
				if (a != contentContainer2.lockID)
				{
					GameObject gameObject3 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
					this.ConfigureContentContainer(contentContainer2, gameObject3, flag && flag2);
					a = contentContainer2.lockID;
					GameObject gameObject4 = this.ContentUIPools[typeof(CodexContentLockedIndicator)].GetFreeElement(gameObject3, true).gameObject;
				}
			}
			else
			{
				GameObject gameObject3 = this.contentContainerPool.GetFreeElement(this.contentContainers.gameObject, true).gameObject;
				this.ConfigureContentContainer(contentContainer2, gameObject3, flag && flag2);
				flag2 = !flag2;
				if (contentContainer2.content != null)
				{
					foreach (ICodexWidget codexWidget2 in contentContainer2.content)
					{
						GameObject gameObject5 = this.ContentUIPools[codexWidget2.GetType()].GetFreeElement(gameObject3, true).gameObject;
						codexWidget2.Configure(gameObject5, this.displayPane, this.textStyles);
						if (codexWidget2 == codexWidget)
						{
							rectTransform = gameObject5.rectTransform();
						}
					}
				}
			}
		}
		string text3 = "";
		string text4 = id;
		int num3 = 0;
		while (text4 != CodexCache.FormatLinkID("HOME") && num3 < 10)
		{
			num3++;
			if (text4 != null)
			{
				if (text4 != id)
				{
					text3 = text3.Insert(0, CodexCache.entries[text4].name + " > ");
				}
				else
				{
					text3 = text3.Insert(0, CodexCache.entries[text4].name);
				}
				text4 = CodexCache.entries[text4].parentId;
			}
			else
			{
				text4 = CodexCache.entries[CodexCache.FormatLinkID("HOME")].id;
				text3 = text3.Insert(0, CodexCache.entries[text4].name + " > ");
			}
		}
		this.currentLocationText.text = ((text3 == "") ? ("<b>" + UI.StripLinkFormatting(CodexCache.entries["HOME"].name) + "</b>") : text3);
		if (this.history.Count == 0)
		{
			this.history.Add(new CodexScreen.HistoryEntry(id, Vector3.zero, text));
			this.currentHistoryIdx = 0;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.Back)
		{
			this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
			this.currentHistoryIdx--;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.Forward)
		{
			this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
			this.currentHistoryIdx++;
		}
		else if (historyMovement == CodexScreen.HistoryDirection.NewArticle || historyMovement == CodexScreen.HistoryDirection.Up)
		{
			if (this.currentHistoryIdx == this.history.Count - 1)
			{
				this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, text));
				this.history[this.currentHistoryIdx].position = this.displayPane.transform.localPosition;
				this.currentHistoryIdx++;
			}
			else
			{
				for (int j = this.history.Count - 1; j > this.currentHistoryIdx; j--)
				{
					this.history.RemoveAt(j);
				}
				this.history.Add(new CodexScreen.HistoryEntry(this.activeEntryID, Vector3.zero, text));
				this.history[this.history.Count - 2].position = this.displayPane.transform.localPosition;
				this.currentHistoryIdx++;
			}
		}
		if (this.currentHistoryIdx > 0)
		{
			this.backButtonButton.GetComponent<Image>().color = Color.black;
			this.backButton.text = UI.FormatAsLink(string.Format(UI.CODEX.BACK_BUTTON, UI.StripLinkFormatting(CodexCache.entries[this.history[this.history.Count - 2].id].name)), CodexCache.entries[this.history[this.history.Count - 2].id].id);
			this.backButtonButton.GetComponent<ToolTip>().toolTip = string.Format(UI.CODEX.BACK_BUTTON_TOOLTIP, this.history[this.currentHistoryIdx - 1].name);
		}
		else
		{
			this.backButtonButton.GetComponent<Image>().color = Color.grey;
			this.backButton.text = UI.StripLinkFormatting(GameUtil.ColourizeString(Color.grey, string.Format(UI.CODEX.BACK_BUTTON, CodexCache.entries["HOME"].name)));
			this.backButtonButton.GetComponent<ToolTip>().toolTip = UI.CODEX.BACK_BUTTON_NO_HISTORY_TOOLTIP;
		}
		if (this.currentHistoryIdx < this.history.Count - 1)
		{
			this.fwdButtonButton.GetComponent<Image>().color = Color.black;
			this.fwdButtonButton.GetComponent<ToolTip>().toolTip = string.Format(UI.CODEX.FORWARD_BUTTON_TOOLTIP, this.history[this.currentHistoryIdx + 1].name);
		}
		else
		{
			this.fwdButtonButton.GetComponent<Image>().color = Color.grey;
			this.fwdButtonButton.GetComponent<ToolTip>().toolTip = UI.CODEX.FORWARD_BUTTON_NO_HISTORY_TOOLTIP;
		}
		if (targetPosition != Vector3.zero)
		{
			if (this.scrollToTargetRoutine != null)
			{
				base.StopCoroutine(this.scrollToTargetRoutine);
			}
			this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(targetPosition));
			return;
		}
		if (rectTransform != null)
		{
			if (this.scrollToTargetRoutine != null)
			{
				base.StopCoroutine(this.scrollToTargetRoutine);
			}
			this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(rectTransform));
			return;
		}
		this.displayScrollRect.content.SetLocalPosition(Vector3.zero);
	}

	// Token: 0x06005E29 RID: 24105 RVA: 0x00230BC4 File Offset: 0x0022EDC4
	private void HistoryStepBack()
	{
		if (this.currentHistoryIdx == 0)
		{
			return;
		}
		this.ChangeArticle(this.history[this.currentHistoryIdx - 1].id, false, this.history[this.currentHistoryIdx - 1].position, CodexScreen.HistoryDirection.Back);
	}

	// Token: 0x06005E2A RID: 24106 RVA: 0x00230C14 File Offset: 0x0022EE14
	private void HistoryStepForward()
	{
		if (this.currentHistoryIdx == this.history.Count - 1)
		{
			return;
		}
		this.ChangeArticle(this.history[this.currentHistoryIdx + 1].id, false, this.history[this.currentHistoryIdx + 1].position, CodexScreen.HistoryDirection.Forward);
	}

	// Token: 0x06005E2B RID: 24107 RVA: 0x00230C70 File Offset: 0x0022EE70
	private void HistoryStepUp()
	{
		if (string.IsNullOrEmpty(CodexCache.entries[this.activeEntryID].parentId))
		{
			return;
		}
		this.ChangeArticle(CodexCache.entries[this.activeEntryID].parentId, false, default(Vector3), CodexScreen.HistoryDirection.Up);
	}

	// Token: 0x06005E2C RID: 24108 RVA: 0x00230CC0 File Offset: 0x0022EEC0
	private IEnumerator ScrollToTarget(RectTransform targetWidgetTransform)
	{
		yield return 0;
		this.displayScrollRect.content.SetLocalPosition(Vector3.down * (this.displayScrollRect.content.InverseTransformPoint(targetWidgetTransform.GetPosition()).y + 12f));
		this.scrollToTargetRoutine = null;
		yield break;
	}

	// Token: 0x06005E2D RID: 24109 RVA: 0x00230CD6 File Offset: 0x0022EED6
	private IEnumerator ScrollToTarget(Vector3 position)
	{
		yield return 0;
		this.displayScrollRect.content.SetLocalPosition(position);
		this.scrollToTargetRoutine = null;
		yield break;
	}

	// Token: 0x06005E2E RID: 24110 RVA: 0x00230CEC File Offset: 0x0022EEEC
	public void FocusContainer(ContentContainer target)
	{
		if (target == null || target.go == null)
		{
			return;
		}
		RectTransform rectTransform = target.go.transform.GetChild(0) as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		if (this.scrollToTargetRoutine != null)
		{
			base.StopCoroutine(this.scrollToTargetRoutine);
		}
		this.scrollToTargetRoutine = base.StartCoroutine(this.ScrollToTarget(rectTransform));
	}

	// Token: 0x06005E2F RID: 24111 RVA: 0x00230D54 File Offset: 0x0022EF54
	private void ConfigureContentContainer(ContentContainer container, GameObject containerGameObject, bool bgColor = false)
	{
		container.go = containerGameObject;
		LayoutGroup layoutGroup = containerGameObject.GetComponent<LayoutGroup>();
		if (layoutGroup != null)
		{
			UnityEngine.Object.DestroyImmediate(layoutGroup);
		}
		if (!Game.Instance.unlocks.IsUnlocked(container.lockID) && !string.IsNullOrEmpty(container.lockID))
		{
			layoutGroup = containerGameObject.AddComponent<VerticalLayoutGroup>();
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		}
		switch (container.contentLayout)
		{
		case ContentContainer.ContentLayout.Vertical:
			layoutGroup = containerGameObject.AddComponent<VerticalLayoutGroup>();
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		case ContentContainer.ContentLayout.Horizontal:
			layoutGroup = containerGameObject.AddComponent<HorizontalLayoutGroup>();
			layoutGroup.childAlignment = TextAnchor.MiddleLeft;
			(layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandHeight = ((layoutGroup as HorizontalOrVerticalLayoutGroup).childForceExpandWidth = false);
			(layoutGroup as HorizontalOrVerticalLayoutGroup).spacing = 8f;
			return;
		case ContentContainer.ContentLayout.Grid:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 4;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(128f, 180f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(6f, 6f);
			return;
		case ContentContainer.ContentLayout.GridTwoColumn:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 2;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(264f, 32f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(0f, 12f);
			return;
		case ContentContainer.ContentLayout.GridTwoColumnTall:
			layoutGroup = containerGameObject.AddComponent<GridLayoutGroup>();
			(layoutGroup as GridLayoutGroup).constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			(layoutGroup as GridLayoutGroup).constraintCount = 2;
			(layoutGroup as GridLayoutGroup).cellSize = new Vector2(264f, 64f);
			(layoutGroup as GridLayoutGroup).spacing = new Vector2(0f, 12f);
			return;
		default:
			return;
		}
	}

	// Token: 0x04003EDE RID: 16094
	private string _activeEntryID;

	// Token: 0x04003EDF RID: 16095
	private Dictionary<Type, UIGameObjectPool> ContentUIPools = new Dictionary<Type, UIGameObjectPool>();

	// Token: 0x04003EE0 RID: 16096
	private Dictionary<Type, GameObject> ContentPrefabs = new Dictionary<Type, GameObject>();

	// Token: 0x04003EE1 RID: 16097
	private List<GameObject> categoryHeaders = new List<GameObject>();

	// Token: 0x04003EE2 RID: 16098
	private Dictionary<CodexEntry, GameObject> entryButtons = new Dictionary<CodexEntry, GameObject>();

	// Token: 0x04003EE3 RID: 16099
	private Dictionary<SubEntry, GameObject> subEntryButtons = new Dictionary<SubEntry, GameObject>();

	// Token: 0x04003EE4 RID: 16100
	private UIGameObjectPool contentContainerPool;

	// Token: 0x04003EE5 RID: 16101
	[SerializeField]
	private KScrollRect displayScrollRect;

	// Token: 0x04003EE6 RID: 16102
	[SerializeField]
	private RectTransform scrollContentPane;

	// Token: 0x04003EE7 RID: 16103
	private bool editingSearch;

	// Token: 0x04003EE8 RID: 16104
	private List<CodexScreen.HistoryEntry> history = new List<CodexScreen.HistoryEntry>();

	// Token: 0x04003EE9 RID: 16105
	private int currentHistoryIdx;

	// Token: 0x04003EEA RID: 16106
	[Header("Hierarchy")]
	[SerializeField]
	private Transform navigatorContent;

	// Token: 0x04003EEB RID: 16107
	[SerializeField]
	private Transform displayPane;

	// Token: 0x04003EEC RID: 16108
	[SerializeField]
	private Transform contentContainers;

	// Token: 0x04003EED RID: 16109
	[SerializeField]
	private Transform widgetPool;

	// Token: 0x04003EEE RID: 16110
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003EEF RID: 16111
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x04003EF0 RID: 16112
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04003EF1 RID: 16113
	[SerializeField]
	private LocText backButton;

	// Token: 0x04003EF2 RID: 16114
	[SerializeField]
	private KButton backButtonButton;

	// Token: 0x04003EF3 RID: 16115
	[SerializeField]
	private KButton fwdButtonButton;

	// Token: 0x04003EF4 RID: 16116
	[SerializeField]
	private LocText currentLocationText;

	// Token: 0x04003EF5 RID: 16117
	[Header("Prefabs")]
	[SerializeField]
	private GameObject prefabNavigatorEntry;

	// Token: 0x04003EF6 RID: 16118
	[SerializeField]
	private GameObject prefabCategoryHeader;

	// Token: 0x04003EF7 RID: 16119
	[SerializeField]
	private GameObject prefabContentContainer;

	// Token: 0x04003EF8 RID: 16120
	[SerializeField]
	private GameObject prefabTextWidget;

	// Token: 0x04003EF9 RID: 16121
	[SerializeField]
	private GameObject prefabTextWithTooltipWidget;

	// Token: 0x04003EFA RID: 16122
	[SerializeField]
	private GameObject prefabImageWidget;

	// Token: 0x04003EFB RID: 16123
	[SerializeField]
	private GameObject prefabDividerLineWidget;

	// Token: 0x04003EFC RID: 16124
	[SerializeField]
	private GameObject prefabSpacer;

	// Token: 0x04003EFD RID: 16125
	[SerializeField]
	private GameObject prefabLargeSpacer;

	// Token: 0x04003EFE RID: 16126
	[SerializeField]
	private GameObject prefabLabelWithIcon;

	// Token: 0x04003EFF RID: 16127
	[SerializeField]
	private GameObject prefabLabelWithLargeIcon;

	// Token: 0x04003F00 RID: 16128
	[SerializeField]
	private GameObject prefabContentLocked;

	// Token: 0x04003F01 RID: 16129
	[SerializeField]
	private GameObject prefabVideoWidget;

	// Token: 0x04003F02 RID: 16130
	[SerializeField]
	private GameObject prefabIndentedLabelWithIcon;

	// Token: 0x04003F03 RID: 16131
	[SerializeField]
	private GameObject prefabRecipePanel;

	// Token: 0x04003F04 RID: 16132
	[SerializeField]
	private GameObject PrefabConfigurableConsumerRecipePanel;

	// Token: 0x04003F05 RID: 16133
	[SerializeField]
	private GameObject PrefabTemperatureTransitionPanel;

	// Token: 0x04003F06 RID: 16134
	[SerializeField]
	private GameObject prefabConversionPanel;

	// Token: 0x04003F07 RID: 16135
	[SerializeField]
	private GameObject prefabCollapsibleHeader;

	// Token: 0x04003F08 RID: 16136
	[SerializeField]
	private GameObject prefabCritterLifecycleWidget;

	// Token: 0x04003F09 RID: 16137
	[SerializeField]
	private GameObject prefabElementCategoryList;

	// Token: 0x04003F0A RID: 16138
	[Header("Text Styles")]
	[SerializeField]
	private TextStyleSetting textStyleTitle;

	// Token: 0x04003F0B RID: 16139
	[SerializeField]
	private TextStyleSetting textStyleSubtitle;

	// Token: 0x04003F0C RID: 16140
	[SerializeField]
	private TextStyleSetting textStyleBody;

	// Token: 0x04003F0D RID: 16141
	[SerializeField]
	private TextStyleSetting textStyleBodyWhite;

	// Token: 0x04003F0E RID: 16142
	private Dictionary<CodexTextStyle, TextStyleSetting> textStyles = new Dictionary<CodexTextStyle, TextStyleSetting>();

	// Token: 0x04003F0F RID: 16143
	private List<CodexEntry> searchResults = new List<CodexEntry>();

	// Token: 0x04003F10 RID: 16144
	private List<SubEntry> subEntrySearchResults = new List<SubEntry>();

	// Token: 0x04003F11 RID: 16145
	private Coroutine scrollToTargetRoutine;

	// Token: 0x02001CED RID: 7405
	public enum PlanCategory
	{
		// Token: 0x0400857B RID: 34171
		Home,
		// Token: 0x0400857C RID: 34172
		Tips,
		// Token: 0x0400857D RID: 34173
		MyLog,
		// Token: 0x0400857E RID: 34174
		Investigations,
		// Token: 0x0400857F RID: 34175
		Emails,
		// Token: 0x04008580 RID: 34176
		Journals,
		// Token: 0x04008581 RID: 34177
		ResearchNotes,
		// Token: 0x04008582 RID: 34178
		Creatures,
		// Token: 0x04008583 RID: 34179
		Plants,
		// Token: 0x04008584 RID: 34180
		Food,
		// Token: 0x04008585 RID: 34181
		Tech,
		// Token: 0x04008586 RID: 34182
		Diseases,
		// Token: 0x04008587 RID: 34183
		Roles,
		// Token: 0x04008588 RID: 34184
		Buildings,
		// Token: 0x04008589 RID: 34185
		Elements
	}

	// Token: 0x02001CEE RID: 7406
	public enum HistoryDirection
	{
		// Token: 0x0400858B RID: 34187
		Back,
		// Token: 0x0400858C RID: 34188
		Forward,
		// Token: 0x0400858D RID: 34189
		Up,
		// Token: 0x0400858E RID: 34190
		NewArticle
	}

	// Token: 0x02001CEF RID: 7407
	public class HistoryEntry
	{
		// Token: 0x0600A73D RID: 42813 RVA: 0x00399F10 File Offset: 0x00398110
		public HistoryEntry(string entry, Vector3 pos, string articleName)
		{
			this.id = entry;
			this.position = pos;
			this.name = articleName;
		}

		// Token: 0x0400858F RID: 34191
		public string id;

		// Token: 0x04008590 RID: 34192
		public Vector3 position;

		// Token: 0x04008591 RID: 34193
		public string name;
	}
}
