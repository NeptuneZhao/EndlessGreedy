using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Placeable")]
public class Placeable : KMonoBehaviour
{
	// Token: 0x06002246 RID: 8774 RVA: 0x000BEB28 File Offset: 0x000BCD28
	public bool IsValidPlaceLocation(int cell, out string reason)
	{
		if (this.placementRules.Contains(Placeable.PlacementRules.RestrictToWorld) && (int)Grid.WorldIdx[cell] != this.restrictWorldId)
		{
			reason = UI.TOOLS.PLACE.REASONS.RESTRICT_TO_WORLD;
			return false;
		}
		if (!this.occupyArea.CanOccupyArea(cell, this.occupyArea.objectLayers[0]))
		{
			reason = UI.TOOLS.PLACE.REASONS.CAN_OCCUPY_AREA;
			return false;
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.OnFoundation))
		{
			bool flag = this.occupyArea.TestAreaBelow(cell, null, new Func<int, object, bool>(this.FoundationTest));
			if (this.checkRootCellOnly)
			{
				flag = this.FoundationTest(Grid.CellBelow(cell), null);
			}
			if (!flag)
			{
				reason = UI.TOOLS.PLACE.REASONS.ON_FOUNDATION;
				return false;
			}
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.VisibleToSpace))
		{
			bool flag2 = this.occupyArea.TestArea(cell, null, new Func<int, object, bool>(this.SunnySpaceTest));
			if (this.checkRootCellOnly)
			{
				flag2 = this.SunnySpaceTest(cell, null);
			}
			if (!flag2)
			{
				reason = UI.TOOLS.PLACE.REASONS.VISIBLE_TO_SPACE;
				return false;
			}
		}
		reason = "ok!";
		return true;
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000BEC2C File Offset: 0x000BCE2C
	private bool SunnySpaceTest(int cell, object data)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int x;
		int startY;
		Grid.CellToXY(cell, out x, out startY);
		int num = (int)Grid.WorldIdx[cell];
		if (num == 255)
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(num);
		int top = world.WorldOffset.y + world.WorldSize.y;
		return !Grid.Solid[cell] && !Grid.Foundation[cell] && (Grid.ExposedToSunlight[cell] >= 253 || this.ClearPathToSky(x, startY, top));
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000BECC0 File Offset: 0x000BCEC0
	private bool ClearPathToSky(int x, int startY, int top)
	{
		for (int i = startY; i < top; i++)
		{
			int i2 = Grid.XYToCell(x, i);
			if (Grid.Solid[i2] || Grid.Foundation[i2])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000BECFE File Offset: 0x000BCEFE
	private bool FoundationTest(int cell, object data)
	{
		return Grid.IsValidBuildingCell(cell) && (Grid.Solid[cell] || Grid.Foundation[cell]);
	}

	// Token: 0x04001346 RID: 4934
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x04001347 RID: 4935
	public string kAnimName;

	// Token: 0x04001348 RID: 4936
	public string animName;

	// Token: 0x04001349 RID: 4937
	public List<Placeable.PlacementRules> placementRules = new List<Placeable.PlacementRules>();

	// Token: 0x0400134A RID: 4938
	[NonSerialized]
	public int restrictWorldId;

	// Token: 0x0400134B RID: 4939
	public bool checkRootCellOnly;

	// Token: 0x02001394 RID: 5012
	public enum PlacementRules
	{
		// Token: 0x0400670D RID: 26381
		OnFoundation,
		// Token: 0x0400670E RID: 26382
		VisibleToSpace,
		// Token: 0x0400670F RID: 26383
		RestrictToWorld
	}
}
