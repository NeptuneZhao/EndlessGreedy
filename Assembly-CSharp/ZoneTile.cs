using System;
using ProcGen;
using UnityEngine;

// Token: 0x02000B68 RID: 2920
[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
	// Token: 0x060057BA RID: 22458 RVA: 0x001F5970 File Offset: 0x001F3B70
	protected override void OnSpawn()
	{
		int[] placementCells = this.building.PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ModifyCellWorldZone(placementCells[i], 0);
		}
		base.Subscribe<ZoneTile>(1606648047, ZoneTile.OnObjectReplacedDelegate);
	}

	// Token: 0x060057BB RID: 22459 RVA: 0x001F59B1 File Offset: 0x001F3BB1
	protected override void OnCleanUp()
	{
		if (!this.wasReplaced)
		{
			this.ClearZone();
		}
	}

	// Token: 0x060057BC RID: 22460 RVA: 0x001F59C1 File Offset: 0x001F3BC1
	private void OnObjectReplaced(object data)
	{
		this.ClearZone();
		this.wasReplaced = true;
	}

	// Token: 0x060057BD RID: 22461 RVA: 0x001F59D0 File Offset: 0x001F3BD0
	private void ClearZone()
	{
		foreach (int num in this.building.PlacementCells)
		{
			GameObject gameObject;
			if (!Grid.ObjectLayers[(int)this.building.Def.ObjectLayer].TryGetValue(num, out gameObject) || !(gameObject != base.gameObject) || !(gameObject != null) || !(gameObject.GetComponent<ZoneTile>() != null))
			{
				SubWorld.ZoneType subWorldZoneType = global::World.Instance.zoneRenderData.GetSubWorldZoneType(num);
				byte zone_id = (subWorldZoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)subWorldZoneType);
				SimMessages.ModifyCellWorldZone(num, zone_id);
			}
		}
	}

	// Token: 0x04003954 RID: 14676
	[MyCmpReq]
	public Building building;

	// Token: 0x04003955 RID: 14677
	private bool wasReplaced;

	// Token: 0x04003956 RID: 14678
	private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>(delegate(ZoneTile component, object data)
	{
		component.OnObjectReplaced(data);
	});
}
