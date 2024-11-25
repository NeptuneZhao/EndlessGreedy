using System;

// Token: 0x020008AE RID: 2222
public class FakeFloorAdder : KMonoBehaviour
{
	// Token: 0x06003E07 RID: 15879 RVA: 0x001569BC File Offset: 0x00154BBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.initiallyActive)
		{
			this.SetFloor(true);
		}
	}

	// Token: 0x06003E08 RID: 15880 RVA: 0x001569D4 File Offset: 0x00154BD4
	public void SetFloor(bool active)
	{
		if (this.isActive == active)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		Building component = base.GetComponent<Building>();
		foreach (CellOffset offset in this.floorOffsets)
		{
			CellOffset rotatedOffset = component.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			if (active)
			{
				Grid.FakeFloor.Add(num);
			}
			else
			{
				Grid.FakeFloor.Remove(num);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
		this.isActive = active;
	}

	// Token: 0x06003E09 RID: 15881 RVA: 0x00156A5B File Offset: 0x00154C5B
	protected override void OnCleanUp()
	{
		this.SetFloor(false);
		base.OnCleanUp();
	}

	// Token: 0x04002618 RID: 9752
	public CellOffset[] floorOffsets;

	// Token: 0x04002619 RID: 9753
	public bool initiallyActive = true;

	// Token: 0x0400261A RID: 9754
	private bool isActive;
}
