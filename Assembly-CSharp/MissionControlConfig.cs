using System;
using TUNING;
using UnityEngine;

// Token: 0x020002F1 RID: 753
public class MissionControlConfig : IBuildingConfig
{
	// Token: 0x06000FCF RID: 4047 RVA: 0x0005A498 File Offset: 0x00058698
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x0005A4A0 File Offset: 0x000586A0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MissionControl";
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

	// Token: 0x06000FD1 RID: 4049 RVA: 0x0005A53C File Offset: 0x0005873C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		BuildingDef def = go.GetComponent<BuildingComplete>().Def;
		Prioritizable.AddRef(go);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGetDef<PoweredController.Def>();
		go.AddOrGetDef<SkyVisibilityMonitor.Def>().skyVisibilityInfo = MissionControlConfig.SKY_VISIBILITY_INFO;
		go.AddOrGetDef<MissionControl.Def>();
		MissionControlWorkable missionControlWorkable = go.AddOrGet<MissionControlWorkable>();
		missionControlWorkable.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		missionControlWorkable.workLayer = Grid.SceneLayer.BuildingUse;
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x0005A5BD File Offset: 0x000587BD
	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Laboratory.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x0005A5EB File Offset: 0x000587EB
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x0005A5F3 File Offset: 0x000587F3
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		MissionControlConfig.AddVisualizer(go);
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x0005A5FB File Offset: 0x000587FB
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 2;
		skyVisibilityVisualizer.RangeMin = -1;
		skyVisibilityVisualizer.RangeMax = 1;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x0400099B RID: 2459
	public const string ID = "MissionControl";

	// Token: 0x0400099C RID: 2460
	public const float EFFECT_DURATION = 600f;

	// Token: 0x0400099D RID: 2461
	public const float SPEED_MULTIPLIER = 1.2f;

	// Token: 0x0400099E RID: 2462
	public const int SCAN_RADIUS = 1;

	// Token: 0x0400099F RID: 2463
	public const int VERTICAL_SCAN_OFFSET = 2;

	// Token: 0x040009A0 RID: 2464
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 2), 1, new CellOffset(0, 2), 1, 0);
}
