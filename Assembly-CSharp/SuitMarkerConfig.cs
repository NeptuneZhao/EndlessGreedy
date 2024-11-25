using System;
using TUNING;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class SuitMarkerConfig : IBuildingConfig
{
	// Token: 0x060014AE RID: 5294 RVA: 0x00071E88 File Offset: 0x00070088
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SuitMarker";
		int width = 1;
		int height = 3;
		string anim = "changingarea_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "SuitMarker");
		return buildingDef;
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x00071F00 File Offset: 0x00070100
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("SuitLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasAtmoSuit;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("SuitMarker"),
			new Tag("SuitLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x00071F76 File Offset: 0x00070176
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000BCA RID: 3018
	public const string ID = "SuitMarker";
}
