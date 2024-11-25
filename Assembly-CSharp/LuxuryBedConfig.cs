using System;
using TUNING;
using UnityEngine;

// Token: 0x0200027F RID: 639
public class LuxuryBedConfig : IBuildingConfig
{
	// Token: 0x06000D34 RID: 3380 RVA: 0x0004BC58 File Offset: 0x00049E58
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LuxuryBed";
		int width = 4;
		int height = 2;
		string anim = "elegantbed_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x0004BCB0 File Offset: 0x00049EB0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LuxuryBedType, false);
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x0004BCDC File Offset: 0x00049EDC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		Bed bed = go.AddOrGet<Bed>();
		bed.effects = new string[]
		{
			"LuxuryBedStamina",
			"BedHealth"
		};
		bed.workLayer = Grid.SceneLayer.BuildingFront;
		Sleepable sleepable = go.AddOrGet<Sleepable>();
		sleepable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_sleep_bed_kanim")
		};
		sleepable.workLayer = Grid.SceneLayer.BuildingFront;
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Bed.Id;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x04000837 RID: 2103
	public const string ID = "LuxuryBed";
}
