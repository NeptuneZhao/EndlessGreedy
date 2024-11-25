using System;
using TUNING;
using UnityEngine;

// Token: 0x02000322 RID: 802
public class OxygenMaskMarkerConfig : IBuildingConfig
{
	// Token: 0x060010C8 RID: 4296 RVA: 0x0005E8E4 File Offset: 0x0005CAE4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OxygenMaskMarker";
		int width = 1;
		int height = 2;
		string anim = "oxygen_checkpoint_arrow_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] construction_materials = raw_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "OxygenMaskMarker");
		return buildingDef;
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x0005E95C File Offset: 0x0005CB5C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		SuitMarker suitMarker = go.AddOrGet<SuitMarker>();
		suitMarker.LockerTags = new Tag[]
		{
			new Tag("OxygenMaskLocker")
		};
		suitMarker.PathFlag = PathFinder.PotentialPath.Flags.HasOxygenMask;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("OxygenMaskMarker"),
			new Tag("OxygenMaskLocker")
		};
		go.AddTag(GameTags.JetSuitBlocker);
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0005E9D2 File Offset: 0x0005CBD2
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000A41 RID: 2625
	public const string ID = "OxygenMaskMarker";
}
