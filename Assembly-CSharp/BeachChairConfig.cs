using System;
using TUNING;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class BeachChairConfig : IBuildingConfig
{
	// Token: 0x060000B9 RID: 185 RVA: 0x000064B0 File Offset: 0x000046B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BeachChair";
		int width = 2;
		int height = 3;
		string anim = "beach_chair_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			400f,
			2f
		};
		string[] construction_materials = new string[]
		{
			"BuildableRaw",
			"BuildingFiber"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER4, none, 0.2f);
		buildingDef.Floodable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = true;
		return buildingDef;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00006534 File Offset: 0x00004734
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RecBuilding, false);
		go.AddOrGet<BeachChairWorkable>().basePriority = RELAXATION.PRIORITY.TIER4;
		BeachChair beachChair = go.AddOrGet<BeachChair>();
		beachChair.specificEffectUnlit = "BeachChairUnlit";
		beachChair.specificEffectLit = "BeachChairLit";
		beachChair.trackingEffect = "RecentlyBeachChair";
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.RecRoom.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
		go.AddOrGet<AnimTileable>();
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x060000BB RID: 187 RVA: 0x000065BC File Offset: 0x000047BC
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400007E RID: 126
	public const string ID = "BeachChair";

	// Token: 0x0400007F RID: 127
	public static readonly int TAN_LUX = DUPLICANTSTATS.STANDARD.Light.HIGH_LIGHT;

	// Token: 0x04000080 RID: 128
	private const float TANK_SIZE_KG = 20f;

	// Token: 0x04000081 RID: 129
	private const float SPILL_RATE_KG = 0.05f;
}
