using System;
using TUNING;
using UnityEngine;

// Token: 0x0200022E RID: 558
public class JetSuitMarkerConfig : IBuildingConfig
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x00043CC8 File Offset: 0x00041EC8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "JetSuitMarker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_jetsuit_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingUse;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitMarker");
		return buildingDef;
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00043D5C File Offset: 0x00041F5C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("JetSuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasJetPack;
		suitMarker.interactAnim = Assets.GetAnim("anim_interacts_changingarea_jetsuit_arrow_kanim");
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("JetSuitMarker"),
			new Tag("JetSuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00043DE7 File Offset: 0x00041FE7
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x0400079F RID: 1951
	public const string ID = "JetSuitMarker";
}
