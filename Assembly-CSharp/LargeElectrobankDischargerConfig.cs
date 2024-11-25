﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200023A RID: 570
public class LargeElectrobankDischargerConfig : IBuildingConfig
{
	// Token: 0x06000BCB RID: 3019 RVA: 0x000451FC File Offset: 0x000433FC
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00045204 File Offset: 0x00043404
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LargeElectrobankDischarger";
		int width = 2;
		int height = 2;
		string anim = "electrobank_discharger_large_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 480f;
		buildingDef.GeneratorBaseCapacity = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x000452C0 File Offset: 0x000434C0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.capacityKg = 1f;
		storage.storageFilters = STORAGEFILTERS.POWER_BANKS;
		go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;
		go.AddOrGet<ElectrobankDischarger>().wattageRating = 480f;
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x00045332 File Offset: 0x00043532
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x040007B9 RID: 1977
	public const string ID = "LargeElectrobankDischarger";

	// Token: 0x040007BA RID: 1978
	public const float DISCHARGE_RATE = 480f;
}
