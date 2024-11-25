using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF5 RID: 3573
public class GroupSelectorHeaderWidget : MonoBehaviour
{
	// Token: 0x06007162 RID: 29026 RVA: 0x002AE10C File Offset: 0x002AC30C
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorHeaderWidget.ItemCallbacks item_callbacks)
	{
		GroupSelectorHeaderWidget.<>c__DisplayClass11_0 CS$<>8__locals1 = new GroupSelectorHeaderWidget.<>c__DisplayClass11_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.widget_id = widget_id;
		this.widgetID = CS$<>8__locals1.widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		if (this.itemCallbacks.getTitleHoverText != null)
		{
			this.label.GetComponent<ToolTip>().OnToolTip = (() => CS$<>8__locals1.<>4__this.itemCallbacks.getTitleHoverText(CS$<>8__locals1.widget_id));
		}
		bool adding_item2 = true;
		Func<object, IList<int>> <>9__5;
		Func<object, object, string> <>9__6;
		this.addItemButton.onClick += delegate()
		{
			GroupSelectorHeaderWidget <>4__this = CS$<>8__locals1.<>4__this;
			Vector3 position = CS$<>8__locals1.<>4__this.addItemButton.transform.GetPosition();
			Func<object, IList<int>> display_list_query;
			if ((display_list_query = <>9__5) == null)
			{
				display_list_query = (<>9__5 = ((object widget_go) => CS$<>8__locals1.<>4__this.itemCallbacks.getHeaderButtonOptions(widget_go, adding_item2)));
			}
			Action<object> onItemAdded = CS$<>8__locals1.<>4__this.itemCallbacks.onItemAdded;
			Func<object, object, string> get_item_hover_text;
			if ((get_item_hover_text = <>9__6) == null)
			{
				get_item_hover_text = (<>9__6 = ((object widget_go, object item_data) => CS$<>8__locals1.<>4__this.itemCallbacks.getItemHoverText(widget_go, adding_item2, item_data)));
			}
			<>4__this.RebuildSubPanel(position, display_list_query, onItemAdded, get_item_hover_text);
		};
		bool adding_item = false;
		Func<object, IList<int>> <>9__8;
		Func<object, object, string> <>9__9;
		this.removeItemButton.onClick += delegate()
		{
			GroupSelectorHeaderWidget <>4__this = CS$<>8__locals1.<>4__this;
			Vector3 position = CS$<>8__locals1.<>4__this.removeItemButton.transform.GetPosition();
			Func<object, IList<int>> display_list_query;
			if ((display_list_query = <>9__8) == null)
			{
				display_list_query = (<>9__8 = ((object widget_go) => CS$<>8__locals1.<>4__this.itemCallbacks.getHeaderButtonOptions(widget_go, adding_item)));
			}
			Action<object> onItemRemoved = CS$<>8__locals1.<>4__this.itemCallbacks.onItemRemoved;
			Func<object, object, string> get_item_hover_text;
			if ((get_item_hover_text = <>9__9) == null)
			{
				get_item_hover_text = (<>9__9 = ((object widget_go, object item_data) => CS$<>8__locals1.<>4__this.itemCallbacks.getItemHoverText(widget_go, adding_item, item_data)));
			}
			<>4__this.RebuildSubPanel(position, display_list_query, onItemRemoved, get_item_hover_text);
		};
		this.sortButton.onClick += delegate()
		{
			GroupSelectorHeaderWidget <>4__this = CS$<>8__locals1.<>4__this;
			Vector3 position = CS$<>8__locals1.<>4__this.sortButton.transform.GetPosition();
			Func<object, IList<int>> getValidSortOptionIndices = CS$<>8__locals1.<>4__this.itemCallbacks.getValidSortOptionIndices;
			Action<object> on_item_selected;
			if ((on_item_selected = CS$<>8__locals1.<>9__10) == null)
			{
				on_item_selected = (CS$<>8__locals1.<>9__10 = delegate(object item_data)
				{
					CS$<>8__locals1.<>4__this.itemCallbacks.onSort(CS$<>8__locals1.<>4__this.widgetID, item_data);
				});
			}
			Func<object, object, string> get_item_hover_text;
			if ((get_item_hover_text = CS$<>8__locals1.<>9__11) == null)
			{
				get_item_hover_text = (CS$<>8__locals1.<>9__11 = ((object widget_go, object item_data) => CS$<>8__locals1.<>4__this.itemCallbacks.getSortHoverText(item_data)));
			}
			<>4__this.RebuildSubPanel(position, getValidSortOptionIndices, on_item_selected, get_item_hover_text);
		};
		if (this.itemCallbacks.getTitleButtonHoverText != null)
		{
			this.addItemButton.GetComponent<ToolTip>().OnToolTip = (() => CS$<>8__locals1.<>4__this.itemCallbacks.getTitleButtonHoverText(CS$<>8__locals1.widget_id, true));
			this.removeItemButton.GetComponent<ToolTip>().OnToolTip = (() => CS$<>8__locals1.<>4__this.itemCallbacks.getTitleButtonHoverText(CS$<>8__locals1.widget_id, false));
		}
	}

	// Token: 0x06007163 RID: 29027 RVA: 0x002AE224 File Offset: 0x002AC424
	private void RebuildSubPanel(Vector3 pos, Func<object, IList<int>> display_list_query, Action<object> on_item_selected, Func<object, object, string> get_item_hover_text)
	{
		this.itemsPanel.gameObject.transform.SetPosition(pos + new Vector3(2f, 2f, 0f));
		IList<int> list = display_list_query(this.widgetID);
		if (list.Count > 0)
		{
			this.ClearSubPanelOptions();
			foreach (int idx2 in list)
			{
				int idx = idx2;
				GroupSelectorWidget.ItemData itemData = this.options[idx];
				GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, this.itemsPanel.gameObject, true);
				KButton component = gameObject.GetComponent<KButton>();
				component.fgImage.sprite = this.options[idx].sprite;
				component.onClick += delegate()
				{
					on_item_selected(this.options[idx].userData);
					this.RebuildSubPanel(pos, display_list_query, on_item_selected, get_item_hover_text);
				};
				if (get_item_hover_text != null)
				{
					gameObject.GetComponent<ToolTip>().OnToolTip = (() => get_item_hover_text(this.widgetID, this.options[idx].userData));
				}
			}
			this.itemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.itemsPanel.childCount);
			this.itemsPanel.gameObject.SetActive(true);
			this.itemsPanel.GetComponent<Selectable>().Select();
			return;
		}
		this.CloseSubPanel();
	}

	// Token: 0x06007164 RID: 29028 RVA: 0x002AE3E4 File Offset: 0x002AC5E4
	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.itemsPanel.gameObject.SetActive(false);
	}

	// Token: 0x06007165 RID: 29029 RVA: 0x002AE400 File Offset: 0x002AC600
	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.itemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	// Token: 0x04004E0D RID: 19981
	public LocText label;

	// Token: 0x04004E0E RID: 19982
	[SerializeField]
	private GameObject itemTemplate;

	// Token: 0x04004E0F RID: 19983
	[SerializeField]
	private RectTransform itemsPanel;

	// Token: 0x04004E10 RID: 19984
	[SerializeField]
	private KButton addItemButton;

	// Token: 0x04004E11 RID: 19985
	[SerializeField]
	private KButton removeItemButton;

	// Token: 0x04004E12 RID: 19986
	[SerializeField]
	private KButton sortButton;

	// Token: 0x04004E13 RID: 19987
	[SerializeField]
	private int numExpectedPanelColumns = 3;

	// Token: 0x04004E14 RID: 19988
	private object widgetID;

	// Token: 0x04004E15 RID: 19989
	private GroupSelectorHeaderWidget.ItemCallbacks itemCallbacks;

	// Token: 0x04004E16 RID: 19990
	private IList<GroupSelectorWidget.ItemData> options;

	// Token: 0x02001EF4 RID: 7924
	public struct ItemCallbacks
	{
		// Token: 0x04008C20 RID: 35872
		public Func<object, string> getTitleHoverText;

		// Token: 0x04008C21 RID: 35873
		public Func<object, bool, string> getTitleButtonHoverText;

		// Token: 0x04008C22 RID: 35874
		public Func<object, bool, IList<int>> getHeaderButtonOptions;

		// Token: 0x04008C23 RID: 35875
		public Action<object> onItemAdded;

		// Token: 0x04008C24 RID: 35876
		public Action<object> onItemRemoved;

		// Token: 0x04008C25 RID: 35877
		public Func<object, bool, object, string> getItemHoverText;

		// Token: 0x04008C26 RID: 35878
		public Func<object, IList<int>> getValidSortOptionIndices;

		// Token: 0x04008C27 RID: 35879
		public Func<object, string> getSortHoverText;

		// Token: 0x04008C28 RID: 35880
		public Action<object, object> onSort;
	}
}
