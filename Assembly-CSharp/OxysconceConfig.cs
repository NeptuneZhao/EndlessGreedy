using System;
using TUNING;
using UnityEngine;

// Token: 0x02000325 RID: 805
public class OxysconceConfig : IBuildingConfig
{
	// Token: 0x060010D4 RID: 4308 RVA: 0x0005EE61 File Offset: 0x0005D061
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x0005EE68 File Offset: 0x0005D068
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Oxysconce";
		int width = 1;
		int height = 1;
		string anim = "oxy_sconce_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER0, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.Breakable = true;
		return buildingDef;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x0005EEE8 File Offset: 0x0005D0E8
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		new CellOffset(0, 0);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 240f;
		storage.showInUI = true;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.OxyRock.CreateTag();
		manualDeliveryKG.capacity = 240f;
		manualDeliveryKG.refillMass = 96f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		go.AddOrGet<StorageMeter>();
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x0005EF7D File Offset: 0x0005D17D
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			Tutorial.Instance.oxygenGenerators.Add(game_object);
		};
	}

	// Token: 0x04000A52 RID: 2642
	public const string ID = "Oxysconce";

	// Token: 0x04000A53 RID: 2643
	private const float OXYLITE_STORAGE = 240f;
}
