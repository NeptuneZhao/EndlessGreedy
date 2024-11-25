using System;
using TUNING;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class LadderFastConfig : IBuildingConfig
{
	// Token: 0x06000BB4 RID: 2996 RVA: 0x00044DF4 File Offset: 0x00042FF4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LadderFast";
		int width = 1;
		int height = 1;
		string anim = "ladder_plastic_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateLadderDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00044E7D File Offset: 0x0004307D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1.2f;
		ladder.downwardsMovementSpeedMultiplier = 1.2f;
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00044EA7 File Offset: 0x000430A7
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040007B5 RID: 1973
	public const string ID = "LadderFast";
}
