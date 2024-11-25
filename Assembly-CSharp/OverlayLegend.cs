﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D07 RID: 3335
public class OverlayLegend : KScreen
{
	// Token: 0x060067A5 RID: 26533 RVA: 0x0026B318 File Offset: 0x00269518
	[ContextMenu("Set all fonts color")]
	public void SetAllFontsColor()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].fontColor == Color.clear)
				{
					overlayInfo.infoUnits[i].fontColor = Color.white;
				}
			}
		}
	}

	// Token: 0x060067A6 RID: 26534 RVA: 0x0026B3B0 File Offset: 0x002695B0
	[ContextMenu("Set all tooltips")]
	public void SetAllTooltips()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			string text = overlayInfo.name;
			text = text.Replace("NAME", "");
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				string text2 = overlayInfo.infoUnits[i].description;
				text2 = text2.Replace(text, "");
				text2 = text + "TOOLTIPS." + text2;
				overlayInfo.infoUnits[i].tooltip = text2;
			}
		}
	}

	// Token: 0x060067A7 RID: 26535 RVA: 0x0026B474 File Offset: 0x00269674
	[ContextMenu("Set Sliced for empty icons")]
	public void SetSlicedForEmptyIcons()
	{
		foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
		{
			for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
			{
				if (overlayInfo.infoUnits[i].icon == this.emptySprite)
				{
					overlayInfo.infoUnits[i].sliceIcon = true;
				}
			}
		}
	}

	// Token: 0x060067A8 RID: 26536 RVA: 0x0026B508 File Offset: 0x00269708
	protected override void OnSpawn()
	{
		base.ConsumeMouseScroll = true;
		base.OnSpawn();
		if (OverlayLegend.Instance == null)
		{
			OverlayLegend.Instance = this;
			this.activeUnitObjs = new List<GameObject>();
			this.inactiveUnitObjs = new List<GameObject>();
			foreach (OverlayLegend.OverlayInfo overlayInfo in this.overlayInfoList)
			{
				overlayInfo.name = Strings.Get(overlayInfo.name);
				for (int i = 0; i < overlayInfo.infoUnits.Count; i++)
				{
					overlayInfo.infoUnits[i].description = Strings.Get(overlayInfo.infoUnits[i].description);
					if (!string.IsNullOrEmpty(overlayInfo.infoUnits[i].tooltip))
					{
						overlayInfo.infoUnits[i].tooltip = Strings.Get(overlayInfo.infoUnits[i].tooltip);
					}
				}
			}
			base.GetComponent<LayoutElement>().minWidth = (float)(DlcManager.FeatureClusterSpaceEnabled() ? 322 : 288);
			this.ClearLegend();
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060067A9 RID: 26537 RVA: 0x0026B664 File Offset: 0x00269864
	protected override void OnLoadLevel()
	{
		OverlayLegend.Instance = null;
		this.activeDiagrams.Clear();
		UnityEngine.Object.Destroy(base.gameObject);
		base.OnLoadLevel();
	}

	// Token: 0x060067AA RID: 26538 RVA: 0x0026B688 File Offset: 0x00269888
	private void SetLegend(OverlayLegend.OverlayInfo overlayInfo)
	{
		if (overlayInfo == null)
		{
			this.ClearLegend();
			return;
		}
		if (!overlayInfo.isProgrammaticallyPopulated && (overlayInfo.infoUnits == null || overlayInfo.infoUnits.Count == 0))
		{
			this.ClearLegend();
			return;
		}
		this.Show(true);
		this.title.text = overlayInfo.name;
		if (overlayInfo.isProgrammaticallyPopulated)
		{
			this.PopulateGeneratedLegend(overlayInfo, false);
		}
		else
		{
			this.PopulateOverlayInfoUnits(overlayInfo, false);
			this.PopulateOverlayDiagrams(overlayInfo, false);
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x060067AB RID: 26539 RVA: 0x0026B704 File Offset: 0x00269904
	public void SetLegend(OverlayModes.Mode mode, bool refreshing = false)
	{
		if (this.currentMode != null && this.currentMode.ViewMode() == mode.ViewMode() && !refreshing)
		{
			return;
		}
		this.ClearLegend();
		OverlayLegend.OverlayInfo legend = this.overlayInfoList.Find((OverlayLegend.OverlayInfo ol) => ol.mode == mode.ViewMode());
		this.currentMode = mode;
		this.SetLegend(legend);
	}

	// Token: 0x060067AC RID: 26540 RVA: 0x0026B778 File Offset: 0x00269978
	public GameObject GetFreeUnitObject()
	{
		if (this.inactiveUnitObjs.Count == 0)
		{
			this.inactiveUnitObjs.Add(Util.KInstantiateUI(this.unitPrefab, this.inactiveUnitsParent, false));
		}
		GameObject gameObject = this.inactiveUnitObjs[0];
		this.inactiveUnitObjs.RemoveAt(0);
		this.activeUnitObjs.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060067AD RID: 26541 RVA: 0x0026B7D8 File Offset: 0x002699D8
	private void RemoveActiveObjects()
	{
		while (this.activeUnitObjs.Count > 0)
		{
			this.activeUnitObjs[0].transform.Find("Icon").GetComponent<Image>().enabled = false;
			this.activeUnitObjs[0].GetComponentInChildren<LocText>().enabled = false;
			this.activeUnitObjs[0].transform.SetParent(this.inactiveUnitsParent.transform);
			this.activeUnitObjs[0].SetActive(false);
			this.inactiveUnitObjs.Add(this.activeUnitObjs[0]);
			this.activeUnitObjs.RemoveAt(0);
		}
	}

	// Token: 0x060067AE RID: 26542 RVA: 0x0026B88E File Offset: 0x00269A8E
	public void ClearLegend()
	{
		this.RemoveActiveObjects();
		this.ClearFilters();
		this.ClearDiagrams();
		this.Show(false);
	}

	// Token: 0x060067AF RID: 26543 RVA: 0x0026B8A9 File Offset: 0x00269AA9
	public void ClearFilters()
	{
		if (this.filterMenu != null)
		{
			UnityEngine.Object.Destroy(this.filterMenu.gameObject);
		}
		this.filterMenu = null;
	}

	// Token: 0x060067B0 RID: 26544 RVA: 0x0026B8D0 File Offset: 0x00269AD0
	public void ClearDiagrams()
	{
		for (int i = 0; i < this.activeDiagrams.Count; i++)
		{
			if (this.activeDiagrams[i] != null)
			{
				UnityEngine.Object.Destroy(this.activeDiagrams[i]);
			}
		}
		this.activeDiagrams.Clear();
		Vector2 sizeDelta = this.diagramsParent.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = 0f;
		this.diagramsParent.GetComponent<RectTransform>().sizeDelta = sizeDelta;
	}

	// Token: 0x060067B1 RID: 26545 RVA: 0x0026B954 File Offset: 0x00269B54
	public OverlayLegend.OverlayInfo GetOverlayInfo(OverlayModes.Mode mode)
	{
		for (int i = 0; i < this.overlayInfoList.Count; i++)
		{
			if (this.overlayInfoList[i].mode == mode.ViewMode())
			{
				return this.overlayInfoList[i];
			}
		}
		return null;
	}

	// Token: 0x060067B2 RID: 26546 RVA: 0x0026B9A4 File Offset: 0x00269BA4
	private void PopulateOverlayInfoUnits(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (overlayInfo.infoUnits != null && overlayInfo.infoUnits.Count > 0)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<OverlayLegend.OverlayInfoUnit>.Enumerator enumerator = overlayInfo.infoUnits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OverlayLegend.OverlayInfoUnit overlayInfoUnit = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					if (overlayInfoUnit.icon != null)
					{
						Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
						component.gameObject.SetActive(true);
						component.sprite = overlayInfoUnit.icon;
						component.color = overlayInfoUnit.color;
						component.enabled = true;
						component.type = (overlayInfoUnit.sliceIcon ? Image.Type.Sliced : Image.Type.Simple);
					}
					else
					{
						freeUnitObject.transform.Find("Icon").gameObject.SetActive(false);
					}
					if (!string.IsNullOrEmpty(overlayInfoUnit.description))
					{
						LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
						componentInChildren.text = string.Format(overlayInfoUnit.description, overlayInfoUnit.formatData);
						componentInChildren.color = overlayInfoUnit.fontColor;
						componentInChildren.enabled = true;
					}
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					if (!string.IsNullOrEmpty(overlayInfoUnit.tooltip))
					{
						component2.toolTip = string.Format(overlayInfoUnit.tooltip, overlayInfoUnit.tooltipFormatData);
						component2.enabled = true;
					}
					else
					{
						component2.enabled = false;
					}
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				return;
			}
		}
		this.activeUnitsParent.SetActive(false);
	}

	// Token: 0x060067B3 RID: 26547 RVA: 0x0026BB50 File Offset: 0x00269D50
	private void PopulateOverlayDiagrams(OverlayLegend.OverlayInfo overlayInfo, bool isRefresh = false)
	{
		if (!isRefresh)
		{
			if (overlayInfo.mode == OverlayModes.Temperature.ID)
			{
				Game.TemperatureOverlayModes temperatureOverlayMode = Game.Instance.temperatureOverlayMode;
				if (temperatureOverlayMode != Game.TemperatureOverlayModes.AbsoluteTemperature)
				{
					if (temperatureOverlayMode == Game.TemperatureOverlayModes.RelativeTemperature)
					{
						this.ClearDiagrams();
						overlayInfo = this.overlayInfoList.Find((OverlayLegend.OverlayInfo match) => match.name == UI.OVERLAYS.RELATIVETEMPERATURE.NAME);
					}
				}
				else
				{
					SimDebugView.Instance.user_temperatureThresholds[0] = 0f;
					SimDebugView.Instance.user_temperatureThresholds[1] = 2073f;
				}
			}
			if (overlayInfo.diagrams != null && overlayInfo.diagrams.Count > 0)
			{
				this.diagramsParent.SetActive(true);
				using (List<GameObject>.Enumerator enumerator = overlayInfo.diagrams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject original = enumerator.Current;
						GameObject item = Util.KInstantiateUI(original, this.diagramsParent, false);
						this.activeDiagrams.Add(item);
					}
					return;
				}
			}
			this.diagramsParent.SetActive(false);
		}
	}

	// Token: 0x060067B4 RID: 26548 RVA: 0x0026BC6C File Offset: 0x00269E6C
	private void PopulateGeneratedLegend(OverlayLegend.OverlayInfo info, bool isRefresh = false)
	{
		if (isRefresh)
		{
			this.RemoveActiveObjects();
			this.ClearDiagrams();
		}
		if (info.infoUnits != null && info.infoUnits.Count > 0)
		{
			this.PopulateOverlayInfoUnits(info, isRefresh);
		}
		this.PopulateOverlayDiagrams(info, false);
		List<LegendEntry> customLegendData = this.currentMode.GetCustomLegendData();
		if (customLegendData != null)
		{
			this.activeUnitsParent.SetActive(true);
			using (List<LegendEntry>.Enumerator enumerator = customLegendData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LegendEntry legendEntry = enumerator.Current;
					GameObject freeUnitObject = this.GetFreeUnitObject();
					Image component = freeUnitObject.transform.Find("Icon").GetComponent<Image>();
					component.gameObject.SetActive(legendEntry.displaySprite);
					component.sprite = legendEntry.sprite;
					component.color = legendEntry.colour;
					component.enabled = true;
					component.type = Image.Type.Simple;
					LocText componentInChildren = freeUnitObject.GetComponentInChildren<LocText>();
					componentInChildren.text = legendEntry.name;
					componentInChildren.color = Color.white;
					componentInChildren.enabled = true;
					ToolTip component2 = freeUnitObject.GetComponent<ToolTip>();
					component2.enabled = (legendEntry.desc != null || legendEntry.desc_arg != null);
					component2.toolTip = ((legendEntry.desc_arg == null) ? legendEntry.desc : string.Format(legendEntry.desc, legendEntry.desc_arg));
					freeUnitObject.SetActive(true);
					freeUnitObject.transform.SetParent(this.activeUnitsParent.transform);
				}
				goto IL_165;
			}
		}
		this.activeUnitsParent.SetActive(false);
		IL_165:
		if (!isRefresh && this.currentMode.legendFilters != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.toolParameterMenuPrefab, this.diagramsParent.transform.parent.gameObject, false);
			gameObject.transform.SetAsFirstSibling();
			this.filterMenu = gameObject.GetComponent<ToolParameterMenu>();
			this.filterMenu.PopulateMenu(this.currentMode.legendFilters);
			this.filterMenu.onParametersChanged += this.OnFiltersChanged;
			this.OnFiltersChanged();
		}
		this.ConfigureUIHeight();
	}

	// Token: 0x060067B5 RID: 26549 RVA: 0x0026BE70 File Offset: 0x0026A070
	private void OnFiltersChanged()
	{
		this.currentMode.OnFiltersChanged();
		this.PopulateGeneratedLegend(this.GetOverlayInfo(this.currentMode), true);
		Game.Instance.ForceOverlayUpdate(false);
	}

	// Token: 0x060067B6 RID: 26550 RVA: 0x0026BE9B File Offset: 0x0026A09B
	private void DisableOverlay()
	{
		this.filterMenu.onParametersChanged -= this.OnFiltersChanged;
		this.filterMenu.ClearMenu();
		this.filterMenu.gameObject.SetActive(false);
		this.filterMenu = null;
	}

	// Token: 0x060067B7 RID: 26551 RVA: 0x0026BED8 File Offset: 0x0026A0D8
	private void ConfigureUIHeight()
	{
		this.scrollRectLayout.enabled = false;
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
		this.scrollRectLayout.preferredWidth = this.scrollRectLayout.rectTransform().sizeDelta.x;
		float y = this.scrollRectLayout.rectTransform().sizeDelta.y;
		this.scrollRectLayout.preferredHeight = Mathf.Min(y, 512f);
		this.scrollRectLayout.GetComponent<VerticalLayoutGroup>().enabled = false;
		this.scrollRectLayout.enabled = true;
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.gameObject.rectTransform());
	}

	// Token: 0x040045FC RID: 17916
	public static OverlayLegend Instance;

	// Token: 0x040045FD RID: 17917
	[SerializeField]
	private LocText title;

	// Token: 0x040045FE RID: 17918
	[SerializeField]
	private Sprite emptySprite;

	// Token: 0x040045FF RID: 17919
	[SerializeField]
	private List<OverlayLegend.OverlayInfo> overlayInfoList;

	// Token: 0x04004600 RID: 17920
	[SerializeField]
	private GameObject unitPrefab;

	// Token: 0x04004601 RID: 17921
	[SerializeField]
	private GameObject activeUnitsParent;

	// Token: 0x04004602 RID: 17922
	[SerializeField]
	private GameObject diagramsParent;

	// Token: 0x04004603 RID: 17923
	[SerializeField]
	private GameObject inactiveUnitsParent;

	// Token: 0x04004604 RID: 17924
	[SerializeField]
	private GameObject toolParameterMenuPrefab;

	// Token: 0x04004605 RID: 17925
	[SerializeField]
	private LayoutElement scrollRectLayout;

	// Token: 0x04004606 RID: 17926
	private ToolParameterMenu filterMenu;

	// Token: 0x04004607 RID: 17927
	private OverlayModes.Mode currentMode;

	// Token: 0x04004608 RID: 17928
	private List<GameObject> inactiveUnitObjs;

	// Token: 0x04004609 RID: 17929
	private List<GameObject> activeUnitObjs;

	// Token: 0x0400460A RID: 17930
	private List<GameObject> activeDiagrams = new List<GameObject>();

	// Token: 0x02001E22 RID: 7714
	[Serializable]
	public class OverlayInfoUnit
	{
		// Token: 0x0600AAA4 RID: 43684 RVA: 0x003A2E78 File Offset: 0x003A1078
		public OverlayInfoUnit(Sprite icon, string description, Color color, Color fontColor, object formatData = null, bool sliceIcon = false)
		{
			this.icon = icon;
			this.description = description;
			this.color = color;
			this.fontColor = fontColor;
			this.formatData = formatData;
			this.sliceIcon = sliceIcon;
		}

		// Token: 0x04008976 RID: 35190
		public Sprite icon;

		// Token: 0x04008977 RID: 35191
		public string description;

		// Token: 0x04008978 RID: 35192
		public string tooltip;

		// Token: 0x04008979 RID: 35193
		public Color color;

		// Token: 0x0400897A RID: 35194
		public Color fontColor;

		// Token: 0x0400897B RID: 35195
		public object formatData;

		// Token: 0x0400897C RID: 35196
		public object tooltipFormatData;

		// Token: 0x0400897D RID: 35197
		public bool sliceIcon;
	}

	// Token: 0x02001E23 RID: 7715
	[Serializable]
	public class OverlayInfo
	{
		// Token: 0x0400897E RID: 35198
		public string name;

		// Token: 0x0400897F RID: 35199
		public HashedString mode;

		// Token: 0x04008980 RID: 35200
		public List<OverlayLegend.OverlayInfoUnit> infoUnits;

		// Token: 0x04008981 RID: 35201
		public List<GameObject> diagrams;

		// Token: 0x04008982 RID: 35202
		public bool isProgrammaticallyPopulated;
	}
}
