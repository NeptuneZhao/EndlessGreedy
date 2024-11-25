using System;
using TUNING;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class HEPBridgeTileConfig : IBuildingConfig
{
	// Token: 0x06000AC8 RID: 2760 RVA: 0x00040525 File Offset: 0x0003E725
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0004052C File Offset: 0x0003E72C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HEPBridgeTile";
		int width = 2;
		int height = 1;
		string anim = "radbolt_joint_plate_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Tile;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER5, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.InitialOrientation = Orientation.R180;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.UseHighEnergyParticleInputPort = true;
		buildingDef.HighEnergyParticleInputOffset = new CellOffset(1, 0);
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HEPBridgeTile");
		return buildingDef;
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x00040608 File Offset: 0x0003E808
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		go.AddOrGet<TileTemperature>();
		HighEnergyParticleStorage highEnergyParticleStorage = go.AddOrGet<HighEnergyParticleStorage>();
		highEnergyParticleStorage.autoStore = true;
		highEnergyParticleStorage.showInUI = false;
		highEnergyParticleStorage.capacity = 501f;
		HighEnergyParticleRedirector highEnergyParticleRedirector = go.AddOrGet<HighEnergyParticleRedirector>();
		highEnergyParticleRedirector.directorDelay = 0.5f;
		highEnergyParticleRedirector.directionControllable = false;
		highEnergyParticleRedirector.Direction = EightDirection.Right;
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0004067F File Offset: 0x0003E87F
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		go.AddOrGet<HEPBridgeTileVisualizer>();
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x00040697 File Offset: 0x0003E897
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x000406A8 File Offset: 0x0003E8A8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.HEPPassThrough, false);
		go.AddOrGet<BuildingCellVisualizer>();
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject inst)
		{
			Rotatable component = inst.GetComponent<Rotatable>();
			HighEnergyParticleRedirector component2 = inst.GetComponent<HighEnergyParticleRedirector>();
			switch (component.Orientation)
			{
			case Orientation.Neutral:
				component2.Direction = EightDirection.Left;
				return;
			case Orientation.R90:
				component2.Direction = EightDirection.Up;
				return;
			case Orientation.R180:
				component2.Direction = EightDirection.Right;
				return;
			case Orientation.R270:
				component2.Direction = EightDirection.Down;
				return;
			default:
				return;
			}
		};
	}

	// Token: 0x0400071D RID: 1821
	public const string ID = "HEPBridgeTile";
}
