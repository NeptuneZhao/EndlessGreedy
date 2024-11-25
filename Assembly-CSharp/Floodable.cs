using System;
using UnityEngine;

// Token: 0x02000568 RID: 1384
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Floodable")]
public class Floodable : KMonoBehaviour
{
	// Token: 0x1700014D RID: 333
	// (get) Token: 0x0600200D RID: 8205 RVA: 0x000B4937 File Offset: 0x000B2B37
	public bool IsFlooded
	{
		get
		{
			return this.isFlooded;
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x0600200E RID: 8206 RVA: 0x000B493F File Offset: 0x000B2B3F
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x000B494C File Offset: 0x000B2B4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Floodable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x000B49A4 File Offset: 0x000B2BA4
	private void OnElementChanged(object data)
	{
		bool flag = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (Grid.IsSubstantialLiquid(this.building.PlacementCells[i], 0.35f))
			{
				flag = true;
				break;
			}
		}
		if (flag != this.isFlooded)
		{
			this.isFlooded = flag;
			this.operational.SetFlag(Floodable.notFloodedFlag, !this.isFlooded);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Flooded, this.isFlooded, this);
		}
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x000B4A33 File Offset: 0x000B2C33
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04001220 RID: 4640
	[MyCmpReq]
	private Building building;

	// Token: 0x04001221 RID: 4641
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04001222 RID: 4642
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x04001223 RID: 4643
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001224 RID: 4644
	public static Operational.Flag notFloodedFlag = new Operational.Flag("not_flooded", Operational.Flag.Type.Functional);

	// Token: 0x04001225 RID: 4645
	private bool isFlooded;

	// Token: 0x04001226 RID: 4646
	private HandleVector<int>.Handle partitionerEntry;
}
