﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class MercuryCeilingLightConfig : IBuildingConfig
{
	// Token: 0x06000DA9 RID: 3497 RVA: 0x0004E147 File Offset: 0x0004C347
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x0004E150 File Offset: 0x0004C350
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MercuryCeilingLight";
		int width = 3;
		int height = 1;
		string anim = "mercurylight_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.AddLogicPowerPort = true;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = CellOffset.none;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.UtilityInputOffset = CellOffset.none;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x0004E1ED File Offset: 0x0004C3ED
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 60000;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = global::LightShape.Quad;
		lightShapePreview.width = 3;
		lightShapePreview.direction = DiscreteShadowCaster.Direction.South;
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x0004E220 File Offset: 0x0004C420
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Mercury).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 0.26000002f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		MercuryLight.Def def = go.AddOrGetDef<MercuryLight.Def>();
		def.FUEL_MASS_PER_SECOND = 0.13000001f;
		def.MAX_LUX = 60000f;
		def.TURN_ON_DELAY = 60f;
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x0004E2A8 File Offset: 0x0004C4A8
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.autoRespondToOperational = false;
		light2D.overlayColour = LIGHT2D.MERCURYCEILINGLIGHT_LUX_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.MERCURYCEILINGLIGHT_COLOR;
		light2D.Range = 8f;
		light2D.Angle = 2.6f;
		light2D.Direction = LIGHT2D.MERCURYCEILINGLIGHT_DIRECTIONVECTOR;
		light2D.Offset = LIGHT2D.MERCURYCEILINGLIGHT_OFFSET;
		light2D.shape = global::LightShape.Quad;
		light2D.drawOverlay = true;
		light2D.Lux = 60000;
		light2D.LightDirection = DiscreteShadowCaster.Direction.South;
		light2D.Width = 3;
		light2D.FalloffRate = 0.4f;
	}

	// Token: 0x0400088E RID: 2190
	public const string ID = "MercuryCeilingLight";

	// Token: 0x0400088F RID: 2191
	public const float MERCURY_CONSUMED_PER_SECOOND = 0.13000001f;

	// Token: 0x04000890 RID: 2192
	public const float CHARGING_DELAY = 60f;
}
