using System;

// Token: 0x02000C9D RID: 3229
public class Lure : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>
{
	// Token: 0x06006356 RID: 25430 RVA: 0x0025006C File Offset: 0x0024E26C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.DoNothing();
		this.on.Enter(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.AddToScenePartitioner)).Exit(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.RemoveFromScenePartitioner));
	}

	// Token: 0x06006357 RID: 25431 RVA: 0x002500C0 File Offset: 0x0024E2C0
	private void AddToScenePartitioner(Lure.Instance smi)
	{
		Extents extents = new Extents(smi.cell, smi.def.radius);
		smi.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, smi, extents, GameScenePartitioner.Instance.lure, null);
	}

	// Token: 0x06006358 RID: 25432 RVA: 0x00250108 File Offset: 0x0024E308
	private void RemoveFromScenePartitioner(Lure.Instance smi)
	{
		GameScenePartitioner.Instance.Free(ref smi.partitionerEntry);
	}

	// Token: 0x04004371 RID: 17265
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State off;

	// Token: 0x04004372 RID: 17266
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State on;

	// Token: 0x02001D85 RID: 7557
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400878D RID: 34701
		public CellOffset[] defaultLurePoints = new CellOffset[1];

		// Token: 0x0400878E RID: 34702
		public int radius = 50;

		// Token: 0x0400878F RID: 34703
		public Tag[] initialLures;
	}

	// Token: 0x02001D86 RID: 7558
	public new class Instance : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.GameInstance
	{
		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600A8E8 RID: 43240 RVA: 0x0039E490 File Offset: 0x0039C690
		public int cell
		{
			get
			{
				if (this._cell == -1)
				{
					this._cell = Grid.PosToCell(base.transform.GetPosition());
				}
				return this._cell;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600A8E9 RID: 43241 RVA: 0x0039E4B7 File Offset: 0x0039C6B7
		// (set) Token: 0x0600A8EA RID: 43242 RVA: 0x0039E4D3 File Offset: 0x0039C6D3
		public CellOffset[] LurePoints
		{
			get
			{
				if (this._lurePoints == null)
				{
					return base.def.defaultLurePoints;
				}
				return this._lurePoints;
			}
			set
			{
				this._lurePoints = value;
			}
		}

		// Token: 0x0600A8EB RID: 43243 RVA: 0x0039E4DC File Offset: 0x0039C6DC
		public Instance(IStateMachineTarget master, Lure.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A8EC RID: 43244 RVA: 0x0039E4ED File Offset: 0x0039C6ED
		public override void StartSM()
		{
			base.StartSM();
			if (base.def.initialLures != null)
			{
				this.SetActiveLures(base.def.initialLures);
			}
		}

		// Token: 0x0600A8ED RID: 43245 RVA: 0x0039E514 File Offset: 0x0039C714
		public void ChangeLureCellPosition(int newCell)
		{
			bool flag = base.IsInsideState(base.sm.on);
			if (flag)
			{
				this.GoTo(base.sm.off);
			}
			this.LurePoints = new CellOffset[]
			{
				Grid.GetOffset(Grid.PosToCell(base.smi.transform.GetPosition()), newCell)
			};
			this._cell = newCell;
			if (flag)
			{
				this.GoTo(base.sm.on);
			}
		}

		// Token: 0x0600A8EE RID: 43246 RVA: 0x0039E590 File Offset: 0x0039C790
		public void SetActiveLures(Tag[] lures)
		{
			this.lures = lures;
			if (lures == null || lures.Length == 0)
			{
				this.GoTo(base.sm.off);
				return;
			}
			this.GoTo(base.sm.on);
		}

		// Token: 0x0600A8EF RID: 43247 RVA: 0x0039E5C3 File Offset: 0x0039C7C3
		public bool IsActive()
		{
			return this.GetCurrentState() == base.sm.on;
		}

		// Token: 0x0600A8F0 RID: 43248 RVA: 0x0039E5D8 File Offset: 0x0039C7D8
		public bool HasAnyLure(Tag[] creature_lures)
		{
			if (this.lures == null || creature_lures == null)
			{
				return false;
			}
			foreach (Tag a in creature_lures)
			{
				foreach (Tag b in this.lures)
				{
					if (a == b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04008790 RID: 34704
		private int _cell = -1;

		// Token: 0x04008791 RID: 34705
		private Tag[] lures;

		// Token: 0x04008792 RID: 34706
		public HandleVector<int>.Handle partitionerEntry;

		// Token: 0x04008793 RID: 34707
		private CellOffset[] _lurePoints;
	}
}
