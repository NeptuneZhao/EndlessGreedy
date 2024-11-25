using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CA4 RID: 3236
public class CrewListScreen<EntryType> : KScreen where EntryType : CrewListEntry
{
	// Token: 0x060063B7 RID: 25527 RVA: 0x00252D9C File Offset: 0x00250F9C
	protected override void OnActivate()
	{
		base.OnActivate();
		this.ClearEntries();
		this.SpawnEntries();
		this.PositionColumnTitles();
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x060063B8 RID: 25528 RVA: 0x00252DCB File Offset: 0x00250FCB
	protected override void OnCmpEnable()
	{
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		this.Reconstruct();
	}

	// Token: 0x060063B9 RID: 25529 RVA: 0x00252DE4 File Offset: 0x00250FE4
	private void ClearEntries()
	{
		for (int i = this.EntryObjects.Count - 1; i > -1; i--)
		{
			Util.KDestroyGameObject(this.EntryObjects[i]);
		}
		this.EntryObjects.Clear();
	}

	// Token: 0x060063BA RID: 25530 RVA: 0x00252E2A File Offset: 0x0025102A
	protected void RefreshCrewPortraitContent()
	{
		this.EntryObjects.ForEach(delegate(EntryType eo)
		{
			eo.RefreshCrewPortraitContent();
		});
	}

	// Token: 0x060063BB RID: 25531 RVA: 0x00252E56 File Offset: 0x00251056
	protected virtual void SpawnEntries()
	{
		if (this.EntryObjects.Count != 0)
		{
			this.ClearEntries();
		}
	}

	// Token: 0x060063BC RID: 25532 RVA: 0x00252E6C File Offset: 0x0025106C
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.autoColumn)
		{
			this.UpdateColumnTitles();
		}
		bool flag = false;
		List<MinionIdentity> liveIdentities = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		if (this.EntryObjects.Count != liveIdentities.Count || this.EntryObjects.FindAll((EntryType o) => liveIdentities.Contains(o.Identity)).Count != this.EntryObjects.Count)
		{
			flag = true;
		}
		if (flag)
		{
			this.Reconstruct();
		}
		this.UpdateScroll();
	}

	// Token: 0x060063BD RID: 25533 RVA: 0x00252EFD File Offset: 0x002510FD
	public void Reconstruct()
	{
		this.ClearEntries();
		this.SpawnEntries();
	}

	// Token: 0x060063BE RID: 25534 RVA: 0x00252F0C File Offset: 0x0025110C
	private void UpdateScroll()
	{
		if (this.PanelScrollbar)
		{
			if (this.EntryObjects.Count <= this.maxEntriesBeforeScroll)
			{
				this.PanelScrollbar.value = Mathf.Lerp(this.PanelScrollbar.value, 1f, 10f);
				this.PanelScrollbar.gameObject.SetActive(false);
				return;
			}
			this.PanelScrollbar.gameObject.SetActive(true);
		}
	}

	// Token: 0x060063BF RID: 25535 RVA: 0x00252F84 File Offset: 0x00251184
	private void SetHeadersActive(bool state)
	{
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			this.ColumnTitlesContainer.GetChild(i).gameObject.SetActive(state);
		}
	}

	// Token: 0x060063C0 RID: 25536 RVA: 0x00252FC0 File Offset: 0x002511C0
	protected virtual void PositionColumnTitles()
	{
		if (this.ColumnTitlesContainer == null)
		{
			return;
		}
		if (this.EntryObjects.Count <= 0)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		int childCount = this.EntryObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(i).GetComponent<OverviewColumnIdentity>();
			if (component != null)
			{
				GameObject gameObject = Util.KInstantiate(this.Prefab_ColumnTitle, null, null);
				gameObject.name = component.Column_DisplayName;
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				gameObject.transform.SetParent(this.ColumnTitlesContainer);
				componentInChildren.text = (component.StringLookup ? Strings.Get(component.Column_DisplayName) : component.Column_DisplayName);
				gameObject.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.SORTCOLUMN, componentInChildren.text);
				gameObject.rectTransform().anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x, 0f);
				OverviewColumnIdentity overviewColumnIdentity = gameObject.GetComponent<OverviewColumnIdentity>();
				if (overviewColumnIdentity == null)
				{
					overviewColumnIdentity = gameObject.AddComponent<OverviewColumnIdentity>();
				}
				overviewColumnIdentity.Column_DisplayName = component.Column_DisplayName;
				overviewColumnIdentity.columnID = component.columnID;
				overviewColumnIdentity.xPivot = component.xPivot;
				overviewColumnIdentity.Sortable = component.Sortable;
				if (overviewColumnIdentity.Sortable)
				{
					overviewColumnIdentity.GetComponentInChildren<ImageToggleState>(true).gameObject.SetActive(true);
				}
			}
		}
		this.UpdateColumnTitles();
		this.sortToggleGroup = base.gameObject.AddComponent<ToggleGroup>();
		this.sortToggleGroup.allowSwitchOff = true;
	}

	// Token: 0x060063C1 RID: 25537 RVA: 0x00253184 File Offset: 0x00251384
	protected void SortByName(bool reverse)
	{
		List<EntryType> list = new List<EntryType>(this.EntryObjects);
		list.Sort(delegate(EntryType a, EntryType b)
		{
			string text = a.Identity.GetProperName() + a.gameObject.GetInstanceID().ToString();
			string strB = b.Identity.GetProperName() + b.gameObject.GetInstanceID().ToString();
			return text.CompareTo(strB);
		});
		this.ReorderEntries(list, reverse);
	}

	// Token: 0x060063C2 RID: 25538 RVA: 0x002531CC File Offset: 0x002513CC
	protected void UpdateColumnTitles()
	{
		if (this.EntryObjects.Count <= 0 || !this.EntryObjects[0].gameObject.activeSelf)
		{
			this.SetHeadersActive(false);
			return;
		}
		this.SetHeadersActive(true);
		for (int i = 0; i < this.ColumnTitlesContainer.childCount; i++)
		{
			RectTransform rectTransform = this.ColumnTitlesContainer.GetChild(i).rectTransform();
			for (int j = 0; j < this.EntryObjects[0].transform.childCount; j++)
			{
				OverviewColumnIdentity component = this.EntryObjects[0].transform.GetChild(j).GetComponent<OverviewColumnIdentity>();
				if (component != null && component.Column_DisplayName == rectTransform.name)
				{
					rectTransform.pivot = new Vector2(component.xPivot, rectTransform.pivot.y);
					rectTransform.anchoredPosition = new Vector2(component.rectTransform().anchoredPosition.x + this.columnTitleHorizontalOffset, 0f);
					rectTransform.sizeDelta = new Vector2(component.rectTransform().sizeDelta.x, rectTransform.sizeDelta.y);
					if (rectTransform.anchoredPosition.x == 0f)
					{
						rectTransform.gameObject.SetActive(false);
					}
					else
					{
						rectTransform.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x060063C3 RID: 25539 RVA: 0x00253348 File Offset: 0x00251548
	protected void ReorderEntries(List<EntryType> sortedEntries, bool reverse)
	{
		for (int i = 0; i < sortedEntries.Count; i++)
		{
			if (reverse)
			{
				sortedEntries[i].transform.SetSiblingIndex(sortedEntries.Count - 1 - i);
			}
			else
			{
				sortedEntries[i].transform.SetSiblingIndex(i);
			}
		}
	}

	// Token: 0x040043B5 RID: 17333
	public GameObject Prefab_CrewEntry;

	// Token: 0x040043B6 RID: 17334
	public List<EntryType> EntryObjects = new List<EntryType>();

	// Token: 0x040043B7 RID: 17335
	public Transform ScrollRectTransform;

	// Token: 0x040043B8 RID: 17336
	public Transform EntriesPanelTransform;

	// Token: 0x040043B9 RID: 17337
	protected Vector2 EntryRectSize = new Vector2(750f, 64f);

	// Token: 0x040043BA RID: 17338
	public int maxEntriesBeforeScroll = 5;

	// Token: 0x040043BB RID: 17339
	public Scrollbar PanelScrollbar;

	// Token: 0x040043BC RID: 17340
	protected ToggleGroup sortToggleGroup;

	// Token: 0x040043BD RID: 17341
	protected Toggle lastSortToggle;

	// Token: 0x040043BE RID: 17342
	protected bool lastSortReversed;

	// Token: 0x040043BF RID: 17343
	public GameObject Prefab_ColumnTitle;

	// Token: 0x040043C0 RID: 17344
	public Transform ColumnTitlesContainer;

	// Token: 0x040043C1 RID: 17345
	public bool autoColumn;

	// Token: 0x040043C2 RID: 17346
	public float columnTitleHorizontalOffset;
}
