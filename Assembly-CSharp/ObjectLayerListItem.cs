using System;
using UnityEngine;

// Token: 0x020009CB RID: 2507
public class ObjectLayerListItem
{
	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x060048CE RID: 18638 RVA: 0x001A042A File Offset: 0x0019E62A
	// (set) Token: 0x060048CF RID: 18639 RVA: 0x001A0432 File Offset: 0x0019E632
	public ObjectLayerListItem previousItem { get; private set; }

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x060048D0 RID: 18640 RVA: 0x001A043B File Offset: 0x0019E63B
	// (set) Token: 0x060048D1 RID: 18641 RVA: 0x001A0443 File Offset: 0x0019E643
	public ObjectLayerListItem nextItem { get; private set; }

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x060048D2 RID: 18642 RVA: 0x001A044C File Offset: 0x0019E64C
	// (set) Token: 0x060048D3 RID: 18643 RVA: 0x001A0454 File Offset: 0x0019E654
	public GameObject gameObject { get; private set; }

	// Token: 0x060048D4 RID: 18644 RVA: 0x001A045D File Offset: 0x0019E65D
	public ObjectLayerListItem(GameObject gameObject, ObjectLayer layer, int new_cell)
	{
		this.gameObject = gameObject;
		this.layer = layer;
		this.Refresh(new_cell);
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x001A0486 File Offset: 0x0019E686
	public void Clear()
	{
		this.Refresh(Grid.InvalidCell);
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x001A0494 File Offset: 0x0019E694
	public bool Refresh(int new_cell)
	{
		if (this.cell != new_cell)
		{
			if (this.cell != Grid.InvalidCell && Grid.Objects[this.cell, (int)this.layer] == this.gameObject)
			{
				GameObject value = null;
				if (this.nextItem != null && this.nextItem.gameObject != null)
				{
					value = this.nextItem.gameObject;
				}
				Grid.Objects[this.cell, (int)this.layer] = value;
			}
			if (this.previousItem != null)
			{
				this.previousItem.nextItem = this.nextItem;
			}
			if (this.nextItem != null)
			{
				this.nextItem.previousItem = this.previousItem;
			}
			this.previousItem = null;
			this.nextItem = null;
			this.cell = new_cell;
			if (this.cell != Grid.InvalidCell)
			{
				GameObject gameObject = Grid.Objects[this.cell, (int)this.layer];
				if (gameObject != null && gameObject != this.gameObject)
				{
					ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
					this.nextItem = objectLayerListItem;
					objectLayerListItem.previousItem = this;
				}
				Grid.Objects[this.cell, (int)this.layer] = this.gameObject;
			}
			return true;
		}
		return false;
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x001A05D8 File Offset: 0x0019E7D8
	public bool Update(int cell)
	{
		return this.Refresh(cell);
	}

	// Token: 0x04002FA8 RID: 12200
	private int cell = Grid.InvalidCell;

	// Token: 0x04002FA9 RID: 12201
	private ObjectLayer layer;
}
