using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class SmallElectrobankDischargerConfig : IBuildingConfig
{
	// Token: 0x0600139A RID: 5018 RVA: 0x0006C243 File Offset: 0x0006A443
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x0006C24C File Offset: 0x0006A44C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SmallElectrobankDischarger";
		int width = 1;
		int height = 1;
		string anim = "electrobank_discharger_small_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFoundationRotatable;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.GeneratorWattageRating = 60f;
		buildingDef.GeneratorBaseCapacity = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "small";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x0006C310 File Offset: 0x0006A510
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.capacityKg = 1f;
		storage.storageFilters = STORAGEFILTERS.POWER_BANKS;
		go.AddOrGet<TreeFilterable>().allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;
		go.AddOrGet<ElectrobankDischarger>().wattageRating = 60f;
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x0006C371 File Offset: 0x0006A571
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000B41 RID: 2881
	public const string ID = "SmallElectrobankDischarger";

	// Token: 0x04000B42 RID: 2882
	public const float DISCHARGE_RATE = 60f;
}
