using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF6 RID: 3574
public class GroupSelectorWidget : MonoBehaviour
{
	// Token: 0x06007167 RID: 29031 RVA: 0x002AE46F File Offset: 0x002AC66F
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorWidget.ItemCallbacks item_callbacks)
	{
		this.widgetID = widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		this.addItemButton.onClick += this.OnAddItemClicked;
	}

	// Token: 0x06007168 RID: 29032 RVA: 0x002AE4A0 File Offset: 0x002AC6A0
	public void Reconfigure(IList<int> selected_option_indices)
	{
		this.selectedOptionIndices.Clear();
		this.selectedOptionIndices.AddRange(selected_option_indices);
		this.selectedOptionIndices.Sort();
		this.addItemButton.isInteractable = (this.selectedOptionIndices.Count < this.options.Count);
		this.RebuildSelectedVisualizers();
	}

	// Token: 0x06007169 RID: 29033 RVA: 0x002AE4F8 File Offset: 0x002AC6F8
	private void OnAddItemClicked()
	{
		if (!this.IsSubPanelOpen())
		{
			if (this.RebuildSubPanelOptions() > 0)
			{
				this.unselectedItemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.unselectedItemsPanel.childCount);
				this.unselectedItemsPanel.gameObject.SetActive(true);
				this.unselectedItemsPanel.GetComponent<Selectable>().Select();
				return;
			}
		}
		else
		{
			this.CloseSubPanel();
		}
	}

	// Token: 0x0600716A RID: 29034 RVA: 0x002AE564 File Offset: 0x002AC764
	private void OnItemAdded(int option_idx)
	{
		if (this.itemCallbacks.onItemAdded != null)
		{
			this.itemCallbacks.onItemAdded(this.widgetID, this.options[option_idx].userData);
			this.RebuildSubPanelOptions();
		}
	}

	// Token: 0x0600716B RID: 29035 RVA: 0x002AE5A1 File Offset: 0x002AC7A1
	private void OnItemRemoved(int option_idx)
	{
		if (this.itemCallbacks.onItemRemoved != null)
		{
			this.itemCallbacks.onItemRemoved(this.widgetID, this.options[option_idx].userData);
		}
	}

	// Token: 0x0600716C RID: 29036 RVA: 0x002AE5D8 File Offset: 0x002AC7D8
	private void RebuildSelectedVisualizers()
	{
		foreach (GameObject original in this.selectedVisualizers)
		{
			Util.KDestroyGameObject(original);
		}
		this.selectedVisualizers.Clear();
		foreach (int idx in this.selectedOptionIndices)
		{
			GameObject item = this.CreateItem(idx, new Action<int>(this.OnItemRemoved), this.selectedItemsPanel.gameObject, true);
			this.selectedVisualizers.Add(item);
		}
	}

	// Token: 0x0600716D RID: 29037 RVA: 0x002AE69C File Offset: 0x002AC89C
	private GameObject CreateItem(int idx, Action<int> on_click, GameObject parent, bool is_selected_item)
	{
		GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, parent, true);
		KButton component = gameObject.GetComponent<KButton>();
		component.onClick += delegate()
		{
			on_click(idx);
		};
		component.fgImage.sprite = this.options[idx].sprite;
		if (parent == this.selectedItemsPanel.gameObject)
		{
			HierarchyReferences component2 = component.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				Component reference = component2.GetReference("CancelImg");
				if (reference != null)
				{
					reference.gameObject.SetActive(true);
				}
			}
		}
		gameObject.GetComponent<ToolTip>().OnToolTip = (() => this.itemCallbacks.getItemHoverText(this.widgetID, this.options[idx].userData, is_selected_item));
		return gameObject;
	}

	// Token: 0x0600716E RID: 29038 RVA: 0x002AE76E File Offset: 0x002AC96E
	public bool IsSubPanelOpen()
	{
		return this.unselectedItemsPanel.gameObject.activeSelf;
	}

	// Token: 0x0600716F RID: 29039 RVA: 0x002AE780 File Offset: 0x002AC980
	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.unselectedItemsPanel.gameObject.SetActive(false);
	}

	// Token: 0x06007170 RID: 29040 RVA: 0x002AE79C File Offset: 0x002AC99C
	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.unselectedItemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	// Token: 0x06007171 RID: 29041 RVA: 0x002AE7FC File Offset: 0x002AC9FC
	private int RebuildSubPanelOptions()
	{
		IList<int> list = this.itemCallbacks.getSubPanelDisplayIndices(this.widgetID);
		if (list.Count > 0)
		{
			this.ClearSubPanelOptions();
			using (IEnumerator<int> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					if (!this.selectedOptionIndices.Contains(num))
					{
						this.CreateItem(num, new Action<int>(this.OnItemAdded), this.unselectedItemsPanel.gameObject, false);
					}
				}
				goto IL_7E;
			}
		}
		this.CloseSubPanel();
		IL_7E:
		return list.Count;
	}

	// Token: 0x04004E17 RID: 19991
	[SerializeField]
	private GameObject itemTemplate;

	// Token: 0x04004E18 RID: 19992
	[SerializeField]
	private RectTransform selectedItemsPanel;

	// Token: 0x04004E19 RID: 19993
	[SerializeField]
	private RectTransform unselectedItemsPanel;

	// Token: 0x04004E1A RID: 19994
	[SerializeField]
	private KButton addItemButton;

	// Token: 0x04004E1B RID: 19995
	[SerializeField]
	private int numExpectedPanelColumns = 3;

	// Token: 0x04004E1C RID: 19996
	private object widgetID;

	// Token: 0x04004E1D RID: 19997
	private GroupSelectorWidget.ItemCallbacks itemCallbacks;

	// Token: 0x04004E1E RID: 19998
	private IList<GroupSelectorWidget.ItemData> options;

	// Token: 0x04004E1F RID: 19999
	private List<int> selectedOptionIndices = new List<int>();

	// Token: 0x04004E20 RID: 20000
	private List<GameObject> selectedVisualizers = new List<GameObject>();

	// Token: 0x02001EFA RID: 7930
	[Serializable]
	public struct ItemData
	{
		// Token: 0x0600AD2A RID: 44330 RVA: 0x003A8DC1 File Offset: 0x003A6FC1
		public ItemData(Sprite sprite, object user_data)
		{
			this.sprite = sprite;
			this.userData = user_data;
		}

		// Token: 0x04008C3C RID: 35900
		public Sprite sprite;

		// Token: 0x04008C3D RID: 35901
		public object userData;
	}

	// Token: 0x02001EFB RID: 7931
	public struct ItemCallbacks
	{
		// Token: 0x04008C3E RID: 35902
		public Func<object, IList<int>> getSubPanelDisplayIndices;

		// Token: 0x04008C3F RID: 35903
		public Action<object, object> onItemAdded;

		// Token: 0x04008C40 RID: 35904
		public Action<object, object> onItemRemoved;

		// Token: 0x04008C41 RID: 35905
		public Func<object, object, bool, string> getItemHoverText;
	}
}
