using System;

// Token: 0x02000D89 RID: 3465
public class OwnablesSidescreenCategoryRow : KMonoBehaviour
{
	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06006D18 RID: 27928 RVA: 0x00290340 File Offset: 0x0028E540
	private AssignableSlot[] slots
	{
		get
		{
			return this.data.slots;
		}
	}

	// Token: 0x06006D19 RID: 27929 RVA: 0x0029034D File Offset: 0x0028E54D
	public void SetCategoryData(OwnablesSidescreenCategoryRow.Data categoryData)
	{
		this.DeleteAllRows();
		this.data = categoryData;
		this.titleLabel.text = categoryData.name;
	}

	// Token: 0x06006D1A RID: 27930 RVA: 0x0029036D File Offset: 0x0028E56D
	public void SetOwner(Assignables owner)
	{
		this.owner = owner;
		if (owner != null)
		{
			this.RecreateAllItemRows();
			return;
		}
		this.DeleteAllRows();
	}

	// Token: 0x06006D1B RID: 27931 RVA: 0x0029038C File Offset: 0x0028E58C
	private void RecreateAllItemRows()
	{
		this.DeleteAllRows();
		this.itemRows = new OwnablesSidescreenItemRow[this.slots.Length];
		IAssignableIdentity component = this.owner.gameObject.GetComponent<IAssignableIdentity>();
		for (int i = 0; i < this.slots.Length; i++)
		{
			AssignableSlot slot = this.slots[i];
			this.itemRows[i] = this.CreateRow(slot, component);
		}
	}

	// Token: 0x06006D1C RID: 27932 RVA: 0x002903F0 File Offset: 0x0028E5F0
	private OwnablesSidescreenItemRow CreateRow(AssignableSlot slot, IAssignableIdentity ownerIdentity)
	{
		this.originalItemRow.gameObject.SetActive(false);
		OwnablesSidescreenItemRow component = Util.KInstantiateUI(this.originalItemRow.gameObject, this.originalItemRow.transform.parent.gameObject, false).GetComponent<OwnablesSidescreenItemRow>();
		component.OnSlotRowClicked = (Action<OwnablesSidescreenItemRow>)Delegate.Combine(component.OnSlotRowClicked, new Action<OwnablesSidescreenItemRow>(this.OnRowClicked));
		component.gameObject.SetActive(true);
		component.SetData(this.owner, slot, !this.data.IsSlotApplicable(ownerIdentity, slot));
		return component;
	}

	// Token: 0x06006D1D RID: 27933 RVA: 0x00290484 File Offset: 0x0028E684
	private void OnRowClicked(OwnablesSidescreenItemRow row)
	{
		Action<OwnablesSidescreenItemRow> onSlotRowClicked = this.OnSlotRowClicked;
		if (onSlotRowClicked == null)
		{
			return;
		}
		onSlotRowClicked(row);
	}

	// Token: 0x06006D1E RID: 27934 RVA: 0x00290498 File Offset: 0x0028E698
	private void DeleteAllRows()
	{
		this.originalItemRow.gameObject.SetActive(false);
		if (this.itemRows != null)
		{
			for (int i = 0; i < this.itemRows.Length; i++)
			{
				this.itemRows[i].ClearData();
				this.itemRows[i].DeleteObject();
			}
			this.itemRows = null;
		}
	}

	// Token: 0x06006D1F RID: 27935 RVA: 0x002904F4 File Offset: 0x0028E6F4
	public void SetSelectedRow_VisualsOnly(AssignableSlotInstance slotInstance)
	{
		if (this.itemRows == null)
		{
			return;
		}
		for (int i = 0; i < this.itemRows.Length; i++)
		{
			OwnablesSidescreenItemRow ownablesSidescreenItemRow = this.itemRows[i];
			ownablesSidescreenItemRow.SetSelectedVisualState(ownablesSidescreenItemRow.SlotInstance == slotInstance);
		}
	}

	// Token: 0x04004A63 RID: 19043
	public Action<OwnablesSidescreenItemRow> OnSlotRowClicked;

	// Token: 0x04004A64 RID: 19044
	public LocText titleLabel;

	// Token: 0x04004A65 RID: 19045
	public OwnablesSidescreenItemRow originalItemRow;

	// Token: 0x04004A66 RID: 19046
	private Assignables owner;

	// Token: 0x04004A67 RID: 19047
	private OwnablesSidescreenCategoryRow.Data data;

	// Token: 0x04004A68 RID: 19048
	private OwnablesSidescreenItemRow[] itemRows;

	// Token: 0x02001EAC RID: 7852
	public struct AssignableSlotData
	{
		// Token: 0x0600AC16 RID: 44054 RVA: 0x003A67D3 File Offset: 0x003A49D3
		public AssignableSlotData(AssignableSlot slot, Func<IAssignableIdentity, bool> isApplicableCallback)
		{
			this.slot = slot;
			this.IsApplicableCallback = isApplicableCallback;
		}

		// Token: 0x04008B31 RID: 35633
		public AssignableSlot slot;

		// Token: 0x04008B32 RID: 35634
		public Func<IAssignableIdentity, bool> IsApplicableCallback;
	}

	// Token: 0x02001EAD RID: 7853
	public struct Data
	{
		// Token: 0x0600AC17 RID: 44055 RVA: 0x003A67E4 File Offset: 0x003A49E4
		public Data(string name, OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData)
		{
			this.name = name;
			this.slotsData = slotsData;
			this.slots = new AssignableSlot[slotsData.Length];
			for (int i = 0; i < slotsData.Length; i++)
			{
				this.slots[i] = slotsData[i].slot;
			}
		}

		// Token: 0x0600AC18 RID: 44056 RVA: 0x003A6830 File Offset: 0x003A4A30
		public bool IsSlotApplicable(IAssignableIdentity identity, AssignableSlot slot)
		{
			for (int i = 0; i < this.slotsData.Length; i++)
			{
				OwnablesSidescreenCategoryRow.AssignableSlotData assignableSlotData = this.slotsData[i];
				if (assignableSlotData.slot == slot)
				{
					return assignableSlotData.IsApplicableCallback(identity);
				}
			}
			return false;
		}

		// Token: 0x04008B33 RID: 35635
		public string name;

		// Token: 0x04008B34 RID: 35636
		public AssignableSlot[] slots;

		// Token: 0x04008B35 RID: 35637
		private OwnablesSidescreenCategoryRow.AssignableSlotData[] slotsData;
	}
}
