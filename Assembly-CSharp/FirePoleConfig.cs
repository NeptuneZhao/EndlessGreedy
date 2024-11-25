using System;
using TUNING;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class FirePoleConfig : IBuildingConfig
{
	// Token: 0x06000686 RID: 1670 RVA: 0x0002C288 File Offset: 0x0002A488
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FirePole";
		int width = 1;
		int height = 1;
		string anim = "firepole_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateLadderDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0002C311 File Offset: 0x0002A511
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.isPole = true;
		ladder.upwardsMovementSpeedMultiplier = 0.25f;
		ladder.downwardsMovementSpeedMultiplier = 4f;
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x0002C342 File Offset: 0x0002A542
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040004AF RID: 1199
	public const string ID = "FirePole";
}
