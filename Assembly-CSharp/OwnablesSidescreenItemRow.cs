using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D8A RID: 3466
public class OwnablesSidescreenItemRow : KMonoBehaviour
{
	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x06006D22 RID: 27938 RVA: 0x00290544 File Offset: 0x0028E744
	// (set) Token: 0x06006D21 RID: 27937 RVA: 0x0029053B File Offset: 0x0028E73B
	public bool IsLocked { get; private set; }

	// Token: 0x170007AF RID: 1967
	// (get) Token: 0x06006D23 RID: 27939 RVA: 0x0029054C File Offset: 0x0028E74C
	public bool SlotIsAssigned
	{
		get
		{
			return this.Slot != null && this.SlotInstance != null && !this.SlotInstance.IsUnassigning() && this.SlotInstance.IsAssigned();
		}
	}

	// Token: 0x170007B0 RID: 1968
	// (get) Token: 0x06006D25 RID: 27941 RVA: 0x00290581 File Offset: 0x0028E781
	// (set) Token: 0x06006D24 RID: 27940 RVA: 0x00290578 File Offset: 0x0028E778
	public AssignableSlotInstance SlotInstance { get; private set; }

	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x06006D27 RID: 27943 RVA: 0x00290592 File Offset: 0x0028E792
	// (set) Token: 0x06006D26 RID: 27942 RVA: 0x00290589 File Offset: 0x0028E789
	public AssignableSlot Slot { get; private set; }

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x06006D29 RID: 27945 RVA: 0x002905A3 File Offset: 0x0028E7A3
	// (set) Token: 0x06006D28 RID: 27944 RVA: 0x0029059A File Offset: 0x0028E79A
	public Assignables Owner { get; private set; }

	// Token: 0x06006D2A RID: 27946 RVA: 0x002905AB File Offset: 0x0028E7AB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnRowClicked));
		this.SetSelectedVisualState(false);
	}

	// Token: 0x06006D2B RID: 27947 RVA: 0x002905E1 File Offset: 0x0028E7E1
	private void OnRowClicked()
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(this);
	}

	// Token: 0x06006D2C RID: 27948 RVA: 0x002905F4 File Offset: 0x0028E7F4
	public void SetLockState(bool locked)
	{
		this.IsLocked = locked;
		this.Refresh();
	}

	// Token: 0x06006D2D RID: 27949 RVA: 0x00290604 File Offset: 0x0028E804
	public void SetData(Assignables owner, AssignableSlot slot, bool IsLocked)
	{
		if (this.Owner != null)
		{
			this.ClearData();
		}
		this.Owner = owner;
		this.Slot = slot;
		this.SlotInstance = owner.GetSlot(slot);
		this.subscribe_IDX = this.Owner.Subscribe(-1585839766, delegate(object o)
		{
			this.Refresh();
		});
		this.SetLockState(IsLocked);
		if (!IsLocked)
		{
			this.Refresh();
		}
	}

	// Token: 0x06006D2E RID: 27950 RVA: 0x00290674 File Offset: 0x0028E874
	public void ClearData()
	{
		if (this.Owner != null && this.subscribe_IDX != -1)
		{
			this.Owner.Unsubscribe(this.subscribe_IDX);
		}
		this.Owner = null;
		this.Slot = null;
		this.SlotInstance = null;
		this.IsLocked = false;
		this.subscribe_IDX = -1;
		this.DisplayAsEmpty();
	}

	// Token: 0x06006D2F RID: 27951 RVA: 0x002906D2 File Offset: 0x0028E8D2
	private void Refresh()
	{
		if (this.IsNullOrDestroyed())
		{
			return;
		}
		if (this.IsLocked)
		{
			this.DisplayAsLocked();
			return;
		}
		if (!this.SlotIsAssigned)
		{
			this.DisplayAsEmpty();
			return;
		}
		this.DisplayAsOccupied();
	}

	// Token: 0x06006D30 RID: 27952 RVA: 0x00290704 File Offset: 0x0028E904
	public void SetSelectedVisualState(bool shouldDisplayAsSelected)
	{
		int new_state_index = shouldDisplayAsSelected ? 1 : 0;
		this.toggle.ChangeState(new_state_index);
	}

	// Token: 0x06006D31 RID: 27953 RVA: 0x00290728 File Offset: 0x0028E928
	private void DisplayAsOccupied()
	{
		Assignable assignable = this.SlotInstance.assignable;
		string properName = assignable.GetProperName();
		string text = this.Slot.Name + ": " + properName;
		this.textLabel.SetText(text);
		this.itemIcon.sprite = Def.GetUISprite(assignable.gameObject, "ui", false).first;
		this.itemIcon.gameObject.SetActive(true);
		this.lockedIcon.gameObject.SetActive(false);
		InfoDescription component = assignable.gameObject.GetComponent<InfoDescription>();
		string simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED_GENERIC, properName);
		if (component != null && !string.IsNullOrEmpty(component.description))
		{
			simpleTooltip = string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.ITEM_ASSIGNED, properName, component.description);
		}
		this.tooltip.SetSimpleTooltip(simpleTooltip);
	}

	// Token: 0x06006D32 RID: 27954 RVA: 0x00290808 File Offset: 0x0028EA08
	private void DisplayAsEmpty()
	{
		this.textLabel.SetText(((this.Slot != null) ? (this.Slot.Name + ": ") : "") + OwnablesSidescreenItemRow.EMPTY_TEXT);
		this.lockedIcon.gameObject.SetActive(false);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.tooltip.SetSimpleTooltip((this.Slot != null) ? string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_ITEM_ASSIGNED, this.Slot.Name) : null);
	}

	// Token: 0x06006D33 RID: 27955 RVA: 0x002908AC File Offset: 0x0028EAAC
	private void DisplayAsLocked()
	{
		this.lockedIcon.gameObject.SetActive(true);
		this.itemIcon.sprite = null;
		this.itemIcon.gameObject.SetActive(false);
		this.textLabel.SetText(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_APPLICABLE, this.Slot.Name));
		this.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.OWNABLESSIDESCREEN.TOOLTIPS.NO_APPLICABLE, this.Slot.Name));
	}

	// Token: 0x06006D34 RID: 27956 RVA: 0x00290931 File Offset: 0x0028EB31
	protected override void OnCleanUp()
	{
		this.ClearData();
	}

	// Token: 0x04004A69 RID: 19049
	private static string EMPTY_TEXT = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_ASSIGNED;

	// Token: 0x04004A6A RID: 19050
	public KImage lockedIcon;

	// Token: 0x04004A6B RID: 19051
	public KImage itemIcon;

	// Token: 0x04004A6C RID: 19052
	public LocText textLabel;

	// Token: 0x04004A6D RID: 19053
	public ToolTip tooltip;

	// Token: 0x04004A6E RID: 19054
	[Header("Icon settings")]
	public KImage frameOuterBorder;

	// Token: 0x04004A6F RID: 19055
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x04004A74 RID: 19060
	public MultiToggle toggle;

	// Token: 0x04004A75 RID: 19061
	private int subscribe_IDX = -1;
}
