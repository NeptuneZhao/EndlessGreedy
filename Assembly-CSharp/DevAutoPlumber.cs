using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020006B6 RID: 1718
public class DevAutoPlumber
{
	// Token: 0x06002B40 RID: 11072 RVA: 0x000F2E63 File Offset: 0x000F1063
	public static void AutoPlumbBuilding(Building building)
	{
		DevAutoPlumber.DoElectricalPlumbing(building);
		DevAutoPlumber.DoLiquidAndGasPlumbing(building);
		DevAutoPlumber.SetupSolidOreDelivery(building);
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000F2E78 File Offset: 0x000F1078
	public static void DoElectricalPlumbing(Building building)
	{
		if (!building.Def.RequiresPowerInput)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(building), building.Def.PowerInputOffset);
		GameObject gameObject = Grid.Objects[cell, 26];
		if (gameObject != null)
		{
			gameObject.Trigger(-790448070, null);
		}
		DevAutoPlumber.PlaceSourceAndUtilityConduit(building, Assets.GetBuildingDef("DevGenerator"), Assets.GetBuildingDef("WireRefined"), Game.Instance.electricalConduitSystem, new int[]
		{
			26,
			29
		}, DevAutoPlumber.PortSelection.PowerInput);
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x000F2F03 File Offset: 0x000F1103
	public static void DoLiquidAndGasPlumbing(Building building)
	{
		DevAutoPlumber.SetupPlumbingInput(building);
		DevAutoPlumber.SetupPlumbingOutput(building);
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x000F2F14 File Offset: 0x000F1114
	public static void SetupSolidOreDelivery(Building building)
	{
		ManualDeliveryKG component = building.GetComponent<ManualDeliveryKG>();
		if (component != null)
		{
			DevAutoPlumber.TrySpawnElementOreFromTag(component.RequestedItemTag, Grid.PosToCell(building), component.Capacity) == null;
			return;
		}
		foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
		{
			using (List<Tag>.Enumerator enumerator2 = complexRecipe.fabricators.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current == building.Def.PrefabID)
					{
						foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
						{
							DevAutoPlumber.TrySpawnElementOreFromTag(recipeElement.material, Grid.PosToCell(building), recipeElement.amount * 10f) == null;
						}
					}
				}
			}
		}
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000F3034 File Offset: 0x000F1234
	private static GameObject TrySpawnElementOreFromTag(Tag t, int cell, float amount)
	{
		Element element = ElementLoader.GetElement(t);
		if (element == null)
		{
			element = ElementLoader.elements.Find((Element match) => match.HasTag(t));
		}
		if (element != null)
		{
			return element.substance.SpawnResource(Grid.CellToPos(cell), amount, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		}
		return null;
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000F30A0 File Offset: 0x000F12A0
	private static void SetupPlumbingInput(Building building)
	{
		ConduitConsumer component = building.GetComponent<ConduitConsumer>();
		if (component == null)
		{
			return;
		}
		BuildingDef sourceDef = null;
		BuildingDef conduitDef = null;
		int[] conduitTypeLayers = null;
		UtilityNetworkManager<FlowUtilityNetwork, Vent> utlityNetworkManager = null;
		ConduitType conduitType = component.ConduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				conduitDef = Assets.GetBuildingDef("InsulatedLiquidConduit");
				sourceDef = Assets.GetBuildingDef("DevPumpLiquid");
				utlityNetworkManager = Game.Instance.liquidConduitSystem;
				conduitTypeLayers = new int[]
				{
					16,
					19
				};
			}
		}
		else
		{
			conduitDef = Assets.GetBuildingDef("InsulatedGasConduit");
			sourceDef = Assets.GetBuildingDef("DevPumpGas");
			utlityNetworkManager = Game.Instance.gasConduitSystem;
			conduitTypeLayers = new int[]
			{
				12,
				15
			};
		}
		GameObject gameObject = DevAutoPlumber.PlaceSourceAndUtilityConduit(building, sourceDef, conduitDef, utlityNetworkManager, conduitTypeLayers, DevAutoPlumber.PortSelection.UtilityInput);
		Element element = DevAutoPlumber.GuessMostRelevantElementForPump(building);
		if (element != null)
		{
			gameObject.GetComponent<DevPump>().SelectedTag = element.tag;
			return;
		}
		gameObject.GetComponent<DevPump>().SelectedTag = ElementLoader.FindElementByHash(SimHashes.Vacuum).tag;
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x000F318C File Offset: 0x000F138C
	private static void SetupPlumbingOutput(Building building)
	{
		ConduitDispenser component = building.GetComponent<ConduitDispenser>();
		if (component == null)
		{
			return;
		}
		BuildingDef sourceDef = null;
		BuildingDef conduitDef = null;
		int[] conduitTypeLayers = null;
		UtilityNetworkManager<FlowUtilityNetwork, Vent> utlityNetworkManager = null;
		ConduitType conduitType = component.ConduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				conduitDef = Assets.GetBuildingDef("InsulatedLiquidConduit");
				sourceDef = Assets.GetBuildingDef("LiquidVent");
				utlityNetworkManager = Game.Instance.liquidConduitSystem;
				conduitTypeLayers = new int[]
				{
					16,
					19
				};
			}
		}
		else
		{
			conduitDef = Assets.GetBuildingDef("InsulatedGasConduit");
			sourceDef = Assets.GetBuildingDef("GasVent");
			utlityNetworkManager = Game.Instance.gasConduitSystem;
			conduitTypeLayers = new int[]
			{
				12,
				15
			};
		}
		DevAutoPlumber.PlaceSourceAndUtilityConduit(building, sourceDef, conduitDef, utlityNetworkManager, conduitTypeLayers, DevAutoPlumber.PortSelection.UtilityOutput);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x000F323C File Offset: 0x000F143C
	private static Element GuessMostRelevantElementForPump(Building destinationBuilding)
	{
		ConduitConsumer consumer = destinationBuilding.GetComponent<ConduitConsumer>();
		Tag targetTag = destinationBuilding.GetComponent<ConduitConsumer>().capacityTag;
		ElementConverter elementConverter = destinationBuilding.GetComponent<ElementConverter>();
		ElementConsumer elementConsumer = destinationBuilding.GetComponent<ElementConsumer>();
		RocketEngineCluster rocketEngineCluster = destinationBuilding.GetComponent<RocketEngineCluster>();
		return ElementLoader.elements.Find(delegate(Element match)
		{
			if (elementConverter != null)
			{
				bool flag = false;
				for (int i = 0; i < elementConverter.consumedElements.Length; i++)
				{
					if (elementConverter.consumedElements[i].Tag == match.tag)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			else if (elementConsumer != null)
			{
				bool flag2 = false;
				if (ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag == match.tag)
				{
					flag2 = true;
				}
				if (!flag2)
				{
					return false;
				}
			}
			else if (rocketEngineCluster != null)
			{
				bool flag3 = false;
				if (rocketEngineCluster.fuelTag == match.tag)
				{
					flag3 = true;
				}
				if (!flag3)
				{
					return false;
				}
			}
			return (consumer.ConduitType != ConduitType.Liquid || match.IsLiquid) && (consumer.ConduitType != ConduitType.Gas || match.IsGas) && (match.HasTag(targetTag) || !(targetTag != GameTags.Any));
		});
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x000F32A8 File Offset: 0x000F14A8
	private static GameObject PlaceSourceAndUtilityConduit(Building destinationBuilding, BuildingDef sourceDef, BuildingDef conduitDef, IUtilityNetworkMgr utlityNetworkManager, int[] conduitTypeLayers, DevAutoPlumber.PortSelection portSelection)
	{
		Building building = null;
		List<int> list = new List<int>();
		int cell = DevAutoPlumber.FindClearPlacementLocation(Grid.PosToCell(destinationBuilding), new List<int>(conduitTypeLayers)
		{
			1
		}.ToArray(), list);
		bool flag = false;
		int num = 10;
		while (!flag)
		{
			num--;
			building = DevAutoPlumber.PlaceConduitSourceBuilding(cell, sourceDef);
			if (building == null)
			{
				return null;
			}
			List<int> list2 = DevAutoPlumber.GenerateClearConduitPath(building, destinationBuilding, conduitTypeLayers, portSelection);
			if (list2 == null)
			{
				list.Add(Grid.PosToCell(building));
				building.Trigger(-790448070, null);
			}
			else
			{
				flag = true;
				DevAutoPlumber.BuildConduits(list2, conduitDef, utlityNetworkManager);
			}
		}
		return building.gameObject;
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x000F3344 File Offset: 0x000F1544
	private static int FindClearPlacementLocation(int nearStartingCell, int[] placementBlockingObjectLayers, List<int> rejectLocations)
	{
		Func<int, object, bool> fn = delegate(int test, object unusedData)
		{
			foreach (int num in new int[]
			{
				test,
				Grid.OffsetCell(test, 1, 0),
				Grid.OffsetCell(test, 1, -1),
				Grid.OffsetCell(test, 0, -1),
				Grid.OffsetCell(test, 0, 1),
				Grid.OffsetCell(test, 1, 1)
			})
			{
				if (!Grid.IsValidCell(num))
				{
					return false;
				}
				if (Grid.Solid[num])
				{
					return false;
				}
				if (Grid.ObjectLayers[1].ContainsKey(num))
				{
					return false;
				}
				foreach (int num2 in placementBlockingObjectLayers)
				{
					if (Grid.ObjectLayers[num2].ContainsKey(num))
					{
						return false;
					}
				}
				if (rejectLocations.Contains(test))
				{
					return false;
				}
			}
			return true;
		};
		int max_depth = 20;
		return GameUtil.FloodFillFind<object>(fn, null, nearStartingCell, max_depth, false, false);
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000F337C File Offset: 0x000F157C
	private static List<int> GenerateClearConduitPath(Building sourceBuilding, Building destinationBuilding, int[] conduitTypeLayers, DevAutoPlumber.PortSelection portSelection)
	{
		new List<int>();
		if (sourceBuilding == null)
		{
			return null;
		}
		int conduitStart = -1;
		int conduitEnd = -1;
		switch (portSelection)
		{
		case DevAutoPlumber.PortSelection.UtilityInput:
			conduitStart = Grid.OffsetCell(Grid.PosToCell(sourceBuilding), sourceBuilding.Def.UtilityOutputOffset);
			conduitEnd = Grid.OffsetCell(Grid.PosToCell(destinationBuilding), destinationBuilding.Def.UtilityInputOffset);
			break;
		case DevAutoPlumber.PortSelection.UtilityOutput:
			conduitStart = Grid.OffsetCell(Grid.PosToCell(destinationBuilding), destinationBuilding.Def.UtilityOutputOffset);
			conduitEnd = Grid.OffsetCell(Grid.PosToCell(sourceBuilding), sourceBuilding.Def.UtilityInputOffset);
			break;
		case DevAutoPlumber.PortSelection.PowerInput:
			conduitStart = Grid.OffsetCell(Grid.PosToCell(sourceBuilding), sourceBuilding.Def.PowerOutputOffset);
			conduitEnd = Grid.OffsetCell(Grid.PosToCell(destinationBuilding), destinationBuilding.Def.PowerInputOffset);
			break;
		}
		return DevAutoPlumber.GetGridPath(conduitStart, conduitEnd, delegate(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			foreach (int layer in conduitTypeLayers)
			{
				GameObject x = Grid.Objects[cell, layer];
				bool flag = x == sourceBuilding.gameObject || x == destinationBuilding.gameObject;
				bool flag2 = cell == conduitEnd || cell == conduitStart;
				if (x != null && (!flag || (flag && !flag2)))
				{
					return false;
				}
			}
			return true;
		}, 20);
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x000F34EC File Offset: 0x000F16EC
	private static Building PlaceConduitSourceBuilding(int cell, BuildingDef def)
	{
		List<Tag> selected_elements = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		return def.Build(cell, Orientation.Neutral, null, selected_elements, 273.15f, true, GameClock.Instance.GetTime()).GetComponent<Building>();
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x000F3530 File Offset: 0x000F1730
	private static void BuildConduits(List<int> path, BuildingDef conduitDef, object utilityNetwork)
	{
		List<Tag> selected_elements = new List<Tag>
		{
			SimHashes.Cuprite.CreateTag()
		};
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < path.Count; i++)
		{
			list.Add(conduitDef.Build(path[i], Orientation.Neutral, null, selected_elements, 273.15f, true, GameClock.Instance.GetTime()));
		}
		if (list.Count < 2)
		{
			return;
		}
		IUtilityNetworkMgr utilityNetworkMgr = (IUtilityNetworkMgr)utilityNetwork;
		for (int j = 1; j < list.Count; j++)
		{
			UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(Grid.PosToCell(list[j - 1]), Grid.PosToCell(list[j]));
			utilityNetworkMgr.AddConnection(utilityConnections, Grid.PosToCell(list[j - 1]), true);
			utilityNetworkMgr.AddConnection(utilityConnections.InverseDirection(), Grid.PosToCell(list[j]), true);
			IUtilityItem component = list[j].GetComponent<KAnimGraphTileVisualizer>();
			if (component != null)
			{
				component.UpdateConnections(utilityNetworkMgr.GetConnections(Grid.PosToCell(list[j]), true));
			}
		}
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x000F3640 File Offset: 0x000F1840
	private static List<int> GetGridPath(int startCell, int endCell, Func<int, bool> testFunction, int maxDepth = 20)
	{
		DevAutoPlumber.<>c__DisplayClass14_0 CS$<>8__locals1;
		CS$<>8__locals1.testFunction = testFunction;
		CS$<>8__locals1.endCell = endCell;
		List<int> list = new List<int>();
		CS$<>8__locals1.frontier = new List<int>();
		CS$<>8__locals1.touched = new List<int>();
		CS$<>8__locals1.crumbs = new Dictionary<int, int>();
		CS$<>8__locals1.frontier.Add(startCell);
		CS$<>8__locals1.newFrontier = new List<int>();
		int num = 0;
		while (!CS$<>8__locals1.touched.Contains(CS$<>8__locals1.endCell))
		{
			num++;
			if (num > maxDepth || CS$<>8__locals1.frontier.Count == 0)
			{
				break;
			}
			foreach (int fromCell in CS$<>8__locals1.frontier)
			{
				DevAutoPlumber.<GetGridPath>g___ExpandFrontier|14_0(fromCell, ref CS$<>8__locals1);
			}
			CS$<>8__locals1.frontier.Clear();
			foreach (int item in CS$<>8__locals1.newFrontier)
			{
				CS$<>8__locals1.frontier.Add(item);
			}
			CS$<>8__locals1.newFrontier.Clear();
		}
		int num2 = CS$<>8__locals1.endCell;
		list.Add(num2);
		while (CS$<>8__locals1.crumbs.ContainsKey(num2))
		{
			num2 = CS$<>8__locals1.crumbs[num2];
			list.Add(num2);
		}
		list.Reverse();
		return list;
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x000F37C0 File Offset: 0x000F19C0
	[CompilerGenerated]
	internal static void <GetGridPath>g___ExpandFrontier|14_0(int fromCell, ref DevAutoPlumber.<>c__DisplayClass14_0 A_1)
	{
		foreach (int num in new int[]
		{
			Grid.CellAbove(fromCell),
			Grid.CellBelow(fromCell),
			Grid.CellLeft(fromCell),
			Grid.CellRight(fromCell)
		})
		{
			if (!A_1.newFrontier.Contains(num) && !A_1.frontier.Contains(num) && !A_1.touched.Contains(num) && A_1.testFunction(num))
			{
				A_1.newFrontier.Add(num);
				A_1.crumbs.Add(num, fromCell);
			}
			A_1.touched.Add(num);
			if (num == A_1.endCell)
			{
				break;
			}
		}
		A_1.touched.Add(fromCell);
	}

	// Token: 0x020014AD RID: 5293
	private enum PortSelection
	{
		// Token: 0x04006A9C RID: 27292
		UtilityInput,
		// Token: 0x04006A9D RID: 27293
		UtilityOutput,
		// Token: 0x04006A9E RID: 27294
		PowerInput
	}
}
