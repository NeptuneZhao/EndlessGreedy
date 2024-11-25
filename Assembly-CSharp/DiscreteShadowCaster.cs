﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A30 RID: 2608
public static class DiscreteShadowCaster
{
	// Token: 0x06004B89 RID: 19337 RVA: 0x001AE500 File Offset: 0x001AC700
	public static DiscreteShadowCaster.Direction OctantToDirection(DiscreteShadowCaster.Octant octant)
	{
		switch (octant)
		{
		case DiscreteShadowCaster.Octant.N_NW:
		case DiscreteShadowCaster.Octant.N_NE:
			return DiscreteShadowCaster.Direction.North;
		case DiscreteShadowCaster.Octant.E_NE:
		case DiscreteShadowCaster.Octant.E_SE:
			return DiscreteShadowCaster.Direction.East;
		case DiscreteShadowCaster.Octant.S_SE:
		case DiscreteShadowCaster.Octant.S_SW:
			return DiscreteShadowCaster.Direction.South;
		case DiscreteShadowCaster.Octant.W_SW:
		case DiscreteShadowCaster.Octant.W_NW:
			return DiscreteShadowCaster.Direction.West;
		default:
			return DiscreteShadowCaster.Direction.South;
		}
	}

	// Token: 0x06004B8A RID: 19338 RVA: 0x001AE534 File Offset: 0x001AC734
	public static Vector2I DirectionToVector(DiscreteShadowCaster.Direction dir)
	{
		switch (dir)
		{
		case DiscreteShadowCaster.Direction.North:
			return new Vector2I(0, 1);
		case DiscreteShadowCaster.Direction.East:
			return new Vector2I(1, 0);
		case DiscreteShadowCaster.Direction.South:
			return new Vector2I(0, -1);
		case DiscreteShadowCaster.Direction.West:
			return new Vector2I(-1, 0);
		default:
			return default(Vector2I);
		}
	}

	// Token: 0x06004B8B RID: 19339 RVA: 0x001AE584 File Offset: 0x001AC784
	public static Vector2I TravelDirectionToOrtogonalDiractionVector(DiscreteShadowCaster.Direction dir)
	{
		switch (dir)
		{
		case DiscreteShadowCaster.Direction.North:
		case DiscreteShadowCaster.Direction.South:
			return new Vector2I(1, 0);
		case DiscreteShadowCaster.Direction.East:
		case DiscreteShadowCaster.Direction.West:
			return new Vector2I(0, 1);
		default:
			return default(Vector2I);
		}
	}

