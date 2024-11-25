using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D87 RID: 3463
public class OwnablesSecondSideScreenRow : KMonoBehaviour
{
	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x06006CF8 RID: 27896 RVA: 0x0028F986 File Offset: 0x0028DB86
	// (set) Token: 0x06006CF7 RID: 27895 RVA: 0x0028F97D File Offset: 0x0028DB7D
	public AssignableSlotInstance minionSlotInstance { get; private set; }

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x06006CFA RID: 27898 RVA: 0x0028F997 File Offset: 0x0028DB97
	// (set) Token: 0x06006CF9 RID: 27897 RVA: 0x0028F98E File Offset: 0x0028DB8E
	public Assignable item { get; private set; }

	// Token: 0x06006CFB RID: 27899 RVA: 0x0028F9A0 File Offset: 0x0028DBA0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggle = base.GetComponent<MultiToggle>();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnMultitoggleClicked));
		this.eyeButton.onClick.AddListener(new UnityAction(this.FocusCameraOnAssignedItem));
	}

	// Token: 0x06006CFC RID: 27900 RVA: 0x0028FA04 File Offset: 0x0028DC04
	public void SetData(AssignableSlotInstance minion, Assignable item_assignable)
	{
		this.minionSlotInstance = minion;
		this.item = item_assignable;
		this.changeAssignmentListenerIDX = this.item.Subscribe(684616645, new Action<object>(this._OnItemAssignationChanged));
		this.destroyListenerIDX = this.item.Subscribe(1969584890, new Action<object>(this._OnRowItemDestroyed));
		this.Refresh();
	}

	// Token: 0x06006CFD RID: 27901 RVA: 0x0028FA6C File Offset: 0x0028DC6C
	public void Refresh()
	{
		if (this.item != null)
		{
			this.item.PrefabID();
			string properName = this.item.GetProperName();
			this.nameLabel.text = properName;
			this.icon.sprite = Def.GetUISprite(this.item.gameObject, "ui", false).first;
			bool flag = this.item.IsAssigned() && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable != this.item;
			if (this.item.IsAssigned())
			{
				this.statusLabel.SetText(string.Format(flag ? OwnablesSecondSideScreenRow.ASSIGNED_TO_OTHER : OwnablesSecondSideScreenRow.ASSIGNED_TO_SELF, this.item.assignee.GetProperName()));
			}
			else
			{
				this.statusLabel.SetText(OwnablesSecondSideScreenRow.NOT_ASSIGNED);
			}
			InfoDescription component = this.item.gameObject.GetComponent<InfoDescription>();
			bool flag2 = component != null && !string.IsNullOrEmpty(component.description);
			string simpleTooltip = flag2 ? component.description : properName;
			this.tooltip.SizingSetting = (flag2 ? ToolTip.ToolTipSizeSetting.MaxWidthWrapContent : ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap);
			this.tooltip.SetSimpleTooltip(simpleTooltip);
		}
		else
		{
			this.nameLabel.text = OwnablesSecondSideScreenRow.NO_DATA_MESSAGE;
			this.tooltip.SetSimpleTooltip(null);
		}
		bool flag3 = this.item != null && this.minionSlotInstance != null && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable == this.item;
		this.toggle.ChangeState(flag3 ? 1 : 0);
		this.emptyIcon.gameObject.SetActive(this.item == null);
		this.icon.gameObject.SetActive(this.item != null);
		this.eyeButton.gameObject.SetActive(this.item != null);
		this.statusLabel.gameObject.SetActive(this.item != null);
	}

	// Token: 0x06006CFE RID: 27902 RVA: 0x0028FC90 File Offset: 0x0028DE90
	public void ClearData()
	{
		if (this.item != null)
		{
			if (this.destroyListenerIDX != -1)
			{
				this.item.Unsubscribe(this.destroyListenerIDX);
			}
			if (this.changeAssignmentListenerIDX != -1)
			{
				this.item.Unsubscribe(this.changeAssignmentListenerIDX);
			}
		}
		this.minionSlotInstance = null;
		this.item = null;
		this.destroyListenerIDX = -1;
		this.changeAssignmentListenerIDX = -1;
		this.Refresh();
	}

	// Token: 0x06006CFF RID: 27903 RVA: 0x0028FD01 File Offset: 0x0028DF01
	private void _OnItemAssignationChanged(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemAssigneeChanged = this.OnRowItemAssigneeChanged;
		if (onRowItemAssigneeChanged == null)
		{
			return;
		}
		onRowItemAssigneeChanged(this);
	}

	// Token: 0x06006D00 RID: 27904 RVA: 0x0028FD14 File Offset: 0x0028DF14
	private void _OnRowItemDestroyed(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemDestroyed = this.OnRowItemDestroyed;
		if (onRowItemDestroyed == null)
		{
			return;
		}
		onRowItemDestroyed(this);
	}

	// Token: 0x06006D01 RID: 27905 RVA: 0x0028FD27 File Offset: 0x0028DF27
	private void OnMultitoggleClicked()
	{
		Action<OwnablesSecondSideScreenRow> onRowClicked = this.OnRowClicked;
		if (onRowClicked == null)
		{
			return;
		}
		onRowClicked(this);
	}

	// Token: 0x06006D02 RID: 27906 RVA: 0x0028FD3C File Offset: 0x0028DF3C
	private void FocusCameraOnAssignedItem()
	{
		if (this.item != null)
		{
			GameObject gameObject = this.item.gameObject;
			if (this.item.HasTag(GameTags.Equipped))
			{
				gameObject = this.item.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			}
			GameUtil.FocusCamera(gameObject.transform, false);
		}
	}

	// Token: 0x04004A44 RID: 19012
	public static string NO_DATA_MESSAGE = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_FOUND;

	// Token: 0x04004A45 RID: 19013
	public static string NOT_ASSIGNED = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.NOT_ASSIGNED;

	// Token: 0x04004A46 RID: 19014
	public static string ASSIGNED_TO_SELF = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_SELF_STATUS;

	// Token: 0x04004A47 RID: 19015
	public static string ASSIGNED_TO_OTHER = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_OTHER_STATUS;

	// Token: 0x04004A48 RID: 19016
	public KImage icon;

	// Token: 0x04004A49 RID: 19017
	public KImage emptyIcon;

	// Token: 0x04004A4A RID: 19018
	public LocText nameLabel;

	// Token: 0x04004A4B RID: 19019
	public LocText statusLabel;

	// Token: 0x04004A4C RID: 19020
	public Button eyeButton;

	// Token: 0x04004A4D RID: 19021
	public ToolTip tooltip;

	// Token: 0x04004A4E RID: 19022
	public Action<OwnablesSecondSideScreenRow> OnRowItemAssigneeChanged;

	// Token: 0x04004A4F RID: 19023
	public Action<OwnablesSecondSideScreenRow> OnRowItemDestroyed;

	// Token: 0x04004A50 RID: 19024
	public Action<OwnablesSecondSideScreenRow> OnRowClicked;

	// Token: 0x04004A53 RID: 19027
	private MultiToggle toggle;

	// Token: 0x04004A54 RID: 19028
	private int changeAssignmentListenerIDX = -1;

	// Token: 0x04004A55 RID: 19029
	private int destroyListenerIDX = -1;
}
