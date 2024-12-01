using HarmonyLib;
using TUNING;
using UnityEngine;

[HarmonyPatch(typeof(IceCooledFanConfig))]
internal class SuperIceCooledFan
{
	[HarmonyPrefix, HarmonyPatch(nameof(IceCooledFanConfig.CreateBuildingDef))]
	public static bool Prefix(ref BuildingDef __result)
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef(
			"IceCooledFan", 2, 2, "fanice_kanim", 30, 30f, new float[] { 400f },
			MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER2, decor: BUILDINGS.DECOR.NONE);
		obj.SelfHeatKilowattsWhenActive = -1000f;
		obj.ExhaustKilowattsWhenActive = -1000f;
		obj.Overheatable = false;
		obj.ViewMode = OverlayModes.Temperature.ID;
		obj.AudioCategory = "Metal";
		__result = obj;
		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(IceCooledFanConfig.ConfigureBuildingTemplate))]
	public static bool Prefix(GameObject go)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = 50f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 50f;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 273.15f;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		IceCooledFan iceCooledFan = go.AddOrGet<IceCooledFan>();
		iceCooledFan.coolingRate = 200f;
		iceCooledFan.targetTemperature = 273.15f;
		iceCooledFan.iceStorage = storage;
		iceCooledFan.liquidStorage = storage2;
		iceCooledFan.minCooledTemperature = 273.15f;
		iceCooledFan.minEnvironmentMass = 0.25f;
		iceCooledFan.minCoolingRange = new Vector2I(-2, 0);
		iceCooledFan.maxCoolingRange = new Vector2I(2, 4);
		iceCooledFan.consumptionTag = GameTags.IceOre;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTags.IceOre;
		manualDeliveryKG.capacity = 50f;
		manualDeliveryKG.refillMass = 10f;
		manualDeliveryKG.MinimumMass = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		go.AddOrGet<IceCooledFanWorkable>().overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_icefan_kanim") };
		return false;
	}
}