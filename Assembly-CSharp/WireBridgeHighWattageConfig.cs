using System;
using TUNING;
using UnityEngine;

// Token: 0x020003FD RID: 1021
public class WireBridgeHighWattageConfig : IBuildingConfig
{
	// Token: 0x06001589 RID: 5513 RVA: 0x00076407 File Offset: 0x00074607
	protected virtual string GetID()
	{
		return "WireBridgeHighWattage";
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x00076410 File Offset: 0x00074610
	public override BuildingDef CreateBuildingDef()
	{
		string id = this.GetID();
		int width = 1;
		int height = 1;
		string anim = "heavywatttile_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.HighWattBridgeTile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER5, none, 0.2f);
		BuildingTemplates.CreateFoundationTileDef(buildingDef);
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridgesFront;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireBridgeHighWattage");
		return buildingDef;
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x000764F0 File Offset: 0x000746F0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
		simCellOccupier.doReplaceElement = true;
		simCellOccupier.movementSpeedMultiplier = DUPLICANTSTATS.MOVEMENT_MODIFIERS.PENALTY_3;
		simCellOccupier.notifyOnMelt = true;
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddOrGet<TileTemperature>();
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x00076549 File Offset: 0x00074749
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x00076567 File Offset: 0x00074767
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AddNetworkLink(go).visualizeOnly = true;
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x00076584 File Offset: 0x00074784
	public override void DoPostConfigureComplete(GameObject go)
	{
		this.AddNetworkLink(go).visualizeOnly = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.WireBridges, false);
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x000765AB File Offset: 0x000747AB
	protected virtual WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = go.AddOrGet<WireUtilityNetworkLink>();
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max20000;
		wireUtilityNetworkLink.link1 = new CellOffset(-1, 0);
		wireUtilityNetworkLink.link2 = new CellOffset(1, 0);
		return wireUtilityNetworkLink;
	}

	// Token: 0x04000C32 RID: 3122
	public const string ID = "WireBridgeHighWattage";
}
