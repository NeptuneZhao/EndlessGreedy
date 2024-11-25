using System;
using TUNING;
using UnityEngine;

// Token: 0x02000234 RID: 564
public class LadderBedConfig : IBuildingConfig
{
	// Token: 0x06000BAA RID: 2986 RVA: 0x00044B54 File Offset: 0x00042D54
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00044B5C File Offset: 0x00042D5C
	public override BuildingDef CreateBuildingDef()
	{
		string id = LadderBedConfig.ID;
		int width = 2;
		int height = 2;
		string anim = "ladder_bed_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloorOrBuildingAttachPoint;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.AttachmentSlotTag = GameTags.LadderBed;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		return buildingDef;
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00044BDC File Offset: 0x00042DDC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BedType, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 2), GameTags.LadderBed, null)
		};
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00044C34 File Offset: 0x00042E34
	public override void DoPostConfigureComplete(GameObject go)
	{
		CellOffset[] offsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		};
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 0.75f;
		ladder.downwardsMovementSpeedMultiplier = 0.75f;
		ladder.offsets = offsets;
		go.AddOrGetDef<LadderBed.Def>().offsets = offsets;
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		Bed bed = go.AddOrGet<Bed>();
		bed.effects = new string[]
		{
			"LadderBedStamina",
			"BedHealth"
		};
		bed.workLayer = Grid.SceneLayer.BuildingFront;
		Sleepable sleepable = go.AddOrGet<Sleepable>();
		sleepable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_ladder_bed_kanim")
		};
		sleepable.workLayer = Grid.SceneLayer.BuildingFront;
		go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.Bed.Id;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
	}

	// Token: 0x040007B3 RID: 1971
	public static string ID = "LadderBed";
}
