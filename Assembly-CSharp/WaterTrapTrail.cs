using System;

// Token: 0x02000B5D RID: 2909
public class WaterTrapTrail : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>
{
	// Token: 0x060056EE RID: 22254 RVA: 0x001F11AC File Offset: 0x001EF3AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.retracted;
		base.serializable = StateMachine.SerializeType.Never;
		this.retracted.EventHandler(GameHashes.TrapArmWorkPST, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.loose, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.ShouldBeVisible)).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
		this.loose.EventHandlerTransition(GameHashes.TagsChanged, this.retracted, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.OnTagsChangedWhenOnLooseState)).EventHandler(GameHashes.TrapCaptureCompleted, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
	}

	// Token: 0x060056EF RID: 22255 RVA: 0x001F12A8 File Offset: 0x001EF4A8
	public static bool OnTagsChangedWhenOnLooseState(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		if (smi2 != null)
		{
			smi2.CAPTURING_SYMBOL_NAME = WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + smi.sm.depthAvailable.Get(smi).ToString();
		}
		return WaterTrapTrail.ShouldBeInvisible(smi, tagOBJ);
	}

	// Token: 0x060056F0 RID: 22256 RVA: 0x001F12F4 File Offset: 0x001EF4F4
	public static bool ShouldBeInvisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		return !WaterTrapTrail.ShouldBeVisible(smi, tagOBJ);
	}

	// Token: 0x060056F1 RID: 22257 RVA: 0x001F1300 File Offset: 0x001EF500
	public static bool ShouldBeVisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		bool isOperational = smi.IsOperational;
		bool flag = smi.HasTag(GameTags.TrapArmed);
		bool flag2 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.capture) && !smi2.IsInsideState(smi2.sm.operational.capture.idle) && !smi2.IsInsideState(smi2.sm.operational.capture.release);
		bool flag3 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.unarmed) && smi2.GetWorkable().WorkInPstAnimation;
		return isOperational && (flag || flag2 || flag3);
	}

	// Token: 0x060056F2 RID: 22258 RVA: 0x001F13B8 File Offset: 0x001EF5B8
	public static void RefreshDepthAvailable(WaterTrapTrail.Instance smi, float dt)
	{
		bool flag = WaterTrapTrail.ShouldBeVisible(smi, null);
		int num = Grid.PosToCell(smi);
		int num2 = flag ? WaterTrapGuide.GetDepthAvailable(num, smi.gameObject) : 0;
		int num3 = 4;
		if (num2 != smi.sm.depthAvailable.Get(smi))
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num3; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num2);
				component.SetSymbolVisiblity(WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + i.ToString(), i == num2);
			}
			int cell = Grid.OffsetCell(num, 0, -num2);
			smi.ChangeTrapCellPosition(cell);
			WaterTrapGuide.OccupyArea(smi.gameObject, num2);
			smi.sm.depthAvailable.Set(num2, smi, false);
		}
		smi.SetRangeVisualizerOffset(new Vector2I(0, -num2));
		smi.SetRangeVisualizerVisibility(flag);
	}

	// Token: 0x040038F8 RID: 14584
	private static string CAPTURING_SYMBOL_OVERRIDE_NAME = "creatureSymbol";

	// Token: 0x040038F9 RID: 14585
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State retracted;

	// Token: 0x040038FA RID: 14586
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State loose;

	// Token: 0x040038FB RID: 14587
	private StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter depthAvailable = new StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter(-1);

	// Token: 0x02001BB1 RID: 7089
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001BB2 RID: 7090
	public new class Instance : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.GameInstance
	{
		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x0600A431 RID: 42033 RVA: 0x0038BCF9 File Offset: 0x00389EF9
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x0600A432 RID: 42034 RVA: 0x0038BD06 File Offset: 0x00389F06
		public Lure.Instance lureSMI
		{
			get
			{
				if (this._lureSMI == null)
				{
					this._lureSMI = base.gameObject.GetSMI<Lure.Instance>();
				}
				return this._lureSMI;
			}
		}

		// Token: 0x0600A433 RID: 42035 RVA: 0x0038BD27 File Offset: 0x00389F27
		public Instance(IStateMachineTarget master, WaterTrapTrail.Def def) : base(master, def)
		{
		}

		// Token: 0x0600A434 RID: 42036 RVA: 0x0038BD31 File Offset: 0x00389F31
		public override void StartSM()
		{
			base.StartSM();
			this.RegisterListenersToCellChanges();
		}

		// Token: 0x0600A435 RID: 42037 RVA: 0x0038BD40 File Offset: 0x00389F40
		private void RegisterListenersToCellChanges()
		{
			int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
			CellOffset[] array = new CellOffset[widthInCells * 4];
			for (int i = 0; i < 4; i++)
			{
				int y = -(i + 1);
				for (int j = 0; j < widthInCells; j++)
				{
					array[i * widthInCells + j] = new CellOffset(j, y);
				}
			}
			Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
			this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
			this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
		}

		// Token: 0x0600A436 RID: 42038 RVA: 0x0038BE1C File Offset: 0x0038A01C
		private void UnregisterListenersToCellChanges()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
		}

		// Token: 0x0600A437 RID: 42039 RVA: 0x0038BE3E File Offset: 0x0038A03E
		private void OnLowerCellChanged(object o)
		{
			WaterTrapTrail.RefreshDepthAvailable(base.smi, 0f);
		}

		// Token: 0x0600A438 RID: 42040 RVA: 0x0038BE50 File Offset: 0x0038A050
		protected override void OnCleanUp()
		{
			this.UnregisterListenersToCellChanges();
			base.OnCleanUp();
		}

		// Token: 0x0600A439 RID: 42041 RVA: 0x0038BE5E File Offset: 0x0038A05E
		public void SetRangeVisualizerVisibility(bool visible)
		{
			this.rangeVisualizer.RangeMax.x = (visible ? 0 : -1);
		}

		// Token: 0x0600A43A RID: 42042 RVA: 0x0038BE77 File Offset: 0x0038A077
		public void SetRangeVisualizerOffset(Vector2I offset)
		{
			this.rangeVisualizer.OriginOffset = offset;
		}

		// Token: 0x0600A43B RID: 42043 RVA: 0x0038BE85 File Offset: 0x0038A085
		public void ChangeTrapCellPosition(int cell)
		{
			if (this.lureSMI != null)
			{
				this.lureSMI.ChangeLureCellPosition(cell);
			}
			base.gameObject.GetComponent<TrapTrigger>().SetTriggerCell(cell);
		}

		// Token: 0x0400806F RID: 32879
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04008070 RID: 32880
		[MyCmpGet]
		private RangeVisualizer rangeVisualizer;

		// Token: 0x04008071 RID: 32881
		private HandleVector<int>.Handle partitionerEntry_buildings;

		// Token: 0x04008072 RID: 32882
		private HandleVector<int>.Handle partitionerEntry_solids;

		// Token: 0x04008073 RID: 32883
		private Lure.Instance _lureSMI;
	}
}
