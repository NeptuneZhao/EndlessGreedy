using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class TravelTubeEntranceConfig : IBuildingConfig
{
	// Token: 0x060014FD RID: 5373 RVA: 0x00073AAC File Offset: 0x00071CAC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TravelTubeEntrance";
		int width = 3;
		int height = 2;
		string anim = "tube_launcher_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		return buildingDef;
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x00073B3C File Offset: 0x00071D3C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		TravelTubeEntrance travelTubeEntrance = go.AddOrGet<TravelTubeEntrance>();
		travelTubeEntrance.waxPerLaunch = 0.05f;
		travelTubeEntrance.joulesPerLaunch = 10000f;
		travelTubeEntrance.jouleCapacity = 40000f;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		List<Storage.StoredItemModifier> defaultStoredItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Preserve
		};
		storage.SetDefaultStoredItemModifiers(defaultStoredItemModifiers);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.requestedItemTag = SimHashes.MilkFat.CreateTag();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.refillMass = 0.05f;
		manualDeliveryKG.SetStorage(storage);
		go.AddOrGet<TravelTubeEntrance.Work>();
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<EnergyConsumerSelfSustaining>();
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x00073C0C File Offset: 0x00071E0C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
	}

	// Token: 0x04000BF1 RID: 3057
	public const string ID = "TravelTubeEntrance";

	// Token: 0x04000BF2 RID: 3058
	public const float WAX_PER_LAUNCH = 0.05f;

	// Token: 0x04000BF3 RID: 3059
	public const int STORAGE_WAX_LAUNCHECOUNT_CAPACITY = 200;

	// Token: 0x04000BF4 RID: 3060
	private const float JOULES_PER_LAUNCH = 10000f;

	// Token: 0x04000BF5 RID: 3061
	private const float LAUNCHES_FROM_FULL_CHARGE = 4f;
}
