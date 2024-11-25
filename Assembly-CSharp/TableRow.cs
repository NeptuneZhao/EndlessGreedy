using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CB9 RID: 3257
[AddComponentMenu("KMonoBehaviour/scripts/TableRow")]
public class TableRow : KMonoBehaviour
{
	// Token: 0x0600648A RID: 25738 RVA: 0x00259744 File Offset: 0x00257944
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.selectMinionButton != null)
		{
			this.selectMinionButton.onClick += this.SelectMinion;
			this.selectMinionButton.onDoubleClick += this.SelectAndFocusMinion;
		}
	}

	// Token: 0x0600648B RID: 25739 RVA: 0x00259793 File Offset: 0x00257993
	public GameObject GetScroller(string scrollerID)
	{
		return this.scrollers[scrollerID];
	}

	// Token: 0x0600648C RID: 25740 RVA: 0x002597A1 File Offset: 0x002579A1
	public GameObject GetScrollerBorder(string scrolledID)
	{
		return this.scrollerBorders[scrolledID];
	}

	// Token: 0x0600648D RID: 25741 RVA: 0x002597B0 File Offset: 0x002579B0
	public void SelectMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.Select(minionIdentity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x0600648E RID: 25742 RVA: 0x002597E4 File Offset: 0x002579E4
	public void SelectAndFocusMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
	}

	// Token: 0x0600648F RID: 25743 RVA: 0x00259838 File Offset: 0x00257A38
	public void ConfigureAsWorldDivider(Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		ScrollRect scroll_rect = base.gameObject.GetComponentInChildren<ScrollRect>();
		this.rowType = TableRow.RowType.WorldDivider;
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.scrollerID != "")
			{
				TableColumn value = keyValuePair.Value;
				break;
			}
		}
		scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
		{
			if (screen.CheckScrollersDirty())
			{
				return;
			}
			screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
		});
	}

	// Token: 0x06006490 RID: 25744 RVA: 0x002598E4 File Offset: 0x00257AE4
	public void ConfigureContent(IAssignableIdentity minion, Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		this.minion = minion;
		KImage componentInChildren = base.GetComponentInChildren<KImage>(true);
		componentInChildren.colorStyleSetting = ((minion == null) ? this.style_setting_default : this.style_setting_minion);
		componentInChildren.ColorState = KImage.ColorSelector.Inactive;
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null && minion as StoredMinionIdentity != null)
		{
			component.alpha = 0.6f;
		}
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			GameObject gameObject;
			if (minion == null)
			{
				if (this.isDefault)
				{
					gameObject = keyValuePair.Value.GetDefaultWidget(base.gameObject);
				}
				else
				{
					gameObject = keyValuePair.Value.GetHeaderWidget(base.gameObject);
				}
			}
			else
			{
				gameObject = keyValuePair.Value.GetMinionWidget(base.gameObject);
			}
			this.widgets.Add(keyValuePair.Value, gameObject);
			keyValuePair.Value.widgets_by_row.Add(this, gameObject);
			if (keyValuePair.Value.scrollerID != "")
			{
				foreach (string text in keyValuePair.Value.screen.column_scrollers)
				{
					if (!(text != keyValuePair.Value.scrollerID))
					{
						if (!this.scrollers.ContainsKey(text))
						{
							GameObject gameObject2 = Util.KInstantiateUI(this.scrollerPrefab, base.gameObject, true);
							ScrollRect scroll_rect = gameObject2.GetComponent<ScrollRect>();
							this.scrollbar = gameObject2.GetComponentInChildren<Scrollbar>();
							scroll_rect.horizontalScrollbar = this.scrollbar;
							scroll_rect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
							scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
							{
								if (screen.CheckScrollersDirty())
								{
									return;
								}
								screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
							});
							this.scrollers.Add(text, scroll_rect.content.gameObject);
							if (scroll_rect.content.transform.parent.Find("Border") != null)
							{
								this.scrollerBorders.Add(text, scroll_rect.content.transform.parent.Find("Border").gameObject);
							}
						}
						gameObject.transform.SetParent(this.scrollers[text].transform);
						this.scrollers[text].transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
					}
				}
			}
		}
		this.RefreshColumns(columns);
		if (minion != null)
		{
			base.gameObject.name = minion.GetProperName();
		}
		else if (this.isDefault)
		{
			base.gameObject.name = "defaultRow";
		}
		if (this.selectMinionButton)
		{
			this.selectMinionButton.transform.SetAsLastSibling();
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			float width = rectTransform.rect.width;
			keyValuePair2.Value.transform.SetParent(base.gameObject.transform);
			rectTransform.anchorMin = (rectTransform.anchorMax = new Vector2(0f, 1f));
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
			RectTransform rectTransform2 = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform();
			Vector3 a = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform().GetLocalPosition() - new Vector3(rectTransform2.sizeDelta.x / 2f, -1f * (rectTransform2.sizeDelta.y / 2f), 0f);
			a.y = 0f;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 374f);
			rectTransform.SetLocalPosition(a + Vector3.up * rectTransform.GetLocalPosition().y + Vector3.up * -rectTransform.anchoredPosition.y);
		}
	}

	// Token: 0x06006491 RID: 25745 RVA: 0x00259DF4 File Offset: 0x00257FF4
	public void RefreshColumns(Dictionary<string, TableColumn> columns)
	{
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.on_load_action != null)
			{
				keyValuePair.Value.on_load_action(this.minion, keyValuePair.Value.widgets_by_row[this]);
			}
		}
	}

	// Token: 0x06006492 RID: 25746 RVA: 0x00259E74 File Offset: 0x00258074
	public void RefreshScrollers()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.scrollers)
		{
			ScrollRect component = keyValuePair.Value.transform.parent.GetComponent<ScrollRect>();
			component.GetComponent<LayoutElement>().minWidth = Mathf.Min(768f, component.content.sizeDelta.x);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			rectTransform.sizeDelta = new Vector2(this.scrollers[keyValuePair2.Key].transform.parent.GetComponent<LayoutElement>().minWidth, rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x06006493 RID: 25747 RVA: 0x00259F84 File Offset: 0x00258184
	public GameObject GetWidget(TableColumn column)
	{
		if (this.widgets.ContainsKey(column) && this.widgets[column] != null)
		{
			return this.widgets[column];
		}
		global::Debug.LogWarning("Widget is null or row does not contain widget for column " + ((column != null) ? column.ToString() : null));
		return null;
	}

	// Token: 0x06006494 RID: 25748 RVA: 0x00259FDD File Offset: 0x002581DD
	public IAssignableIdentity GetIdentity()
	{
		return this.minion;
	}

	// Token: 0x06006495 RID: 25749 RVA: 0x00259FE5 File Offset: 0x002581E5
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets.ContainsValue(widget);
	}

	// Token: 0x06006496 RID: 25750 RVA: 0x00259FF4 File Offset: 0x002581F4
	public void Clear()
	{
		foreach (KeyValuePair<TableColumn, GameObject> keyValuePair in this.widgets)
		{
			keyValuePair.Key.widgets_by_row.Remove(this);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004415 RID: 17429
	public TableRow.RowType rowType;

	// Token: 0x04004416 RID: 17430
	private IAssignableIdentity minion;

	// Token: 0x04004417 RID: 17431
	private Dictionary<TableColumn, GameObject> widgets = new Dictionary<TableColumn, GameObject>();

	// Token: 0x04004418 RID: 17432
	private Dictionary<string, GameObject> scrollers = new Dictionary<string, GameObject>();

	// Token: 0x04004419 RID: 17433
	private Dictionary<string, GameObject> scrollerBorders = new Dictionary<string, GameObject>();

	// Token: 0x0400441A RID: 17434
	public bool isDefault;

	// Token: 0x0400441B RID: 17435
	public KButton selectMinionButton;

	// Token: 0x0400441C RID: 17436
	[SerializeField]
	private ColorStyleSetting style_setting_default;

	// Token: 0x0400441D RID: 17437
	[SerializeField]
	private ColorStyleSetting style_setting_minion;

	// Token: 0x0400441E RID: 17438
	[SerializeField]
	private GameObject scrollerPrefab;

	// Token: 0x0400441F RID: 17439
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x02001DBD RID: 7613
	public enum RowType
	{
		// Token: 0x04008820 RID: 34848
		Header,
		// Token: 0x04008821 RID: 34849
		Default,
		// Token: 0x04008822 RID: 34850
		Minion,
		// Token: 0x04008823 RID: 34851
		StoredMinon,
		// Token: 0x04008824 RID: 34852
		WorldDivider
	}
}
