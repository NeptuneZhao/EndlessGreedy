using System;
using UnityEngine;

// Token: 0x020009D2 RID: 2514
public class SocialChoreTracker
{
	// Token: 0x06004913 RID: 18707 RVA: 0x001A2A94 File Offset: 0x001A0C94
	public SocialChoreTracker(GameObject owner, CellOffset[] chore_offsets)
	{
		this.owner = owner;
		this.choreOffsets = chore_offsets;
		this.chores = new Chore[this.choreOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(owner), this.choreOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("PrintingPodSocialize", owner, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x001A2B08 File Offset: 0x001A0D08
	public void Update(bool update = true)
	{
		if (this.updating)
		{
			return;
		}
		this.updating = true;
		int num = 0;
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			CellOffset offset = this.choreOffsets[i];
			Chore chore = this.chores[i];
			if (update && num < this.choreCount && this.IsOffsetValid(offset))
			{
				num++;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = ((this.CreateChoreCB != null) ? this.CreateChoreCB(i) : null);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
		this.updating = false;
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x001A2BB9 File Offset: 0x001A0DB9
	private void OnCellChanged(object data)
	{
		if (this.owner.HasTag(GameTags.Operational))
		{
			this.Update(true);
		}
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x001A2BD4 File Offset: 0x001A0DD4
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.Update(false);
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x001A2BF0 File Offset: 0x001A0DF0
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this.owner), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, true);
	}

	// Token: 0x04002FD0 RID: 12240
	public Func<int, Chore> CreateChoreCB;

	// Token: 0x04002FD1 RID: 12241
	public int choreCount;

	// Token: 0x04002FD2 RID: 12242
	private GameObject owner;

	// Token: 0x04002FD3 RID: 12243
	private CellOffset[] choreOffsets;

	// Token: 0x04002FD4 RID: 12244
	private Chore[] chores;

	// Token: 0x04002FD5 RID: 12245
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04002FD6 RID: 12246
	private bool updating;
}
