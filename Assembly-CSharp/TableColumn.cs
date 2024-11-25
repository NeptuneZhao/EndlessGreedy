using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CB7 RID: 3255
public class TableColumn : IRender1000ms
{
	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x0600647A RID: 25722 RVA: 0x00259486 File Offset: 0x00257686
	public bool isRevealed
	{
		get
		{
			return this.revealed == null || this.revealed();
		}
	}

	// Token: 0x0600647B RID: 25723 RVA: 0x002594A0 File Offset: 0x002576A0
	public TableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip = null, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip = null, Func<bool> revealed = null, bool should_refresh_columns = false, string scrollerID = "")
	{
		this.on_load_action = on_load_action;
		this.sort_comparer = sort_comparison;
		this.on_tooltip = on_tooltip;
		this.on_sort_tooltip = on_sort_tooltip;
		this.revealed = revealed;
		this.scrollerID = scrollerID;
		if (should_refresh_columns)
		{
			SimAndRenderScheduler.instance.Add(this, false);
		}
	}

	// Token: 0x0600647C RID: 25724 RVA: 0x002594FC File Offset: 0x002576FC
	protected string GetTooltip(ToolTip tool_tip_instance)
	{
		GameObject gameObject = tool_tip_instance.gameObject;
		HierarchyReferences component = tool_tip_instance.GetComponent<HierarchyReferences>();
		if (component != null && component.HasReference("Widget"))
		{
			gameObject = component.GetReference("Widget").gameObject;
		}
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_tooltip != null)
		{
			this.on_tooltip(tableRow.GetIdentity(), gameObject, tool_tip_instance);
		}
		return "";
	}

	// Token: 0x0600647D RID: 25725 RVA: 0x002595C4 File Offset: 0x002577C4
	protected string GetSortTooltip(ToolTip sort_tooltip_instance)
	{
		GameObject gameObject = sort_tooltip_instance.transform.parent.gameObject;
		TableRow tableRow = null;
		foreach (KeyValuePair<TableRow, GameObject> keyValuePair in this.widgets_by_row)
		{
			if (keyValuePair.Value == gameObject)
			{
				tableRow = keyValuePair.Key;
				break;
			}
		}
		if (tableRow != null && this.on_sort_tooltip != null)
		{
			this.on_sort_tooltip(tableRow.GetIdentity(), gameObject, sort_tooltip_instance);
		}
		return "";
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x0600647E RID: 25726 RVA: 0x00259668 File Offset: 0x00257868
	public bool isDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x0600647F RID: 25727 RVA: 0x00259670 File Offset: 0x00257870
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets_by_row.ContainsValue(widget);
	}

	// Token: 0x06006480 RID: 25728 RVA: 0x0025967E File Offset: 0x0025787E
	public virtual GameObject GetMinionWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06006481 RID: 25729 RVA: 0x0025968B File Offset: 0x0025788B
	public virtual GameObject GetHeaderWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06006482 RID: 25730 RVA: 0x00259698 File Offset: 0x00257898
	public virtual GameObject GetDefaultWidget(GameObject parent)
	{
		global::Debug.LogError("Table Column has no Widget prefab");
		return null;
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x002596A5 File Offset: 0x002578A5
	public void Render1000ms(float dt)
	{
		this.MarkDirty(null, TableScreen.ResultValues.False);
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x002596AF File Offset: 0x002578AF
	public void MarkDirty(GameObject triggering_obj = null, TableScreen.ResultValues triggering_object_state = TableScreen.ResultValues.False)
	{
		this.dirty = true;
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x002596B8 File Offset: 0x002578B8
	public void MarkClean()
	{
		this.dirty = false;
	}

	// Token: 0x0400440B RID: 17419
	public Action<IAssignableIdentity, GameObject> on_load_action;

	// Token: 0x0400440C RID: 17420
	public Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip;

	// Token: 0x0400440D RID: 17421
	public Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip;

	// Token: 0x0400440E RID: 17422
	public Comparison<IAssignableIdentity> sort_comparer;

	// Token: 0x0400440F RID: 17423
	public Dictionary<TableRow, GameObject> widgets_by_row = new Dictionary<TableRow, GameObject>();

	// Token: 0x04004410 RID: 17424
	public string scrollerID;

	// Token: 0x04004411 RID: 17425
	public TableScreen screen;

	// Token: 0x04004412 RID: 17426
	public MultiToggle column_sort_toggle;

	// Token: 0x04004413 RID: 17427
	private Func<bool> revealed;

	// Token: 0x04004414 RID: 17428
	protected bool dirty;
}
