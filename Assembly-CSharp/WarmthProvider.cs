using System;
using System.Collections.Generic;

// Token: 0x020005F5 RID: 1525
public class WarmthProvider : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>
{
	// Token: 0x06002504 RID: 9476 RVA: 0x000CF096 File Offset: 0x000CD296
	public static bool IsWarmCell(int cell)
	{
		return WarmthProvider.WarmCells.ContainsKey(cell) && WarmthProvider.WarmCells[cell] > 0;
	}

	// Token: 0x06002505 RID: 9477 RVA: 0x000CF0B5 File Offset: 0x000CD2B5
	public static int GetWarmthValue(int cell)
	{
		if (!WarmthProvider.WarmCells.ContainsKey(cell))
		{
			return -1;
		}
		return (int)WarmthProvider.WarmCells[cell];
	}

	// Token: 0x06002506 RID: 9478 RVA: 0x000CF0D4 File Offset: 0x000CD2D4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.EventTransition(GameHashes.ActiveChanged, this.on, (WarmthProvider.Instance smi) => smi.GetComponent<Operational>().IsActive).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.RemoveWarmCells));
		this.on.EventTransition(GameHashes.ActiveChanged, this.off, (WarmthProvider.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Operational, this.off, true).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.AddWarmCells));
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x000CF18F File Offset: 0x000CD38F
	private static void AddWarmCells(WarmthProvider.Instance smi)
	{
		smi.AddWarmCells();
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x000CF197 File Offset: 0x000CD397
	private static void RemoveWarmCells(WarmthProvider.Instance smi)
	{
		smi.RemoveWarmCells();
	}

	// Token: 0x040014FD RID: 5373
	public static Dictionary<int, byte> WarmCells = new Dictionary<int, byte>();

	// Token: 0x040014FE RID: 5374
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State off;

	// Token: 0x040014FF RID: 5375
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State on;

	// Token: 0x020013DD RID: 5085
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006839 RID: 26681
		public Vector2I OriginOffset;

		// Token: 0x0400683A RID: 26682
		public Vector2I RangeMin;

		// Token: 0x0400683B RID: 26683
		public Vector2I RangeMax;

		// Token: 0x0400683C RID: 26684
		public Func<int, bool> blockingCellCallback = new Func<int, bool>(Grid.IsSolidCell);
	}

	// Token: 0x020013DE RID: 5086
	public new class Instance : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.GameInstance
	{
		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x0600889B RID: 34971 RVA: 0x0032E928 File Offset: 0x0032CB28
		public bool IsWarming
		{
			get
			{
				return base.IsInsideState(base.sm.on);
			}
		}

		// Token: 0x0600889C RID: 34972 RVA: 0x0032E93B File Offset: 0x0032CB3B
		public Instance(IStateMachineTarget master, WarmthProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x0600889D RID: 34973 RVA: 0x0032E948 File Offset: 0x0032CB48
		public override void StartSM()
		{
			EntityCellVisualizer component = base.GetComponent<EntityCellVisualizer>();
			if (component != null)
			{
				component.AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
			}
			this.WorldID = base.gameObject.GetMyWorldId();
			this.SetupRange();
			this.CreateCellListeners();
			base.StartSM();
		}

		// Token: 0x0600889E RID: 34974 RVA: 0x0032E99C File Offset: 0x0032CB9C
		private void SetupRange()
		{
			Vector2I u = Grid.PosToXY(base.transform.GetPosition());
			Vector2I vector2I = base.def.OriginOffset;
			this.range_min = base.def.RangeMin;
			this.range_max = base.def.RangeMax;
			Rotatable rotatable;
			if (base.gameObject.TryGetComponent<Rotatable>(out rotatable))
			{
				vector2I = rotatable.GetRotatedOffset(vector2I);
				Vector2I rotatedOffset = rotatable.GetRotatedOffset(this.range_min);
				Vector2I rotatedOffset2 = rotatable.GetRotatedOffset(this.range_max);
				this.range_min.x = ((rotatedOffset.x < rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_min.y = ((rotatedOffset.y < rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
				this.range_max.x = ((rotatedOffset.x > rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_max.y = ((rotatedOffset.y > rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
			}
			this.origin = u + vector2I;
		}

		// Token: 0x0600889F RID: 34975 RVA: 0x0032EAD0 File Offset: 0x0032CCD0
		public bool ContainsCell(int cell)
		{
			if (this.cellsInRange == null)
			{
				return false;
			}
			for (int i = 0; i < this.cellsInRange.Length; i++)
			{
				if (this.cellsInRange[i] == cell)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060088A0 RID: 34976 RVA: 0x0032EB08 File Offset: 0x0032CD08
		private void UnmarkAllCellsInRange()
		{
			if (this.cellsInRange != null)
			{
				for (int i = 0; i < this.cellsInRange.Length; i++)
				{
					int num = this.cellsInRange[i];
					if (WarmthProvider.WarmCells.ContainsKey(num))
					{
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b - 1;
					}
				}
			}
			this.cellsInRange = null;
		}

		// Token: 0x060088A1 RID: 34977 RVA: 0x0032EB68 File Offset: 0x0032CD68
		private void UpdateCellsInRange()
		{
			this.UnmarkAllCellsInRange();
			Grid.PosToCell(this);
			List<int> list = new List<int>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int num = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(num, this.WorldID) && this.IsCellVisible(num))
					{
						list.Add(num);
						if (!WarmthProvider.WarmCells.ContainsKey(num))
						{
							WarmthProvider.WarmCells.Add(num, 0);
						}
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b + 1;
					}
				}
			}
			this.cellsInRange = list.ToArray();
		}

		// Token: 0x060088A2 RID: 34978 RVA: 0x0032EC7A File Offset: 0x0032CE7A
		public void AddWarmCells()
		{
			this.UpdateCellsInRange();
		}

		// Token: 0x060088A3 RID: 34979 RVA: 0x0032EC82 File Offset: 0x0032CE82
		public void RemoveWarmCells()
		{
			this.UnmarkAllCellsInRange();
		}

		// Token: 0x060088A4 RID: 34980 RVA: 0x0032EC8A File Offset: 0x0032CE8A
		protected override void OnCleanUp()
		{
			this.RemoveWarmCells();
			this.ClearCellListeners();
			base.OnCleanUp();
		}

		// Token: 0x060088A5 RID: 34981 RVA: 0x0032ECA0 File Offset: 0x0032CEA0
		public bool IsCellVisible(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
			Vector2I vector2I2 = Grid.CellToXY(cell);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, base.def.blockingCellCallback, false, false);
		}

		// Token: 0x060088A6 RID: 34982 RVA: 0x0032ECEA File Offset: 0x0032CEEA
		public void OnSolidCellChanged(object obj)
		{
			if (this.IsWarming)
			{
				this.UpdateCellsInRange();
			}
		}

		// Token: 0x060088A7 RID: 34983 RVA: 0x0032ECFC File Offset: 0x0032CEFC
		private void CreateCellListeners()
		{
			Grid.PosToCell(this);
			List<HandleVector<int>.Handle> list = new List<HandleVector<int>.Handle>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int cell = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(cell, this.WorldID))
					{
						list.Add(GameScenePartitioner.Instance.Add("WarmthProvider Visibility", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidCellChanged)));
					}
				}
			}
			this.partitionEntries = list.ToArray();
		}

		// Token: 0x060088A8 RID: 34984 RVA: 0x0032EDEC File Offset: 0x0032CFEC
		private void ClearCellListeners()
		{
			if (this.partitionEntries != null)
			{
				for (int i = 0; i < this.partitionEntries.Length; i++)
				{
					HandleVector<int>.Handle handle = this.partitionEntries[i];
					GameScenePartitioner.Instance.Free(ref handle);
				}
			}
		}

		// Token: 0x0400683D RID: 26685
		public int WorldID;

		// Token: 0x0400683E RID: 26686
		private int[] cellsInRange;

		// Token: 0x0400683F RID: 26687
		private HandleVector<int>.Handle[] partitionEntries;

		// Token: 0x04006840 RID: 26688
		public Vector2I range_min;

		// Token: 0x04006841 RID: 26689
		public Vector2I range_max;

		// Token: 0x04006842 RID: 26690
		public Vector2I origin;
	}
}
