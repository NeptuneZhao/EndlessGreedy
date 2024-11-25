using System;
using TUNING;
using UnityEngine;

// Token: 0x0200003B RID: 59
public class CeilingLightConfig : IBuildingConfig
{
	// Token: 0x06000119 RID: 281 RVA: 0x00008644 File Offset: 0x00006844
	public override BuildingDef CreateBuildingDef()
	{
		string id = "CeilingLight";
		int width = 1;
		int height = 1;
		string anim = "ceilinglight_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 10f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000086BD File Offset: 0x000068BD
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 1800;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = global::LightShape.Cone;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000086E1 File Offset: 0x000068E1
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000086F4 File Offset: 0x000068F4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.CEILINGLIGHT_COLOR;
		light2D.Range = 8f;
		light2D.Angle = 2.6f;
		light2D.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
		light2D.Offset = LIGHT2D.CEILINGLIGHT_OFFSET;
		light2D.shape = global::LightShape.Cone;
		light2D.drawOverlay = true;
		light2D.Lux = 1800;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x040000AE RID: 174
	public const string ID = "CeilingLight";
}
