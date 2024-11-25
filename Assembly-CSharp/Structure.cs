using System;
using UnityEngine;

// Token: 0x020005C8 RID: 1480
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Structure")]
public class Structure : KMonoBehaviour
{
	// Token: 0x06002406 RID: 9222 RVA: 0x000C9752 File Offset: 0x000C7952
	public bool IsEntombed()
	{
		return this.isEntombed;
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000C975C File Offset: 0x000C795C
	public static bool IsBuildingEntombed(Building building)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(building)))
		{
			return false;
		}
		for (int i = 0; i < building.PlacementCells.Length; i++)
		{
			int num = building.PlacementCells[i];
			if (Grid.Element[num].IsSolid && !Grid.Foundation[num])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000C97B4 File Offset: 0x000C79B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Extents extents = this.building.GetExtents();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Structure.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		base.Subscribe<Structure>(-887025858, Structure.RocketLandedDelegate);
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x000C981D File Offset: 0x000C7A1D
	public void UpdatePosition()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.building.GetExtents());
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x000C983A File Offset: 0x000C7A3A
	private void RocketChanged(object data)
	{
		this.OnSolidChanged(data);
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000C9844 File Offset: 0x000C7A44
	private void OnSolidChanged(object data)
	{
		bool flag = Structure.IsBuildingEntombed(this.building);
		if (flag != this.isEntombed)
		{
			this.isEntombed = flag;
			if (this.isEntombed)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			}
			this.operational.SetFlag(Structure.notEntombedFlag, !this.isEntombed);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, this.isEntombed, this);
			base.Trigger(-1089732772, null);
		}
	}

	// Token: 0x0600240C RID: 9228 RVA: 0x000C98DF File Offset: 0x000C7ADF
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04001489 RID: 5257
	[MyCmpReq]
	private Building building;

	// Token: 0x0400148A RID: 5258
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400148B RID: 5259
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400148C RID: 5260
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x0400148D RID: 5261
	private bool isEntombed;

	// Token: 0x0400148E RID: 5262
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400148F RID: 5263
	private static EventSystem.IntraObjectHandler<Structure> RocketLandedDelegate = new EventSystem.IntraObjectHandler<Structure>(delegate(Structure cmp, object data)
	{
		cmp.RocketChanged(data);
	});
}
