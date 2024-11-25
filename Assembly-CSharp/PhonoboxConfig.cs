using System;
using TUNING;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class PhonoboxConfig : IBuildingConfig
{
	// Token: 0x060010F1 RID: 4337 RVA: 0x0005F8D0 File Offset: 0x0005DAD0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Phonobox";
		int width = 5;
		int height = 3;
		string anim = "jukebot_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		return buildingDef;
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x0005F958 File Offset: 0x0005DB58
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<Phonobox>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x0005F9B1 File Offset: 0x0005DBB1
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A63 RID: 2659
	public const string ID = "Phonobox";
}
