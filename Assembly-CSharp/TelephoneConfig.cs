using System;
using TUNING;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class TelephoneConfig : IBuildingConfig
{
	// Token: 0x060014C6 RID: 5318 RVA: 0x00072B21 File Offset: 0x00070D21
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x00072B28 File Offset: 0x00070D28
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Telephone";
		int width = 1;
		int height = 2;
		string anim = "telephone_kanim";
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
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		return buildingDef;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x00072BBC File Offset: 0x00070DBC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		Telephone telephone = go.AddOrGet<Telephone>();
		telephone.babbleEffect = "TelephoneBabble";
		telephone.chatEffect = "TelephoneChat";
		telephone.longDistanceEffect = "TelephoneLongDistance";
		telephone.trackingEffect = "RecentlyTelephoned";
		go.AddOrGet<TelephoneCallerWorkable>().basePriority = RELAXATION.PRIORITY.TIER5;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x00072C48 File Offset: 0x00070E48
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000BD4 RID: 3028
	public const string ID = "Telephone";

	// Token: 0x04000BD5 RID: 3029
	public const float ringTime = 15f;

	// Token: 0x04000BD6 RID: 3030
	public const float callTime = 25f;
}
