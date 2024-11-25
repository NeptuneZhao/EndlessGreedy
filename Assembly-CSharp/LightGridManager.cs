using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A39 RID: 2617
public static class LightGridManager
{
	// Token: 0x06004BDF RID: 19423 RVA: 0x001B0854 File Offset: 0x001AEA54
	public static int ComputeFalloff(float fallOffRate, int cell, int originCell, global::LightShape lightShape, DiscreteShadowCaster.Direction lightDirection)
	{
		int num = originCell;
		if (lightShape == global::LightShape.Quad)
		{
			Vector2I vector2I = Grid.CellToXY(num);
			Vector2I vector2I2 = Grid.CellToXY(cell);
			switch (lightDirection)
			{
			case DiscreteShadowCaster.Direction.North:
			case DiscreteShadowCaster.Direction.South:
			{
				Vector2I vector2I3 = new Vector2I(vector2I2.x, vector2I.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			case DiscreteShadowCaster.Direction.East:
			case DiscreteShadowCaster.Direction.West:
			{
				Vector2I vector2I3 = new Vector2I(vector2I.x, vector2I2.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			}
		}
		return LightGridManager.CalculateFalloff(fallOffRate, cell, num);
	}

	// Token: 0x06004BE0 RID: 19424 RVA: 0x001B08E2 File Offset: 0x001AEAE2
	private static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06004BE1 RID: 19425 RVA: 0x001B08FF File Offset: 0x001AEAFF
	public static void Initialise()
	{
		LightGridManager.previewLux = new int[Grid.CellCount];
	}

	// Token: 0x06004BE2 RID: 19426 RVA: 0x001B0910 File Offset: 0x001AEB10
	public static void Shutdown()
	{
		LightGridManager.previewLux = null;
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06004BE3 RID: 19427 RVA: 0x001B0924 File Offset: 0x001AEB24
	public static void DestroyPreview()
	{
		foreach (global::Tuple<int, int> tuple in LightGridManager.previewLightCells)
		{
			LightGridManager.previewLux[tuple.first] = 0;
		}
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06004BE4 RID: 19428 RVA: 0x001B0988 File Offset: 0x001AEB88
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux)
	{
		LightGridManager.CreatePreview(origin_cell, radius, shape, lux, 0, DiscreteShadowCaster.Direction.South);
	}

	// Token: 0x06004BE5 RID: 19429 RVA: 0x001B0998 File Offset: 0x001AEB98
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux, int width, DiscreteShadowCaster.Direction direction)
	{
		LightGridManager.previewLightCells.Clear();
		ListPool<int, LightGridManager.LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
		pooledList.Add(origin_cell);
		DiscreteShadowCaster.GetVisibleCells(origin_cell, pooledList, (int)radius, width, direction, shape, true);
		foreach (int num in pooledList)
		{
			if (Grid.IsValidCell(num))
			{
				int num2 = lux / LightGridManager.ComputeFalloff(0.5f, num, origin_cell, shape, direction);
				LightGridManager.previewLightCells.Add(new global::Tuple<int, int>(num, num2));
				LightGridManager.previewLux[num] = num2;
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x040031AE RID: 12718
	public const float DEFAULT_FALLOFF_RATE = 0.5f;

	// Token: 0x040031AF RID: 12719
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x040031B0 RID: 12720
	public static int[] previewLux;

	// Token: 0x02001A49 RID: 6729
	public class LightGridEmitter
	{
		// Token: 0x06009F96 RID: 40854 RVA: 0x0037D54C File Offset: 0x0037B74C
		public void UpdateLitCells()
		{
			DiscreteShadowCaster.GetVisibleCells(this.state.origin, this.litCells, (int)this.state.radius, this.state.width, this.state.direction, this.state.shape, true);
		}

		// Token: 0x06009F97 RID: 40855 RVA: 0x0037D5A0 File Offset: 0x0037B7A0
		public void AddToGrid(bool update_lit_cells)
		{
			DebugUtil.DevAssert(!update_lit_cells || this.litCells.Count == 0, "adding an already added emitter", null);
			if (update_lit_cells)
			{
				this.UpdateLitCells();
			}
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					int num2 = Mathf.Max(0, Grid.LightCount[num] + this.ComputeLux(num));
					Grid.LightCount[num] = num2;
					LightGridManager.previewLux[num] = num2;
				}
			}
		}

		// Token: 0x06009F98 RID: 40856 RVA: 0x0037D644 File Offset: 0x0037B844
		public void RemoveFromGrid()
		{
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					Grid.LightCount[num] = Mathf.Max(0, Grid.LightCount[num] - this.ComputeLux(num));
					LightGridManager.previewLux[num] = 0;
				}
			}
			this.litCells.Clear();
		}

		// Token: 0x06009F99 RID: 40857 RVA: 0x0037D6C8 File Offset: 0x0037B8C8
		public bool Refresh(LightGridManager.LightGridEmitter.State state, bool force = false)
		{
			if (!force && EqualityComparer<LightGridManager.LightGridEmitter.State>.Default.Equals(this.state, state))
			{
				return false;
			}
			this.RemoveFromGrid();
			this.state = state;
			this.AddToGrid(true);
			return true;
		}

		// Token: 0x06009F9A RID: 40858 RVA: 0x0037D6F7 File Offset: 0x0037B8F7
		private int ComputeLux(int cell)
		{
			return this.state.intensity / this.ComputeFalloff(cell);
		}

		// Token: 0x06009F9B RID: 40859 RVA: 0x0037D70C File Offset: 0x0037B90C
		private int ComputeFalloff(int cell)
		{
			return LightGridManager.ComputeFalloff(this.state.falloffRate, cell, this.state.origin, this.state.shape, this.state.direction);
		}

		// Token: 0x04007BE8 RID: 31720
		private LightGridManager.LightGridEmitter.State state = LightGridManager.LightGridEmitter.State.DEFAULT;

		// Token: 0x04007BE9 RID: 31721
		private List<int> litCells = new List<int>();

		// Token: 0x020025F9 RID: 9721
		[Serializable]
		public struct State : IEquatable<LightGridManager.LightGridEmitter.State>
		{
			// Token: 0x0600C0E1 RID: 49377 RVA: 0x003DD1C4 File Offset: 0x003DB3C4
			public bool Equals(LightGridManager.LightGridEmitter.State rhs)
			{
				return this.origin == rhs.origin && this.shape == rhs.shape && this.radius == rhs.radius && this.intensity == rhs.intensity && this.falloffRate == rhs.falloffRate && this.colour == rhs.colour && this.width == rhs.width && this.direction == rhs.direction;
			}

			// Token: 0x0400A90C RID: 43276
			public int origin;

			// Token: 0x0400A90D RID: 43277
			public global::LightShape shape;

			// Token: 0x0400A90E RID: 43278
			public int width;

			// Token: 0x0400A90F RID: 43279
			public DiscreteShadowCaster.Direction direction;

			// Token: 0x0400A910 RID: 43280
			public float radius;

			// Token: 0x0400A911 RID: 43281
			public int intensity;

			// Token: 0x0400A912 RID: 43282
			public float falloffRate;

			// Token: 0x0400A913 RID: 43283
			public Color colour;

			// Token: 0x0400A914 RID: 43284
			public static readonly LightGridManager.LightGridEmitter.State DEFAULT = new LightGridManager.LightGridEmitter.State
			{
				origin = Grid.InvalidCell,
				shape = global::LightShape.Circle,
				radius = 4f,
				intensity = 1,
				falloffRate = 0.5f,
				colour = Color.white,
				direction = DiscreteShadowCaster.Direction.South,
				width = 4
			};
		}
	}
}
