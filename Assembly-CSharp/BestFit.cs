using System;
using System.Collections.Generic;
using ProcGen;
using TUNING;

// Token: 0x02000E00 RID: 3584
public class BestFit
{
	// Token: 0x060071BA RID: 29114 RVA: 0x002B0B64 File Offset: 0x002AED64
	public static Vector2I BestFitWorlds(List<WorldPlacement> worldsToArrange, bool ignoreBestFitY = false)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		Vector2I vector2I = default(Vector2I);
		List<WorldPlacement> list2 = new List<WorldPlacement>(worldsToArrange);
		list2.Sort((WorldPlacement a, WorldPlacement b) => b.height.CompareTo(a.height));
		int height = list2[0].height;
		foreach (WorldPlacement worldPlacement in list2)
		{
			Vector2I vector2I2 = default(Vector2I);
			while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height), list))
			{
				if (ignoreBestFitY)
				{
					vector2I2.x++;
				}
				else if (vector2I2.y + worldPlacement.height >= height + 32)
				{
					vector2I2.y = 0;
					vector2I2.x++;
				}
				else
				{
					vector2I2.y++;
				}
			}
			vector2I.x = Math.Max(worldPlacement.width + vector2I2.x, vector2I.x);
			vector2I.y = Math.Max(worldPlacement.height + vector2I2.y, vector2I.y);
			list.Add(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height));
			worldPlacement.SetPosition(vector2I2);
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			vector2I.x += 136;
			vector2I.y = Math.Max(vector2I.y, 136);
		}
		return vector2I;
	}

	// Token: 0x060071BB RID: 29115 RVA: 0x002B0D20 File Offset: 0x002AEF20
	private static bool UnoccupiedSpace(BestFit.Rect RectA, List<BestFit.Rect> placed)
	{
		foreach (BestFit.Rect rect in placed)
		{
			if (RectA.X1 < rect.X2 && RectA.X2 > rect.X1 && RectA.Y1 < rect.Y2 && RectA.Y2 > rect.Y1)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060071BC RID: 29116 RVA: 0x002B0DB0 File Offset: 0x002AEFB0
	public static Vector2I GetGridOffset(IList<WorldContainer> existingWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I result = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I.x, vector2I.y, newWorldSize.x, newWorldSize.y), list))
		{
			if (vector2I.x + newWorldSize.x >= widthInCells)
			{
				vector2I.x = 0;
				vector2I.y++;
			}
			else
			{
				vector2I.x++;
			}
		}
		Debug.Assert(vector2I.x + newWorldSize.x <= Grid.WidthInCells, "BestFit is trying to expand the grid width, this is unsupported and will break the SIM.");
		result.y = Math.Max(newWorldSize.y + vector2I.y, Grid.HeightInCells);
		newWorldOffset = vector2I;
		return result;
	}

	// Token: 0x060071BD RID: 29117 RVA: 0x002B0EF4 File Offset: 0x002AF0F4
	public static int CountRocketInteriors(IList<WorldContainer> existingWorlds)
	{
		int num = 0;
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I rocket_INTERIOR_SIZE = ROCKETRY.ROCKET_INTERIOR_SIZE;
		Vector2I vector2I;
		while (BestFit.PlaceWorld(list, rocket_INTERIOR_SIZE, out vector2I))
		{
			num++;
			list.Add(new BestFit.Rect(vector2I.x, vector2I.y, rocket_INTERIOR_SIZE.x, rocket_INTERIOR_SIZE.y));
		}
		return num;
	}

	// Token: 0x060071BE RID: 29118 RVA: 0x002B0FBC File Offset: 0x002AF1BC
	private static bool PlaceWorld(List<BestFit.Rect> placedWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		Vector2I vector2I = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I2 = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, newWorldSize.x, newWorldSize.y), placedWorlds))
		{
			if (vector2I2.x + newWorldSize.x >= widthInCells)
			{
				vector2I2.x = 0;
				vector2I2.y++;
			}
			else
			{
				vector2I2.x++;
			}
		}
		vector2I.y = Math.Max(newWorldSize.y + vector2I2.y, Grid.HeightInCells);
		newWorldOffset = vector2I2;
		return vector2I2.x + newWorldSize.x <= Grid.WidthInCells && vector2I2.y + newWorldSize.y <= Grid.HeightInCells;
	}

	// Token: 0x02001F04 RID: 7940
	private struct Rect
	{
		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x0600AD3C RID: 44348 RVA: 0x003A9183 File Offset: 0x003A7383
		public int X1
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x0600AD3D RID: 44349 RVA: 0x003A918B File Offset: 0x003A738B
		public int X2
		{
			get
			{
				return this.x + this.width + 2;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x0600AD3E RID: 44350 RVA: 0x003A919C File Offset: 0x003A739C
		public int Y1
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x0600AD3F RID: 44351 RVA: 0x003A91A4 File Offset: 0x003A73A4
		public int Y2
		{
			get
			{
				return this.y + this.height + 2;
			}
		}

		// Token: 0x0600AD40 RID: 44352 RVA: 0x003A91B5 File Offset: 0x003A73B5
		public Rect(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		// Token: 0x04008C60 RID: 35936
		private int x;

		// Token: 0x04008C61 RID: 35937
		private int y;

		// Token: 0x04008C62 RID: 35938
		private int width;

		// Token: 0x04008C63 RID: 35939
		private int height;
	}
}
