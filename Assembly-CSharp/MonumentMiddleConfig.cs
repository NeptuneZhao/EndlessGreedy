using System;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x020002FC RID: 764
public class MonumentMiddleConfig : IBuildingConfig
{
	// Token: 0x0600100A RID: 4106 RVA: 0x0005AD34 File Offset: 0x00058F34
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MonumentMiddle";
		int width = 5;
		int height = 5;
		string anim = "monument_mid_a_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			2500f,
			2500f,
			5000f
		};
		string[] construction_materials = new string[]
		{
			SimHashes.Ceramic.ToString(),
			SimHashes.Polypropylene.ToString(),
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tier, 0.2f);
		BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = "MonumentMiddle";
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		return buildingDef;
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0005AE28 File Offset: 0x00059028
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), "MonumentTop", null)
		};
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Middle;
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0005AE8C File Offset: 0x0005908C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0005AE8E File Offset: 0x0005908E
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0005AE90 File Offset: 0x00059090
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Middle;
			monumentPart.stateUISymbol = "mid";
		};
	}

	// Token: 0x040009B2 RID: 2482
	public const string ID = "MonumentMiddle";
}
