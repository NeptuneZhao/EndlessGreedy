using System;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x020002FD RID: 765
public class MonumentTopConfig : IBuildingConfig
{
	// Token: 0x06001010 RID: 4112 RVA: 0x0005AED4 File Offset: 0x000590D4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MonumentTop";
		int width = 5;
		int height = 5;
		string anim = "monument_upper_a_kanim";
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
			SimHashes.Glass.ToString(),
			SimHashes.Diamond.ToString(),
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tier, 0.2f);
		BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.AttachmentSlotTag = "MonumentTop";
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		return buildingDef;
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0005AFC6 File Offset: 0x000591C6
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Top;
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0005AFF0 File Offset: 0x000591F0
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0005AFF2 File Offset: 0x000591F2
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0005AFF4 File Offset: 0x000591F4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Top;
			monumentPart.stateUISymbol = "upper";
		};
	}

	// Token: 0x040009B3 RID: 2483
	public const string ID = "MonumentTop";
}
