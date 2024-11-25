using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class ArcadeMachineConfig : IBuildingConfig
{
	// Token: 0x06000075 RID: 117 RVA: 0x00005034 File Offset: 0x00003234
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ArcadeMachine";
		int width = 3;
		int height = 3;
		string anim = "arcade_cabinet_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 1200f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		return buildingDef;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000050BC File Offset: 0x000032BC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<ArcadeMachine>();
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0000510E File Offset: 0x0000330E
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000061 RID: 97
	public const string ID = "ArcadeMachine";

	// Token: 0x04000062 RID: 98
	public const string SPECIFIC_EFFECT = "PlayedArcade";

	// Token: 0x04000063 RID: 99
	public const string TRACKING_EFFECT = "RecentlyPlayedArcade";
}
