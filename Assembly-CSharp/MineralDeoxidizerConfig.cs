﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020002A0 RID: 672
public class MineralDeoxidizerConfig : IBuildingConfig
{
	// Token: 0x06000DE4 RID: 3556 RVA: 0x0004FFA8 File Offset: 0x0004E1A8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MineralDeoxidizer";
		int width = 1;
		int height = 2;
		string anim = "mineraldeoxidizer_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 1));
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.Breakable = true;
		return buildingDef;
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x00050048 File Offset: 0x0004E248
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		CellOffset cellOffset = new CellOffset(0, 1);
		Prioritizable.AddRef(go);
		Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
		electrolyzer.maxMass = 1.8f;
		electrolyzer.hasMeter = false;
		electrolyzer.emissionOffset = cellOffset;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 330f;
		storage.showInUI = true;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Algae"), 0.55f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.5f, SimHashes.Oxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true)
		};
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = new Tag("Algae");
		manualDeliveryKG.capacity = 330f;
		manualDeliveryKG.refillMass = 132f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x00050159 File Offset: 0x0004E359
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x040008B6 RID: 2230
	public const string ID = "MineralDeoxidizer";

	// Token: 0x040008B7 RID: 2231
	private const float ALGAE_BURN_RATE = 0.55f;

	// Token: 0x040008B8 RID: 2232
	private const float ALGAE_STORAGE = 330f;

	// Token: 0x040008B9 RID: 2233
	private const float OXYGEN_GENERATION_RATE = 0.5f;

	// Token: 0x040008BA RID: 2234
	private const float OXYGEN_TEMPERATURE = 303.15f;
}
