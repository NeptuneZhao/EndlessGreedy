using System;
using TUNING;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class WireBridgeConfig : IBuildingConfig
{
	// Token: 0x06001581 RID: 5505 RVA: 0x000762AC File Offset: 0x000744AC
	protected virtual string GetID()
	{
		return "WireBridge";
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x000762B4 File Offset: 0x000744B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = this.GetID();
		int width = 3;
		int height = 1;
		string anim = "utilityelectricbridge_kanim";
		int hitpoints = 30;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.WireBridge;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.ObjectLayer = ObjectLayer.WireConnectors;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireBridge");
		return buildingDef;
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x0007637D File Offset: 0x0007457D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x00076385 File Offset: 0x00074585
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x000763A3 File Offset: 0x000745A3
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x000763C0 File Offset: 0x000745C0
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000763D6 File Offset: 0x000745D6
	protected virtual WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = go.AddOrGet<WireUtilityNetworkLink>();
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max1000;
		wireUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		wireUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return wireUtilityNetworkLink;
	}

	// Token: 0x04000C31 RID: 3121
	public const string ID = "WireBridge";
}