	// Token: 0x06004B8C RID: 19340 RVA: 0x001AE5C2 File Offset: 0x001AC7C2
	public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, global::LightShape shape, bool canSeeThroughTransparent = true)
	{
		DiscreteShadowCaster.GetVisibleCells(cell, visiblePoints, range, 0, DiscreteShadowCaster.Direction.South, shape, canSeeThroughTransparent);
	}

	// Token: 0x06004B8D RID: 19341 RVA: 0x001AE5D4 File Offset: 0x001AC7D4
	public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, int width, DiscreteShadowCaster.Direction direction, global::LightShape shape, bool canSeeThroughTransparent = true)
	{
		visiblePoints.Add(cell);
		Vector2I cellPos = Grid.CellToXY(cell);
		if (shape == global::LightShape.Circle)
		{
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.N_NW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.N_NE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.E_NE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.E_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.W_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.W_NW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			return;
		}
		if (shape == global::LightShape.Cone)
		{
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			return;
		}
		if (shape == global::LightShape.Quad)
		{
			DiscreteShadowCaster.ScanQuad(cellPos, direction, width, range, visiblePoints, canSeeThroughTransparent);
		}
	}

	// Token: 0x06004B8E RID: 19342 RVA: 0x001AE73C File Offset: 0x001AC93C
	public static void ScanQuad(Vector2I cellPos, DiscreteShadowCaster.Direction direction, int width, int range, List<int> visiblePoints, bool canSeeThroughTransparent)
	{
		if (width <= 0 || range <= 0)
		{
			return;
		}
		Vector2I[] array = new Vector2I[width];
		int s = (width % 2 == 0) ? (width / 2 - 1) : Mathf.FloorToInt((float)(width - 1) * 0.5f);
		Vector2I v = DiscreteShadowCaster.DirectionToVector(direction);
		Vector2I v2 = DiscreteShadowCaster.TravelDirectionToOrtogonalDiractionVector(direction);
		Vector2I u = cellPos - v2 * s;
		Vector2I vector2I = new Vector2I(-1, -1);
		for (int i = 0; i < width; i++)
		{
			Vector2I vector2I2 = u + v2 * i;
			bool flag = DiscreteShadowCaster.DoesOcclude(vector2I2.x, vector2I2.y, canSeeThroughTransparent);
			array[i] = (flag ? vector2I : vector2I2);
		}
		foreach (Vector2I u2 in array)
		{
			if (!(u2 == vector2I))
			{
				bool flag2 = false;
				int num = 0;
				while (!flag2 && num < range)
				{
					Vector2I vector2I3 = u2 + v * num;
					flag2 = (flag2 || DiscreteShadowCaster.DoesOcclude(vector2I3.x, vector2I3.y, canSeeThroughTransparent));
					if (!flag2)
					{
						int item = Grid.XYToCell(vector2I3.x, vector2I3.y);
						if (!visiblePoints.Contains(item))
						{
							visiblePoints.Add(item);
						}
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06004B8F RID: 19343 RVA: 0x001AE88C File Offset: 0x001ACA8C
	private static bool DoesOcclude(int x, int y, bool canSeeThroughTransparent = false)
	{
		int num = Grid.XYToCell(x, y);
		return Grid.IsValidCell(num) && (!canSeeThroughTransparent || !Grid.Transparent[num]) && Grid.Solid[num];
	}

	// Token: 0x06004B90 RID: 19344 RVA: 0x001AE8C8 File Offset: 0x001ACAC8
	private static void ScanOctant(Vector2I cellPos, int range, int depth, DiscreteShadowCaster.Octant octant, double startSlope, double endSlope, List<int> visiblePoints, bool canSeeThroughTransparent = true)
	{
		int num = range * range;
		int num2 = 0;
		int num3 = 0;
		switch (octant)
		{
		case DiscreteShadowCaster.Octant.N_NW:
			num3 = cellPos.y + depth;
			if (num3 >= Grid.HeightInCells)
			{
				return;
			}
			num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 < 0)
			{
				num2 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 - 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2++;
			}
			num2--;
			break;
		case DiscreteShadowCaster.Octant.N_NE:
			num3 = cellPos.y + depth;
			if (num3 >= Grid.HeightInCells)
			{
				return;
			}
			num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 >= Grid.WidthInCells)
			{
				num2 = Grid.WidthInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 + 1 < Grid.HeightInCells && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 - 1, canSeeThroughTransparent))
						{
							double slope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false);
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 + 1 < Grid.HeightInCells && DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2--;
			}
			num2++;
			break;
		case DiscreteShadowCaster.Octant.E_NE:
			num2 = cellPos.x + depth;
			if (num2 >= Grid.WidthInCells)
			{
				return;
			}
			num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 >= Grid.HeightInCells)
			{
				num3 = Grid.HeightInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 + 1 < Grid.HeightInCells && !DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 + 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 + 1 < Grid.HeightInCells && DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3--;
			}
			num3++;
			break;
		case DiscreteShadowCaster.Octant.E_SE:
			num2 = cellPos.x + depth;
			if (num2 >= Grid.WidthInCells)
			{
				return;
			}
			num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 < 0)
			{
				num3 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 - 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3++;
			}
			num3--;
			break;
		case DiscreteShadowCaster.Octant.S_SE:
			num3 = cellPos.y - depth;
			if (num3 < 0)
			{
				return;
			}
			num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 >= Grid.WidthInCells)
			{
				num2 = Grid.WidthInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 + 1 < Grid.WidthInCells && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 + 1, canSeeThroughTransparent))
						{
							double slope2 = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope2, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 + 1 < Grid.WidthInCells && DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2--;
			}
			num2++;
			break;
		case DiscreteShadowCaster.Octant.S_SW:
			num3 = cellPos.y - depth;
			if (num3 < 0)
			{
				return;
			}
			num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 < 0)
			{
				num2 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 + 1, canSeeThroughTransparent))
						{
							double slope3 = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope3, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2++;
			}
			num2--;
			break;
		case DiscreteShadowCaster.Octant.W_SW:
			num2 = cellPos.x - depth;
			if (num2 < 0)
			{
				return;
			}
			num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 < 0)
			{
				num3 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 - 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3++;
			}
			num3--;
			break;
		case DiscreteShadowCaster.Octant.W_NW:
			num2 = cellPos.x - depth;
			if (num2 < 0)
			{
				return;
			}
			num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 >= Grid.HeightInCells)
			{
				num3 = Grid.HeightInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 + 1 < Grid.HeightInCells && !DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 + 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 + 1 < Grid.HeightInCells && DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3--;
			}
			num3++;
			break;
		}
		if (num2 < 0)
		{
			num2 = 0;
		}
		else if (num2 >= Grid.WidthInCells)
		{
			num2 = Grid.WidthInCells - 1;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		else if (num3 >= Grid.HeightInCells)
		{
			num3 = Grid.HeightInCells - 1;
		}
		if (depth < range & !DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
		{
			DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, endSlope, visiblePoints, canSeeThroughTransparent);
		}
	}

	// Token: 0x06004B91 RID: 19345 RVA: 0x001AF47F File Offset: 0x001AD67F
	private static double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
	{
		if (pInvert)
		{
			return (pY1 - pY2) / (pX1 - pX2);
		}
		return (pX1 - pX2) / (pY1 - pY2);
	}

	// Token: 0x06004B92 RID: 19346 RVA: 0x001AF494 File Offset: 0x001AD694
	private static int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
	{
		return (pX1 - pX2) * (pX1 - pX2) + (pY1 - pY2) * (pY1 - pY2);
	}

	// Token: 0x02001A43 RID: 6723
	public enum Octant
	{
		// Token: 0x04007BD3 RID: 31699
		N_NW,
		// Token: 0x04007BD4 RID: 31700
		N_NE,
		// Token: 0x04007BD5 RID: 31701
		E_NE,
		// Token: 0x04007BD6 RID: 31702
		E_SE,
		// Token: 0x04007BD7 RID: 31703
		S_SE,
		// Token: 0x04007BD8 RID: 31704
		S_SW,
		// Token: 0x04007BD9 RID: 31705
		W_SW,
		// Token: 0x04007BDA RID: 31706
		W_NW
	}

	// Token: 0x02001A44 RID: 6724
	public enum Direction
	{
		// Token: 0x04007BDC RID: 31708
		North,
		// Token: 0x04007BDD RID: 31709
		East,
		// Token: 0x04007BDE RID: 31710
		South,
		// Token: 0x04007BDF RID: 31711
		West
	}
}
