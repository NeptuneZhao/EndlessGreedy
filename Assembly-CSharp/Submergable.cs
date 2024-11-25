using System;
using UnityEngine;

// Token: 0x020005CA RID: 1482
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Submergable")]
public class Submergable : KMonoBehaviour
{
	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06002423 RID: 9251 RVA: 0x000C9D84 File Offset: 0x000C7F84
	public bool IsSubmerged
	{
		get
		{
			return this.isSubmerged;
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06002424 RID: 9252 RVA: 0x000C9D8C File Offset: 0x000C7F8C
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x06002425 RID: 9253 RVA: 0x000C9D9C File Offset: 0x000C7F9C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Submergable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
		this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
	}

	// Token: 0x06002426 RID: 9254 RVA: 0x000C9E30 File Offset: 0x000C8030
	private void OnElementChanged(object data)
	{
		bool flag = true;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (!Grid.IsLiquid(this.building.PlacementCells[i]))
			{
				flag = false;
				break;
			}
		}
		if (flag != this.isSubmerged)
		{
			this.isSubmerged = flag;
			this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
		}
	}

	// Token: 0x06002427 RID: 9255 RVA: 0x000C9EBA File Offset: 0x000C80BA
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x04001499 RID: 5273
	[MyCmpReq]
	private Building building;

	// Token: 0x0400149A RID: 5274
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400149B RID: 5275
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x0400149C RID: 5276
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400149D RID: 5277
	public static Operational.Flag notSubmergedFlag = new Operational.Flag("submerged", Operational.Flag.Type.Functional);

	// Token: 0x0400149E RID: 5278
	private bool isSubmerged;

	// Token: 0x0400149F RID: 5279
	private HandleVector<int>.Handle partitionerEntry;
}
