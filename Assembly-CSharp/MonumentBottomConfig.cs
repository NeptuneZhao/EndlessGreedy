using System;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class MonumentBottomConfig : IBuildingConfig
{
	// Token: 0x06001004 RID: 4100 RVA: 0x0005ABA8 File Offset: 0x00058DA8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MonumentBottom";
		int width = 5;
		int height = 5;
		string anim = "monument_base_a_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			7500f,
			2500f
		};
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString(),
			SimHashes.Obsidian.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tier, 0.2f);
		BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = "MonumentBottom";
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		return buildingDef;
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x0005AC88 File Offset: 0x00058E88
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), "MonumentMiddle", null)
		};
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Bottom;
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x0005ACEC File Offset: 0x00058EEC
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0005ACEE File Offset: 0x00058EEE
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0005ACF0 File Offset: 0x00058EF0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Bottom;
			monumentPart.stateUISymbol = "base";
		};
	}

	// Token: 0x040009B1 RID: 2481
	public const string ID = "MonumentBottom";
}
