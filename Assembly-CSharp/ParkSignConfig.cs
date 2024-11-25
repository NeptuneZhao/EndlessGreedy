using System;
using TUNING;
using UnityEngine;

// Token: 0x0200032A RID: 810
public class ParkSignConfig : IBuildingConfig
{
	// Token: 0x060010EA RID: 4330 RVA: 0x0005F600 File Offset: 0x0005D800
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ParkSign", 1, 2, "parksign_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.ANY_BUILDABLE, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.NONE, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		return buildingDef;
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x0005F65A File Offset: 0x0005D85A
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Park, false);
		go.AddOrGet<ParkSign>();
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x0005F674 File Offset: 0x0005D874
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A58 RID: 2648
	public const string ID = "ParkSign";
}
