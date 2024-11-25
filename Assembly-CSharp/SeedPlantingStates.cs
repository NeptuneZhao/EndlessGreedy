using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class SeedPlantingStates : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>
{
	// Token: 0x06000450 RID: 1104 RVA: 0x000229D4 File Offset: 0x00020BD4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findSeed;
		GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.PLANTINGSEED.NAME;
		string tooltip = CREATURES.STATUSITEMS.PLANTINGSEED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.UnreserveSeed)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.DropAll)).Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride));
		this.findSeed.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			SeedPlantingStates.FindSeed(smi);
			if (smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			SeedPlantingStates.ReserveSeed(smi);
			smi.GoTo(this.moveToSeed);
		});
		this.moveToSeed.MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetSeedCell), this.findPlantLocation, this.behaviourcomplete, false);
		this.findPlantLocation.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (!smi.targetSeed)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			SeedPlantingStates.FindDirtPlot(smi);
			if (smi.targetPlot != null || smi.targetDirtPlotCell != Grid.InvalidCell)
			{
				smi.GoTo(this.pickupSeed);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.pickupSeed.PlayAnim("gather").Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PickupComplete)).OnAnimQueueComplete(this.moveToPlantLocation);
		this.moveToPlantLocation.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			if (smi.targetPlot != null)
			{
				smi.GoTo(this.moveToPlot);
				return;
			}
			if (smi.targetDirtPlotCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToDirt);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToDirt.MoveTo((SeedPlantingStates.Instance smi) => smi.targetDirtPlotCell, this.planting, this.behaviourcomplete, false);
		this.moveToPlot.Enter(delegate(SeedPlantingStates.Instance smi)
		{
			if (smi.targetPlot == null || smi.targetSeed == null)
			{
				smi.GoTo(this.behaviourcomplete);
			}
		}).MoveTo(new Func<SeedPlantingStates.Instance, int>(SeedPlantingStates.GetPlantableCell), this.planting, this.behaviourcomplete, false);
		this.planting.Enter(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.RemoveMouthOverride)).PlayAnim("plant").Exit(new StateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State.Callback(SeedPlantingStates.PlantComplete)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToPlantSeed, false);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00022BC0 File Offset: 0x00020DC0
	private static void AddMouthOverride(SeedPlantingStates.Instance smi)
	{
		SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
		KAnim.Build.Symbol symbol = smi.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(smi.def.prefix + "sq_mouth_cheeks");
		if (symbol != null)
		{
			component.AddSymbolOverride("sq_mouth", symbol, 1);
		}
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00022C21 File Offset: 0x00020E21
	private static void RemoveMouthOverride(SeedPlantingStates.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth", 1);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00022C3C File Offset: 0x00020E3C
	private static void PickupComplete(SeedPlantingStates.Instance smi)
	{
		if (!smi.targetSeed)
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} is null", new object[]
			{
				smi.targetSeed
			});
			return;
		}
		SeedPlantingStates.UnreserveSeed(smi);
		int num = Grid.PosToCell(smi.targetSeed);
		if (smi.seed_cell != num)
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} moved {1} != {2}", new object[]
			{
				smi.targetSeed,
				num,
				smi.seed_cell
			});
			smi.targetSeed = null;
			return;
		}
		if (smi.targetSeed.HasTag(GameTags.Stored))
		{
			global::Debug.LogWarningFormat("PickupComplete seed {0} was stored by {1}", new object[]
			{
				smi.targetSeed,
				smi.targetSeed.storage
			});
			smi.targetSeed = null;
			return;
		}
		smi.targetSeed = EntitySplitter.Split(smi.targetSeed, 1f, null);
		smi.GetComponent<Storage>().Store(smi.targetSeed.gameObject, false, false, true, false);
		SeedPlantingStates.AddMouthOverride(smi);
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00022D3C File Offset: 0x00020F3C
	private static void PlantComplete(SeedPlantingStates.Instance smi)
	{
		PlantableSeed plantableSeed = smi.targetSeed ? smi.targetSeed.GetComponent<PlantableSeed>() : null;
		PlantablePlot plantablePlot;
		if (plantableSeed && SeedPlantingStates.CheckValidPlotCell(smi, plantableSeed, smi.targetDirtPlotCell, out plantablePlot))
		{
			if (plantablePlot)
			{
				if (plantablePlot.Occupant == null)
				{
					plantablePlot.ForceDeposit(smi.targetSeed.gameObject);
				}
			}
			else
			{
				plantableSeed.TryPlant(true);
			}
		}
		smi.targetSeed = null;
		smi.seed_cell = Grid.InvalidCell;
		smi.targetPlot = null;
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00022DC8 File Offset: 0x00020FC8
	private static void DropAll(SeedPlantingStates.Instance smi)
	{
		smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00022DF0 File Offset: 0x00020FF0
	private static int GetPlantableCell(SeedPlantingStates.Instance smi)
	{
		int num = Grid.PosToCell(smi.targetPlot);
		if (Grid.IsValidCell(num))
		{
			return Grid.CellAbove(num);
		}
		return num;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00022E1C File Offset: 0x0002101C
	private static void FindDirtPlot(SeedPlantingStates.Instance smi)
	{
		smi.targetDirtPlotCell = Grid.InvalidCell;
		PlantableSeed component = smi.targetSeed.GetComponent<PlantableSeed>();
		PlantableCellQuery plantableCellQuery = PathFinderQueries.plantableCellQuery.Reset(component, 20);
		smi.GetComponent<Navigator>().RunQuery(plantableCellQuery);
		if (plantableCellQuery.result_cells.Count > 0)
		{
			smi.targetDirtPlotCell = plantableCellQuery.result_cells[UnityEngine.Random.Range(0, plantableCellQuery.result_cells.Count)];
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00022E8C File Offset: 0x0002108C
	private static bool CheckValidPlotCell(SeedPlantingStates.Instance smi, PlantableSeed seed, int cell, out PlantablePlot plot)
	{
		plot = null;
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num;
		if (seed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(cell);
		}
		else
		{
			num = Grid.CellBelow(cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (!Grid.Solid[num])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[num, 1];
		if (gameObject)
		{
			plot = gameObject.GetComponent<PlantablePlot>();
			return plot != null;
		}
		return seed.TestSuitableGround(cell);
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00022F05 File Offset: 0x00021105
	private static int GetSeedCell(SeedPlantingStates.Instance smi)
	{
		global::Debug.Assert(smi.targetSeed);
		global::Debug.Assert(smi.seed_cell != Grid.InvalidCell);
		return smi.seed_cell;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00022F34 File Offset: 0x00021134
	private static void FindSeed(SeedPlantingStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Pickupable targetSeed = null;
		int num = 100;
		foreach (object obj in Components.PlantableSeeds)
		{
			PlantableSeed plantableSeed = (PlantableSeed)obj;
			if ((plantableSeed.HasTag(GameTags.Seed) || plantableSeed.HasTag(GameTags.CropSeed)) && !plantableSeed.HasTag(GameTags.Creatures.ReservedByCreature) && Vector2.Distance(smi.transform.position, plantableSeed.transform.position) <= 25f)
			{
				int navigationCost = component.GetNavigationCost(Grid.PosToCell(plantableSeed));
				if (navigationCost != -1 && navigationCost < num)
				{
					targetSeed = plantableSeed.GetComponent<Pickupable>();
					num = navigationCost;
				}
			}
		}
		smi.targetSeed = targetSeed;
		smi.seed_cell = (smi.targetSeed ? Grid.PosToCell(smi.targetSeed) : Grid.InvalidCell);
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00023044 File Offset: 0x00021244
	private static void ReserveSeed(SeedPlantingStates.Instance smi)
	{
		GameObject gameObject = smi.targetSeed ? smi.targetSeed.gameObject : null;
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x00023094 File Offset: 0x00021294
	private static void UnreserveSeed(SeedPlantingStates.Instance smi)
	{
		GameObject go = smi.targetSeed ? smi.targetSeed.gameObject : null;
		if (smi.targetSeed != null)
		{
			go.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

	// Token: 0x040002EF RID: 751
	private const int MAX_NAVIGATE_DISTANCE = 100;

	// Token: 0x040002F0 RID: 752
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findSeed;

	// Token: 0x040002F1 RID: 753
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToSeed;

	// Token: 0x040002F2 RID: 754
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State pickupSeed;

	// Token: 0x040002F3 RID: 755
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State findPlantLocation;

	// Token: 0x040002F4 RID: 756
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlantLocation;

	// Token: 0x040002F5 RID: 757
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToPlot;

	// Token: 0x040002F6 RID: 758
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State moveToDirt;

	// Token: 0x040002F7 RID: 759
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State planting;

	// Token: 0x040002F8 RID: 760
	public GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.State behaviourcomplete;

	// Token: 0x02001097 RID: 4247
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007C69 RID: 31849 RVA: 0x0030573F File Offset: 0x0030393F
		public Def(string prefix)
		{
			this.prefix = prefix;
		}

		// Token: 0x04005D32 RID: 23858
		public string prefix;
	}

	// Token: 0x02001098 RID: 4248
	public new class Instance : GameStateMachine<SeedPlantingStates, SeedPlantingStates.Instance, IStateMachineTarget, SeedPlantingStates.Def>.GameInstance
	{
		// Token: 0x06007C6A RID: 31850 RVA: 0x00305750 File Offset: 0x00303950
		public Instance(Chore<SeedPlantingStates.Instance> chore, SeedPlantingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToPlantSeed);
		}

		// Token: 0x04005D33 RID: 23859
		public PlantablePlot targetPlot;

		// Token: 0x04005D34 RID: 23860
		public int targetDirtPlotCell = Grid.InvalidCell;

		// Token: 0x04005D35 RID: 23861
		public Element plantElement = ElementLoader.FindElementByHash(SimHashes.Dirt);

		// Token: 0x04005D36 RID: 23862
		public Pickupable targetSeed;

		// Token: 0x04005D37 RID: 23863
		public int seed_cell = Grid.InvalidCell;
	}
}
