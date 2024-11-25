using System;
using UnityEngine;

// Token: 0x02000CAA RID: 3242
public class SuperCheckboxTableColumn : CheckboxTableColumn
{
	// Token: 0x060063EA RID: 25578 RVA: 0x00254478 File Offset: 0x00252678
	public SuperCheckboxTableColumn(CheckboxTableColumn[] columns_affected, Action<IAssignableIdentity, GameObject> on_load_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip) : base(on_load_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, null, null)
	{
		this.columns_affected = columns_affected;
	}

	// Token: 0x060063EB RID: 25579 RVA: 0x002544B4 File Offset: 0x002526B4
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_super_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x060063EC RID: 25580 RVA: 0x00254544 File Offset: 0x00252744
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_super_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x060063ED RID: 25581 RVA: 0x002545D4 File Offset: 0x002527D4
	public override GameObject GetMinionWidget(GameObject parent)
	{
		GameObject widget_go = Util.KInstantiateUI(this.prefab_super_checkbox, parent, true);
		if (widget_go.GetComponent<ToolTip>() != null)
		{
			widget_go.GetComponent<ToolTip>().OnToolTip = (() => this.GetTooltip(widget_go.GetComponent<ToolTip>()));
		}
		MultiToggle component = widget_go.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.on_press_action(widget_go);
		}));
		return widget_go;
	}

	// Token: 0x040043E3 RID: 17379
	public GameObject prefab_super_checkbox = Assets.UIPrefabs.TableScreenWidgets.SuperCheckbox_Horizontal;

	// Token: 0x040043E4 RID: 17380
	public CheckboxTableColumn[] columns_affected;
}
