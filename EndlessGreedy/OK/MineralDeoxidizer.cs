using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(MineralDeoxidizerConfig))]
internal class SuperMineralDeoxidizer
{
	[HarmonyPostfix]
	[HarmonyPatch("ConfigureBuildingTemplate")]
	public static void Postfix(GameObject go)
	{
		go.AddOrGet<Electrolyzer>().maxMass = 2.5f;
		CellOffset cellOffset = new CellOffset(0, 1);
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(new Tag("Algae"), 0.5f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(1f, SimHashes.Oxygen, 298.15f, false, false, cellOffset.x, cellOffset.y, 1f, byte.MaxValue, 0, true)
		};
	}
}