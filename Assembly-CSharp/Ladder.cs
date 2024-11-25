using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006F6 RID: 1782
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Ladder")]
public class Ladder : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06002D8B RID: 11659 RVA: 0x000FF9C4 File Offset: 0x000FDBC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.offsets)
		{
			CellOffset offset = cellOffset;
			if (component != null)
			{
				offset = component.GetRotatedCellOffset(cellOffset);
			}
			int i2 = Grid.OffsetCell(Grid.PosToCell(this), offset);
			Grid.HasPole[i2] = this.isPole;
			Grid.HasLadder[i2] = !this.isPole;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Ladders, false);
		Components.Ladders.Add(this);
	}

	// Token: 0x06002D8C RID: 11660 RVA: 0x000FFA62 File Offset: 0x000FDC62
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x000FFA98 File Offset: 0x000FDC98
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.offsets)
		{
			CellOffset offset = cellOffset;
			if (component != null)
			{
				offset = component.GetRotatedCellOffset(cellOffset);
			}
			int num = Grid.OffsetCell(Grid.PosToCell(this), offset);
			if (Grid.Objects[num, 24] == null)
			{
				Grid.HasPole[num] = false;
				Grid.HasLadder[num] = false;
			}
		}
		Components.Ladders.Remove(this);
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x000FFB30 File Offset: 0x000FDD30
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = null;
		if (this.upwardsMovementSpeedMultiplier != 1f)
		{
			list = new List<Descriptor>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.upwardsMovementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.upwardsMovementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x04001A72 RID: 6770
	public float upwardsMovementSpeedMultiplier = 1f;

	// Token: 0x04001A73 RID: 6771
	public float downwardsMovementSpeedMultiplier = 1f;

	// Token: 0x04001A74 RID: 6772
	public bool isPole;

	// Token: 0x04001A75 RID: 6773
	public CellOffset[] offsets = new CellOffset[]
	{
		CellOffset.none
	};
}
