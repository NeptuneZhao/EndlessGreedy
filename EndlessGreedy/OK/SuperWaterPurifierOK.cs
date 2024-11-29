using HarmonyLib;
using UnityEngine;

namespace SuperWaterPurifier
{
	[HarmonyPatch(typeof(WaterPurifierConfig), "DoPostConfigureComplete")]
	public class Patches
	{
		[HarmonyPostfix]
		public static void Postfix(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			Storage storage = BuildingTemplates.CreateDefaultStorage(go);
			storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
			go.AddOrGet<WaterPurifier>();
			Prioritizable.AddRef(go);
			go.AddOrGet<ElementConverter>().outputElements = new ElementConverter.OutputElement[2]
			{
				new ElementConverter.OutputElement(500f, SimHashes.Water, 0f, useEntityTemperature: false, storeOutput: true, 0f, 0.5f, 0.75f),
				new ElementConverter.OutputElement(0.2f, SimHashes.ToxicSand, 0f, useEntityTemperature: false, storeOutput: true, 0f, 0.5f, 0.25f)
			};
		}
	}
}