using HarmonyLib;
using UnityEngine;

namespace SuperMineralDeoxidizer
{
	[HarmonyPatch(typeof(MineralDeoxidizerConfig), "ConfigureBuildingTemplate")]
	internal class Patches
	{
		[HarmonyPostfix]
		public static void Postfix(GameObject go)
		{
			CellOffset cellOffset = new CellOffset(0, 1);
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
			{
				new ElementConverter.ConsumedElement(new Tag("Algae"), 0.5f, true)
			};
			elementConverter.outputElements = new ElementConverter.OutputElement[]
			{
				new ElementConverter.OutputElement(1f, SimHashes.Oxygen, 300.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true)
			};
		}
	}
}