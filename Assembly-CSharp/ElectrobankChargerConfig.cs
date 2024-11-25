using System;
using TUNING;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class ElectrobankChargerConfig : IBuildingConfig
{
	// Token: 0x06000239 RID: 569 RVA: 0x0000F571 File Offset: 0x0000D771
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000F578 File Offset: 0x0000D778
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ElectrobankCharger";
		int width = 2;
		int height = 2;
		string anim = "electrobank_charger_small_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "small";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return buildingDef;
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000F628 File Offset: 0x0000D828
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1f;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTags.EmptyPortableBattery;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.refillMass = 20f;
		manualDeliveryKG.MassPerUnit = 20f;
		manualDeliveryKG.MinimumMass = 20f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.PowerFetch.IdHash;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000F6C3 File Offset: 0x0000D8C3
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<ElectrobankCharger.Def>();
	}

	// Token: 0x04000166 RID: 358
	public const string ID = "ElectrobankCharger";

	// Token: 0x04000167 RID: 359
	public const float CHARGE_RATE = 480f;
}
