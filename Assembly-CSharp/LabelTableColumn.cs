using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CAB RID: 3243
public class LabelTableColumn : TableColumn
{
	// Token: 0x060063EE RID: 25582 RVA: 0x00254662 File Offset: 0x00252862
	public LabelTableColumn(Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, string> get_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, int widget_width = 128, bool should_refresh_columns = false) : base(on_load_action, sort_comparison, on_tooltip, on_sort_tooltip, null, should_refresh_columns, "")
	{
		this.get_value_action = get_value_action;
		this.widget_width = widget_width;
	}

	// Token: 0x060063EF RID: 25583 RVA: 0x00254694 File Offset: 0x00252894
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Label, parent, true);
		LayoutElement component = gameObject.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component.preferredWidth = (component.minWidth = (float)this.widget_width);
		return gameObject;
	}

	// Token: 0x060063F0 RID: 25584 RVA: 0x002546D8 File Offset: 0x002528D8
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject gameObject = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Label, parent, true);
		ToolTip tt = gameObject.GetComponent<ToolTip>();
		tt.OnToolTip = (() => this.GetTooltip(tt));
		LayoutElement component = gameObject.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component.preferredWidth = (component.minWidth = (float)this.widget_width);
		return gameObject;
	}

	// Token: 0x060063F1 RID: 25585 RVA: 0x00254750 File Offset: 0x00252950
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject widget_go = null;
		widget_go = Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.LabelHeader, parent, true);
		MultiToggle componentInChildren = widget_go.GetComponentInChildren<MultiToggle>(true);
		this.column_sort_toggle = componentInChildren;
		MultiToggle multiToggle = componentInChildren;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.screen.SetSortComparison(this.sort_comparer, this);
			this.screen.SortRows();
		}));
		ToolTip tt = widget_go.GetComponent<ToolTip>();
		tt.OnToolTip = delegate()
		{
			this.on_tooltip(null, widget_go, tt);
			return "";
		};
		tt = widget_go.GetComponentInChildren<MultiToggle>().GetComponent<ToolTip>();
		tt.OnToolTip = delegate()
		{
			this.on_sort_tooltip(null, widget_go, tt);
			return "";
		};
		LayoutElement component = widget_go.GetComponentInChildren<LocText>().GetComponent<LayoutElement>();
		component.preferredWidth = (component.minWidth = (float)this.widget_width);
		return widget_go;
	}

	// Token: 0x040043E5 RID: 17381
	public Func<IAssignableIdentity, GameObject, string> get_value_action;

	// Token: 0x040043E6 RID: 17382
	private int widget_width = 128;
}
