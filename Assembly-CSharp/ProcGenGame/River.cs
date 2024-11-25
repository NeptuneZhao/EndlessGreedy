﻿using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

namespace ProcGenGame
{
	// Token: 0x02000E09 RID: 3593
	public class River : River, SymbolicMapElement
	{
		// Token: 0x06007216 RID: 29206 RVA: 0x002B621F File Offset: 0x002B441F
		public River(River other) : base(other, true)
		{
		}

		// Token: 0x06007217 RID: 29207 RVA: 0x002B622C File Offset: 0x002B442C
		public void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd)
		{
			Element element = ElementLoader.FindElementByName(base.backgroundElement);
			Sim.PhysicsData defaultValues = element.defaultValues;
			Element element2 = ElementLoader.FindElementByName(base.element);
			Sim.PhysicsData defaultValues2 = element2.defaultValues;
			defaultValues2.temperature = base.temperature;
			Sim.DiseaseCell invalid = Sim.DiseaseCell.Invalid;
			for (int i = 0; i < this.pathElements.Count; i++)
			{
				Segment segment = this.pathElements[i];
				Vector2 vector = segment.e1 - segment.e0;
				Vector2 normalized = new Vector2(-vector.y, vector.x).normalized;
				List<Vector2I> line = ProcGen.Util.GetLine(segment.e0, segment.e1);
				for (int j = 0; j < line.Count; j++)
				{
					for (float num = 0.5f; num <= base.widthCenter; num += 1f)
					{
						Vector2 vector2 = line[j] + normalized * num;
						int num2 = Grid.XYToCell((int)vector2.x, (int)vector2.y);
						if (Grid.IsValidCell(num2))
						{
							SetValues(num2, element2, defaultValues2, invalid);
						}
						Vector2 vector3 = line[j] - normalized * num;
						num2 = Grid.XYToCell((int)vector3.x, (int)vector3.y);
						if (Grid.IsValidCell(num2))
						{
							SetValues(num2, element2, defaultValues2, invalid);
						}
					}
					for (float num3 = 0.5f; num3 <= base.widthBorder; num3 += 1f)
					{
						Vector2 vector4 = line[j] + normalized * (base.widthCenter + num3);
						int num4 = Grid.XYToCell((int)vector4.x, (int)vector4.y);
						if (Grid.IsValidCell(num4))
						{
							defaultValues.temperature = temperatureMin + world.heatOffset[num4] * temperatureRange;
							SetValues(num4, element, defaultValues, invalid);
						}
						Vector2 vector5 = line[j] - normalized * (base.widthCenter + num3);
						num4 = Grid.XYToCell((int)vector5.x, (int)vector5.y);
						if (Grid.IsValidCell(num4))
						{
							defaultValues.temperature = temperatureMin + world.heatOffset[num4] * temperatureRange;
							SetValues(num4, element, defaultValues, invalid);
						}
					}
				}
			}
		}

		// Token: 0x06007218 RID: 29208 RVA: 0x002B64B0 File Offset: 0x002B46B0
		public static void ProcessRivers(Chunk world, List<River> rivers, Sim.Cell[] cells, Sim.DiseaseCell[] dcs)
		{
			TerrainCell.SetValuesFunction setValues = delegate(int index, object elem, Sim.PhysicsData pd, Sim.DiseaseCell dc)
			{
				if (Grid.IsValidCell(index))
				{
					cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
					dcs[index] = dc;
					return;
				}
				global::Debug.LogError(string.Concat(new string[]
				{
					"Process::SetValuesFunction Index [",
					index.ToString(),
					"] is not valid. cells.Length [",
					cells.Length.ToString(),
					"]"
				}));
			};
			float temperatureMin = 265f;
			float temperatureRange = 30f;
			for (int i = 0; i < rivers.Count; i++)
			{
				rivers[i].ConvertToMap(world, setValues, temperatureMin, temperatureRange, null);
			}
		}

		// Token: 0x06007219 RID: 29209 RVA: 0x002B650C File Offset: 0x002B470C
		public static River GetRiverForCell(List<River> rivers, int cell)
		{
			return new River(rivers.Find((River river) => Grid.PosToCell(river.SourcePosition()) == cell || Grid.PosToCell(river.SinkPosition()) == cell));
		}

		// Token: 0x0600721A RID: 29210 RVA: 0x002B6540 File Offset: 0x002B4740
		private static void GetRiverLocation(List<River> rivers, ref GameSpawnData gsd)
		{
			for (int i = 0; i < rivers.Count; i++)
			{
				Vector2 vector = rivers[i].SourcePosition();
				Vector2 vector2 = rivers[i].SinkPosition();
				if (vector.y < vector2.y)
				{
				}
			}
		}
	}
}
