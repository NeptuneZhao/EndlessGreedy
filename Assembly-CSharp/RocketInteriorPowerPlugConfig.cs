using System;
using TUNING;
using UnityEngine;

// Token: 0x02000394 RID: 916
public class RocketInteriorPowerPlugConfig : IBuildingConfig
{
	// Token: 0x06001313 RID: 4883 RVA: 0x00069D2C File Offset: 0x00067F2C
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x00069D34 File Offset: 0x00067F34
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RocketInteriorPowerPlug";
		int width = 1;
		int height = 1;
		string anim = "rocket_floor_plug_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnRocketEnvelope;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.RequiresPowerOutput = true;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "RocketInteriorPowerPlug");
		return buildingDef;
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x00069DE8 File Offset: 0x00067FE8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.GetComponent<KPrefabID>().AddTag(GameTags.RocketInteriorBuilding, false);
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x00069E0A File Offset: 0x0006800A
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<OperationalController.Def>();
		go.AddOrGet<WireUtilitySemiVirtualNetworkLink>().link1 = new CellOffset(0, 0);
	}

	// Token: 0x04000B18 RID: 2840
	public const string ID = "RocketInteriorPowerPlug";
}
