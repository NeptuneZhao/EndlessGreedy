using System;
using TUNING;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class MissionControlClusterConfig : IBuildingConfig
{
	// Token: 0x06000FC6 RID: 4038 RVA: 0x0005A2E6 File Offset: 0x000584E6
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x0005A2F0 File Offset: 0x000584F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControlCluster";
		int width = 3;
		int height = 3;
		string anim = "mission_control_station_kanim";
		int hitpoints = 100;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.DefaultAnimState = "off";
		return buildingDef;
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x0005A38C File Offset: 0x0005858C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlClusterConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControlCluster.Def>();
		MissionControlClusterWorkable missionControlClusterWorkable = go.AddOrGet<MissionControlClusterWorkable>();
		missionControlClusterWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlClusterWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x0005A40D File Offset: 0x0005860D
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x0005A43B File Offset: 0x0005863B
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x0005A443 File Offset: 0x00058643
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlClusterConfig.AddVisualizer(go);
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x0005A44B File Offset: 0x0005864B
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x04000994 RID: 2452
	public const string ID = "MissionControlCluster";

	// Token: 0x04000995 RID: 2453
	public const int WORK_RANGE_RADIUS = 2;

	// Token: 0x04000996 RID: 2454
	public const float EFFECT_DURATION = 600f;

	// Token: 0x04000997 RID: 2455
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x04000998 RID: 2456
	public const int SCAN_RADIUS = 1;

	// Token: 0x04000999 RID: 2457
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x0400099A RID: 2458
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
