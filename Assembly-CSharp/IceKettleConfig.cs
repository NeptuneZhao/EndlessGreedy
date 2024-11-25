﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200021A RID: 538
public class IceKettleConfig : IBuildingConfig
{
	// Token: 0x06000B1B RID: 2843 RVA: 0x00042711 File Offset: 0x00040911
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00042718 File Offset: 0x00040918
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceKettle";
		int width = 2;
		int height = 2;
		string anim = "icemelter_kettle_kanim";
		int hitpoints = 100;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		float num = 3.7500002f;
		buildingDef.SelfHeatKilowattsWhenActive = num * 0.4f;
		buildingDef.ExhaustKilowattsWhenActive = num - buildingDef.SelfHeatKilowattsWhenActive;
		buildingDef.Floodable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.DefaultAnimState = "on";
		buildingDef.POIUnlockable = true;
		buildingDef.ShowInBuildMenu = true;
		return buildingDef;
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x000427D8 File Offset: 0x000409D8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddTag(GameTags.LiquidSource);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = Mathf.Ceil(152.80188f);
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.capacity = Mathf.Ceil(152.80188f);
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.requestedItemTag = IceKettleConfig.FUEL_TAG;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.ShowStatusItem = false;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 1000f;
		storage2.showInUI = true;
		storage2.allowItemRemoval = false;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		ManualDeliveryKG manualDeliveryKG2 = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG2.capacity = 1000f;
		manualDeliveryKG2.SetStorage(storage2);
		manualDeliveryKG2.requestedItemTag = IceKettleConfig.TARGET_ELEMENT_TAG;
		manualDeliveryKG2.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG2.refillMass = 100f;
		manualDeliveryKG2.ShowStatusItem = false;
		Storage storage3 = go.AddComponent<Storage>();
		storage3.capacityKg = 500f;
		storage3.showInUI = true;
		storage3.allowItemRemoval = true;
		storage3.showDescriptor = true;
		storage3.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		IceKettle.Def def = go.AddOrGetDef<IceKettle.Def>();
		def.exhaust_tag = SimHashes.CarbonDioxide;
		def.targetElementTag = IceKettleConfig.TARGET_ELEMENT_TAG;
		def.KGToMeltPerBatch = 100f;
		def.KGMeltedPerSecond = 20f;
		def.fuelElementTag = IceKettleConfig.FUEL_TAG;
		def.TargetTemperature = 298.15f;
		def.EnergyPerUnitOfLumber = 4000f;
		def.ExhaustMassPerUnitOfLumber = 0.142f;
		go.AddOrGet<IceKettleWorkable>().storage = storage3;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0004298F File Offset: 0x00040B8F
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000751 RID: 1873
	public const string ID = "IceKettle";

	// Token: 0x04000752 RID: 1874
	public const SimHashes TARGET_ELEMENT = SimHashes.Ice;

	// Token: 0x04000753 RID: 1875
	public const float MASS_KG_PER_BATCH = 100f;

	// Token: 0x04000754 RID: 1876
	public const float CAPACITY = 1000f;

	// Token: 0x04000755 RID: 1877
	public const float FINAL_PRODUCT_CAPACITY = 500f;

	// Token: 0x04000756 RID: 1878
	public static Tag TARGET_ELEMENT_TAG = SimHashes.Ice.CreateTag();

	// Token: 0x04000757 RID: 1879
	public const float TARGET_TEMPERATURE = 298.15f;

	// Token: 0x04000758 RID: 1880
	public const float PRODUCTION_PER_SECOND = 20f;

	// Token: 0x04000759 RID: 1881
	public static Tag FUEL_TAG = SimHashes.WoodLog.CreateTag();

	// Token: 0x0400075A RID: 1882
	public const SimHashes EXHAUST_TAG = SimHashes.CarbonDioxide;

	// Token: 0x0400075B RID: 1883
	public const float TOTAL_ENERGY_OF_LUMBER = 7750f;

	// Token: 0x0400075C RID: 1884
	public const float ENERGY_OF_LUMBER_TAKEN_FOR_BUILDING_SELF_HEAT = 3750f;

	// Token: 0x0400075D RID: 1885
	public const float ENERGY_PER_UNIT_OF_LUMBER_TAKEN_FOR_MELTING = 4000f;

	// Token: 0x0400075E RID: 1886
	public const float FUEL_UNITS_REQUIRED_TO_MELT_ABSOLUTE_ZERO_BATCH = 15.280188f;

	// Token: 0x0400075F RID: 1887
	public const float FUEL_CAPACITY = 152.80188f;
}
