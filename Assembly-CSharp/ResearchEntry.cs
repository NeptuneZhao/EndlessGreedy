using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000D21 RID: 3361
[AddComponentMenu("KMonoBehaviour/scripts/ResearchEntry")]
public class ResearchEntry : KMonoBehaviour
{
	// Token: 0x060068FE RID: 26878 RVA: 0x00275088 File Offset: 0x00273288
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techLineMap = new Dictionary<Tech, UILineRenderer>();
		this.BG.color = this.defaultColor;
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			float num = this.targetTech.width / 2f + 18f;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (tech.center.y > this.targetTech.center.y + 2f)
			{
				zero = new Vector2(0f, 20f);
				zero2 = new Vector2(0f, -20f);
			}
			else if (tech.center.y < this.targetTech.center.y - 2f)
			{
				zero = new Vector2(0f, -20f);
				zero2 = new Vector2(0f, 20f);
			}
			UILineRenderer component = Util.KInstantiateUI(this.linePrefab, this.lineContainer.gameObject, true).GetComponent<UILineRenderer>();
			float num2 = 32f;
			component.Points = new Vector2[]
			{
				new Vector2(0f, 0f) + zero,
				new Vector2(-num2, 0f) + zero,
				new Vector2(-num2, tech.center.y - this.targetTech.center.y) + zero2,
				new Vector2(-(this.targetTech.center.x - num - (tech.center.x + num)) + 2f, tech.center.y - this.targetTech.center.y) + zero2
			};
			component.LineThickness = (float)this.lineThickness_inactive;
			component.color = this.inactiveLineColor;
			this.techLineMap.Add(tech, component);
		}
		this.QueueStateChanged(false);
		if (this.targetTech != null)
		{
			using (List<TechInstance>.Enumerator enumerator2 = Research.Instance.GetResearchQueue().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.tech == this.targetTech)
					{
						this.QueueStateChanged(true);
					}
				}
			}
		}
	}

	// Token: 0x060068FF RID: 26879 RVA: 0x00275348 File Offset: 0x00273548
	public void SetTech(Tech newTech)
	{
		if (newTech == null)
		{
			global::Debug.LogError("The research provided is null!");
			return;
		}
		if (this.targetTech == newTech)
		{
			return;
		}
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (newTech.costsByResearchTypeID.ContainsKey(researchType.id) && newTech.costsByResearchTypeID[researchType.id] > 0f)
			{
				GameObject gameObject = Util.KInstantiateUI(this.progressBarPrefab, this.progressBarContainer.gameObject, true);
				Image image = gameObject.GetComponentsInChildren<Image>()[2];
				Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
				image.color = researchType.color;
				component.sprite = researchType.sprite;
				this.progressBarsByResearchTypeID[researchType.id] = gameObject;
			}
		}
		if (this.researchScreen == null)
		{
			this.researchScreen = base.transform.parent.GetComponentInParent<ResearchScreen>();
		}
		if (newTech.IsComplete())
		{
			this.ResearchCompleted(false);
		}
		this.targetTech = newTech;
		this.researchName.text = this.targetTech.Name;
		string text = "";
		foreach (TechItem techItem in this.targetTech.unlockedItems)
		{
			if (SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(techItem.requiredDlcIds, techItem.forbiddenDlcIds))
			{
				HierarchyReferences component2 = this.GetFreeIcon().GetComponent<HierarchyReferences>();
				if (text != "")
				{
					text += ", ";
				}
				text += techItem.Name;
				component2.GetReference<KImage>("Icon").sprite = techItem.UISprite();
				component2.GetReference<KImage>("Background");
				KImage reference = component2.GetReference<KImage>("DLCOverlay");
				bool flag = techItem.requiredDlcIds != null;
				reference.gameObject.SetActive(flag);
				if (flag)
				{
					reference.color = DlcManager.GetDlcBannerColor(techItem.requiredDlcIds[techItem.requiredDlcIds.Length - 1]);
				}
				string text2 = string.Format("{0}\n{1}", techItem.Name, techItem.description);
				if (flag)
				{
					text2 += "\n";
					foreach (string dlcId in techItem.requiredDlcIds)
					{
						text2 += string.Format(RESEARCH.MESSAGING.DLC.DLC_CONTENT, DlcManager.GetDlcTitle(dlcId));
					}
				}
				component2.GetComponent<ToolTip>().toolTip = text2;
			}
		}
		text = string.Format(UI.RESEARCHSCREEN_UNLOCKSTOOLTIP, text);
		this.researchName.GetComponent<ToolTip>().toolTip = string.Format("{0}\n{1}\n\n{2}", this.targetTech.Name, this.targetTech.desc, text);
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.toggle.onPointerEnter += delegate()
		{
			this.researchScreen.TurnEverythingOff();
			this.OnHover(true, this.targetTech);
		};
		this.toggle.soundPlayer.AcceptClickCondition = (() => !this.targetTech.IsComplete());
		this.toggle.onPointerExit += delegate()
		{
			this.researchScreen.TurnEverythingOff();
		};
	}

	// Token: 0x06006900 RID: 26880 RVA: 0x002756E8 File Offset: 0x002738E8
	public void SetEverythingOff()
	{
		if (!this.isOn)
		{
			return;
		}
		this.borderHighlight.gameObject.SetActive(false);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_inactive;
			keyValuePair.Value.color = this.inactiveLineColor;
		}
		this.isOn = false;
	}

	// Token: 0x06006901 RID: 26881 RVA: 0x0027577C File Offset: 0x0027397C
	public void SetEverythingOn()
	{
		if (this.isOn)
		{
			return;
		}
		this.UpdateProgressBars();
		this.borderHighlight.gameObject.SetActive(true);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_active;
			keyValuePair.Value.color = this.activeLineColor;
		}
		base.transform.SetAsLastSibling();
		this.isOn = true;
	}

	// Token: 0x06006902 RID: 26882 RVA: 0x00275820 File Offset: 0x00273A20
	public void OnHover(bool entered, Tech hoverSource)
	{
		this.SetEverythingOn();
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			ResearchEntry entry = this.researchScreen.GetEntry(tech);
			if (entry != null)
			{
				entry.OnHover(entered, this.targetTech);
			}
		}
	}

	// Token: 0x06006903 RID: 26883 RVA: 0x0027589C File Offset: 0x00273A9C
	private void OnResearchClicked()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null && activeResearch.tech != this.targetTech)
		{
			this.researchScreen.CancelResearch();
		}
		Research.Instance.SetActiveResearch(this.targetTech, true);
		if (DebugHandler.InstantBuildMode)
		{
			Research.Instance.CompleteQueue();
		}
		this.UpdateProgressBars();
	}

	// Token: 0x06006904 RID: 26884 RVA: 0x002758F8 File Offset: 0x00273AF8
	private void OnResearchCanceled()
	{
		if (this.targetTech.IsComplete())
		{
			return;
		}
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.researchScreen.CancelResearch();
		Research.Instance.CancelResearch(this.targetTech, true);
	}

	// Token: 0x06006905 RID: 26885 RVA: 0x00275954 File Offset: 0x00273B54
	public void QueueStateChanged(bool isSelected)
	{
		if (isSelected)
		{
			if (!this.targetTech.IsComplete())
			{
				this.toggle.isOn = true;
				this.BG.color = this.pendingColor;
				this.titleBG.color = this.pendingHeaderColor;
				this.toggle.ClearOnClick();
				this.toggle.onClick += this.OnResearchCanceled;
			}
			else
			{
				this.toggle.isOn = false;
			}
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
			{
				keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		if (this.targetTech.IsComplete())
		{
			this.toggle.isOn = false;
			this.BG.color = this.completedColor;
			this.titleBG.color = this.completedHeaderColor;
			this.defaultColor = this.completedColor;
			this.toggle.ClearOnClick();
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.progressBarsByResearchTypeID)
			{
				keyValuePair2.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		this.toggle.isOn = false;
		this.BG.color = this.defaultColor;
		this.titleBG.color = this.incompleteHeaderColor;
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.progressBarsByResearchTypeID)
		{
			keyValuePair3.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = new Color(0.52156866f, 0.52156866f, 0.52156866f);
		}
	}

	// Token: 0x06006906 RID: 26886 RVA: 0x00275BF8 File Offset: 0x00273DF8
	public void UpdateFilterState(bool state)
	{
		this.filterLowlight.gameObject.SetActive(!state);
	}

	// Token: 0x06006907 RID: 26887 RVA: 0x00275C1B File Offset: 0x00273E1B
	public void SetPercentage(float percent)
	{
	}

	// Token: 0x06006908 RID: 26888 RVA: 0x00275C20 File Offset: 0x00273E20
	public void UpdateProgressBars()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
		{
			Transform child = keyValuePair.Value.transform.GetChild(0);
			float fillAmount;
			if (this.targetTech.IsComplete())
			{
				fillAmount = 1f;
				child.GetComponentInChildren<LocText>().text = this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
			}
			else
			{
				TechInstance orAdd = Research.Instance.GetOrAdd(this.targetTech);
				if (orAdd == null)
				{
					continue;
				}
				child.GetComponentInChildren<LocText>().text = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
				fillAmount = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key] / this.targetTech.costsByResearchTypeID[keyValuePair.Key];
			}
			child.GetComponentsInChildren<Image>()[2].fillAmount = fillAmount;
			child.GetComponent<ToolTip>().SetSimpleTooltip(Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).description);
		}
	}

	// Token: 0x06006909 RID: 26889 RVA: 0x00275DD8 File Offset: 0x00273FD8
	private GameObject GetFreeIcon()
	{
		GameObject gameObject = Util.KInstantiateUI(this.iconPrefab, this.iconPanel, false);
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600690A RID: 26890 RVA: 0x00275DF3 File Offset: 0x00273FF3
	private Image GetFreeLine()
	{
		return Util.KInstantiateUI<Image>(this.linePrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x0600690B RID: 26891 RVA: 0x00275E0C File Offset: 0x0027400C
	public void ResearchCompleted(bool notify = true)
	{
		this.BG.color = this.completedColor;
		this.titleBG.color = this.completedHeaderColor;
		this.defaultColor = this.completedColor;
		if (notify)
		{
			this.unlockedTechMetric[ResearchEntry.UnlockedTechKey] = this.targetTech.Id;
			ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedTechMetric, "ResearchCompleted");
		}
		this.toggle.ClearOnClick();
		if (notify)
		{
			ResearchCompleteMessage message = new ResearchCompleteMessage(this.targetTech);
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x04004724 RID: 18212
	[Header("Labels")]
	[SerializeField]
	private LocText researchName;

	// Token: 0x04004725 RID: 18213
	[Header("Transforms")]
	[SerializeField]
	private Transform progressBarContainer;

	// Token: 0x04004726 RID: 18214
	[SerializeField]
	private Transform lineContainer;

	// Token: 0x04004727 RID: 18215
	[Header("Prefabs")]
	[SerializeField]
	private GameObject iconPanel;

	// Token: 0x04004728 RID: 18216
	[SerializeField]
	private GameObject iconPrefab;

	// Token: 0x04004729 RID: 18217
	[SerializeField]
	private GameObject linePrefab;

	// Token: 0x0400472A RID: 18218
	[SerializeField]
	private GameObject progressBarPrefab;

	// Token: 0x0400472B RID: 18219
	[Header("Graphics")]
	[SerializeField]
	private Image BG;

	// Token: 0x0400472C RID: 18220
	[SerializeField]
	private Image titleBG;

	// Token: 0x0400472D RID: 18221
	[SerializeField]
	private Image borderHighlight;

	// Token: 0x0400472E RID: 18222
	[SerializeField]
	private Image filterHighlight;

	// Token: 0x0400472F RID: 18223
	[SerializeField]
	private Image filterLowlight;

	// Token: 0x04004730 RID: 18224
	[SerializeField]
	private Sprite hoverBG;

	// Token: 0x04004731 RID: 18225
	[SerializeField]
	private Sprite completedBG;

	// Token: 0x04004732 RID: 18226
	[Header("Colors")]
	[SerializeField]
	private Color defaultColor = Color.blue;

	// Token: 0x04004733 RID: 18227
	[SerializeField]
	private Color completedColor = Color.yellow;

	// Token: 0x04004734 RID: 18228
	[SerializeField]
	private Color pendingColor = Color.magenta;

	// Token: 0x04004735 RID: 18229
	[SerializeField]
	private Color completedHeaderColor = Color.grey;

	// Token: 0x04004736 RID: 18230
	[SerializeField]
	private Color incompleteHeaderColor = Color.grey;

	// Token: 0x04004737 RID: 18231
	[SerializeField]
	private Color pendingHeaderColor = Color.grey;

	// Token: 0x04004738 RID: 18232
	private Sprite defaultBG;

	// Token: 0x04004739 RID: 18233
	[MyCmpGet]
	private KToggle toggle;

	// Token: 0x0400473A RID: 18234
	private ResearchScreen researchScreen;

	// Token: 0x0400473B RID: 18235
	private Dictionary<Tech, UILineRenderer> techLineMap;

	// Token: 0x0400473C RID: 18236
	private Tech targetTech;

	// Token: 0x0400473D RID: 18237
	private bool isOn = true;

	// Token: 0x0400473E RID: 18238
	private Coroutine fadeRoutine;

	// Token: 0x0400473F RID: 18239
	public Color activeLineColor;

	// Token: 0x04004740 RID: 18240
	public Color inactiveLineColor;

	// Token: 0x04004741 RID: 18241
	public int lineThickness_active = 6;

	// Token: 0x04004742 RID: 18242
	public int lineThickness_inactive = 2;

	// Token: 0x04004743 RID: 18243
	public Material StandardUIMaterial;

	// Token: 0x04004744 RID: 18244
	private Dictionary<string, GameObject> progressBarsByResearchTypeID = new Dictionary<string, GameObject>();

	// Token: 0x04004745 RID: 18245
	public static readonly string UnlockedTechKey = "UnlockedTech";

	// Token: 0x04004746 RID: 18246
	private Dictionary<string, object> unlockedTechMetric = new Dictionary<string, object>
	{
		{
			ResearchEntry.UnlockedTechKey,
			null
		}
	};
}
