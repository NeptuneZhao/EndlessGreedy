using System;
using UnityEngine;

// Token: 0x02000CB3 RID: 3251
public class ConsumableInfoTableColumn : CheckboxTableColumn
{
	// Token: 0x06006426 RID: 25638 RVA: 0x00256574 File Offset: 0x00254774
	public ConsumableInfoTableColumn(IConsumableUIItem consumable_info, Action<IAssignableIdentity, GameObject> load_value_action, Func<IAssignableIdentity, GameObject, TableScreen.ResultValues> get_value_action, Action<GameObject> on_press_action, Action<GameObject, TableScreen.ResultValues> set_value_action, Comparison<IAssignableIdentity> sort_comparison, Action<IAssignableIdentity, GameObject, ToolTip> on_tooltip, Action<IAssignableIdentity, GameObject, ToolTip> on_sort_tooltip, Func<GameObject, string> get_header_label) : base(load_value_action, get_value_action, on_press_action, set_value_action, sort_comparison, on_tooltip, on_sort_tooltip, () => DebugHandler.InstantBuildMode || ConsumerManager.instance.isDiscovered(consumable_info.ConsumableId.ToTag()))
	{
		this.consumable_info = consumable_info;
		this.get_header_label = get_header_label;
	}

	// Token: 0x06006427 RID: 25639 RVA: 0x002565C0 File Offset: 0x002547C0
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		GameObject headerWidget = base.GetHeaderWidget(parent);
		if (headerWidget.GetComponentInChildren<LocText>() != null)
		{
			headerWidget.GetComponentInChildren<LocText>().text = this.get_header_label(headerWidget);
		}
		headerWidget.GetComponentInChildren<MultiToggle>().gameObject.SetActive(false);
		return headerWidget;
	}

	// Token: 0x040043FB RID: 17403
	public IConsumableUIItem consumable_info;

	// Token: 0x040043FC RID: 17404
	public Func<GameObject, string> get_header_label;
}
