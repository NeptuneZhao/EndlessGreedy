using HarmonyLib;
using UnityEngine;

/// <summary>
/// 冲水马桶
/// </summary>
[HarmonyPatch(typeof(FlushToiletConfig))]
internal class SuperFlushToilet
{
	[HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static void Postfix(GameObject go)
	{
		FlushToilet flushToilet = go.AddOrGet<FlushToilet>();
		flushToilet.massEmittedPerUse = 10f;
		flushToilet.diseasePerFlush = 0;
		flushToilet.diseaseOnDupePerFlush = 0;
	}
}

/// <summary>
/// 淋浴间
/// </summary>
[HarmonyPatch(typeof(ShowerConfig))]
internal class SuperShower
{
	[HarmonyPrefix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static bool Prefix(GameObject go)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.WashStation, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.AdvancedWashStation, false);
		Shower shower = go.AddOrGet<Shower>();
		shower.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_shower_kanim")
		};

		shower.workTime = 5f;
		shower.outputTargetElement = SimHashes.DirtyWater;
		shower.fractionalDiseaseRemoval = 0.95f;
		shower.absoluteDiseaseRemoval = -2000;
		shower.workLayer = Grid.SceneLayer.BuildingFront;
		shower.trackUses = true;

		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Water).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
		conduitConsumer.capacityKG = 5f;

		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.invertElementFilter = true;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.Water
		};

		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Water"), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(50f, SimHashes.DirtyWater, 0f, false, true, 0f, 0.5f, 1f, byte.MaxValue, 0, true)
		};

		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<RequireOutputs>().ignoreFullPipe = true;
		go.AddOrGetDef<RocketUsageRestriction.Def>();
		return false;
	}
}
