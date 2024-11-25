using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030C RID: 780
public class ObjectDispenserConfig : IBuildingConfig
{
	// Token: 0x06001063 RID: 4195 RVA: 0x0005CAA0 File Offset: 0x0005ACA0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ObjectDispenser";
		int width = 1;
		int height = 2;
		string anim = "object_dispenser_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(ObjectDispenser.PORT_ID, new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.OBJECTDISPENSER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.OBJECTDISPENSER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.OBJECTDISPENSER.LOGIC_PORT_INACTIVE, false, false)
		};
		SoundEventVolumeCache.instance.AddVolume("ventliquid_kanim", "LiquidVent_squirt", NOISE_POLLUTION.NOISY.TIER0);
		return buildingDef;
	}

	// Token: 0x06001064 RID: 4196 RVA: 0x0005CB98 File Offset: 0x0005AD98
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<ObjectDispenser>().dropOffset = new CellOffset(1, 0);
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.showInUI = true;
		storage.allowItemRemoval = false;
		storage.showDescriptor = true;
		storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
		storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
		UnityEngine.Object.DestroyImmediate(go.GetComponent<LogicOperationalController>());
	}

	// Token: 0x04000A15 RID: 2581
	public const string ID = "ObjectDispenser";
}
