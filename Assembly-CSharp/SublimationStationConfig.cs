﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class SublimationStationConfig : IBuildingConfig
{
	// Token: 0x06001498 RID: 5272 RVA: 0x00070F21 File Offset: 0x0006F121
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x00070F28 File Offset: 0x0006F128
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SublimationStation";
		int width = 2;
		int height = 1;
		string anim = "sublimation_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.Breakable = true;
		return buildingDef;
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x00070FC8 File Offset: 0x0006F1C8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		CellOffset cellOffset = new CellOffset(0, 0);
		Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
		electrolyzer.maxMass = 1.8f;
		electrolyzer.hasMeter = false;
		electrolyzer.emissionOffset = cellOffset;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 600f;
		storage.showInUI = true;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(SimHashes.ToxicSand.CreateTag(), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.66f, SimHashes.ContaminatedOxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true)
		};
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.ToxicSand.CreateTag();
		manualDeliveryKG.capacity = 600f;
		manualDeliveryKG.refillMass = 240f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x000710D9 File Offset: 0x0006F2D9
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000BBF RID: 3007
	public const string ID = "SublimationStation";

	// Token: 0x04000BC0 RID: 3008
	private const float DIRT_CONSUME_RATE = 1f;

	// Token: 0x04000BC1 RID: 3009
	private const float DIRT_STORAGE = 600f;

	// Token: 0x04000BC2 RID: 3010
	private const float OXYGEN_GENERATION_RATE = 0.66f;

	// Token: 0x04000BC3 RID: 3011
	private const float OXYGEN_TEMPERATURE = 303.15f;
}
